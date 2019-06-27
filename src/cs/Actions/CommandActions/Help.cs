
using System.Text;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.CommandActions
{
	public class Help : ACommandAction
	{
		public Help() : base()
		{
			Name = Prefix + "help";
			Description = "Affiche toutes les options.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			ActionType savedType = ActionType.Other;
			StringBuilder result = new StringBuilder();
			result.Append("Voici toutes les options du bot par catégorie :\n");

			foreach (var kvp in DataManager.actions) {
				var action = kvp.Value;
				var currentType = action.Type;

				if (currentType != savedType) {
					savedType = currentType;
					result.Append("\n[").Append(currentType).Append("]\n");
				}

				if (action.Accessibility != AccessibilityType.Invisible) {
					result.Append("- ").Append(action.Name).Append(" : ").Append(action.Description).Append("\n");
				}
				else if (message.Author.Id == DataManager.master_id) {
					result.Append("- ").Append(action.Name).Append(" : ").Append(action.Description).Append(" [Hidden]\n");
				}
			}

			result.Append("\n[").Append(ActionType.Other).Append("]\n");
			foreach (var action in DataManager.otherActions) {
				result.Append("- ").Append(action.Name).Append(" : ").Append(action.Description).Append("\n");
			}
			
			await message.Channel.SendMessagesAsync(result.ToString());
		}
	}
}
