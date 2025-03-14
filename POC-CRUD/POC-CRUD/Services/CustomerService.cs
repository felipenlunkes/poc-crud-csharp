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

    public void Add(Customer product) => _repository.Add(product);

    public void Update(Customer product) => _repository.Update(product);

    public void Delete(int id) => _repository.Delete(id);
}