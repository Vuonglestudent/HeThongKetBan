using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public class FileStorageService : IStorageService
    {
        public readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public FileStorageService(IWebHostEnvironment webHostEnviroment)
        {
            webHostEnviroment.WebRootPath = webHostEnviroment.ContentRootPath;
            _userContentFolder = Path.Combine(webHostEnviroment.WebRootPath, USER_CONTENT_FOLDER_NAME);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            try
            {
                await Task.Run(() => File.Delete(filePath));
            }
            catch
            {
            }
            //if (!File.Exists(fileName))
            //{
            //    await Task.Run(() => File.Delete(filePath));
            //}
        }

        public string GetFileUrl(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && fileName.Contains("http"))
                return fileName;

            return Startup.DomainName + $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}