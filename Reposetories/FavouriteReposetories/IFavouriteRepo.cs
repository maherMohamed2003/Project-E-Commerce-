using E_Commerce_Proj.DTOs.Product;

namespace E_Commerce_Proj.Reposetories.FavouriteReposetories
{
    public interface IFavouriteRepo
    {
        public Task<List<DisplayProductDTO>> GetAllFavouriteProductsAsync(int userId);
        public Task<string> AddToFavouriteAsync(int userId, int productId);
        public Task<string> RemoveFromFavouriteAsync(int userId, int productId);
        public Task<string> ClearFavouriteListAsync(int userId);
    }
}
