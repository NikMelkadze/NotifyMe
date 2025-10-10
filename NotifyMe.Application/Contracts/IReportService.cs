using NotifyMe.Application.Models;

namespace NotifyMe.Application.Contracts;

public interface IReportService
{
    Task<IEnumerable<SavedProduct>> GetTopSavedProducts(int top, CancellationToken cancellationToken);
}