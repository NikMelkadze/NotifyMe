using NotifyMe.Application.Models.User;

namespace NotifyMe.Application.Contracts;

public interface IUserRepository
{
    Task Register(RegisterModel register);
    Task<string> LogIn(LoginModel loginModel);
    Task Edit(int id, EditUserModel request, CancellationToken cancellationToken);
    Task<UserDetailsModel> Details(int id, CancellationToken cancellationToken);
}