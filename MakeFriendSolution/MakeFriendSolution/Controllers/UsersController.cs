using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Application;
using Microsoft.AspNetCore.Http;

namespace MakeFriendSolution.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private readonly ISessionService _sessionService;
        private readonly IUserApplication _userApplication;
        private readonly IFeatureApplication _featureApplication;
        private readonly INotificationApplication _notificationApp;
        private readonly IRelationshipApplication _relationshipApp;

        public UsersController(MakeFriendDbContext context, IStorageService storageService,
            ISessionService sessionService, IUserApplication userApplication, IFeatureApplication featureApplication,
            INotificationApplication notificationApp, IRelationshipApplication relationshipApp)
        {
            _context = context;
            _storageService = storageService;
            _sessionService = sessionService;
            _userApplication = userApplication;
            _featureApplication = featureApplication;
            _notificationApp = notificationApp;
            _relationshipApp = relationshipApp;
        }

        /// <summary>
        /// Lấy danh sách những người mới đăng ký tài khoản
        /// </summary>
        /// <param name="request">Thông tin phân trang</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("newUsers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDisplay>))]
        public async Task<IActionResult> GetNewestUsers([FromQuery] PagingRequest request)
        {
            var users = await _userApplication.GetActiveUsers(Guid.Empty, false);

            var loginInfo = _sessionService.GetDataFromToken();
            if (loginInfo != null)
            {
                users = users.Where(x => x.Id != loginInfo.UserId).ToList();
            }

            users = users.OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            var userDisplays = await _userApplication.GetUserDisplay(users);

            return Ok(userDisplays);
        }


        /// <summary>
        /// Danh sách những người được yêu thích nhiều nhất
        /// </summary>
        /// <param name="request">Thông tin phân trang</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("favoritest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDisplay>))]
        public async Task<IActionResult> GetFavoritestUsers([FromQuery] PagingRequest request)
        {
            var users = await _userApplication.GetActiveUsers(Guid.Empty, false);

            var loginInfo = _sessionService.GetDataFromToken();
            if (loginInfo != null)
            {
                users = users.Where(x => x.Id != loginInfo.UserId).ToList();
            }
            //Get user display
            var userDisplays = await _userApplication.GetUserDisplay(users);

            var response = userDisplays
                .OrderByDescending(x => x.NumberOfFavoritors)
                .ThenBy(x => x.FullName)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            foreach (UserDisplay item in response)
            {
                item.AvatarPath = _storageService.GetFileUrl(item.AvatarPath);
                if (loginInfo != null)
                {
                    item.Followed = await _userApplication.IsFollowed(item.Id, loginInfo.UserId);
                    item.Favorited = await _userApplication.IsLiked(item.Id, loginInfo.UserId);
                }
            }

            var pageTotal = users.Count / request.PageSize;
            return Ok(new
            {
                data = response,
                pageTotal = pageTotal
            });
        }


        /// <summary>
        /// Đổi mật khẩu đăng nhập
        /// </summary>
        /// <param name="request">Thông tin tài khoản và mật khẩu mới</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("changePassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.ConfirmPassword) || string.IsNullOrEmpty(request.NewPassword) || string.IsNullOrEmpty(request.ConfirmPassword))
            {
                return BadRequest(new
                {
                    Message = "Vui lòng điền đầy đủ thông tin"
                });
            }

            var user = await _userApplication.GetUserByEmail(request.Email);

            if (user == null)
            {
                return NotFound(new
                {
                    Message = "Can not find user with email = " + request.Email
                });
            }

            if (user.Status != Models.Enum.EUserStatus.Active)
            {
                return BadRequest(new
                {
                    Message = "Account is not active"
                });
            }

            if (user.PassWord != request.OldPassword.Trim())
            {
                return BadRequest(new
                {
                    Message = "Password is not correct"
                });
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new
                {
                    Message = "Confirm password is not correct"
                });
            }

            try
            {
                user.PassWord = request.NewPassword;
                user = await _userApplication.UpdateUser(user, false);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return StatusCode(500, new
                {
                    Message = e.Message
                });
            }

            return Ok(new
            {
                Message = "Update password success"
            });
        }


        /// <summary>
        /// Cập nhật avatar
        /// </summary>
        /// <param name="request">File avatar</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("avatar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateAvatar([FromForm] UpdateAvatarRequest request)
        {
            var user = await _userApplication.GetById(request.UserId);
            if (user.Status != EUserStatus.Active)
            {
                return BadRequest(new
                {
                    Message = "Account is not active!"
                });
            }
            var oldAvatar = user.AvatarPath;

            if (request.Avatar == null)
            {
                return BadRequest(new
                {
                    Message = "File is required!"
                });
            }

            user.AvatarPath = await _userApplication.SaveFile(request.Avatar);

            //if (oldAvatar != "image.png")
            //{
            //    await _storageService.DeleteFileAsync(oldAvatar);
            //}

            user = await _userApplication.UpdateUser(user, false);

            var response = new UserResponse(user, _storageService);
            return Ok(response);
        }


        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId">ID user</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var user = await _userApplication.GetById(userId);
            if (user == null)
                return NotFound("Can not find user by id = " + userId);

            var respone = new UserResponse(user, _storageService);
            var sessionUser = _sessionService.GetDataFromToken();
            if (sessionUser == null)
            {
                return BadRequest(new
                {
                    Message = "Can not read session"
                });
            }
            //Get Follow & Favorite
            respone.Followed = await _userApplication.IsFollowed(userId, sessionUser.UserId);
            respone.Favorited = await _userApplication.IsLiked(userId, sessionUser.UserId);

            respone.Blocked = await _userApplication.GetBlockStatus(sessionUser.UserId, userId);
            var features = await _featureApplication.GetFeatureResponseByUserId(user.Id);
            respone.Features = features.Item1;
            respone.SearchFeatures = features.Item2;


            try
            {
                respone.Relationship = await _relationshipApp.GetByUserId(userId);

            }
            catch (Exception)
            {
            }

            return Ok(respone);
        }

        /// <summary>
        /// Follow người dùng
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("follow")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Follow([FromForm] Guid userId)
        {
            var sessionUser = _sessionService.GetDataFromToken();

            if (sessionUser == null)
            {
                return BadRequest(new
                {
                    Message = "Can not read token"
                });
            }

            var followed = await _context.Follows
                .Where(x => x.FromUserId == sessionUser.UserId && x.ToUserId == userId)
                .FirstOrDefaultAsync();

            var user = await _userApplication.GetById(sessionUser.UserId);

            var message = "";

            if (followed == null)
            {
                var follow = new Follow()
                {
                    Content = "",
                    FromUserId = sessionUser.UserId,
                    ToUserId = userId
                };
                user.NumberOfFiends++;
                user = await _userApplication.UpdateUser(user, false);
                _context.Follows.Add(follow);
                message = "Followed";

                var nt = new Notification()
                {
                    CreatedAt = DateTime.Now,
                    FromId = sessionUser.UserId,
                    ToId = userId,
                    Type = "follow"
                };
                var noticeRes = await _notificationApp.CreateNotification(nt);
                if (noticeRes != null)
                {
                    await _notificationApp.SendNotification(noticeRes);
                }
            }
            else
            {
                user.NumberOfFiends--;
                user = await _userApplication.UpdateUser(user, false);
                _context.Follows.Remove(followed);
                message = "Unfollowed";
            }

            try
            {
                await _context.SaveChangesAsync();
                await _userApplication.UpdateSimilarityScores(sessionUser.UserId);
            }
            catch (Exception e)
            {
                return StatusCode(501, new
                {
                    Message = e.Message
                });
            }
            return Ok(new
            {
                Message = message
            });
        }


        /// <summary>
        /// Yêu thích người dùng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("favorite")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Favorite([FromForm] Guid userId)
        {
            var sessionUser = _sessionService.GetDataFromToken();

            if (sessionUser == null)
            {
                return BadRequest(new
                {
                    Message = "Can not read session"
                });
            }

            if (sessionUser.UserId == userId)
            {
                return BadRequest(new
                {
                    Message = "Can not favorite yourself"
                });
            }

            var favorited = await _userApplication.GetFavoriteById(sessionUser.UserId, userId);

            var user = await _userApplication.GetById(userId);

            var message = "";

            if (favorited == null)
            {
                var favorite = new Favorite()
                {
                    Content = "",
                    FromUserId = sessionUser.UserId,
                    ToUserId = userId
                };
                user.NumberOfLikes++;

                user = await _userApplication.UpdateUser(user, false);
                _context.Favorites.Add(favorite);
                message = "Favorited";

                var nt = new Notification()
                {
                    CreatedAt = DateTime.Now,
                    FromId = sessionUser.UserId,
                    ToId = userId,
                    Type = "like"
                };
                var noticeRes = await _notificationApp.CreateNotification(nt);
                if (noticeRes != null)
                {
                    await _notificationApp.SendNotification(noticeRes);
                }
            }
            else
            {
                user.NumberOfLikes--;

                user = await _userApplication.UpdateUser(user, false);
                _context.Favorites.Remove(favorited);
                message = "Unfavorited";
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(501, new
                {
                    Message = e.Message
                });
            }
            return Ok(new
            {
                Message = message
            });
        }

        /// <summary>
        /// Lấy danh sách bạn bè
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("follow/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDisplay>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetFriends(Guid userId, [FromQuery] PagingRequest request)
        {
            var sessionUser = _sessionService.GetDataFromToken();
            if (sessionUser == null)
            {
                return BadRequest(new
                {
                    Message = "Can not read session"
                });
            }

            if (userId != sessionUser.UserId)
            {
                return StatusCode(401);
            }

            var followers = await _context.Follows.Where(x => x.FromUserId == userId).Include(x => x.ToUser).ToListAsync();
            var response = followers.Select(x => new UserDisplay(x.ToUser, _storageService))
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .OrderByDescending(x => x.CreatedAt).ToList();
            foreach (var item in response)
            {
                item.Followed = await _userApplication.IsFollowed(item.Id, sessionUser.UserId);

                item.Favorited = await _userApplication.IsLiked(item.Id, sessionUser.UserId);
            }

            return Ok(response);
        }

        /// <summary>
        /// Lấy danh sách những người yêu thích
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("favorite/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDisplay>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetFavoritors(Guid userId)
        {
            var sessionUser = _sessionService.GetDataFromToken();
            if (sessionUser == null)
            {
                return BadRequest(new
                {
                    Message = "Can not read session"
                });
            }

            if (userId != sessionUser.UserId)
            {
                return StatusCode(401);
            }

            var favoritors = await _context
                .Favorites
                .Where(x => x.FromUserId == userId)
                .Include(x => x.ToUser)
                .ToListAsync();

            var response = favoritors.Select(x => new UserDisplay(x.ToUser, _storageService));
            foreach (var item in response)
            {
                item.Followed = await _userApplication.IsFollowed(item.Id, sessionUser.UserId);
                item.Favorited = await _userApplication.IsLiked(item.Id, sessionUser.UserId);
            }

            return Ok(response);
        }


        /// <summary>
        /// Lọc người dùng
        /// </summary>
        /// <param name="request">Các feature lọc</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("filterUsers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDisplay>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FilterUsers([FromQuery] FilterUserRequest request)
        {
            var users = new List<AppUser>();

            switch (request.Feature)
            {
                case "FullName":
                    users = request.IsAscending ? await _context.Users.OrderBy(x => x.FullName).ToListAsync() : await _context.Users.OrderByDescending(x => x.FullName).ToListAsync();

                    break;

                case "Like":
                    users = request.IsAscending ? await _context.Users.OrderBy(x => x.NumberOfLikes).ToListAsync() : await _context.Users.OrderByDescending(x => x.NumberOfLikes).ToListAsync();
                    break;

                case "Follow":
                    users = request.IsAscending ? await _context.Users.OrderBy(x => x.NumberOfFiends).ToListAsync() : await _context.Users.OrderByDescending(x => x.NumberOfFiends).ToListAsync();

                    break;

                case "ImageCount":
                    users = request.IsAscending ? await _context.Users.OrderBy(x => x.NumberOfImages).ToListAsync() : await _context.Users.OrderByDescending(x => x.NumberOfImages).ToListAsync();

                    break;

                case "Status":
                    users = request.IsAscending ? await _context.Users.OrderBy(x => x.Status).ToListAsync() : await _context.Users.OrderByDescending(x => x.Status).ToListAsync();

                    break;

                default:
                    break;
            }

            var pageTotal = users.Count / request.PageSize;

            users = users
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize).ToList();

            var response = await _userApplication.GetUserDisplay(users);

            return Ok(new
            {
                data = response,
                pageTotal = pageTotal
            });
        }


        /// <summary>
        /// Tạo user mẫu để demo
        /// </summary>
        /// <param name="request">SignUp request, các thông tin đăng ký</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("createDemoUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDisplay))]
        public async Task<IActionResult> CreateDemoUser([FromForm] SignUpSystemRequest request)
        {
            var user = new AppUser()
            {
                Email = request.Email,
                FullName = request.FullName,
                PassWord = request.Password,
                IsInfoUpdated = false,
                TypeAccount = ETypeAccount.System,
                AvatarPath = "image.png",
                Status = EUserStatus.Active,
                UserName = request.Email,
                Role = ERole.User,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }


        /// <summary>
        /// Vô hiệu hóa tài khoản
        /// </summary>
        /// <param name="userId">ID người bị vô hiệu hóa</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("block/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BlockUser(Guid userId)
        {
            var message = await _userApplication.DisableUser(userId);
            return Ok(new
            {
                Message = message
            });
        }


        /// <summary>
        /// Lấy danh sách đen - những người đã chặn
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("blackList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBlackList()
        {
            var userInfo = _sessionService.GetDataFromToken();
            if (userInfo == null)
            {
                return NotFound(new
                {
                    Message = "Can not found user"
                });
            }

            var blocks = await _context.BlockUsers.Where(x => x.FromUserId == userInfo.UserId && x.IsLock).ToListAsync();
            var userDisplays = new List<UserDisplay>();

            foreach (var item in blocks)
            {
                var user = new UserDisplay(await _userApplication.GetById(item.ToUserId), _storageService);
                userDisplays.Add(user);
            }

            return Ok(userDisplays);
        }


        /// <summary>
        /// Thêm người dùng vào danh sách đen
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("blackList/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddToBlackList(Guid userId)
        {
            if (!await _userApplication.IsExist(userId))
                return NotFound(new
                {
                    Message = "Not found User"
                });

            var userInfo = _sessionService.GetDataFromToken();
            if (userId == userInfo.UserId)
            {
                return BadRequest(new
                {
                    Message = "Can not block yourself"
                });
            }
            var follow = await _context.Follows
                .Where(x => (x.FromUserId == userId && x.ToUserId == userInfo.UserId) ||
                    (x.FromUserId == userInfo.UserId && x.ToUserId == userId))
                .ToListAsync();

            var block = await _context.BlockUsers
                .Where(x => x.FromUserId == userInfo.UserId && x.ToUserId == userId)
                .FirstOrDefaultAsync();

            if (block == null)
            {
                block = new BlockUser()
                {
                    FromUserId = userInfo.UserId,
                    ToUserId = userId,
                    IsLock = true
                };
                var user1 = await _userApplication.GetById(userInfo.UserId);
                user1.UpdatedAt = DateTime.Now;
                _context.Users.Update(user1);
                _context.BlockUsers.Add(block);
                await _context.SaveChangesAsync();

                await _userApplication.UpdateSimilarityScores(userInfo.UserId);
                return Ok(new
                {
                    Message = "Locked"
                });
            }

            var Message = "Unlocked";

            if (!block.IsLock)
            {
                Message = "Locked";
            }

            block.IsLock = !block.IsLock;
            var user = await _userApplication.GetById(userInfo.UserId);
            user.UpdatedAt = DateTime.Now;

            _context.Follows.RemoveRange(follow);
            _context.Users.Update(user);
            _context.BlockUsers.Update(block);
            await _context.SaveChangesAsync();

            await _userApplication.UpdateSimilarityScores(userInfo.UserId);
            return Ok(new
            {
                Message = Message
            });
        }

        [HttpPut("position")]
        [Authorize]
        public async Task<IActionResult> SavePosition([FromBody] SavePositionRequest request)
        {
            try
            {
                await _userApplication.SavePostion(request);
                return Ok(new { Message = "Saved successful" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("around")]
        [Authorize]
        public async Task<IActionResult> FindAround(FindAroundRequest request)
        {
            try
            {
                var users = await _userApplication.FindAround(request);
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("friends")]
        [Authorize]
        public async Task<IActionResult> SearchFriend([FromQuery] string name)
        {
            var userInfo = _sessionService.GetDataFromToken();
            try
            {
                var users = await _userApplication.SearchFriend(userInfo.UserId, name);
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("display/{userId}")]
        public async Task<IActionResult> GetDisplayUser(Guid userId)
        {
            try
            {
                return Ok(await _userApplication.GetUserDisplayById(userId));
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}