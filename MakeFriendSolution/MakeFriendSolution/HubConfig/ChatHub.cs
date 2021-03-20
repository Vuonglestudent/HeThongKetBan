using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakeFriendSolution.HubConfig
{
    public class ChatHub : Hub
    {
		// Use this variable to track user count
		private static int _userCount = 0;
		 
	     // Overridable hub methods  
	     public override async Task OnConnectedAsync()
		{
			_userCount++;
			await Response();

		}
		public override async Task OnDisconnectedAsync(Exception exception)
		{
			_userCount--;
			await Response();
		}


		private async Task Response()
        {
			var response = new
			{
				type = "onlineCount",
				onlineCount = _userCount
			};

			await this.Clients.All.SendAsync("transferData", response);
		}

		public string GetConnectionId() => Context.ConnectionId;
	}
}