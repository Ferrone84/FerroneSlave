
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class UnFlip : ACommandAction
	{
		public UnFlip() : base()
		{
			Name = Prefix + "unflip";
			Description = "Replace une table.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync(Utilities.EmoteManager.TextEmoji.Unflip);
		}
	}
}
