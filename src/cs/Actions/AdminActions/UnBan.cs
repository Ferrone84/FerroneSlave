
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.AdminActions
{
	public class UnBan : AAdminAction
	{
		public UnBan() : base()
		{
			Name = Prefix + "unban";
			Description = "Retire un utilisateur de la liste des utilisateurs bannis.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string msg = "Il faut mettre l'id ou la mention de la personne. Ex : " + Name + " 227490882033680384";
			try {
				var userId = message.Content.Split(' ')[1];
				ulong userId_ = System.Convert.ToUInt64(userId.OnlyKeepDigits());
				msg = "Cet utilisateur n'était pas banni.";

				if (DataManager.baned_people.Contains(userId_)) {
					DataManager.baned_people.Remove(userId_);
					msg = "L'utilisateur a bien été retiré des utilisateurs bannis.";
				}

				Utilities.SaveStateManager.Save(DataManager.Binary.BANNED_FILE, DataManager.baned_people);
			}
			catch (System.Exception e) {
				e.Display(Name);
			}

			await message.Channel.SendMessageAsync(msg);
		}
	}
}
