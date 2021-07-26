using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class SocialLoginRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Provider { get; set; }
    }
}