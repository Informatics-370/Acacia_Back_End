using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using Acacia_Back_End.Infrastructure.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Acacia_Back_End.Controllers
{
    public class GiftBoxController : BaseApiController
    {
        private readonly IGenericRepository<GiftBox> _giftBoxRepo;
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly IGenericRepository<GiftBoxPrice> _giftboxPriceRepo;
        private readonly IGenericRepository<Product> _productsRepo;

        public GiftBoxController(IGenericRepository<GiftBox> giftBoxRepo, IMapper mapper, Context context, IGenericRepository<GiftBoxPrice> giftboxPriceRepo, IGenericRepository<Product> productsRepo)
        {
            _giftBoxRepo = giftBoxRepo;
            _mapper = mapper;
            _context = context;
            _giftboxPriceRepo = giftboxPriceRepo;
            _productsRepo = productsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ListGiftBoxVM>>> GetGiftboxes([FromQuery]SpecParams specParams)
        {
            var giftboxes = await _giftBoxRepo.GetGiftBoxesAsync(specParams);

            var data = _mapper.Map<IReadOnlyList<ListGiftBoxVM>>(giftboxes.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList());

            return Ok(new Pagination<ListGiftBoxVM>(specParams.PageIndex, specParams.PageSize, giftboxes.Count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GiftBoxVM>> GetGiftboxById(int id)
        {
            var giftbox = await _giftBoxRepo.GetGiftBoxByIdAsync(id);

            if (giftbox == null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<GiftBoxVM>(giftbox));
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<AddGiftBoxVM>> AddGiftBox([FromForm] AddGiftBoxVM giftboxVM)
        {
            var formCollection = await Request.ReadFormAsync();
            var imageUrl = "";
            if (formCollection.Files.Count() > 0)
            {
                var file = formCollection.Files.First();
                imageUrl = SaveGiftBoxImage(file).Result;
            }
            else
            {
                imageUrl = "images/products/default.jpg";
            }
           
            var newgiftbox = new GiftBox
            {
                Name = giftboxVM.Name,
                Description = giftboxVM.Description,
                GiftBoxImage = imageUrl,
                Products = new List<Product>(),
                PriceHistory = new List<GiftBoxPrice>(),
            };
            newgiftbox.PriceHistory.Add(new GiftBoxPrice()
            {
                StartDate = DateTime.Now,
                EndDate = null,
                Price = giftboxVM.Price,
                PackagingCosts = giftboxVM.PackagingCosts
            });
            var array = JsonConvert.DeserializeObject<ProductVM[]>(giftboxVM.Products);
            foreach (var prodId in array)
            {
                Product productToAdd = await _productsRepo.GetProductByIdAsync(prodId.Id);
                newgiftbox.Products.Add(productToAdd);
            }

            var result = await _giftBoxRepo.AddEntity(newgiftbox);
            if (result == true) return Ok(giftboxVM);
            return BadRequest(new ApiResponse(400));
        }

        [HttpPut("{id}"), DisableRequestSizeLimit]
        public async Task<ActionResult<GiftBoxVM>> UpdateGiftBox([FromForm] AddGiftBoxVM giftboxVM, int id)
        {
            // GET Giftbox and Current GiftBox
            var giftbox = await _giftBoxRepo.GetGiftBoxByIdAsync(id);
            if (giftbox == null && giftbox == null) return NotFound(new ApiResponse(404));

            var formCollection = await Request.ReadFormAsync();
            var imageUrl = "";
            if (formCollection.Files.Count() > 0)
            {
                var file = formCollection.Files.First();
                imageUrl = SaveGiftBoxImage(file).Result;
            }

            // Updating Current GiftBox
            giftbox.Name = giftboxVM.Name;
            giftbox.Description = giftboxVM.Description;
            giftbox.PriceHistory.Add(new GiftBoxPrice()
            {
                StartDate = DateTime.Now,
                EndDate = null,
                Price = giftboxVM.Price,
                PackagingCosts = giftboxVM.PackagingCosts
            });
            if (imageUrl != "")
            {
                giftbox.GiftBoxImage = imageUrl;
            }

            // Checks if price has changed
            var currentPrice = giftbox.PriceHistory.OrderByDescending(pp => pp.StartDate).First();
            if (giftboxVM.Price != currentPrice.Price || giftboxVM.PackagingCosts != currentPrice.PackagingCosts)
            {
                giftbox.PriceHistory.OrderByDescending(pp => pp.StartDate).First().EndDate = DateTime.Now;
                giftbox.PriceHistory.Add(new GiftBoxPrice()
                {
                    GiftBoxId = giftboxVM.Id,
                    Price = giftboxVM.Price,
                    PackagingCosts = giftboxVM.PackagingCosts,
                    StartDate = DateTime.Now
                });
            }

            giftbox.Products = new List<Product>();
            var array = JsonConvert.DeserializeObject<ProductVM[]>(giftboxVM.Products);
            foreach (var prodId in array)
            {
                Product productToAdd = await _productsRepo.GetProductByIdAsync(prodId.Id);
                giftbox.Products.Add(productToAdd);
            }

            var result = await _giftBoxRepo.UpdateEntity(giftbox);
            if (result == true) return Ok(giftboxVM);
            return BadRequest(new ApiResponse(400));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGiftBox(int id)
        {
            var result = await _giftBoxRepo.RemoveEntity(id);

            if(result == true) return Ok(); 
            return BadRequest(new ApiResponse(400));
        }

        private async Task<string> SaveGiftBoxImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                string filePath = Path.Combine("wwwroot/images/giftboxes/", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return "images/giftboxes/" + uniqueFileName;
            }
            return null;
        }

        private async Task<string> UpdateGiftBoxImage(IFormFile file, string imageurl)
        {
            if (file != null && file.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                string filePath = Path.Combine("wwwroot/images/giftboxes/", uniqueFileName);

                // Remove the existing image if it exists
                string existingFilePath = imageurl;
                if (System.IO.File.Exists("wwwroot/" + existingFilePath))
                {
                    System.IO.File.Delete("wwwroot/" + existingFilePath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return "images/giftboxes/" + uniqueFileName;
            }
            return null;
        }
    }
}
