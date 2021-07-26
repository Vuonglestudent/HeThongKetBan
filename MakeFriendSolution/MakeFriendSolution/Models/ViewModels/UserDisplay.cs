﻿using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class UserDisplay
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AvatarPath { get; set; }
        public string Summary { get; set; }
        public int NumberOfFollowers { get; set; } = 0;
        public bool Followed { get; set; } = false;
        public double Distance { get; set; }
        public int NumberOfFavoritors { get; set; } = 0;
        public bool Favorited { get; set; } = false;
        public int NumberOfImages { get; set; }
        public double Point { get; set; } = 0;
        public string Status { get; set; }
        public EGender Gender { get; set; }
        public IStorageService _storageService { get; set; }

        public UserDisplay()
        {
        }

        public UserDisplay(AppUser user, IStorageService storageService)
        {
            this._storageService = storageService;
            Id = user.Id;
            FullName = user.FullName;
            CreatedAt = user.CreatedAt;
            Summary = user.Summary;
            Dob = user.Dob;
            Point = user.Point;
            AvatarPath = user.AvatarPath;
            NumberOfFavoritors = user.NumberOfLikes;
            NumberOfFollowers = user.NumberOfFiends;
            NumberOfImages = user.NumberOfImages;
            Status = user.Status.ToString();
            this.AvatarPath = storageService.GetFileUrl(AvatarPath);
            Distance = user.Distance;
        }

        public UserDisplay(UserResponse user)
        {
            Id = user.Id;
            FullName = user.FullName;
            Summary = user.Summary;
            Dob = user.Dob;
            Point = user.Point;
            AvatarPath = user.AvatarPath;
            NumberOfFavoritors = user.NumberOfFavoritors;
            NumberOfFollowers = user.NumberOfFollowers;
            NumberOfImages = user.NumberOfImages;
            Status = user.Status.ToString();
            AvatarPath = user.AvatarPath;
        }
    }
}