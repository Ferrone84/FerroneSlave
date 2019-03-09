using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBot.Data;

namespace DiscordBot.Events.EventsHandlers
{
	public class ActionsMessageReceived : IGuildMessageReceivedEventHandler, IDMMessageReceivedEventHandler
	{
		public async Task DM_Message_Received(SocketUserMessage message)
		{
			await ActionsHandler(message);
		}

		public async Task Guild_Message_Received(SocketUserMessage message)
		{
			await ActionsHandler(message);
		}

		private async Task ActionsHandler(SocketUserMessage message)
		{
			try {
				string message_lower = message.Content.ToLower();
				string commandName = message_lower.Split(' ')[0];
				Actions.Action action = DataManager.actions.GetValueOrDefault(commandName);

				if (action?.CheckChannelAccessibility(message) == false) {
					await message.Channel.SendMessageAsync("Commande non accessible d'ici.");
					return;
				}
				switch (action?.Accessibility) {
					case Actions.Action.AccessibilityType.Usable:
						if (action.Type == Actions.Action.ActionType.Delete) {
							await message.DeleteAsync();
						}
						action.Use(message);
						break;
					case Actions.Action.AccessibilityType.Visible:
						if (message.Author.IsAdmin()) {
							action.Use(message);
						}
						else {
							await message.Channel.SendMessageAsync("Il faut être admin pour utiliser cette commande.");
						}
						break;
					case Actions.Action.AccessibilityType.Invisible:
						if (message.Author.Id == DataManager.master_id) {
							action.Use(message);
						}
						break;
				}

				if (action == null) {
					foreach (var action_ in DataManager.otherActions) {
						Regex regex = new Regex(action_.Regex);
						if (regex.Match(message_lower).Success) {
							action_.Use(message);
						}
					}
				}
			}
			catch (System.Exception e) {
				e.DisplayException("Nouveau mode de commande => " + System.Reflection.MethodBase.GetCurrentMethod().ToString());
			}
		}
	}
}
