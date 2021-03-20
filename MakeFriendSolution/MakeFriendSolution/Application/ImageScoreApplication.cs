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
    public class ImageScoreApplication: IImageScoreApplication
    {
        private readonly MakeFriendDbContext _context;
        public ImageScoreApplication(MakeFriendDbContext context)
        {
            _context = context;
        }

        public async Task<ImageScore> GetImageScore()
        {
            var scores = await _context.ImageScores.FirstOrDefaultAsync();
            if(scores == null)
            {
                scores = new ImageScore()
                {
                    Drawings = 0,
                    Hentai = 0.2,
                    Neutral = 0,
                    Porn = 0.2,
                    Sexy = 0.5,
                    UpdatedAt = DateTime.Now
                };

                _context.ImageScores.Add(scores);
                await SaveChangesAsync();
            }
            return scores;
        }
        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateImageScore(ImageScore update)
        {
            var scores = await GetImageScore();
            scores.Drawings = update.Drawings;
            if (update.Porn >= 0 && update.Porn <= 100)
                scores.Porn = update.Porn;

            if (update.Sexy >= 0 && update.Sexy <= 100)
                scores.Sexy = update.Sexy;

            if (update.Hentai >= 0 && update.Hentai <= 100)
                scores.Hentai = update.Hentai;

            scores.Neutral = update.Neutral;
            scores.UpdatedAt = DateTime.Now;
            scores.AutoFilter = update.AutoFilter;
            scores.Active = update.Active;

            _context.ImageScores.Update(scores);

            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ValidateImage(DetectImageResponse scores)
        {
            var val = await GetImageScore();

            if (scores.Porn * 100 >= val.Porn || scores.Sexy * 100 >= val.Sexy || scores.Hentai * 100 >= val.Hentai)
                return false;

            return true;
        }
    }
}