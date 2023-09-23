using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Controllers
{
    public class AuditTrailController : BaseApiController
    {
        private readonly IAuditRepository _auditRepo;

        public AuditTrailController(IAuditRepository auditRepo)
        {
            _auditRepo = auditRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<AuditTrailVM>>> AuditTrail([FromQuery] AuditSpecParams specParams)
        {
            var auditTrail = await _auditRepo.GetAuditTrailAsync(specParams);

            var data = auditTrail.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList();

            return Ok(new Pagination<AuditTrailVM>(specParams.PageIndex, specParams.PageSize, auditTrail.Count, data));
        }
    }
}
