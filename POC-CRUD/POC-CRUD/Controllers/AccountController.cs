using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        
        var user = _accountService.AddAccount(request);
        
        return Ok(user);
    }
    
    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] Guid accountId, [FromBody] Account request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = _accountService.UpdateAccount(accountId, request);
        
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public IActionResult Delete([FromBody] Guid accountId)
    {
        _accountService.RemoveAccount(accountId);
        
        return NoContent();
    }
    
    public IActionResult GetById([FromBody] Guid accountId)
    {
        var account = _accountService.GetById(accountId);
        
        return Ok(account);
    }
}