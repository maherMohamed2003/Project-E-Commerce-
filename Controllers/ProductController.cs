using E_Commerce_Proj.DTOs.Product;
using E_Commerce_Proj.DTOs.ProductDTOs;
using E_Commerce_Proj.Reposetories.ProductReposetories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _repo;

        public ProductController(IProductRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repo.GetOneProductAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet]
        [Route("GetAllProduct/")]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _repo.GetAllProductsAsync();
            if (products == null || products.Count == 0)
                return NotFound();
            return Ok(products);
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductDTO newProduct)
        {
            var result = await _repo.AddProductAsync(newProduct);
            if (result == "Product added successfully")
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct( int id, UpdateProductDTO updatedProduct)
        {
            var result = await _repo.UpdateProductAsync(id, updatedProduct);
            if (result == "Product updated successfully.")
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _repo.DeleteProductAsync(id);
            if (result == "Product Deleted Successfully")
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        [Route("SearchProduct/{name}")]
        public async Task<IActionResult> SearchProduct(string name)
        {
            var products = await _repo.SearchAboutProductAsync(name);
            if (products == null || products.Count == 0)
                return NotFound("No Results Found!");
            return Ok(products);
        }

        }
}
