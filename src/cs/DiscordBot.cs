using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using System.Threading;

using DiscordBot.Data;
using DiscordBot.Events;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace DiscordBot
{
	public class DiscordBot
	{
		private EventHandlersManager eventHandlersManager;

		public async Task MainAsync()
		{
			DiscordSocketConfig discordSocketConfig = new DiscordSocketConfig {
				MessageCacheSize = 100
			};

			DataManager._client = new DiscordSocketClient(discordSocketConfig);
			eventHandlersManager = new EventHandlersManager(DataManager._client);
			DataManager._client.Log += Log;
			try {
				eventHandlersManager.AddHandlers(GetAllEventsHandlers("DiscordBot.Events.EventsHandlers").ToArray());
			}
			catch (Exception e) { e.Display("MainAsync() => EventHandlersManager.AddHandlers"); }

			DataManager.delay_controller = new CancellationTokenSource();
			await DataManager._client.LoginAsync(TokenType.Bot, Utils.Token);
			await DataManager._client.StartAsync();

			Console.CancelKeyPress += async delegate (object sender, ConsoleCancelEventArgs e) {
				e.Cancel = true;
				await Deconnection();
			};

			// Block this task until the program is closed.
			try {
				await Task.Delay(-1, DataManager.delay_controller.Token);
			}
			catch (TaskCanceledException) {
				await Deconnection();
			}
		}

		private async Task Deconnection()
		{
			try {
				Console.WriteLine("Le bot a bien été coupé.");
				eventHandlersManager.RemoveEvents(DataManager._client);
				DataManager._client.Log -= Log;
				await DataManager._client.LogoutAsync();
				await DataManager._client.StopAsync();
				DataManager._client.Dispose();
				Environment.Exit(0);
			}
			catch (Exception e) {
				e.Display(MethodBase.GetCurrentMethod().ToString());
			}
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());

			return Task.CompletedTask;
		}

		private List<IEventHandler> GetAllEventsHandlers(string nameSpace)
		{
			List<IEventHandler> eventsHandlers = new List<IEventHandler>();
			
			try {
				var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace.StartsWith(nameSpace));
				foreach (var t in types) {
					if (!t.Name.Contains("d_")) {
						eventsHandlers.Add((t.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes) as IEventHandler));
					}
				}
			}
			catch (Exception e) {
				e.Display(MethodBase.GetCurrentMethod().ToString());
			}
			return eventsHandlers;
		}
	}
}
