using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using POC_CRUD.DTOs;
using POC_CRUD.Exceptions;
using POC_CRUD.Models;
using POC_CRUD.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace POC_CRUD.Services;

public class AuthLoginService : IService
{
    
    private readonly IConfiguration _configuration;
    private readonly UserRepository _userRepository;
    private readonly AccountService _accountService;
    private readonly EmailService _emailService; 
    private readonly ILogger<ExceptionHandler> _logger;

    private const int TemporaryPasswordSize = 16;

    public AuthLoginService(IConfiguration configuration, UserRepository userRepository, AccountService accountService, EmailService emailService, ILogger<ExceptionHandler> logger)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _accountService = accountService;
        _emailService = emailService;
        _logger = logger;
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

    public void RecoverPassword(RecoverPasswordDto recoverPasswordDto)
    {
        var user = _userRepository.GetByEmail(recoverPasswordDto.Email);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        // Gerar nova senha aleatória
        var newPassword = GenerateTemporaryPassword();
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        _userRepository.Update(user);
        
        var emailPayload = new EmailPayload()
        {
            To = user.Email,
            Subject = "Recuperação de senha - POC-CRUD",
            Body = $"Olá! Sua nova senha é: <strong>{newPassword}</strong><br>Recomendamos que você a troque após o login."
        };

        try
        {
            _emailService.SendEmail(emailPayload).Wait();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error changing user password: " + exception.Message);
        }

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
            throw new ValidationException("There are an account linked to this user: " + account.Id);
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
        {
            throw new InvalidCredentialsException("Invalid credentials for user");
        }
        
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
            new Claim(ClaimTypes.Role, user.IsAdmin == true ? "Admin" : "User") 
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
    
    private string GenerateTemporaryPassword()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
       
        var random = new Random();
        
        return new string(Enumerable.Repeat(chars, TemporaryPasswordSize)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}