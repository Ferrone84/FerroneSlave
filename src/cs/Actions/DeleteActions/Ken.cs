
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class Ken : ADeleteAction
	{
		public Ken() : base()
		{
			Name = Prefix + "ken";
			Description = "Tu ne le sais pas, mais tu es déjà mort.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("OMAE WA MOU ... SHINDEIRU !");
		}
	}
}
