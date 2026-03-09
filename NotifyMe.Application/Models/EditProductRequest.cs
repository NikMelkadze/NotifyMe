using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Models;

public record EditProductRequest(ProductStatus? Status, NotificationType? NotificationType);