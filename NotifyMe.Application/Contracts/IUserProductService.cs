namespace NotifyMe.Application.Contracts;

public interface IUserProductService
{
    public Task SaveProduct(string url,int userId);
}