using System.ComponentModel.DataAnnotations;
using AngleSharp;
using AngleSharp.Dom;
using NotifyMe.Infrastructure.Contracts;
using NotifyMe.Infrastructure.Models;

namespace NotifyMe.Infrastructure.Services.Common;

public abstract class ShopHandlerBase(IHttpClientService httpClientService, IBrowsingContext? browsingContext)
{
    protected string Price { get; set; } = null!;
    protected string? DiscountedPrice { get; set; }

    public abstract Task<ProductInformation> GetProductInformation(string url, CancellationToken cancellationToken);

    protected static string GetProductName(IDocument content)
    {
        var element = content.QuerySelector("meta[property='og:title']");
        return element?.GetAttribute("content") ?? throw new ValidationException("Wrong Domain");
    }

    protected async Task<IDocument> GetDocument(string url, CancellationToken cancellationToken)
    {
        var html = await httpClientService.GetHtml(url, cancellationToken);
        return await browsingContext.OpenAsync(req => req.Content(html), cancellationToken);
    }
}