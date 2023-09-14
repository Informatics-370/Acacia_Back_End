using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Models.CustomerReturns;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Acacia_Back_End.Controllers
{
    public class InventoryController : BaseApiController
    {
        private readonly IGenericRepository<WriteOff> _writeoffRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<CustomerReturn> _returnsRepo;
        private readonly UserManager<AppUser> _userManager;

        public InventoryController(IGenericRepository<WriteOff> writeoffRepo, IMapper mapper, IGenericRepository<CustomerReturn> returnsRepo, UserManager<AppUser> userManager)
        {
            _writeoffRepo = writeoffRepo;
            _mapper = mapper;
            _returnsRepo = returnsRepo;
            _userManager = userManager;
        }

        [HttpGet("WriteOffs")]
        public async Task<ActionResult<Pagination<WriteOffVM>>> GetWriteOffLog([FromQuery] WriteOffParams searchParams)
        {
            var writeOffs = await _writeoffRepo.GetWriteOffsAsync(searchParams);

            var data = _mapper.Map<IReadOnlyList<WriteOffVM>>(writeOffs.Skip((searchParams.PageIndex - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList());

            return Ok(new Pagination<WriteOffVM>(searchParams.PageIndex, searchParams.PageSize, writeOffs.Count, data));
        }

        [HttpGet("WriteOffs/{id}")]
        public async Task<ActionResult<WriteOffVM>> ViewWriteOff(int id)
        {
            var writeOff = await _writeoffRepo.GetWriteOffByIdAsync(id);
            if(writeOff == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<WriteOffVM>(writeOff));
        }

        [HttpPost]
        public async Task<ActionResult> CreateWriteoff(CreateWriteOffVM writeOffVM)
        {
            if (writeOffVM.Quantity == 0) return BadRequest(new ApiResponse(400, "Please enter a valid Quantity amount"));

            if (writeOffVM.ProductId == 0) return BadRequest(new ApiResponse(400, "Please enter a valid product"));

            WriteOffReason status;
            switch (writeOffVM.Reason)
            {
                case "Damaged":
                    status = WriteOffReason.Damaged;
                break;
                case "Theft":
                    status = WriteOffReason.Theft;
                    break;
                case "Obsolete":
                    status = WriteOffReason.Obsolete;
                    break;
                case "Losted":
                    status = WriteOffReason.Losted;
                    break;
                case "Other":
                    status = WriteOffReason.Other;
                    break;
                default:
                    status = WriteOffReason.Other;
                    break;
            }

            var result = await _writeoffRepo.CreateWriteOff(new WriteOff
            {
                Date = DateTime.Now,
                ProductId = writeOffVM.ProductId,
                Quantity = writeOffVM.Quantity,
                Reason = status
            });
            if (result == true) return Ok();
            return BadRequest(new ApiResponse(400));
        }

        [HttpPost("LogReturn")]
        public async Task<ActionResult> LogReturn(LogCustomerReturnVM customerReturn)
        {
            var returnedItems = _mapper.Map<List<ReturnItem>>(customerReturn.ReturnItems);

            var result1 = await _returnsRepo.VerifyReturnRequest(customerReturn.OrderId, customerReturn.CustomerEmail, returnedItems);
            if (result1 == false) return BadRequest(new ApiResponse(400, "Your return request was not approved by the system"));

            var result = await _returnsRepo.LogCustomerReturnAsync(customerReturn.OrderId, customerReturn.CustomerEmail, customerReturn.Description, returnedItems);
            if (result == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<CustomerReturnVM>(result));
        }

        [HttpGet("ReturnsLog/{id}")]
        public async Task<ActionResult<CustomerReturnVM>> GetCustomerReturn(int id)
        {
            var customerReturn = await _returnsRepo.GetCustomerReturnByIdAsync(id);
            if (customerReturn == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<CustomerReturnVM>(customerReturn));
        }


        [HttpGet("ReturnsLog")]
        public async Task<ActionResult<Pagination<CustomerReturnVM>>> GetReturnsLog([FromQuery] SpecParams searchParams)
        {
            var returnLog = await _returnsRepo.GetReturnsLogAsync(searchParams);

            var data = _mapper.Map<IReadOnlyList<CustomerReturnVM>>(returnLog.Skip((searchParams.PageIndex - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToList());

            return Ok(new Pagination<CustomerReturnVM>(searchParams.PageIndex, searchParams.PageSize, returnLog.Count, data));
        }
    }
}
