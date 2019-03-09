using Discord;
using Discord.WebSocket;
using DiscordBot.Data;
using System.Threading.Tasks;

namespace DiscordBot.Utilities
{
	public class EmoteManager
	{
		public static class TextEmoji
		{
			public const string Nsfw = @"🔞";
			public const string CheckMark = @"✅";
			public const string CrossMark = @"❎";
			public const string Skull = @"💀";
			public const string Peach = @"🍑";
			public const string Smirk = @"😏";
			public const string Flip = @"(╯°□°）╯︵ ┻━┻";
			public const string Unflip = @"┬─┬﻿ ノ( ゜-゜ノ)";
			public const string Lenny = @"( ͡° ͜ʖ ͡°)";
			public const string InvalidEmote = @"❌";
		}

		public static IEmote Nsfw { get; } = new Emoji(TextEmoji.Nsfw);
		public static IEmote CheckMark { get; } = new Emoji(TextEmoji.CheckMark);
		public static IEmote CrossMark { get; } = new Emoji(TextEmoji.CrossMark);
		public static IEmote Skull { get; } = new Emoji(TextEmoji.Skull);
		public static IEmote Peach { get; } = new Emoji(TextEmoji.Peach);
		public static IEmote Smirk { get; } = new Emoji(TextEmoji.Smirk);
		public static IEmote InvalidEmote { get; } = new Emoji(TextEmoji.InvalidEmote);

		public struct Guilds
		{
			public static class Zawarudo
			{
				private static readonly SocketGuild guild = DataManager._client.GetGuild(309407896070782976);

				public static IEmote Pepe => GetEmote(329281047730585601, guild).Result ?? InvalidEmote;
				public static IEmote Aret => GetEmote(452977127722188811, guild).Result ?? InvalidEmote;
				public static IEmote Mickey => GetEmote(452977414440615976, guild).Result ?? InvalidEmote;
			}

			public static class Prod
			{
				private static readonly SocketGuild guild = DataManager._client.GetGuild(456443419896709123);

				public static IEmote Ban => GetEmote(553719355322531851, guild).Result ?? InvalidEmote;
				public static IEmote Minus => GetEmote(553716950979575819, guild).Result ?? InvalidEmote;
				public static IEmote Plus => GetEmote(553716932826890250, guild).Result ?? InvalidEmote;
				public static IEmote Edit => GetEmote(553716553502163070, guild).Result ?? InvalidEmote;
			}

			public static class Tests
			{
				private static readonly SocketGuild guild = DataManager._client.GetGuild(543925483008426016);

				public static IEmote Taric => GetEmote(553721390260289662, guild).Result ?? InvalidEmote;
			}
		}

		private static async Task<GuildEmote> GetEmote(ulong idEmoji, IGuild guild)
		{
			try {
				return await guild.GetEmoteAsync(idEmoji);
			}
			catch (System.Exception) {
				return null;
			}
		}
	}
}
