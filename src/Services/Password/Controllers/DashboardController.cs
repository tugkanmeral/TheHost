using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using En = Password.Entities;

namespace Password.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class DashboardController : ControllerBase
{
    IDashboardService _dashboardService;
    AppSettings _appSettings;
    public DashboardController(IDashboardService dashboardService, AppSettings appSettings)
    {
        _dashboardService = dashboardService;
        _appSettings = appSettings;
    }

    // GET: api/<PasswordsController>
    [HttpGet]
    public async Task<IActionResult> TotalPasswordCount()
    {
        var count = await _dashboardService.TotalPasswordCount(User.GetUserId());
        return Ok(count);
    }
}