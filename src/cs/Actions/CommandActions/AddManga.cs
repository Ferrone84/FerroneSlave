
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;
using Supremes;

namespace DiscordBot.Actions.CommandActions
{
	public class AddManga : ACommandAction
	{
		public AddManga() : base()
		{
			Name = Prefix + "addmanga";
			Description = "Ajoute le manga en paramètre à la liste. (!addmanga one-piece)";
		}

		public override async Task Invoke(IUserMessage message)
		{
			await message.Channel.SendMessageAsync(AddMangaJapscan(message));
		}

		private string AddMangaJapscan(IUserMessage message)
		{
			string msg = string.Empty;
			try {
				string manga = message.Content.ToLower().Split(' ')[1];

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
				else {
					msg = "La commande est mal écrite. Elle doit être de la forme : !addmanga one-piece";
				}
			}
			return msg;
		}

		private string AddMangaLireScan(IUserMessage message)
		{
			string msg = string.Empty;
			try {
				string manga = message.Content.ToLower().Split(' ')[1];

				if (DataManager.mangasData.ContainsKey(manga)) {
					return "Ce manga est déjà dans la liste poto ! :)";
				}

				Supremes.Nodes.Document document = null;
				string site = "https://www.lirescan.me/";
				string link = site + manga + "-lecture-en-ligne/";
				try {
					document = Dcsoup.Parse(new Uri(link), 15000);
				}
				catch (Exception) {
					return "Timeout on : <" + link + ">";
				}

				string documentString = document.ToString().RemoveChars(new List<char>() { '\n', ' ' }).Replace("<html><head></head><body></body></html>", "");
				if (documentString == string.Empty) {
					throw new ArgumentOutOfRangeException();
				}
				else {
					string result = "\n" + manga;
					System.IO.File.AppendAllText(DataManager.Text.MANGASDATA_FILE, result);
					DataManager.mangasData = Utils.InitMangasData();

					DataManager.database.addManga(manga);
					msg = "Manga '" + manga + "' ajouté à la liste.";
				}
			}
			catch (ArgumentOutOfRangeException) {
				msg = "Ce manga n'existe pas ou le site ne le possède pas. Le nom doit être de la forme : one-piece (minuscules séparées par un tiret)";
			}
			catch (Exception) {
				msg = "La commande est mal écrite. Elle doit être de la forme : !addmanga one-piece";
			}
			return msg;
		}
	}
}
