using System;
using System.Linq;
using System.Reflection;

namespace DiscordBot.Utilities
{
	public class ActionUtils
	{
		public static void RegisterActions(string nameSpace)
		{
			string[] badTypes = new string[] { "Action", "AOtherAction", "ADeleteAction", "ACommandAction", "AAdminAction", "ActionType", "AccessibilityType", "ChannelAccessibilityType", "<Invoke>d__1", "<>c" };

			try {
				var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith(nameSpace));
				foreach (var t in types) {
					if (!badTypes.Contains(t.Name)) {
						if (nameSpace == "DiscordBot.Actions.OtherActions") {
							(t.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as Actions.OtherActions.AOtherAction).Register();
						}
						else {
							(t.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as Actions.Action).Register();
						}
					}
				}
			}
			catch (Exception e) {
				e.DisplayException(System.Reflection.MethodBase.GetCurrentMethod().ToString());
			}
		}
	}
}
