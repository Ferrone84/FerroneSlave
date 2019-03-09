
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordBot
{
	public static class GuildExtentions
	{
		public static async Task<IMessage> GetMessageFromId(this SocketGuild guild, ulong messageId)
		{
			IMessage resultMessage = null;

			foreach (var channel in guild.Channels) {
				if (!(channel is SocketTextChannel)) {
					continue;
				}

				var message = await ((SocketTextChannel)channel).GetMessageAsync(messageId);
				if (message != null) {
					return message;
				}
			}

			return resultMessage;
		}
	}
}
