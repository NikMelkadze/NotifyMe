using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models.User;

namespace NotifyMe.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserRepository userRepository) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        await userRepository.Register(registerModel);
        return Ok();
    }

    [HttpPost("log-in")]
    public async Task<ActionResult<string>> LogIn([FromBody] LoginModel loginModel)
    {
        return Ok(await userRepository.LogIn(loginModel));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDetailsModel>> Details(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Ok(await userRepository.Details(int.Parse(userId!), cancellationToken));
    }


    [HttpPatch]
    public async Task<IActionResult> Edit([FromBody] EditUserModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await userRepository.Edit(int.Parse(userId!), request, cancellationToken);
        return Ok();
    }
}