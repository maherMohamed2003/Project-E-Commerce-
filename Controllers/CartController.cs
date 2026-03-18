using E_Commerce_Proj.DTOs.CartDTOs;
using E_Commerce_Proj.Reposetories.CartReposetories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepo _repo;

        public CartController(ICartRepo repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("AddItemToCart")]
        public async Task<IActionResult> AddItemToCart(AddCartItemDTO itemDTO)
        {
            var result = await _repo.AddItemToCartAsync(itemDTO);
            if (result == "Something Went Wrong")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetCartItemsPerUser/{id}/")]
        public async Task<IActionResult> GetCartItemsPerUser(int id)
        {
            var result = await _repo.GetCartItemsPerUserAsync(id);
            return Ok(result);

        }

        [HttpDelete]
        [Route("ClearCart/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var result = await _repo.ClearCartAsync(userId);
            if (result == "Something Went Wrong")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete]
        [Route("RemoveItemFromCart/")]
        public async Task<IActionResult> RemoveItemFromCart(RemoveItemFromCartDTO dto)
        {
            var result = await _repo.RemoveItemFromCartAsync(dto.userId, dto.productId);
            if (result == "Something Went Wrong")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateItemQuentity")]
        public async Task<IActionResult> UpdateItemQuentity(AddCartItemDTO itemDTO)
        {
            var result = await _repo.UpdateItemQuentityAsync(itemDTO);
            if (result == "Something Went Wrong")
                return BadRequest(result);
            return Ok(result);
        }

        }
}