using MakeFriendSolution.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PassWord { get; set; }
        public ERole Role { get; set; }
        public ETypeAccount TypeAccount { get; set; }
        public string FullName { get; set; }
        public EGender Gender { get; set; }
        public string AvatarPath { get; set; }
        public ELocation Location { get; set; }
        public EUserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //
        public int NumberOfFiends { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfImages { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsUpdatePosition { get; set; }

        public bool IsInfoUpdated { get; set; }
        public string PasswordForgottenCode { get; set; }
        public DateTime PasswordForgottenPeriod { get; set; }
        public int NumberOfPasswordConfirmations { get; set; }

        //

        public string Title { get; set; }
        public string Summary { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public DateTime Dob { get; set; }
        public EJob Job { get; set; }

        /// <summary>
        /// Dưới đây là các thông số dùng để tính toán
        /// </summary>

        public EGender FindPeople { get; set; }
        public EAgeGroup FindAgeGroup { get; set; }
        
        //

        public ICollection<ThumbnailImage> ThumbnailImages { get; set; }
        public ICollection<HaveMessage> SendMessages { get; set; }
        public ICollection<HaveMessage> ReceiveMessages { get; set; }
        public ICollection<Follow> Followed { get; set; }
        public ICollection<Follow> BeingFollowedBy { get; set; }
        public ICollection<Favorite> Favorited { get; set; }
        public ICollection<Favorite> BeingFavoritedBy { get; set; }
        public ICollection<BlockUser> UserWasBlock { get; set; }
        public ICollection<BlockUser> BlockedByUsers { get; set; }
        public ICollection<SimilarityScore> SimilarityScores { get; set; }

        public ICollection<UserFeature> HaveFeatures { get; set; }
        public ICollection<SearchFeature> SearchFeatures { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }

        public ICollection<Relationship> RelationshipFrom { get; set; }
        public ICollection<Relationship> RelationshipTo { get; set; }

        [NotMapped]
        public double Point { get; set; } = 0;
        [NotMapped]
        public double Distance { get; set; }

        public AppUser(AppUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            PassWord = user.PassWord;
            Role = user.Role;
            FullName = user.FullName;
            Gender = user.Gender;
            AvatarPath = user.AvatarPath;
            Location = user.Location;
            Status = user.Status;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            CreatedAt = user.CreatedAt;

            //

            Title = user.Title;
            Summary = user.Summary;
            FindPeople = user.FindPeople;
            Weight = user.Weight;
            Height = user.Height;
            Dob = user.Dob;
            Job = user.Job;

        }

        public AppUser()
        {
        }
    }
}