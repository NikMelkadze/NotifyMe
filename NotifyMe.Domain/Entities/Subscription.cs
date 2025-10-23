using System.ComponentModel.DataAnnotations;

namespace NotifyMe.Domain.Entities;

public class Subscription
{
    [Key] public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
}