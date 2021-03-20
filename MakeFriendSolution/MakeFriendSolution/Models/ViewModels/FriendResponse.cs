using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class FriendResponse
    {
        public UserDisplay User { get; set; }
        public ICollection<MessageResponse> Messages { get; set; }
    }
}