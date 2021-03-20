using MakeFriendSolution.Application;
using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class FeaturesController : ControllerBase
    {
        private readonly MakeFriendDbContext _context;
        private readonly IStorageService _storageService;
        private ISessionService _sessionService;
        private readonly IUserApplication _userApplication;
        private readonly IFeatureApplication _featureApplication;

        public FeaturesController(MakeFriendDbContext context, IStorageService storageService, ISessionService sessionService, IUserApplication userApplication, IFeatureApplication featureApplication)
        {
            _context = context;
            _storageService = storageService;
            _sessionService = sessionService;
            _userApplication = userApplication;
            _featureApplication = featureApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetFeatures()
        {
            var features = await _featureApplication.GetFeatures();
            return Ok(features);
        }
        [HttpPost]
        public async Task<IActionResult> AddFeature([FromBody] CreateFeatureRequest request)
        {
            var feature = new Feature()
            {
                Name = request.Name,
                IsCalculated = request.IsCalculated,
                IsSearchFeature = request.IsSearchFeature,
                WeightRate = request.WeightRate,
            };

            feature = await _featureApplication.AddFeature(feature);
            await SetUpdateScore();
            return Ok(feature);
        }
        [HttpPost("content")]
        public async Task<IActionResult> AddFeatureDetail([FromBody] CreateFeatureDetailRequest request)
        {
            var featureDetail = new FeatureDetail()
            {
                Content = request.Content,
                Weight = request.Weight,
                FeatureId = request.FeatureId
            };

            featureDetail = await _featureApplication.AddFeatureDetail(featureDetail);
            await SetUpdateScore();
            return Ok(featureDetail);
        }
        [HttpPut("content")]
        public async Task<IActionResult> UpdateFeatureDetail([FromBody] UpdateFeatureDetailRequest request)
        {
            var featureDetail = await _context.FeatureDetails.FindAsync(request.Id);
            
            if (featureDetail == null)
            {
                return NotFound(new
                {
                    Message = "Can not find feature with Id = " + request.Id
                });
            }

            featureDetail.Weight = request.Weight;
            featureDetail.Content = request.Content;
            var updateContent = await _featureApplication.UpdateFeatureDetail(featureDetail);
            await SetUpdateScore();
            return Ok(updateContent);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeature([FromBody] UpdateFeatureRequest request)
        {
            var updateFeature = await _featureApplication.GetFeatureById(request.Id);
            if (updateFeature == null)
            {
                return NotFound(new
                {
                    Message = "Can not find feature with Id = " + request.Id
                });
            }

            updateFeature.IsCalculated = request.IsCalculated;
            updateFeature.Name = request.Name;
            updateFeature.WeightRate = request.WeightRate;
            updateFeature.IsSearchFeature = request.IsSearchFeature;
            updateFeature = await _featureApplication.UpdateFeature(updateFeature);
            await SetUpdateScore();
            return Ok(updateFeature);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(int id)
        {

            var status = await _featureApplication.DeleteFeature(id);
            if (status)
            {
                await SetUpdateScore();
                return Ok();
            }

            else
                return BadRequest(new
                {
                    Message = "Không thể xóa feature!"
                });
        }

        [HttpDelete("content/{id}")]
        public async Task<IActionResult> DeleteFeatureDetail(int id)
        {
            var status = await _featureApplication.DeleteFeatureDetail(id);
            if (status)
            {
                await SetUpdateScore();
                return Ok();
            }
            else
                return BadRequest(new
                {
                    Message = "Không thể xóa feature content!"
                });
        }

        private async Task<bool> SetUpdateScore()
        {
            var update = await _context.SimilariryFeatures.FirstOrDefaultAsync();
            update.UpdatedAt = DateTime.Now;

            _context.SimilariryFeatures.Update(update);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
