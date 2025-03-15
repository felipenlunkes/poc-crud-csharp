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
    public ActionResult<Customer> Get(Guid id)
    {
        var customer = _service.GetById(id);
        if (customer == null)
            return NotFound();

        return customer;
    }

    [HttpPost]
    public IActionResult Post(Customer customer)
    {
        _service.Add(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Customer customer)
    {
        if (id != customer.Id)
            return BadRequest();

        _service.Update(id, customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _service.Delete(id);
        return NoContent();
    }
}