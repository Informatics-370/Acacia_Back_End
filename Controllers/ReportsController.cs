using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Errors;
using Acacia_Back_End.Helpers;
using Acacia_Back_End.Infrastructure.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Acacia_Back_End.Controllers
{
    public class ReportsController : BaseApiController
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportsController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        [HttpGet("ProductTrends")]
        public async Task<ActionResult<Pagination<ProductVM>>> GetProducts([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetProductsReportAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(reports);
        }

        [HttpGet("SalesReport")]
        public async Task<ActionResult<SalesReportVM>> GetSalesReport([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetSaleOrdersReportAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(reports);
        }

        [HttpGet("SupplierOrderReport")]
        public async Task<ActionResult<SupplierReportVM>> SupplierOrder([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetSupplierOrdersReportAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(reports);
        }

        [HttpGet("ProfitabilityReport")]
        public async Task<ActionResult<ProfitabilityReportVM>> GetProfitabilityReport([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetProfitabilityReportAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(reports);
        }

        [HttpGet("Promotions")]
        public async Task<ActionResult<PromotionVM>> GetPromotionsReport([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetPromotionsReportAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(reports);
        }
        [HttpGet("ProductsList")]
        public async Task<ActionResult<ProductVM>> GetProductsList([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetProductsListAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<IReadOnlyList<ProductVM>>(reports));
        }

        [HttpGet("SuppliersList")]
        public async Task<ActionResult<UserVM>> GetSuppliersList([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetSupplierListAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<IReadOnlyList<SupplierVM>>(reports));
        }

        [HttpGet("UsersList")]
        public async Task<ActionResult<ReportsVM>> GetUsersList([FromQuery] ReportParams specParams)
        {
            var reports = await _reportService.GetUsersListAsync(specParams);

            if (reports == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<IReadOnlyList<UserVM>>(reports));
        }
    }
}
