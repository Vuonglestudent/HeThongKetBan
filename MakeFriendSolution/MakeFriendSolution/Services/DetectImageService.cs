using MakeFriendSolution.Models;
using MakeFriendSolution.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MakeFriendSolution.Services
{
    public class DetectImageService : IDetectImageService
    {
        private readonly IConfiguration _config;
        private string urlService;
        public DetectImageService(IConfiguration config)
        {
            _config = config;
            urlService = _config["PythonService"];
        }
        public DetectImageResponse DetectImage(string fileName)
        {
            var client = new RestClient(urlService + "/predict-image");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            //var fileName = "." + _storage.GetFileUrl(await _storage.SaveFile(image));
            request.AddFile("image", fileName);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            var result = JsonConvert.DeserializeObject<DetectImageResponse>(response.Content);

            return result;
        }
    }
}
