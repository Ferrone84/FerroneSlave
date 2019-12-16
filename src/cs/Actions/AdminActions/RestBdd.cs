
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.AdminActions
{
	public class RestBdd : AAdminAction
	{
		public RestBdd() : base()
		{
			Name = Prefix + "restbdd";
			Description = "Restaure la bdd.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			try {
				System.IO.File.Copy(Data.DataManager.Binary.DB_FILE_SAVE, Data.DataManager.Binary.DB_FILE, true);
				await message.Channel.SendMessageAsync("La bdd a bien été restaurée.");
			}
			catch (System.Exception e) {
				e.Display(Name);
				await message.Channel.SendMessagesAsync(e.Message);
			}
		}
	}
}
