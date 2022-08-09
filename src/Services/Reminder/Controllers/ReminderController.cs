using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Reminder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReminderController : ControllerBase
{
    private readonly IReminderService _reminderService;
    public ReminderController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpPost]
    public IActionResult Post(ReminderRequest request)
    {
        _reminderService.SaveReminder(request, User.GetUserId());
        return Ok();
    }
}