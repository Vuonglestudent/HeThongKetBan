using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class DetectImageResponse
    {
        public double Drawings { get; set; }
        public double Hentai { get; set; }
        public double Neutral { get; set; }
        public double Porn { get; set; }
        public double Sexy { get; set; }
    }
}
