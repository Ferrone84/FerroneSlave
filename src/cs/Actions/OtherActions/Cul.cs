
using System.Threading.Tasks;
using Discord;
using DiscordBot.Utilities;

namespace DiscordBot.Actions.OtherActions
{
	public class Cul : AOtherAction
	{
		public Cul() : base()
		{
			Name = Prefix + "cul";
			Regex = @"cul|ass";
			Description = "`" + EmoteManager.TextEmoji.Peach + "`";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.AddReactionAsync(EmoteManager.Peach);
		}
	}
}
