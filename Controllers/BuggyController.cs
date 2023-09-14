using Microsoft.AspNetCore.Mvc;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace Acacia_Back_End.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly Context _context;
        public BuggyController(Context context) 
        {
            _context = context;
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "secret stuff";
        }

        [HttpGet("not found")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(42);
            
            if(thing == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);

            var thingToReturn = thing.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
