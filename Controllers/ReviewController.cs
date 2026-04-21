using E_Commerce_Proj.DTOs.ReviewDTOs;
using E_Commerce_Proj.Reposetories.ReviewReposetories;
using Microsoft.AspNetCore.Mvc;
using storeProject.Models;

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
        [Route("AddReview/")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDTO rev)
        {
            var result = await _repo.AddReviewAsync(rev.UserId, rev.ProductId, rev);
            if (result != "Review added successfully")
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateReview/")]
        public async Task<IActionResult> UpdateReview( [FromBody] UpdateReviewDTO rev)
        {
            var result = await _repo.UpdateReviewAsync(rev.UserId, rev.ReviewId, rev);
            if (result == "Review updated successfully")
            {
                return Ok(result);
            }
            else
                return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteReview/")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var result = await _repo.DeleteReviewAsync(reviewId);
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
