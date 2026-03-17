using E_Commerce_Proj.DTOs.ReviewDTOs;

namespace E_Commerce_Proj.Reposetories.ReviewReposetories
{
    public interface IReviewRepo
    {
        public Task<string> AddReviewAsync(int userId , int productId , AddReviewDTO rev);
        public Task<string> UpdateReviewAsync(int userId , int reviewId , AddReviewDTO rev);
        public Task<string> DeleteReviewAsync(int userId , int reviewId);
    }
}
