using Acacia_Back_End.Core;
using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class CompanyController : BaseApiController
    {
        private readonly IGenericRepository<Company> _companyRepo;
        private readonly IMapper _mapper;

        public CompanyController(IGenericRepository<Company> companyRepo, IMapper mapper)
        {
            _companyRepo = companyRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyVM>> GetCompany(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);

            if (company == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<CompanyVM>(company));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyVM>> UpdateCompany(CompanyVM company, int id)
        {
            var mycompany = await _companyRepo.GetByIdAsync(id);

            if (mycompany == null) return NotFound(new ApiResponse(404));

            mycompany.VatNumber = company.VatNumber;
            mycompany.AddressLine1 = company.AddressLine1;
            mycompany.AddressLine2 = company.AddressLine2;
            mycompany.Suburb = company.Suburb;
            mycompany.City = company.City;
            mycompany.Province = company.Province;
            mycompany.PostalCode = company.PostalCode;

            var result = await _companyRepo.UpdateEntity(mycompany);

            if (result == true) return Ok(_mapper.Map<CompanyVM>(company));
            return NotFound(new ApiResponse(400));
        }
    }
}
