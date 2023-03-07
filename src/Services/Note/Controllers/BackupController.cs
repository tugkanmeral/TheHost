using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using En = Note.Entities;

namespace Note.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BackupController : ControllerBase
{
    IBackupService _backupService;
    public BackupController(IBackupService backupService)
    {
        _backupService = backupService;
    }


    // POST api/<NotesController>
    [HttpGet]
    public async Task<ItemsResponse> Export([FromBody] En.Note note)
    {
        var notes = await _backupService.ExportNotes(User.GetUserId());

        ItemsResponse response = new();
        response.Items = notes;

        return response;
    }

    // GET: api/<NotesController>
    [HttpPost]
    public async Task<IActionResult> Import(List<Note.Entities.Note> notes)
    {
        await _backupService.ImportNotes(User.GetUserId(), notes);

        return Ok();
    }
}
