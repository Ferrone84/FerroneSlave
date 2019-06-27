
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.AdminActions
{
	public class Ban : AAdminAction
	{
		public Ban() : base()
		{
			Name = Prefix + "ban";
			Description = "Ajoute un utilisateur à la liste des utilisateurs bannis.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string msg = "Il faut mettre l'id ou la mention de la personne. Ex : " + Name + " 227490882033680384";
			try {
				var userId = message.Content.Split(' ')[1];
				ulong userId_ = System.Convert.ToUInt64(userId.OnlyKeepDigits());
				msg = "Cet utilisateur était déjà banni.";

				if (userId_ == DataManager.master_id) {
					msg = "Bien essayé, mais non. 😏";
					await message.Channel.SendMessageAsync(msg);
					return;
				}

				if (!DataManager.baned_people.Contains(userId_)) {
					DataManager.baned_people.Add(userId_);
					msg = "L'utilisateur a bien été banni.";
				}

				Utilities.SaveStateManager.Save(DataManager.Binary.BANNED_FILE, DataManager.baned_people);
			}
			catch (System.Exception e) {
				e.DisplayException(Name);
			}

			await message.Channel.SendMessageAsync(msg);
		}
	}
}
