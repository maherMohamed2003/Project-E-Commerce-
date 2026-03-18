using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.Product;
using E_Commerce_Proj.Reposetories.ProductReposetories;
using Microsoft.EntityFrameworkCore;
using storeProject.Models;

namespace E_Commerce_Proj.Reposetories.FavouriteReposetories
{
    public class FavouriteRepo : IFavouriteRepo
    {
        private readonly AppDbContext _context;
        private readonly IProductRepo _repo;

        public FavouriteRepo(AppDbContext context, IProductRepo repo)
        {
            _context = context;
            _repo = repo;
        }

        public async Task<string> AddToFavouriteAsync(int userId, int productId)
        {
            var user = await _context.Customers.Include(x => x.Favourite).FirstOrDefaultAsync(x => x.Id == userId);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (user == null || product == null)
                return "Something Went Wrong";
            var favouriteItem = await _context.favoriteItems.FirstOrDefaultAsync(x => x.ProductId == productId && x.Favourite.Id == user.Favourite.Id);
            if (favouriteItem != null)
                return "Product Already In Favourite List";
            var newFavouriteItem = new FavoriteItem
            {
                ProductId = productId,
                FavouriteId = user.Favourite.Id
            };
            await _context.favoriteItems.AddAsync(newFavouriteItem);
            await _context.SaveChangesAsync();
            return "Product Added To Favourite List";

        }

        public async Task<string> ClearFavouriteListAsync(int userId)
        {
            var user = await _context.Customers.Include(x => x.Favourite).ThenInclude(x => x.FavoriteItems).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return "Something Went Wrong";
            _context.favoriteItems.RemoveRange(user.Favourite.FavoriteItems);
            await _context.SaveChangesAsync();
            return "Favourite List Cleared";
        }

        public async Task<List<DisplayProductDTO>> GetAllFavouriteProductsAsync(int userId)
        {
            var user = await _context.Customers.Include(c => c.Favourite).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return null;
            var productIds = await _context.favoriteItems.Select(x => x.ProductId).ToListAsync();

            var res = new List<DisplayProductDTO>();
            foreach(var id in productIds)
            {
                var prod = await _repo.GetOneProductAsync(id);
                if (prod == null) continue;
                res.Add(prod);
            }
            return res;
        }

        public async Task<string> RemoveFromFavouriteAsync(int userId, int productId)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(x => x.Id == userId);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (user == null || product == null)
                return "Something Went Wrong";
            var favouriteItem = await _context.favoriteItems.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (favouriteItem == null)
                return "This Item Is Not In Your Favourite List";
            _context.favoriteItems.Remove(favouriteItem);
            await _context.SaveChangesAsync();
            return "Done";
        }
    }
}
