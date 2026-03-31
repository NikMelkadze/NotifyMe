using Microsoft.EntityFrameworkCore;
using NotifyMe.Domain.Entities;
using NotifyMe.Domain.Enums;
using NotifyMe.Persistence.Configurations;
using Shop = NotifyMe.Domain.Entities.Shop;

namespace NotifyMe.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<SavedProduct> SavedProducts { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Shop> Shop { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserSavedProductConfiguration).Assembly);

        modelBuilder.Entity<Shop>().HasData(new List<Shop>()
            {
                new()
                {
                    Id = 1,
                    Name = "Megatechnica",
                    Category = ShopCategory.Technic,
                },
                new()
                {
                    Id = 2,
                    Name = "Itworks",
                    Category = ShopCategory.Technic,
                },
                new()
                {
                    Id = 3,
                    Name = "Dressup",
                    Category = ShopCategory.Fashion,
                },
                new()
                {
                    Id = 4,
                    Name = "Europroduct",
                    Category = ShopCategory.Grocery,
                },
                new()
                {
                    Id = 5,
                    Name = "Agrohub",
                    Category = ShopCategory.Grocery,
                }
            }
        );
    }
}