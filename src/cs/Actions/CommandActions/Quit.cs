
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Quit : ACommandAction
	{
		public Quit() : base()
		{
			Name = "/q";
			Description = "Shut down the system.";
			Accessibility = AccessibilityType.Invisible;
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("Good bye");
			Data.DataManager.delay_controller.Cancel();
		}
	}
}
