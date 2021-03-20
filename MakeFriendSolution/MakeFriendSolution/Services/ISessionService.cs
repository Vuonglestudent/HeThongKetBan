using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public interface ISessionService
    {
        void SetSessionUser(AppUser userInfo);

        LoginInfo GetSessionUser();

        LoginInfo GetDataFromToken();

        void Logout();
    }
}