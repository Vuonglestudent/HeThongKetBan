using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class HaveMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public string MessageType { get; set; }
        public string FilePath { get; set; }
        public DateTime SentAt { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
        [NotMapped]
        public List<string> FilePaths { get; set; }
    }
}