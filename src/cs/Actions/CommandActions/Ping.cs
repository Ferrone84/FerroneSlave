
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Ping : ACommandAction
	{
		public Ping() : base()
		{
			Name = Prefix + "ping";
			Description = "Affiche le ping du bot.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessagesAsync("Pong! Mon ping est de : " + Data.DataManager._client.Latency.ToString() + "ms.");
		}
	}
}
