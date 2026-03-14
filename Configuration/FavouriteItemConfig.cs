using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;

namespace E_Commerce_Proj.Configuration
{
    public class FavouriteItemConfig : IEntityTypeConfiguration<FavoriteItem>
    {
        public void Configure(EntityTypeBuilder<FavoriteItem> builder)
        {
            builder.HasKey(f => f.Id);


            builder.HasOne(f => f.Favourite)
                .WithMany(u => u.FavoriteItems)
                .HasForeignKey(f => f.FavouriteId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
