namespace NotifyMe.Application.Models.User;

public class RecoveryPasswordModel
{
    public required string Email { get; set; } = null!;
    public required string Code { get; set; } = null!;
    public required string Password { get; set; } = null!;
    public required string ConfirmPassword  { get; set; } = null!;
}