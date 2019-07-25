
using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Dice : ACommandAction
	{
		public Dice() : base()
		{
			Name = Prefix + "dice";
			Description = "Lance un dé.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			int maxRoll = 100;
			var words = message.Content.ToLower().Split(' ');

			if (words.Length >= 2) {
				try {
					maxRoll = Int32.Parse(words[1]);
				}
				catch (System.Exception) {
					await message.Channel.SendMessageAsync("Y'en a toujours un pour abuser hein ? Fdp va."); //(2.147.483.647)
					return;
				}
				if (maxRoll < 1) {
					await message.Channel.SendMessageAsync("T'as déjà vu un dé avec des faces nulles ou négatives dukon ?");
					return;
				}
			}
			await message.Channel.SendMessageAsync("Vous avez obtenu un **" + roll(maxRoll) + "** " + Utilities.EmoteManager.GameDie + " !");
		}

		private int roll(int max = 100) {
			return new System.Random().Next(max) + 1;
		}
	}
}
