
namespace DiscordBot.Actions.CommandActions
{
	public abstract class ACommandAction : Action
	{
		public ACommandAction()
		{
			Type = ActionType.Command;
			Prefix = prefixs[Type];
			Accessibility = AccessibilityType.Usable;
			ChannelAccessibility = ChannelAccessibilityType.All;
		}
	}
}
