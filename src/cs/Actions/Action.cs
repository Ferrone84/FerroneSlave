
using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Actions
{
	public abstract class Action
	{
		protected string name;
		protected string prefix;
		protected string description;
		protected ActionType type;
		protected AccessibilityType accessibility;
		protected ChannelAccessibilityType channelAccessibility;

		public string Name { get { return name; } protected set { name = value; } }
		public string Prefix { get { return prefix; } protected set { prefix = value; } }
		public string Description { get { return description; } protected set { description = value; } }
		public ActionType Type { get { return type; } protected set { type = value; } }
		public AccessibilityType Accessibility { get { return accessibility; } protected set { accessibility = value; } }
		public ChannelAccessibilityType ChannelAccessibility { get { return channelAccessibility; } protected set { channelAccessibility = value; } }

		public enum ActionType
		{
			Command,
			Admin,
			Delete,
			Other
		}

		public enum AccessibilityType
		{
			Usable,
			Visible,
			Invisible
		}

		public enum ChannelAccessibilityType
		{
			DM,
			Guild,
			All
		}

		protected Dictionary<ActionType, string> prefixs = new Dictionary<ActionType, string>() {
			{ ActionType.Command, "!" },
			{ ActionType.Admin, "!!" },
			{ ActionType.Delete, "$" },
			{ ActionType.Other, string.Empty },
		};

		public abstract Task Invoke(IUserMessage message);

		public bool CheckChannelAccessibility(IUserMessage message)
		{
			bool result = false;
			IMessageChannel channel = message.Channel;

			switch (ChannelAccessibility) {
				case ChannelAccessibilityType.DM:
					result = channel is SocketDMChannel;
					break;
				case ChannelAccessibilityType.Guild:
					result = channel is SocketGuildChannel;
					break;
				case ChannelAccessibilityType.All:
					result = (channel is SocketDMChannel || channel is SocketGuildChannel);
					break;
				default:
					result = false;
					break;
			}

			return result;
		}
	}
}
