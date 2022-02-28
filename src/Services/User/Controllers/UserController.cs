using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Password.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/<UserController>
    // [HttpGet]
    // public IActionResult Get()
    // {
    //     var users = _userService.GetUsers();
    //     return Ok(users);
    // }

    // GET api/<UserController>/5
    // [HttpGet("{id}")]
    // public IActionResult Get(string id)
    // {
    //     var user = _userService.GetUser(id);
    //     return Ok(user);
    // }

    // POST api/<UserController>
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Post([FromBody] User user)
    {
        _userService.InsertUser(user);
        return Ok();
    }

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _userService.Delete(id);
        return Ok();
    }
}