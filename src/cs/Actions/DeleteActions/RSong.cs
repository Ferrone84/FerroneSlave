
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.DeleteActions
{
	public class RSong : ADeleteAction
	{
		public RSong() : base()
		{
			Name = Prefix + "rsong";
			Description = "Random Song.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string result = string.Empty;
			try {
				string maxId = DataManager.database.getMaxId("musics");
				Random r = new Random();
				do {
					int rInt = r.Next(1, Convert.ToInt32(maxId));
					result = DataManager.database.getLineColumn("musics", "title", rInt.ToString());

				} while (result == String.Empty);

			}
			catch (Exception e) {
				e.DisplayException(Name);
			}

			result = result.RemoveChars(new List<char>() { '(', '\'', ')', ',' }).Replace("///", "://");
			
			await message.Channel.SendMessageAsync(result);
		}
	}
}
