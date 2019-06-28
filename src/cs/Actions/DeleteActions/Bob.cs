
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class Bob : ADeleteAction
	{
		public Bob() : base()
		{
			Name = Prefix + "bob";
			Description = "I LikE ThIs!";
		}

		public override async Task Invoke(IUserMessage message)
		{
			StringBuilder result = new StringBuilder();
			string msg = message.Content.ToLower().Replace(Name+' ', "");
			if (message.Content == Name || msg == string.Empty) { 
				await message.Channel.SendMessageAsync("Il faut spécifier un message à cette commande. ("+Name+" bruno est moche)");
				return; 
			}

			foreach (char letter in msg) {
				result.Append( (new System.Random().Next(2) == 0) ? char.ToUpper(letter) : letter);
			}
			await message.Channel.SendMessageAsync(result.ToString());
		}
	}
}
