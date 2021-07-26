using MakeFriendSolution.Application;
using MakeFriendSolution.EF;
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
    public class NotificationsController : ControllerBase
    {

        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private readonly ISessionService _sessionService;
        private readonly IUserApplication _userApplication;
        private readonly IFeatureApplication _featureApplication;
        private readonly INotificationApplication _notificationApp;

        public NotificationsController(MakeFriendDbContext context, IStorageService storageService, ISessionService sessionService, IUserApplication userApplication, IFeatureApplication featureApplication, INotificationApplication notificationApp)
        {
            _context = context;
            _storageService = storageService;
            _sessionService = sessionService;
            _userApplication = userApplication;
            _featureApplication = featureApplication;
            _notificationApp = notificationApp;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(Guid userId, [FromQuery] PagingRequest request)
        {
            var nts = await _notificationApp.GetNotifications(new NotificationRequest()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                UserId = userId
            });

            return Ok(nts);
        }

        [Authorize]
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            var delete = await _notificationApp.DeleteNotification(notificationId);

            if (delete)
                return Ok();
            else
                return BadRequest();
        }

    }
}
