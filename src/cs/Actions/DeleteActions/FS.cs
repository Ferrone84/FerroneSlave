
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class FS : ADeleteAction
	{
		public FS() : base()
		{
			Name = Prefix + "fs";
			Description = "Formate la phrase qui suit avec de jolies lettres.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string msg = message.Content.ToLower().Substring(Name.Length).ToLower();
			
			if (msg == string.Empty) {
				await message.Channel.SendMessageAsync("$fs vide !");
				return;
			}

			if (message.Author.Id == Data.Users.Fluttershy.Id) {
				msg = FormateSentence(msg) + " by FlutterShy / Blossom / Pupute";
			}
			else {
				msg = FormateSentence(msg) + " by " + message.Author.Username;
			}

			await message.Channel.SendMessageAsync(msg);
		}

		private string FormateSentence(string sentence)
		{
			StringBuilder result = new StringBuilder();
			string indicator = ":regional_indicator_";

			for (int i = 0; i < sentence.Length; i++) {
				if (sentence[i] == ' ') {
					result.Append("     ");
				}
				else if ('a' <= sentence[i] && 'z' >= sentence[i]) {
					result.Append(indicator).Append(sentence[i]).Append(": ");
				}
				else {
					result.Append(sentence[i]).Append(" ");
				}
			}

			return result.ToString();
		}
	}
}
