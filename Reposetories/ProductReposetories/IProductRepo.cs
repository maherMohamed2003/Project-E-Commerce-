using E_Commerce_Proj.DTOs.Product;
using E_Commerce_Proj.DTOs.ProductDTOs;

namespace E_Commerce_Proj.Reposetories.ProductReposetories
{
    public interface IProductRepo
    {
        public Task<DisplayProductDTO> GetOneProductAsync(int id);
        public Task<List<DisplayProductDTO>> GetAllProductsAsync();
        public Task<string> AddProductAsync(AddProductDTO newProduct);
        public Task<string> UpdateProductAsync(int id, UpdateProductDTO updatedProduct);
        public Task<string> DeleteProductAsync(int id);
    }
}
