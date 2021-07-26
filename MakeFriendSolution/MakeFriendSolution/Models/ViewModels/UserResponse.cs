using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class UserResponse
    {
        //private IStorageService _storageService;

        public UserResponse(AppUser user, IStorageService storageService)
        {

            Id = user.Id;
            UserName = user.UserName;
            FullName = user.FullName;
            Gender = user.Gender.ToString();
            Status = user.Status.ToString();
            Role = user.Role.ToString();
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Summary = user.Summary;
            IsInfoUpdated = user.IsInfoUpdated;
            NumberOfFavoritors = user.NumberOfLikes;
            NumberOfFollowers = user.NumberOfFiends;
            NumberOfImages = user.NumberOfImages;
            Age = this.CalculateAge(user.Dob);
            //
            Job = user.Job.ToString();
            Location = user.Location.ToString();
            Title = user.Title;
            Weight = user.Weight;
            Height = user.Height;
            Dob = user.Dob;
            FindPeople = user.FindPeople.ToString();
            FindAgeGroup = user.FindAgeGroup.ToString();
            CreatedAt = user.CreatedAt;
            //GetImagePath(user);
            AvatarPath = storageService.GetFileUrl(user.AvatarPath);
        }

        //private void GetImagePath(AppUser user)
        //{
        //    try
        //    {
        //        var a = _storageService.GetFileUrl(user.AvatarPath);
        //        byte[] imageBits = System.IO.File.ReadAllBytes($"./{a}");
        //        this.AvatarPath = Convert.ToBase64String(imageBits);
        //        this.HasAvatar = true;
        //    }
        //    catch
        //    {
        //        this.HasAvatar = false;
        //        this.AvatarPath = user.AvatarPath;
        //    }
        //}

        private int CalculateAge(DateTime birthDay)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDay.Year;
            if (birthDay > today.AddYears(-age))
                age--;

            return age;
        }

        public Guid Id { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string AvatarPath { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Point { get; set; }
        public bool HasAvatar { get; set; }
        public string Summary { get; set; }
        public int NumberOfFollowers { get; set; }
        public int NumberOfFavoritors { get; set; }
        public int NumberOfImages { get; set; }
        public bool Followed { get; set; }
        public bool Favorited { get; set; }
        public bool Blocked { get; set; } = false;
        public int Age { get; set; }
        public bool IsInfoUpdated { get; set; }
        public string Token { get; set; }
        //
        public string Title { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public DateTime Dob { get; set; }
        public string Job { get; set; }
        public string Location { get; set; }
        public string FindPeople { get; set; }
        public string FindAgeGroup { get; set; }
        public DateTime CreatedAt { get; set; }
        public RelationshipResponse Relationship { get; set; }
        public List<FeatureResponse> Features { get; set; }
        public List<FeatureResponse> SearchFeatures { get; set; }
    }
}