using MakeFriendSolution.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class UserCalculateVM
    {
        public Guid UserId { get; set; }
        public int Age { get; set; }
        public EGender Gender { get; set; }
        public double Point { get; set; } = 0;
        public List<FeatureViewModel> FeatureViewModels { get; set; }
    }
}
