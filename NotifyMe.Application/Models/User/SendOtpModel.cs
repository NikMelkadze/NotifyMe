using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Models.User;

public class SendOtpModel
{
    public required string Email { get; set; } = null!;
    public required OtpType Type { get; set; }
    public required OtpOperationType OperationType { get; set; }
}