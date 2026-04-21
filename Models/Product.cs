using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity {  get; set; }
        public string ImageURL { get; set; }
        public int Rate { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
        public ICollection<Review> reviews { get; set; }
        public ICollection<OrderItem> orderItems { get; set; }
        public ICollection<CartItem> cartItems { get; set; }
        public ICollection<FavoriteItem> favoriteItems { get; set; }

    }
}
