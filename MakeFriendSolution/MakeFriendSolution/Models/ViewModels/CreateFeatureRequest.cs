using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class CreateFeatureRequest
    {
        public string Name { get; set; }
        public double WeightRate { get; set; }
        public bool IsCalculated { get; set; }
        public bool IsSearchFeature { get; set; }
    }
}
