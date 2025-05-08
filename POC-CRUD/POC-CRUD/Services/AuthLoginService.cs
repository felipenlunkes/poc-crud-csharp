using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using POC_CRUD.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using POC_CRUD.DTOs;
using POC_CRUD.Exceptions;
using POC_CRUD.Models;
using POC_CRUD.Repositories;

namespace POC_CRUD.Services;

public class AuthLoginService : IService
{
    private readonly IConfiguration _configuration;
    private readonly UserRepository _userRepository;
    private readonly AccountService _accountService;

    public AuthLoginService(IConfiguration configuration, UserRepository userRepository, AccountService accountService)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _accountService = accountService;
    }
    
        public User AddUser(User user)
    {

        if (_userRepository.GetByEmail(user.Email) != null)
        {
            throw new ValidationException("A user with this same email is already registered: " + user.Email);
        }
        
        user.Id = Guid.NewGuid();
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        _userRepository.Add(user);
        
        user.Password = null;
        
        return user;
    }
    
    public User Update(Guid userId, User user)
    {
        
        var userToUpdate = _userRepository.GetById(userId);

        if (userToUpdate == null)
        {
            throw new NotFoundException("User not found: " + userId);
        }
        
        userToUpdate.IsAdmin = user.IsAdmin;
        userToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        userToUpdate.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        _userRepository.Add(user);
        
        userToUpdate.Password = null;
        
        return userToUpdate;
    }
    
    public void RemoveUser(Guid userId)
    {
        var user = _userRepository.GetById(userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        // Checar se existe uma conta não excluída que esteja vinculada a esse usuário
            
        var account = _accountService.GetByUserId(userId);

        if (account != null)
        {
            throw new ValidationException("Thre are an account linked to this user: " + account.Id);
        }
        
        _userRepository.RemoveById(userId);
    }
    
    public User GetById(Guid id)
    {
        var user = _userRepository.GetById(id);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        user.Password = null;
        
        return user;
    }
    
    public IEnumerable<User> Query(UserQueryDto filter)
    {
        return _userRepository.Query(filter);
    }
    
    public IActionResult Login(LoginRequest request)
    {
        var user = _userRepository.GetByEmail(request.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return new UnauthorizedObjectResult("Invalid credential");

        var token = GenerateToken(user, _configuration);
        
        return new OkObjectResult(new
        {
            token,
            user = new
            {
                id = user.Id,
                email = user.Email,
                isAdmin = user.IsAdmin
            }
        });
    }
    
    private string GenerateToken(User user, IConfiguration config)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User") 
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(config["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}