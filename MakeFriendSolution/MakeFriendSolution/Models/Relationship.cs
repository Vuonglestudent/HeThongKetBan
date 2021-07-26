using MakeFriendSolution.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class Relationship
    {
        public int Id { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public AppUser FromUser { get; set; }
        public AppUser ToUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ERelationShip RelationShipType { get; set; }
        public bool HasRelationship { get; set; }
        public bool IsAccept { get; set; }
    }
}