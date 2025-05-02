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
    public IEnumerable<Customer> Get() => _service.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Customer> Get([FromBody] Guid id)
    {
        var customer = _service.GetById(id);
        if (customer == null)
            return NotFound();

        return customer;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Customer customer)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        _service.Add(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public IActionResult Put([FromBody] Guid id, [FromBody] Customer customer)
    {

        if (id != customer.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _service.Update(id, customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromBody] Guid id)
    {
        _service.Delete(id);
        return NoContent();
    }
}