using System.ComponentModel.DataAnnotations;

namespace NotifyMe.Application.Models.User;

public class UpdatePasswordModel
{
    public required string OldPassword { get; set; } = null!;

    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter and one special character.")]
    public string Password { get; set; } = null!;

    public string ConfirmPassword { get; set; } = null!;
}