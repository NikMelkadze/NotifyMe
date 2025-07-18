using Microsoft.AspNetCore.Mvc;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models;

namespace NotifyMe.Api.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserRepository userRepository) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserModel userModel)
    {
        await userRepository.AddUser(userModel);
        return Ok();
    }
    
    [HttpPost("log-in")]
    public async Task<ActionResult<string>> LogIn([FromBody] LoginModel loginModel)
    {
        return Ok(await userRepository.LogIn(loginModel));
    }
}