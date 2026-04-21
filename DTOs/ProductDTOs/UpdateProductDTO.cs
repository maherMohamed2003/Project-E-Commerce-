namespace E_Commerce_Proj.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Rate { get; set; }
        public decimal Discount { get; set; }
    }
}
