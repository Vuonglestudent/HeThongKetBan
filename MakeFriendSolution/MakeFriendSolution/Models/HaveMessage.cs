using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class HaveMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public DateTime SentAt { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}