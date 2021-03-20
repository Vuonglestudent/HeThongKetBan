using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class SearchFeature
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int FeatureDetailId { get; set; }
        public int FeatureId { get; set; }
        public AppUser User { get; set; }
        public Feature Feature { get; set; }
        public FeatureDetail FeatureDetail { get; set; }
    }
}
