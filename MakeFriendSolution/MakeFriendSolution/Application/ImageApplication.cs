using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public class ImageApplication : IImageApplication
    {
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private readonly ISessionService _sessionService;

        public ImageApplication(MakeFriendDbContext context, IStorageService storageService, ISessionService sessionService)
        {
            _context = context;
            _storageService = storageService;
            _sessionService = sessionService;
        }

        public async Task<ThumbnailImage> GetImageById(int imageId)
        {
            return await _context.ThumbnailImages.FindAsync(imageId);
        }

        public async Task<List<ImageResponse>> GetImageByUserId(Guid userId)
        {
            var images = await _context.ThumbnailImages.Where(x => x.UserId == userId && x.Status == Models.Enum.ImageStatus.Approved).ToListAsync();
            var response = new List<ImageResponse>();
            foreach (var image in images)
            {
                var imageRes = new ImageResponse(image, _storageService);
                response.Add(imageRes);
            }

            return response;
        }

        public async Task<bool> IsExist(int imageId)
        {
            return await _context.ThumbnailImages.AnyAsync(x => x.Id == imageId);
        }

        public async Task<string> LikeImage(LikeImageRequest request)
        {
            var isLike = await _context.LikeImages
            .AnyAsync(x => x.UserId == request.UserId && x.ImageId == request.ImageId);

            var message = "";

            if (isLike)
            {
                await this.Unlike(request);
                message = "Unliked";
            }
            else
            {
                await this.Like(request);
                message = "Liked";
            }

            return message;
        }

        private async Task<bool> Unlike(LikeImageRequest request)
        {
            var likeImage = await _context.LikeImages.Where(x => x.UserId == request.UserId && x.ImageId == request.ImageId).FirstOrDefaultAsync();
            var image = await _context.ThumbnailImages.FindAsync(request.ImageId);
            image.NumberOflikes--;
            
            try
            {
                _context.LikeImages.Remove(likeImage);
                if(image.NumberOflikes < 0)
                {
                    image.NumberOflikes = 0;
                }
                _context.ThumbnailImages.Update(image);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
        private async Task<bool> Like(LikeImageRequest request)
        {
            var likeImage = new LikeImage()
            {
                UserId = request.UserId,
                ImageId = request.ImageId
            };
            var image = await _context.ThumbnailImages.FindAsync(request.ImageId);
            image.NumberOflikes++;
            try
            {
                _context.ThumbnailImages.Update(image);
                _context.LikeImages.Add(likeImage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ImageResponse>> CreateImages(List<ThumbnailImage> images)
        {
            try
            {
                _context.ThumbnailImages.AddRange(images);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }
            var imagesResponse = new List<ImageResponse>();
            foreach (var image in images)
            {
                if(image.Status == Models.Enum.ImageStatus.Approved)
                    imagesResponse.Add(new ImageResponse(image, _storageService));
            }
            return imagesResponse;
        }

        public async Task<List<ImageResponse>> GetWaitingImages(PagingRequest request)
        {
            var images = await _context.ThumbnailImages.Where(x => x.Status == Models.Enum.ImageStatus.Waiting)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var imagesResponse = new List<ImageResponse>();
            foreach (var image in images)
            {
                imagesResponse.Add(new ImageResponse(image, _storageService));
            }
            return imagesResponse;
        }

        public async Task<bool> ApproveImage(int imageId)
        {
            var image = await GetImageById(imageId);

            if (image == null || image.Status == ImageStatus.BlockOut)
            {
                return false;
            }

            image.Status = ImageStatus.Approved;

            try
            {
                _context.ThumbnailImages.Update(image);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> BlockOutImage(int imageId)
        {
            var image = await GetImageById(imageId);

            if (image == null)
            {
                return false;
            }

            image.Status = ImageStatus.BlockOut;

            try
            {
                _context.ThumbnailImages.Update(image);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<NewsResponse>> GetNewImages(PagingRequest request)
        {
            var userInfo = _sessionService.GetDataFromToken();
            var images = await _context.ThumbnailImages.Where(x => x.Status == ImageStatus.Approved)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var blocks = await _context.BlockUsers
                .Where(x => x.FromUserId == userInfo.UserId || x.ToUserId == userInfo.UserId)
                .ToListAsync();

            var blockIds = new List<Guid>();
            foreach (var item in blocks)
            {
                if (item.FromUserId == userInfo.UserId)
                    blockIds.Add(item.ToUserId);
                else
                    blockIds.Add(item.FromUserId);
            }

            images = (from i in images
                      where !blockIds.Contains(i.UserId)
                      select i).ToList().Skip((request.PageIndex - 1) * request.PageSize)
                      .Take(request.PageSize).ToList();

            foreach (var item in images)
            {
                item.User = await _context.Users.FindAsync(item.UserId);
            }

            //foreach (var item in blocks)
            //{
            //    var block = images.Where(x => x.UserId == item.FromUserId).ToList();
            //    foreach (var image in block)
            //    {
            //        images.Remove(image);
            //    }
            //}

            var response = new List<NewsResponse>();
            foreach (var item in images)
            {
                var imageRes = new NewsResponse(item, _storageService);
                imageRes.liked = await this.IsLiked(userInfo.UserId, item.Id);
                imageRes.Followed = await this.IsFollowed(userInfo.UserId, item.UserId);
                response.Add(imageRes);
            }

            return response;
        }

        private async Task<bool> IsLiked(Guid userId, int imageId)
        {
            return await _context.LikeImages.AnyAsync(x => x.UserId == userId && x.ImageId == imageId);
        }
        private async Task<bool> IsFollowed(Guid fromId, Guid toId)
        {
            return await _context.Follows.Where(x => x.FromUserId == fromId && x.ToUserId == toId).AnyAsync();
        }
    }
}
