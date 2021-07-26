using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class FindAroundRequest : PagingRequest
    {
        public Guid UserId { get; set; }
        public int Gender { get; set; }
        public int AgeGroup { get; set; }
        public int Distance { get; set; }
    }
}