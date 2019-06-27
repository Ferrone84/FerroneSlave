
using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Date : ACommandAction
	{
		public Date() : base()
		{
			Name = Prefix + "date";
			Description = "Affiche la date.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string result = string.Empty;
			string message_lower = message.Content.ToLower();

			if (message_lower.Contains("day")) {
				result = DateTime.Now.Day.ToString();
			}
			else if (message_lower.Contains("month")) {
				result = DateTime.Now.Month.ToString();
			}
			else if (message_lower.Contains("year")) {
				result = DateTime.Now.Year.ToString();
			}
			else if (message_lower.Contains("time")) {
				result = DateTime.Now.TimeOfDay.ToString();
				result = result.Remove(8, result.Length - 8);
			}
			else
				result = DateTime.Now.ToString();

			await message.Channel.SendMessagesAsync(result);
		}
	}
}
