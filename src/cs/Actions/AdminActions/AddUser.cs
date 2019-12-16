
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.AdminActions
{
	public class AddUser : AAdminAction
	{
		public AddUser() : base()
		{
			Name = Prefix + "adduser";
			Description = "Ajoute un utilisateur à la bdd.";
			Accessibility = AccessibilityType.Invisible;
		}

		public override async Task Invoke(IUserMessage message)
		{
			string errorMessage = "Il faut rentrer des arguments. Ex : " + Name + " 293780484822138881 ferrone nico";

			if (message.Content.ToLower().Length <= Name.Length) {
				await message.Channel.SendMessageAsync(errorMessage);
				return;
			}
			try {
				var args = message.Content.Split(' ');
				Data.DataManager.database.addUser(args[1], args[2], args[3]);
			}
			catch (System.Exception e) {
				e.Display(Name);
				await message.Channel.SendMessageAsync(errorMessage);
			}
		}
	}
}
