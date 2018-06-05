using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using HtmlAgilityPack;
using Supremes;
using System.Threading;
using System.IO;

namespace DiscordBot
{
	public class Program
	{
		const string TOKEN_FILE_NAME = @"resources/token.txt";
		const string MANGASDATA_FILE_NAME = @"resources/data.txt";
		const string MANGASATTENTE_FILE_NAME = @"resources/mangas_en_attentes.txt";

		Dictionary<string, ulong> channels = new Dictionary<string, ulong>()
		{
			{ "general",        309407896070782976 },
			{ "mangas",         439960408703369217 },
			{ "mangas_liste",   440228865881800704 },
			{ "debug",          353262627880697868 },
			{ "zone51",         346760327540506643 },
			{ "peguts",         392118626561294346 }
		};

		Dictionary<string, string> all_actions = new Dictionary<string, string>()
		{
			{ "==a" , "Les commandes" },
			{ "!ping" , "Affiche le ping du bot." },
			{ "!date" , "Affiche la date." },
			{ "!flip" , "Lance une table." },
			{ "!unflip" , "Replace une table." },
			{ "!clean" , "Clean l'espace de message." },
			{ "!mangas" , "Affiche la liste des mangas traités." },
			{ "!scans" , "Affiche le dernier scan pour chaque mangas traités." },
			{ "!lastchapter" , "Affiche le dernier scan pour le manga en paramètre. (!lastchapter one-piece)" },
			{ "!addmanga" , "Ajoute le manga en paramètre à la liste. (!addmanga one-piece)" },
			{ "!help" , "Affiche toutes les options." },

			{ "==b" , "Les deletes" },
			{ "$fs" , "Formate la phrase qui suit avec de jolies lettres." },
			{ "$lenny" , "Affiche le meme 'lenny'." },
			{ "$popopo" , "Affiche le meme 'popopo'." },
			{ "$ken" , "Tu ne le sais pas, mais tu es déjà mort." },
			{ "$amaury meme" , "Tout le monde sait ce que c'est." },
			{ "$mytho ultime" , "El mytho ultima." },
			{ "$los" , "Trigger la dreamteam de LOS !!" },

			{ "==c" , "Autres" },
			{ "fap" , "Si ta phrase contient un fap alors ... ;)" },
			{ "bite" , "Si ta phrase contient une bite alors PEPE." },
			{ "musique de génie" , "Le jour où tu veux écouter de la vrai musique." },
			{ "Gamabunta" , "Meme naruto." },
			{ "invocation" , "Idem." },
			{ "welcome" , "Meme Resident Evil 4." },
			{ "évidemment" , "Meme Antoine Daniel." },
			{ "je vais chier" , "Meme South Park." },
			{ "omae wa mou shindeiru" , "Si t'es flemmard, 'omae' suffit xD." },
			{ "hanauta sancho" , "Le génie de Brook." },
			{ "DETROIT SMAAAAAAAAASH" , "Cqfd." },

			{ "==d" , "Automatique" },
			{ "Les hashtags" , "Plus utilisé depuis 2012 (excepté par moi)." },
			{ "BALDSIGNAL !!" , "Se lance quand on utilise l'émote du baldsignal !!" }
		};

		static Dictionary<string, string> pseudo = new Dictionary<string, string>()
		{
			{ "Ferrone",    "Ferrone" },
			{ "Luc",        "𝓕𝓵𝓾𝓽𝓽𝓮𝓻𝓢𝓱𝔂" },
			{ "Bruno",      "Faellyss" }
		};

		Dictionary<string, string> mention = new Dictionary<string, string>()
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

		private DiscordSocketClient _client;
		private CancellationTokenSource delay_controller;
		private ulong master_id = 293780484822138881;

		SortedDictionary<string, string> mangasData = new SortedDictionary<string, string>();
		List<string> baned_people = new List<string>() { /*pseudo["Luc"], "Faellyss"*/ };
		const string alpha = "abcdefghijklmnopqrstuvwxyz";
		private string flip = "(╯°□°）╯︵ ┻━┻";
		private string unflip = "┬─┬﻿ ノ( ゜-゜ノ)";

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();

			_client.Log += Log;
			_client.MessageReceived += MessageReceived;
			delay_controller = new CancellationTokenSource();

			string token = getToken();
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			//mes setups
			setupMangasData();

			//Thread qui regarde les nouveaux scans
			Thread thread = new Thread(getAllNewChapters);
			thread.Start();

			// Block this task until the program is closed.
			try
			{
				await Task.Delay(-1, delay_controller.Token);
			}
			catch (TaskCanceledException e)
			{
				Console.WriteLine("Le bot a bien été coupé.");
			}
			_client.MessageReceived -= MessageReceived;
			await _client.LogoutAsync();
			await _client.StopAsync();
			_client.Dispose();
		}

		private async Task MessageReceived(SocketMessage message)
		{
			if (baned_people.Contains(message.Author.Username) || message.Author.IsBot)
			{
				return;
			}

			string message_lower = message.Content.ToLower();
			//Console.WriteLine("normal : "+message.Content);
			//Console.WriteLine("lower  : "+message_lower);

			if (message.Author.Id == master_id)
			{
				if (message.Content.Contains("/quit"))
				{
					delay_controller.Cancel();
					return;
				}
			}

			///////////////////////////////////////////////////////////////////
			//							Les commandes
			///////////////////////////////////////////////////////////////////

			if (message_lower == "!ping")
			{
				string msg = "Pong! Mon ping est de : " + _client.Latency.ToString() + "ms.";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.StartsWith("!date"))
			{
				string _t = "";
				if (message_lower.Contains("day"))
				{
					_t = DateTime.Now.Day.ToString();
				}
				else if (message_lower.Contains("month"))
				{
					_t = DateTime.Now.Month.ToString();
				}
				else if (message_lower.Contains("year"))
				{
					_t = DateTime.Now.Year.ToString();
				}
				else if (message_lower.Contains("time"))
				{
					_t = DateTime.Now.TimeOfDay.ToString();
					_t = _t.Remove(8, _t.Length - 8);
				}
				else
					_t = DateTime.Now.ToString();
				string msg = _t;
				await message.Channel.SendMessageAsync(msg);
			}
			if (SentenceContainsWord(message.ToString(), "rage") || message_lower == "!flip" || message_lower.Contains(unflip))
			{
				string msg = flip;
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "!unflip" || message_lower.Contains(flip))
			{
				string msg = unflip;
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "!clean")
			{
				string msg = "Clean en cours...";
				for (int i = 0; i < 60; i++)
					msg += "\n";
				msg += "\nClean terminé.";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "!mangas")
			{
				var msgs = displayMangasList().Split('|');
				foreach (string msg in msgs)
					await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "!scans")
			{
				try
				{
					var msgs = displayCompleteMangasList().Split('|');
					foreach (string msg in msgs)
						await message.Channel.SendMessageAsync(msg);
				}
				catch (Exception e)
				{
					displayException(e, "!scans");
				}
			}
			if (message_lower.StartsWith("!lastchapter"))
			{
				string msg = lastChapter(message_lower);
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.StartsWith("!addmanga"))
			{
				string msg = addManga(message_lower);
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "!help")
			{
				string msg = displayAllActions();
				await message.Channel.SendMessageAsync(msg);
			}


			///////////////////////////////////////////////////////////////////
			//							Les deletes
			///////////////////////////////////////////////////////////////////

			if (message_lower.StartsWith("$fs"))
			{
				DeleteMessage(message);
				string msg = message_lower.Substring(3).ToLower();
				if (message.Author.Username == pseudo["Luc"])
					msg = FormateSentence(msg) + " by FlutterShy / Blossom / Pupute";
				else
					msg = FormateSentence(msg) + " by " + message.Author.Username;
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "$lenny")
			{
				DeleteMessage(message);
				string msg = "( ͡° ͜ʖ ͡°)";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "$popopo")
			{
				DeleteMessage(message);
				string msg = "https://cdn.discordapp.com/attachments/346760327540506643/353847873458274304/popopo.png";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "$ken")
			{
				DeleteMessage(message);
				string msg = "OMAE WA MOU ... SHINDEIRU !";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "$amaury meme")
			{
				DeleteMessage(message);
				string msg = "https://cdn.discordapp.com/attachments/309407896070782976/353833262273134592/Sans_titre.png";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower == "$mytho ultime")
			{
				DeleteMessage(message);
				string msg = "https://cdn.discordapp.com/attachments/346760327540506643/402253939573129217/amaury_ultime.jpg";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.StartsWith("$los"))
			{
				DeleteMessage(message);
				string msg = "LOS ? " + mention["Bringer"] + " " + mention["Renaud"] + " " + mention["Dimitri"] + " " + mention["Bruno"] + " " + mention["Pierre"] + " " + mention["Mayeul"] + " " + mention["Ferrone"];
				if (message.Author.Username == pseudo["Luc"])
					msg = "Luc qui casse les couilles a vouloir trigger le LOS !";
				else
					msg += " - " + message.Author.Username + " veut jouer !";

				if (message_lower != "$los")
				{
					msg += " [" + message_lower.Substring(5) + "]";
				}
				await message.Channel.SendMessageAsync(msg);
			}


			///////////////////////////////////////////////////////////////////
			//							  Autres
			///////////////////////////////////////////////////////////////////

			if (message_lower.Contains("fap"))
			{
				string msg = "https://giphy.com/gifs/fap-Bk2NzCbwFH6sE";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("bite"))
			{
				int it = CountIterations(message.ToString().ToLower(), "bite");
				string pepe = "<:pepe:329281047730585601> ";
				string msg = "";
				for (int i = 0; i < it; i++)
					msg += pepe;
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("musique de génie"))
			{
				string msg = "https://www.youtube.com/watch?v=kXYiU_JCYtU&index=146&list=PLi7ipd_Aw87Xv1s_L8p1NZ6cg9Ny0dQEi&ab_channel=LinkinPark";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message.Content.Contains("Gamabunta"))
			{
				string msg = "Kuchiyose no jutsu ! https://vignette3.wikia.nocookie.net/naruto/images/8/84/Gamabunta.png/revision/latest?cb=20160623114719";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("invocation"))
			{
				string msg = "Kuchiyose no jutsu !";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("welcome"))
			{
				string msg = "https://www.youtube.com/watch?v=o0kGvgXmmgk&ab_channel=DiegoSousa";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("évidemment") || message_lower.Contains("evidemment"))
			{
				string msg = "https://www.youtube.com/watch?v=A4-MlnvXo2I&ab_channel=FansDesRoisD%27Internet";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("je vais chier") || message_lower.Contains("jvais chier") || message_lower.Contains("va chier"))
			{
				string msg = "http://ci.memecdn.com/2675825.jpg";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("omae"))
			{
				string msg = "NANIIII !!??";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("sancho"))
			{
				string msg = "https://cdn.discordapp.com/attachments/346760327540506643/397856433145905156/yahazu_giri.png";
				await message.Channel.SendMessageAsync(msg);
			}
			if (message_lower.Contains("detroit") || message_lower.Contains("smash"))
			{
				string msg = "https://giphy.com/gifs/hero-smash-boku-XE8j547LpglrO";
				await message.Channel.SendMessageAsync(msg);
			}


			///////////////////////////////////////////////////////////////////
			//							  Automatique
			///////////////////////////////////////////////////////////////////

			if (message_lower.Contains("#") && message.Author.Username != "Ferrone")
			{
				string msg = "Les hashtags c'est démodé quand même :/ depuis 2012 connard.";
				await message.Channel.SendMessageAsync(msg);
			}
			if ((message_lower.Contains("bald") && message_lower.Contains("signal")) || message_lower.Contains("baldsignal"))
			{
				string msg = mention["Ferrone"];
				await message.Channel.SendMessageAsync(msg);
			}

			///////////////////////////////////////////////////////////////////
			//							En construction
			///////////////////////////////////////////////////////////////////
			/*if (message.Channel.ToString().Contains("@") && message.Channel.ToString().Contains("#"))
			{
				//await message.Channel.SendMessageAsync("arrête de m'embetter");
				//await message.Channel.SendMessageAsync(FormateSentence("salut ca va"));
			}*/


			//debug zone
			if (message_lower == "!d")
			{
				string msg = "";
				sendMessageTo(channels["debug"], msg);
				Console.WriteLine(msg);
			}

			Console.WriteLine("Message reçu de " + message.Author.Username + " : " + message_lower);
		}

		private void sendMessageTo(ulong channel, string message)
		{
			try
			{
				((SocketTextChannel)_client.GetChannel(channel)).SendMessageAsync(message);
			}
			catch (Exception e)
			{
				displayException(e, "Impossible to send message, sendMessageTo(ulong channel, string message)");
			}
		}

		private string lastChapter(string message_lower)
		{
			string msg = "";
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

		private string addManga(string message_lower)
		{
			string msg = "";
			try
			{
				string manga = message_lower.Split(' ')[1];
				if (mangasData.ContainsKey(manga))
				{
					msg = "Ce manga est déjà dans la liste poto ! :)";
					return msg;
				}
				string chapter = getLastChapterOf(manga);

				string result = "\n" + manga + "|" + chapter;
				System.IO.File.AppendAllText(MANGASDATA_FILE_NAME, result);
				setupMangasData();

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

		private void getAllNewChapters()
		{
			var time = DateTime.Now;
			Console.WriteLine("getAllNewChapters (" + time + ")");
			foreach (KeyValuePair<string, string> kvp in mangasData)
			{
				try
				{
					string chapter = getLastChapterOf(kvp.Key);
					if (kvp.Value != chapter)
					{
						string toWrite = imitateMangasData();
						int pos = toWrite.IndexOf(kvp.Key) + kvp.Key.Length + 1;
						int nextLinePos = toWrite.IndexOf('\n', pos);

						if (nextLinePos == -1)
							nextLinePos = toWrite.Length;

						toWrite = toWrite.Remove(pos, nextLinePos - pos);
						toWrite = toWrite.Insert(pos, chapter);

						System.IO.File.WriteAllText(MANGASDATA_FILE_NAME, toWrite);

						string msg = "Nouveau scan trouvé pour " + kvp.Key + " : \n\t" + chapter;
						sendMessageTo(channels["mangas"], msg);

						setupMangasData();
					}
				}
				catch (Exception e)
				{
					displayException(e, "Exception on getAllNewChapters()");
				}
			}
			var now = DateTime.Now - time;
			Console.WriteLine("search done. (" + DateTime.Now + ") [" + now + "]");

			Thread.Sleep(10800000);
			getAllNewChapters();
		}

		private string imitateMangasData()
		{
			string result = "";

			foreach (KeyValuePair<string, string> kvp in mangasData)
			{
				result += kvp.Key + "|" + kvp.Value + "\n";
			}

			return result.Substring(0, result.Length - 1);
		}

		private string getLastChapterOf(string manga)
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

		private void setupMangasData()
		{
			try
			{
				mangasData = new SortedDictionary<string, string>();
				string[] lines = System.IO.File.ReadAllLines(MANGASDATA_FILE_NAME);
				foreach (string line in lines)
				{
					var data = line.Split('|');
					var manga = data[0];
					var content = data[1];

					mangasData.Add(manga, content);
				}
			}
			catch (Exception e)
			{
				displayException(e, "Exception on setupMangasData()");
			}
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());

			return Task.CompletedTask;
		}

		private string GetPing(DateTime old)
		{
			return ((long)((TimeSpan)(DateTime.Now - old)).TotalMilliseconds).ToString();
		}

		private int CountIterations(string sentence, string word)
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

		private bool SentenceContainsWord(string sentence, string word)
		{
			string[] split = sentence.Split(' ');
			for (int i = 0; i < split.Length; i++)
			{
				if (split[i] == word)
					return true;
			}
			return false;
		}

		private string FormateSentence(string sentence)
		{
			string res = "";
			string indicator = ":regional_indicator_";

			for (int i = 0; i < sentence.Length; i++)
			{

				if (sentence[i] == ' ')
					res += "     ";
				else if (alpha.Contains(sentence[i]))
					res += indicator + sentence[i] + ": ";
				else
					res += sentence[i] + " ";
			}

			return res;
		}

		private async void DeleteMessage(SocketMessage message)
		{
			try
			{
				await message.DeleteAsync();
			}
			catch (Exception e)
			{
				displayException(e, "DeleteException on DeleteMessage(SocketMessage message)");
			}
		}

		private string displayAllActions()
		{
			string result = "Voici toutes les options du bot par catégorie :\n";
			foreach (KeyValuePair<string, string> entry in all_actions)
			{
				if (entry.Key.StartsWith("=="))
					result += "\n[" + entry.Value + "]\n";
				else
					result += "\t - " + entry.Key + " : " + entry.Value + "\n";
			}
			return result;
		}

		private string displayAllTags()
		{
			string result = "Voici tous les tags :\n";
			foreach (KeyValuePair<string, string> entry in mention)
			{
				result += "\t - " + entry.Key + " : " + entry.Value + "\n";
			}
			return result;
		}

		private string displayMangasList()
		{
			int count = 0, number = 1;
			string result = "Voici les mangas qui sont check avec le web parser :\n";
			foreach (KeyValuePair<string, string> kvp in mangasData)
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

		private string displayCompleteMangasList()
		{
			int count = 0;
			string result = "Voici, pour chaque manga, son dernier scan :\n\n";
			foreach (KeyValuePair<string, string> kvp in mangasData)
			{
				var kvpValue = kvp.Value.Split(new[] { " => " }, StringSplitOptions.None);
				result += "```asciidoc\n[" + kvp.Key + "]```" + kvpValue[0] + "\n" + kvpValue[1] + "\n\n";
				if (count++ >= 10)
				{
					count = 0;
					result += "|\n.";
				}
			}

			return result;
		}





		private void displayException(Exception e, string message = "Error")
		{
			Console.WriteLine(message + " : \n" + e.Message + "\n");
			Console.WriteLine(e.StackTrace);
		}

		private string getToken()
		{
			string[] lines = System.IO.File.ReadAllLines(TOKEN_FILE_NAME);
			return lines[0];
		}
	}



	class BotStateManager
	{
		public static void Save<T>(string filename, T obj)
		{
			using (Stream stream = File.Open(filename, FileMode.Create))
			{
				var binary_serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				binary_serializer.Serialize(stream, obj);
			}
		}
		public static T Load<T>(string filename)
		{
			using (Stream stream = File.Open(filename, FileMode.Open))
			{
				var binary_serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				return (T)binary_serializer.Deserialize(stream);
			}
		}
	}
}
