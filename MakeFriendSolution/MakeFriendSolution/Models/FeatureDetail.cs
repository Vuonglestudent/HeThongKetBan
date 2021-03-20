using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class FeatureDetail
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Weight { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
        public ICollection<UserFeature> UserFeatures { get; set; }
        public ICollection<SearchFeature> SearchFeatures { get; set; }
    }
}
