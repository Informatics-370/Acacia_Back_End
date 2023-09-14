using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Models.SupplierOrders;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Extensions;
using Acacia_Back_End.Helpers;
using Acacia_Back_End.Infrastructure.Services;
using AutoMapper;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Utilities.Collections;
using System.Diagnostics.Contracts;
using Acacia_Back_End.Core.Models.CustomerReturns;
using Acacia_Back_End.Core.Models.SupplierReturns;

namespace Acacia_Back_End.Controllers
{
    public class SupplierOrdersController : BaseApiController
    {
        private readonly IGenericRepository<SupplierOrder> _orderService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailservice;
        private readonly IGenericRepository<SupplierReturn> _returnsRepo;
        private readonly IConfiguration _config;

        public SupplierOrdersController(IGenericRepository<SupplierOrder> orderService, IMapper mapper, IEmailService emailservice, IGenericRepository<SupplierReturn> returnsRepo, IConfiguration config)
        {
            _orderService = orderService;
            _mapper = mapper;
            _emailservice = emailservice;
            _returnsRepo = returnsRepo;
            _config = config;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierOrderVM>> GetOrderById(int id)
        {
            var order = await _orderService.GetSupplierOrderByIdAsync(id);

            if (order == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<SupplierOrderVM>(order));
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<SupplierOrderVM>>> GetOrders([FromQuery] OrderParams searchParams)
        {
            var orders = await _orderService.GetSupplierOrdersAsync(searchParams);
            var data = _mapper.Map<IReadOnlyList<SupplierOrderVM>>(orders.Skip((searchParams.PageIndex - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList());
            return Ok(new Pagination<SupplierOrderVM>(searchParams.PageIndex, searchParams.PageSize, orders.Count, data));
        }

        [HttpPost("PlaceOrder")]
        public async Task<ActionResult<SupplierOrderVM>> CreateOrder(ConfigureSupplierOrderVM orderVM)
        {
            var managerEmail = HttpContext.User.RetrieveEmailFromPrincipal();
            if (orderVM.OrderItems.Count <= 0) return BadRequest(new ApiResponse(400, "Please provide valid products for the order."));
            var order = await _orderService.CreateSupplierOrderAsync(managerEmail, orderVM.SupplierId, orderVM.OrderItems);
            if (order == null) return BadRequest(new ApiResponse(400, "Issues"));
            return Ok(order);
        }

        [HttpPut("ApproveOrder/{id}")]
        public async Task<ActionResult<SupplierOrderVM>> ApproveOrder(ConfigureSupplierOrderVM orderVM, int id)
        {
            var result = await _orderService.ApproveSupplierOrder(id, orderVM.OrderItems);
            if (result == false) return BadRequest(new ApiResponse(400));
            return Ok();
        }


        [HttpPut("ConfirmPayment/{id}"), DisableRequestSizeLimit]
        public async Task<ActionResult<SupplierOrderVM>> ConfirmPayment([FromForm] OrderDocsVM orderVM, int id)
        {
            string invoiceFileName = await SaveUploadedFile(orderVM.InvoiceUrl, "invoices/");
            string proofOfPaymentFileName = await SaveUploadedFile(orderVM.ProofOfPaymentUrl, "pops/");

            var result = await _orderService.ConfirmSupplierOrderPayment(id, invoiceFileName, proofOfPaymentFileName);
            if (result == false) return BadRequest(new ApiResponse(400));
            return Ok();
        }

        [HttpPut("ConfirmOrderDelivery/{id}")]
        public async Task<ActionResult<SupplierOrderVM>> ConfirmOrderDelivery(ConfigureSupplierOrderVM orderVM, int id)
        {
            var result = await _orderService.ConfirmSupplierOrderDelivery(id, orderVM.OrderItems);
            if (result == false) return BadRequest(new ApiResponse(400));
            return Ok();
        }

        [HttpPut("CancelOrder/{id}")]
        public async Task<ActionResult<SupplierOrderVM>> CancelOrder(int id)
        {
            List<SupplierOrderItemVM> items = new List<SupplierOrderItemVM>();
            var result = await _orderService.CancelSupplierOrder(id);
            if (result == false) return BadRequest(new ApiResponse(400));
            return Ok();
        }

        [HttpPost("LogReturn")]
        public async Task<ActionResult> LogReturn(LogSupplierReturnVM supplierReturn)
        {
            var returnedItems = _mapper.Map<List<SupplierReturnItem>>(supplierReturn.ReturnItems);

            var result1 = await _returnsRepo.VerifySupplierReturnRequest(supplierReturn.SupplierOrderId, returnedItems);
            if (result1 == false) return BadRequest(new ApiResponse(400, "Your return request was not approved by the system"));

            var result = await _returnsRepo.LogSupplierReturnAsync(supplierReturn.SupplierOrderId, supplierReturn.ManagerEmail, supplierReturn.Description, returnedItems);
            if (result == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<SupplierReturnVM>(result));
        }

        [HttpGet("ReturnsLog/{id}")]
        public async Task<ActionResult<SupplierReturnVM>> GetSupplierReturn(int id)
        {
            var supplierReturn = await _returnsRepo.GetSupplierReturnByIdAsync(id);
            if (supplierReturn == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<SupplierReturnVM>(supplierReturn));
        }


        [HttpGet("ReturnsLog")]
        public async Task<ActionResult<Pagination<SupplierReturnVM>>> GetReturnsLog([FromQuery] SpecParams searchParams)
        {
            var returnLog = await _returnsRepo.GetSupplierReturnsLogAsync(searchParams);

            var data = _mapper.Map<IReadOnlyList<SupplierReturnVM>>(returnLog.Skip((searchParams.PageIndex - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList());

            return Ok(new Pagination<SupplierReturnVM>(searchParams.PageIndex, searchParams.PageSize, returnLog.Count, data));
        }

        [HttpGet("PrintInvoice/{id}")]
        public async Task<ActionResult> PrintInvoice(int id)
        {
            var order = await _orderService.GetSupplierOrderByIdAsync(id);
            if (order == null) return NotFound(new ApiResponse(404));

            string invoiceFilePath = Path.Combine("wwwroot/", order.InvoiceUrl);
            if (!System.IO.File.Exists(invoiceFilePath))
            {
                return NotFound(new ApiResponse(404, "Invoice file not found."));
            }

            byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(invoiceFilePath);
            string fileName = order.InvoiceUrl;
            return File(pdfBytes, "application/pdf", fileName);
        }

        [HttpGet("PrintPOP/{id}")]
        public async Task<ActionResult> PrintProofOfPayment(int id)
        {
            var order = await _orderService.GetSupplierOrderByIdAsync(id);
            if (order == null) return NotFound(new ApiResponse(404));

            string invoiceFilePath = Path.Combine("wwwroot/", order.ProofOfPaymentUrl);
            if (!System.IO.File.Exists(invoiceFilePath))
            {
                return NotFound(new ApiResponse(404, "Invoice file not found."));
            }

            byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(invoiceFilePath);
            string fileName = order.ProofOfPaymentUrl;
            return File(pdfBytes, "application/pdf", fileName);
        }

        private async Task<string> SaveUploadedFile(IFormFile file, string filetype)
        {
            if (file != null && file.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                string filePath = Path.Combine("wwwroot/" + filetype, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return filetype + uniqueFileName;
            }
            return null;
        }
    }
}
