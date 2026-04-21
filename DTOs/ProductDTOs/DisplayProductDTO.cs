using E_Commerce_Proj.DTOs.Review;
using storeProject.Models;

namespace E_Commerce_Proj.DTOs.Product
{
    public class DisplayProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public int Rate { get; set; }
        public decimal Discount { get; set; }
        public List<DisplayReviewDTO> Reviews { get; set; }

    }
}
