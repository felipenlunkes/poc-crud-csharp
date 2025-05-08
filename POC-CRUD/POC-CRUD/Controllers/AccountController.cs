using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC_CRUD.DTOs;
using POC_CRUD.Models;
using POC_CRUD.Services;

namespace POC_CRUD.Controllers;

[Route("rest/v{version:apiVersion}/account")]
[ApiVersion("1")] // Define a versão da API
public class AccountController : ControllerBase
{
    
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public IActionResult Add([FromBody] Account request)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var account = _accountService.AddAccount(request);
        
        return Ok(account);
    }
    
    [Authorize]
    [HttpPut("{accountId}")]
    public IActionResult Update(Guid accountId, [FromBody] Account request)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = _accountService.UpdateAccount(accountId, request);
        
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{accountId}")]
    public IActionResult Delete(Guid accountId)
    {
        _accountService.RemoveAccount(accountId);
        
        return NoContent();
    }
    
    [Authorize]
    [HttpGet("{accountId}")]
    public IActionResult GetById(Guid accountId)
    {
        var account = _accountService.GetById(accountId);
        
        return Ok(account);
    }
    
    [Authorize]
    [HttpGet("user/{userId}")]
    public IActionResult GetUserId(Guid userId)
    {
        var account = _accountService.GetByUserId(userId);
        
        return Ok(account);
    }
    
    [Authorize]
    [HttpGet("query")]
    public IActionResult Query([FromQuery] AccountQueryDto filter)
    {
        var results = _accountService.Query(filter);
        
        return Ok(results);
    }
}