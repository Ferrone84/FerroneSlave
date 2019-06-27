
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class Latata : AOtherAction
	{
		public Latata() : base()
		{
			Name = Prefix + "latata";
			Regex = @"l+a+t+a+t+a+";
			Description = "Do I really need to say something?";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://www.youtube.com/watch?v=9mQk7Evt6Vs");
		}
	}
}
