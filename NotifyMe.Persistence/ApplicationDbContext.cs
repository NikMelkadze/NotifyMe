using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Entities;

namespace NotifyMe.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<UserSavedProduct> UserSavedProducts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
}