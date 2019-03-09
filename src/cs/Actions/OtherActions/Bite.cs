
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Utilities;

namespace DiscordBot.Actions.OtherActions
{
	public class Bite : AOtherAction
	{
		public Bite() : base()
		{
			Name = Prefix + "bite";
			Regex = @"bite|bit";
			Description = "Si ta phrase contient une ou plusieurs bite(s), alors PEPE*nombre_de_bites.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			Regex regex = new Regex(Regex);
			StringBuilder result = new StringBuilder();
			IEmote pepe = EmoteManager.Guilds.Zawarudo.Pepe;
			int pepeCount = regex.Matches(message.ToString().ToLower()).Count;
			string append = (pepe.ToString() == EmoteManager.TextEmoji.InvalidEmote) ? EmoteManager.TextEmoji.Smirk : pepe.ToString();

			for (int i = 0; i < pepeCount; i++) {
				result.Append(append);
			}

			await message.Channel.SendMessagesAsync(result.ToString());
		}
	}
}
