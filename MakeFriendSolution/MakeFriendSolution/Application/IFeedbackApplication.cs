using MakeFriendSolution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public interface IFeedbackApplication
    {
        Task<FeedbackResponse> Create(FeedbackRequest request);
        Task<FeedbackResponse> Update(FeedbackRequest request);
        Task<List<FeedbackResponse>> Get(PagingRequest request);
        Task Delete(Guid userId, int feedbackId);
    }
}