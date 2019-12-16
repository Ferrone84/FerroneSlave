using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.CommandActions
{
	public class AddMangas : ACommandAction
	{
		public AddMangas() : base()
		{
			Name = Prefix + "addmangas";
			Description = "Ajoute les mangas en paramètre à la liste. (!addmangas one-piece bleach naruto)";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string[] mangas = null;
			string msg = string.Empty;

			try {
				mangas = message.Content.Replace(Name + " ", string.Empty).Split(' ');
			}
			catch (Exception e) {
				e.Display();
				await message.Channel.SendMessageAsync("La commande est mal écrite. Elle doit être de la forme : !addmangas one-piece bleach naruto");
				return;
			}

			foreach (string manga in mangas) {
				msg += manga + " : " + AddMangaJapscan(manga) + "\n";
			}
			await message.Channel.SendMessagesAsync(msg);
		}

		private string AddMangaJapscan(string manga)
		{
			string msg = string.Empty;

			try {
				if (DataManager.mangasData.ContainsKey(manga)) {
					return "Ce manga est déjà dans la liste poto ! :)";
				}

				string site = "https://www.japscan.co/";
				string link = site + "lecture-en-ligne/" + manga + "/";

				string crawlerResult = new Crawler.Crawler(link).CrawlCloudFlare();

				//Si on arrive ici sans exception c'est que c'est bon
				string result = "\n" + manga;
				System.IO.File.AppendAllText(DataManager.Text.MANGASDATA_FILE, result);
				DataManager.mangasData = Utils.InitMangasData();

				DataManager.database.addManga(manga);
				msg = "Manga '" + manga + "' ajouté à la liste.";
			}
			catch (Exception e) {
				if (e is ArgumentOutOfRangeException || e is WebException) {
					msg = "Ce manga n'existe pas ou le site ne le possède pas. Le nom doit être de la forme : one-piece (minuscules séparées par un tiret)";
				}
			}

			return msg;
		}
	}
}
