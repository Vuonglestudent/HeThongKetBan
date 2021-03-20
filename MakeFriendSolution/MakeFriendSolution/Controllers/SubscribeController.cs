using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MakeFriendSolution.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly IMailchimpService _mailchimpService;
        public SubscribeController(IMailchimpService mailchimp)
        {
            _mailchimpService = mailchimp;
        }


        /// <summary>
        /// Đăng ký nhận email từ website
        /// </summary>
        /// <param name="mailChimp">Thông tin đăng ký</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Subscribe([FromForm] MailChimpModel mailChimp)
        {
            if (string.IsNullOrEmpty(mailChimp.Email))
                return BadRequest(new
                {
                    Message = "Email is required"
                });

            try
            {
                await _mailchimpService.Subscribe(mailChimp);
                return Ok(new
                {
                    Message = "Thanks for your Subscribe!"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = e.Message
                });
            }
        }
    }
}
