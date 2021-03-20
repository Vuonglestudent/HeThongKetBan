using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class LikeImageRequest
    {
        public Guid UserId { get; set; }
        public int ImageId { get; set; }
    }
}
