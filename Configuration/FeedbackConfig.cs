using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;
namespace E_Commerce_Proj.Configuration
{
    public class FeedbackConfig : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Comment)
                .IsRequired()
                .HasMaxLength(700)
                .HasColumnName("FeedBack");

            builder.HasOne(f => f.customer)
                .WithMany(c => c.feedBacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
