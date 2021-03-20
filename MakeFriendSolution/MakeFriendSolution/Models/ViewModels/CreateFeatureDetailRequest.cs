using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class CreateFeatureDetailRequest
    {
        public string Content { get; set; }
        public int Weight { get; set; }
        public int FeatureId { get; set; }
    }
}
