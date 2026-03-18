using E_Commerce_Proj.DTOs.ReviewDTOs;
using E_Commerce_Proj.Reposetories.ReviewReposetories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepo _repo;

        public ReviewController( IReviewRepo repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("AddReview/{userId}/{reviewId}/")]
        public async Task<IActionResult> AddReview(int userId, int productId, [FromBody] AddReviewDTO rev)
        {
            var result = await _repo.AddReviewAsync(userId, productId, rev);
            if (result == "Review added successfully")
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateReview/{userId}/{reviewId}/")]
        public async Task<IActionResult> UpdateReview( int userId, int reviewId, [FromBody] AddReviewDTO rev)
        {
            var result = await _repo.UpdateReviewAsync(userId, reviewId, rev);
            if (result == "Review updated successfully")
            {
                return Ok(result);
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteReview/{userId}/")]
        public async Task<IActionResult> DeleteReview(int userId, [FromBody] int reviewId)
        {
            var result = await _repo.DeleteReviewAsync(userId, reviewId);
            if (result == "Review deleted successfully")
            {
                return Ok(result);
            }
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("GetAllReviewsByProductId/{productId}")]
        public async Task<IActionResult> GetAllReviewsByProductId( int productId)
        {
            var result = await _repo.GetAllReviewsByProductIdAsync(productId);
            if (result == null)
            {
                return NotFound("The Product Not Found");
            }
            return Ok(result);
        }
        }
}
