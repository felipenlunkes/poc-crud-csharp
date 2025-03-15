using POC_CRUD.Data;
using POC_CRUD.Models;

namespace POC_CRUD.Repositories;

using System.Collections.Generic;
using System.Linq;

public class CustomerRepository : IRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Customer> GetAll() => _context.Customers.ToList();

    public Customer GetById(Guid id) => _context.Customers.Find(id);

    public void Add(Customer product)
    {
        _context.Customers.Add(product);
        _context.SaveChanges();
    }

    public void Update(Customer product)
    {
        _context.Customers.Update(product);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var product = _context.Customers.Find(id);
        if (product == null)
        {
            return;
        }

        _context.Customers.Remove(product);
        _context.SaveChanges();
    }
}
