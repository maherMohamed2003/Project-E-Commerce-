using E_Commerce_Proj.DTOs.Product;

namespace E_Commerce_Proj.DTOs.CategoryDTOs
{
    public class DisplayCategoryDTO
    {
        public string CategoryName { get; set; }

        public List<DisplayProductDTO> Products { get; set; }
    }
}
