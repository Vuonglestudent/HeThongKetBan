using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class UpdateFeatureDetailRequest
    {
        public int FeatureId { get; set; }
        public string Content { get; set; }
        public int Weight { get; set; }
        public int Id { get; set; }
    }
}
