
using DiscordBot.Data;
using Supremes;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;

namespace DiscordBot.Utilities
{
	public class ThreadUtils
	{
		// --- banned people ---
		public static void EmptyBannedPeopleStack()
		{
			DataManager.people_spam = new Dictionary<ulong, int>();
			Thread.Sleep(15000);
			EmptyBannedPeopleStack();
		}

		// --- mangas ---
		public static async void MangasCrawlerOnLireScanV2()
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
				string data = String.Empty;
				List<string> processedMangas = new List<string>();
				string text_data = File.ReadAllText(DataManager.Text.MANGASDATA_RSS_FILE);

				foreach (SyndicationItem item in feed.Items) {
					String title = item.Title.Text;
					String link = item.Links[0].Uri.ToString();
					String description = item.Summary.Text;
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
							bool isVF = (pNotif.Text == String.Empty);

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
				e.DisplayException(System.Reflection.MethodBase.GetCurrentMethod().ToString());
			}

			var now = DateTime.Now - time;
			("search done. (" + DateTime.Now + ") [" + now + "]").Println();
			Thread.Sleep(1800000);  //30min
			MangasCrawlerOnLireScanV2();
		}

		private static string MangaNameToLowerCase(string mangaName)
		{
			//One Piece
			//one-piece
			return mangaName.Replace(" ", "-").Replace(".", "").Replace("Scan-", "").Replace("VF", "").Replace(mangaName.OnlyKeepDigits(), "").ToLower().Replace("--", "");
		}

		private static string MangaNameToUpperCase(string mangaName)
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
	}
}
