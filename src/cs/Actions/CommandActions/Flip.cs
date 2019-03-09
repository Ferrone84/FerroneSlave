
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Flip : ACommandAction
	{
		public Flip() : base()
		{
			Name = Prefix + "flip";
			Description = "Lance une table.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync(Utilities.EmoteManager.TextEmoji.Flip);
		}
	}
}
