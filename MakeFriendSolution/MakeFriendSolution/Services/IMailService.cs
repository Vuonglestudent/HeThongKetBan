using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public interface IMailService
    {
        Task<string> SendMail(MailClass mailClass);

        string GetMailBody(LoginInfo info);

        string GetMailBodyToForgotPassword(LoginInfo info);
    }
}