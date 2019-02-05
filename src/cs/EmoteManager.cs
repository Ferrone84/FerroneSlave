using System;
using System.IO;
using System.Collections.Generic;

using Discord;
using Discord.WebSocket;
using System.Threading;


namespace DiscordBot
{
	public class EmoteManager
	{
		static class TextEmoji
		{
			public const string nsfw = "🔞";
			public const string check_mark = "✅";
			public const string cross_mark = "❎";
			public const string skull = "💀";
		}

		public static IEmote Nsfw { get; } = new Emoji(TextEmoji.nsfw);
		public static IEmote CheckMark { get; } = new Emoji(TextEmoji.check_mark);
		public static IEmote CrossMark { get; } = new Emoji(TextEmoji.cross_mark);
		public static IEmote Skull { get; } = new Emoji(TextEmoji.skull);
	}
}
