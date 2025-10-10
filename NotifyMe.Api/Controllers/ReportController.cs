using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Api.Controllers;

[ApiController]
[Route("report")]
public class ReportController(IReportService reportService) : Controller
{
    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<SavedProduct>>> GetTopSavedProducts([FromQuery] int top,
        CancellationToken cancellationToken)
    {
        return Ok(await reportService.GetTopSavedProducts(top, cancellationToken));
    }

    [HttpGet("{shop}/products")]
    public async Task<ActionResult<IEnumerable<SavedProduct>>> GetTopSavedProducts([FromRoute] Shop shop,
        [FromQuery] int top, CancellationToken cancellationToken)
    {
        return Ok(await reportService.GetCompanyTopSavedProducts(top, shop, cancellationToken));
    }
}