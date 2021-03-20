using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public interface IImageScoreApplication
    {
        Task<ImageScore> GetImageScore();
        Task<bool> UpdateImageScore(ImageScore update);
        Task<bool> ValidateImage(DetectImageResponse scores);
    }
}