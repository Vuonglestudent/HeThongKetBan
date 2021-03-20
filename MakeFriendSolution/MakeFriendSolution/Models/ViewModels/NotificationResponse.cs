using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public Guid FromId { get; set; }
        public string FullName { get; set; }
        public Guid ToId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        //public bool HasAvatar { get; set; }
        public string Avatar { get; set; }
    }
}
