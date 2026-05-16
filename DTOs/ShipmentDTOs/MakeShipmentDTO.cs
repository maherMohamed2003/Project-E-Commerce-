namespace E_Commerce_Proj.DTOs.ShipmentDTOs
{
    public class MakeShipmentDTO
    {
            public string senderName { get; set; }
            public string senderPhone { get; set; }
            public string senderAddress { get; set; }
            public string receiverName { get; set; }
            public string receiverAddress { get; set; }
            public string receiverPhone { get; set; }
            public decimal egpAmount { get; set; }
            public int clientID { get; set; }
        

    }
}
