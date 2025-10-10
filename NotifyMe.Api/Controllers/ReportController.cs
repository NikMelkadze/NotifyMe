using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models;

namespace NotifyMe.Api.Controllers;

[ApiController]
[Route("report")]
public class ReportController(IReportService reportService) : Controller
{
    [HttpGet("products{top}")]
    public async Task<ActionResult<IEnumerable<SavedProduct>>> GetTopSavedProducts([FromRoute] int top,
        CancellationToken cancellationToken)
    {
        return Ok(await reportService.GetTopSavedProducts(top, cancellationToken));
    }
}