
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class Lenny : ADeleteAction
	{
		public Lenny() : base()
		{
			Name = Prefix + "lenny";
			Description = "Affiche le meme 'lenny'.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync(Utilities.EmoteManager.TextEmoji.Lenny);
		}
	}
}
