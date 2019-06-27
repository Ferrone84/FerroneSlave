using System.Collections.Generic;
using System.Threading.Tasks;

using Discord.WebSocket;
using DiscordBot.Data;

namespace DiscordBot.Events.EventsHandlers
{
	public class BanListenerMessageReceived : IGuildMessageReceivedEventHandler
	{
		public async Task Guild_Message_Received(SocketUserMessage message)
		{
			if (message.Author.Id == DataManager.master_id || message.Author.Id == Users.Bot.Id || DataManager.baned_people.Contains(message.Author.Id)) {
				return;
			}

			ulong authorId = message.Author.Id;
			if (!DataManager.people_spam.ContainsKey(authorId)) {
				DataManager.people_spam.Add(authorId, 0);
			}
			else {
				DataManager.people_spam[authorId]++;
			}

			foreach (KeyValuePair<ulong, int> kvp in DataManager.people_spam) {
				if (kvp.Value > 4) {
					SocketUser user = DataManager._client.GetUser(kvp.Key);
					await message.Channel.SendMessageAsync("You, [" + user.Mention + "], are a spammer ! You got banned from the bot-services for undetermined time !");
					DataManager.baned_people.Add(kvp.Key);
					Utilities.SaveStateManager.Save(DataManager.Binary.BANNED_FILE, DataManager.baned_people);
				}
			}
		}
	}
}
