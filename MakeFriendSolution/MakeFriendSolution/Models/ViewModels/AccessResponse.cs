using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class AccessResponse
    {
        public int AuthorizeCount { get; set; } = 0;
        public int UnauthorizeCount { get; set; } = 0;
        public int Total { get; set; } = 0;
        public DateTime Period { get; set; } = DateTime.Now.Date;

        public AccessResponse(Access access)
        {
            AuthorizeCount = access.AuthorizeCount;
            UnauthorizeCount = access.UnauthorizeCount;
            Total = AuthorizeCount + UnauthorizeCount;
            Period = access.Date;
        }

        public AccessResponse()
        {
        }
    }
}