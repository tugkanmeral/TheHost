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
    AppSettings _appSettings;
    public PasswordsController(IPasswordService passwordService, AppSettings appSettings)
    {
        _passwordService = passwordService;
        _appSettings = appSettings;
    }

    // GET: api/<PasswordsController>
    [HttpGet]
    public IActionResult Get(int skip = 0, int take = 10)
    {
        var passwords = _passwordService.GetPasswords(User.GetUserId(), skip, take);
        return Ok(passwords);
    }

    // GET api/<PasswordsController>/5
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var password = _passwordService.GetPassword(id, User.GetUserId(), _appSettings.PasswordPrivateKey);
        return Ok(password);
    }

    // POST api/<PasswordsController>
    [HttpPost]
    public IActionResult Post([FromBody] En.Password password)
    {
        _passwordService.InsertPassword(password, User.GetUserId(), _appSettings.PasswordPrivateKey);
        return Ok();
    }

    // PUT api/<PasswordsController>/5
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] En.Password password)
    {
        password.Id = id;
        _passwordService.UpdatePassword(password, User.GetUserId(), _appSettings.PasswordPrivateKey);
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