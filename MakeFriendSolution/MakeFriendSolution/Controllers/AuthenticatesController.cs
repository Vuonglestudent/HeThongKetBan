using MakeFriendSolution.Application;
using MakeFriendSolution.Common;
using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MakeFriendSolution.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticatesController : ControllerBase
    {
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private readonly IMailchimpService _mailchimpService;
        private readonly IFeatureApplication _featureApplication;

        private LoginInfo _loginInfo = new LoginInfo();

        public AuthenticatesController(MakeFriendDbContext context, IStorageService storageService, IMailService mailService, IConfiguration config, IMailchimpService mailchimpService, IFeatureApplication featureApplication)
        {
            _context = context;
            _storageService = storageService;
            _mailService = mailService;
            _config = config;
            _mailchimpService = mailchimpService;
            _featureApplication = featureApplication;
        }

        /// <summary>
        /// Code validation khi sử dụng chức năng quên mật khẩu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("CodeValidation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CodeValidation([FromForm] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest(new
                {
                    Message = "Mật khẩu không được phép để trống."
                });
            }
            var user = await _context.Users.Where(x => x.Email == request.Email.Trim()).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "Không tìm thấy user với email: " + request.Email
                });
            }

            if (DateTime.Now > user.PasswordForgottenPeriod)
            {
                return BadRequest(new
                {
                    Message = "Mã đã hết hạn, vui lòng xác nhận lại email để nhận mã mới!"
                });
            }

            if (user.NumberOfPasswordConfirmations <= 0)
            {
                return BadRequest(new
                {
                    Message = "Số lần xác nhận đã hết, vui lòng xác nhận lại email sau " + user.PasswordForgottenPeriod.ToShortTimeString()
                });
            }

            if (user.PasswordForgottenCode != request.Code.Trim())
            {
                try
                {
                    user.NumberOfPasswordConfirmations--;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return StatusCode(500, new
                    {
                        Message = e.Message
                    });
                }

                return BadRequest(new
                {
                    Message = "Mã xác nhận không chính xác, số lần còn lại: " + user.NumberOfPasswordConfirmations
                });
            }

            user.PassWord = request.NewPassword;

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return StatusCode(501, new
                {
                    Message = e.Message
                });
            }
            return Ok(new
            {
                Message = "Mật khẩu đã được cập nhật, vui lòng đăng nhập."
            });
        }

        /// <summary>
        /// Quên mật khẩu
        /// </summary>
        /// <param name="email">Emai đăng ký tài khoản</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDisplay))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword([FromForm] string email)
        {
            var user = await _context.Users.Where(a => a.Email == email.Trim()).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new
                {
                    Message = "Không tìm thấy user với email: " + email
                });
            }

            //Trong thời hạn xác nhận mã
            if (DateTime.Now < user.PasswordForgottenPeriod)
            {
                string time = user.PasswordForgottenPeriod.ToShortTimeString();
                var UserData = this.GetForgotPasswordUserResponse(user);
                return Ok(new
                {
                    UserData = UserData,
                    Message = "Vui lòng nhập mã xác nhận trong email của bạn. Để nhận mã mới vui lòng xác nhận lại email sau " + time
                });
            }

            Random random = new Random();
            user.PasswordForgottenCode = random.Next(1000, 9999).ToString();
            user.PasswordForgottenPeriod = DateTime.Now.AddMinutes(15);
            user.NumberOfPasswordConfirmations = 3;
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return StatusCode(500, new
                {
                    Message = e.Message
                });
            }

            LoginInfo loginInfo = new LoginInfo()
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.UserName,
                Message = user.PasswordForgottenCode
            };

            MailClass mailClass = this.GetMailForgotPasswordObject(loginInfo);
            string result = await _mailService.SendMail(mailClass);

            if (result == MessageMail.MailSent)
            {
                var userData = this.GetForgotPasswordUserResponse(user);

                return Ok(new
                {
                    UserData = userData,
                    Message = "Mã xác nhận đã được gửi vào mail của bạn, nhập mã để thay đổi mật khẩu, mã sẽ hết hiệu lực sau " + user.PasswordForgottenPeriod.ToShortTimeString()
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = result
                });
            }
        }


        /// <summary>
        /// Xác nhận email đăng ký tài khoản
        /// </summary>
        /// <param name="userId">Mã người dùng</param>
        /// <returns>ActionResult</returns>
        [AllowAnonymous]
        [HttpPost("confirmMail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDisplay))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> ConfirmMail(Guid userId)
        {
            try
            {
                LoginInfo loginInfo = new LoginInfo()
                {
                    UserId = userId
                };

                loginInfo = await this.CheckRecordExistence(loginInfo);

                if (loginInfo == null)
                    return BadRequest(new
                    {
                        Message = MessageMail.InvalidUser
                    });
                else
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null)
                    {
                        return NotFound(new
                        {
                            Message = "Can not find user with id = " + userId
                        });
                    }
                    user.Status = EUserStatus.Active;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok("Xác nhận email thành công, vui lòng đăng nhập!");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Message = e.Message
                });
            }
        }

        //[AllowAnonymous]
        //[HttpPost("reconfirmMail")]
        //public async Task<IActionResult> ReconfirmMail(string username)
        //{
        //    var appUser = await _context.Users.Where(x => x.UserName == username.Trim()).FirstOrDefaultAsync();

        //    if (appUser == null)
        //    {
        //        return NotFound("Can not find user by username = " + username);
        //    }

        //    if (appUser.Status != EUserStatus.IsVerifying)
        //    {
        //        return BadRequest(new
        //        {
        //            Message = "User has been confirmed, please login"
        //        });
        //    }

        //    var info = new LoginInfo()
        //    {
        //        Email = appUser.Email,
        //        FullName = appUser.FullName,
        //        UserId = appUser.Id
        //    };

        //    MailClass mailClass = this.GetMailObject(info);
        //    string result = await _mailService.SendMail(mailClass);

        //    if (result == MessageMail.MailSent)
        //    {
        //        return Ok(new
        //        {
        //            Message = MessageMail.VerifyMail
        //        });
        //    }
        //    else return BadRequest(new
        //    {
        //        Message = result
        //    });
        //}


        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary>
        /// <param name="request">Thông tin đăng nhập</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var user = await _context.Users
                .Where(x => x.Email == request.Email.Trim()).FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest(new
                {
                    Message = "Tên đăng nhập hoặc mật khẩu không chính xác!"
                });
            }

            if (user.TypeAccount != ETypeAccount.System)
            {
                return BadRequest(new
                {
                    Message = "Vui lòng đăng nhập với tài khoản " + user.TypeAccount.ToString()
                });
            }

            if (user.PassWord != request.Password.Trim())
            {
                return BadRequest(new
                {
                    Message = "Tên đăng nhập hoặc mật khẩu không chính xác!"
                });
            }

            if (user.Status == EUserStatus.IsVerifying)
            {
                var info = new LoginInfo()
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    IsMailConfirmed = false,
                    Message = MessageMail.VerifyMail,
                    UserId = user.Id,
                    UserName = user.UserName
                };

                return BadRequest(new
                {
                    Message = MessageMail.VerifyMail
                });
            }

            if (user.Status == EUserStatus.Inactive)
            {
                return BadRequest(new
                {
                    Message = "Tài khoản của bạn đã bị khóa bởi quản trị viên!"
                });
            }

            if(!await _featureApplication.CheckUserFeature(user.Id))
            {
                user.IsInfoUpdated = false;
            }

            var userResponse = new UserResponse(user, _storageService);
            var ft = await _featureApplication.GetFeatureResponseByUserId(userResponse.Id);
            userResponse.Features = ft.Item1;
            userResponse.SearchFeatures = ft.Item2;
            userResponse.Token = this.GenerateJSONWebToken(user);

            return Ok(userResponse);
        }

        /// <summary>
        /// Đăng nhập với tài khoản facebook
        /// </summary>
        /// <param name="request">Facebook login info</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("social")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult FacebookLogin([FromForm] SocialLoginRequest request)
        {
            var user = _context.Users.Where(x => x.Email == request.Email.Trim()).FirstOrDefault();

            //Email chưa được sử dụng trong hệ thống, đăng ký tài khoản mới
            if (user == null)
            {
                var newUser = new AppUser()
                {
                    NumberOfFiends = 0,
                    NumberOfImages = 0,
                    NumberOfLikes = 0,
                    Email = request.Email,
                    Role = ERole.User,
                    FullName = request.FullName,
                    UserName = request.Email,
                    AvatarPath = request.Avatar,
                    IsInfoUpdated = false,
                    PassWord = Guid.NewGuid().ToString()
                };

                switch (request.Provider.ToLower())
                {
                    case "facebook":
                        newUser.TypeAccount = ETypeAccount.Facebook;
                        break;

                    case "google":
                        newUser.TypeAccount = ETypeAccount.Google;
                        break;

                    default:
                        return BadRequest(new { Message = "Provider is not correct" });
                }

                var mailchimp = new MailChimpModel()
                {
                    Email = request.Email,
                    Name = request.FullName
                };

                _mailchimpService.Subscribe(mailchimp);

                try
                {
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    var response = new UserResponse(newUser, _storageService);
                    response.Token = this.GenerateJSONWebToken(newUser);
                    return Ok(response);
                }
                catch (Exception e)
                {
                    return BadRequest(new
                    {
                        Message = e.Message
                    });
                }
            }
            else // Email đã có trong hệ thống, tiến hành đăng nhập
            {
                if (user.Status == EUserStatus.Inactive)
                {
                    return BadRequest(new
                    {
                        Message = "Tài khoản của bạn đã bị khóa bởi quản trị viên!"
                    });
                }

                var userResponse = new UserResponse(user, _storageService);

                userResponse.Token = this.GenerateJSONWebToken(user);

                //_sessionService.SetSessionUser(user);

                return Ok(userResponse);
            }
        }



        /// <summary>
        /// Đăng ký tài khoản hệ thống
        /// </summary>
        /// <param name="signUpRequest">Thông tin đăng ký</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signUp")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> SignUp([FromForm] SignUpSystemRequest signUpRequest)
        {
            _loginInfo = new LoginInfo();
            LoginInfo loginInfo = new LoginInfo();
            var request = new LoginInfo()
            {
                Email = signUpRequest.Email,
                FullName = signUpRequest.FullName,
                Password = signUpRequest.Password,
                UserName = signUpRequest.Email
            };
            loginInfo = await this.CheckRecordExistence(request);
            _loginInfo = loginInfo;
            if (loginInfo == null)
            {
                var user = new AppUser()
                {
                    CreatedAt = DateTime.Now,
                    Email = request.Email,
                    UserName = request.UserName,
                    FullName = request.FullName,
                    PassWord = request.Password,
                    Role = ERole.User,
                    Status = EUserStatus.IsVerifying,
                    AvatarPath = "image.png"
                };
                //var mailchimp = new MailChimpModel()
                //{
                //    Email = request.Email,
                //    Name = request.FullName
                //};


                try
                {
                    //await _mailchimpService.Subscribe(mailchimp);
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(new
                    {
                        Message = e.InnerException
                    });
                }
                _loginInfo = new LoginInfo()
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    IsMailConfirmed = false,
                    Message = MessageMail.Success,
                    UserId = user.Id,
                    UserName = user.UserName
                };
            }

            if (_loginInfo.Message == MessageMail.UserAlreadyCreated)
            {
                return BadRequest(new
                {
                    Message = _loginInfo.Message
                });
            }

            if (_loginInfo.Message == MessageMail.VerifyMail)
            {
                MailClass mailClass = this.GetMailObject(_loginInfo);
                await _mailService.SendMail(mailClass);

                return BadRequest(new
                {
                    Message = MessageMail.VerifyMail
                });
            }

            var message = "";
            if (_loginInfo.Message == MessageMail.Success)
            {
                MailClass mailClass = this.GetMailObject(_loginInfo);
                message = await _mailService.SendMail(mailClass);
            }

            if (message != MessageMail.MailSent)
                return BadRequest(new
                {
                    Message = message
                });
            else
            {
                return Ok(new
                {
                    Message = MessageMail.UserCreatedVerifyMail
                });
            }
        }


        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Logout()
        {
            //_sessionService.Logout();
            return Ok("logged out");
        }

        /// <summary>
        /// Validate token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("validateToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CheckToken()
        {
            return Ok();
        }

        private async Task<LoginInfo> CheckRecordExistence(LoginInfo info)
        {
            LoginInfo loginInfo = null;

            loginInfo = await this.GetLoginUser(info);

            if (loginInfo != null)
            {
                if (!loginInfo.IsMailConfirmed)
                {
                    loginInfo.Message = MessageMail.VerifyMail;
                }
                else
                {
                    loginInfo.Message = MessageMail.UserAlreadyCreated;
                }
            }

            return loginInfo;
        }

        private Object GetForgotPasswordUserResponse(AppUser user)
        {
            var avatarPath = _storageService.GetFileUrl(user.AvatarPath);
            return new
            {
                Email = user.Email,
                FullName = user.FullName,
                AvatarPath = avatarPath,
            };
        }

        private MailClass GetMailForgotPasswordObject(LoginInfo userInfo)
        {
            MailClass mailClass = new MailClass();
            mailClass.Subject = "Mail Confirmation";
            mailClass.Body = _mailService.GetMailBodyToForgotPassword(userInfo);
            mailClass.ToMails = new List<string>()
            {
                userInfo.Email
            };

            return mailClass;
        }

        private string GenerateJSONWebToken(AppUser userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, userInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, userInfo.FullName),
                new Claim(JwtRegisteredClaimNames.NameId, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: credentials
            );

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        private MailClass GetMailObject(LoginInfo userInfo)
        {
            MailClass mailClass = new MailClass();
            mailClass.Subject = "Mail Confirmation";
            mailClass.Body = _mailService.GetMailBody(userInfo);
            mailClass.ToMails = new List<string>()
            {
                userInfo.Email
            };

            return mailClass;
        }

        private async Task<LoginInfo> GetLoginUser(LoginInfo info)
        {
            AppUser user = new AppUser();

            if (!string.IsNullOrEmpty(info.Email))
            {
                user = await _context.Users.Where(x => x.Email == info.Email.Trim()).FirstOrDefaultAsync();
            }
            else if (!string.IsNullOrEmpty(info.UserName))
            {
                user = await _context.Users.Where(x => x.UserName == info.UserName.Trim()).FirstOrDefaultAsync();
            }
            else
            {
                user = await _context.Users.FindAsync(info.UserId);
            }

            if (user == null)
                return null;

            var loginInfo = new LoginInfo()
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.UserName
            };

            loginInfo.IsMailConfirmed = true;

            if (user.Status == EUserStatus.IsVerifying)
                loginInfo.IsMailConfirmed = false;

            return loginInfo;
        }
    }
}