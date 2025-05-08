using POC_CRUD.Data;
using POC_CRUD.DTOs;
using POC_CRUD.Exceptions;
using POC_CRUD.Models;

namespace POC_CRUD.Repositories;

public class AccountRepository : IRepository
{
    private readonly AppDbContext _dbContext;

    public AccountRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public void Add(Account account)
    {
        _dbContext.Accounts.Add(account);
        _dbContext.SaveChanges();
    }

    public Account GetById(Guid id)
    {
        return _dbContext.Accounts.FirstOrDefault(a => a.Id == id && a.Removed == false);
    }
    
    public Account GetByCpf(string cpf) 
    {
        return _dbContext.Accounts.FirstOrDefault(a => a.Cpf == cpf && a.Removed == false);
    }
    
    public Account GetByCnpj(string cnpj) 
    {
        return _dbContext.Accounts.FirstOrDefault(a => a.Cnpj == cnpj && a.Removed == false);
    }

    public Account GetByUserId(Guid userId)
    {
        return _dbContext.Accounts.FirstOrDefault(a => a.UserId == userId && a.Removed == false);
    }

    public void RemoveById(Guid id)
    {
        var accountFound = _dbContext.Accounts.FirstOrDefault(a => a.Id == id && a.Removed == false);

        if (accountFound == null)
        {
            throw new NotFoundException("Account not found to remove: " + id);
        }
        
        accountFound.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        accountFound.Removed = true;

        _dbContext.Accounts.Update(accountFound);
        _dbContext.SaveChanges();
    }
    
    public List<Account> Query(AccountQueryDto filter)
    {
        var query = _dbContext.Accounts.AsQueryable();

        if (filter.UserId != null) {
            query = query.Where(a => a.UserId == filter.UserId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(a => a.Name.Contains(filter.Name));
        }

        if (!string.IsNullOrWhiteSpace(filter.BusinessName))
        {
            query = query.Where(a => a.BusinessName.Contains(filter.BusinessName));
        }

        if (!string.IsNullOrWhiteSpace(filter.Cpf))
        {
            query = query.Where(a => a.Cpf.Equals(filter.Cpf));
        }

        if (!string.IsNullOrWhiteSpace(filter.Cnpj))
        {
            query = query.Where(a => a.Cnpj.Equals(filter.Cnpj));
        }

        if (filter.AllowsAdvertising != null)
        {
            query = query.Where(a => a.AllowsAdvertising.Equals(filter.AllowsAdvertising));
        }
        
        if (filter.CreatedAtFrom != null)
        {
            query = query.Where(a => a.CreatedAt <= filter.CreatedAtFrom);
        }

        if (filter.CreatedAtTo != null)
        {
            query = query.Where(a => a.CreatedAt <= filter.CreatedAtTo);
        }

        query = query.Where(a => !a.Removed)
            .OrderBy(a => a.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);

        return query.ToList();
    }
}