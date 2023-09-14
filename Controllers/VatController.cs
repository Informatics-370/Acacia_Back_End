using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Acacia_Back_End.Controllers
{
    public class VatController : BaseApiController
    {
        private readonly IGenericRepository<Vat> _vatRepo;
        private readonly IMapper _mapper;

        public VatController(IGenericRepository<Vat> vatRepo, IMapper mapper)
        {
            _vatRepo = vatRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VatVM>> GetVat(int id)
        {
            var vat = await _vatRepo.GetByIdAsync(id);

            if (vat == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<VatVM>(vat));
        }

        [HttpGet]
        [Route("CurrentVat")]
        public async Task<ActionResult<VatVM>> GetActiveVat()
        {
            var vat = await _vatRepo.GetActiveVat();

            if (vat == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<VatVM>(vat));
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<VatVM>>> GetVatList([FromQuery] VatSpecParams specParams)
        {
            var vats = await _vatRepo.GetVatsAsync(specParams);

            var data = _mapper.Map<IReadOnlyList<VatVM>>(vats.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList());

            return Ok(new Pagination<VatVM>(specParams.PageIndex, specParams.PageSize, vats.Count, data));
        }

        [HttpPost]
        public async Task<ActionResult> CreateVat(decimal vatPercentage)
        {
            // Get Active Vat
            var activeVat = await _vatRepo.GetActiveVat();
            if (activeVat != null)
            {
                // Update Active Vat Value
                activeVat.EndDate = DateTime.Now;
                activeVat.IsActive = false;
                var res = await _vatRepo.UpdateEntity(activeVat);
                if (res == false) return BadRequest(new ApiResponse(400));
            }
            // Create New Vat value
            Vat vat = new Vat()
            {
                Percentage = vatPercentage,
                StartDate = DateTime.Now,
                IsActive = true,
            };
            var result = await _vatRepo.AddEntity(vat);
            if (result == true) return Ok();
            return BadRequest("There was a problem adding the new vat");
        }
    }
}
