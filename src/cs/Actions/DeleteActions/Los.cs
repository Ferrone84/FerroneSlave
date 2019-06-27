
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.DeleteActions
{
	public class Los : ADeleteAction
	{
		public Los() : base()
		{
			Name = Prefix + "los";
			Description = "Trigger la dreamteam de LOS !!";
		}

		public override async Task Invoke(IUserMessage message)
		{
			if (message.Author.Id == 150338863234154496) {
				await message.Channel.SendMessageAsync("Luc qui casse les couilles à vouloir trigger le LOS !");
				return;
			}

			StringBuilder msg = new StringBuilder("LOS ? ");
			string message_lower = message.Content.ToLower();

			foreach (var user in Data.DataManager.guild.Users) {
				foreach (var role in user.Roles) {
					if (role.Id == 471428502621650947 && user.Id != message.Author.Id) {
						msg.Append("<@").Append(user.Id).Append("> ");
					}
				}
			}
			msg.Append(" - ").Append(message.Author.Username).Append(" veut jouer !");

			if (message_lower != Name) {
				msg.Append(" [").Append(message_lower.Substring(5)).Append("]");
			}

			await message.Channel.SendMessageAsync(msg.ToString());
		}
	}
}
