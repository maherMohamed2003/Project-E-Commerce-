using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;

namespace E_Commerce_Proj.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(o => o.OrderStatus).HasMaxLength(50).IsRequired();
            builder.Property(o => o.Address).HasMaxLength(200).IsRequired();

            // Relationships
            builder.HasMany(o => o.orderItems)
                        .WithOne(oi => oi.order)
                        .HasForeignKey(oi => oi.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.shipping)
                        .WithOne(s => s.order)
                        .HasForeignKey<Shipping>(s => s.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
