using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;

namespace E_Commerce_Proj.Configuration
{
    public class ShippingConfig : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.ShippingCarrier)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.TrackingNumber)
                .IsRequired();
            builder.Property(s => s.ShippingStatus)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
