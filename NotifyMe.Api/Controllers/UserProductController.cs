using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Models;

namespace NotifyMe.Api.Controllers;
[Authorize]
[Route("user-product")]
public class UserProductController(IUserProductService userProductService) : Controller
{
    [HttpPost]
    public async Task<ActionResult> SaveProduct([FromBody] AddProductRequest productRequest ,CancellationToken cancellationToken)
    {
        var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
        await userProductService.SaveProduct(productRequest.Url,int.Parse(userId!),productRequest.NotificationType,cancellationToken);
        return Ok();
    } 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserSavedProduct>>> GetProducts(CancellationToken cancellationToken)
    {
        var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await userProductService.GetProducts(int.Parse(userId!),cancellationToken));
    }
} 