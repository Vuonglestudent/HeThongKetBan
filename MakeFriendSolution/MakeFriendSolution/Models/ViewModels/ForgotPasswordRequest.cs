using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class ForgotPasswordRequest
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}