using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class FilterUserViewModel : PagingRequest
    {
        public bool IsFilter { get; set; }
        public string Location { get; set; }
        public string FullName { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public string Gender { get; set; }
    }
}