namespace storeProject.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
        public int CartId { get; set; }
        public Cart cart { get; set; }
    }
}
