using MakeFriendSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.Models.ViewModels
{
    public class MessageResponse
    {
        public MessageResponse()
        {
        }

        public MessageResponse(HaveMessage message, IStorageService storageService)
        {
            Id = message.Id;
            SenderId = message.SenderId;
            ReceiverId = message.ReceiverId;
            Content = message.Content;
            SentAt = message.SentAt;
            MessageType = message.MessageType;
            SetImagePaths(storageService, message.FilePath);
        }

        private void SetImagePaths(IStorageService storageService, string paths)
        {
            this.FilePaths = new List<string>();
            if (paths != null)
            {
                var arr = paths.Split(",");
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = arr[i].Replace(" ", "");
                    arr[i] = storageService.GetFileUrl(arr[i]);
                    FilePaths.Add(arr[i]);
                }
            }
        }

        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public List<string> FilePaths { get; set; }
        public string MessageType { get; set; }
        public DateTime SentAt { get; set; }
    }
}