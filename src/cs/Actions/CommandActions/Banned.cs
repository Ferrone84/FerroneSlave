
using System.Text;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.CommandActions
{
	public class Banned : ACommandAction
	{
		public Banned() : base()
		{
			Name = Prefix + "banned";
			Description = "Affiche la liste des gens bannis.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			StringBuilder result = new StringBuilder();

			if (DataManager.baned_people.Count == 0) {
				result.Append("La liste est vide.");
			}
			else {
				result.Append("Voici la liste des gens bannis : \n");
				foreach (var bannedUser in DataManager.baned_people) {
					result.Append("\t - " + DataManager._client.GetUser(bannedUser).ToString() + " (" + bannedUser + ")");
				}
			}

			await message.Channel.SendMessagesAsync(result.ToString());
		}
	}
}
