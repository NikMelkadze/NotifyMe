namespace NotifyMe.Domain.Entities;

public class UserSavedProducts 
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsActive { get; set; }
}