
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Actions.AdminActions
{
	public class Delete : AAdminAction
	{
		public Delete() : base()
		{
			Name = Prefix + "delete";
			Description = "Supprime le nombre de messages spécifiés en partant de la fin.";
			ChannelAccessibility = ChannelAccessibilityType.Guild;
		}

		public override async Task Invoke(IUserMessage message)
		{
			try {
				int numberOfDelete = System.Convert.ToInt32(message.Content.Split(' ')[1]) + 1;
				var messages = message.Channel.GetMessages(numberOfDelete);
				await (message.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
			}
			catch (System.Exception e) {
				e.DisplayException(Name);
				await message.Channel.SendMessageAsync("La commande doit être du type !!delete 10");
			}
		}
	}
}
