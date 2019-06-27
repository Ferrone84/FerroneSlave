
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class Popopo : ADeleteAction
	{
		public Popopo() : base()
		{
			Name = Prefix + "popopo";
			Description = "Affiche le meme 'popopo'.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/346760327540506643/353847873458274304/popopo.png");
		}
	}
}
