using System.Security.Claims;
using E_Commerce_Proj.Abstracts.Feedback;
using E_Commerce_Proj.DTOs.Feedback;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepo _repo;

        public FeedbackController( IFeedbackRepo reopo)
        {
            _repo = reopo;
        }

        [HttpPost]
        [Route("newFeed/")]
        public async Task<IActionResult> AddFeed([FromBody] AddFeedBackDTO newFeedBackDTO)
        {
            var res = await _repo.AddFeedbackAsync(newFeedBackDTO);
            if (res == null)
                return BadRequest("Sorry, Try agian later");
            return Ok();
        }

        [HttpGet]
        [Route("DisplayAllFeedbacks/")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var res = await _repo.GetAllFeedbacks();
            if (res == null)
                return NotFound("Not Feedbacks Sent Yet!");
            return Ok(res);
        }

        [HttpDelete]
        [Route("DeleteFeed/{id}")]
        public async Task<IActionResult> DeleteFeed( int id)
        {
            var res = await _repo.DeleteFeedbackAsync(id);
            if (res == null)
                return NotFound("Feedback Not Found!");
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAllFeedbacksPerUser/{userId}")]
        public async Task<IActionResult> DisplayAllFeedbacksPerOneUser( int userId)
        { 
            var res = await _repo.DisplayAllFeedbacksFromOneUser(userId);
            if (res == null)
                return NotFound("No Feedbacks Sent Yet!");
            return Ok(res);
        }
    }
}
