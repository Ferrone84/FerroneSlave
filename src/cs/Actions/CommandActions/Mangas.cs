
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Mangas : ACommandAction
	{
		public Mangas() : base()
		{
			Name = Prefix + "mangas";
			Description = "Affiche la liste des mangas traités.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			int number = 1;
			StringBuilder result = new StringBuilder("Voici les mangas qui sont check avec le web parser :\n");

			foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in Data.DataManager.mangasData) {
				result.Append("- ").Append(kvp.Key).Append(" (**").Append(number).Append("**)\n");
				number++;
			}
			
			await message.Channel.SendMessagesAsync(result.ToString());
		}
	}
}
