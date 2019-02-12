#define UTILS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ServiceModel.Syndication;
using System.Xml;

using Discord;
using Discord.WebSocket;
using Supremes;
using System.Threading;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace DiscordBot
{
	public class Utils
	{
		public const int timeout = 15000;
        public const char splitChar = '|';
        public const char otherSplitChar = '/';


		public static void init()
		{
			if (isLinux) {
				Data.Python.PYTHON_EXE = @"/usr/bin/python3";
			}
			if (isTestBot) {
				Data.channels["debugs"] = Data.channels["tests"];
			}
		}

		public static bool isLinux {
			get {
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}

		public static bool isTestBot {
			get {
				return File.ReadAllText(Data.Text.ISBOT_FILE) == "1";
			}
		}

		public static void displayException(Exception e, string message = "Error")
		{
			Console.WriteLine(message + " : (" + e.GetType().Name + ") \n" + e.Message + "\n");
			Console.WriteLine(e.StackTrace + "\n");
		}


		/*
		 * runPython("filename").aff();
		 * runPython("filename", "a", "b").aff();
		 * runPython("filename", new string[] { "c", "d" }).aff();
		*/
		public static string runPython(string fileName, params string[] args)
		{
			string result = String.Empty;
			string arguments = String.Empty;

			if (args.Length != 0) {
				arguments += " ";
				for (int i = 0; i < args.Length; i++) //un Select serait mieux, mais flemme
				{
					arguments += args[i];
					if (i != args.Length - 1)
						arguments += " ";
				}
			}

			// Create new process start info 
			ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(Data.Python.PYTHON_EXE);

			// make sure we can read the output from stdout 
			myProcessStartInfo.UseShellExecute = false;
			myProcessStartInfo.RedirectStandardOutput = true;
			myProcessStartInfo.Arguments = fileName + arguments;

			Process myProcess = new Process();
			// assign start information to the process 
			myProcess.StartInfo = myProcessStartInfo;

			// start the process 
			myProcess.Start();

			// Read the standard output of the app we called.  
			// in order to avoid deadlock we will read output first 
			// and then wait for process terminate: 
			StreamReader myStreamReader = myProcess.StandardOutput;
			result = myStreamReader.ReadLine();

			//if you need to read multiple lines, you might use: 
			//string myString = myStreamReader.ReadToEnd()

			// wait exit signal from the app we called and then close it. 
			myProcess.WaitForExit();
			myProcess.Close();

			return result;
		}

		public static string onlyKeepDigits(string str)
		{
			return new string((from c in str
							   where char.IsDigit(c)
							   select c
			).ToArray());
		}

		public static string onlyKeepLetters(string str)
		{
			return new string((from c in str
							   where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)
							   select c
			).ToArray());
		}

		public static string onlyKeepLetters(string str, List<char> chars)
		{
			return new string((from c in str
							   where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || chars.Contains(c)
							   select c
			).ToArray());
		}

		public static string onlyKeep(string str, List<char> chars)
		{
			return new string((from c in str
							   where chars.Contains(c)
							   select c
			).ToArray());
		}

		public static string removeChars(string str, List<char> chars)
		{
			return new string((from c in str
							   where !chars.Contains(c)
							   select c
			).ToArray());
		}

		public static string flatSplit(string str, int maxLength = 1995)
		{
			"flatSplit digoulasse".debug();
			string result = String.Empty;

			int start = 0;
			while (str.Length >= maxLength) {
				int end = maxLength;
				result += str.Substring(start, end) + splitChar;
				str = str.Remove(0, end);
			}
			result += str;

			return result;
		}

		public static string splitBodies(string content, string delim = "\n", int maxLength = 1995)
		{
			if (content.Length < maxLength) { return content; }
			if (!content.Contains(delim)) { return flatSplit(content, maxLength); }

			int start = 0;
			string buffer = content;
			string result = String.Empty;

			while (buffer.Length >= maxLength) {
				string tmp = content.Substring(start, maxLength);
				int lastDelim = tmp.LastIndexOf(delim);
				if (lastDelim == -1) { return flatSplit(content, maxLength); }

				result += content.Substring(start, lastDelim) + splitChar;
				lastDelim += delim.Length;
				buffer = buffer.Remove(0, lastDelim);
				start += lastDelim;
			}
			result += buffer;

			return result;
		}

		public static IEnumerable<string> Split(string str, int chunkSize)
		{
			return Enumerable.Range(0, str.Length / chunkSize)
				.Select(i => str.Substring(i * chunkSize, chunkSize));
		}

		public static SocketChannel getChannel(ulong channel)
		{
			return Data._client.GetChannel(channel);
		}

		public static IEnumerable<IMessage> getMessages(SocketTextChannel channel)
		{
			return ((SocketTextChannel) channel).GetMessagesAsync(0, Direction.After).FlattenAsync().Result;
		}

		public static IEnumerable<IMessage> getMessages(SocketChannel channel, int number)
		{
			return ((SocketTextChannel) channel).GetMessagesAsync(number).FlattenAsync().Result;
		}

		public static string getYtLink(string message)
		{
			string result = String.Empty;
			string pattern = @"(https?:\/\/)?(www\.|m\.)?youtu(be.com\/watch\/?\?v=|\.be\/)([-_A-Za-z0-9]+)";

			Regex regex = new Regex(pattern);
			var match = regex.Match(message);

			if (match.Success) {
				result = match.Value;
			}

			return result;
		}

		//-----

		public static string FormateSentence(string sentence)
		{
			string res = String.Empty;
			string indicator = ":regional_indicator_";

			for (int i = 0; i < sentence.Length; i++) {
				if (sentence[i] == ' ')
					res += "     ";
				else if ('a' <= sentence[i] && 'z' >= sentence[i])
					res += indicator + sentence[i] + ": ";
				else
					res += sentence[i] + " ";
			}

			return res;
		}

		public static int CountIterations(string sentence, string word)
		{
			Regex regex = new Regex(word);
			var matches = regex.Matches(sentence);
			return matches.Count;
		}

		public static bool SentenceContainsWord(string sentence, string word)
		{
			string[] split = sentence.Split(' ');
			for (int i = 0; i < split.Length; i++) {
				if (split[i] == word)
					return true;
			}
			return false;
		}

		public static string displayMangasList()
		{
			int number = 1;
			string result = "Voici les mangas qui sont check avec le web parser :\n";

			foreach (KeyValuePair<string, string> kvp in Data.mangasData) {
				result += "- " + kvp.Key + " (**" + number + "**)\n";
				number++;
			}

			return splitBodies(result);
		}

		public static string displayCompleteMangasList()
		{
			string result = "Voici, pour chaque manga, son dernier scan :\n\n";

			foreach (KeyValuePair<string, string> kvp in Data.mangasData) {
				var kvpValue = kvp.Value.Split(new[] { " => " }, StringSplitOptions.None);
				result += "```asciidoc\n[" + kvp.Key + "]```" + kvpValue[0] + "\n" + kvpValue[1] + "\n\n";
			}

			return splitBodies(result, "\n\n");
		}

		//lirescan V2
		public static async void mangasCrawlerOnLireScanV2()
		{
			string site = "https://www.lirescan.me/";
			string url = site + "rss/";
			var time = DateTime.Now;
			("mangasCrawlerOnLireScanV2 (" + time + ")").println();

			try {
				XmlReader reader = XmlReader.Create(url);
				SyndicationFeed feed = SyndicationFeed.Load(reader);
				reader.Close();

				int crawler_counter = 0;
				string data = String.Empty;
				List<string> processedMangas = new List<string>();
				string text_data = System.IO.File.ReadAllText(Data.Text.MANGASDATA_RSS_FILE);

				foreach (SyndicationItem item in feed.Items) {
					String title = item.Title.Text;
					String link = item.Links[0].Uri.ToString();
					String description = item.Summary.Text;
					string mangaName = mangaNameToLowerCase(title);
					bool mangaExists = Data.mangasData.ContainsKey(mangaName);
					link = link.Replace("http://www.lirescan.com/", "");
					link = site + mangaName + link;

					string chapter = title + splitChar + link + splitChar + description;

					if (mangaExists && !processedMangas.Contains(mangaName)) {
                        Supremes.Nodes.Document document = null;
                        try {
                            document = Dcsoup.Parse(new Uri(link), timeout);
                        }
                        catch (Exception) {
                            ("Timeout on : <" + link + ">").debug();
                            throw new TimeoutException("Timeout on : <" + link + ">");
                        }

                        var pNotif = document.Select("p[id=notif]");
                        bool isVF = (pNotif.Text == String.Empty);
                        bool alreadyInDataList = false;
						int tmp_counter = 0, data_counter = 2000;

						foreach (string dataLine in text_data.Split('\n')) {
							if (chapter.Equals(dataLine)) {
								data_counter = tmp_counter;
								alreadyInDataList = true;
							}
							tmp_counter++;
						}

						bool newChapter = false;
						if (alreadyInDataList) {
							if (crawler_counter < data_counter) {
								("rentre (" + crawler_counter.ToString() + " < " + data_counter.ToString() + " )").debug();
								newChapter = true;
							}
						}
						else {
							"rentre (notInList)".debug();
							newChapter = true;
						}

						if (newChapter) {
							string scanValue = title + " => <" + link + ">";
							string subs = String.Empty;
							var users = Data.database.getSubs(mangaName);
							string msg = "Nouveau scan trouvé pour " + mangaName + " : \n\t" + scanValue;

							foreach (var user in users) {
								subs += "<@" + user + "> ";
							}
							if (isVF) {
								await sendMessageTo(Data.channels["mangas"], msg + " " + subs);
							}
						}

						processedMangas.Add(mangaName);
						data += chapter + "\n";
					}

					crawler_counter++;
				}

				System.IO.File.WriteAllText(Data.Text.MANGASDATA_RSS_FILE, data);
			}
			catch (Exception e) {
				await sendMessageTo(Data.channels["debug"], "Le crawl des mangas a échoué, car la connexion au site a échouée.\n" + e);
				displayException(e, "Exception on mangasCrawlerOnLireScan()");
			}

			var now = DateTime.Now - time;
			("search done. (" + DateTime.Now + ") [" + now + "]").println();
			//Thread.Sleep(10800000);     //3h
			Thread.Sleep(1800000);  //30min
			mangasCrawlerOnLireScanV2();
		}

		private static string mangaNameToLowerCase(string mangaName)
		{
			//One Piece
			//one-piece
			return mangaName.Replace(" ", "-").Replace(".", "").Replace("Scan-", "").Replace("VF", "").Replace(onlyKeepDigits(mangaName), "").ToLower().Replace("--", "");
		}

		private static string mangaNameToUpperCase(string mangaName)
		{
			//one-piece
			//One Piece
			string result = String.Empty;
			var tokens = mangaName.Split('-');
			foreach (var token in tokens) {
				var new_token = char.ToUpper(token[0]) + token.Substring(1);
				result += new_token + " ";
			}

			return result.Substring(0, result.Length - 1);
		}

		public static string addManga(string message_lower)
		{
			return "Need rework."; //faire un crawl pour voir si le manga existe, je l'ai delete, mais je l'avais déjà fait pour check les mangas existant sur le site
		}
		/*
		public static string old_addManga(string message_lower)
		{
			string msg = String.Empty;
			try {
				string manga = message_lower.Split(' ')[1];
				if (Data.mangasData.ContainsKey(manga)) {
					msg = "Ce manga est déjà dans la liste poto ! :)";
					return msg;
				}
				string chapter = getLastChapterOf(manga);

				string result = "\n" + manga + "|" + chapter;
				System.IO.File.AppendAllText(Data.Text.MANGASDATA_FILE_NAME, result);
				setupMangasData();

				Data.database.addManga(manga);
				msg = "Manga '" + manga + "' ajouté à la liste.";
			}
			catch (ArgumentOutOfRangeException) {
				msg = "Ce manga n'existe pas. Le nom doit être de la forme : one-piece (minuscules séparées par un tiret)";
			}
			catch (Exception) {
				msg = "La commande est mal écrite. Elle doit être de la forme : !addmanga one-piece";
			}
			return msg;
		}*/

		public static string displayAllActions()
		{
			string type = String.Empty;
			string typeSave = String.Empty;
			string result = "Voici toutes les options du bot par catégorie :\n";

			foreach (var action in Data.actions.getActions) {
				if (action.Item2 == String.Empty) { continue; }
				type = Actions.getActionType(action.Item1);

				if (type != typeSave) {
					typeSave = type;
					result += "\n[" + type + "]\n";
				}
                
				result += "- " + action.Item1.Split(otherSplitChar)[0] + " : " + action.Item2 + "\n";
			}

			return splitBodies(result);
		}

        public static async Task sendMessageTo(ulong channel, string message)
        {
            try {
                foreach (var msg in splitBodies(message).Split(splitChar)) {
                    await ((SocketTextChannel)Data._client.GetChannel(channel)).SendMessageAsync(msg);
                }
            }
            catch (Exception e) {
                displayException(e, "Impossible to send message, sendMessageTo(ulong channel, string message)");
            }
        }

        public static async Task sendFileTo(ulong channel, string filePath, string message)
        {
            try {
                await ((SocketTextChannel)Data._client.GetChannel(channel)).SendFileAsync(filePath, message);
            }
            catch (Exception e) {
                displayException(e, "Impossible to send message, sendFileTo(ulong channel, string filePath, string message)");
            }
        }

        public static async Task reply(SocketMessage message, string response)
		{
			try {
				foreach (var msg in splitBodies(response).Split(splitChar)) {
					await message.Channel.SendMessageAsync(msg);
				}
			}
			catch (Exception e) {
				displayException(e, "Impossible to send message, reply(SocketMessage message, string msg)");
				await message.Channel.SendMessageAsync("Le message est trop long <3");
			}
		}

		public static void fillTrafficData()
		{
			var time = DateTime.Now;
			Console.WriteLine("fillTrafficData (" + time + ")");

			string duration = traffic();
			string day = time.DayOfWeek.ToString();
			string hour = time.Hour.ToString() + "h" + time.Minute.ToString();
			string result = day + ":" + hour + "=" + duration + "\n";

			if (Int32.Parse(time.Hour.ToString()) > 8 && Int32.Parse(time.Hour.ToString()) < 21)
				System.IO.File.AppendAllText(Data.Text.TRAJETS_FILE, result);

			var now = DateTime.Now - time;
			Console.WriteLine("fill traffic data done. (" + DateTime.Now + ") [" + now + "]");

			Thread.Sleep(1800000);      //30min
			fillTrafficData();
		}

		public static string randomPpSong()
		{
			Random r = new Random();
			int rInt = r.Next(0, Data.pp_songs.Count - 1);
			var result = Data.pp_songs[rInt];

			return result;
		}

		public static string randomSong()
		{
			string result = String.Empty;
			try {
				string maxId = Data.database.getMaxId("musics");
				Random r = new Random();
				do {
					int rInt = r.Next(1, Convert.ToInt32(maxId));
					result = Data.database.getLineColumn("musics", "title", rInt.ToString());

				} while (result == String.Empty);

			}
			catch (Exception e) {
				displayException(e, "randomSong");
			}

			result = removeChars(result, new List<char>() { '(', '\'', ')', ',' });

			return result.Replace("///", "://");
		}

		public static string traffic()
		{
			string result = String.Empty;
			string url = "https://maps.googleapis.com/maps/api/directions/json?origin=Centre+d'Enseignement+et+de+Recherche+en+Informatique&destination=Robion&key=" + getApiKey();

			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();
			Stream data = response.GetResponseStream();
			StreamReader reader = new StreamReader(data);

			// json-formatted string from maps api
			string responseFromServer = reader.ReadToEnd();
			//Console.WriteLine(responseFromServer);
			dynamic details = JObject.Parse(responseFromServer);
			Console.WriteLine(details.routes[0].legs[0].duration.text);
			var duration = details.routes[0].legs[0].duration.text;
			result = (string) duration;

			response.Close();

			return result;
		}

		public static string imitateMangasData()
		{
			string result = String.Empty;

			foreach (KeyValuePair<string, string> kvp in Data.mangasData) {
				result += kvp.Key + "|" + kvp.Value + "\n";
			}

			return result.Substring(0, result.Length - 1);
		}

		public static string GetPing(DateTime old)
		{
			return ((long) ((TimeSpan) (DateTime.Now - old)).TotalMilliseconds).ToString();
		}

		public static async void DeleteMessage(SocketMessage message)
		{
			try {
				await message.DeleteAsync();
			}
			catch (Exception e) {
				displayException(e, "DeleteException on DeleteMessage(SocketMessage message)");
			}
		}

		public static bool isAdmin(ulong id)
		{
			return Data.database.isAdmin(id.ToString());
		}

		public static void alert(ulong channel, string message = "")
		{
			ThreadStart threadMethod = alertCataclysm;

			if (message != String.Empty) {
				threadMethod = () => simpleAlert(channel, message);
			}
			Thread thread = new Thread(threadMethod);
			thread.Start();
		}

		public static void savePokemons()
		{
			string allText = System.IO.File.ReadAllText(Data.Text.POKEMONS_FILE);
			string[] pokemons = allText.Split("<pokemon>\r\n");

			foreach (string pokemon in pokemons) {
				string[] infos = pokemon.Split("\r\n");

				int id = Int32.Parse(infos[0]);
				string urlIcon = infos[1];
				string name = infos[2].ToLower();
				int catchRate = Int32.Parse(infos[3]);
				int rarityTier = Int32.Parse(infos[4]);

				Data.database.addPokemon(id, urlIcon, name, catchRate, rarityTier);
			}
		}

		public static string getPokemonCatchRate(String pokemonName)
		{
			return Data.database.getPokemonInfo(pokemonName, Data.PokemonInfo.name, Data.PokemonInfo.catchRate);
		}

		public static string getPokemonRarityTier(String pokemonName)
		{
			return Data.database.getPokemonInfo(pokemonName, Data.PokemonInfo.name, Data.PokemonInfo.rarityTier);
		}

		public static string getPokemonCatchChances(float hp_percent, float catch_rate, float bonus_ball, float bonus_statut)
		{
			double formula = (1 - (2 / 3.0) * (hp_percent / 100.0)) * catch_rate * bonus_ball * bonus_statut;

			return formula.ToString("0.00");
		}

		public static Embed getAllPokemonInfo(String pokemonName)
		{
			string pokemonInfos = Data.database.getPokemonInfos(pokemonName);
			if (pokemonInfos == String.Empty) {
				return null;
			}

			pokemonInfos = pokemonInfos.Replace("(", "")
			    .Replace(")", "")
			    .Replace("'", "")
			    .Replace("///", "://");
			string[] infos = pokemonInfos.Split(", ");

			int id = Int32.Parse(infos[1]);
			string urlIcon = infos[2];
            //https://img.pokemondb.net/sprites/black-white/anim/normal/unown-a.gif
            //http://www.pokestadium.com/sprites/xy/unown.gif
            //http://www.pokestadium.com/assets/img/sprites/official-art/unown.png
            urlIcon = "http://www.pokestadium.com/sprites/xy/" + infos[3] + ".gif";
            string nameEn = infos[3]; nameEn = char.ToUpper(nameEn[0]) + nameEn.Substring(1);
			string nameFr = infos[4]; nameFr = char.ToUpper(nameFr[0]) + nameFr.Substring(1);
			int catchRate = Int32.Parse(infos[5]);
			int rarityTier = Int32.Parse(infos[6]);

            string pokepediaUrl = "https://www.pokepedia.fr/" + nameFr;
            string prowikiUrl = "https://prowiki.info/index.php?title=" + nameEn;

            return new EmbedBuilder()
                .WithTitle("-- " + nameEn + " / " + nameFr + " --")
                .WithDescription("[Pokepedia](" + pokepediaUrl + ") - [ProWiki](" + prowikiUrl + ")")
                .WithColor(new Color(89, 181, 255))
                .WithThumbnailUrl(urlIcon)
                .AddField("ID", id, true)
                .AddField("Catch rate", catchRate, true)
                .AddField("Rarity tier", rarityTier, true)
                .Build();
		}

		public static void emptyBannedPeopleStack()
		{
			Data.people_spam = new Dictionary<ulong, int>();
			Thread.Sleep(15000);
			emptyBannedPeopleStack();
		}

		public static string banUser(string userId)
		{
			ulong userId_ = Convert.ToUInt64(onlyKeepDigits(userId));
			string msg = "Cet utilisateur était déjà banni.";

			if (userId_ == Data.master_id) {
				return "Bien essayé, mais non. 😏";
			}

			if (!Data.baned_people.Contains(userId_)) {
				Data.baned_people.Add(userId_);
				msg = "L'utilisateur a bien été banni.";
			}

            SaveStateManager.Save(Data.Binary.BANNED_FILE, Data.baned_people);

			return msg;
		}

		public static string unbanUser(string userId)
		{
			ulong userId_ = Convert.ToUInt64(onlyKeepDigits(userId));
			string msg = "Cet utilisateur n'était pas banni.";

			if (Data.baned_people.Contains(userId_)) {
				Data.baned_people.Remove(userId_);
				msg = "L'utilisateur a bien été retiré des utilisateurs bannis.";
            }

            SaveStateManager.Save(Data.Binary.BANNED_FILE, Data.baned_people);

            return msg;
		}

        public static string getBannedUsersList()
        {
            if (Data.baned_people.Count == 0) {
                return "La liste est vide.";
            }

            string result = "Voici la liste des gens bannis : \n";
            foreach (var bannedUser in Data.baned_people) {
                result += "\t - " + Data._client.GetUser(bannedUser).ToString() + " (" + bannedUser + ")";
            }

            return result;
        }

		public static void actionUsed(string action)
		{
			var actions_used = Data.actions_used;

			if (actions_used.ContainsKey(action)) {
				actions_used[action]++;
			}
			else {
				actions_used.Add(action, 1);
			}
			
			SaveStateManager.Save(Data.Binary.POP_ACTIONS_FILE, Data.actions_used);
		}

		public static string getPopActions()
		{
			string result = "Liste des actions les plus populaires :\n";
			var sortedActionsUsed = from entry in Data.actions_used orderby entry.Value descending select entry;

			foreach (KeyValuePair<string, int> kvp in sortedActionsUsed) {
				result += "- `" + kvp.Key + "` : " + kvp.Value.ToString() + "\n";
			}
			return result;
		}

        public static Embed getNsfwEmbed(IUser author)
        {
            return new EmbedBuilder()
                .WithTitle("NSFW POLICE FORCE")
                .WithDescription("Report to this field : <#389537278671978497>")
                .WithColor(new Color(200, 25, 25))
                .WithThumbnailUrl("https://cdn.discordapp.com/emojis/539905759580782602.png?v=1")
                .AddField("\u200B", "You "+author.Mention+" sir, are DISGUSTING !", false)
                .Build();
        }

        public static IEmote getEmoteFromGuild(SocketGuild guild, string emoteName)
        {
            IEmote result = null;

            foreach (var emote in guild.Emotes) {
                if (emote.ToString() == emoteName) {
                    result = emote;
                }
            }

            return result;
        }

        public static async Task<IMessage> getMessageFromId(ulong messageId, SocketGuild guild)
        {
            IMessage resultMessage = null;

            foreach (var channel in guild.Channels) {
                if (!(channel is SocketTextChannel)) {
                    continue;
                }
				
				var message = await ((SocketTextChannel)channel).GetMessageAsync(messageId);
				if (message != null) {
					return message;
				}
			}

            return resultMessage;
        }

        public static Embed quote(IMessage message, IUser user = null)
        {
            var channel = message.Channel;
            if (!(channel is SocketGuildChannel)) {
                return null;
            }

            int maximum = -1;
            Color color = new Color(75, 75, 75);
            var guild = ((SocketGuildChannel)channel).Guild;

            foreach (var role in ((SocketGuildUser)message.Author).Roles) {
                if (maximum < role.Position) {
                    maximum = role.Position;
                    color = role.Color;
                }
            }

            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithDescription(message.Content)
                .WithColor(color)
                .WithAuthor(message.Author.ToString(), message.Author.GetAvatarUrl(), "https://discordapp.com/channels/" + guild.Id + '/' + channel.Id + '/' + message.Id)
                .WithTimestamp(message.CreatedAt);

            if (user != null) {
                embedBuilder.WithFooter("Quoted by: " + user.Username);
            }

            if (message.Attachments.Count != 0) {
                foreach (IAttachment attachment in message.Attachments) {
                    embedBuilder.WithImageUrl(attachment.Url);
                }
            }

            return embedBuilder.Build();
        }

        public static IUserMessage isThisNsfwInProgress(IUserMessage message)
        {
            foreach (var msg in Data.nsfw_content_inprocess) {
                if (msg.Key.Id == message.Id) {
                    return msg.Value;
                }
            }
            return null;
        }

        public static void removeSnfwMessage(IUserMessage message)
        {
            foreach (var msg in Data.nsfw_content_inprocess) {
                if (msg.Key.Id == message.Id) {
                    Data.nsfw_content_inprocess.Remove(message);
                }
            }
        }

        public static async void nsfwProcessing(IUserMessage message)
        {
            //renvoyer dans le meme channel un embed qui met l'icone NSFW + qui dit qui a commit la faute
            var embed = Utils.getNsfwEmbed(message.Author);

            if (embed != null) {
                await message.Channel.SendMessageAsync("", false, embed);
            }
            else {
                await message.Channel.SendMessageAsync("What do fock.");
            }

            //renvoyer le message dans le channel nsfw
            ulong channelTo = Data.channels["nsfw"];

            if (message.Attachments.Count == 0) {
                await Utils.sendMessageTo(channelTo, "*--Content proposed by " + message.Author.Mention + "--*\n" + message.Content);
            }
            else {
                foreach (IAttachment attachment in message.Attachments) {
                    await Utils.sendMessageTo(channelTo, "*--Content proposed by " + message.Author.Mention + "--*\n" + message.Content + "\n" + attachment.Url);
                }
            }

            //supprimer le message originel
            try {
                await message.DeleteAsync();
            }
            catch (Exception) { }
        }


		//Privates
		private async static void simpleAlert(ulong channel, string message)
		{
			await sendMessageTo(channel, message);
			Thread.Sleep(60000);
			simpleAlert(channel, message);
		}

		private async static void alertCataclysm()
		{
			await sendMessageTo(54545454545, "blblbl");
		}

		//Setups
		public static void setupMangasData()
		{
			try {
				Data.mangasData = new SortedDictionary<string, string>();
				string[] lines = System.IO.File.ReadAllLines(Data.Text.MANGASDATA_FILE);
				foreach (string line in lines) {
					var data = line.Split(splitChar);
					var manga = data[0];
					var content = data[1];

					Data.mangasData.Add(manga, content);
				}
			}
			catch (Exception e) {
				displayException(e, "Exception on setupMangasData()");
			}
		}

		public static void setupPpSong()
		{
			string[] lines = System.IO.File.ReadAllLines(Data.Text.PP_FILE);
			foreach (string line in lines) {
				Data.pp_songs.Add(line);
			}
		}

		public static void setupOtherActionsList()
		{
			foreach (var action in Data.actions.getActions) {
				if (Actions.getActionType(action.Item1) == "Autres") {
					Data.autres.Add(action.Item1.Split(otherSplitChar)[0]);
				}
			}
		}

		//Getters
		public static string getToken()
		{
			string[] lines = System.IO.File.ReadAllLines(Data.Text.TOKEN_FILE);
			return lines[0];
		}

		public static string getApiKey()
		{
			string[] lines = System.IO.File.ReadAllLines(Data.Text.TOKEN_FILE);
			return lines[1];
		}
	}
}
