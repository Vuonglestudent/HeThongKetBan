using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class ImageRequest
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}