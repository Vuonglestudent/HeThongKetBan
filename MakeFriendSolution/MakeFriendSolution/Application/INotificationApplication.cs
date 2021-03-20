using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public interface INotificationApplication
    {
        Task<NotificationResponse> CreateNotification(Notification notification);
        Task<bool> DeleteNotification(int id);
        Task SendNotification(NotificationResponse notification);
        Task<List<NotificationResponse>> GetNotifications(NotificationRequest request);
    }
}
