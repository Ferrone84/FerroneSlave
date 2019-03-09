
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class DetroitSmash : AOtherAction
	{
		public DetroitSmash() : base()
		{
			Name = Prefix + "detroit smash";
			Regex = @"d+e+t+r+o+i+t+\s*s+m+a+s+h+";
			Description = "Cqfd.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://giphy.com/gifs/hero-smash-boku-XE8j547LpglrO");
		}
	}
}
