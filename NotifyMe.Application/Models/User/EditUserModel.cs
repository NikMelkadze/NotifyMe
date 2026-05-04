using System.ComponentModel.DataAnnotations;

namespace NotifyMe.Application.Models.User;

public class EditUserModel
{
    [Phone, StringLength(9, MinimumLength = 9, ErrorMessage = "Phone number must be exactly 9 digits.")]
    public string? PhoneNumber { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}