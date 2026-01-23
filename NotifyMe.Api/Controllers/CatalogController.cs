using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;

namespace NotifyMe.Api.Controllers;

[Route("catalogs")]
public class CatalogController(ICatalogService catalogService) : Controller
{
    [HttpGet("shops")]
    public Task<ActionResult<string[]>> GetProducts()
    {
        return Task.FromResult<ActionResult<string[]>>(Ok(catalogService.GetShops()));
    }
}