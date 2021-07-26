using MakeFriendSolution.Models.ViewModels;
using System;

namespace MakeFriendSolution.Models
{
    public class Feedback
    {
        public Feedback()
        {

        }
        public Feedback(FeedbackRequest request)
        {
            UserId = request.UserId;
            Title = request.Title;
            Content = request.Content;
            Vote = request.Vote;
            CreatedAt = DateTime.Now;
        }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Vote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AppUser AppUser { get; set; }
    }
}