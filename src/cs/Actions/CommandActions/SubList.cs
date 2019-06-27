
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class SubList : ACommandAction
	{
		public SubList() : base()
		{
			Name = Prefix + "sublist";
			Description = "Affiche la liste de tous les abonnements aux mangas.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string msg = string.Empty;
			string message_lower = message.Content.ToLower();

			if (message_lower.Length > 8) {
				msg = Data.DataManager.database.subList(message.Author.Id.ToString(), message_lower.Split(' ')[1]);
			}
			else {
				msg = Data.DataManager.database.subList(message.Author.Id.ToString());
			}

			await message.Channel.SendMessagesAsync(msg);
		}
	}
}
