using College_Social_Network_Project.Models;
using College_Social_Network_Project.Controllers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

using System.Web.SessionState;
using System.Web.Mvc;

namespace College_Social_Network_Project.Hubs
{

    public class ChatHub : Hub
    {
        //private Social_NetworkEntities db = new Social_NetworkEntities();
        public static IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

        public override Task OnConnected()
        {
            string userId = new ChatController().AddUserConnection(Guid.Parse(Context.ConnectionId));
            Clients.All.BroadcastOnlineUser(userId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = new ChatController().RemoveUserConnection(Guid.Parse(Context.ConnectionId));
            Clients.All.BroadcastOfflineUser(userId);
            return base.OnDisconnected(stopCalled);
        }

        public void GetUsersToChat()
        {
          
            string UserId = (HttpContext.Current.User.Identity.GetUserId());
            List<UserDTO> users = new ChatController().GetUsersToChat();
            Clients.Clients(new ChatController().GetUSerConnections(UserId)).BroadcastUsersToChat(users);
        }

        public static void OfflineUser(string UserId)
        {
            context.Clients.All.BroadcastOfflineUser(UserId);
        }
        public static void RecieveMessage(string fromUserId, string toUserId, string message)
        {
            context.Clients.Clients(new ChatController().GetUSerConnections(toUserId)).BroadcastRecieveMessage(fromUserId, message);
        }

    }
}
