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
		public static int timeout = 15000;
		public static char splitChar = '|';
		public static string flip = "(╯°□°）╯︵ ┻━┻";
		public static string unflip = "┬─┬﻿ ノ( ゜-゜ノ)";

		public static string DB_FILE_SAVE = @"bdd_save.db";
		public static string DB_FILE_NAME = @"resources/bdd.db";
		public static string PP_FILE_NAME = @"resources/pp.txt";
		public static string LOGS_FILE_NAME = @"resources/logs.txt";
		public static string PYTHON_DIR_PATH = @"resources/python/";
		public static string TOKEN_FILE_NAME = @"resources/token.txt";
		public static string POP_ACTIONS_FILE = @"resources/pop_actions.txt";
		public static string TRAJETS_FILE_NAME = @"resources/trajets.txt";
		public static string POKEMONS_FILE_NAME = @"resources/pokemons.p";
		public static string ERRORSLOG_FILE_NAME = @"resources/errors.txt";
		public static string MANGASDATA_FILE_NAME = @"resources/data.txt";
		public static string MANGASDATA_FILE_NAME2 = @"resources/data2.txt";
		public static string MANGASDATA_FILE_NAME3 = @"resources/feed_data.txt";

		public static string PYTHON_EXE = @"C:\Users\utilisateur\AppData\Local\Programs\Python\Python37\python.exe";


		public static void init()
		{
			if (isLinux) {
				PYTHON_EXE = @"/usr/bin/python3";
			}
		}

		public static bool isLinux {
			get {
				int p = (int) Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}

		public static void displayException(Exception e, string message = "Error")
		{
			Console.WriteLine(message + " : \n" + e.Message + "\n");
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
			string file = PYTHON_DIR_PATH + fileName;

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
			ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(PYTHON_EXE);

			// make sure we can read the output from stdout 
			myProcessStartInfo.UseShellExecute = false;
			myProcessStartInfo.RedirectStandardOutput = true;
			myProcessStartInfo.Arguments = file + arguments;

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
			return Program._client.GetChannel(channel);
		}

		public static IEnumerable<IMessage> getMessages(SocketChannel channel)
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
			return matches.Count();
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

			foreach (KeyValuePair<string, string> kvp in Program.mangasData) {
				result += "- " + kvp.Key + " (**" + number + "**)\n";
				number++;
			}

			return splitBodies(result);
		}

		public static string displayCompleteMangasList()
		{
			string result = "Voici, pour chaque manga, son dernier scan :\n\n";

			foreach (KeyValuePair<string, string> kvp in Program.mangasData) {
				var kvpValue = kvp.Value.Split(new[] { " => " }, StringSplitOptions.None);
				result += "```asciidoc\n[" + kvp.Key + "]```" + kvpValue[0] + "\n" + kvpValue[1] + "\n\n";
			}

			return splitBodies(result, "\n\n");
		}

		public static string lastChapter(string message_lower)
		{
			string msg = String.Empty;
			try {
				string manga = message_lower.Split(' ')[1];
				manga.debug();
				msg = getLastChapterOf(manga);
			}
			catch (ArgumentOutOfRangeException) {
				msg = "Ce manga n'existe pas. Le nom doit être de la forme : one-piece";
			}
			catch (TimeoutException) {
				msg = "Le site doit être en maintenance (timeout du crawl).";
			}
			catch (Exception) {
				msg = "La commande est mal écrite. Elle doit être de la forme : !lastchapter one-piece";
			}
			return msg;
		}

		public static string getLastChapterOf(string manga)
		{
			return "Impossible at the time being.";
		}

		//japscan V1
		public static string old_getLastChapterOf(string manga)
		{
			string site = "https://www.japscan.cc";
			string url = site + "/mangas/" + manga;
			Supremes.Nodes.Document document = null;
			try {
				document = Dcsoup.Parse(new Uri(url), timeout);
			}
			catch (Exception) {
				throw new TimeoutException("Timeout on : <" + url + ">");
			}
			var divChaptersList = document.Select("div[id=liste_chapitres]");

			var firstChapterCatch = divChaptersList.Select("ul").Select("li").Select("a")[0];
			var firstChapterName = firstChapterCatch.Text;
			var link = site + firstChapterCatch.ToString().Split('\"')[1];
			var version = "(VF)";
			try {
				if (manga == "my-hero-academia") { //TODO mettre un meilleur scotch que ça
					var words = firstChapterName.Split(" ");
					var chapterNumber = Int32.Parse(words[4]);
					if (chapterNumber > 200) {
						goto Skip;
					}
				}
				version = divChaptersList.Select("ul").Select("li").Select("span")[0].Text;
			}
			catch (ArgumentOutOfRangeException) { /*displayException(e, "ArgumentOutOfRangeException, getLastChapterOf(string manga)");*/ }

			if (version == "(RAW)")
				version = "(JAP)";
			Skip:
			return firstChapterName + " **" + version + "** => <" + link + ">";
		}

		//japscan V2
		public static string new_getLastChapterOf(string manga)
		{
			string site = "https://www.japscan.to";
			string url = site + "/manga/" + manga;
			("url : " + url).debug();

			Supremes.Nodes.Document document = null;
			try {
				document = Dcsoup.Parse(new Uri(url), timeout);
			}
			catch (Exception) {
				("Timeout on : <" + url + ">").debug();
				throw new TimeoutException("Timeout on : <" + url + ">");
			}
			("document: " + document).debug();

			System.IO.File.WriteAllText(ERRORSLOG_FILE_NAME, document.ToString());

			var divChaptersList = document.Select("div[id=chapters_list]");
			("divChaptersList: " + divChaptersList).debug();

			var firstChapterCatch = divChaptersList.Select("div").Select("div").Select("a")[0];
			var firstChapterName = firstChapterCatch.Text;
			var link = site + firstChapterCatch.ToString().Split('\"')[1];
			("firstChapterName : " + firstChapterName).debug();
			("link : " + link).debug();
			var version = "(VF)";
			try {
				if (manga == "my-hero-academia") { //TODO mettre un meilleur scotch que ça
					var words = firstChapterName.Split(" ");
					var chapterNumber = Int32.Parse(words[4]);
					if (chapterNumber > 200) {
						goto Skip;
					}
				}
				version = divChaptersList.Select("div").Select("div").Select("span")[0].Text;
			}
			catch (ArgumentOutOfRangeException) { /*displayException(e, "ArgumentOutOfRangeException, getLastChapterOf(string manga)");*/ }

			if (version == "(RAW)") {
				version = "(JAP)";
			}
		Skip:
			return firstChapterName + " **" + version + "** => <" + link + ">";
		}

		//lirescan V1
		public static async void mangasCrawlerOnLireScan()
		{
			string site = "https://www.lirescan.me/";
			string url = site + "rss/";
			var time = DateTime.Now;
			("mangasCrawlerOnLireScan (" + time + ")").println();

			try {
				XmlReader reader = XmlReader.Create(url);
				SyndicationFeed feed = SyndicationFeed.Load(reader);
				reader.Close();

				List<string> processedMangas = new List<string>();
				foreach (SyndicationItem item in feed.Items) {
					String title = item.Title.Text;
					String link = item.Links[0].Uri.ToString();
					String description = item.Summary.Text;
					string mangaName = mangaNameToLowerCase(title);
					bool mangaExists = Program.mangasData.ContainsKey(mangaName);

					link = link.Replace("http://www.lirescan.com/", "");
					link = site + mangaName + link;

					if (mangaExists && !processedMangas.Contains(mangaName)) {
						string scanValue = title + " => <" + link + ">";

						if (Program.mangasData[mangaName] != scanValue) {
							Program.mangasData.Remove(mangaName);
							Program.mangasData.Add(mangaName, scanValue);

							string subs = String.Empty;
							var users = Program.database.getSubs(mangaName);
							string msg = "Nouveau scan trouvé pour " + mangaName + " : \n\t" + scanValue;

							foreach (var user in users) {
								subs += "<@" + user + "> ";
							}
							await sendMessageTo(Program.channels["mangas"], msg + " " + subs);
						}
						processedMangas.Add(mangaName);
					}
				}

				System.IO.File.WriteAllText(MANGASDATA_FILE_NAME2, imitateMangasData());
			}
			catch (Exception e) {
				await sendMessageTo(Program.channels["debug"], "Le crawl des mangas a échoué, car la connexion au site a échouée.\n" + e);
				displayException(e, "Exception on mangasCrawlerOnLireScan()");
			}

			var now = DateTime.Now - time;
			("search done. (" + DateTime.Now + ") [" + now + "]").println();
			//Thread.Sleep(10800000);     //3h
			Thread.Sleep(1800000);  //30min
			mangasCrawlerOnLireScan();
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
				string text_data = System.IO.File.ReadAllText(MANGASDATA_FILE_NAME3);

				foreach (SyndicationItem item in feed.Items) {
					String title = item.Title.Text;
					String link = item.Links[0].Uri.ToString();
					String description = item.Summary.Text;
					string mangaName = mangaNameToLowerCase(title);
					bool mangaExists = Program.mangasData.ContainsKey(mangaName);
					link = link.Replace("http://www.lirescan.com/", "");
					link = site + mangaName + link;

					string chapter = title + splitChar + link + splitChar + description;
					string full_d = mangaName + "/" + mangaExists + "/" + !text_data.Contains(chapter) + "/" + !processedMangas.Contains(mangaName);
					//full_d.debug();
					if (mangaExists && !processedMangas.Contains(mangaName)) {
						bool alreadyInDataList = false;
						int tmp_counter = 0, data_counter = 2000;
						foreach (string dataLine in text_data.Split("\n")) {
							//string dataLine = data_.Replace(tmp_counter.ToString()+splitChar, "");
							if (chapter.Equals(dataLine)) {
								data_counter = tmp_counter;
								alreadyInDataList = true;
							}
							tmp_counter++;
						}

						if (alreadyInDataList) {
							if (crawler_counter < data_counter) {
								("rentre (" + crawler_counter.ToString() + " < " + data_counter.ToString() + " )").debug();
								string scanValue = title + " => <" + link + ">";
								string subs = String.Empty;
								var users = Program.database.getSubs(mangaName);
								string msg = "Nouveau scan trouvé pour " + mangaName + " : \n\t" + scanValue;

								foreach (var user in users) {
									subs += "<@" + user + "> ";
								}
								await sendMessageTo(Program.channels["mangas"], msg + " " + subs);
								processedMangas.Add(mangaName);
							}
						}
						else {
							"rentre (notInList)".debug();
							string scanValue = title + " => <" + link + ">";
							string subs = String.Empty;
							var users = Program.database.getSubs(mangaName);
							string msg = "Nouveau scan trouvé pour " + mangaName + " : \n\t" + scanValue;

							foreach (var user in users) {
								subs += "<@" + user + "> ";
							}
							await sendMessageTo(Program.channels["mangas"], msg + " " + subs);
							processedMangas.Add(mangaName);
						}
					}
					else if (mangaExists) {
						//await sendMessageTo(Program.channels["debugs"], "Rentre pas : " + full_d);
					}

					data += /*crawler_counter.ToString() + splitChar +*/ chapter + "\n";
					crawler_counter++;
				}
				//await sendMessageTo(Program.channels["debugs"], "\nFIN\n");

				System.IO.File.WriteAllText(MANGASDATA_FILE_NAME3, data);
			}
			catch (Exception e) {
				await sendMessageTo(Program.channels["debug"], "Le crawl des mangas a échoué, car la connexion au site a échouée.\n" + e);
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
			var tokens = mangaName.Split("-");
			foreach (var token in tokens) {
				var new_token = char.ToUpper(token[0]) + token.Substring(1);
				result += new_token + " ";
			}

			return result.Substring(0, result.Length - 1);
		}

		public static string addManga(string message_lower)
		{
			return "";
		}

		public static string old_addManga(string message_lower)
		{
			string msg = String.Empty;
			try {
				string manga = message_lower.Split(' ')[1];
				if (Program.mangasData.ContainsKey(manga)) {
					msg = "Ce manga est déjà dans la liste poto ! :)";
					return msg;
				}
				string chapter = getLastChapterOf(manga);

				string result = "\n" + manga + "|" + chapter;
				System.IO.File.AppendAllText(MANGASDATA_FILE_NAME, result);
				setupMangasData();

				Program.database.addManga(manga);
				msg = "Manga '" + manga + "' ajouté à la liste.";
			}
			catch (ArgumentOutOfRangeException) {
				msg = "Ce manga n'existe pas. Le nom doit être de la forme : one-piece (minuscules séparées par un tiret)";
			}
			catch (Exception) {
				msg = "La commande est mal écrite. Elle doit être de la forme : !addmanga one-piece";
			}
			return msg;
		}

		public static string displayAllActions()
		{
			string type = String.Empty;
			string typeSave = String.Empty;
			string result = "Voici toutes les options du bot par catégorie :\n";

			foreach (var action in Program.actions.getActions) {
				if (action.Item2 == String.Empty) { continue; }
				type = Actions.getActionType(action.Item1);

				if (type != typeSave) {
					typeSave = type;
					result += "\n[" + type + "]\n";
				}

				result += "\t - " + action.Item1 + " : " + action.Item2 + "\n";
			}

			return splitBodies(result);
		}

		public static async void getAllNewChapters()
		{
			var time = DateTime.Now;
			Console.WriteLine("getAllNewChapters (" + time + ")");
			foreach (KeyValuePair<string, string> kvp in Program.mangasData) {
				try {
					string chapter = getLastChapterOf(kvp.Key);
					if (kvp.Value != chapter) {
						string toWrite = imitateMangasData(); //digoulasse ^9000
						int pos = toWrite.IndexOf(kvp.Key) + kvp.Key.Length + 1;
						int nextLinePos = toWrite.IndexOf('\n', pos);

						if (nextLinePos == -1)
							nextLinePos = toWrite.Length;

						toWrite = toWrite.Remove(pos, nextLinePos - pos);
						toWrite = toWrite.Insert(pos, chapter);

						System.IO.File.WriteAllText(MANGASDATA_FILE_NAME, toWrite);

						string subs = String.Empty;
						var users = Program.database.getSubs(kvp.Key);

						foreach (var user in users) {
							subs += "<@" + user + "> ";
						}

						string msg = "Nouveau scan trouvé pour " + kvp.Key + " : \n\t" + chapter;
						if (!chapter.Contains("VUS") && !chapter.Contains("JAP") && !chapter.Contains("SPOILER") && !chapter.Contains("RAW"))
							await sendMessageTo(Program.channels["mangas"], msg + " " + subs);

						setupMangasData();
					}
				}
				catch (TimeoutException) {
					await sendMessageTo(Program.channels["debug"], "Le crawl des mangas a échoué, car la connexion au site a été plus longue que le timeout de " + timeout + "ms.");
					goto Loop;
				}
				catch (Exception e) {
					displayException(e, "Exception on getAllNewChapters()");
				}
			}

		Loop:
			var now = DateTime.Now - time;
			Console.WriteLine("search done. (" + DateTime.Now + ") [" + now + "]");
			//Thread.Sleep(10800000);     //3h
			Thread.Sleep(1800000);  //30min
			getAllNewChapters();
		}

		public static async Task sendMessageTo(ulong channel, string message)
		{
			try {
				foreach (var msg in splitBodies(message).Split(splitChar)) {
					await ((SocketTextChannel) Program._client.GetChannel(channel)).SendMessageAsync(msg);
				}
			}
			catch (Exception e) {
				displayException(e, "Impossible to send message, sendMessageTo(ulong channel, string message)");
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
				System.IO.File.AppendAllText(TRAJETS_FILE_NAME, result);

			var now = DateTime.Now - time;
			Console.WriteLine("fill traffic data done. (" + DateTime.Now + ") [" + now + "]");

			Thread.Sleep(1800000);      //30min
			fillTrafficData();
		}

		public static string randomPpSong()
		{
			Random r = new Random();
			int rInt = r.Next(0, Program.pp_songs.Count - 1);
			var result = Program.pp_songs[rInt];

			return result;
		}

		public static string randomSong()
		{
			string result = String.Empty;
			try {
				string maxId = Program.database.getMaxId("musics");
				Random r = new Random();
				do {
					int rInt = r.Next(1, Convert.ToInt32(maxId));
					result = Program.database.getLineColumn("musics", "title", rInt.ToString());

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

			foreach (KeyValuePair<string, string> kvp in Program.mangasData) {
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

		public static bool verifyAdmin(SocketMessage message)
		{
			return Program.database.idAdmin(message.Author.Id.ToString());
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
			string allText = System.IO.File.ReadAllText(POKEMONS_FILE_NAME);
			string[] pokemons = allText.Split("<pokemon>\r\n");

			foreach (string pokemon in pokemons) {
				string[] infos = pokemon.Split("\r\n");

				int id = Int32.Parse(infos[0]);
				string urlIcon = infos[1];
				string name = infos[2].ToLower();
				int catchRate = Int32.Parse(infos[3]);
				int rarityTier = Int32.Parse(infos[4]);

				Program.database.addPokemon(id, urlIcon, name, catchRate, rarityTier);
			}
		}

		public static string getPokemonCatchRate(String pokemonName)
		{
			return Program.database.getPokemonInfo(pokemonName, Program.PokemonInfo.name, Program.PokemonInfo.catchRate);
		}

		public static string getPokemonRarityTier(String pokemonName)
		{
			return Program.database.getPokemonInfo(pokemonName, Program.PokemonInfo.name, Program.PokemonInfo.rarityTier);
		}

		public static string getPokemonCatchChances(float hp_percent, float catch_rate, float bonus_ball, float bonus_statut)
		{
			double formula = (1 - (2 / 3.0) * (hp_percent / 100.0)) * catch_rate * bonus_ball * bonus_statut;

			return formula.ToString("0.00");
		}

		public static Embed getAllPokemonInfo(String pokemonName)
		{
			string pokemonInfos = Program.database.getPokemonInfos(pokemonName);
			if (pokemonInfos == String.Empty) {
				return null;
			}

			pokemonInfos = pokemonInfos.Replace("(", "");
			pokemonInfos = pokemonInfos.Replace(")", "");
			pokemonInfos = pokemonInfos.Replace("'", "");
			pokemonInfos = pokemonInfos.Replace("///", "://");
			string[] infos = pokemonInfos.Split(", ");

			int id = Int32.Parse(infos[1]);
			string urlIcon = infos[2]; //"https://img.pokemondb.net/sprites/black-white/anim/normal/unown-a.gif";
			string nameEn = infos[3]; nameEn = char.ToUpper(nameEn[0]) + nameEn.Substring(1);
			string nameFr = infos[4]; nameFr = char.ToUpper(nameFr[0]) + nameFr.Substring(1);
			int catchRate = Int32.Parse(infos[5]);
			int rarityTier = Int32.Parse(infos[6]);

			EmbedBuilder builder = new EmbedBuilder() {
				Title = "-- " + nameEn + " / " + nameFr + " --",
				Description = "Pokemon info:",
				Color = new Color(89, 181, 255)
			};

			builder.ThumbnailUrl = urlIcon;

			builder.AddField("ID", id, true);
			builder.AddField("Catch rate", catchRate, true);
			builder.AddField("Rarity tier", rarityTier, true);

			return builder.Build();
		}

		public static void emptyBannedPeopleStack()
		{
			Program.people_spam = new Dictionary<ulong, int>();
			Thread.Sleep(15000);
			emptyBannedPeopleStack();
		}

		public static string banUser(string userId)
		{
			ulong userId_ = Convert.ToUInt64(userId);
			string msg = "Cet utilisateur était déjà banni.";

			if (userId_ == Program.master_id) {
				return "Bien essayé, mais non. 😏";
			}

			if (!Program.baned_people.Contains(userId_)) {
				Program.baned_people.Add(userId_);
				msg = "L'utilisateur a bien été banni.";
			}

			return msg;
		}

		public static string unbanUser(string userId)
		{
			ulong userId_ = Convert.ToUInt64(userId);
			string msg = "Cet utilisateur n'était pas banni.";

			if (Program.baned_people.Contains(userId_)) {
				Program.baned_people.Remove(userId_);
				msg = "L'utilisateur a bien été retiré des utilisateurs bannis.";
			}

			return msg;
		}

		public static void actionUsed(string action)
		{
			var actions_used = Program.actions_used;

			if (actions_used.ContainsKey(action)) {
				actions_used[action]++;
			}
			else {
				actions_used.Add(action, 1);
			}

			string actions = String.Empty;

			foreach (KeyValuePair<string, int> kvp in actions_used) {
				actions += kvp.Key + splitChar + kvp.Value.ToString() + "\n";
			}

			File.WriteAllText(POP_ACTIONS_FILE, actions);
		}

		public static string getPopActions()
		{
			string result = "Liste des actions les plus populaires :\n";
			var sortedActionsUsed = from entry in Program.actions_used orderby entry.Value descending select entry;

			foreach (KeyValuePair<string, int> kvp in sortedActionsUsed) {
				result += "- `" + kvp.Key + "` : " + kvp.Value.ToString() + "\n";
			}
			return result;
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
				Program.mangasData = new SortedDictionary<string, string>();
				string[] lines = System.IO.File.ReadAllLines(MANGASDATA_FILE_NAME2);
				foreach (string line in lines) {
					var data = line.Split(splitChar);
					var manga = data[0];
					var content = data[1];

					Program.mangasData.Add(manga, content);
				}
			}
			catch (Exception e) {
				displayException(e, "Exception on setupMangasData()");
			}
		}

		public static void setupPpSong()
		{
			string[] lines = System.IO.File.ReadAllLines(PP_FILE_NAME);
			foreach (string line in lines) {
				Program.pp_songs.Add(line);
			}
		}

		public static void setupOtherActionsList()
		{
			foreach (var action in Program.actions.getActions) {
				if (Actions.getActionType(action.Item1) == "Autres") {
					Program.autres.Add(action.Item1);
				}
			}
		}

		public static void setupPopActions()
		{
			string[] lines = System.IO.File.ReadAllLines(POP_ACTIONS_FILE);
			foreach (string line in lines) {
				var values = line.Split(splitChar);
				Program.actions_used.Add(values[0], Int32.Parse(values[1]));
			}
		}

		//Getters
		public static string getToken()
		{
			string[] lines = System.IO.File.ReadAllLines(TOKEN_FILE_NAME);
			return lines[0];
		}

		public static string getApiKey()
		{
			string[] lines = System.IO.File.ReadAllLines(TOKEN_FILE_NAME);
			return lines[1];
		}
	}

	static class Extensions
	{
		public static void debug<T>(this T t)
		{
			Console.WriteLine("/" + t + "/");
		}

		public static void debug<T>(this T[] list)
		{
			int count = 0;
			Console.WriteLine("Liste : ");

			foreach (var line in list) {
				Console.WriteLine("[" + count + "] : /" + line + "/");
				count++;
			}
			Console.WriteLine("");
		}

		public static void debug<T>(this List<T> list)
		{
			int count = 0;
			Console.WriteLine("Liste : ");

			foreach (var line in list) {
				Console.WriteLine("[" + count + "] : /" + line + "/");
				count++;
			}
			Console.WriteLine("");
		}

		public static void debug<T, G>(this Dictionary<T, G> dict)
		{
			if (dict.Count == 0) {
				Console.WriteLine("Empty dictionnary.");
				return;
			}
			int count = 0;
			Console.WriteLine("Dictionnary : ");

			foreach (var line in dict) {
				Console.WriteLine("[" + count + "] : /" + line.Key + "/ : /" + line.Value + "/");
				count++;
			}
			Console.WriteLine("");
		}

		public static void debug<T, G>(this SortedDictionary<T, G> dict)
		{
			int count = 0;
			Console.WriteLine("Dictionnary : ");

			foreach (var line in dict) {
				Console.WriteLine("[" + count + "] : /" + line.Key + "/ : /" + line.Value + "/");
				count++;
			}
			Console.WriteLine("");
		}

		public static void println<T>(this T t)
		{
			Console.WriteLine(t);
		}

		public static void print<T>(this T t)
		{
			Console.Write(t);
		}

		public static void print<T>(this T[] list)
		{
			int count = 0;
			Console.WriteLine("Liste : ");

			foreach (var line in list) {
				Console.WriteLine("[" + count + "] : " + line);
				count++;
			}
			Console.WriteLine("");
		}

		public static void print<T>(this List<T> list)
		{
			int count = 0;
			Console.WriteLine("Liste : ");

			foreach (var line in list) {
				Console.WriteLine("[" + count + "] : " + line);
				count++;
			}
			Console.WriteLine("");
		}

		public static void print<T, G>(this IDictionary<T, G> dict)
		{
			int count = 0;
			Console.WriteLine("Dictionnary : ");

			foreach (var line in dict) {
				Console.WriteLine("[" + count + "] : " + line.Key + " : " + line.Value);
				count++;
			}
			Console.WriteLine("");
		}
	}
}
