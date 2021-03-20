using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class FeatureViewModel
    {
        public int FeatureId { get; set; }
        public int FeatureDetailId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsCalculated { get; set; }
        public bool IsSearchFeature { get; set; }
        public int weight { get; set; }
        public double Rate { get; set; }
    }
}
