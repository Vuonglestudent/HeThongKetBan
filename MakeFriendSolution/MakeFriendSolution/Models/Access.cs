using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class Access
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int AuthorizeCount { get; set; }
        public int UnauthorizeCount { get; set; }
    }
}