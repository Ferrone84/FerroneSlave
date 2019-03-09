
namespace DiscordBot.Actions.DeleteActions
{
	public abstract class ADeleteAction : Action
	{
		public ADeleteAction()
		{
			Type = ActionType.Delete;
			Prefix = prefixs[Type];
			Accessibility = AccessibilityType.Usable;
			ChannelAccessibility = ChannelAccessibilityType.All;
		}
	}
}
