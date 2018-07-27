using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		public static string flip = "(╯°□°）╯︵ ┻━┻";
		public static string unflip = "┬─┬﻿ ノ( ゜-゜ノ)";

		public static string DB_FILE_SAVE = @"bdd_save.db";
		public static string DB_FILE_NAME = @"resources/bdd.db";
		public static string PP_FILE_NAME = @"resources/pp.txt";
		public static string LOGS_FILE_NAME = @"resources/logs.txt";
		public static string PYTHON_DIR_PATH = @"resources/python/";
		public static string TOKEN_FILE_NAME = @"resources/token.txt";
		public static string TRAJETS_FILE_NAME = @"resources/trajets.txt";
		public static string MANGASDATA_FILE_NAME = @"resources/data.txt";

		public static string PYTHON_EXE = @"C:\Users\utilisateur\AppData\Local\Programs\Python\Python37\python.exe";



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

			if (args.Length != 0)
			{
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

		public static string moreThanTwoThousandsChars(string str)
		{
			string result = String.Empty;

			int start = 0;
			while (str.Length >= 2000)
			{
				result += str.Substring(start, 1999) + "|";
				start += 1999;
				str = str.Remove(0, 1999);
			}
			result += str;

			return result;
			/*if (str == String.Empty)
				return new List<string>();

			str.debug();
			var a = (List<string>) Split(str, 1999);
			foreach (var b in a)
				b.aff();
			return a;*/
		}

		public static IEnumerable<string> Split(string str, int chunkSize)
		{
			return Enumerable.Range(0, str.Length / chunkSize)
				.Select(i => str.Substring(i * chunkSize, chunkSize));
		}

		public static string FormateSentence(string sentence)
		{
			string res = String.Empty;
			string indicator = ":regional_indicator_";

			for (int i = 0; i < sentence.Length; i++)
			{
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
			int it = 0;
			string[] split = sentence.Split(' ');
			for (int i = 0; i < split.Length; i++)
			{
				if (split[i].Contains(word))
					it++;
			}

			return it;
		}

		public static bool SentenceContainsWord(string sentence, string word)
		{
			string[] split = sentence.Split(' ');
			for (int i = 0; i < split.Length; i++)
			{
				if (split[i] == word)
					return true;
			}
			return false;
		}

		public static string displayMangasList()
		{
			int count = 0, number = 1;
			string result = "Voici les mangas qui sont check avec le web parser :\n";
			foreach (KeyValuePair<string, string> kvp in Program.mangasData)
			{
				result += "- " + kvp.Key + " (**" + number + "**)\n";
				number++;
				if (count++ >= 30)
				{
					count = 0;
					result += "|\n";
				}
			}
			return result;
		}

		public static string displayCompleteMangasList()
		{
			int count = 0;
			string result = "Voici, pour chaque manga, son dernier scan :\n\n";
			foreach (KeyValuePair<string, string> kvp in Program.mangasData)
			{
				var kvpValue = kvp.Value.Split(new[] { " => " }, StringSplitOptions.None);
				result += "```asciidoc\n[" + kvp.Key + "]```" + kvpValue[0] + "\n" + kvpValue[1] + "\n\n";
				if (count++ > 8)
				{
					count = 0;
					result += "|\n.";
				}
			}

			return result;
		}

		public static string lastChapter(string message_lower)
		{
			string msg = String.Empty;
			try
			{
				string manga = message_lower.Split(' ')[1];
				msg = getLastChapterOf(manga);
			}
			catch (ArgumentOutOfRangeException)
			{
				msg = "Ce manga n'existe pas. Le nom doit être de la forme : one-piece";
			}
			catch (Exception)
			{
				msg = "La commande est mal écrite. Elle doit être de la forme : !lastchapter one-piece";
			}
			return msg;
		}

		public static string getLastChapterOf(string manga)
		{
			string url = "http://www.japscan.cc/mangas/" + manga;

			var document = Dcsoup.Parse(new Uri(url), 30000);
			var divChaptersList = document.Select("div[id=liste_chapitres]");

			var firstChapterCatch = divChaptersList.Select("ul").Select("li").Select("a")[0];
			var firstChapterName = firstChapterCatch.Text;
			var link = "http:" + firstChapterCatch.ToString().Split('\"')[1];
			var version = "(VF)";
			try
			{
				version = divChaptersList.Select("ul").Select("li").Select("span")[0].Text;
			}
			catch (ArgumentOutOfRangeException) { /*displayException(e, "ArgumentOutOfRangeException, getLastChapterOf(string manga)");*/ }

			if (version == "(RAW)")
				version = "(JAP)";

			return firstChapterName + " **" + version + "** => <" + link + ">";
		}

		public static string addManga(string message_lower)
		{
			string msg = String.Empty;
			try
			{
				string manga = message_lower.Split(' ')[1];
				if (Program.mangasData.ContainsKey(manga))
				{
					msg = "Ce manga est déjà dans la liste poto ! :)";
					return msg;
				}
				string chapter = Utils.getLastChapterOf(manga);

				string result = "\n" + manga + "|" + chapter;
				System.IO.File.AppendAllText(Utils.MANGASDATA_FILE_NAME, result);
				setupMangasData();

				Program.database.addManga(manga);
				msg = "Manga '" + manga + "' ajouté à la liste.";
			}
			catch (ArgumentOutOfRangeException)
			{
				msg = "Ce manga n'existe pas. Le nom doit être de la forme : one-piece (minuscules séparées par un tiret)";
			}
			catch (Exception)
			{
				msg = "La commande est mal écrite. Elle doit être de la forme : !addmanga one-piece";
			}
			return msg;
		}

		public static string displayAllActions()
		{
			string type = String.Empty;
			string typeSave = String.Empty;
			string result = "Voici toutes les options du bot par catégorie :\n";

			foreach (var action in Program.actions.getActions)
			{
				if (action.Item2 == String.Empty) { continue; }

				if (action.Item1.StartsWith("!!")) { type = "Les Commandes admin"; }
				else if (action.Item1.StartsWith("!")) { type = "Les Commandes"; }
				else if (action.Item1.StartsWith("$")) { type = "Les Deletes"; }
				else { type = "Autres"; }

				if (type != typeSave)
				{
					typeSave = type;
					result += "\n[" + type + "]\n";
				}

				result += "\t - " + action.Item1 + " : " + action.Item2 + "\n";
			}

			return result;
		}

		public static void getAllNewChapters()
		{
			var time = DateTime.Now;
			Console.WriteLine("getAllNewChapters (" + time + ")");
			foreach (KeyValuePair<string, string> kvp in Program.mangasData)
			{
				try
				{
					string chapter = Utils.getLastChapterOf(kvp.Key);
					if (kvp.Value != chapter)
					{
						string toWrite = imitateMangasData();
						int pos = toWrite.IndexOf(kvp.Key) + kvp.Key.Length + 1;
						int nextLinePos = toWrite.IndexOf('\n', pos);

						if (nextLinePos == -1)
							nextLinePos = toWrite.Length;

						toWrite = toWrite.Remove(pos, nextLinePos - pos);
						toWrite = toWrite.Insert(pos, chapter);

						System.IO.File.WriteAllText(Utils.MANGASDATA_FILE_NAME, toWrite);

						string subs = String.Empty;
						var users = Program.database.getSubs(kvp.Key);

						foreach (var user in users)
						{
							subs += "<@" + user + "> ";
						}

						string msg = "Nouveau scan trouvé pour " + kvp.Key + " : \n\t" + chapter;
						if (!chapter.Contains("VUS") && !chapter.Contains("JAP") && !chapter.Contains("SPOILER") && !chapter.Contains("RAW"))
							sendMessageTo(Program.channels["mangas"], msg + " " + subs);

						setupMangasData();
					}
				}
				catch (Exception e)
				{
					Utils.displayException(e, "Exception on getAllNewChapters()");
				}
			}
			var now = DateTime.Now - time;
			Console.WriteLine("search done. (" + DateTime.Now + ") [" + now + "]");

			Thread.Sleep(10800000); //3h
									//Thread.Sleep(1800000);		//30min
			getAllNewChapters();
		}

		public static void sendMessageTo(ulong channel, string message)
		{
			try
			{
				((SocketTextChannel)Program._client.GetChannel(channel)).SendMessageAsync(message);
			}
			catch (Exception e)
			{
				Utils.displayException(e, "Impossible to send message, sendMessageTo(ulong channel, string message)");
			}
		}

		public static async Task reply(SocketMessage message, string msg)
		{
			try
			{
				await message.Channel.SendMessageAsync(msg);
			}
			catch (Exception e)
			{
				Utils.displayException(e, "Impossible to send message, reply(SocketMessage message, string msg)");
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
				System.IO.File.AppendAllText(Utils.TRAJETS_FILE_NAME, result);

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
			result = (string)duration;

			response.Close();

			return result;
		}

		public static string imitateMangasData()
		{
			string result = String.Empty;

			foreach (KeyValuePair<string, string> kvp in Program.mangasData)
			{
				result += kvp.Key + "|" + kvp.Value + "\n";
			}

			return result.Substring(0, result.Length - 1);
		}

		public static string GetPing(DateTime old)
		{
			return ((long)((TimeSpan)(DateTime.Now - old)).TotalMilliseconds).ToString();
		}

		public static async void DeleteMessage(SocketMessage message)
		{
			try
			{
				await message.DeleteAsync();
			}
			catch (Exception e)
			{
				Utils.displayException(e, "DeleteException on DeleteMessage(SocketMessage message)");
			}
		}

		public static string displayAllTags()
		{
			string result = "Voici tous les tags :\n";
			foreach (KeyValuePair<string, string> entry in Program.mention)
			{
				result += "\t - " + entry.Key + " : " + entry.Value + "\n";
			}
			return result;
		}

		public static bool verifyAdmin(SocketMessage message)
		{
			return Program.database.idAdmin(message.Author.Id.ToString());
		}

		public static void setupMangasData()
		{
			try
			{
				Program.mangasData = new SortedDictionary<string, string>();
				string[] lines = System.IO.File.ReadAllLines(Utils.MANGASDATA_FILE_NAME);
				foreach (string line in lines)
				{
					var data = line.Split('|');
					var manga = data[0];
					var content = data[1];

					Program.mangasData.Add(manga, content);
				}
			}
			catch (Exception e)
			{
				Utils.displayException(e, "Exception on setupMangasData()");
			}
		}

		public static void setupPpSong()
		{
			string[] lines = System.IO.File.ReadAllLines(Utils.PP_FILE_NAME);
			foreach (string line in lines)
			{
				Program.pp_songs.Add(line);
			}
		}

		public static string getToken()
		{
			string[] lines = System.IO.File.ReadAllLines(Utils.TOKEN_FILE_NAME);
			return lines[0];
		}

		public static string getApiKey()
		{
			string[] lines = System.IO.File.ReadAllLines(Utils.TOKEN_FILE_NAME);
			return lines[1];
		}
	}

	static class Extensions
	{
		public static void aff(this string str)
		{
			Console.WriteLine(str);
		}
		public static void debug(this string str)
		{
			Console.WriteLine("/" + str + "/");
		}

		public static void aff(this int entier)
		{
			Console.WriteLine(entier);
		}

		public static void aff(this bool var)
		{
			Console.WriteLine(var);
		}
	}
}
