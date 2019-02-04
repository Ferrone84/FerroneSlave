using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using System.Threading;
using System.Text.RegularExpressions;

namespace DiscordBot
{
	public class Program
	{
		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			Data._client = new DiscordSocketClient();

			Data._client.Log += Log;
			Data._client.Ready += ready;
			Data._client.MessageReceived += MessageReceived;
			Data._client.MessageReceived += MessageReceivedBanListener;
			Data._client.ReactionAdded += ReactionAdded;
			Data.delay_controller = new CancellationTokenSource();

			await Data._client.LoginAsync(TokenType.Bot, Utils.getToken());
			await Data._client.StartAsync();

			Console.CancelKeyPress += async delegate (object sender, ConsoleCancelEventArgs e) {
				e.Cancel = true;
				await deconnection();
			};

			// Block this task until the program is closed.
			try {
				await Task.Delay(-1, Data.delay_controller.Token);
			}
			catch (TaskCanceledException) {
				await deconnection();
			}
		}

		private async Task deconnection()
		{
			try {
				Console.WriteLine("Le bot a bien été coupé.");
				Data._client.MessageReceived -= MessageReceived;
				Data._client.MessageReceived -= MessageReceivedBanListener;
				Data._client.ReactionAdded -= ReactionAdded;
				await Data._client.LogoutAsync();
				await Data._client.StopAsync();
				Data._client.Dispose();
				Environment.Exit(0);
			}
			catch (Exception e) {
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
			Data.actions = new Actions();
			Data.database = new Database();
			Data.autres = new List<string>();
			Data.pp_songs = new List<string>();
			Data.people_spam = new Dictionary<ulong, int>();
			Data.mangasData = new SortedDictionary<string, string>();
			Data.nsfw_content_inprocess = new Dictionary<IUserMessage, IUserMessage>();
			Data.baned_people = SaveStateManager.Load<List<ulong>>(Data.Binary.BANNED_FILE) ?? new List<ulong>();
			Data.actions_used = SaveStateManager.Load<Dictionary<string, int>>(Data.Binary.POP_ACTIONS_FILE) ?? new Dictionary<string, int>();

			//mes setups
			Utils.init();
			Utils.setupPpSong();
			Utils.setupMangasData();
			Utils.setupOtherActionsList();
			Data.guild = Data._client.GetGuild(309407896070782976);

			//Thread qui regarde les nouveaux scans
			Thread mangas_thread = new Thread(Utils.mangasCrawlerOnLireScanV2);
			mangas_thread.Start();

			Thread people_spam_thread = new Thread(Utils.emptyBannedPeopleStack);
			people_spam_thread.Start();

			await Utils.sendMessageTo(Data.channels["debugs"], "Bot ready");
		}

		private async Task ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
		{
			try {
				IUserMessage message = channel.GetMessageAsync(cachedMessage.Id).Result as IUserMessage;

				if (Data.baned_people.Contains(reaction.User.Value.Id) || reaction.User.Value.IsBot) {
					return;
				}

				if (Utils.isAdmin(reaction.UserId)) {
					IUserMessage nsfwMessage = Utils.isThisNsfwInProgress(message);

					if (nsfwMessage != null) {
						if (reaction.Emote.ToString() == "✅") {
							await message.DeleteAsync();
							Utils.nsfwProcessing(nsfwMessage);
							Utils.removeSnfwMessage(message);
						}
						else if (reaction.Emote.ToString() == "❎") {
							await message.DeleteAsync();
							await nsfwMessage.RemoveAllReactionsAsync();
							Utils.removeSnfwMessage(message);
						}
					}
					else if (channel.Id == Data.channels["musique"] && reaction.Emote.ToString() == "❎") {
						string result = Data.database.removeMusic(Utils.getYtLink(message.Content));

						if (result == String.Empty) {
							await message.RemoveAllReactionsAsync();
							await message.AddReactionAsync(new Emoji("💀"));
							await Utils.sendMessageTo(Data.channels["debugs"], "Message n°" + reaction.MessageId + " deleted from musique database. (" + message.Content + ")");
						}
					}
					else if (reaction.Emote.ToString() == Utils.NSFW_EMOJI) {
						Utils.nsfwProcessing(message);
					}
				}
				else {
					if (reaction.Emote.ToString() == Utils.NSFW_EMOJI) {
						int result = 0;
						//IEmote nsfw = Utils.getEmoteFromGuild(guild, Utils.NSFW_EMOJI);
						IEmote nsfw = new Emoji(Utils.NSFW_EMOJI);
						var reactedUsers = await message.GetReactionUsersAsync(nsfw, 100).FlattenAsync();

						IUser user = null;
						using (IEnumerator<IUser> enumerator = reactedUsers.GetEnumerator()) {
							while (enumerator.MoveNext()) {
								if (result == 0) {
									user = enumerator.Current;
								}
								result++;
							}
						}

						if (result == 1) {
							await message.AddReactionAsync(nsfw);
							Embed embed = Utils.quote(message, reaction.User.Value);
							IUserMessage messageSent = null;

							if (embed != null) {
								messageSent = await message.Channel.SendMessageAsync("<@&328899154887835678> Is this NSFW ? *reported by " + user.Mention + "*", false, embed);
							}
							else {
								messageSent = await message.Channel.SendMessageAsync("<@&328899154887835678> Is this NSFW ? *reported by " + user.Mention + "*");
							}
							await messageSent.AddReactionsAsync(new Emoji[] { new Emoji("✅"), new Emoji("❎") });
							Data.nsfw_content_inprocess.Add(messageSent, message);
						}
					}
				}
			}
			catch (Exception e) {
				Utils.displayException(e, "ReactionAdded");
			}
		}

		private async Task MessageReceivedBanListener(SocketMessage message)
		{
			if (Data.baned_people.Contains(message.Author.Id) || message.Author.IsBot || message.Author.Id == Data.master_id) {
				return;
			}

			ulong authorId = message.Author.Id;
			if (!Data.people_spam.ContainsKey(authorId)) {
				Data.people_spam.Add(authorId, 0);
			}
			else {
				Data.people_spam[authorId]++;
			}

			foreach (KeyValuePair<ulong, int> kvp in Data.people_spam) {
				if (kvp.Value > 4) {
					SocketUser user = Data._client.GetUser(kvp.Key);
					await message.Channel.SendMessageAsync("You, [" + user.Mention + "], are a spammer ! You got banned from the bot-services for undetermined time !");
					Data.baned_people.Add(kvp.Key);
				}
			}
		}


		private async Task MessageReceived(SocketMessage message)
		{
			string message_lower = message.Content.ToLower();

			if (message.Author.Id == Data.master_id) {
				if (message_lower == "/q") {
					Data.delay_controller.Cancel();
					return;
				}
			}

			///////////////////////////////////////////////////////////////////
			//							  Channels actions
			///////////////////////////////////////////////////////////////////

			if (message.Channel.Id == Data.channels["musique"]) {
				string msg = String.Empty;

				if ((msg = Utils.getYtLink(message.Content)) != String.Empty) {
					try {
						Data.database.addMusic(msg);
						await ((SocketUserMessage) message).AddReactionAsync(new Emoji("✅"));
					}
					catch (Exception) {
						var emote = await Data.guild.GetEmoteAsync(452977127722188811);
						await ((SocketUserMessage) message).AddReactionAsync(emote);
					}
				}
			}

			if (message.Author.Id == 123591822579597315 && false) //disabled for now
			{
				string alertTitle = String.Empty;
				try {
					foreach (var embed in message.Embeds) {
						alertTitle = embed.Title;
					}
				}
				catch (Exception e) {
					Utils.displayException(e, "foreach (var embed in message.Embeds)");
				}

				if (alertTitle.Contains("Nitain")) {
					await message.Channel.SendMessageAsync("<@&482688599201021982>");
				}
			}

			///////////////////////////////////////////////////////////////////
			//							  Limited users
			///////////////////////////////////////////////////////////////////
			if (Data.baned_people.Contains(message.Author.Id) || message.Author.IsBot) {
				return;
			}


			///////////////////////////////////////////////////////////////////
			//							  Execute command
			///////////////////////////////////////////////////////////////////

			try {
				if (message_lower.StartsWith("!!") && !Utils.isAdmin(message.Author.Id)) {
					if (Data.actions.actionExist(message_lower))
						await message.Channel.SendMessageAsync("Wesh t'es pas admin kestu fais le fou avec moi ?");
					else
						await message.Channel.SendMessageAsync("L'action demandée (" + message_lower + ") n'existe pas.");
					goto End;
				}

				foreach (var action in Data.actions.getActions) {
					if (message_lower.StartsWith(action.Item1)) {
						string msg = action.Item3.Invoke(message);
						Utils.actionUsed(action.Item1);

						if (message_lower.StartsWith("$")) { Utils.DeleteMessage(message); }

						if (msg.Contains(Utils.splitChar.ToString())) {
							foreach (string ms in msg.Split(Utils.splitChar)) {
								await message.Channel.SendMessageAsync(ms);
							}
						}
						else if (msg != String.Empty) {
							var messageSent = await message.Channel.SendMessageAsync(msg);
						}

						break;
					}
					else if (Data.autres.Contains(action.Item1.Split(Utils.otherSplitChar)[0]))
					{
						if (message_lower.Contains(action.Item1.Split(Utils.otherSplitChar)[0]))
						{
							string msg = action.Item3.Invoke(message);
							Utils.actionUsed(action.Item1.Split(Utils.otherSplitChar)[0]);

							if (msg.Contains(Utils.splitChar.ToString()))
							{
								foreach (string ms in msg.Split(Utils.splitChar))
								{
									await message.Channel.SendMessageAsync(ms);
								}
							}
							else if (msg != String.Empty) {
								await message.Channel.SendMessageAsync(msg);
							}
						}
						else if (action.Item1.Split(Utils.otherSplitChar).Length != 1)
						{
							Regex regex = new Regex(action.Item1.Split(Utils.otherSplitChar)[1]);
							if (regex.Match(message_lower).Success)
							{
								string msg = action.Item3.Invoke(message);
								Utils.actionUsed(action.Item1.Split(Utils.otherSplitChar)[0]);

								if (msg.Contains(Utils.splitChar.ToString()))
								{
									foreach (string ms in msg.Split(Utils.splitChar))
									{
										await message.Channel.SendMessageAsync(ms);
									}
								}
								else if (msg != String.Empty) {
									await message.Channel.SendMessageAsync(msg);
								}
							}
						}
					}
				}
			}
			catch (Exception e) {
				Utils.displayException(e, "Main foreach actions");
				await message.Channel.SendMessageAsync("La commande n'as pas fonctionnée comme prévu.");
			}

			///////////////////////////////////////////////////////////////////
			//							  Embeds
			///////////////////////////////////////////////////////////////////
			try {
				if (message_lower.StartsWith("!pokemon")) {
					var words = message_lower.Split(' ');

					if (words.Length == 2) {
						var embed = Utils.getAllPokemonInfo(words[1]);

						if (embed != null) {
							await message.Channel.SendMessageAsync("", false, embed);
						}
						else {
							await message.Channel.SendMessageAsync("Le pokemon '" + words[1] + "' n'existe pas.");
						}
					}
				}
				else if (message_lower.StartsWith("!quote")) {
					var args = message_lower.Split(' ');

					if (args.Length == 2) {
						try {
							ulong messageId = UInt64.Parse(args[1]);

							if (message.Channel is SocketGuildChannel) {
								IMessage msg = await Utils.getMessageFromId(messageId, ((SocketGuildChannel)message.Channel).Guild);
								if (msg == null) {
									throw new ArgumentException("Le message '"+messageId+"' n'existe pas (ou n'est plus dans le cache du bot).");
								}
								await message.Channel.SendMessageAsync("", false, Utils.quote(msg, message.Author));
							}
						}
						catch (ArgumentException e) {
							await message.Channel.SendMessageAsync(e.Message);
						}
						catch (Exception) {
							await message.Channel.SendMessageAsync("This command can be use like this : !quote message_id (je parle du vrai ID, écrivez pas message_id bande de fdp).");
						}
					}
				}
			}
			catch (Exception e) {
				Utils.displayException(e, "Main embed actions");
				await message.Channel.SendMessageAsync("La commande n'as pas fonctionnée comme prévu.");
			}

			///////////////////////////////////////////////////////////////////
			//							  Automatique
			///////////////////////////////////////////////////////////////////

			if ((message_lower.Contains("bald") && message_lower.Contains("signal")) || message_lower.Contains("baldsignal")) {
				string msg = "<@" + Data.master_id + ">";
				await message.Channel.SendMessageAsync(msg);
			}

			///////////////////////////////////////////////////////////////////
			//							  Debug Zone
			///////////////////////////////////////////////////////////////////

			if (message_lower.StartsWith("!d")) {
				try {

				}
				catch (Exception e) {
					Utils.displayException(e, "!d");
					foreach (var errors in Utils.splitBodies(e.Message + "\n" + e.StackTrace).Split(Utils.splitChar)) {
						await message.Channel.SendMessageAsync(errors);
					}
				}

				return;
			}

		End:
			string logprint = message.Author.Username + " (" + DateTime.Now.ToString() + ") : " + message_lower;
			Console.WriteLine(logprint);
			if (message.Channel.Id != Data.channels["debug"] && message.Channel.Id != Data.channels["debugs"])
				System.IO.File.AppendAllText(Data.Text.LOGS_FILE, logprint + "\n");
		}
	}
}
