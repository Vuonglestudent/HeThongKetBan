using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MakeFriendSolution.HubConfig.Models
{
    public class UserConnection
    {
        private static readonly List<UserConnection> Users = new List<UserConnection>();
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public string ConnectionId { get; set; }
        public static void Remove(UserConnection user)
        {
            if(Users.Count > 0)
            {
                Users.Remove(user);
            }
        }

        public static UserConnection Get(string connectionId)
        {
            return Users.Where(u => u.ConnectionId == connectionId).FirstOrDefault();
        }
        public static List<UserConnection> Get(Guid userId)
        {
            if(Users.Count == 0)
            {
                return null;
            }
            return Users.Where(u => u.UserId == userId).ToList();
        }

        public static UserConnection Get(Guid userId, string connectionId, string userName, string avatarPath)
        {
            lock (Users)
            {
                var current = Users.SingleOrDefault(u => u.ConnectionId == connectionId);

                if (current == default(UserConnection))
                {
                    current = new UserConnection
                    {
                        UserId = userId,
                        UserName = userName,
                        AvatarPath = avatarPath,
                        ConnectionId = connectionId,
                    };

                    Users.Add(current);
                }
                else
                {
                    current.UserId = userId;
                    current.UserName = userName;
                    current.AvatarPath = avatarPath;
                }

                return current;
            }
        }
    }
}