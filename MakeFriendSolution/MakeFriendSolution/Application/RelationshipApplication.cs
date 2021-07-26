using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public class RelationshipApplication : IRelationshipApplication
    {
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private readonly INotificationApplication _notificationApplication;

        public RelationshipApplication(MakeFriendDbContext context, IStorageService storageService, INotificationApplication notificationApplication)
        {
            _context = context;
            _notificationApplication = notificationApplication;
            _storageService = storageService;
        }

        public async Task Accept(int id)
        {
            // 1. Kiểm tra tồn tại
            var relationship = await _context.Relationships
                .Where(x => x.Id == id && x.HasRelationship)
                .FirstOrDefaultAsync();

            if (relationship == null)
                throw new Exception("Can not find relationship");

            var relationShipsFrom = await _context.Relationships
                .Where(x => (x.FromId == relationship.FromId || x.ToId == relationship.FromId) && x.HasRelationship)
                .ToListAsync();

            var relationShipsTo = await _context.Relationships
                .Where(x => (x.FromId == relationship.ToId || x.ToId == relationship.ToId) && x.HasRelationship)
                .ToListAsync();

            var relationShips = relationShipsFrom.Concat(relationShipsTo);

            relationShips = relationShips.Where(x => x.Id != relationship.Id).ToList();

            foreach (var item in relationShips)
            {
                item.HasRelationship = false;
                item.IsAccept = false;
                item.UpdatedAt = DateTime.Now;
            }

            relationship.IsAccept = true;
            relationship.UpdatedAt = DateTime.Now;
            relationship.HasRelationship = true;

            await _notificationApplication.DeleteFromUseId(relationship.FromId);
            await _notificationApplication.DeleteFromUseId(relationship.ToId);

            try
            {
                _context.Relationships.UpdateRange(relationShips);
                _context.Relationships.Update(relationship);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Can not accept this relationship");
            }
        }

        public async Task<Relationship> Create(RelationshipRequest request)
        {
            var relationship = await _context.Relationships
                .Where(x => (x.FromId == request.FromId && x.ToId == request.ToId) || (x.FromId == request.ToId && x.ToId == request.FromId))
                .FirstOrDefaultAsync();

            //Chưa có relationship
            if(relationship == null)
            {
                if (request.RelationShipType == Models.Enum.ERelationShip.Không_có_gì)
                    return null;

                relationship = new Relationship()
                {
                    Id = 0,
                    FromId = request.FromId,
                    ToId = request.ToId,
                    HasRelationship = true,
                    RelationShipType = request.RelationShipType,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Relationships.Add(relationship);

            }
            //Khác Null, đã từng có relationship
            else
            {
                if (request.RelationShipType == relationship.RelationShipType)
                    return relationship;

                relationship.IsAccept = false;
                relationship.UpdatedAt = DateTime.Now;

                if (request.RelationShipType == Models.Enum.ERelationShip.Không_có_gì)
                {
                    relationship.HasRelationship = false;
                }
                else
                {
                    relationship.HasRelationship = true;
                    relationship.RelationShipType = request.RelationShipType;
                }

                _context.Relationships.Update(relationship);
            }

            try
            {
                await _context.SaveChangesAsync();
                return relationship;
            }
            catch (Exception)
            {
                throw new Exception("Can not save Relationship");
            }

        }

        public async Task Decline(int id)
        {
            var relationship = await _context.Relationships
                .Where(x => x.Id == id && x.HasRelationship)
                .FirstOrDefaultAsync();

            if (relationship == null)
                throw new Exception("Can not find relationship");

            relationship.UpdatedAt = DateTime.Now;
            relationship.HasRelationship = false;

            var notis = await _context.Notifications
                .Where(x => ((x.FromId == relationship.FromId && x.ToId == relationship.ToId)
                || (x.FromId == relationship.ToId && x.ToId == relationship.FromId))
                && x.Type == "relationship").ToListAsync();

            _context.Notifications.RemoveRange(notis);
            try
            {
                _context.Relationships.Update(relationship);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Can not decline this relationship");
            }
        }

        public async Task Delete(Guid userId)
        {
            var relationship = await _context.Relationships
                .Where(x => (x.FromId == userId || x.ToId == userId) && x.HasRelationship)
                .FirstOrDefaultAsync();

            if(relationship != null)
            {
                relationship.HasRelationship = false;
            }

            try
            {
                _context.Relationships.Update(relationship);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Can not delete this relationship");
            }

        }

        public async Task<RelationshipResponse> GetById(Guid fromId, Guid toId)
        {
            var relationship = await _context.Relationships
                .Where(x => ((x.FromId == fromId && x.ToId == toId) || (x.FromId == toId && x.ToId == fromId)) && x.HasRelationship)
                .FirstOrDefaultAsync();

            if (relationship == null)
                throw new Exception("Can not find relationship");

            var fromUser = await _context.Users.FindAsync(relationship.FromId);
            var toUser = await _context.Users.FindAsync(relationship.ToId);

            return new RelationshipResponse(relationship, fromUser, toUser, _storageService);
        }

        public async Task<RelationshipResponse> GetByUserId(Guid userId)
        {
            var relationship = await _context.Relationships
                .Where(x => (x.FromId == userId || x.ToId == userId) && x.HasRelationship && x.IsAccept)
                .FirstOrDefaultAsync();

            if (relationship == null)
                throw new Exception("Can not find relationship");

            var fromUser = await _context.Users.FindAsync(relationship.FromId);
            var toUser = await _context.Users.FindAsync(relationship.ToId);

            return new RelationshipResponse(relationship, fromUser, toUser, _storageService);
        }
    }
}
