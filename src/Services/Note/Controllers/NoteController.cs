using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using En = Note.Entities;

namespace Note.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    INoteService _noteService;
    AppSettings _appSettings;
    public NoteController(INoteService noteService, AppSettings appSettings)
    {
        _noteService = noteService;
        _appSettings = appSettings;
    }

    [HttpPost]
    public IActionResult Post([FromBody] En.Note note)
    {
        _noteService.InsertNote(note, User.GetUserId());
        return Ok();
    }
}
