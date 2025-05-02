using POC_CRUD.Data;
using POC_CRUD.Exceptions;
using POC_CRUD.Models;

namespace POC_CRUD.Repositories;

public class UserRepository : IRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public User GetById(Guid id)
    {
        return _dbContext.Users.Find(id);
    }
    
    public User GetByEmail(string email)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Email == email);
    }

    public void RemoveById(Guid id)
    {
        var userFound = _dbContext.Users.FirstOrDefault(u => u.Id == id && u.Removed == false);

        if (userFound == null)
        {
            throw new NotFoundException("Usuário não encontrado para remover: " + id);
        }
        
        userFound.UpdatedAt = DateTime.UtcNow;
        userFound.Removed = true;
        
        _dbContext.Users.Add(userFound);
    } 
    
}