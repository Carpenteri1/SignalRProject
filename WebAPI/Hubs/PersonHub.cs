using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class PersonHub : Hub
    {
        private const string webAppClient = nameof(webAppClient);
        private const string consoleClient = nameof(consoleClient);
        private const string hubUser = nameof(hubUser);
        private string messageTo = string.Empty;
        public async Task SendMessage(string user ,string messageFrom)
        {
            if (messageFrom == string.Empty) return;

            if (user == webAppClient)messageTo = $"Hey {webAppClient}";
            if(user ==  consoleClient) messageTo = $"Hey {consoleClient}";
            await Clients.All.SendAsync("Receive", user, messageFrom,messageTo);
        }
    }
}