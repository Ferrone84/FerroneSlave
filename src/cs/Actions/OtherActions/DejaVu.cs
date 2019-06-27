
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class DejaVu : AOtherAction
	{
		public DejaVu() : base()
		{
			Name = Prefix + "DEJA VU";
			Regex = @"d[ée]+j[aà]+\s*vu+";
			Description = "`" + Utilities.EmoteManager.TextEmoji.Smirk + "`";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://youtu.be/dv13gl0a-FA?t=60");
		}
	}
}
