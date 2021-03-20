using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class BlockUser
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsLock { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }

        public AppUser FromUser { get; set; }
        public AppUser ToUser { get; set; }
    }
}