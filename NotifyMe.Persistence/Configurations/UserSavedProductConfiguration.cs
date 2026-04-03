using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotifyMe.Domain.Entities;

namespace NotifyMe.Persistence.Configurations;

public class UserSavedProductConfiguration : IEntityTypeConfiguration<SavedProduct>
{
    public void Configure(EntityTypeBuilder<SavedProduct> builder)
    {
        builder.HasOne(e => e.Shop).WithMany(e => e.UserSavedProducts).HasForeignKey(e => e.ShopId);
    }
}