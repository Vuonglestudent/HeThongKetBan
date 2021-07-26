using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public interface IRelationshipApplication
    {
        Task<Relationship> Create(RelationshipRequest request);
        Task Delete(Guid userId);
        Task<RelationshipResponse> GetByUserId(Guid userId);
        Task<RelationshipResponse> GetById(Guid fromId, Guid toId);
        Task Accept(int id);
        Task Decline(int id);
    }
}
