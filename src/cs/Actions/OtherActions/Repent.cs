
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class Repent : AOtherAction
	{
		public Repent() : base()
		{
			Name = Prefix + "repent";
			Regex = Name;
			Description = "YOU WILL HAVE TO REPENT TO THIS MAN.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/309407896070782976/515615289988087808/repent.mp4");
		}
	}
}
