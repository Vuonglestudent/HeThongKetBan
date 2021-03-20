using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class StartChatRequest : PagingRequest
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}