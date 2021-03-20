using MakeFriendSolution.Common;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public class MailService : IMailService
    {
        public string GetMailBody(LoginInfo info)
        {
            string url = Startup.DomainName + "api/v1/Authenticates/confirmMail?userId=" + info.UserId;
            return string.Format(@"<div style='text-align:center;'>
                                    <h1>Welcome to our Web Site</h1>
                                    <h3>Click below button for verify your Email Id</h3>
                                    <form method='post' action='{0}' style='display: inline;'>
                                      <button type = 'submit' style=' display: block;
                                                                    text-align: center;
                                                                    font-weight: bold;
                                                                    background-color: #008CBA;
                                                                    font-size: 16px;
                                                                    border-radius: 10px;
                                                                    color:#ffffff;
                                                                    cursor:pointer;
                                                                    width:100%;
                                                                    padding:10px;'>
                                        Confirm Mail
                                      </button>
                                    </form>
                                </div>", url, info.UserId);
        }

        public string GetMailBodyToForgotPassword(LoginInfo info)
        {
            return string.Format(@"<div style='text-align:center;'>
                                    <h1>Hello {0}, Welcome to our Web Site</h1>
                                    <h3>This mail contains a confirmation code for forgetting your password.</h3>
                                    <p>Your code validation: <b>{1}</b></p>
                                </div>", info.FullName, info.Message);
        }

        public async Task<string> SendMail(MailClass mailClass)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailClass.FromMail);
                    mailClass.ToMails.ForEach(x =>
                    {
                        mail.To.Add(x);
                    });

                    mail.Subject = mailClass.Subject;
                    mail.Body = mailClass.Body;
                    mail.IsBodyHtml = mailClass.IsBodyHtml;
                    mailClass.Attachments.ForEach(x =>
                    {
                        mail.Attachments.Add(new Attachment(x));
                    });

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(mailClass.FromMail, mailClass.FromMailPassword);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                        return MessageMail.MailSent;
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}