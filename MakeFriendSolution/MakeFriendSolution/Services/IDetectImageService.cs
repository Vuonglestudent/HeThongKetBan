using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public interface IDetectImageService
    {
        DetectImageResponse DetectImage(string fileName);
    }
}
