using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Data;
using DiscordBot.Utilities;

namespace DiscordBot.Events.EventsHandlers
{
	public class ChannelMessageReceived : IGuildMessageReceivedEventHandler
	{
		public async Task Guild_Message_Received(SocketUserMessage message)
		{
			if (message.Channel.Id == Channels.Musique.Id) {
				(string id, string url) link = message.Content.GetYoutubeLink();

				if (link.url != string.Empty) {
					try {
						DataManager.database.addMusic(link);
						await (message as SocketUserMessage).AddReactionAsync(EmoteManager.CheckMark);
					}
					catch (System.Exception) {
						await (message as SocketUserMessage).AddReactionAsync(EmoteManager.Guilds.Zawarudo.Aret);
					}
				}
			}
		}
	}
}
