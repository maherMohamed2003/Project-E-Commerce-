using System.Security.Claims;
using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.Feedback;
using Microsoft.EntityFrameworkCore;
using storeProject.Models;

namespace E_Commerce_Proj.Abstracts.Feedback
{
    public class FeedbackRepo : IFeedbackRepo
    {
        private readonly AppDbContext _context;
        public FeedbackRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddFeedbackAsync(AddFeedBackDTO add)
        {
            var newFeed = new FeedBack
            {
                Comment = add.Comment,
                FeedBackDate = DateTime.Now,
                CustomerId = add.CustomerId 
            };
            await _context.feedBacks.AddAsync(newFeed); 
            await _context.SaveChangesAsync();
            return "Feedback Sent Successfully!";
        }

        public async Task<string> DeleteFeedbackAsync(int id)
        {
            var feed = await _context.feedBacks.FirstOrDefaultAsync(x => x.Id == id);
            if (feed == null)
                return null;
            _context.feedBacks.Remove(feed);
            await _context.SaveChangesAsync();
            return "Feedback Deleted Successfully!";
        }

        public async Task<List<DisplayFeedback>> DisplayAllFeedbacksFromOneUser(int userId)
        {
            var feedbacks = await _context.feedBacks.Where(x => x.CustomerId == userId).Select(x => new DisplayFeedback
            {
                Comment = x.Comment,
                FeedBackDate = x.FeedBackDate,
                AuthorName = x.customer.FName + " " + x.customer.LName
            }).ToListAsync();
            if(feedbacks == null || feedbacks.Count == 0)
                return null;
            return feedbacks;
        }

        public async Task<List<DisplayFeedback>> GetAllFeedbacks()
        {
            var feedbacks = _context.feedBacks.Select(x => new DisplayFeedback
            {
                Comment = x.Comment,
                FeedBackDate = x.FeedBackDate,
                AuthorName = x.customer.FName + " " + x.customer.LName
            }).ToList();

            return feedbacks;
        }
    }
}
