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
        private static List<ApplicationUser> AdminsLst;

        public ChatHub(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, ApplicationContext applicationContext)
        {
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
            this.applicationContext = applicationContext;
            GetAdminLst().Wait();
        }
        //find a better solution later
        public async Task GetAdminLst() => AdminsLst = (List<ApplicationUser>)await userManager.GetUsersInRoleAsync("admins");
        
        public async Task RegisterUser()
        {
            var user = await userManager.GetUserAsync(contextAccessor.HttpContext.User);
            user.SignalRConnectionId = Context.ConnectionId;
            await userManager.UpdateAsync(user);
            await Clients.Client(Context.ConnectionId)
               .SendAsync("RecivePartnerCredentials", AdminsLst.FirstOrDefault().FirstName,AdminsLst.FirstOrDefault().Email);
                //get random available admin later
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
        //var AdminLst = ((List<ApplicationUser>)await userManager.GetUsersInRoleAsync("admins")).FirstOrDefault();

        public async Task SendMessageAdminClientToServer(string message, string recipient)
        {
            var user = await userManager.FindByEmailAsync(recipient);
            if (user.SignalRConnectionId != null)
            {
                await Clients.Client(user.SignalRConnectionId)
                    .SendAsync("SendMessageServerToUserClient",message);
            }
        }
        public async Task SendMessageUserClientToServer(string message, string recipient)
        {
            var user = await userManager.FindByEmailAsync(recipient);
            if (user.SignalRConnectionId != null && AdminsLst.Contains(user)) //added for security reasons
            {
                await Clients.Client(user.SignalRConnectionId)
                    .SendAsync("SendMessageServerToAdminClient", message);
            }
        }

    }
}
