using E_Commerce_Proj.DTOs.CategoryDTOs;

namespace E_Commerce_Proj.Reposetories.CategoryReposetories
{
    public interface ICategoryRepo
    {
        public Task<string> AddCategoryAsync(AddCategoryDTO name);
        public Task<DisplayCategoryDTO> GetOneCategoryProductsAsync(int id);
        public Task<List<DisplayCategoryDTO>> GetAllCategoriesProductsAsync();
    }
}
