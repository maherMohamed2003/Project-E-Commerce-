namespace E_Commerce_Proj.DTOs.OrderDTOs
{
    public class DisplayOrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }

    }
}
