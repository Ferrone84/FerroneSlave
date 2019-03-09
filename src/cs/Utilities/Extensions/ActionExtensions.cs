
using DiscordBot.Actions;
using DiscordBot.Actions.OtherActions;
using DiscordBot.Data;

namespace DiscordBot
{
	public static class ActionExtensions
	{
		public static void Register(this Action action)
		{
			DataManager.actions[action.Name] = action;
		}

		public static void Register(this AOtherAction action)
		{
			DataManager.otherActions.Add(action);
		}

		public static void Use(this Action action, Discord.IUserMessage message)
		{
			string actionName = action.Name;

			if (action.Accessibility != Action.AccessibilityType.Invisible) {
				var actions_used = DataManager.actions_used;

				if (actions_used.ContainsKey(actionName)) {
					actions_used[actionName]++;
				}
				else {
					actions_used.Add(actionName, 1);
				}
				Utilities.SaveStateManager.Save(DataManager.Binary.POP_ACTIONS_FILE, DataManager.actions_used);
			}

			new System.Threading.Thread(() => action.Invoke(message)).Start();
		}
	}
}
