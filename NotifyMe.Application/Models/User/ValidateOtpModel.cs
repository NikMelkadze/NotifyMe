namespace NotifyMe.Application.Models.User;

public class ValidateOtpModel
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}