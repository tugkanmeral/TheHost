using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using En = Password.Entities;

namespace Password.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PasswordsController : ControllerBase
{
    IPasswordService _passwordService;
    public PasswordsController(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    // GET: api/<PasswordsController>
    [HttpGet]
    public IActionResult Get()
    {
        var passwords = _passwordService.GetPasswords(User.GetUserId());
        return Ok(passwords);
    }

    // GET api/<PasswordsController>/5
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var password = _passwordService.GetPassword(id, User.GetUserId(), User.GetUserMasterKey());
        return Ok(password);
    }

    // POST api/<PasswordsController>
    [HttpPost]
    public IActionResult Post([FromBody] En.Password password)
    {
        _passwordService.InsertPassword(password, User.GetUserId(), User.GetUserMasterKey());
        return Ok();
    }

    // PUT api/<PasswordsController>/5
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] En.Password password)
    {
        password.Id = id;
        _passwordService.UpdatePassword(password, User.GetUserId(), User.GetUserMasterKey());
        return Ok();
    }

    // DELETE api/<PasswordsController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _passwordService.DeletePassword(id, User.GetUserId());
        return Ok();
    }
}