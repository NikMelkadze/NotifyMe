using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;

namespace NotifyMe.Api.Controllers;
[Route("User")]
public class UserController(IUserService userService) : Controller
{
    [HttpPost("/item")]
    public async Task<ActionResult> SaveProduct([FromBody] string url)
    {
        await userService.SaveProduct(url);
        return Ok();
    }
}