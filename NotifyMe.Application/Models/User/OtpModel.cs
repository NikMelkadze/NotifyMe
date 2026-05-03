using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Application.Models.User;

public class OtpModel
{
    public string Email { get; set; } = null!;
    public OtpType Type { get; set; }
    public OtpOperationType OperationType { get; set; }
}