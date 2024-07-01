using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace ChatDemoAPI
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToUser(string recipientUsername, string senderUsername, string message)
        {
            var recipientConnectionId = UserHandler.ConnectedIds[recipientUsername];
            if (recipientConnectionId != null)
            {
                await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", senderUsername, message);
            }
        }

        public override Task OnConnectedAsync()
        {
            string username = Context.User.Identity.Name;
            UserHandler.ConnectedIds[username] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string username = Context.User.Identity.Name;
            UserHandler.ConnectedIds.Remove(username);
            return base.OnDisconnectedAsync(exception);
        }
    }

    public static class UserHandler
    {
        public static Dictionary<string, string> ConnectedIds = new Dictionary<string, string>();
    }
}
