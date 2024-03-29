using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using En = Note.Entities;

namespace Note.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    INoteService _noteService;
    AppSettings _appSettings;
    public NotesController(INoteService noteService, AppSettings appSettings)
    {
        _noteService = noteService;
        _appSettings = appSettings;
    }


    // POST api/<NotesController>
    [HttpPost]
    public IActionResult Post([FromBody] En.Note note)
    {
        _noteService.InsertNote(note, User.GetUserId());
        return Ok();
    }

    // GET: api/<NotesController>
    [HttpGet]
    public async Task<ItemsResponse> Get(int skip = 0, int take = 10, string? searchText = null, [FromQuery(Name = "tags")] string[]? tags = null)
    {
        var notes = _noteService.GetNotes(User.GetUserId(), skip, take, searchText, tags);
        var totalNotesCount = await _noteService.GetTotalNotesCount(User.GetUserId());

        ItemsResponse response = new();
        response.Items = notes;
        response.TotalItemCount = totalNotesCount;
        response.Skip = skip;
        response.Take = take;

        return response;
    }

    // GET api/<NotesController>/5
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var note = _noteService.GetNote(id, User.GetUserId());
        return Ok(note);
    }

    // PUT api/<NotesController>/5
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] En.Note note)
    {
        note.Id = id;
        _noteService.UpdateNote(note, User.GetUserId());
        return Ok();
    }

    // DELETE api/<NotesController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _noteService.DeleteNote(id, User.GetUserId());
        return Ok();
    }
}
