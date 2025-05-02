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
    public IEnumerable<Customer> Get()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Customer> Get([FromBody] Guid id)
    {
        var customer = _service.GetById(id);
        if (customer == null)
            return NotFound();

        return customer;
    }

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

    [HttpPut("{id}")]
    public IActionResult Update([FromBody] Guid id, [FromBody] Customer customer)
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