using MakeFriendSolution.Application;
using MakeFriendSolution.Models;
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
    public class RelationshipsController : ControllerBase
    {
        private readonly IRelationshipApplication _relationshipApp;
        private readonly INotificationApplication _notificationApp;
        private readonly ISessionService _sessionService;
        public RelationshipsController(IRelationshipApplication relationshipApp, 
            INotificationApplication notificationApp, ISessionService sessionService)
        {
            _relationshipApp = relationshipApp;
            _notificationApp = notificationApp;
            _sessionService = sessionService;   

        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetById([FromQuery] GetRelationshipRequest request)
        {
            var user = _sessionService.GetDataFromToken();
            try
            {
                var relationship = await _relationshipApp.GetById(request.FromId, request.ToId);
                return Ok(relationship);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]RelationshipRequest request)
        {
            try
            {
                var relationship = await _relationshipApp.Create(request);
                var notification = new Notification()
                {
                    CreatedAt = DateTime.Now,
                    FromId = request.FromId,
                    ToId = request.ToId,
                    Type = "relationship"
                };

                var notificationResponse = await _notificationApp.CreateNotification(notification);

                try
                {
                    await _notificationApp.SendNotification(notificationResponse);
                }
                catch (Exception)
                {

                }
                return Ok(relationship);
            }
            catch (Exception e)
            {

                return BadRequest(new
                {
                    Message = e.Message
                });
            }
        }

        [HttpPost("accept/{id}")]
        [Authorize]
        public async Task<IActionResult> Accept(int id)
        {
            try
            {
                await _relationshipApp.Accept(id);
                return Ok(new { Message = "Successful" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("decline/{id}")]
        [Authorize]
        public async Task<IActionResult> Decline(int id)
        {
            try
            {
                await _relationshipApp.Decline(id);
                return Ok(new { Message = "Successful" });

            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRelationship(Guid userId)
        {
            try
            {
                await _relationshipApp.Delete(userId);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(new
                {
                    Message = e.Message
                });
            }
        }
    }

    public class GetRelationshipRequest
    {
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }

    }
}
