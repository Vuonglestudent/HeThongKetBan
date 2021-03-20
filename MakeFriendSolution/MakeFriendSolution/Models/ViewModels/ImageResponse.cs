using MakeFriendSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class ImageResponse
    {

        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public int NumberOfLikes { get; set; }
        public bool liked { get; set; } = false;
        public string ImagePath { get; set; }
        public bool HasImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public ImageResponse()
        {
        }

        public ImageResponse(ThumbnailImage image, IStorageService storageService)
        {
            Id = image.Id;
            UserId = image.UserId;
            Title = image.Title;
            ImagePath = image.ImagePath;
            CreatedAt = image.CreatedAt;
            NumberOfLikes = image.NumberOflikes;

            ImagePath = storageService.GetFileUrl(image.ImagePath);
            //GetImagePath(image);
        }

        //private void GetImagePath(ThumbnailImage image)
        //{
        //    try
        //    {
        //        byte[] imageBits = System.IO.File.ReadAllBytes($"./{_storageService.GetFileUrl(image.ImagePath)}");
        //        this.ImagePath = Convert.ToBase64String(imageBits);
        //        this.HasImage = true;
        //    }
        //    catch
        //    {
        //        this.HasImage = false;
        //        this.ImagePath = image.ImagePath;
        //    }
        //}
    }
}