
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.AdminActions
{
	public class Display : AAdminAction
	{
		public Display() : base()
		{
			Name = Prefix + "display";
			Description = "Affiche la bdd.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessagesAsync(Data.DataManager.database.GetDisplayedTables());
		}
	}
}
