
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class Fap : ADeleteAction
	{
		public Fap() : base()
		{
			Name = Prefix + "fap";
			Description = "Si t'aime te fap ;)";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://giphy.com/gifs/fap-Bk2NzCbwFH6sE");
		}
	}
}
