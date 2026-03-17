using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.ReviewDTOs;
using storeProject.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Proj.Reposetories.ReviewReposetories
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext _context;

        public ReviewRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddReviewAsync(int userId, int productId, AddReviewDTO rev)
        {
            var newReview = new Review
            {
                ProductId = productId,
                ReviewTaxt = rev.ReviewTaxt,
                Rating = rev.Rating,
                CustomerId = userId,
                ReviewDate = DateTime.Now
            };
            await _context.Reviews.AddAsync(newReview);
            await _context.SaveChangesAsync();
            return "Review added successfully";
        }

        public async Task<string> DeleteReviewAsync(int userId, int reviewId)
        {
            var rev = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId && userId == x.CustomerId);
            if (rev == null)
                return "NotFound";
            _context.Reviews.Remove(rev);
            await _context.SaveChangesAsync();
            return "Review deleted successfully";
        }

        public async Task<string> UpdateReviewAsync(int userId, int reviewId, AddReviewDTO rev)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId && userId == x.CustomerId);
            if (review == null)
                return "NotFound";
            review.ReviewTaxt = rev.ReviewTaxt;
            review.Rating = rev.Rating;
            _context.Reviews.Update(review);
            _context.SaveChanges();
            return "Review updated successfully";
        }
    }
}
