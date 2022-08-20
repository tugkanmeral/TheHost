using Microsoft.AspNetCore.Mvc;

namespace Tool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToolController : ControllerBase
{
    [HttpGet("guid")]
    public string Get()
    {
        return Guid.NewGuid().ToString();
    }
}
