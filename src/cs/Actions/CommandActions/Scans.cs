
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Scans : ACommandAction
	{
		public Scans() : base()
		{
			Name = Prefix + "scans";
			Description = "Affiche le dernier scan pour chaque mangas traités.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			StringBuilder result = new StringBuilder("Voici, pour chaque manga, son dernier scan :\n\n");
			try {
				foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in Data.DataManager.mangasData) {
					string[] kvpValue = kvp.Value.Split(" => ");
					if (kvpValue.Length == 1) {
						kvpValue = new string[] {"Empty", "Empty"};
					}
					result.Append("```asciidoc\n[").Append(kvp.Key).Append("]```").Append(kvpValue[0]).Append("\n").Append(kvpValue[1]).Append("\n\n");
				}
			}
			catch (System.Exception e) {
				e.Display(Name);
				result = new StringBuilder(e.Message);
			}
			await message.Channel.SendMessagesAsync(result.ToString(), "\n\n");
		}
	}
}
