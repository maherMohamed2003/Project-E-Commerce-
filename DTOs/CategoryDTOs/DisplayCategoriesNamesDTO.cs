namespace E_Commerce_Proj.DTOs.CategoryDTOs
{
    public class DisplayCategoriesNamesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int numberOfProducts { get; set; }
        public decimal totalPriceOfProducts { get; set; }
    }
}
