using Discord.WebSocket;
using DiscordBot.Data;
using Supremes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;

namespace DiscordBot.Utilities
{
	public class ThreadUtils
	{
		// --- mangas japscan ---
		public static async void MangasCrawlerJapscan()
		{
			string site = "https://www.japscan.co/";
			string url = site + "rss/";
			var time = DateTime.Now;
			("MangasCrawlerJapscan (" + time + ")").Println();

			try {
				//string rss_test = File.ReadAllText(DataManager.TEXT_DIR + "rss_test.xml");
				//XmlReader reader = XmlReader.Create(new StringReader(rss_test));

				string crawlResult = new Crawler.Crawler(url).CrawlCloudFlare();

				XmlReader reader = XmlReader.Create(new StringReader(crawlResult));
				SyndicationFeed feed = SyndicationFeed.Load(reader);
				reader.Close();

				string splitChar = "|";
				string data = string.Empty;
				List<string> processedMangas = new List<string>();
				string text_data = File.ReadAllText(DataManager.Text.MANGASDATA_RSS_FILE);

				foreach (SyndicationItem item in feed.Items) {
					string title = item.Title.Text;
					string link = item.Links[0].Uri.ToString();
					DateTimeOffset pubDate = item.PublishDate;
					string description = item.Summary.Text;
					string mangaName = link.Split("/")[2];
					bool mangaExists = DataManager.mangasData.ContainsKey(mangaName);

					link = site + link.Substring(1);
					string chapter = title + splitChar + link + splitChar + description + splitChar + pubDate.ToString();

					if (mangaExists && !processedMangas.Contains(mangaName)) {

						bool alreadyInDataList = false;
						string savedChapter = string.Empty;
						string chapterWithoutDate = title + splitChar + link + splitChar + description;

						foreach (string dataLine in text_data.Split('\n')) {
							if (dataLine != string.Empty && chapterWithoutDate.Equals(dataLine.Replace(splitChar + dataLine.Split(splitChar)[3], string.Empty))) {
								savedChapter = dataLine;
								alreadyInDataList = true;
							}
						}

						bool newChapter = false;
						if (alreadyInDataList) {
							var newDate = DateTimeOffset.Parse(savedChapter.Split(splitChar)[3]);

							if (pubDate.CompareTo(newDate) > 0) {
								("rentre (" + mangaName + " : " + pubDate.ToString() + " < " + newDate.ToString() + " )").Debug();
								newChapter = true;
							}
						}
						else {
							"rentre (notInList)".Debug();
							newChapter = true;
						}

						if (newChapter) {

							bool isVF = false;
							try {
								isVF = new Crawler.Crawler(link).CrawlCloudFlare().Split("role=\"alert\"")[1].Split("</div>")[0] == string.Empty;
							}
							catch (Exception) { isVF = true; }

							string subs = "NOT VF";
							string scanValue = "Scan " + title + " => <" + link + ">";
							string msg = "Nouveau scan trouvé pour " + mangaName + " : \n\t" + scanValue;

							if (isVF) {
								subs = string.Empty;
								var users = DataManager.database.getSubs(mangaName);
								foreach (var user in users) {
									subs += "<@" + user + "> ";
								}
							}
							await Channels.Mangas.SendMessageAsync(msg + " " + subs);
						}

						processedMangas.Add(mangaName);
						data += chapter + "\n";
					}
				}

				File.WriteAllText(DataManager.Text.MANGASDATA_RSS_FILE, data);
			}
			catch (Exception e) {
				if (e is WebException) {
					await Channels.Debug.SendMessagesAsync("Le crawl des mangas a échoué, car la connexion au site a échouée.\n" + e);
				}
				e.Display(System.Reflection.MethodBase.GetCurrentMethod().ToString());
			}

			var now = DateTime.Now - time;
			("search done. (" + DateTime.Now + ") [" + now + "]").Println();
			Thread.Sleep(1800000);  //30min
			MangasCrawlerJapscan();
		}

		// --- qwertee ---
		public static async void QwerteeThread()
		{
			string msg = string.Empty;
			string url = "https://www.qwertee.com/";

			try {
				string crawlResult = new Crawler.Crawler(url).Crawl("div.big-slides-wrap");
				crawlResult = crawlResult.Split("big-slides-wrap")[1];

				int number = 0;
				foreach (string picture in crawlResult.Split("<picture>")) {
					if (number != 0 && number != 4) {
						string img = picture.Split("</picture>")[0];
						string src = img.Split("<img src=\"")[1].Split("\">")[0];
						msg += src + "\n";
					}
					number++;
				}

				crawlResult = new Crawler.Crawler(url).Crawl("div.index-countdown");

				int flatTime = int.Parse(crawlResult.Split("data-time=\"")[1].Split("\">")[0]) - 86400;
				int hours = flatTime / 3600;
				int minutes = (flatTime % 3600) / 60;
				int seconds = flatTime % 60;
				string time = hours + " heures " + minutes + " minutes " + seconds + " secondes";

				var channel = Channels.Qwertee;
				await (channel as SocketTextChannel).DeleteMessagesAsync(channel.GetMessages(1));
				await channel.SendMessageAsync(msg + "\nVoici les t-shirts du jour pour une durée restante de (" + time + ") : ");
			}
			catch (Exception e) {
				e.Display();
			}

			Thread.Sleep(86400000);  //24h
			QwerteeThread();
		}

		// --- banned people ---
		public static void EmptyBannedPeopleStackThread()
		{
			DataManager.people_spam = new Dictionary<ulong, int>();
			Thread.Sleep(15000);
			EmptyBannedPeopleStackThread();
		}

		// --- mangas lirescan ---
		public static async void MangasCrawlerOnLireScanV2Thread()
		{
			string site = "https://www.lirescan.me/";
			string url = site + "rss/";
			var time = DateTime.Now;
			("mangasCrawlerOnLireScanV2 (" + time + ")").Println();

			try {
				XmlReader reader = XmlReader.Create(url);
				SyndicationFeed feed = SyndicationFeed.Load(reader);
				reader.Close();

				string splitChar = "|";
				int crawler_counter = 0;
				string data = string.Empty;
				List<string> processedMangas = new List<string>();
				string text_data = File.ReadAllText(DataManager.Text.MANGASDATA_RSS_FILE);

				foreach (SyndicationItem item in feed.Items) {
					string title = item.Title.Text;
					string link = item.Links[0].Uri.ToString();
					string description = item.Summary.Text;
					string mangaName = MangaNameToLowerCase(title);
					bool mangaExists = DataManager.mangasData.ContainsKey(mangaName);
					link = link.Replace("http://www.lirescan.com/", "");
					link = site + mangaName + link;

					string chapter = title + splitChar + link + splitChar + description;

					if (mangaExists && !processedMangas.Contains(mangaName)) {
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
								("rentre (" + crawler_counter.ToString() + " < " + data_counter.ToString() + " )").Debug();
								newChapter = true;
							}
						}
						else {
							"rentre (notInList)".Debug();
							newChapter = true;
						}

						if (newChapter) {
							Supremes.Nodes.Document document = null;
							try {
								document = Dcsoup.Parse(new Uri(link), 15000);
							}
							catch (Exception) {
								("Timeout on : <" + link + ">").Debug();
								throw new TimeoutException("Timeout on : <" + link + ">");
							}

							var pNotif = document.Select("p[id=notif]");
							bool isVF = (pNotif.Text == string.Empty);

							if (isVF) {
								string scanValue = title + " => <" + link + ">";
								string subs = string.Empty;
								var users = DataManager.database.getSubs(mangaName);
								string msg = "Nouveau scan trouvé pour " + mangaName + " : \n\t" + scanValue;

								foreach (var user in users) {
									subs += "<@" + user + "> ";
								}
								await Channels.Mangas.SendMessageAsync(msg + " " + subs);
							}
						}

						processedMangas.Add(mangaName);
						data += chapter + "\n";
					}

					crawler_counter++;
				}

				File.WriteAllText(DataManager.Text.MANGASDATA_RSS_FILE, data);
			}
			catch (Exception e) {
				await Channels.Debug.SendMessagesAsync("Le crawl des mangas a échoué, car la connexion au site a échouée.\n" + e);
				e.Display(System.Reflection.MethodBase.GetCurrentMethod().ToString());
			}

			var now = DateTime.Now - time;
			("search done. (" + DateTime.Now + ") [" + now + "]").Println();
			Thread.Sleep(1800000);  //30min
			MangasCrawlerOnLireScanV2Thread();
		}

		// --- Utils ---

		private static string MangaNameToLowerCase(string mangaName)
		{
			//One Piece => one-piece
			return mangaName.Replace(" ", "-").Replace(".", "").Replace("Scan-", "").Replace("VF", "").Replace(mangaName.OnlyKeepDigits(), "").ToLower().Replace("--", "");
		}

		private static string MangaNameToUpperCase(string mangaName)
		{
			//one-piece => One Piece
			string result = string.Empty;
			var tokens = mangaName.Split('-');
			foreach (var token in tokens) {
				var new_token = char.ToUpper(token[0]) + token.Substring(1);
				result += new_token + " ";
			}

			return result.Substring(0, result.Length - 1);
		}
	}
}
