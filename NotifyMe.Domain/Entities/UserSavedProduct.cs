using System.ComponentModel.DataAnnotations;

namespace NotifyMe.Domain.Entities;

public class UserSavedProduct 
{
    [Key]
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsActive { get; set; }
}