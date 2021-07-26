using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class RelationshipResponse
    {
        public RelationshipResponse() { }
        public RelationshipResponse(Relationship relationShip, AppUser fromUser, AppUser toUser, IStorageService storageService)
        {
            Id = relationShip.Id;
            FromId = fromUser.Id;
            ToId = toUser.Id;
            CreatedAt = relationShip.CreatedAt;
            UpdatedAt = relationShip.UpdatedAt;
            FromName = fromUser.FullName;
            ToName = toUser.FullName;
            FromAvatar = storageService.GetFileUrl(fromUser.AvatarPath);
            ToAvatar = storageService.GetFileUrl(toUser.AvatarPath);
            RelationshipType = relationShip.RelationShipType;
        }
        public int Id { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public ERelationShip RelationshipType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string FromAvatar { get; set; }
        public string ToAvatar { get; set; }
    }
}
