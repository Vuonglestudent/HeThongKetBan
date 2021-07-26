using MakeFriendSolution.Common;
using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.Enum;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public class UserApplication : IUserApplication
    {
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private readonly ISessionService _sessionService;
        private readonly IFeatureApplication _featureApplication;

        public UserApplication()
        {
        }

        public UserApplication(MakeFriendDbContext context, IStorageService storageService, ISessionService sessionService, IFeatureApplication featureApplication)
        {
            _context = context;
            _storageService = storageService;
            _sessionService = sessionService;
            _featureApplication = featureApplication;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<bool> IsLiked(Guid userId, Guid currentUserId)
        {
            return await _context.Favorites.AnyAsync(x => x.FromUserId == currentUserId && x.ToUserId == userId);
        }

        public async Task<bool> IsFollowed(Guid userId, Guid currentUserId)
        {
            return await _context.Follows.AnyAsync(x => x.FromUserId == currentUserId && x.ToUserId == userId);
        }

        public async Task<bool> GetBlockStatus(Guid currentUserId, Guid toUserId)
        {
            return await _context.BlockUsers.AnyAsync(x => x.FromUserId == currentUserId && x.ToUserId == toUserId && x.IsLock);
        }

        public static int CalculateAge(DateTime birthDay)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDay.Year;
            if (birthDay > today.AddYears(-age))
                age--;
            return age;
        }

        public void FilterUers(ref List<AppUser> users, FilterUserViewModel filter)
        {
            if (filter.Location != null && filter.Location != "")
            {
                if (Enum.TryParse(filter.Location.Trim(), out ELocation locate))
                {
                    users = users.Where(x => x.Location == locate).ToList();
                }
            }

            if (filter.FullName != null && filter.FullName.Trim() != "")
            {
                string name = filter.FullName.NonUnicode().ToLower().Trim();
                users = users.Where(x => x.FullName.NonUnicode().ToLower().Contains(name)).ToList();
            }

            if (filter.Gender != null && filter.Gender.Trim() != "")
            {
                if (Enum.TryParse(filter.Gender.Trim(), out EGender gender))
                    users = users.Where(x => x.Gender == gender).ToList();
            }


            if (filter.AgeGroup != null && filter.AgeGroup != "")
            {
                if (Enum.TryParse(filter.AgeGroup.Trim(), out EAgeGroup ageGroup))
                {
                    users = users.Where(x => GetAgeGroup(x.Dob) == ageGroup).ToList();
                }
            }
            //if (filter.FromAge != 0)
            //{
            //    users = users.Where(x => CalculateAge(x.Dob) >= filter.FromAge).ToList();
            //}

            //if (filter.ToAge > filter.FromAge)
            //{
            //    users = users.Where(x => CalculateAge(x.Dob) <= filter.ToAge).ToList();
            //}
        }

        public async Task<List<UserDisplay>> GetUserDisplay(List<AppUser> users, bool nonImage = false)
        {
            var userDisplays = new List<UserDisplay>();
            //Get Session user - Check login
            var sessionUser = _sessionService.GetDataFromToken();
            if (sessionUser == null)
            {
                sessionUser = new LoginInfo()
                {
                    UserId = Guid.NewGuid()
                };
            }

            foreach (var user in users)
            {
                var userDisplay = new UserDisplay(user, _storageService);

                if (!nonImage)
                {
                    userDisplay.Followed = await IsFollowed(user.Id, sessionUser.UserId);
                    userDisplay.Favorited = await IsLiked(user.Id, sessionUser.UserId);
                }

                userDisplays.Add(userDisplay);
            }
            return userDisplays;
        }

        public async Task<List<UserDisplay>> GetUserDisplayByUserResponse(List<UserResponse> users)
        {
            var userDisplays = new List<UserDisplay>();
            //Get Session user - Check login
            var sessionUser = _sessionService.GetDataFromToken();
            if (sessionUser == null)
            {
                sessionUser = new LoginInfo()
                {
                    UserId = Guid.NewGuid()
                };
            }

            foreach (var user in users)
            {
                var userDisplay = new UserDisplay(user);

                userDisplay.Followed = await IsFollowed(user.Id, sessionUser.UserId);
                userDisplay.Favorited = await IsLiked(user.Id, sessionUser.UserId);

                userDisplays.Add(userDisplay);
            }
            return userDisplays;
        }

        public async Task<UserResponse> GetUserResponse(AppUser user)
        {
            var Features = await _featureApplication.GetFeatureResponseByUserId(user.Id);
            var userResponse = new UserResponse(user, _storageService)
            {
                Features = Features.Item1,
                SearchFeatures = Features.Item2
            };
            return userResponse;
        }

        public AppUser BidingUserRequest(AppUser user, UserRequest request)
        {
            if (Enum.TryParse(request.Gender, out EGender gender))
            {
                user.Gender = gender;
            }

            if (Enum.TryParse(request.FindPeople, out EGender findPeople))
            {
                user.FindPeople = findPeople;
            }

            if (Enum.TryParse(request.FindAgeGroup, out EAgeGroup findAgeGroup))
            {
                user.FindAgeGroup = findAgeGroup;
            }


            if (Enum.TryParse(request.Location, out ELocation location))
            {
                user.Location = location;
            }


            if (Enum.TryParse(request.Job, out EJob job))
            {
                user.Job = job;
            }


            if (request.PhoneNumber != "" && request.PhoneNumber != null)
            {
                user.PhoneNumber = request.PhoneNumber;
            }

            if (request.FullName != "" && request.FullName != null)
            {
                user.FullName = request.FullName;
            }

            if (request.Title != "" && request.Title != null)
            {
                user.Title = request.Title;
            }

            if (request.Summary != "" && request.Summary != null)
            {
                user.Summary = request.Summary;
            }

            if (request.Weight >= 20 && request.Weight <= 200)
            {
                user.Weight = request.Weight;
            }

            if (request.Height >= 120 && request.Height <= 200)
            {
                user.Height = request.Height;
            }

            if (CalculateAge(request.Dob) >= 10 && CalculateAge(request.Dob) <= 100)
            {
                user.Dob = request.Dob;
            }

            return user;
        }

        public EAgeGroup GetAgeGroup(DateTime birthDay)
        {
            int age = CalculateAge(birthDay);
            if (age < 18)
                return EAgeGroup.Dưới_18_tuổi;
            else if (age < 26)
                return EAgeGroup.Từ_18_đến_25;
            else if (age < 31)
                return EAgeGroup.Từ_25_đến_30;
            else if (age < 41)
                return EAgeGroup.Từ_31_đến_40;
            else if (age < 51)
                return EAgeGroup.Từ_41_đến_50;
            else return EAgeGroup.Trên_50;
        }

        public async Task<AppUser> GetById(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<AppUser> UpdateUser(AppUser user, bool isUpdateScore)
        {
            var updateScore = await _context.SimilariryFeatures.FirstOrDefaultAsync();
            if (isUpdateScore)
            {
                updateScore.UpdatedAt = DateTime.Now;
                _context.SimilariryFeatures.Update(updateScore);
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> IsExist(Guid userId)
        {
            return await _context.Users.AnyAsync(x => x.Id == userId);
        }

        public async Task<List<AppUser>> GetActiveUsers(Guid userId, bool getLock)
        {
            if (!getLock)
            {
                return await _context.Users
                .Where(x => x.Status == Models.Enum.EUserStatus.Active && x.IsInfoUpdated)
                .ToListAsync();
            }

            var blocksFrom = await _context.BlockUsers.Where(x => x.FromUserId == userId && x.IsLock).ToListAsync();
            var blocksTo = await _context.BlockUsers.Where(x => x.ToUserId == userId && x.IsLock).ToListAsync();
            List<Guid> blocksId = new List<Guid>();
            foreach (var item in blocksFrom)
            {
                blocksId.Add(item.ToUserId);
            }

            foreach (var item in blocksTo)
            {
                blocksId.Add(item.FromUserId);
            }

            var users = await _context.Users
                .Where(x => x.Status == Models.Enum.EUserStatus.Active && x.IsInfoUpdated)
                .ToListAsync();

            foreach (var item in blocksId)
            {
                users.Remove(users.Where(x => x.Id == item).FirstOrDefault());
            }

            return users;
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Favorite> GetFavoriteById(Guid fromId, Guid toId)
        {
            return await _context.Favorites
                .Where(x => x.FromUserId == fromId && x.ToUserId == toId)
                .FirstOrDefaultAsync();
        }

        public async Task<string> DisableUser(Guid userId)
        {
            string message = "Do nothing";
            var user = await _context.Users.FindAsync(userId);

            if (user.Status == EUserStatus.Active)
            {
                user.Status = EUserStatus.Inactive;
                message = "Blocked";
            }
            else if (user.Status == EUserStatus.Inactive)
            {
                user.Status = EUserStatus.Active;
                message = "Unclocked";
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> UpdateSimilarityScores(Guid userId)
        {
            var oldScores = await _context.SimilarityScores.Where(x => x.FromUserId == userId).ToListAsync();
            var features = (await _featureApplication.GetFeatures()).Where(x => x.IsCalculated).ToList();
            var columns = features.Count;

            var users = await GetUsersToCalculate(userId);

            int rows = users.Count;

            double[,] usersMatrix = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var vm = users[i].FeatureViewModels
                        .Where(x => x.FeatureId == features[j].Id)
                        .FirstOrDefault();

                    if (vm == null)
                        usersMatrix[i, j] = -1;
                    else
                    {
                        if (!vm.IsCalculated)
                        {
                            usersMatrix[i, j] = 0;
                            continue;
                        }
                        usersMatrix[i, j] = vm.Rate * vm.weight;
                    }
                }
            }

            SimilarityMatrix m = new SimilarityMatrix
            {
                Row = rows,
                Column = columns,
                Matrix = usersMatrix
            };

            List<double> kq = new List<double>();
            kq = m.SimilarityCalculate();

            users.RemoveAt(0);

            var similarityScores = new List<SimilarityScore>();

            for (int i = 1; i < kq.Count; i++)
            {
                users[i - 1].Point = kq[i];
                var score = new SimilarityScore()
                {
                    FromUserId = userId,
                    ToUserId = users[i - 1].UserId,
                    Score = kq[i]
                };
                similarityScores.Add(score);
            }
            try
            {
                _context.SimilarityScores.AddRange(similarityScores);
                _context.SimilarityScores.RemoveRange(oldScores);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Tuple<List<UserDisplay>, int>> GetSimilarityScores(Guid userId, FilterUserViewModel request)
        {
            List<UserDisplay> userResponses = new List<UserDisplay>();

            if (!request.IsFilter)
            {
                var tempScore = await _context.SimilarityScores.Where(x => x.FromUserId == userId)
                    .OrderByDescending(x => x.Score)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize).ToListAsync();

                List<AppUser> users = new List<AppUser>();
                foreach (var item in tempScore)
                {
                    var user = await GetById(item.ToUserId);
                    user.Point = item.Score;
                    users.Add(user);
                }
                userResponses = await GetUserDisplay(users);
                var total = tempScore.Count / request.PageSize;
                return Tuple.Create(userResponses, total);
            }
            else
            {
                var userInfo = await GetById(userId);
                var users = await GetActiveUsers(userId, true);
                FilterUers(ref users, request);

                var total = users.Count / request.PageSize;
                users = users.OrderByDescending(x => x.Point)
                    .OrderByDescending(x=>x.CreatedAt)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize).ToList();

                userResponses = await GetUserDisplay(users);

                return Tuple.Create(userResponses, total);
            }
        }

        public async Task<Tuple<List<UserDisplay>, int>> GetSimilarityUsers(Guid userId, FilterUserViewModel request)
        {
            var updateScore = await _context.SimilariryFeatures.FirstOrDefaultAsync();
            var user = await GetById(userId);
            if (updateScore.UpdatedAt > user.UpdatedAt)
            {
                var update = await UpdateSimilarityScores(userId);
                user.UpdatedAt = DateTime.Now;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            var result = await GetSimilarityScores(userId, request);

            return result;
        }

        public async Task<List<UserCalculateVM>> GetUsersToCalculate(Guid userId)
        {
            var blockUser = await _context.BlockUsers
                .Where(x => x.FromUserId == userId || x.ToUserId == userId).ToListAsync();

            var follow = await _context.Follows
                .Where(x => x.FromUserId == userId).ToListAsync();

            var user = await _context.Users.FindAsync(userId);

            var users = await (from u in _context.Users
                               //where (u.Gender == user.FindPeople || user.FindPeople == EGender.Tất_Cả)
                               //&& (GetAgeGroup(u.Dob) == user.FindAgeGroup || user.FindAgeGroup == EAgeGroup.Tất_cả)
                               select new UserCalculateVM()
                               {
                                   UserId = u.Id,
                                   Dob = u.Dob,
                                   Age = CalculateAge(u.Dob),
                                   Gender = u.Gender
                               }).ToListAsync();

            users = users.Where(u => (u.Gender == user.FindPeople || user.FindPeople == EGender.Tất_Cả)
                               && (GetAgeGroup(u.Dob) == user.FindAgeGroup || user.FindAgeGroup == EAgeGroup.Tất_cả)).ToList();

            List<Guid> userIds = new List<Guid>
            {
                userId
            };

            foreach (var item in blockUser)
            {
                if (item.FromUserId == userId)
                    userIds.Add(item.ToUserId);
                else
                    userIds.Add(item.FromUserId);
            }

            foreach (var item in follow)
            {
                userIds.Add(item.ToUserId);
            }

            foreach (var item in userIds)
            {
                users.Remove(users.Where(x => x.UserId == item).FirstOrDefault());
            }

            foreach (var item in users)
            {
                item.FeatureViewModels = await _featureApplication.GetFeatureViewModelByUserId(item.UserId);
            }

            //users.Add();
            var calculateUser = new UserCalculateVM()
            {
                UserId = user.Id,
                Age = CalculateAge(user.Dob),
                Gender = user.Gender
            };

            calculateUser.FeatureViewModels = await _featureApplication.GetFeatureViewModelByUserId(calculateUser.UserId);

            var searchFeatures = await _context.SearchFeatures.Where(x => x.UserId == userId).ToListAsync();

            for (int i = 0; i < calculateUser.FeatureViewModels.Count; i++)
            {
                for (int j = 0; j < searchFeatures.Count; j++)
                {
                    if (calculateUser.FeatureViewModels[i].FeatureId == searchFeatures[j].FeatureId)
                    {
                        var search = await _featureApplication.GetFeatureViewModel(searchFeatures[j].FeatureDetailId);
                        calculateUser.FeatureViewModels[i] = search;
                    }
                }
            }

            users.Insert(0, calculateUser);

            return users;
        }

        public async Task SavePostion(SavePositionRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);

            if (user == null)
                throw new Exception("Can not find user with id = " + request.UserId);

            user.Latitude = request.Latitude;
            user.Longitude = request.Longitude;
            user.IsUpdatePosition = true;
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Can not save position");
            }
        }

        public async Task<List<UserDisplay>> FindAround(FindAroundRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);

            if (user == null)
                throw new Exception("Can not find user");

            var users = await _context.Users.Where(x => x.Status == EUserStatus.Active && x.IsUpdatePosition).ToListAsync();

            users.Remove(user);

            try
            {
                if (request.Gender == 1)
                {
                    users = users.Where(x => x.Gender == EGender.Nam).ToList();
                }

                else if (request.Gender == 2)
                {
                    users = users.Where(x => x.Gender == EGender.Nữ).ToList();
                }

                if (request.AgeGroup != -1)
                {
                    users = users.Where(x => GetAgeGroup(x.Dob) == (EAgeGroup)request.AgeGroup).ToList();
                }
            }
            catch (Exception)
            {
                throw new Exception("Parameter not correct");
            }


            var currentCoordinates = new Coordinates(user.Latitude, user.Longitude);

            foreach (var u in users)
            {
                var distance = new Coordinates(u.Latitude, u.Longitude).DistanceTo(currentCoordinates);
                u.Distance = distance;
            }

            users = users.Where(x => x.Distance <= request.Distance).ToList();

            users = users.OrderBy(x => x.Distance)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize).ToList();

            var responses = new List<UserDisplay>();
            foreach (var item in users)
            {
                var res = new UserDisplay(item, _storageService);
                res.Followed = await IsFollowed(request.UserId, item.Id);
                res.Favorited = await IsLiked(request.UserId, item.Id);

                responses.Add(res);
            }

            return responses;
        }

        public async Task<List<UserDisplay>> SearchFriend(Guid userId, string name)
        {
            name = name.NonUnicode().ToLower().Trim();

            var follows = await _context.Follows
                .Where(x => x.FromUserId == userId)
                .Include(x => x.ToUser)
                .ToListAsync();

            var users = follows.Select(x => x.ToUser).ToList();

            users = users.Where(x => x.FullName.NonUnicode().ToLower().Contains(name)).ToList();

            users = users
                .Take(9).ToList();

            var resposnes = new List<UserDisplay>();

            foreach (var item in users)
            {
                var user = new UserDisplay(item, _storageService);
                resposnes.Add(user);
            }

            return resposnes;
        }

        public async Task<UserDisplay> GetUserDisplayById(Guid userId)
        {
            try
            {
                return new UserDisplay(await _context.Users.FindAsync(userId), _storageService);
            }
            catch (Exception)
            {
                throw new Exception("Can not find user");
            }
        }
    }
}