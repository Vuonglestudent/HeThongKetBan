using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public SessionService(IHttpContextAccessor accessor)
        {
            contextAccessor = accessor;
        }

        private HttpContext Context
        {
            get
            {
                return contextAccessor.HttpContext;
            }
        }

        public void SetSessionUser(AppUser userInfo)
        {
            var user = new
            {
                UserId = userInfo.Id,
                userInfo.Email,
                userInfo.UserName,
                userInfo.FullName
            };
            Context.Session.SetString("SessionUser", JsonConvert.SerializeObject(user));
        }

        public LoginInfo GetSessionUser()
        {
            try
            {
                return JsonConvert.DeserializeObject<LoginInfo>(Context.Session.GetString("SessionUser"));
            }
            catch
            {
                return null;
            }
        }

        public LoginInfo GetDataFromToken()
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();

            if (claims.Count == 0)
                return null;

            LoginInfo info = new LoginInfo()
            {
                UserId = new Guid(claims[0].Value),
                FullName = claims[1].Value,
                UserName = claims[2].Value,
                Email = claims[3].Value
            };
            return info;
        }

        public void Logout()
        {
            Context.Session.Clear();
        }
    }
}