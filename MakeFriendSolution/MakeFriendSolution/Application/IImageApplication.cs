using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public interface IImageApplication
    {
        Task<List<ImageResponse>> GetImageByUserId(Guid userId);
        Task<ThumbnailImage> GetImageById(int imageId);
        Task<string> LikeImage(LikeImageRequest request);
        Task<bool> IsExist(int imageId);
        Task<List<ImageResponse>> CreateImages(List<ThumbnailImage> images);
        Task<List<ImageResponse>> GetWaitingImages(PagingRequest request);

        Task<bool> ApproveImage(int imageId);
        Task<bool> BlockOutImage(int imageId);
        Task<List<NewsResponse>> GetNewImages(PagingRequest request);
    }
}
