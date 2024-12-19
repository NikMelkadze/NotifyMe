using NotifyMe.Domain.Entities;

namespace NotifyMe.Application.Contracts;

public interface IUserService
{
    public Task SaveProduct(string url);
}