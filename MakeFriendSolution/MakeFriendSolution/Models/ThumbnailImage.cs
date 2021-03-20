using MakeFriendSolution.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class ThumbnailImage
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public int NumberOflikes { get; set; }
        public ImageStatus Status { get; set; }
        public AppUser User { get; set; }
        public ICollection<LikeImage> LikeImages { get; set; }
    }
}