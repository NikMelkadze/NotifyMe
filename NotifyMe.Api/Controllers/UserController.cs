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
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel,
        CancellationToken cancellationToken)
    {
        await userRepository.Register(registerModel, cancellationToken);
        return Ok();
    }

    [HttpPost("log-in")]
    public async Task<ActionResult<string>> LogIn([FromBody] LoginModel loginModel, CancellationToken cancellationToken)
    {
        return Ok(await userRepository.LogIn(loginModel, cancellationToken));
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

    [HttpPost("otp")]
    public async Task<ActionResult> SendOtp([FromBody] OtpModel request, CancellationToken cancellationToken)
    {
        await userRepository.SendOtp(request.Email, request.OperationType, request.Type, cancellationToken);
        return Ok();
    }
}