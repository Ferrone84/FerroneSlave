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
		public static class TextEmoji
		{
			public const string nsfw = "🔞";
			public const string check_mark = "✅";
			public const string cross_mark = "❎";
			public const string skull = "💀";
			public const string peach = "🍑";
			public const string smirk = "😏";
			public const string flip = "(╯°□°）╯︵ ┻━┻";
			public const string unflip = "┬─┬﻿ ノ( ゜-゜ノ)";
		}

		public static IEmote Nsfw { get; } = new Emoji(TextEmoji.nsfw);
		public static IEmote CheckMark { get; } = new Emoji(TextEmoji.check_mark);
		public static IEmote CrossMark { get; } = new Emoji(TextEmoji.cross_mark);
		public static IEmote Skull { get; } = new Emoji(TextEmoji.skull);
		public static IEmote Peach { get; } = new Emoji(TextEmoji.peach);
		public static IEmote Smirk { get; } = new Emoji(TextEmoji.smirk);
	}
}
