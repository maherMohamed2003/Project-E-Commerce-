namespace E_Commerce_Proj.DTOs.ShipmentDTOs
{
    public class ShippingResponseDTO
    {
        public int id { get; set; }
        public string senderName { get; set; }
        public string senderPhone { get; set; }
        public string senderAddress { get; set; }
        public string receiverName { get; set; }
        public string receiverAddress { get; set; }
        public int receiverPhone { get; set; }
        public decimal egpAmount { get; set; }
        public string clientName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string DriverName { get; set; }
        public string nowStatus { get; set; }
    }
}
