using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class UpdateAvatarRequest
    {
        public Guid UserId { get; set; }
        public IFormFile Avatar { get; set; }
        public string Title { get; set; }
    }
}