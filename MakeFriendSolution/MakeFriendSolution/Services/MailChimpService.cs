using MailChimp.Net;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using MakeFriendSolution.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public class MailchimpService : IMailchimpService
    {
        private IMailChimpManager mailChimpManager;
        private readonly IConfiguration _config;
        public MailchimpService(IConfiguration config)
        {
            _config = config;
            mailChimpManager = new MailChimpManager(_config["MailChimp:ApiKey"]);
        }

        public async Task Subscribe(MailChimpModel mailChimp)
        {
            var listId = _config["MailChimp:ListId"];
            // Use the Status property if updating an existing member
            var member = new Member { EmailAddress = $"{mailChimp.Email}", StatusIfNew = Status.Subscribed };
            member.MergeFields.Add("LNAME", mailChimp.Name);

            await this.mailChimpManager.Members.AddOrUpdateAsync(listId, member);
        }
    }
}
