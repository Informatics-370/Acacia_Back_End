using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Extensions;
using Acacia_Back_End.Helpers;
using Acacia_Back_End.Infrastructure.Services;
using AutoMapper;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using System.IO;
using Acacia_Back_End.Core.Models.CustomerReturns;

namespace Acacia_Back_End.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IGenericRepository<Order> _orderService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailservice;

        public OrdersController(IGenericRepository<Order> orderService, IMapper mapper, IEmailService emailservice)
        {
            _orderService = orderService;
            _mapper = mapper;
            _emailservice = emailservice;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderVM orderVM)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var address = _mapper.Map<OrderAddressVM, OrderAddress>(orderVM.ShipToAddress);

            var result = await _orderService.VerifyStockForSaleOrder(orderVM.BasketId);
            if (result == false) return BadRequest(new ApiResponse(400, "Sorry there is not enough stock to fulfill your order request."));

            var order = await _orderService.CreateOrderAsync(email, orderVM.DeliveryMethodId, orderVM.OrderTypeId, orderVM.BasketId, address);

            if (order == null) return BadRequest(new ApiResponse(400, "Issues"));

            return Ok(order);
        }

        [HttpGet("VerifyCartStock/{cartId}")]
        public async Task<ActionResult<bool>> VerifyCartStock(string cartId)
        {
            var result = await _orderService.VerifyStockForSaleOrder(cartId);
            if (result == false) return BadRequest(new ApiResponse(400, "Sorry there is not enough stock to fulfill your order request."));

            return Ok();
        }

        [HttpGet("User")]
        public async Task<ActionResult<Pagination<OrderVM>>> GetUserOrders([FromQuery] OrderParams searchParams)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetUserOrdersAsync(email, searchParams);

            var data = _mapper.Map<IReadOnlyList<OrderVM>>(orders.Skip((searchParams.PageIndex - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList());

            return Ok(new Pagination<OrderVM>(searchParams.PageIndex, searchParams.PageSize, orders.Count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderVM>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<OrderVM>(order));
        }


        [HttpGet]
        public async Task<ActionResult<Pagination<OrderVM>>> GetOrders([FromQuery] OrderParams searchParams)
        {
            var orders = await _orderService.GetOrdersAsync(searchParams);

            var data = _mapper.Map<IReadOnlyList<OrderVM>>(orders.Skip((searchParams.PageIndex - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList());

            return Ok(new Pagination<OrderVM>(searchParams.PageIndex, searchParams.PageSize, orders.Count, data));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var methods = await _orderService.GetDeliveryMethodsAsync();

            if (methods == null) return NotFound(new ApiResponse(404));

            return Ok(methods);
        }

        [HttpGet("OrderTypes")]
        public async Task<ActionResult<IReadOnlyList<OrderType>>> GetOrderTypes()
        {
            var orderTypes = await _orderService.GetOrderTypesAsync();

            if (orderTypes == null) return NotFound(new ApiResponse(404));

            return Ok(orderTypes);
        }

        [HttpPost("DispatchOrder")]
        public async Task<ActionResult<OrderVM>> DispatchOrder(DispatchOrderVM dispatchVM)
        {
            var result = await _orderService.DispatchOrderAsync(dispatchVM.OrderId, dispatchVM.TrackingNumber);
            if (result == false) return NotFound(new ApiResponse(404));

            _emailservice.SendEmail(new EmailVM
            {
                To = dispatchVM.CustomerEmail,
                Subject = "Order " + dispatchVM.OrderId.ToString() + ": Dispatched",
                Body = "Your order has been dispatched by the store. Please may you track the order using this tracking number: " + dispatchVM.TrackingNumber,
            });
            return Ok();
        }

        [HttpPost("PackageOrder")]
        public async Task<ActionResult<OrderVM>> PackageOrder(PackageOrderVM packageVM)
        {
            var result = await _orderService.PackageOrder(packageVM.OrderId);
            if (result == false) return NotFound(new ApiResponse(404));

            _emailservice.SendEmail(new EmailVM
            {
                To = packageVM.CustomerEmail,
                Subject = "Order " + packageVM.OrderId.ToString() + ": Ready for Collection",
                Body = "Your order has been packaged by the store and is ready for collection. Please may you come by to collect it."
            });
            return Ok();
        }

        [HttpGet("GenerateInvoice/{id}")]
        public async Task<ActionResult> GenerateInvoice(int id)
        {
            var order =  await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound(new ApiResponse(404));

            using (MemoryStream ms = new MemoryStream())
            {
                using (Document document = new Document(PageSize.A4))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    // Add content to the PDF
                    document.Add(new Paragraph("Invoice"));
                    document.Add(new Paragraph($"Invoice ID: {id}"));
                    document.Add(new Paragraph($"CustomerEmail: {order.CustomerEmail}"));
                    document.Add(new Paragraph($"DeliveryMethod: {order.DeliveryMethod.Name}"));
                    document.Add(new Paragraph($"OrderDate: {order.OrderDate}"));
                    document.Add(new Paragraph($"OrderType: {order.OrderType.Name}"));
                    document.Add(new Paragraph($"FirstName: {order.ShipToAddress.FirstName}"));
                    document.Add(new Paragraph($"LastName: {order.ShipToAddress.LastName}"));
                    document.Add(new Paragraph($"TrackingNumber: {order.ShipToAddress.TrackingNumber}"));
                    document.Add(new Paragraph($"ComplexName: {order.ShipToAddress.ComplexName}"));
                    document.Add(new Paragraph($"StreetAddress: {order.ShipToAddress.StreetAddress}"));
                    document.Add(new Paragraph($"City: {order.ShipToAddress.City}"));
                    document.Add(new Paragraph($"Province: {order.ShipToAddress.Province}"));
                    document.Add(new Paragraph($"PostalCode: {order.ShipToAddress.PostalCode}"));
                    document.Add(new Paragraph($"SubTotal (Vat Inclusive): {order.SubTotal}"));
                    document.Add(new Paragraph($"VAT: {order.VAT.Percentage}%"));
                    document.Add(new Paragraph($"Delivery price: {order.DeliveryMethod.Price}"));
                    document.Add(new Paragraph($"GroupElephantDiscount: {order.GroupElephantDiscount}"));
                    document.Add(new Paragraph($"Promotions: {order.Savings}"));
                    document.Add(new Paragraph($"Total: {order.GetTotal()}"));

                    foreach (var item in order.OrderItems)
                    {
                        document.Add(new Paragraph($"-------------------------------------------------"));
                        document.Add(new Paragraph($"Product Name: {item.ItemOrdered.ProductName}"));
                        document.Add(new Paragraph($"Product Price: {item.Price}"));
                        document.Add(new Paragraph($"Product Quantity: {item.Quantity}"));
                        document.Add(new Paragraph($"Product Promotion: {item.Promotion} % "));
                        document.Add(new Paragraph($"-------------------------------------------------"));
                    }
                    // Add more content as needed
                    document.Close();
                }
                byte[] pdfBytes = ms.ToArray();
                string fileName = $"Invoice_{id}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
        }
    }
}
