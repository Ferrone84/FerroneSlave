
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class BaldSignal : AOtherAction
	{
		public BaldSignal() : base()
		{
			Name = Prefix + "bald signal";
			Regex = @"bald\s*signal";
			Description = "C'est comme batman, mais en mieux.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync(Data.Users.Ferrone.Mention);
		}
	}
}
