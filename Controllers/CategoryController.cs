using E_Commerce_Proj.DTOs.CategoryDTOs;
using E_Commerce_Proj.Reposetories.CategoryReposetories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repo;

        public CategoryController( ICategoryRepo repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("AddCategory/")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO newCategory)
        {
            var result = await _repo.AddCategoryAsync(newCategory);
            if (result == "Category Added Successfully")
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        [Route("GetOneCategory/{id}")]
        public async Task<IActionResult> GetOneCategory( int id)
        {
            var category = await _repo.GetOneCategoryProductsAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpGet]
        [Route("GetAllCategories/")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _repo.GetAllCategoriesProductsAsync();
            if (categories == null || categories.Count == 0)
                return NotFound();
            return Ok(categories);
        }

        [HttpPut]
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO update)
        {
            var category = await _repo.UpdateCategoryAsync(id, update);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _repo.DeleteCategoryAsync(id);
            if (result == "Category Not Found" || result == "Category Is Empty")
                return NotFound(result);
            return Ok(result);
        }


        }
}
