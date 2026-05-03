using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotifyMe.Application.Contracts;
using NotifyMe.Application.Models.User;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Persistence;
using ValidationException = NotifyMe.Domain.Exceptions.ValidationException;

namespace NotifyMe.Infrastructure.Services;

public class UserRepository(ApplicationDbContext dbContext,INotificationService  notificationService) : IUserRepository
{
    public async Task Register(RegisterModel register,CancellationToken cancellationToken)
    {
        if (register.Password != register.ConfirmPassword)
        {
            throw new ValidationException("Password and re-entered password do not match");
        }

        var existingUser =
            await dbContext.User.SingleOrDefaultAsync(x =>
                x.Email == register.Email || x.PhoneNumber == register.PhoneNumber,cancellationToken);

        if (existingUser is not null)
        {
            throw new ValidationException("User with email address or phone number already exists");
        }

        dbContext.User.Add(new User
        {
            FirstName = register.FirstName,
            LastName = register.LastName,
            Email = register.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
            PhoneNumber = register.PhoneNumber
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<string> LogIn(LoginModel loginModel, CancellationToken cancellationToken)
    {
        var user = await dbContext.User.SingleOrDefaultAsync(x =>
            x.Email == loginModel.EmailOrPhoneNumber || x.PhoneNumber == loginModel.EmailOrPhoneNumber,cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
        {
            throw new ValidationException("Invalid credentials.");
        }

        return GenerateJwtToken(user.Email, user.Id);
    }

    public async Task Edit(int id, EditUserModel request, CancellationToken cancellationToken)
    {
        var user = await dbContext.User
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (request.FirstName != null)
        {
            user!.FirstName = request.FirstName;
        }

        if (request.LastName != null)
        {
            user!.LastName = request.LastName;
        }

        if (request.Email != null)
        {
            user!.Email = request.Email;
        }

        if (request.PhoneNumber != null)
        {
            user!.PhoneNumber = request.PhoneNumber;
        }

        if (request.Password != null)
        {
            user!.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserDetailsModel> Details(int id, CancellationToken cancellationToken)
    {
        var user = await dbContext.User.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        return new UserDetailsModel()
        {
            FirstName = user!.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task SendOtp(string email, OtpOperationType operationType, OtpType type,
        CancellationToken cancellationToken)
    {
        var code = RandomNumberGenerator.GetInt32(0, 10000).ToString("D4");
        
        var userId = await dbContext.User.Where(x => x.Email == email).Select(x=>x.Id).FirstAsync(cancellationToken);


        var otp = new Otp
        {
            Code = code,
            ActiveMinutes = 3,
            CreationDate = DateTime.Now,
            OperationType = operationType,
            UserId = userId,
            Status = OtpStatus.Valid,
            Type = type
        };

        var validOtp = await dbContext.Otp.FirstOrDefaultAsync(x =>
                x.UserId == userId
                && x.Status == OtpStatus.Valid
                && x.Type == type
                && x.OperationType == operationType,
            cancellationToken);

        if (validOtp != null)
        {
            validOtp.Status = OtpStatus.Invalid;
        }

        dbContext.Otp.Add(otp);
        await dbContext.SaveChangesAsync(cancellationToken);

        
        notificationService.SendEmail(email,"NotifyMe - Code",$"ერთჯერადი კოდი : {code}");
    }

    private string GenerateJwtToken(string email, int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = "app-secret-key-notify-strong-token"u8.ToArray();
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