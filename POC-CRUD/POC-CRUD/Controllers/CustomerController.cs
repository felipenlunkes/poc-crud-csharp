using Microsoft.AspNetCore.Authorization;
using POC_CRUD.Models;
using POC_CRUD.Services;

namespace POC_CRUD.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("rest/v{version:apiVersion}/customer")]
[ApiVersion("1")] // Define a versão da API
public class CustomerController : ControllerBase
{
    private readonly CustomerService _service;

    public CustomerController(CustomerService service)
    {
        _service = service;
    }

    [HttpGet]
    public IEnumerable<Customer> GetAll()
    {
        return _service.GetAll();
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<Customer> Get([FromBody] Guid id)
    {
        var customer = _service.GetById(id);
        
        return Ok(customer);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Add([FromBody] Customer customer)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var customerAdded = _service.Add(customer);
        
        return Ok(customerAdded);
    }

    [Authorize]
    [HttpPut("{customerId}")]
    public IActionResult Update(Guid customerId, [FromBody] Customer customer)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customerUpdated = _service.Update(customerId, customer);
        
        return Ok(customerUpdated);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{customerId}")]
    public IActionResult Delete(Guid customerId)
    {
        
        _service.Delete(customerId);
        
        return NoContent();
    }
}