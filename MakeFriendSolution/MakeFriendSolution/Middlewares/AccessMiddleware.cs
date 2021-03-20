using MakeFriendSolution.EF;
using MakeFriendSolution.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MakeFriendSolution.Middlewares
{
    public class AccessMiddleware : IMiddleware
    {
        private readonly MakeFriendDbContext _context;

        public AccessMiddleware(MakeFriendDbContext context)
        {
            _context = context;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var now = DateTime.Now;
            var access = await _context.Accesses.Where(x => x.Date.Date == now.Date).FirstOrDefaultAsync();

            if (access == null)
            {
                access = new Access();
                access.AuthorizeCount = 0;
                access.UnauthorizeCount = 0;
                access.Date = DateTime.Now.Date;
                _context.Accesses.Add(access);
                await _context.SaveChangesAsync();
            }
            var identity = context.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();

            if (claims.Count == 0)
                access.UnauthorizeCount += 1;
            else
                access.AuthorizeCount += 1;

            _context.Accesses.Update(access);
            await _context.SaveChangesAsync();
            await next(context);
        }
    }
}