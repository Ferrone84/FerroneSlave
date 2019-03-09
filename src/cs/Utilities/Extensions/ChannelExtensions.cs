
using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot
{
	public static class ChannelExtentions
	{
		public static async Task SendMessagesAsync(this IMessageChannel channel, string messages, string delim = "\n", int maxLength = 1990)
		{
			foreach (string message in messages.SplitBodies(delim, maxLength)) {
				await channel.SendMessageAsync(message);
			}
		}

		public static IEnumerable<IMessage> GetAllMessages(this IMessageChannel channel, int limit)
		{
			return channel.GetMessagesAsync(0, Direction.After, limit).FlattenAsync().Result;
		}

		public static IEnumerable<IMessage> GetMessages(this IMessageChannel channel, int number)
		{
			return channel.GetMessagesAsync(number).FlattenAsync().Result;
		}
	}
}
