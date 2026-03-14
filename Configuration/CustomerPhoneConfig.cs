using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;

namespace E_Commerce_Proj.Configuration
{
    public class CustomerPhoneConfig : IEntityTypeConfiguration<CustomerPhone>
    {
        public void Configure(EntityTypeBuilder<CustomerPhone> builder)
        {
            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.PhoneNumber).IsRequired().HasMaxLength(15);


            //Relationships
            builder.HasOne(cp => cp.customer)
                   .WithMany(c => c.customerPhones)
                   .HasForeignKey(cp => cp.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
