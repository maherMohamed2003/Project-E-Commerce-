namespace E_Commerce_Proj.DTOs.CartDTOs
{
    public class DisplayCartItemDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string image { get; set; }
        public int Quentity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }


    }
}
