using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class FAQController : BaseApiController
    {
        private readonly IGenericRepository<FAQ> _faqRepo;

        public FAQController(IGenericRepository<FAQ> faqRepo)
        {
            _faqRepo = faqRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FAQ>> GetFaq(int id)
        {
            var faq = await _faqRepo.GetByIdAsync(id);

            if (faq == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(faq);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<FAQ>>> GetFaqs([FromQuery]SpecParams specParams)
        {
            var faqs = await _faqRepo.GetFaqsAsync(specParams);

            if (faqs == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(new Pagination<FAQ>(specParams.PageIndex, specParams.PageSize, faqs.Count, faqs.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList()));
        }

        [HttpPost("add")]
        //[Authorize]
        public async Task<ActionResult<FAQ>> AddFaq(FAQ faq)
        {
            if (faq.Answer == null || faq.Question == null)
            {
                return BadRequest(new ApiResponse(400));
            }

            var result = await _faqRepo.AddEntity(faq);

            if (result == true) return Ok(result);

            return BadRequest(new ApiResponse(400));
        }

        [HttpPut("update/{id}")]
        //[Authorize]
        public async Task<ActionResult<FAQ>> UpdateFaq(int id, FAQ newfaq)
        {
            var faq = await _faqRepo.GetByIdAsync(id);

            if (newfaq == null) return NotFound(new ApiResponse(404));

            faq.Question = newfaq.Question;
            faq.Answer = newfaq.Answer;

            var result = await _faqRepo.UpdateEntity(faq);

            if (result == true) return Ok(faq);

            return BadRequest(new ApiResponse(400));
        }

        [HttpDelete("delete/{id}")]
        //[Authorize]
        public async Task<ActionResult<FAQ>> DeleteFaq(int id)
        {
            var faq = await _faqRepo.GetByIdAsync(id);

            if (faq == null) return NotFound(new ApiResponse(404));

            var result = await _faqRepo.RemoveEntity(id);

            if (result == true) return Ok(faq);

            return BadRequest(new ApiResponse(400));
        }
    }
}
