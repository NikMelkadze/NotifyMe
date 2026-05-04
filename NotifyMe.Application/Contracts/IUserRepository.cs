using NotifyMe.Application.Models.User;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Contracts;

public interface IUserRepository
{
    Task Register(RegisterModel register, CancellationToken cancellationToken);
    Task<string> LogIn(LoginModel loginModel, CancellationToken cancellationToken);
    Task Edit(int id, EditUserModel request, CancellationToken cancellationToken);
    Task<UserDetailsModel> Details(int id, CancellationToken cancellationToken);

    Task RecoveryPassword(string password, string confirmedPassword, string email, string code,
        CancellationToken cancellationToken);
    Task UpdatePassword(int userId,UpdatePasswordModel request, CancellationToken cancellationToken);

    Task SendOtp(string email, OtpOperationType operationType, OtpType type,
        CancellationToken cancellationToken);

    Task ValidateOtp(string email, string code,
        CancellationToken cancellationToken);
}