
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.AdminActions
{
	public class SaveBdd : AAdminAction
	{
		public SaveBdd() : base()
		{
			Name = Prefix + "savebdd";
			Description = "Sauvegarde la bdd.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			try {
				System.IO.File.Copy(Data.DataManager.Binary.DB_FILE, Data.DataManager.Binary.DB_FILE_SAVE, true);
				await message.Channel.SendMessageAsync("La bdd a bien été sauvegardée.");
			}
			catch (System.Exception e) {
				e.Display(Name);
				await message.Channel.SendMessagesAsync(e.Message);
			}
		}
	}
}
