using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public interface IMailchimpService
    {
        Task Subscribe(MailChimpModel mailChimp);
    }
}
