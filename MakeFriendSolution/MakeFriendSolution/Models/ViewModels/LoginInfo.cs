using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class LoginInfo
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsMailConfirmed { get; set; }
    }
}