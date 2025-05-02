using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using POC_CRUD.Models;
using POC_CRUD.Services;

namespace POC_CRUD.Controllers;

[Route("rest/v{version:apiVersion}/user")]
[ApiVersion("1")] // Define a versão da API
public class AuthController : ControllerBase
{
    private readonly AuthLoginService _loginService;

    public AuthController(AuthLoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        return _loginService.Login(request);
    }

    [HttpPost]
    public IActionResult Add([FromBody] User request)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = _loginService.AddUser(request);

        return Ok(user);
    }

    [HttpPut]
    public IActionResult Update([FromBody] Guid userId, [FromBody] User request)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = _loginService.Update(userId, request);

        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public IActionResult Delete([FromBody] Guid userId)
    {
        _loginService.RemoveUser(userId);

        return NoContent();
    }
}