using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.CategoryDTOs;
using E_Commerce_Proj.DTOs.Product;
using E_Commerce_Proj.DTOs.Review;
using storeProject.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Proj.Reposetories.CategoryReposetories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddCategoryAsync(AddCategoryDTO Category)
        {
            var check = await _context.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == Category.CategoryName.ToLower());
            if (check != null)
                return "This Category Is Already Exsits";
            var newCategory = new Category
            {
                Name = Category.CategoryName
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return "Category Added Successfully";
        }

        public async Task<string> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return "Category Not Found";
            var categoryProducts = await GetOneCategoryProductsAsync(category.Id);
            if(categoryProducts == null)
                return "Category Is Empty";
            foreach(var product in categoryProducts.Products)
            {
                var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.Name == product.Name);
                if (productInDb != null)
                {
                    _context.Products.Remove(productInDb);
                }
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return "Category And Its Products Deleted Successfully";
        }

        public async Task<List<DisplayCategoryDTO>> GetAllCategoriesProductsAsync()
        {
            var categories = await _context.Categories.Select(c => new DisplayCategoryDTO
            {
                CategoryName = c.Name,
                Products = c.products.Select(p => new DisplayProductDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    CategoryName = c.Name,
                    Images = p.productImages.Select(i => i.Image).ToList(),
                    Reviews = p.reviews.Select(r => new DisplayReviewDTO
                    {
                        Rating = r.Rating,
                        ReviewTaxt = r.ReviewTaxt,
                        ReviewDate = r.ReviewDate
                    }).ToList()
                }).ToList()
            }).ToListAsync();
            return categories;
        }
            
        public Task<DisplayCategoryDTO> GetOneCategoryProductsAsync(int id)
        {
            var category = _context.Categories.Where(c => c.Id == id).Select(c => new DisplayCategoryDTO
            {
                CategoryName = c.Name,
                Products = c.products.Select(p => new DisplayProductDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    CategoryName = c.Name,
                    Images = p.productImages.Select(i => i.Image).ToList(),
                    Reviews = p.reviews.Select(r => new DisplayReviewDTO
                    {
                        Rating = r.Rating,
                        ReviewTaxt = r.ReviewTaxt,
                        ReviewDate = r.ReviewDate
                    }).ToList()
                }).ToList()
            }).FirstOrDefaultAsync();
            return category;
        }

        public async Task<DisplayCategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO update)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return null;
            category.Name = update.CategoryName;
            await _context.SaveChangesAsync();
            var updatedCategory = await GetOneCategoryProductsAsync(category.Id);
            return updatedCategory;
        }

    }
}
