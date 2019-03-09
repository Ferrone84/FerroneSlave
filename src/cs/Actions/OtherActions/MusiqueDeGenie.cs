
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class MusiqueDeGenie : AOtherAction
	{
		public MusiqueDeGenie() : base()
		{
			Name = Prefix + "musique de génie";
			Regex = @"musique de g[ée]nie";
			Description = "Le jour où tu veux écouter de la vrai musique.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://www.youtube.com/watch?v=kXYiU_JCYtU");
		}
	}
}
