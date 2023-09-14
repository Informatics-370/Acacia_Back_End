using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class WishlistController : BaseApiController
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IMapper _mapper;

        public WishlistController(IWishlistRepository wishlistRepository, IMapper mapper)
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerWishlist>> GetWishlistById(string id)
        {
            var wishlist = await _wishlistRepository.GetWishlistAsync(id);
            return Ok(wishlist ?? new CustomerWishlist(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerWishlist>> UpdateWishlist(CustomerWishlistVM wishlist)
        {
            var CustomerWishlist = _mapper.Map<CustomerWishlistVM, CustomerWishlist>(wishlist);
            var updatedWishlist = await _wishlistRepository.UpdateWishlistAsync(CustomerWishlist);
            return Ok(updatedWishlist);
        }

        [HttpDelete]
        public async Task DeleteWishlistAsync(string id)
        {
            await _wishlistRepository.DeleteWishlistAsync(id);
        }
    }
}
