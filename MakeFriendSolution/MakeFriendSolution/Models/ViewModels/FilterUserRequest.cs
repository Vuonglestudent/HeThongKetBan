using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class FilterUserRequest: PagingRequest
    {
        public string Feature { get; set; }
        public bool IsAscending { get; set; }
    }
}
