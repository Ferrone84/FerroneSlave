
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class NiceAndFluffy : AOtherAction
	{
		public NiceAndFluffy() : base()
		{
			Name = Prefix + "Nice and flufy";
			Regex = @Utilities.RegexUtils.MatchAllWordsDisordered("nice", "and", "fluffy");
			Description = "Cook-master.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/456443420378923010/593817969470210088/gordon-ramsey-meme.jpg");
		}
	}
}
