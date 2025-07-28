using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Models;

public record EditProductRequest(bool IsActive,NotificationType NotificationType);