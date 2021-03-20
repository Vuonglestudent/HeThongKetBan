using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class ChatResponse
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool hasAvatar { get; set; }
        public string Avatar { get; set; }

    }
}
