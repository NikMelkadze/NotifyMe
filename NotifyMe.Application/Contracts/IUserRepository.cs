using NotifyMe.Application.Models;

namespace NotifyMe.Application.Contracts;

public interface IUserRepository
{
    Task AddUser(UserModel user);
    Task<string> LogIn(LoginModel loginModel);
}