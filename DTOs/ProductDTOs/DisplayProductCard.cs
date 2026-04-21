namespace E_Commerce_Proj.DTOs.ProductDTOs
{
    public class DisplayProductCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
