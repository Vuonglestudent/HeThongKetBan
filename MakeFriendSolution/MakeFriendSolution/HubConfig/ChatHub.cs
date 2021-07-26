using MakeFriendSolution.HubConfig.Models;
using MakeFriendSolution.Models.ViewModels;
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
            //
            HangUp();

            await base.OnDisconnectedAsync(exception);
        }

		private async Task Response()
        {
			await this.Clients.All.SendAsync("onlineCount", _userCount);
		}

		public string GetConnectionId() => Context.ConnectionId;

        public RtcIceServer[] GetIceServers()
        {
            // Perhaps Ice server management.

            return new RtcIceServer[] { new RtcIceServer() { Username = "", Credential = "" } };
        }


        public UserConnection GetUserById(Guid userId)
        {
            var users = UserConnection.Get(userId);
            return users?[0];
        }

        public UserConnection GetUserByConnectionId(string connectionId)
        {
            var user = UserConnection.Get(connectionId);
            return user;
        }

        public UserConnection SaveMyInfo(Guid userId, string connectionId, string userName, string avatarPath)
        {
            var user = UserConnection.Get(userId, connectionId, userName, avatarPath);
            return user;
        }

        public void HangUp()
        {
            var callingUser = UserConnection.Get(Context.ConnectionId);

            if (callingUser == null)
            {
                return;
            }
            UserConnection.Remove(callingUser);
        }

        // WebRTC Signal Handler
        public async Task SendSignal(string data, Guid userId)
        {
            var receivers = UserConnection.Get(userId);

            // Make sure receivers are valid
            if (receivers == null)
            {
                return;
            }

            // These folks are in a call together, let's let em talk WebRTC
            await Clients.Clients(receivers.Select(x=>x.ConnectionId).ToList()).SendAsync("receiveSignal", data);
        }

        public async Task CallRequest(Guid userId, CallType callType)
        {
            var caller = UserConnection.Get(this.GetConnectionId());
            var receivers = UserConnection.Get(userId);

            if(caller == null || receivers == null)
            {
                Console.WriteLine("Caller null || receiver null");
            }
            await this.Clients.Clients(receivers.Select(x => x.ConnectionId).ToList()).SendAsync("callRequest", caller, callType);
            //await this.Clients.Clients(receiver.ConnectionId).SendAsync("callRequest", caller, callType);
        }

        public async Task CallAccept(Guid userId)
        {
            var users = UserConnection.Get(userId);
            try
            {
                await this.Clients.Clients(users.Select(x=>x.ConnectionId).ToList()).SendAsync("callStatus", CallStatus.Accept);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task CallReject(string connectionId)
        {
            try
            {
                await this.Clients.Client(connectionId).SendAsync("callStatus", CallStatus.Reject);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }

    public enum CallType
    {
        VideoCall,
        VoiceCall
    }

    public enum CallStatus
    {
        Accept,
        Reject
    }
}