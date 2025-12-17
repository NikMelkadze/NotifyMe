using System.ComponentModel.DataAnnotations;

namespace NotifyMe.Application.Models;

public record UserModel(
    [Required, EmailAddress] string Email,
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter and one special character.")]
    [Required]
    string Password,
    [Required]
    string ConfirmPassword,
    [Required] string FirstName,
    [Required] string LastName,
    [Required, Phone, StringLength(9, MinimumLength = 9, ErrorMessage = "Phone number must be exactly 9 digits.")]
    string PhoneNumber);

public record LoginModel(string EmailOrPhoneNumber, string Password);