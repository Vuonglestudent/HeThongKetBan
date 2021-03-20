using MakeFriendSolution.Application;
using MakeFriendSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DetectImagesController : ControllerBase
    {
        private readonly IImageScoreApplication _imageScoreApp;

        public DetectImagesController(IImageScoreApplication imageScoreApplication)
        {
            _imageScoreApp = imageScoreApplication;
        }
    
        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetImageScore()
        {
            return Ok(await _imageScoreApp.GetImageScore());
        }

        [HttpPut]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateImageScore([FromForm] ImageScore imageScore)
        {
            var success = await _imageScoreApp.UpdateImageScore(imageScore);
            if (!success)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
