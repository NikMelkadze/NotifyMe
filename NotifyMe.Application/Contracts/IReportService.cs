using NotifyMe.Application.Models;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Contracts;

public interface IReportService
{
    Task<IEnumerable<SavedProduct>> GetTopSavedProducts(int top, CancellationToken cancellationToken);
    Task<IEnumerable<SavedProduct>> GetCompanyTopSavedProducts(int top, string shop, CancellationToken cancellationToken);
}