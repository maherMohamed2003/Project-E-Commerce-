namespace storeProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string Address { get; set; }
        public Shipping shipping { get; set; }
        public int? CustomerId { get; set; }
        public Customer? customer { get; set; }
        public virtual ICollection<OrderItem> orderItems { get; set; }
    }
}
