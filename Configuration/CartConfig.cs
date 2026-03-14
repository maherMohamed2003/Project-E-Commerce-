using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using storeProject.Models;

namespace E_Commerce_Proj.Configuration
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);

            //Relationships
            builder.HasOne(c => c.customer)
                .WithOne(c => c.cart)
                .HasForeignKey<Cart>(x => x.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.cartItems)
                .WithOne(x => x.cart)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
