using MakeFriendSolution.Common;
using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Application
{
    public class FeatureApplication : IFeatureApplication
    {
        private readonly MakeFriendDbContext _context;
        public FeatureApplication(MakeFriendDbContext context)
        {
            _context = context;
        }
        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Feature> AddFeature(Feature feature)
        {
            _context.Features.Add(feature);
            await Save();
            await UpdateGlobalVariable();
            return feature;
        }

        public async Task<FeatureDetail> AddFeatureDetail(FeatureDetail featureDetail)
        {
            _context.FeatureDetails.Add(featureDetail);
            await Save();
            await UpdateGlobalVariable();
            return featureDetail;
        }

        public async Task<bool> DeleteFeature(int id)
        {
            var userFeatures = await _context.UserFeatures.Where(x => x.FeatureId == id).ToListAsync();
            var searchFeatures = await _context.SearchFeatures.Where(x => x.FeatureId == id).ToListAsync();
            var featureDetail = await _context.FeatureDetails.Where(x => x.FeatureId == id).ToListAsync();
            var feature = await _context.Features.FindAsync(id);

            _context.UserFeatures.RemoveRange(userFeatures);
            _context.SearchFeatures.RemoveRange(searchFeatures);
            _context.FeatureDetails.RemoveRange(featureDetail);

            _context.Features.Remove(feature);
            try
            {
                await Save();
                await UpdateGlobalVariable();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteFeatureDetail(int id)
        {
            var userFeatures = await _context.UserFeatures.Where(x => x.FeatureDetailId == id).ToListAsync();
            var searchFeatures = await _context.SearchFeatures.Where(x => x.FeatureDetailId == id).ToListAsync();
            var featureDetail = await _context.FeatureDetails.FindAsync(id);
            _context.UserFeatures.RemoveRange(userFeatures);
            _context.SearchFeatures.RemoveRange(searchFeatures);
            _context.FeatureDetails.Remove(featureDetail);
            try
            {
                await Save();
                await UpdateGlobalVariable();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<Feature> GetFeatureById(int id)
        {
            if (!GlobalVariable.HaveData)
            {
                await UpdateGlobalVariable();
            }

            var feature = GlobalVariable.Features.Where(x => x.Id == id).FirstOrDefault();
            if (feature == null)
                return null;
            feature.FeatureDetails = GlobalVariable.FeatureDetails.Where(x => x.FeatureId == id).ToList();
            return feature;
        }
        public async Task UpdateGlobalVariable()
        {
            GlobalVariable.Features = await _context.Features.ToListAsync();
            GlobalVariable.FeatureDetails = await _context.FeatureDetails.ToListAsync();
            GlobalVariable.HaveData = true;
        }
        public async Task<List<FeatureDetail>> GetFeatureDetails()
        {
            if (!GlobalVariable.HaveData)
            {
               await UpdateGlobalVariable();
            }
            return GlobalVariable.FeatureDetails;
        }

        public async Task<FeatureResponse> GetFeatureResponse(int featureDetailId)
        {
            if (!GlobalVariable.HaveData)
            {
                await UpdateGlobalVariable();
            }
            var featureDetail = GlobalVariable.FeatureDetails.Where(x => x.Id == featureDetailId).FirstOrDefault();
            var feature = GlobalVariable.Features.Where(x => x.Id == featureDetail.FeatureId).FirstOrDefault();

            return new FeatureResponse()
            {
                FeatureId = feature.Id,
                FeatureDetailId = featureDetail.Id,
                Name = feature.Name,
                Content = featureDetail.Content,
                IsSearchFeature = feature.IsSearchFeature
            };
        }

        public async Task<List<Feature>> GetFeatures()
        {
            if (!GlobalVariable.HaveData)
                await UpdateGlobalVariable();
            return GlobalVariable.Features;
        }

        public async Task<FeatureViewModel> GetFeatureViewModel(int featureDetailId)
        {
            if (!GlobalVariable.HaveData)
            {
                await UpdateGlobalVariable();
            }
            var featureDetail = GlobalVariable.FeatureDetails.Where(x => x.Id == featureDetailId).FirstOrDefault();
            var feature = GlobalVariable.Features.Where(x => x.Id == featureDetail.FeatureId).FirstOrDefault();

            return new FeatureViewModel()
            {
                FeatureId = feature.Id,
                FeatureDetailId = featureDetail.Id,
                IsSearchFeature = feature.IsSearchFeature,
                IsCalculated = feature.IsCalculated,
                Name = feature.Name,
                Content = featureDetail.Content,
                Rate = feature.WeightRate,
                weight = featureDetail.Weight
            };
        }

        public async Task<Feature> UpdateFeature(Feature updateFeature)
        {
            _context.Features.Update(updateFeature);
            await Save();
            await UpdateGlobalVariable();
            return updateFeature;
        }

        public async Task<FeatureDetail> UpdateFeatureDetail(FeatureDetail updateFeatureDetail)
        {
            _context.FeatureDetails.Update(updateFeatureDetail);
            await Save();
            await UpdateGlobalVariable();
            return updateFeatureDetail;
        }

        public async Task<List<FeatureViewModel>> GetFeatureViewModelByUserId(Guid userId)
        {
            var haveFeatures = await _context.UserFeatures.Where(x => x.UserId == userId).ToListAsync();    
            var features = await GetFeatures();
            var featureDetails = await GetFeatureDetails();
            var details = new List<FeatureViewModel>();

            foreach (var item in haveFeatures)
            {
                var detail = await GetFeatureViewModel(item.FeatureDetailId);
                details.Add(detail);
            }

            return details;
        }
        public async Task<(List<FeatureResponse>, List<FeatureResponse>)> GetFeatureResponseByUserId(Guid userId)
        {
            var haveFeatures = await _context.UserFeatures.Where(x => x.UserId == userId).ToListAsync();
            var features = await GetFeatures();

            var featureDetails = await GetFeatureDetails();

            var details = new List<FeatureResponse>();

            foreach (var item in haveFeatures)
            {
                var detail = await GetFeatureResponse(item.FeatureDetailId);
                details.Add(detail);
            }

            //

            var searchFeatures = await _context.SearchFeatures.Where(x => x.UserId == userId).ToListAsync();

            var searchFeatureDetails = new List<FeatureResponse>();

            foreach (var item in searchFeatures)
            {
                var detail = await GetFeatureResponse(item.FeatureDetailId);
                searchFeatureDetails.Add(detail);
            }

            return (details, searchFeatureDetails);
        }

        public async Task<bool> CreateUserFeature(Guid userId, int featureId, int detailId)
        {
            var userFeature = await GetUserFeature(userId, detailId);
            if(userFeature != null)
            {
                _context.UserFeatures.Remove(userFeature);
            }
            var newUserFeature = new UserFeature()
            {
                FeatureId = featureId,
                FeatureDetailId = detailId,
                UserId = userId
            };

            _context.UserFeatures.Add(newUserFeature);
            try
            {
                await Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserFeature(Guid userId, int featureId, int newDetailId)
        {
            var userFeature = await GetUserFeature(userId, featureId);
            if (userFeature == null)
            {
                userFeature = new UserFeature()
                {
                    UserId = userId,
                    FeatureId = featureId,
                    FeatureDetailId = newDetailId
                };
                _context.UserFeatures.Add(userFeature);
            }
            else
            {
                userFeature.FeatureDetailId = newDetailId;
                _context.UserFeatures.Update(userFeature);
            }
            try
            {
                await Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSearchFeature(Guid userId, int featureId, int newDetailId)
        {
            var search = await GetSearchFeature(userId, featureId);
            if (search == null)
            {
                search = new SearchFeature()
                {
                    UserId = userId,
                    FeatureId = featureId,
                    FeatureDetailId = newDetailId
                };
                _context.SearchFeatures.Add(search);
            }
            else
            {
                search.FeatureDetailId = newDetailId;
                _context.SearchFeatures.Update(search);
            }
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<UserFeature> GetUserFeature(Guid userId, int featureId)
        {
            var userFeatures = await _context.UserFeatures
                .Where(x => x.UserId == userId && x.FeatureId == featureId)
                .FirstOrDefaultAsync();

            return userFeatures;
        }

        public async Task<SearchFeature> GetSearchFeature(Guid userId, int featureId)
        {
            var search = await _context.SearchFeatures
                .Where(x => x.UserId == userId && x.FeatureId == featureId)
                .FirstOrDefaultAsync();

            return search;
        }
        public async Task<bool> CheckUserFeature(Guid userId)
        {
            var features = await GetFeatures();
            var userFeatures = await _context.UserFeatures.Where(x => x.UserId == userId).ToListAsync();

            foreach (var item in features)
            {
                if(!userFeatures.Any(x=>x.FeatureId == item.Id))
                {
                    var user = await _context.Users.FindAsync(userId);
                    user.IsInfoUpdated = false;

                    _context.Users.Update(user);
                    await Save();
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> UpdateHaveFeatures(List<UserFeature> haveFeatures, Guid userId)
        {
            foreach (var item in haveFeatures)
            {
                try
                {
                    if (item.FeatureDetailId == -1)
                        continue;

                    await UpdateUserFeature(userId, item.FeatureId, item.FeatureDetailId);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> UpdateSearchFeatures(List<SearchFeature> searchFeatures, Guid userId)
        {
            foreach (var item in searchFeatures)
            {
                if (item.FeatureDetailId == -1)
                    continue;

                try
                {
                    await UpdateSearchFeature(userId, item.FeatureId, item.FeatureDetailId);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
