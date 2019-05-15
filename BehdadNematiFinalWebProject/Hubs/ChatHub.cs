using BehdadNematiFinalWebProject.Areas.Identity.Data;
using BehdadNematiFinalWebProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ApplicationContext applicationContext;

        public ChatHub(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor,ApplicationContext applicationContext)
        {
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
            this.applicationContext = applicationContext;
        }

        public async Task Register()
        {
            var user = await userManager.GetUserAsync(contextAccessor.HttpContext.User);
            user.SignalRConnectionId = Context.ConnectionId;
            await userManager.UpdateAsync(user);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var user = userManager.Users.FirstOrDefault(x => x.SignalRConnectionId == Context.ConnectionId);
            if (user != null)
            {
                user.SignalRConnectionId = null;
                await userManager.UpdateAsync(user);
            }
        }

        public async Task SendMessageClientToServer(string username, string message)
        {

            var user = ((List<ApplicationUser>)await userManager.GetUsersInRoleAsync("admins")).FirstOrDefault();
            if (user.SignalRConnectionId != null)
            {
                await Clients.Client(user.SignalRConnectionId)
                    .SendAsync("SendMessageServerToClient", username, message);
            }
        }

    }
}
