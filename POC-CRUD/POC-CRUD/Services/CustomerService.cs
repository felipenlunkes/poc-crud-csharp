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

    public IEnumerable<Customer> GetAll() => _repository.GetAll();

    public Customer GetById(Guid id) => _repository.GetById(id);

    public void Add(Customer product)
    {
        product.Id = Guid.NewGuid();
        _repository.Add(product);
    }

    public void Update(Guid id, Customer product)
    {
        var customer = _repository.GetById(id);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found");
        }
        
        product.Id = id;
        _repository.Update(product);
    }

    public void Delete(Guid id)
    {
        var customer = _repository.GetById(id);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found");
        }
        
        _repository.Delete(id);
    }
}