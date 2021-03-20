using MakeFriendSolution.Models.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class SignUpRequest
    {
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}