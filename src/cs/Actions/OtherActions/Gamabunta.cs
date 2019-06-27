
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class Gamabunta : AOtherAction
	{
		public Gamabunta() : base()
		{
			Name = Prefix + "gamabunta";
			Regex = @Utilities.RegexUtils.MatchAllWordsDisordered("naruto", "boss")+"|gamabunta";
			Description = "Meme naruto du BOSS.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("Kuchiyose no jutsu ! https://vignette3.wikia.nocookie.net/naruto/images/8/84/Gamabunta.png/revision/latest?cb=20160623114719");
		}
	}
}
