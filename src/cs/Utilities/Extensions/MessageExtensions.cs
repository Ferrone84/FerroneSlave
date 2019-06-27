
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
	public static class MessageExtensions
	{
		/// <summary>
		/// Quote the message.
		/// Param user = user who wants to quote the message.
		/// </summary>
		/// <param name="message">Message to quote.</param>
		/// <param name="user">User who wants to quote the message.</param>
		/// <returns></returns>
		public static Embed Quote(this IMessage message, IUser user = null)
		{
			var channel = message.Channel;
			if (!(channel is SocketGuildChannel)) {
				return null;
			}

			int maximum = -1;
			Color color = new Color(75, 75, 75);
			var guild = ((SocketGuildChannel)channel).Guild;

			foreach (var role in ((SocketGuildUser)message.Author).Roles) {
				if (maximum < role.Position) {
					maximum = role.Position;
					color = role.Color;
				}
			}

			EmbedBuilder embedBuilder = new EmbedBuilder()
				.WithDescription(message.Content)
				.WithColor(color)
				.WithAuthor(message.Author.ToString(), message.Author.GetAvatarUrl(), "https://discordapp.com/channels/" + guild.Id + '/' + channel.Id + '/' + message.Id)
				.WithTimestamp(message.CreatedAt);

			if (user != null) {
				embedBuilder.WithFooter("Quoted by: " + user.Username);
			}

			if (message.Attachments.Count != 0) {
				foreach (IAttachment attachment in message.Attachments) {
					embedBuilder.WithImageUrl(attachment.Url);
				}
			}

			return embedBuilder.Build();
		}
	}
}
