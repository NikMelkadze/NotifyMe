using System.ComponentModel.DataAnnotations;

namespace NotifyMe.Application.Models;

public record UserModel(
    [Required, EmailAddress] string Email,
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
    [Required]
    string Password,
    [Required] string FirstName,
    [Required] string LastName,
    [Required, Phone, StringLength(9)] string PhoneNumber);

public record LoginModel(string EmailOrPhoneNumber, string Password);