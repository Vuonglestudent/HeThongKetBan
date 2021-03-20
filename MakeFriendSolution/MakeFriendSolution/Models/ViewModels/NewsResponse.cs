using MakeFriendSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class NewsResponse
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public bool HasAvatar { get; set; }
        public string Location { get; set; }
        public bool Followed { get; set; }
        public string Title { get; set; }
        public int NumberOfLikes { get; set; }
        public bool liked { get; set; } = false;
        public string ImagePath { get; set; }
        public bool HasImage { get; set; }
        public DateTime CreatedAt { get; set; }

        public NewsResponse()
        {
        }

        public NewsResponse(ThumbnailImage image, IStorageService storageService)
        {
            Id = image.Id;
            UserId = image.UserId;
            Title = image.Title;
            ImagePath = image.ImagePath;
            CreatedAt = image.CreatedAt;
            NumberOfLikes = image.NumberOflikes;
            FullName = image.User.FullName;
            Location = image.User.Location.ToString();
            ImagePath = storageService.GetFileUrl(image.ImagePath);
            AvatarPath = storageService.GetFileUrl(image.User.AvatarPath);
            //GetImagePath(image);
            //GetAvatarPath(image.User);
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
        //private void GetAvatarPath(AppUser user)
        //{
        //    try
        //    {
        //        byte[] imageBits = System.IO.File.ReadAllBytes($"./{_storageService.GetFileUrl(user.AvatarPath)}");
        //        this.AvatarPath = Convert.ToBase64String(imageBits);
        //        this.HasAvatar = true;
        //    }
        //    catch
        //    {
        //        this.HasAvatar = false;
        //        this.AvatarPath = user.AvatarPath;
        //    }
        //}

    }
}
