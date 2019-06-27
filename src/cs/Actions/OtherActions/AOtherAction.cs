
namespace DiscordBot.Actions.OtherActions
{
	public abstract class AOtherAction : Action
	{
		protected string regex;
		public string Regex { get { return regex; } protected set { regex = value; } }

		public AOtherAction()
		{
			Regex = @"(?=a)b";
			Type = ActionType.Other;
			Prefix = prefixs[Type];
			Accessibility = AccessibilityType.Usable;
			ChannelAccessibility = ChannelAccessibilityType.All;
		}
	}
}
