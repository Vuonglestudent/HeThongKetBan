using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
