using MakeFriendSolution.Application;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackApplication _feedbackApp;
        private readonly ISessionService _sessionService;
        public FeedbacksController(IFeedbackApplication feedbackApp, ISessionService sessionService)
        {
            _feedbackApp = feedbackApp;
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFeedbacks([FromQuery]PagingRequest request)
        {
            try
            {
                var feedbacks = await _feedbackApp.Get(request);
                return Ok(feedbacks);
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = e.Message
                });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFeedback([FromBody]FeedbackRequest request)
        {
            try
            {
                var feedback = await _feedbackApp.Create(request);
                return Ok(feedback);

            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpDelete("{feedbackId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            var user = _sessionService.GetDataFromToken();
            try
            {
                await _feedbackApp.Delete(user.UserId, feedbackId);
                return Ok(new { Message = "successful" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPut("{feedbackId}")]
        [Authorize]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, [FromBody] FeedbackRequest request)
        {
            try
            {
                var feedback = await _feedbackApp.Update(request);
                return Ok(feedback);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}
