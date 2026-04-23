using System.ComponentModel.DataAnnotations;

namespace NotifyMe.Application.Models.User;

public class EditUserModel
{
    [Phone, StringLength(9, MinimumLength = 9, ErrorMessage = "Phone number must be exactly 9 digits.")]
    public string? PhoneNumber { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter and one special character.")]
    public string? Password { get; set; }

    [EmailAddress] public string? Email { get; set; }
}