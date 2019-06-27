
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class HanautaSancho : AOtherAction
	{
		public HanautaSancho() : base()
		{
			Name = Prefix + "hanauta sancho";
			Regex = @"hanauta|sancho|yahazu\s*giri";
			Description = "Le génie de Brook.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/346760327540506643/397856433145905156/yahazu_giri.png");
		}
	}
}
