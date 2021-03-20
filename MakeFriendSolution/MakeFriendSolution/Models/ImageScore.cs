using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class ImageScore
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public bool AutoFilter { get; set; }
        public double Drawings { get; set; }
        public double Hentai { get; set; }
        public double Neutral { get; set; }
        public double Porn { get; set; }
        public double Sexy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
