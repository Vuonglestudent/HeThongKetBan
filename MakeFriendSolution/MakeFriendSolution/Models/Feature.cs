using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double WeightRate { get; set; }
        public bool IsCalculated { get; set; }
        public bool IsSearchFeature { get; set; }

        public ICollection<UserFeature> UserFeatures { get; set; }
        public ICollection<SearchFeature> SearchFeatures { get; set; }
        public ICollection<FeatureDetail> FeatureDetails { get; set; }
    }
}