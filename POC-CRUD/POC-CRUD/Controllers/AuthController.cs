using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using POC_CRUD.DTOs;
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
    
    [Authorize]
    [HttpPut("{userId}")]
    public IActionResult Update(Guid userId, [FromBody] User request)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = _loginService.Update(userId, request);
        
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public IActionResult Delete(Guid userId)
    {
        _loginService.RemoveUser(userId);
        
        return NoContent();
    }
    
    [Authorize]
    [HttpGet("{userId}")]
    public IActionResult Get(Guid userId)
    {
        var user = _loginService.GetById(userId);
        
        return Ok(user);
    }
      
    [Authorize]
    [HttpGet("query")]
    public IActionResult Query([FromQuery] UserQueryDto filter)
    {
        var results = _loginService.Query(filter);
        
        return Ok(results);
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        return _loginService.Login(request);
    }
}