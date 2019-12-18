using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;
using DiscordBot.Utilities;

namespace DiscordBot.Events.EventsHandlers
{
	public class Ready : ISelfReadyEventHandler
	{
		public async Task Self_Ready()
		{
			//inits
			DataManager.database = new Database();
			DataManager.pp_songs = Utils.InitPpSong();
			DataManager.mangasData = Utils.InitMangasData();
			DataManager.people_spam = new Dictionary<ulong, int>();
			DataManager.actions = new Dictionary<string, Actions.Action>();
			DataManager.otherActions = new List<Actions.OtherActions.AOtherAction>();
			DataManager.nsfw_content_inprocess = new Dictionary<IUserMessage, IUserMessage>();
			DataManager.baned_people = SaveStateManager.Load<List<ulong>>(DataManager.Binary.BANNED_FILE) ?? new List<ulong>();
			DataManager.actions_used = SaveStateManager.Load<Dictionary<string, int>>(DataManager.Binary.POP_ACTIONS_FILE) ?? new Dictionary<string, int>();

			//TODO déplacer ça dans la config
			ActionUtils.RegisterActions("DiscordBot.Actions.CommandActions");
			ActionUtils.RegisterActions("DiscordBot.Actions.AdminActions");
			ActionUtils.RegisterActions("DiscordBot.Actions.DeleteActions");
			ActionUtils.RegisterActions("DiscordBot.Actions.OtherActions");

			//mes setups
			Utils.Init();
			DataManager.guild = DataManager._client.GetGuild(309407896070782976);

			try {
				if (!Utils.IsTestBot) {
					//Thread qui regarde les nouveaux scans
					new Thread(ThreadUtils.MangasCrawlerJapscan).Start();
					new Thread(ThreadUtils.QwerteeThread).Start();
				}

				new Thread(ThreadUtils.EmptyBannedPeopleStackThread).Start();
			}
			catch (System.Exception e) {
				e.Display("Threads ready");
				await Channels.Problems.SendMessagesAsync(e.Message);
			}

			await Channels.Debugs.SendMessageAsync("Bot ready");
		}
	}
}
