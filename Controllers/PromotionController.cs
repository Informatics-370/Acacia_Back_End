using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class PromotionController : BaseApiController
    {
        private readonly IGenericRepository<Promotion> _promotionsRepo;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;

        public PromotionController(IGenericRepository<Promotion> promotionsRepo, IGenericRepository<Product> productsRepo, IMapper mapper)
        {
            _promotionsRepo = promotionsRepo;
            _productsRepo = productsRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionVM>> GetPromotion(int id)
        {
            var promotion = await _promotionsRepo.GetPromotionByIdAsync(id);

            if(promotion == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<PromotionVM>(promotion));
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ListPromotionVM>>> GetPromotions([FromQuery] PromotionSpecParams specParams)
        {
            var promotions = await _promotionsRepo.GetPromotionsAsync(specParams);

            // Try implement this in the repsoitory
            var data = _mapper.Map<IReadOnlyList<ListPromotionVM>>(promotions.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList());

            return Ok(new Pagination<ListPromotionVM>(specParams.PageIndex, specParams.PageSize, promotions.Count, data));
        }

        [HttpPost]
        public async Task<ActionResult<PromotionVM>> AddPromotion(AddPromotionVM promotionVM)
        {
            var newpromotion = new Promotion
            {
                Name = promotionVM.Name,
                Description = promotionVM.Description,
                Percentage = promotionVM.Percentage,
                Products = new List<Product>(),
                IsActive = promotionVM.IsActive
            };
            foreach (var prod in promotionVM.Products)
            {
                Product productToAdd = await _productsRepo.GetProductByIdAsync(prod.Id);
                newpromotion.Products.Add(productToAdd);
            }

            if (newpromotion.Products.Count < 1) return BadRequest(new ApiResponse(400));

            var result = await _promotionsRepo.AddEntity(newpromotion);

            if (result == true) return Ok(_mapper.Map<PromotionVM>(newpromotion));

            return BadRequest(new ApiResponse(400));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PromotionVM>> AddPromotion(UpdatePromotionVM promotionVM, int id)
        {
            var promotion = await _promotionsRepo.GetPromotionByIdAsync(id);

            promotion.Name = promotionVM.Name;
            promotion.Description = promotionVM.Description;
            promotion.Percentage = promotionVM.Percentage;
            promotion.Products = new List<Product>();
            promotion.IsActive = promotionVM.IsActive;

            foreach (var prod in promotionVM.Products)
            {
                Product productToAdd = await _productsRepo.GetProductByIdAsync(prod.Id);
                promotion.Products.Add(productToAdd);
            }

            if (promotion.Products.Count < 1) return BadRequest(new ApiResponse(400));

            var result = await _promotionsRepo.UpdateEntity(promotion);

            if (result == true) return Ok(promotionVM);

            return BadRequest(new ApiResponse(400));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePromotion(int id)
        {
            var result = await _promotionsRepo.RemovePromotion(id);

            if (result == false) return BadRequest(new ApiResponse(400));

            return Ok();
        }
    }
}
