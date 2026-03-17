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
        private readonly string[] AllowExtentions = new[] { ".jpg", ".jpeg", ".png" };
        private readonly long MaxImageSize = 5242880; // 5 MB

        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddProductAsync(AddProductDTO newProduct)
        {
            var product = new Product
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Quantity = newProduct.Quantity,
                CategoryId = newProduct.CategoryId
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            foreach (var image in newProduct.productImages)
            {
                if (image.Length < 0 || image.Length > MaxImageSize)
                    return "Image size must be less than 5 MB.";
                if (!AllowExtentions.Contains(Path.GetExtension(image.FileName).ToLower()))
                    return "Image Type Is Blocked.";


                var stream = new MemoryStream();
                image.CopyTo(stream);
                _context.ProductImages.Add(new ProductImage
                {
                    Image = stream.ToArray(),
                    ProductId = product.Id
                });

            }
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
                Images = x.productImages.Select(i => i.Image).ToList()
            }).ToListAsync();

            return products;
        }

        public async Task<DisplayProductDTO> GetOneProductAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new DisplayProductDTO
                {
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
                    Images = p.productImages.Select(i => i.Image).ToList()
                })
                .FirstOrDefaultAsync();
            if (product == null)
                return null;
            return product;
        }

        public async Task<string> UpdateProductAsync(int id, UpdateProductDTO updatedProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if(product == null)
                return "Product not found.";
            product.Name = updatedProduct.Name; 
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;
            await _context.SaveChangesAsync();
            return "Product updated successfully.";
        }
    }
}
