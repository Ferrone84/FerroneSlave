using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using System.Threading;

namespace DiscordBot
{
	public class Program
	{
		public static Dictionary<string, ulong> channels = new Dictionary<string, ulong>()
		{
			{ "general",        309407896070782976 },
			{ "mangas",         439960408703369217 },
			{ "mangas_liste",   440228865881800704 },
			{ "musique",        472354528948387857 },
			{ "debug",          353262627880697868 },
			{ "debugs",         456443420378923010 },
			{ "zone51",         346760327540506643 },
			{ "peguts",         392118626561294346 }
		};

		public static Dictionary<string, string> pseudo = new Dictionary<string, string>()
		{
			{ "Ferrone",    "Ferrone" },
			{ "Luc",        "𝓕𝓵𝓾𝓽𝓽𝓮𝓻𝓢𝓱𝔂" },
			{ "Bruno",      "Faellyss" }
		};

		public static Dictionary<string, string> mention = new Dictionary<string, string>()
		{
			{ "Ferrone",    "<@293780484822138881>" },
			{ "Luc",        "<@150338863234154496>" },
			{ "Renaud",     "<@221984029900144640>" },
			{ "Bringer",    "<@231805289194586112>" },
			{ "Sacha",      "<@200728195652124673>" },
			{ "Dimitri",    "<@227738602312957961>" },
			{ "Pierre",     "<@229215949478297601>" },
			{ "Mayeul",     "<@313784177021550593>" },
			{ "Bruno",      "<@227490882033680384>" }
		};

		public static List<string> autres = new List<string>()
		{
			"bite",
			"musique de génie",
			"Gamabunta",
			"invocation",
			"welcome",
			"évidemment",
			"evidemment",
			"omae",
			"sancho",
			"detroit",
			"smash"
		};

		public static DiscordSocketClient _client;
		public static CancellationTokenSource delay_controller;
		public static ulong master_id = 293780484822138881;

		public static Actions actions;
		public static Database database;
		public static SocketGuild guild;
		public static List<string> pp_songs;
		public static List<string> baned_people;
		public static SortedDictionary<string, string> mangasData;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();

			_client.Log += Log;
			_client.Ready += ready;
			_client.MessageReceived += MessageReceived;
			delay_controller = new CancellationTokenSource();

			await _client.LoginAsync(TokenType.Bot, Utils.getToken());
			await _client.StartAsync();

			Console.CancelKeyPress += async delegate (object sender, ConsoleCancelEventArgs e)
			{
				e.Cancel = true;
				await deconnection();
			};

			// Block this task until the program is closed.
			try
			{
				await Task.Delay(-1, delay_controller.Token);
			}
			catch (TaskCanceledException)
			{
				await deconnection();
			}
		}

		private async Task deconnection()
		{
			try
			{
				Console.WriteLine("Le bot a bien été coupé.");
				_client.MessageReceived -= MessageReceived;
				await _client.LogoutAsync();
				await _client.StopAsync();
				_client.Dispose();
				Environment.Exit(0);
			}
			catch (Exception e)
			{
				Utils.displayException(e, "deconnection");
			}
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());

			return Task.CompletedTask;
		}

		private async Task ready()
		{
			//inits
			actions = new Actions();
			database = new Database();
			pp_songs = new List<string>();
			baned_people = new List<string>();
			mangasData = new SortedDictionary<string, string>();

			//mes setups
			Utils.setupPpSong();
			Utils.setupMangasData();
			guild = _client.GetGuild(309407896070782976);

			//Thread qui regarde les nouveaux scans
			Thread thread = new Thread(Utils.getAllNewChapters);
			thread.Start();

			//Thread qui regarde le temps de trajets
			//Thread traffic_thread = new Thread(fillTrafficData);
			//traffic_thread.Start();
		}

		private async Task MessageReceived(SocketMessage message)
		{
			if (baned_people.Contains(message.Author.Username) || message.Author.IsBot)
			{
				return;
			}

			string message_lower = message.Content.ToLower();

			if (message.Author.Id == master_id)
			{
				if (message_lower == "/q")
				{
					delay_controller.Cancel();
					return;
				}
			}

			if (message.Channel.Id == channels["musique"])
			{
				string msg = String.Empty;

				if ((msg = Utils.getYtLink(message.Content)) != String.Empty)
				{
					try
					{
						database.addMusic(msg);
						await ((SocketUserMessage)message).AddReactionAsync(new Emoji("✅"));
					}
					catch (Exception)
					{
						var emote = await guild.GetEmoteAsync(452977127722188811);
						await ((SocketUserMessage)message).AddReactionAsync(emote);
					}
				}
			}

			///////////////////////////////////////////////////////////////////
			//							  Execute command
			///////////////////////////////////////////////////////////////////

			try
			{
				if (message_lower.StartsWith("!!") && !Utils.verifyAdmin(message))
				{
					if (actions.actionExist(message_lower))
						await message.Channel.SendMessageAsync("Wesh t'es pas admin kestu fais le fou avec moi ?");
					else
						await message.Channel.SendMessageAsync("L'action demandée (" + message_lower + ") n'existe pas.");
					goto End;
				}

				foreach (var action in actions.getActions)
				{
					if (message_lower.StartsWith(action.Item1))
					{
						string msg = action.Item3.Invoke(message);

						if (message_lower.StartsWith("$"))
							Utils.DeleteMessage(message);

						if (msg.Contains("|"))
							foreach (string ms in msg.Split('|'))
								await message.Channel.SendMessageAsync(ms);
						else if (msg != String.Empty)
							await message.Channel.SendMessageAsync(msg);

						break;
					}
					else if (autres.Contains(action.Item1) && message_lower.Contains(action.Item1))
					{
						string msg = action.Item3.Invoke(message);
						if (msg != String.Empty) { await message.Channel.SendMessageAsync(msg); }
					}
				}
			}
			catch (Exception e)
			{
				Utils.displayException(e, "Main foreach actions");
				await message.Channel.SendMessageAsync("La commande n'as pas fonctionnée comme prévu.");
			}

			///////////////////////////////////////////////////////////////////
			//							  Automatique
			///////////////////////////////////////////////////////////////////

			/*if (message_lower.Contains("#") && message.Author.Username != "Ferrone")
			{
				string msg = "Les hashtags c'est démodé quand même :/ depuis 2012 connard.";
				await message.Channel.SendMessageAsync(msg);
			}*/
			if ((message_lower.Contains("bald") && message_lower.Contains("signal")) || message_lower.Contains("baldsignal"))
			{
				string msg = mention["Ferrone"];
				await message.Channel.SendMessageAsync(msg);
			}

			///////////////////////////////////////////////////////////////////
			//							  Debug Zone
			///////////////////////////////////////////////////////////////////

			if (message_lower.StartsWith("!d"))
			{
				/*string msg = "";
				sendMessageTo(channels["debug"], msg);
				Console.WriteLine(msg);*/

				/*
				database.init();
				database.loadUsers();
				database.loadMangas();
				database.addUser("293780484822138881", "ferrone", "nico", 1).aff();
				database.addUser("150338863234154496", "fluttershy", "luc", 0).aff();
				database.subTo("150338863234154496", "one-piece").aff();
				database.makeQuery("UPDATE users SET pseudo=? WHERE uid=?", "Faellyss:227490882033680384").aff();
				*/
				try
				{

				}
				catch (Exception e)
				{
					Utils.displayException(e, "!d");
					foreach (var errors in Utils.moreThanTwoThousandsChars(e.Message + "\n" + e.StackTrace).Split('|'))
						await message.Channel.SendMessageAsync(errors);
				}

				return;
			}

		End:
			string logprint = "Message reçu de " + message.Author.Username + " : " + message_lower;
			Console.WriteLine(logprint);
			//if (message.Channel.Id == channels["general"])
			System.IO.File.AppendAllText(Utils.LOGS_FILE_NAME, logprint + "\n");
		}
	}
}
