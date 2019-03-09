
using System.Threading.Tasks;
using Discord;
using DiscordBot.Utilities;

namespace DiscordBot.Actions.OtherActions
{
	public class Flip : AOtherAction
	{
		public Flip() : base()
		{
			Name = Prefix + EmoteManager.TextEmoji.Flip;
			Regex = Prefix + EmoteManager.TextEmoji.Flip + ")";
			Description = EmoteManager.TextEmoji.Unflip;
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync(EmoteManager.TextEmoji.Unflip);
		}
	}
}
