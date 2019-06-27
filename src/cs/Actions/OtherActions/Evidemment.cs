
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class Evidemment : AOtherAction
	{
		public Evidemment() : base()
		{
			Name = Prefix + "évidemment";
			Regex = @"[ée]videmment";
			Description = "Meme Antoine Daniel.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://media.tenor.com/images/7f54355b70208666f7e9ed5f657b4471/tenor.gif");
		}
	}
}
