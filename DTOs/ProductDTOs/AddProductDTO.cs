using storeProject.Models;

namespace E_Commerce_Proj.DTOs.Product
{
    public class AddProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int Rate { get; set; }
        public decimal Discount { get; set; }
        public IFormFile Image { get; set; }

    }
}
