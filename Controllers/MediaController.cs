using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class MediaController : BaseApiController
    {
        private readonly IGenericRepository<Media> _mediaRepo;

        public MediaController(IGenericRepository<Media> mediaRepo)
        {
            _mediaRepo = mediaRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Media>> GetMedia(int id)
        {
            var media = await _mediaRepo.GetByIdAsync(id);

            if (media == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(media);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<Media>>> GetMediaList([FromQuery] SpecParams specParams)
        {
            var mediaList = await _mediaRepo.GetMediaListAsync(specParams);

            if (mediaList == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(new Pagination<Media>(specParams.PageIndex, specParams.PageSize, mediaList.Count, mediaList.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList()));
        }

        [HttpPost("add")]
        //[Authorize]
        public async Task<ActionResult<Media>> AddMedia(Media media)
        {
            var result = await _mediaRepo.AddEntity(new Media
            {
                Name = media.Name,
                Description = media.Description,
                FileUrl = media.FileUrl,
                Type = MediaType.Video
            });

            if (result == true) return Ok(result);

            return BadRequest(new ApiResponse(400));
        }

        [HttpPut("update/{id}")]
        //[Authorize]
        public async Task<ActionResult<Media>> UpdateMedia(int id, Media newMedia)
        {
            var media = await _mediaRepo.GetByIdAsync(id);
            if (media == null) return NotFound(new ApiResponse(404));

            media.Name = newMedia.Name;
            media.Description = newMedia.Description;
            media.FileUrl = newMedia.FileUrl;


            var result = await _mediaRepo.UpdateEntity(media);
            if (result == true) return Ok(media);
            return BadRequest(new ApiResponse(400));
        }

        [HttpDelete("delete/{id}")]
        //[Authorize]
        public async Task<ActionResult<Media>> DeleteMedia(int id)
        {
            var media = await _mediaRepo.GetByIdAsync(id);

            if (media == null) return NotFound(new ApiResponse(404));

            var result = await _mediaRepo.RemoveEntity(id);

            if (result == true) return Ok(media);

            return BadRequest(new ApiResponse(400));
        }
    }
}
