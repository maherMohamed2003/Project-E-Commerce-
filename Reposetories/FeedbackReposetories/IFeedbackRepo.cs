using E_Commerce_Proj.DTOs.Feedback;

namespace E_Commerce_Proj.Abstracts.Feedback
{
    public interface IFeedbackRepo
    {
        public Task<string> AddFeedbackAsync(AddFeedBackDTO add);
        public Task<List<DisplayFeedback>> GetAllFeedbacks();
        public Task<string> DeleteFeedbackAsync(int id);
        public Task<List<DisplayFeedback>> DisplayAllFeedbacksFromOneUser(int userId);

    }
}
