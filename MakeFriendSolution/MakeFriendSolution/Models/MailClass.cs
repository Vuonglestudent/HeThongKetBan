using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models
{
    public class MailClass
    {
        //public string FromMail { get; set; } = "tam.developer1401@gmail.com";
        //public string FromMailPassword { get; set; } = "Tamdeveloper1401";

        public string FromMail { get; set; } = "ng.th.tam1401@gmail.com";
        public string FromMailPassword { get; set; } = "14011998@Aa";
        public List<string> ToMails { get; set; } = new List<string>();
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public bool IsBodyHtml { get; set; } = true;
        public List<string> Attachments { get; set; } = new List<string>();
    }
}