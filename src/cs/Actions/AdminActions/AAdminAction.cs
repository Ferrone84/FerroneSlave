
namespace DiscordBot.Actions.AdminActions
{
	public abstract class AAdminAction : Action
	{
		public AAdminAction()
		{
			Type = ActionType.Admin;
			Prefix = prefixs[Type];
			Accessibility = AccessibilityType.Visible;
			ChannelAccessibility = ChannelAccessibilityType.All;
		}
	}
}
