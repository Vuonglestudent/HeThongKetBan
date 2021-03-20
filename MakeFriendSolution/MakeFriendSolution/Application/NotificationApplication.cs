using MakeFriendSolution.EF;
using MakeFriendSolution.HubConfig;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public class NotificationApplication:INotificationApplication
    {
        private readonly IHubContext<ChatHub> _hub;
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;

        public NotificationApplication(IHubContext<ChatHub> hub, MakeFriendDbContext context, IStorageService storageService)
        {
            _hub = hub;
            _context = context;
            _storageService = storageService;
        }

        public async Task<NotificationResponse> CreateNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            try
            {
               await _context.SaveChangesAsync();
                var response = await GetNotificationResponse(notification);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<NotificationResponse> GetNotificationResponse(Notification notification)
        {
            var r = new NotificationResponse();

            var imagePath = await (from i in _context.Users
                                   where i.Id == notification.FromId
                                   select new AvatarResponse()
                                   {
                                       AvatarPath = i.AvatarPath,
                                       FullName = i.FullName,
                                       UserId = i.Id
                                   }).FirstOrDefaultAsync();

            //try
            //{
            //    var a = _storageService.GetFileUrl(imagePath.AvatarPath);
            //    byte[] imageBits = System.IO.File.ReadAllBytes($"./{a}");
            //    r.Avatar = Convert.ToBase64String(imageBits);
            //    r.HasAvatar = true;
            //}
            //catch
            //{
            //    r.HasAvatar = false;
            //    r.Avatar = imagePath.AvatarPath;
            //}
            r.Avatar = _storageService.GetFileUrl(imagePath.AvatarPath);
            r.FromId = notification.FromId;
            r.ToId = notification.ToId;
            r.Id = notification.Id;
            r.FullName = imagePath.FullName;
            r.CreatedAt = notification.CreatedAt;
            r.Type = "notification";

            if(notification.Type == "follow")
            {
                r.Content =" đã theo dõi bạn.";
            }
            else if(notification.Type == "likeImage")
            {
                r.Content = " đã thích hình ảnh của bạn.";
            }
            else if(notification.Type == "like")
            {
                r.Content = " đã bày tỏ cảm xúc với bạn.";
            }
            
            return r;
        }

        public async Task<bool> DeleteNotification(int id)
        {
            var notice = await _context.Notifications.FindAsync(id);
            if (notice == null)
                return false;
            try
            {
                _context.Notifications.Remove(notice);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<NotificationResponse>> GetNotifications(NotificationRequest request)
        {
            var nts = await _context.Notifications.Where(x => x.ToId == request.UserId).OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var avatarPaths = new List<AvatarResponse>();

            var response = new List<NotificationResponse>();

            foreach (var item in nts)
            {
                var r = await GetNotificationResponse(item);
                response.Add(r);
            }

            return response;
        }


        public async Task SendNotification(NotificationResponse notification)
        {
            var connectionId = await(from c in _context.Users
                                     where c.Id == notification.ToId
                                     select c.ConnectionId).FirstOrDefaultAsync();


            await _hub.Clients.Clients(connectionId).SendAsync("transferData", notification);
        }

        private class AvatarResponse
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public string AvatarPath { get; set; }
        }
    }
}
