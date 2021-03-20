using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MakeFriendSolution.EF;
using MakeFriendSolution.HubConfig;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MakeFriendSolution.Controllers
{
    /// <summary>
    /// Controller chứa các api nhắn tin, và get tin nhắn
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hub;
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;

        public ChatsController(IHubContext<ChatHub> hub, MakeFriendDbContext context, IStorageService storageService)
        {
            _hub = hub;
            _context = context;
            _storageService = storageService;
        }


        /// <summary>
        /// Tạo tin nhắn
        /// </summary>
        /// <param name="request">Nội dung tin nhắn và ID người nhận, người gửi</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]  
        public async Task<IActionResult> Post([FromForm] CreateMessageRequest request)
        {
            var result = await SaveMessage(request);
            if (result == null)
            {
                return BadRequest("Can not send message now!");
            }

            var sender = await _context.Users.FindAsync(request.SenderId);
            var receiver = await _context.Users.FindAsync(request.ReceiverId);

            var display = new UserDisplay(sender, _storageService);

            var response = new ChatResponse()
            {
                SenderId = result.SenderId,
                ReceiverId = result.ReceiverId,
                Content = result.Content,
                SentAt = result.SentAt,
                Id = result.Id,
                FullName = display.FullName,
                Avatar = display.AvatarPath
            };
            
            await _hub.Clients.Clients(receiver.ConnectionId, sender.ConnectionId).SendAsync("transferData", response);

            return Ok(new { Message = "Request complete!" });
        }


        /// <summary>
        /// Lấy tin nhắn giữa hai người
        /// </summary>
        /// <param name="request"> Id người gửi và Id người nhận</param>
        /// <returns>Danh sách tin nhắn</returns>
        [Authorize]
        [HttpGet("MoreMessages")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MessageResponse>))]
        public async Task<IActionResult> GetMessages([FromQuery] StartChatRequest request)
        {
            var messages = await _context.HaveMessages
                .Where(x => (x.SenderId == request.SenderId && x.ReceiverId == request.ReceiverId) || (x.SenderId == request.ReceiverId && x.ReceiverId == request.SenderId))
                .OrderByDescending(x => x.SentAt)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();

            List<MessageResponse> responses = new List<MessageResponse>();

            foreach (var item in messages)
            {
                MessageResponse res = new MessageResponse(item);
                if (item.SenderId == request.SenderId)
                {
                    res.Type = "sent";
                }
                else
                {
                    res.Type = "received";
                }
                responses.Add(res);
            }
            return Ok(responses);
        }


        /// <summary>
        /// Lấy thông tin những người nhắn tin gần đây
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request">Paging info</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("friends/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendResponse>))]
        public async Task<IActionResult> GetFriendList(Guid userId, [FromQuery] PagingRequest request)
        {
            request.PageSize = 20;
            var messages = new List<MessageResponse>();
            var tempMessages = new List<MessageResponse>();
            var userList = new List<Guid>();
            var recentMessages = await _context.HaveMessages
                .Where(x => x.SenderId == userId || x.ReceiverId == userId)
                .OrderByDescending(x => x.SentAt)
                .ToListAsync();

            foreach (var item in recentMessages)
            {
                var m = new MessageResponse(item);
                if (item.SenderId == userId)
                {
                    m.Type = "sent";
                }
                else
                {
                    m.Type = "received";
                }
                tempMessages.Add(m);
            }

            foreach (MessageResponse message in tempMessages)
            {
                if (message.Type == "sent")
                {
                    if (!messages.Any(x => x.ReceiverId == message.ReceiverId || x.ReceiverId == message.SenderId))
                    {
                        messages.Add(message);
                    }
                }
                else
                {
                    if (!messages.Any(x => x.SenderId == message.ReceiverId || x.SenderId == message.SenderId))
                    {
                        messages.Add(message);
                    }
                }
            }

            messages = messages
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();
            var friendList = new List<FriendResponse>();
            foreach (MessageResponse message in messages)
            {
                AppUser user = new AppUser();
                if (message.SenderId == userId)
                {
                    user = await _context.Users.FindAsync(message.ReceiverId);
                }
                else
                {
                    user = await _context.Users.FindAsync(message.SenderId);
                }

                var userDisplay = new UserDisplay(user, _storageService);
                var messageResponses = new List<MessageResponse>();
                messageResponses.Add(message);

                var friend = new FriendResponse()
                {
                    Messages = messageResponses,
                    User = userDisplay
                };

                friendList.Add(friend);
            }

            return Ok(friendList);
        }


        /// <summary>
        /// Thấy thông tin user và tin nhắn
        /// </summary>
        /// <param name="userId">ID của mình</param>
        /// <param name="friendId">ID của bạn bè</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("chatFriend")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDisplayUser(Guid userId, Guid friendId)
        {
            var user = await _context.Users.FindAsync(friendId);
            if (user == null)
            {
                return NotFound();
            }
            UserDisplay userResponse = new UserDisplay(user, _storageService);

            var messages = await _context.HaveMessages
                .Where(x => (x.SenderId == userId && x.ReceiverId == friendId)
                    || (x.SenderId == friendId && x.ReceiverId == friendId))
                .OrderByDescending(x => x.SentAt)
                //.Take(20)
                .ToListAsync();

            List<MessageResponse> messageResponses = new List<MessageResponse>();

            foreach (var item in messages)
            {
                MessageResponse res = new MessageResponse(item);
                if (item.SenderId == userId)
                {
                    res.Type = "sent";
                }
                else
                {
                    res.Type = "received";
                }
                messageResponses.Add(res);
            }
            var friendResponse = new FriendResponse()
            {
                User = userResponse,
                Messages = messageResponses
            };
            return Ok(friendResponse);
        }

        private async Task<HaveMessage> SaveMessage(CreateMessageRequest message)
        {

            //var isBlock = await _context.BlockUsers.AnyAsync(x => (x.FromUserId == message.SenderId && x.ToUserId == message.ReceiverId) || (x.FromUserId == message.ReceiverId && x.ToUserId == message.SenderId));

            //if (isBlock)
            //{
            //    return null;
            //}

            var newMessage = new HaveMessage()
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                SentAt = DateTime.Now
            };

            try
            {
                _context.HaveMessages.Add(newMessage);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return newMessage;
        }
    }
}