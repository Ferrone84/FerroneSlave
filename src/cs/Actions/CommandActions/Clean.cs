
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Clean : ACommandAction
	{
		public Clean() : base()
		{
			Name = Prefix + "clean";
			Description = "Clean l'espace de message.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			StringBuilder result = new StringBuilder("Clean en cours...");

			for (int i = 0; i < 60; i++) {
				result.Append("\n");
			}
			result.Append("\nClean terminé.");

			await message.Channel.SendMessagesAsync(result.ToString());
		}
	}
}
