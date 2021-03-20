using MakeFriendSolution.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Common
{
    public static class GlobalVariable
    {
        public static List<Feature> Features = null;
        public static List<FeatureDetail> FeatureDetails = null;
        public static bool HaveData = false;

    }
}