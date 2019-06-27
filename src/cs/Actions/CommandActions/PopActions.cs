using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.CommandActions
{
	public class PopAction : ACommandAction
	{
		public PopAction() : base()
		{
			Name = Prefix + "popactions";
			Description = "Permet d'afficher les actions les plus populaires.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			StringBuilder result = new StringBuilder();
			result.Append("Liste des actions les plus populaires :\n");

			var sortedActionsUsed = from entry in DataManager.actions_used orderby entry.Value descending select entry;

			foreach (KeyValuePair<string, int> kvp in sortedActionsUsed) {
				result.Append("- `" + kvp.Key + "` : " + kvp.Value.ToString() + "\n");
			}

			await message.Channel.SendMessagesAsync(result.ToString());
		}
	}
}
