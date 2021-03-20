using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class LikeImage
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ImageId { get; set; }
        public ThumbnailImage Image { get; set; }
        public AppUser User { get; set; }
    }
}