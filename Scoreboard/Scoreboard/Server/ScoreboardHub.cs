using Microsoft.AspNetCore.SignalR;

namespace Server;
public interface IScoreboardHub
{
    Task ScoreUpdate(string home, string away);
}
public class ScoreboardHub : Hub<IScoreboardHub>
{
    public async Task ScoreUpdate(string home, string away)
    {
        await Clients.All.ScoreUpdate(home, away);
    }
}

