using POC_CRUD.Data;
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
        
        accountFound.UpdatedAt = DateTime.UtcNow;
        accountFound.Removed = true;

        _dbContext.Accounts.Add(accountFound);
    }
}