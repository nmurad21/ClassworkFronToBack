using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    public class ChatHub:Hub
    {

        private readonly UserManager<AppUser> _userManager;


        public ChatHub( UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendMessage(string user, string message )
        {
            await Clients.All.SendAsync("RecieveMessage", user, message, DateTime.Now.ToString("dddd, dd MMMM yyyy"));
        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId = Context.ConnectionId;
               var result= _userManager.UpdateAsync(user).Result;
            }
            return base.OnConnectedAsync(); 
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId =null;
                var result = _userManager.UpdateAsync(user).Result;
                Clients.All.SendAsync("Connect", user.Id);
            }
            
            return base.OnConnectedAsync();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
