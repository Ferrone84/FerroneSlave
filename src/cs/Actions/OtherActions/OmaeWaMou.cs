
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class OmaeWaMou : AOtherAction
	{
		public OmaeWaMou() : base()
		{
			Name = Prefix + "omae wa mou shindeiru";
			Regex = @"omae|(wa mou shindeiru)|shindeiru";
			Description = "NANI !?";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("NANIIII !!??");
		}
	}
}
