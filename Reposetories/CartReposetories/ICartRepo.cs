using E_Commerce_Proj.DTOs.CartDTOs;

namespace E_Commerce_Proj.Reposetories.CartReposetories
{
    public interface ICartRepo
    {
        public Task<string> AddItemToCartAsync(AddCartItemDTO itemDTO);
        public Task<List<DisplayCartItemDTO>> GetCartItemsPerUserAsync(int id);
        public Task<string> ClearCartAsync(int userId);
        public Task<string> RemoveItemFromCartAsync(int userId , int productId);

        public Task<string> UpdateItemQuentityAsync(AddCartItemDTO itemDTO);
    }
}
