using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models;
using NotifyMe.Application.Models.UserProducts;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Api.Controllers;

[Authorize]
[Route("user-product")]
public class UserProductController(IUserProductService userProductService) : Controller
{
    [HttpPost]
    public async Task<ActionResult> SaveProduct([FromBody] AddProductRequest productRequest,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await userProductService.SaveProduct(productRequest.Url, int.Parse(userId!), productRequest.NotificationType,
            cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<UserSavedProductsResponse>> GetProducts([FromQuery] bool hasChangedPrice,
        [FromQuery] ProductStatus? status, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(
            await userProductService.GetProducts(int.Parse(userId!), hasChangedPrice, status, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await userProductService.DeleteProduct(id, int.Parse(userId!), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditProduct([FromRoute] int id, [FromBody] EditProductRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await userProductService.EditProduct(id, int.Parse(userId!), request.Status, request.NotificationType,
            cancellationToken);
        return Ok();
    }
}