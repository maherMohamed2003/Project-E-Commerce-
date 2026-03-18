using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;

namespace E_Commerce_Proj.Configuration
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.FName).IsRequired().HasMaxLength(50).HasColumnName("FirstName");
                builder.Property(c => c.LName).IsRequired().HasMaxLength(50).HasColumnName("LastName");
                builder.Property(c => c.Email).IsRequired().HasMaxLength(50);
                builder.Property(c => c.Password).IsRequired().HasMaxLength(300);

            // Relationships
            builder.HasMany(c => c.roles)
                .WithOne(r => r.customer)
                .HasForeignKey(x => x.customerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.orders)
                .WithOne(o => o.customer)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.HasMany(c => c.reviews)
                .WithOne(c => c.customer)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
