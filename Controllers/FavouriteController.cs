using E_Commerce_Proj.DTOs.FavouriteDTOs;
using E_Commerce_Proj.Reposetories.FavouriteReposetories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using storeProject.Models;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteRepo _repo;

        public FavouriteController(IFavouriteRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GatFavouriteItems/{userId}")]
        public async Task<IActionResult> GetAllFavouriteList( int userId)
        {
            var res = await _repo.GetAllFavouriteProductsAsync(userId);
            if (res == null)
                return NotFound("Empty List");
            return Ok(res);
        }

        [HttpPost]
        [Route("AddItemToFavourite")]
        public async Task<IActionResult> AddItemToFavouriteList([FromBody] AddItemToFavouriteList dto)
        {
            var res = await _repo.AddToFavouriteAsync(dto.userId, dto.productId);
            if (res != "Product Added To Favourite List")
                return NotFound("Something Went Wrong");
            return Ok(res);
        }

        [HttpDelete]
        [Route("DeleteItemFromFavouriteList")]
        public async Task<IActionResult> RemoveItem([FromBody] AddItemToFavouriteList dto)
        {
            var res = await _repo.RemoveFromFavouriteAsync(dto.userId, dto.productId);
            if (res != "Done")
                return NotFound("Something Went Wrong");
            return Ok(res);
        }

        [HttpDelete]
        [Route("ClearFavouriteList/{userId}")]
        public async Task<IActionResult> ClearList(int userId)
        {
            var res = await _repo.ClearFavouriteListAsync(userId);
            if (res != "Favourite List Cleared")
                return NotFound("Something Went Wrong");
            return Ok(res);
        }
    }
}
