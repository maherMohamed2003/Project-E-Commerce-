namespace E_Commerce_Proj.DTOs.ReviewDTOs
{
    public class UpdateReviewDTO
    {
        public int UserId { get; set; }
        public int ReviewId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
    }
}
