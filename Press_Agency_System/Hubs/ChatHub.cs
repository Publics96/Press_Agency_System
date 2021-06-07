using Microsoft.AspNet.SignalR;
using Press_Agency_System.Models;
using Press_Agency_System.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Press_Agency_System.Hubs
{
    public class ChatHub : Hub
    {
        public static IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        public void GetUsersToChat()
        {
            var UserId = HttpContext.Current.User.Identity.Name;
            List<UserDTO> users = new AppServices().GetUsersToChat();
            Clients.All.BroadcastUsersToChat(users);

        }

        public override Task OnConnected()
        {
            string userId = new AppServices().AddUserConnection((Context.ConnectionId).ToString());
            Clients.All.BroadcastOnlineUser(userId);
            return base.OnConnected();
        }
        public static void RecieveMessage(string fromUserId, string toUserId, string message)
        {

            context.Clients.Clients(new AppServices().GetUserConnections(toUserId)).BroadcastRecieveMessage(fromUserId, message);
        }



    }
}