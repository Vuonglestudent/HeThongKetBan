using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class GrowthRate
    {
        public int thisMonth { get; set; }
        public int lastMonth { get; set; }
        public double growthRate { get; set; }
    }
}
