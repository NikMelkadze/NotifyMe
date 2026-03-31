using System.ComponentModel.DataAnnotations;
using NotifyMe.Domain.Enums;

namespace NotifyMe.Domain.Entities;

public class Shop
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ShopCategory Category { get; set; }
    
    public ICollection<SavedProduct> UserSavedProducts { get; set; } = null!;
} 