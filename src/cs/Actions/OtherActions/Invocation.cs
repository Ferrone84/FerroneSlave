
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class Invocation : AOtherAction
	{
		public Invocation() : base()
		{
			Name = Prefix + "invocation";
			Description = "Meme naruto.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("Kuchiyose no jutsu ! https://cdn.discordapp.com/attachments/353262627880697868/548627093911633920/FB_IMG_14720248103338365.jpg");
		}
	}
}
