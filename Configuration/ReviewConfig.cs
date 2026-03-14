using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;

namespace E_Commerce_Proj.Configuration
{
    public class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.ReviewTaxt).IsRequired().HasMaxLength(500);
            builder.Property(r => r.Rating).IsRequired();

            
        }
    }
}
