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
    public string? GetToken(AuthRequest request)
    {
        var token = _authService.GetToken(request.Username, request.Password, request.MasterKey);
        return token;
    }
}