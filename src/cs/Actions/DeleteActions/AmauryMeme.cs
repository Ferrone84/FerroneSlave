
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class AmauryMeme : ADeleteAction
	{
		public AmauryMeme() : base()
		{
			Name = Prefix + "amaury";
			Description = "Tout le monde sait ce que c'est.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/309407896070782976/353833262273134592/Sans_titre.png");
		}
	}
}
