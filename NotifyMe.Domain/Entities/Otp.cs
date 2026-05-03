using System.ComponentModel.DataAnnotations;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Domain.Entities;

public class Otp
{
    [Key] public int Id { get; set; }
    public OtpOperationType OperationType { get; set; }
    public int UserId { get; set; }
    public int ActiveMinutes { get; set; }
    public OtpType Type { get; set; }
    public OtpStatus Status { get; set; }
    public string Code { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public int ValidateAttempts { get; set; }

    public User User { get; set; } = null!;
}

public enum OtpType
{
    Email,
    Sms
}
public enum OtpStatus 
{
    Valid,
    Invalid,
}