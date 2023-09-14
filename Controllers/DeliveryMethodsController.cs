using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class DeliveryMethodsController : BaseApiController
    {
        private readonly IGenericRepository<DeliveryMethod> _deliveryRepo;

        public DeliveryMethodsController(IGenericRepository<DeliveryMethod> deliveryRepo)
        {
            _deliveryRepo = deliveryRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _deliveryRepo.ListAllAsync();
            return Ok(deliveryMethods);
        }

        [HttpGet("List")]
        public async Task<ActionResult<Pagination<DeliveryMethod>>> GetDeliveryMethodsAsync([FromQuery] SpecParams specParams)
        {
            var deliveryMethods = await _deliveryRepo.GeDeliveryMethodsAsync(specParams);
            var data = deliveryMethods.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList();
            return Ok(new Pagination<DeliveryMethod>(specParams.PageIndex, specParams.PageSize, deliveryMethods.Count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethod(int id)
        {
            var deliveryMethod = await _deliveryRepo.GetByIdAsync(id);
            if (deliveryMethod == null) return NotFound(new ApiResponse(400));
            return Ok(deliveryMethod);
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryMethod>> AddDeliveryMethod(DeliveryMethod newDeliveryMethod)
        {
            var result = await _deliveryRepo.AddEntity(new DeliveryMethod()
            {
                Name = newDeliveryMethod.Name,
                Description = newDeliveryMethod.Description,
                Price = newDeliveryMethod.Price,
                DeliveryTime = newDeliveryMethod.DeliveryTime
            });
            if (result == false) return NotFound(new ApiResponse(400));
            return Ok(newDeliveryMethod);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DeliveryMethod>> UpdateDeliveryMethid(int id, DeliveryMethod newDeliverymethod)
        {
            var deliveryMethod = await _deliveryRepo.GetByIdAsync(id);
            if (deliveryMethod == null) return NotFound(new ApiResponse(400));

            deliveryMethod.DeliveryTime = newDeliverymethod.DeliveryTime;
            deliveryMethod.Price = newDeliverymethod.Price;
            deliveryMethod.Name = newDeliverymethod.Name;
            deliveryMethod.Description = newDeliverymethod.Description;

            var result = await _deliveryRepo.UpdateEntity(deliveryMethod);
            if(result == false) return BadRequest(new ApiResponse(400));
            return Ok(deliveryMethod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeliveryMethod(int id)
        {
            var result = await _deliveryRepo.RemoveEntity(id);
            if (result == false) return BadRequest(new ApiResponse(400));
            return Ok();
        }
    }
}
