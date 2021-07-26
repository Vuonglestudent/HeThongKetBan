using MakeFriendSolution.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class RelationshipRequest
    {
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public ERelationShip RelationShipType { get; set; }
    }
}
