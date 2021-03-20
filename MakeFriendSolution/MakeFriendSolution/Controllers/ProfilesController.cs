using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Common;
using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using MakeFriendSolution.Application;
using Microsoft.AspNetCore.Http;

namespace MakeFriendSolution.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IUserApplication _userApplication;
        private readonly IFeatureApplication _featureApplication;

        public ProfilesController(MakeFriendDbContext context, IStorageService storageService, IUserApplication userApplication, IFeatureApplication featureApplication)
        {
            _context = context;
            _storageService = storageService;
            _userApplication = userApplication;
            _featureApplication = featureApplication;
        }
        
        /// <summary>
        /// Generate dữ liệu giả phục vụ demo
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateData()
        {
            var tenNam = new List<string>();
            var tenNu = new List<string>();
            var ho = Constain.ho;

            foreach (var item in Constain.tenNam)
            {
                var trim = item.Trim();
                if (trim != "")
                {
                    tenNam.Add(trim);
                }
            }

            foreach (var item in Constain.tenNu)
            {
                var trim = item.Trim();
                if (trim != "")
                {
                    tenNu.Add(trim);
                }
            }

            List<AppUser> users = new List<AppUser>();
            int hoSize = ho.Count;
            int tenNamSize = tenNam.Count;
            int tenNuSize = tenNu.Count;
            Random random = new Random();
            int gmailCount = 1;
            //Random Nu
            for (int i = 0; i < 40; i++)
            {
                var user = new AppUser
                {
                    FullName = ho[random.Next(0, hoSize)] + " " + tenNu[random.Next(0, tenNuSize)],
                    Gender = EGender.Nữ,

                    Email = (gmailCount++).ToString() + "@gmail.com",
                    FindPeople = RandomEnumValue<EGender>(),
                    Height = random.Next(145, 180),
                    Weight = random.Next(30, 70),
                    IsInfoUpdated = true,
                    Job = RandomEnumValue<EJob>(),
                    Location = RandomEnumValue<ELocation>(),
                    PassWord = "1111",
                    PhoneNumber = "+84" + (random.Next(100000000, 999999999)),
                    Role = ERole.User,
                    Status = EUserStatus.Active
                };
                user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
                user.Title = "Kết bạn với " + user.FullName + " nhé!";
                user.TypeAccount = ETypeAccount.Facebook;
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(10, 10000);

                user.AvatarPath = "women/" + random.Next(101, 300) + ".jpg";

                int day = random.Next(1, 29);
                int month = random.Next(1, 13);
                int year = random.Next(1970, 2007);

                int createdDay = random.Next(1, 29);
                int createdMonth = random.Next(1, 13);
                int createdYear = random.Next(2018, 2021);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);

                user.CreatedAt = createdDate;
                user.Dob = dob.Date;

                users.Add(user);
            }
            //Random Nam
            for (int i = 0; i < 50; i++)
            {
                var user = new AppUser
                {
                    FullName = ho[random.Next(0, hoSize)] + " " + tenNam[random.Next(0, tenNamSize)],
                    Gender = EGender.Nam,

                    Email = (gmailCount++).ToString() + "@gmail.com",
                    FindPeople = RandomEnumValue<EGender>(),
                    Height = random.Next(150, 200),
                    Weight = random.Next(40, 80),
                    IsInfoUpdated = true,
                    Job = RandomEnumValue<EJob>(),
                    Location = RandomEnumValue<ELocation>(),
                    PassWord = "1111",
                    PhoneNumber = "+84" + (random.Next(100000000, 999999999)),
                    Role = ERole.User,
                    Status = EUserStatus.Active
                };
                user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
                user.Title = "Kết bạn với " + user.FullName + " nhé!";
                user.TypeAccount = ETypeAccount.Facebook;
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(10, 10000);

                user.AvatarPath = "men/" + random.Next(1, 100) + ".jpg";

                int day = random.Next(1, 29);
                int month = random.Next(1, 13);
                int year = random.Next(1970, 2007);

                int createdDay = random.Next(1, 29);
                int createdMonth = random.Next(1, 13);
                int createdYear = random.Next(2018, 2022);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);
                user.Dob = dob.Date;
                user.CreatedAt = createdDate.Date;

                users.Add(user);
            }

            //Random Nu
            for (int i = 0; i < 80; i++)
            {
                var user = new AppUser
                {
                    FullName = ho[random.Next(0, hoSize)] + " " + tenNu[random.Next(0, tenNuSize)],
                    Gender = EGender.Nữ,

                    Email = (gmailCount++).ToString() + "@gmail.com",
                    FindPeople = RandomEnumValue<EGender>(),
                    Height = random.Next(145, 180),
                    Weight = random.Next(30, 70),
                    IsInfoUpdated = true,
                    Job = RandomEnumValue<EJob>(),
                    Location = RandomEnumValue<ELocation>(),
                    PassWord = "1111",
                    PhoneNumber = "+84" + (random.Next(100000000, 999999999)),
                    Role = ERole.User,
                    Status = EUserStatus.Active
                };
                user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
                user.Title = "Kết bạn với " + user.FullName + " nhé!";
                user.TypeAccount = ETypeAccount.System;
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(10, 10000);

                user.AvatarPath = "women/" + random.Next(101, 300) + ".jpg";

                int day = random.Next(1, 29);
                int month = random.Next(1, 13);
                int year = random.Next(1970, 2007);

                int createdDay = random.Next(1, 29);
                int createdMonth = random.Next(1, 13);
                int createdYear = random.Next(2018, 2021);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);

                user.CreatedAt = createdDate;
                user.Dob = dob.Date;

                users.Add(user);
            }
            //Random Nam
            for (int i = 0; i < 90; i++)
            {
                var user = new AppUser
                {
                    FullName = ho[random.Next(0, hoSize)] + " " + tenNam[random.Next(0, tenNamSize)],
                    Gender = EGender.Nam,

                    Email = (gmailCount++).ToString() + "@gmail.com",
                    FindPeople = RandomEnumValue<EGender>(),
                    Height = random.Next(150, 200),
                    Weight = random.Next(40, 80),
                    IsInfoUpdated = true,
                    Job = RandomEnumValue<EJob>(),
                    Location = RandomEnumValue<ELocation>(),
                    PassWord = "1111",
                    PhoneNumber = "+84" + (random.Next(100000000, 999999999)),
                    Role = ERole.User,
                    Status = EUserStatus.Active
                };
                user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
                user.Title = "Kết bạn với " + user.FullName + " nhé!";
                user.TypeAccount = ETypeAccount.System;
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(10, 10000);

                user.AvatarPath = "men/" + random.Next(1, 100) + ".jpg";

                int day = random.Next(1, 29);
                int month = random.Next(1, 13);
                int year = random.Next(1970, 2007);

                int createdDay = random.Next(1, 29);
                int createdMonth = random.Next(1, 13);
                int createdYear = random.Next(2018, 2021);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);
                user.Dob = dob.Date;
                user.CreatedAt = createdDate.Date;

                users.Add(user);
            }

            //Random InActive Account
            for (int i = 0; i < 36; i++)
            {
                var user = new AppUser
                {
                    FullName = ho[random.Next(0, hoSize)] + " " + tenNu[random.Next(0, tenNuSize)],
                    Gender = RandomEnumValue<EGender>(),

                    Email = (gmailCount++).ToString() + "@gmail.com",
                    FindPeople = RandomEnumValue<EGender>(),
                    Height = random.Next(145, 180),
                    Weight = random.Next(30, 70),
                    IsInfoUpdated = true,
                    Job = RandomEnumValue<EJob>(),
                    Location = RandomEnumValue<ELocation>(),
                    PassWord = "1111",
                    PhoneNumber = "+84" + (random.Next(100000000, 999999999)),
                    Role = ERole.User,
                    Status = RandomEnumValue<EUserStatus>()
                };
                user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
                user.Title = "Kết bạn với " + user.FullName + " nhé!";
                user.TypeAccount = ETypeAccount.Google;
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(10, 10000);

                while(user.Gender == EGender.Tất_Cả)
                {
                    user.Gender = RandomEnumValue<EGender>();
                }

                if (user.Gender == EGender.Nữ)
                {
                    user.AvatarPath = "women/" + random.Next(101, 300) + ".jpg";
                }
                else
                {
                    user.AvatarPath = "men/" + random.Next(1, 100) + ".jpg";
                }

                int day = random.Next(1, 29);
                int month = random.Next(1, 13);
                int year = random.Next(1970, 2007);

                int createdDay = random.Next(1, 29);
                int createdMonth = random.Next(1, 13);
                int createdYear = random.Next(2018, 2021);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);

                user.CreatedAt = createdDate;
                user.Dob = dob.Date;

                users.Add(user);
            }

            //var response = new List<UserResponse>();
            //foreach (var item in users)
            //{
            //    var u = new UserResponse(item, _storageService);
            //    response.Add(u);
            //}
            var fromDate = new DateTime(2018, 1, 1);
            var toDate = new DateTime(2021, 12, 30);
            List<Access> accesses = new List<Access>();

            while (toDate > fromDate)
            {
                var access = new Access
                {
                    AuthorizeCount = random.Next(500, 2000),
                    UnauthorizeCount = random.Next(200, 800),
                    Date = fromDate.Date
                };
                fromDate = fromDate.AddDays(1);
                accesses.Add(access);
            }

            _context.Accesses.AddRange(accesses);
            _context.Users.AddRange(users);
            _context.SaveChanges();
            var newUsers = _context.Users.ToList();

            foreach (var item in newUsers)
            {
                await GenData(item);
            }
            return Ok();
        }

        private static T RandomEnumValue<T>()
        {
            Random random = new Random();
            var values = Enum.GetValues(typeof(T));
            int num = random.Next(0, values.Length);
            return (T)values.GetValue(num);
        }


        /// <summary>
        /// Lấy danh sách bạn gợi ý
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <param name="filter">Các filter feature</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("similar/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDisplay>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetListOfSuggestions(Guid userId, [FromQuery] FilterUserViewModel filter)
        {
            var result = await _userApplication.GetSimilarityUsers(userId, filter);

            return Ok(new {
                data = result.Item1,
                pageTotal = result.Item2
            });
        }

        /// <summary>
        /// Cập nhật thông tin cá nhân và hồ sơ
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UserRequest request)
        {
            var user = await _userApplication.GetById(request.Id);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "Can not find User with Id = " + request.Id
                });
            }

            user = await _userApplication.BidingUserRequest(user, request);
            try
            {
                user.IsInfoUpdated = true;
                user = await _userApplication.UpdateUser(user, true);
                bool isUpdateHaveFeature = await _featureApplication.UpdateHaveFeatures(request.Features, request.Id);
                bool isUpdateSearchFeature = await _featureApplication.UpdateSearchFeatures(request.SearchFeatures, request.Id);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest(new
                {
                    Message = e.InnerException
                });
            }

            var response = new UserResponse(user, _storageService);

            return Ok(response);
        }

        /// <summary>
        /// Filter người dùng
        /// </summary>
        /// <param name="pagingRequest">Phân trang</param>
        /// <param name="requests">Các feature lọc</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("filterFeatures")]
        public async Task<IActionResult> FilterFeatures([FromQuery] PagingRequest pagingRequest, [FromBody] List<FilterFeaturesRequest> requests)
        {
            if(requests.Count == 0)
            {
                var usersNotFilter = await _context.Users
                    .Where(x => x.Status == EUserStatus.Active && x.IsInfoUpdated)
                    .ToListAsync();

                var pageCount = usersNotFilter.Count / pagingRequest.PageSize;

                usersNotFilter = usersNotFilter
                .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
                .Take(pagingRequest.PageSize).ToList();

                var displayUsers = await _userApplication.GetUserDisplay(usersNotFilter);

                return Ok(new
                {
                    data = displayUsers,
                    pageTotal = pageCount
                });

            }
            var data = await FilterFeatures(requests);

            var pageTotal = data.Count / pagingRequest.PageSize;

            var users = data
            .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
            .Take(pagingRequest.PageSize).ToList();

            var response = await _userApplication.GetUserDisplay(users);

            return Ok(new
            {
                data = response,
                pageTotal
            });
        }

        private async Task<List<AppUser>> FilterFeatures(List<FilterFeaturesRequest> requests)
        {
            //List<EBody> bodies = new List<EBody>();
            List<EGender> genders = new List<EGender>();
            //List<EEducation> educations = new List<EEducation>();
            //List<EReligion> religions = new List<EReligion>();
            //List<ECook> cooks = new List<ECook>();
            //List<ELikeTechnology> likeTechnologies = new List<ELikeTechnology>();
            //List<ELikePet> likePets = new List<ELikePet>();
            //List<EPlaySport> playSports = new List<EPlaySport>();
            //List<ETravel> travels = new List<ETravel>();
            //List<EGame> games = new List<EGame>();
            //List<EShopping> shoppings = new List<EShopping>();
            List<ELocation> locations = new List<ELocation>();
            //List<ECharacter> characters = new List<ECharacter>();
            ////
            //List<EFavoriteMovie> favoriteMovies = new List<EFavoriteMovie>();
            //List<EAtmosphereLike> atmosphereLikes = new List<EAtmosphereLike>();
            //List<EDrinkBeer> drinkBeers = new List<EDrinkBeer>();
            //List<ESmoking> smokings = new List<ESmoking>();
            //List<EMarriage> marriages = new List<EMarriage>();
            List<EJob> jobs = new List<EJob>();
            List<EAgeGroup> ageGroups = new List<EAgeGroup>();

            foreach (FilterFeaturesRequest item in requests)
            {
                switch (item.feature)
                {
                    //case "body":
                    //    if (!bodies.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EBody body))
                    //        {
                    //            bodies.Add(body);
                    //        }
                    //    }
                    //    break;

                    case "gender":
                        if (!genders.Any(x => x.ToString() == item.display))
                        {
                            if (Enum.TryParse(item.display.Trim(), out EGender gender))
                            {
                                genders.Add(gender);
                            }
                        }
                        break;
                    //case "education":
                    //    if (!educations.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EEducation education))
                    //        {
                    //            educations.Add(education);
                    //        }
                    //    }
                    //    break;
                    //case "religion":
                    //    if (!religions.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EReligion religion))
                    //        {
                    //            religions.Add(religion);
                    //        }
                    //    }
                    //    break;
                    //case "cook":
                    //    if (!cooks.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out ECook cook))
                    //        {
                    //            cooks.Add(cook);
                    //        }
                    //    }
                    //    break;
                    //case "likeTechnology":
                    //    if (!likeTechnologies.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out ELikeTechnology likeTechnology))
                    //        {
                    //            likeTechnologies.Add(likeTechnology);
                    //        }
                    //    }
                    //    break;
                    //case "likePet":
                    //    if (!likePets.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out ELikePet likePet))
                    //        {
                    //            likePets.Add(likePet);
                    //        }
                    //    }
                    //    break;
                    //case "playSport":
                    //    if (!playSports.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EPlaySport playSport))
                    //        {
                    //            playSports.Add(playSport);
                    //        }
                    //    }
                    //    break;
                    //case "travel":
                    //    if (!travels.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out ETravel travel))
                    //        {
                    //            travels.Add(travel);
                    //        }
                    //    }
                    //    break;
                    //case "game":
                    //    if (!games.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EGame game))
                    //        {
                    //            games.Add(game);
                    //        }
                    //    }
                    //    break;
                    //case "shopping":
                    //    if (!shoppings.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EShopping shopping))
                    //        {
                    //            shoppings.Add(shopping);
                    //        }
                    //    }
                    //    break;
                    case "location":
                        if (!locations.Any(x => x.ToString() == item.display))
                        {
                            if (Enum.TryParse(item.display.Trim(), out ELocation location))
                            {
                                locations.Add(location);
                            }
                        }
                        break;
                    //case "character":
                    //    if (!characters.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out ECharacter character))
                    //        {
                    //            characters.Add(character);
                    //        }
                    //    }
                    //    break;

                    //    //
                    //case "favoriteMovie":
                    //    if (!favoriteMovies.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EFavoriteMovie favoriteMovie))
                    //        {
                    //            favoriteMovies.Add(favoriteMovie);
                    //        }
                    //    }
                    //    break;
                    //case "atmosphereLike":
                    //    if (!atmosphereLikes.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EAtmosphereLike atmosphereLike))
                    //        {
                    //            atmosphereLikes.Add(atmosphereLike);
                    //        }
                    //    }
                    //    break;
                    //case "drinkBeer":
                    //    if (!drinkBeers.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EDrinkBeer drinkBeer))
                    //        {
                    //            drinkBeers.Add(drinkBeer);
                    //        }
                    //    }
                    //    break;
                    //case "smoking":
                    //    if (!smokings.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out ESmoking smoking))
                    //        {
                    //            smokings.Add(smoking);
                    //        }
                    //    }
                    //    break;
                    //case "marriage":
                    //    if (!marriages.Any(x => x.ToString() == item.display))
                    //    {
                    //        if (Enum.TryParse(item.display.Trim(), out EMarriage marriage))
                    //        {
                    //            marriages.Add(marriage);
                    //        }
                    //    }
                    //    break;
                    case "job":
                        if (!jobs.Any(x => x.ToString() == item.display))
                        {
                            if (Enum.TryParse(item.display.Trim(), out EJob job))
                            {
                                jobs.Add(job);
                            }
                        }
                        break;
                    case "ageGroup":
                        if (!ageGroups.Any(x => x.ToString() == item.display))
                        {
                            if (Enum.TryParse(item.display.Trim(), out EAgeGroup ageGroup))
                            {
                                ageGroups.Add(ageGroup);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }

            var users = await _context.Users.Where(x => x.Status == EUserStatus.Active && x.IsInfoUpdated).ToListAsync(); 
            List<AppUser> filterUsers = new List<AppUser>();
            //

            if (jobs.Count > 0)
            {
                foreach (EJob item in jobs)
                {
                    var filter = users.Where(x => x.Job == item).ToList();
                    filterUsers = filterUsers.Concat(filter)
                                        .ToList();
                }
                users = filterUsers;
                filterUsers = new List<AppUser>();
            }
            //


            //if (marriages.Count > 0)
            //{
            //    foreach (EMarriage item in marriages)
            //    {
            //        var filter = users.Where(x => x.Marriage == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}
            ////


            //if (smokings.Count > 0)
            //{
            //    foreach (ESmoking item in smokings)
            //    {
            //        var filter = users.Where(x => x.Smoking == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}
            ////

            //if (drinkBeers.Count > 0)
            //{
            //    foreach (EDrinkBeer item in drinkBeers)
            //    {
            //        var filter = users.Where(x => x.DrinkBeer == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}
            ////

            //if (atmosphereLikes.Count > 0)
            //{
            //    foreach (EAtmosphereLike item in atmosphereLikes)
            //    {
            //        var filter = users.Where(x => x.AtmosphereLike == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}
            ////

            //if (favoriteMovies.Count > 0)
            //{
            //    foreach (EFavoriteMovie item in favoriteMovies)
            //    {
            //        var filter = users.Where(x => x.FavoriteMovie == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}
            ////

            ////
            //if (bodies.Count > 0)
            //{
            //    foreach (EBody item in bodies)
            //    {
            //        var filter = users.Where(x => x.Body == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}
            //
            if(genders.Count > 0)
            {
                foreach (EGender item in genders)
                {
                    var filter = users.Where(x => x.Gender == item).ToList();
                    filterUsers = filterUsers.Concat(filter)
                                        .ToList();
                }
                users = filterUsers;
                filterUsers = new List<AppUser>();
            }

            //
            //if(educations.Count > 0)
            //{
            //    foreach (EEducation item in educations)
            //    {
            //        var filter = users.Where(x => x.Education == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(religions.Count > 0)
            //{
            //    foreach (EReligion item in religions)
            //    {
            //        var filter = users.Where(x => x.Religion == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(cooks.Count > 0)
            //{
            //    foreach (ECook item in cooks)
            //    {
            //        var filter = users.Where(x => x.Cook == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(likeTechnologies.Count > 0)
            //{
            //    foreach (ELikeTechnology item in likeTechnologies)
            //    {
            //        var filter = users.Where(x => x.LikeTechnology == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(likePets.Count > 0)
            //{
            //    foreach (ELikePet item in likePets)
            //    {
            //        var filter = users.Where(x => x.LikePet == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(playSports.Count > 0)
            //{
            //    foreach (EPlaySport item in playSports)
            //    {
            //        var filter = users.Where(x => x.PlaySport == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(travels.Count > 0)
            //{
            //    foreach (ETravel item in travels)
            //    {
            //        var filter = users.Where(x => x.Travel == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(games.Count > 0)
            //{
            //    foreach (EGame item in games)
            //    {
            //        var filter = users.Where(x => x.Game == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            ////
            //if(shoppings.Count > 0)
            //{
            //    foreach (EShopping item in shoppings)
            //    {
            //        var filter = users.Where(x => x.Shopping == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            //
            if(locations.Count > 0)
            {
                foreach (ELocation item in locations)
                {
                    var filter = users.Where(x => x.Location == item).ToList();
                    filterUsers = filterUsers.Concat(filter)
                                        .ToList();
                }
                users = filterUsers;
                filterUsers = new List<AppUser>();
            }

            //
            //if (characters.Count > 0)
            //{
            //    foreach (ECharacter item in characters)
            //    {
            //        var filter = users.Where(x => x.Character == item).ToList();
            //        filterUsers = filterUsers.Concat(filter)
            //                            .ToList();
            //    }
            //    users = filterUsers;
            //    filterUsers = new List<AppUser>();
            //}

            //
            if (ageGroups.Count > 0)
            {
                foreach (EAgeGroup item in ageGroups)
                {
                    var filter = users.Where(x => _userApplication.GetAgeGroup(x.Dob) == item).ToList();
                    filterUsers = filterUsers.Concat(filter)
                                        .ToList();
                }
                users = filterUsers;
                filterUsers = new List<AppUser>();
            }
            return users;
        } 

        [AllowAnonymous]
        [HttpGet("features")]
        public async Task<IActionResult> GetFeatures()
        {
            var Gender = new List<string>();
            List<EGender> Genders = Enum.GetValues(typeof(EGender))
                    .Cast<EGender>()
                    .ToList();
            foreach (var item in Genders)
            {
                Gender.Add(item.ToString());
            }

            var FindPeople = new List<string>();
            List<EGender> findPeople = Enum.GetValues(typeof(EGender))
                    .Cast<EGender>()
                    .ToList();
            foreach (var item in findPeople)
            {
                FindPeople.Add(item.ToString());
            }

            var Job = new List<string>();
            List<EJob> Jobs = Enum.GetValues(typeof(EJob))
                    .Cast<EJob>()
                    .ToList();
            foreach (var item in Jobs)
            {
                Job.Add(item.ToString());
            }

            var Location = new List<string>();
            List<ELocation> Locations = Enum.GetValues(typeof(ELocation))
                    .Cast<ELocation>()
                    .ToList();
            foreach (var item in Locations)
            {
                Location.Add(item.ToString());
            }


            //var TypeAccount = new List<string>();
            //List<ETypeAccount> TypeAccounts = Enum.GetValues(typeof(ETypeAccount))
            //        .Cast<ETypeAccount>()
            //        .ToList();
            //foreach (var item in TypeAccounts)
            //{
            //    TypeAccount.Add(item.ToString());
            //}

            //var UserStatus = new List<string>();
            //List<EUserStatus> UserStatuses = Enum.GetValues(typeof(EUserStatus))
            //        .Cast<EUserStatus>()
            //        .ToList();
            //foreach (var item in UserStatuses)
            //{
            //    UserStatus.Add(item.ToString());
            //}

            var AgeGroup = new List<string>();
            List<EAgeGroup> ageGroups = Enum.GetValues(typeof(EAgeGroup))
                    .Cast<EAgeGroup>()
                    .ToList();
            foreach (var item in ageGroups)
            {
                AgeGroup.Add(item.ToString());
            }

            var features = await _featureApplication.GetFeatures();

            var response = new
            {
                //AtmosphereLike,
                //Body,
                //Character,
                //DrinkBeer,
                //Education,
                //FavoriteMovie,
                Gender,
                FindPeople,
                Job,
                //LifeStyle,
                Location,
                //Marriage,
                //MostValuable,
                //Religion,
                //Smoking,
                //LikeTechnology,
                //LikePet,
                //PlaySport,
                //Travel,
                //Game,
                //Shopping,
                AgeGroup,
                features
            };

            return Ok(response);
        }

        private async Task GenData(AppUser user)
        {
            var random = new Random();
            var features = await _context.Features.Include(x => x.FeatureDetails).ToListAsync();
            var userFeatures = new List<UserFeature>();
            var searchFeatures = new List<SearchFeature>();
            foreach (var item in features)
            {
                var featureDetailId = item.FeatureDetails.ToList()[random.Next(item.FeatureDetails.Count)].Id;
                
                var uf = new UserFeature()
                {
                    FeatureDetailId = featureDetailId,
                    UserId = user.Id,
                    FeatureId = item.Id
                };
                userFeatures.Add(uf);

                if (item.IsSearchFeature)
                {
                    featureDetailId = item.FeatureDetails.ToList()[random.Next(item.FeatureDetails.Count)].Id;
                    var searchfeature = new SearchFeature()
                    {
                        FeatureDetailId = featureDetailId,
                        UserId = user.Id,
                        FeatureId = item.Id
                    };
                    searchFeatures.Add(searchfeature);
                }
            }
            _context.UserFeatures.AddRange(userFeatures);
            _context.SearchFeatures.AddRange(searchFeatures);
            await _context.SaveChangesAsync();
        }

        [AllowAnonymous]
        [HttpGet("testData")]
        public IActionResult TestData()
        {
            var users = _context.Users.Include(x => x.HaveFeatures).Take(2).ToList();
            return Ok(users);
        }
    }
}