using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("getToken")]
    public IActionResult GetToken(AuthRequest request)
    {
        try
        {
            var token = _authService.GetToken(request.Username, request.Password);
            return Ok(token);
        }
        catch (System.Exception _)
        {
            return BadRequest("Check your credentails");
        }
    }
}