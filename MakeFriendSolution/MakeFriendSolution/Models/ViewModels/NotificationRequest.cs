using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class NotificationRequest:PagingRequest
    {
        public Guid UserId { get; set; }
    }
}
