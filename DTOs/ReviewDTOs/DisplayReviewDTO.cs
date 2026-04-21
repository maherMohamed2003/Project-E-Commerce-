namespace E_Commerce_Proj.DTOs.Review
{
    public class DisplayReviewDTO
    {
        public int Id { get; set; }
        public string ReviewTaxt { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public string CustomerName { get; set; }
    }
}
