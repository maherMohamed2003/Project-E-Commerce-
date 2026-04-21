using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.CartDTOs;
using Microsoft.EntityFrameworkCore;
using storeProject.Models;

namespace E_Commerce_Proj.Reposetories.CartReposetories
{
    public class CartRepo : ICartRepo
    {
        private readonly AppDbContext _context;

        public CartRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddItemToCartAsync(AddCartItemDTO itemDTO)
        {
            var nowUser = await _context.Customers.Include(x => x.cart).FirstOrDefaultAsync(x => x.Id == itemDTO.UserId);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == itemDTO.ProductId);
            if (nowUser == null || product == null)
                return "Something Went Wrong";

            if(product.Quantity < itemDTO.Quantity)
                return $"Only {product.Quantity} Items Available In Stock";


            var checkFound = await _context.CartItems.FirstOrDefaultAsync(x => x.CartId == nowUser.cart.Id && x.ProductId == itemDTO.ProductId);
            if(checkFound != null)
            {
                checkFound.Quantity += itemDTO.Quantity;
                await _context.SaveChangesAsync();
                return "Item Quantity Updated Successfully";
            }
            var cartItem = new CartItem
            {
                Quantity = itemDTO.Quantity,
                ProductId = itemDTO.ProductId,
                CartId = nowUser.cart.Id
            };
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return "Item Added Successfully";
        }

        public async Task<string> ClearCartAsync(int userId)
        {
            var nowUser = await _context.Customers.Include(x => x.cart).FirstOrDefaultAsync(x => x.Id == userId);
            if (nowUser == null)
                return "Something Went Wrong";
            var cartItems = _context.CartItems.Where(x => x.CartId == nowUser.cart.Id);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return "Cart Cleared Successfully";
        }

        public Task<List<DisplayCartItemDTO>> GetCartItemsPerUserAsync(int id)
        {
            var cartItems = _context.CartItems.Include(x => x.product).Where(x => x.cart.CustomerId == id).Select(x => new DisplayCartItemDTO
                {
                    Id = x.Id,
                    ProductName = x.product.Name,
                    Quentity = x.Quantity
                }).ToListAsync();
                return cartItems;
        }

        public async Task<string> RemoveItemFromCartAsync(int userId, int productId)
        {
            var nowUser = await _context.Customers.Include(x => x.cart).FirstOrDefaultAsync(x => x.Id == userId);
            if(nowUser == null)
                return "Something Went Wrong";
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.CartId == nowUser.cart.Id && x.ProductId == productId);
            if(cartItem == null)
                return "Item Not Found In Cart";
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return "Item Removed Successfully";
        }

        public async Task<string> UpdateItemQuentityAsync(AddCartItemDTO itemDTO)
        {
            var nowUser = await _context.Customers.Include(x => x.cart).FirstOrDefaultAsync(x => x.Id == itemDTO.UserId);
            if (nowUser == null)
                return "Something Went Wrong";
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.CartId == nowUser.cart.Id && x.ProductId == itemDTO.ProductId);
            if(cartItem == null)
                return "Item Not Found In Cart";
            cartItem.Quantity = itemDTO.Quantity;
            _context.SaveChanges();
            return "Item Quantity Updated Successfully";
        }
    }
}
