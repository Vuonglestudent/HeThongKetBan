using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using MakeFriendSolution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MakeFriendSolution.Controllers
{
    /// <summary>
    /// Các API thống kê lưu lượng truy cập
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly MakeFriendDbContext _context;
        private ISessionService _sessionService;

        public AccessController(MakeFriendDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }
        /// <summary>
        /// Thống kê lưu lượng truy cập theo ngày
        /// </summary>
        /// <param name="Date">Truyền vào ngày muốn thống kê</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("byDate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Access))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAccessCountByDate(DateTime Date)
        {
            if (Date.Date > DateTime.Now.Date)
            {
                return BadRequest(new
                {
                    Message = "Date is not valid"
                });
            }
            var accessCount = await _context.Accesses.Where(x => x.Date.Date == Date.Date).FirstOrDefaultAsync();

            return Ok(accessCount);
        }

        /// <summary>
        /// Thống kê lưu lượng truy cập theo tháng
        /// </summary>
        /// <param name="dateTime">truyền vào tháng muốn thống kê</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("byMonth")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccessResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAccessCountByMonth(DateTime dateTime)
        {
            if (dateTime.Date > DateTime.Now.Date)
            {
                return BadRequest(new
                {
                    Message = "Date is not valid"
                });
            }

            var byMonth = await this.GetByMonth(dateTime);

            var listAccess = byMonth.Item2;
            listAccess = listAccess.OrderBy(x => x.Period).ToList();

            return Ok(new
            {
                accessTotal = byMonth.Item1,
                listAccess = listAccess
            });
        }

        /// <summary>
        /// Thống kê lưu lượng truy cập theo năm
        /// </summary>
        /// <param name="dateTime">Truyền vào năm muốn thống kê</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("byYear")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AccessResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAccessCountByYear(DateTime dateTime)
        {
            if (dateTime.Date > DateTime.Now.Date)
            {
                return BadRequest(new
                {
                    Message = "Date is not valid"
                });
            }

            var byYear = await this.GetByYear(dateTime);

            return Ok(new
            {
                accessTotal = byYear.Item1,
                listAccess = byYear.Item2
            });
        }

        /// <summary>
        /// Lấy số lượng người truy cập trong tháng này, tháng trước và tỉ lệ tăng trưởng so với tháng trước
        /// </summary>
        /// <returns>Số lượng người truy cập trong tháng này, tháng trước và tỉ lệ tăng trưởng so với tháng trước</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getTheNumberOfNewUsersByMonth")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrowthRate))]
        public async Task<IActionResult> GetTheNumberOfNewUsersByMonth()
        {
            var numberOfUsers = await _context.Users
                .Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.CreatedAt.Month == DateTime.Now.Month)
                .CountAsync();

            int numberOfLastMonth = await _context.Users
                .Where(x => x.CreatedAt.Year == DateTime.Now.AddMonths(-1).Year && x.CreatedAt.Month == DateTime.Now.AddMonths(-1).Month)
                .CountAsync();

            double percents = Math.Round(((double)(numberOfUsers) / numberOfLastMonth * 100), 2);

            return Ok(new GrowthRate()
            {
                thisMonth = numberOfUsers,
                lastMonth = numberOfLastMonth,
                growthRate = percents
            });
        }


        /// <summary>
        ///Lấy số lượng tài khoản trong hệ thống 
        /// </summary>
        /// <returns>Số tài khoản active và inactive</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getNumberOfActiveUsers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountCount))]
        public async Task<IActionResult> GetNumberOfActiveUsers()
        {
            var numberOfActiveUsers = await _context.Users.Where(x => x.Status == Models.Enum.EUserStatus.Active).CountAsync();
            var numberOfInactiveUsers = await _context.Users.Where(x => x.Status != Models.Enum.EUserStatus.Active).CountAsync();
            return Ok(new AccountCount()
            { 
                activeAccounts = numberOfActiveUsers,
                inactiveAccounts = numberOfInactiveUsers
            });
        }


        /// <summary>
        /// Lấy số lượng tài khoản của mỗi loại
        /// </summary>
        /// <returns>Số lượng tài khoản hệ thống, facebook và google</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getTheAccountNumberOfEachType")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountTypeResponse))]
        public async Task<IActionResult> GetTheAccountNumberOfEachType()
        {
            var facebook = await _context.Users.Where(x => x.TypeAccount == Models.Enum.ETypeAccount.Facebook).CountAsync();
            var google = await _context.Users.Where(x => x.TypeAccount == Models.Enum.ETypeAccount.Google).CountAsync();
            var system = await _context.Users.Where(x => x.TypeAccount == Models.Enum.ETypeAccount.System).CountAsync();

            return Ok(new AccountTypeResponse()
            {
                facebook = facebook,
                google = google,
                system = system
            }
            );
        }

        private async Task<(AccessResponse, List<AccessResponse>)> GetByMonth(DateTime datetime)
        {
            var accessCount = await _context.Accesses
            .Where(x => x.Date.Date.Year == datetime.Date.Year && x.Date.Date.Month == datetime.Date.Month)
            .ToListAsync();

            var accessResponse = new List<AccessResponse>();
            var accessTotal = new AccessResponse();


            if (datetime.Month == 2)
            {
                for (int i = 1; i <= 28; i++)
                {
                    var access = new AccessResponse();
                    var date = new DateTime(datetime.Year, datetime.Month, i);
                    access.Period = date;

                    var item = accessCount.Where(x => x.Date.Day == i).FirstOrDefault();

                    if (item != null)
                    {
                        accessTotal.AuthorizeCount += item.AuthorizeCount;
                        accessTotal.UnauthorizeCount += item.UnauthorizeCount;
                        access.AuthorizeCount = item.AuthorizeCount;
                        access.UnauthorizeCount = item.UnauthorizeCount;
                        access.Total = access.AuthorizeCount + access.UnauthorizeCount;
                    }

                    accessResponse.Add(access);
                }

                var access2 = new AccessResponse();
                var date2 = new DateTime(datetime.Year, 3, 29);
                access2.Period = date2;
                accessResponse.Add(access2);

                access2 = new AccessResponse();
                date2 = new DateTime(datetime.Year, 3, 30);
                access2.Period = date2;
                accessResponse.Add(access2);

            }
            else
            {
                for (int i = 1; i <= 30; i++)
                {
                    var access = new AccessResponse();
                    var date = new DateTime(datetime.Year, datetime.Month, i);
                    access.Period = date;

                    var item = accessCount.Where(x => x.Date.Day == i).FirstOrDefault();

                    if (item != null)
                    {
                        accessTotal.AuthorizeCount += item.AuthorizeCount;
                        accessTotal.UnauthorizeCount += item.UnauthorizeCount;
                        access.AuthorizeCount = item.AuthorizeCount;
                        access.UnauthorizeCount = item.UnauthorizeCount;
                        access.Total = access.AuthorizeCount + access.UnauthorizeCount;
                    }

                    accessResponse.Add(access);
                }
            }

            accessTotal.Total = accessTotal.AuthorizeCount + accessTotal.UnauthorizeCount;
            accessTotal.Period = datetime;

            return (accessTotal, accessResponse);
        }
        private async Task<(AccessResponse, List<AccessResponse>)> GetByYear(DateTime dateTime)
        {
            var accessCount = new List<AccessResponse>();
            var accessTotal = new AccessResponse();

            for (int i = 1; i <= 12; i++)
            {
                var date = new DateTime(dateTime.Year, i, 1);

                var byMonth = await GetByMonth(date);
                accessTotal.AuthorizeCount += byMonth.Item1.AuthorizeCount;
                accessTotal.UnauthorizeCount += byMonth.Item1.UnauthorizeCount;
                accessTotal.Period = byMonth.Item1.Period;

                accessCount.Add(byMonth.Item1);
            }

            accessTotal.Total = accessTotal.UnauthorizeCount + accessTotal.UnauthorizeCount;
            accessTotal.Period = dateTime;

            return (accessTotal, accessCount);
        }
    }
}