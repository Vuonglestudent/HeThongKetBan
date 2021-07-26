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

            //Random Nu 40
            for (int i = 0; i < 200; i++)
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
                user.TypeAccount = RandomEnumValue<ETypeAccount>();
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(3, 99);

                user.AvatarPath = "women/" + random.Next(101, 300) + ".jpg";

                int day = random.Next(1, 29);
                int month = random.Next(1, 13);
                int year = random.Next(1970, 2007);

                int createdDay = random.Next(1, 29);
                int createdMonth = random.Next(1, 13);
                int createdYear = random.Next(2020, 2022);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);

                user.CreatedAt = createdDate;
                user.Dob = dob.Date;

                users.Add(user);
            }
            //Random Nam 50
            for (int i = 0; i < 185; i++)
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
                user.TypeAccount = RandomEnumValue< ETypeAccount>();
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(3, 99);

                user.AvatarPath = "men/" + random.Next(1, 100) + ".jpg";

                int day = random.Next(1, 29);
                int month = random.Next(1, 13);
                int year = random.Next(1970, 2007);

                int createdDay = random.Next(1, 29);
                int createdMonth = random.Next(1, 13);
                int createdYear = random.Next(2020, 2022);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);
                user.Dob = dob.Date;
                user.CreatedAt = createdDate.Date;

                users.Add(user);
            }

            ////Random Nu 80
            //for (int i = 0; i < 20; i++)
            //{
            //    var user = new AppUser
            //    {
            //        FullName = ho[random.Next(0, hoSize)] + " " + tenNu[random.Next(0, tenNuSize)],
            //        Gender = EGender.Nữ,

            //        Email = (gmailCount++).ToString() + "@gmail.com",
            //        FindPeople = RandomEnumValue<EGender>(),
            //        Height = random.Next(145, 180),
            //        Weight = random.Next(30, 70),
            //        IsInfoUpdated = true,
            //        Job = RandomEnumValue<EJob>(),
            //        Location = RandomEnumValue<ELocation>(),
            //        PassWord = "1111",
            //        PhoneNumber = "+84" + (random.Next(100000000, 999999999)),
            //        Role = ERole.User,
            //        Status = EUserStatus.Active
            //    };
            //    user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
            //    user.Title = "Kết bạn với " + user.FullName + " nhé!";
            //    user.TypeAccount = ETypeAccount.System;
            //    user.UserName = user.Email;
            //    user.NumberOfLikes = random.Next(3, 99);

            //    user.AvatarPath = "women/" + random.Next(101, 300) + ".jpg";

            //    int day = random.Next(1, 29);
            //    int month = random.Next(1, 13);
            //    int year = random.Next(1970, 2007);

            //    int createdDay = random.Next(1, 29);
            //    int createdMonth = random.Next(1, 13);
            //    int createdYear = random.Next(2020, 2021);

            //    var dob = new DateTime(year, month, day);
            //    var createdDate = new DateTime(createdYear, createdMonth, createdDay);

            //    user.CreatedAt = createdDate;
            //    user.Dob = dob.Date;

            //    users.Add(user);
            //}
            ////Random Nam 90
            //for (int i = 0; i < 10; i++)
            //{
            //    var user = new AppUser
            //    {
            //        FullName = ho[random.Next(0, hoSize)] + " " + tenNam[random.Next(0, tenNamSize)],
            //        Gender = EGender.Nam,

            //        Email = (gmailCount++).ToString() + "@gmail.com",
            //        FindPeople = RandomEnumValue<EGender>(),
            //        Height = random.Next(150, 200),
            //        Weight = random.Next(40, 80),
            //        IsInfoUpdated = true,
            //        Job = RandomEnumValue<EJob>(),
            //        Location = RandomEnumValue<ELocation>(),
            //        PassWord = "1111",
            //        PhoneNumber = "+84" + (random.Next(100000000, 999999999)),
            //        Role = ERole.User,
            //        Status = EUserStatus.Active
            //    };
            //    user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
            //    user.Title = "Kết bạn với " + user.FullName + " nhé!";
            //    user.TypeAccount = ETypeAccount.System;
            //    user.UserName = user.Email;
            //    user.NumberOfLikes = random.Next(3, 99);

            //    user.AvatarPath = "men/" + random.Next(1, 100) + ".jpg";

            //    int day = random.Next(1, 29);
            //    int month = random.Next(1, 13);
            //    int year = random.Next(1970, 2007);

            //    int createdDay = random.Next(1, 29);
            //    int createdMonth = random.Next(1, 13);
            //    int createdYear = random.Next(2020, 2021);

            //    var dob = new DateTime(year, month, day);
            //    var createdDate = new DateTime(createdYear, createdMonth, createdDay);
            //    user.Dob = dob.Date;
            //    user.CreatedAt = createdDate.Date;

            //    users.Add(user);
            //}

            //Random InActive Account 36
            for (int i = 0; i < 9; i++)
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
                    Status = EUserStatus.Inactive
                };
                user.Summary = "Mình là " + user.FullName + ", kết bạn với mình nhé!";
                user.Title = "Kết bạn với " + user.FullName + " nhé!";
                user.TypeAccount = RandomEnumValue<ETypeAccount>();
                user.UserName = user.Email;
                user.NumberOfLikes = random.Next(3, 99);

                while (user.Gender == EGender.Tất_Cả)
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
                int createdYear = random.Next(2020, 2021);

                var dob = new DateTime(year, month, day);
                var createdDate = new DateTime(createdYear, createdMonth, createdDay);

                user.CreatedAt = createdDate;
                user.Dob = dob.Date;

                users.Add(user);
            }

            foreach (var item in users)
            {
                _context.Users.Add(item);
            }
            _context.SaveChanges();


            //_context.Users.AddRange(users);
            //_context.SaveChanges();
            //users = _context.Users.ToList();


            foreach (var item in users)
            {
                await GenData(item);
            }
            return Ok("Done");
        }

        [AllowAnonymous]
        [HttpGet("statistic")]
        public IActionResult GenStatistic()
        {
            Random random = new Random();
            var fromDate = new DateTime(2021, 1, 1);
            var toDate = new DateTime(2021, 9, 30);
            List<Access> accesses = new List<Access>();

            while (toDate > fromDate)
            {
                var access = new Access
                {
                    AuthorizeCount = random.Next(100, 230),
                    UnauthorizeCount = random.Next(20, 100),
                    Date = fromDate.Date
                };
                fromDate = fromDate.AddDays(1);
                accesses.Add(access);
            }

            _context.Accesses.AddRange(accesses);
            _context.SaveChanges();
            return Ok("Done");
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

            return Ok(new
            {
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

            user = _userApplication.BidingUserRequest(user, request);
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
            if (requests.Count == 0)
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

            var users = data.OrderByDescending(x=>x.CreatedAt)
            .Skip((pagingRequest.PageIndex - 1) * pagingRequest.PageSize)
            .Take(pagingRequest.PageSize).ToList();

            var response = await _userApplication.GetUserDisplayByUserResponse(users);

            return Ok(new
            {
                data = response,
                pageTotal
            });
        }

        private async Task<List<UserResponse>> FilterFeatures(List<FilterFeaturesRequest> requests)
        {
            List<EGender> genders = new List<EGender>();
            List<ELocation> locations = new List<ELocation>();
            List<EJob> jobs = new List<EJob>();
            List<EAgeGroup> ageGroups = new List<EAgeGroup>();
            List<Filter> filters = new List<Filter>();

            foreach (FilterFeaturesRequest item in requests)
            {
                switch (item.FeatureId)
                {

                    case -1:
                        ageGroups.Add((EAgeGroup)item.ValueId);
                        break;
                    case -2:
                        genders.Add((EGender)item.ValueId);
                        break;

                    default:
                        if (!filters.Any(x => x.FeatureId == item.FeatureId))
                            filters.Add(new Filter() { FeatureId = item.FeatureId, ValueId = new List<int>() });

                        filters.First(x => x.FeatureId == item.FeatureId).ValueId.Add(item.ValueId);

                        break;
                }

            }

            var us = await _context.Users.Where(x => x.Status == EUserStatus.Active && x.IsInfoUpdated).ToListAsync();
            List<AppUser> filterUsers = new List<AppUser>();

            if (genders.Count > 0)
            {
                foreach (EGender item in genders)
                {
                    var filter = us.Where(x => x.Gender == item).ToList();
                    filterUsers = filterUsers.Concat(filter)
                                        .ToList();
                }
                us = filterUsers;
                filterUsers = new List<AppUser>();
            }


            if (ageGroups.Count > 0)
            {
                foreach (EAgeGroup item in ageGroups)
                {
                    var filter = us.Where(x => _userApplication.GetAgeGroup(x.Dob) == item).ToList();
                    filterUsers = filterUsers.Concat(filter)
                                        .ToList();
                }
                us = filterUsers;
                filterUsers = new List<AppUser>();
            }


            var users = (from i in us
                                select new UserResponse(i, _storageService)
                                ).ToList();

            foreach (var item in users)
            {
                var features = await _featureApplication.GetFeatureResponseByUserId(item.Id);
                item.Features = features.Item1;
                //item.SearchFeatures = features.Item2;
            }

            foreach (var i in filters)
            {
                var fUsers = new List<UserResponse>();
                foreach (var j in i.ValueId)
                {
                    foreach (var t in users)
                    {
                        foreach (var y in t.Features)
                        {
                            if (y.FeatureId == i.FeatureId && y.FeatureDetailId == j)
                                fUsers.Add(t);
                        }
                    }
                }

                users = users.Intersect(fUsers).ToList();
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

                Gender,
                FindPeople,
                Job,
                Location,
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

    public class Filter
    {
        public int FeatureId { get; set; }
        public List<int> ValueId { get; set; }
    }
}