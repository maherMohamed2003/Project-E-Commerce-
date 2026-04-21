using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.Product;
using E_Commerce_Proj.DTOs.ProductDTOs;
using E_Commerce_Proj.DTOs.Review;
using Microsoft.EntityFrameworkCore;
using storeProject.Models;

namespace E_Commerce_Proj.Reposetories.ProductReposetories
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductRepo(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> AddProductAsync(AddProductDTO newProduct)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == newProduct.CategoryId);

            if (category == null)
                return "Category Not Found";

            string imageUrl = null;

            // upload image if exists
            if (newProduct.Image != null && newProduct.Image.Length > 0)
            {
                string folderPath = Path.Combine(
                    _env.WebRootPath,
                    "images",
                    "products"
                );

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() +
                               Path.GetExtension(newProduct.Image.FileName);

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newProduct.Image.CopyToAsync(stream);
                }

                //imageUrl = $"/images/products/{fileName}";

                var request = _httpContextAccessor.HttpContext.Request;

                imageUrl = $"{request.Scheme}://{request.Host}/images/products/{fileName}";
            }

            var product = new Product
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Quantity = newProduct.Quantity,
                CategoryId = newProduct.CategoryId,
                ImageURL = imageUrl,
                Rate = newProduct.Rate,
                Discount = newProduct.Discount
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return "Product added successfully";
        }

        public async Task<string> DeleteProductAsync(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
                return "Product Not Found";
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return "Product Deleted Successfully";
        }

        public Task<List<DisplayProductDTO>> GetAllProductsAsync()
        {
            var products = _context.Products.Select(x => new DisplayProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                CategoryName = x.category.Name,
                Reviews = x.reviews.Select(r => new DisplayReviewDTO
                {
                    ReviewTaxt = r.ReviewTaxt,
                    Rating = r.Rating,
                    ReviewDate = r.ReviewDate,
                    CustomerName = r.customer.FName + " " + r.customer.LName
                }).ToList(),
                Image = x.ImageURL,
                Discount = x.Discount,
                Rate = x.Rate
            }).ToListAsync();

            return products;
        }

        public async Task<ProductsOverview> GetAllProductsOverViewAsync()
        {
            var counts = await _context.Products.CountAsync();
            var TotalValue = await _context.Products.SumAsync(p => p.Price * p.Quantity);
            var unSafeQuantity = await _context.Products.Where(p => p.Quantity < 10).CountAsync();

            var res = new ProductsOverview
            {
                CountOfProducts = counts,
                TotalValueOfProducts = TotalValue,
                NumberOfProductsThatUnderSafeQuantity = unSafeQuantity
            };

            return res;
        }

        public async Task<DisplayProductDTO> GetOneProductAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new DisplayProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    CategoryName = p.category.Name,
                    Reviews = p.reviews.Select(r => new DisplayReviewDTO
                    {
                        ReviewTaxt = r.ReviewTaxt,
                        Rating = r.Rating,
                        ReviewDate = r.ReviewDate,
                        CustomerName = r.customer.FName + " " + r.customer.LName
                    }).ToList(),
                    Image = p.ImageURL,

                    Discount = p.Discount,
                    Rate = p.Rate
                })
                .FirstOrDefaultAsync();
            if (product == null)
                return null;
            return product;
        }

        public async Task<List<DisplayProductCard>> GetProductSliderCategory(int CategoryId)
        {
            var products = await _context.Products.Where(c => c.CategoryId == CategoryId).Select(x => new DisplayProductCard {
                Id = x.Id,
                Name = x.Name,
                Discount = x.Discount,
                Rate =x.Rate,
                Price = x.Price,
                ImageUrl = x.ImageURL
            }).ToListAsync();
            List<DisplayProductCard> result;
            if (products.Count < 10)
            {
                result = products.GetRange(0, products.Count());
            }
            else
            {
                result = products.GetRange(0, 10);
            }
            return result;
        }

        public async Task<List<DisplayProductDTO>> SearchAboutProductAsync(string name)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(name))
                .Select(x => new DisplayProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    CategoryName = x.category.Name,
                    Reviews = x.reviews.Select(r => new DisplayReviewDTO
                    {
                        ReviewTaxt = r.ReviewTaxt,
                        Rating = r.Rating,
                        ReviewDate = r.ReviewDate,
                        CustomerName = r.customer.FName + " " + r.customer.LName
                    }).ToList(),
                    Image = x.ImageURL,
                    Discount = x.Discount,
                    Rate = x.Rate
                }).ToListAsync();
            return products;
        }

        public async Task<string> UpdateProductAsync(int id, UpdateProductDTO updatedProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return "Product not found.";
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;
            product.Rate = updatedProduct.Rate;
            product.Discount = updatedProduct.Discount;
            await _context.SaveChangesAsync();
            return "Product updated successfully.";
        }
    }
}
