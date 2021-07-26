using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public interface IStorageService
    {
        Task<string> SaveFile(IFormFile file);
        string GetFileUrl(string fileName);
        string GetFileUrlWithoutDomain(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);
    }
}