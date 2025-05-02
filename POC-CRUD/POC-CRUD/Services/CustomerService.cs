using POC_CRUD.Exceptions;
using POC_CRUD.Models;
using POC_CRUD.Repositories;

namespace POC_CRUD.Services;

public class CustomerService : IService
{
    private readonly CustomerRepository _repository;

    public CustomerService(CustomerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Customer> GetAll()
    {
        return _repository.GetAll();
    }

    public Customer GetById(Guid id)
    {
        return _repository.GetById(id);
    }

    public Customer Add(Customer input)
    {
        input.Id = Guid.NewGuid();
        
        _repository.Add(input);
        
        return input;
    }

    public void Update(Guid id, Customer input)
    {
        var customerToUpdate = _repository.GetById(id);
        
        if (customerToUpdate == null)
        {
            throw new NotFoundException("Customer not found: " + id);
        }
        
        customerToUpdate.Email = input.Email;
        customerToUpdate.Address = input.Address;
        customerToUpdate.Name = input.Name;
        customerToUpdate.UpdatedAt = DateTime.UtcNow;
        
        _repository.Add(input);
    }

    public void Delete(Guid id)
    {
        var customer = _repository.GetById(id);
        
        if (customer == null)
        {
            throw new NotFoundException("Customer not found: " + id);
        }
        
        customer.UpdatedAt = DateTime.UtcNow;
        customer.Removed = true;
        
        _repository.Add(customer);
    }
}