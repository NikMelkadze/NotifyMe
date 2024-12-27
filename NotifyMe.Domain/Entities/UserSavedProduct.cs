using System.ComponentModel.DataAnnotations;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Domain.Entities;

public class UserSavedProduct 
{
    [Key]
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsActive { get; set; }
    public int UserId { get; set; }
    public Shops Shop { get; set; }
}