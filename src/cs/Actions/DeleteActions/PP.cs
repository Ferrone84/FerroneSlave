
using System;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.DeleteActions
{
	public class PP : ADeleteAction
	{
		public PP() : base()
		{
			Name = Prefix + "pp";
			Description = "Random PP Song.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			Random r = new Random();
			int rInt = r.Next(0, DataManager.pp_songs.Count - 1);
			string result = DataManager.pp_songs[rInt];

			await message.Channel.SendMessageAsync(result);
		}
	}
}
