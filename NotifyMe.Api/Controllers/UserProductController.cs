using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;

namespace NotifyMe.Api.Controllers;
[Authorize]
[Route("user-product")]
public class UserProductController(IUserProductService userProductService) : Controller
{
    [HttpPost("/item")]
    public async Task<ActionResult> SaveProduct([FromBody] string url)
    {
        var userEmail=User.FindFirstValue(ClaimTypes.NameIdentifier);
        await userProductService.SaveProduct(url,int.Parse(userEmail!));
        return Ok();
    }
} 