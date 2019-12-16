
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.OtherActions
{
	public class Repent : AOtherAction
	{
		public Repent() : base()
		{
			Name = Prefix + "repent";
			Description = "YOU WILL HAVE TO REPENT TO THIS MAN.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			"ici".Debug();
			await message.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/309407896070782976/515615289988087808/repent.mp4");
			"la".Debug();
		}
	}
}
