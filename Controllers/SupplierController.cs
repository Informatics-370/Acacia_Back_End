using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Security.Cryptography.X509Certificates;

namespace Acacia_Back_End.Controllers
{
    public class SupplierController : BaseApiController
    {
        private readonly IGenericRepository<Supplier> _supplierRepo;
        private readonly IMapper _mapper;

        public SupplierController(IGenericRepository<Supplier> supplierRepo, IMapper mapper)
        {
            _supplierRepo = supplierRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<SupplierVM>>> GetSuppliers([FromQuery] SpecParams specParams)
        {
            var suppliers = await _supplierRepo.GetSuppliersAsync(specParams);

            var data = _mapper.Map<IReadOnlyList<SupplierVM>>(suppliers.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList());

            return Ok(new Pagination<SupplierVM>(specParams.PageIndex, specParams.PageSize, suppliers.Count, data));
        }

        [HttpGet("List")]
        public async Task<ActionResult<SupplierVM>> GetSuppliers()
        {
            var suppliers = await _supplierRepo.ListAllAsync();

            return Ok(_mapper.Map<IReadOnlyList<SupplierVM>>(suppliers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierVM>> GetSupplier(int id)
        {
            var result = await _supplierRepo.GetByIdAsync(id);

            if (result == null) return NotFound(new ApiResponse(404));
            return Ok(result);  
        }

        [HttpPost]
        public async Task<ActionResult<SupplierVM>> AddSupplier(SupplierVM supplierVM)
        {
            Supplier supplier = _mapper.Map<Supplier>(supplierVM);
            var result = await _supplierRepo.AddEntity(supplier); 

            if(result == true) return Ok(supplierVM);
            return BadRequest(new ApiResponse(400));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SupplierVM>> UpdateSupplier(SupplierVM suppliervm, int id)
        {
            var supplier = await _supplierRepo.GetByIdAsync(id);
            if (supplier == null) return NotFound(new ApiResponse(404));

            supplier.Email = suppliervm.Email;
            supplier.Name = suppliervm.Name;
            supplier.PhoneNumber = suppliervm.PhoneNumber;

            var result = await _supplierRepo.UpdateEntity(supplier);

            if(result == true) return Ok(suppliervm);
            return BadRequest(new ApiResponse(400));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            var result = await _supplierRepo.RemoveEntity(id);

            if (result == false) return BadRequest(new ApiResponse(400));

            return Ok(result);
        }
    }
}
