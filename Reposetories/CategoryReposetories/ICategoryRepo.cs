using E_Commerce_Proj.DTOs.CategoryDTOs;
using E_Commerce_Proj.DTOs.Product;

namespace E_Commerce_Proj.Reposetories.CategoryReposetories
{
    public interface ICategoryRepo
    {
        public Task<string> AddCategoryAsync(AddCategoryDTO name);
        public Task<DisplayCategoryDTO> GetOneCategoryProductsAsync(int id);
        public Task<List<DisplayCategoryDTO>> GetAllCategoriesProductsAsync();
        public Task<DisplayCategoryDTO> UpdateCategoryAsync(int id , UpdateCategoryDTO update);
        public Task<string> DeleteCategoryAsync(int id);
    }
}
