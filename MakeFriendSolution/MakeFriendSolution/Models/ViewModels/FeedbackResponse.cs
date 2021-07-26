using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class FeedbackResponse
    {
        public FeedbackResponse()
        {

        }

        public FeedbackResponse(Feedback feedback)
        {
            Id = feedback.Id;
            UserId = feedback.UserId;
            Title = feedback.Title;
            Content = feedback.Content;
            Vote = feedback.Vote;
            CreatedAt = feedback.CreatedAt;
            UpdatedAt = feedback.UpdatedAt;
        }

        public FeedbackResponse(Feedback feedback, AppUser user)
        {
            Id = feedback.Id;
            UserId = feedback.UserId;
            Title = feedback.Title;
            Content = feedback.Content;
            Vote = feedback.Vote;
            CreatedAt = feedback.CreatedAt;
            UpdatedAt = feedback.UpdatedAt;

            UserName = user.FullName;
            Avatar = user.AvatarPath;
        }

        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Vote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
