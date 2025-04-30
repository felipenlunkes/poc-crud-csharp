using Microsoft.AspNetCore.Mvc;
using POC_CRUD.Data;
using POC_CRUD.Models;

namespace POC_CRUD.Repositories;

public class UserRepository : IRepository
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;

    public UserRepository(IConfiguration configuration, AppDbContext dbContext)
    {
        _configuration = configuration;
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
        _dbContext.Users.Remove(_dbContext.Users.FirstOrDefault(u => u.Id == id));
    } 
    
}