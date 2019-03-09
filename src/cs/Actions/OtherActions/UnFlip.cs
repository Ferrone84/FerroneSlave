
using System.Threading.Tasks;
using Discord;
using DiscordBot.Utilities;

namespace DiscordBot.Actions.OtherActions
{
	public class UnFlip : AOtherAction
	{
		public UnFlip() : base()
		{
			Name = Prefix + EmoteManager.TextEmoji.Unflip;
			Regex = Prefix + EmoteManager.TextEmoji.Unflip.Replace("(", "\\(").Replace(")", "\\)");
			Description = EmoteManager.TextEmoji.Flip;
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync(EmoteManager.TextEmoji.Flip);
		}
	}
}
