using Microsoft.AspNetCore.SignalR;

namespace Server;
public interface IScoreboardHub
{
    Task ScoreUpdate(string home, string away);
    Task HomeUpdate(string home);
    Task AwayUpdate(string away);
}
public class ScoreboardHub : Hub<IScoreboardHub>
{
    public async Task ScoreUpdate(string home, string away)
    {
        await Clients.All.ScoreUpdate(home, away);
    }
    public async Task HomeUpdate(string home)
    {
        await Clients.All.HomeUpdate(home);
    }
    public async Task AwayUpdate(string away)
    {
        await Clients.All.AwayUpdate(away);
    }
}

