using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using POC_CRUD.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using POC_CRUD.Exceptions;
using POC_CRUD.Models;
using POC_CRUD.Repositories;

namespace POC_CRUD.Services;

public class AuthLoginService : IService
{
    private readonly IConfiguration _configuration;
    private readonly UserRepository _userRepository;

    public AuthLoginService(IConfiguration configuration, UserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
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
        userToUpdate.UpdatedAt = DateTime.UtcNow;

        _userRepository.Add(user);

        userToUpdate.Password = null;

        return userToUpdate;
    }

    public User AddUser(User user)
    {
        user.Id = Guid.NewGuid();

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _userRepository.Add(user);

        user.Password = null;

        return user;
    }

    public void RemoveUser(Guid userId)
    {
        var user = _userRepository.GetById(userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        _userRepository.RemoveById(userId);
    }

    public IActionResult Login(LoginRequest request)
    {
        if (request == null)
        {
            throw new ValidationException("request is required");
        }

        if (request.Email == null || request.Password == null)
        {
            throw new ValidationException("email and password are required");
        }

        var user = _userRepository.GetByEmail(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return new UnauthorizedObjectResult("Invalid credentials");

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