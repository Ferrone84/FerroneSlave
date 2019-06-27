
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class MythoUltime : ADeleteAction
	{
		public MythoUltime() : base()
		{
			Name = Prefix + "mytho";
			Description = "El mytho ultima.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/346760327540506643/402253939573129217/amaury_ultime.jpg");
		}
	}
}
