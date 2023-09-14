using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Extensions;
using Acacia_Back_End.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class ProductReviewController : BaseApiController
    {
        private readonly IGenericRepository<ProductReview> _reviewRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ProductReviewController(IGenericRepository<ProductReview> reviewRepo, IMapper mapper, UserManager<AppUser> userManager)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet("Reviews")]
        public async Task<ActionResult<Pagination<ProductReviewVM>>> GetProductReviews([FromQuery] ReviewParams specParams)
        {
            var reviews = await _reviewRepo.GetProductReviewsAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<ProductReviewVM>>(reviews.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList());
            return Ok(new Pagination<ProductReviewVM>(specParams.PageIndex, specParams.PageSize, reviews.Count, data));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductReviewVM>> GetProductReview(int id)
        {
            var review = await _reviewRepo.GetProductReviewByIdAsync(id);
            if (review == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<ProductReview, ProductReviewVM>(review));
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<ActionResult> CreateProductReview(CreateProductReviewVM reviewVM)
        {
            var review = _mapper.Map<CreateProductReviewVM, ProductReview>(reviewVM);
            review.Date = DateTime.Now;
            var result = await _reviewRepo.AddEntity(review);
            if (result == true) return Ok();
            return BadRequest(new ApiResponse(400, "There was a problem adding the new product review"));
        }

        [HttpPost("Flag/{id}")]
        [Authorize]
        public async Task<ActionResult> FlagProductReview(int id)
        {
            var result = await _reviewRepo.FlagProductReviewAsync(id);
            if (result == true) return Ok();
            return BadRequest(new ApiResponse(400, "There was a problem flagging product review"));
        }

        [HttpPost("Unflag/{id}")]
        [Authorize]
        public async Task<ActionResult> UnflagProductReview(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var result = await _reviewRepo.UnFlagProductReviewAsync(id, email);
            if (result == true) return Ok();
            return BadRequest(new ApiResponse(400, "There was a problem unflagging product review"));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductReviewVM>> UpdateProductReview(ProductReviewVM reviewVM, int id)
        {
            var review = await _reviewRepo.GetProductReviewByIdAsync(id);

            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            if (email != review.CustomerEmail) return BadRequest(new ApiResponse(400));

            review.Rating = reviewVM.Rating;
            review.Title = reviewVM.Title;
            review.Description = reviewVM.Description;

            var result = await _reviewRepo.UpdateEntity(review);
            if (result == true) return Ok(reviewVM);
            return BadRequest(new ApiResponse(400, "There was a problem updating the product review"));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProductCategory(int id)
        {
            var review = await _reviewRepo.GetProductReviewByIdAsync(id);

            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            if (email != review.CustomerEmail) return BadRequest(new ApiResponse(400));

            var result = await _reviewRepo.RemoveEntity(id);
            if (result == true) return Ok();
            return BadRequest(new ApiResponse(400, "There was a problem deleting the product review"));
        }

    }
}
