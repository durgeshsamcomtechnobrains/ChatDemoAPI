using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SignalRSample.Hubs
{
    public class UserHub : Hub
    {
        public static int TotalView { get; set; } = 0;

        public async Task NewWindowLoaded()
        {
            TotalView++;
            await Clients.All.SendAsync("updateToTotalViews", TotalView);
        }
    }
}
