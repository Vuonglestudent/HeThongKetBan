using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class FeatureResponse
    {
        public int FeatureId { get; set; }
        public int FeatureDetailId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsSearchFeature { get; set; }
    }
}
