
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.AdminActions
{
	public class Say : AAdminAction
	{
		public Say() : base()
		{
			Name = Prefix + "say";
			Description = "Fait dire au bot le contenu du fichier say. ("+ Data.DataManager.Text.SAY_FILE + ")";
			Accessibility = AccessibilityType.Invisible;
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessagesAsync(System.IO.File.ReadAllText(Data.DataManager.Text.SAY_FILE));
		}
	}
}
