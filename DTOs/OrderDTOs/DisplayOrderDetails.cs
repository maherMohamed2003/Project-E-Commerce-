namespace E_Commerce_Proj.DTOs.OrderDTOs
{
    public class DisplayOrderDetails
    {
        public List<DisplayOrderItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ShippingStatus { get; set; }
        public string ShippingCarrier { get; set; }
        public string TrackingNumber { get; set; }
    }
}
