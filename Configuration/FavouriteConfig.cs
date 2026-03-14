using E_Commerce_Proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce_Proj.Configuration
{
    public class FavouriteConfig : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.customer)
                .WithOne(c => c.Favourite)
                .HasForeignKey<Favourite>(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
