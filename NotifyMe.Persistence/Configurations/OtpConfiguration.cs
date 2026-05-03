using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotifyMe.Domain.Entities;

namespace NotifyMe.Persistence.Configurations;

public class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder.HasOne(e => e.User).WithMany(u => u.Otps).HasForeignKey(f => f.UserId).IsRequired();
    }
}