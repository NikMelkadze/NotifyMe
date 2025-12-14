using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models;
using NotifyMe.Domain.Entities;
using NotifyMe.Persistence;
using ValidationException = NotifyMe.Domain.Exceptions.ValidationException;

namespace NotifyMe.Infrastructure.Services;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task AddUser(UserModel user)
    {
        var existingUser = await dbContext.User.SingleOrDefaultAsync(x => x.Email == user.Email || x.PhoneNumber== user.PhoneNumber);

        if (existingUser is not null)
        {
            throw new ValidationException("User with email address or phone number already exists");
        }

        dbContext.User.Add(new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            PhoneNumber = user.PhoneNumber
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task<string> LogIn(LoginModel loginModel)
    {
        var user = await dbContext.User.SingleOrDefaultAsync(x => x.Email == loginModel.EmailOrPhoneNumber || x.PhoneNumber== loginModel.EmailOrPhoneNumber);
        
        if (user is null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
        {
            throw new Domain.Exceptions.ValidationException("Invalid credentials.");
        }

        return GenerateJwtToken(user.Email, user.Id);
    }

    private string GenerateJwtToken(string email, int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("app-secret-key-notify-strong-token");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "your-issuer", // Set the Issuer if needed
            Audience = "your-audience", // Set the Audienc
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}