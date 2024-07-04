using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ChatDemoAPI2.Hubs
{
    public class ChatHub : Hub
    {
        [Authorize]
        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
            //await  Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                string username = Context.User.Identity.Name;                
                UserHandler.ConnectedIds[username] = Context.ConnectionId;                
            }
            await base.OnConnectedAsync();                        
        }

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    if (Context.User.Identity.IsAuthenticated)
        //    {
        //        string username = Context.User.Identity.Name;
        //        UserHandler.ConnectedIds.TryRemove(username, out _);
        //    }
        //    await base.OnDisconnectedAsync(exception);
        //}
    }    

    public static class UserHandler
    {
        //public static Dictionary<string, string> ConnectedIds = new Dictionary<string, string>();
        public static ConcurrentDictionary<string, string> ConnectedIds = new ConcurrentDictionary<string, string>();
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
