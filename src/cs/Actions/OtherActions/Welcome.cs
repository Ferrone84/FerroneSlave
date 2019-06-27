
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class Welcome : AOtherAction
	{
		public Welcome() : base()
		{
			Name = Prefix + "welcome";
			Regex = @"welcome|resident\s*evil\s*4";
			Description = "Meme Resident Evil 4.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://www.youtube.com/watch?v=o0kGvgXmmgk&ab_channel=DiegoSousa");
		}
	}
}
