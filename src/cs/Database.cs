using System;
using System.Collections.Generic;
using System.Linq;

using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
	public class Database
	{
		public Database()
		{

		}

		public void init()
		{
			Utils.runPython(Data.Python.INIT_TABLES_FILE);
		}
		
		public void loadMangas()
		{
			foreach (KeyValuePair<string, string> kvp in Data.mangasData) {
				addManga(kvp.Key);
			}
		}

		public void loadUsers()
		{
			var users = Data.guild.Users;

			foreach (var user in users) {
				if (!user.IsBot) {
					short admin = 0;

					foreach (var role in user.Roles) {
						if (role.Id == 328899154887835678) {
							admin = 1;
							break;
						}
					}

					string username = user.Username.Replace(' ', '-');
					if (user.Id == 150338863234154496) {
						username = "Fluttershy";
					}

					try {
						addUser(user.Id.ToString(), username, "a", admin);
					}
					catch (Exception e) {
						Utils.displayException(e, "Database adduser");
					}
				}
			}
		}

		public string addUser(string uid, string pseudo, string prenom = "", short admin = 0)
		{
			string query = "INSERT INTO users (uid, pseudo, prenom, admin) VALUES (?,?,?,?)".Replace(' ', ':');
			string values = uid + ":" + pseudo + ":" + prenom + ":" + admin;

			return Utils.runPython(Data.Python.QUERY_FILE, query, values).Replace(':', '\n');
		}

		public string addManga(string manga, string scan = " ")
		{
			string query = "INSERT INTO mangas (titre, scan) VALUES (?,?)".Replace(' ', ':');
			string values = manga + ":" + scan;

			return Utils.runPython(Data.Python.QUERY_FILE, query, values).Replace(':', '\n');
		}

		public string addSub(string userId, string mangaId)
		{
			string query = "INSERT INTO subs (user, manga) VALUES (?,?)".Replace(' ', ':');
			string values = userId + ":" + mangaId;

			return Utils.runPython(Data.Python.QUERY_FILE, query, values).Replace(':', '\n');
		}

		public string addMusic(string title)
		{
			string query = "INSERT INTO musics (title) VALUES (?)".Replace(' ', ':');

			return Utils.runPython(Data.Python.QUERY_FILE, query, title.Replace(':', '/')).Replace(':', '\n');
		}

		public string removeMusic(string title)
		{
			string query = "DELETE FROM musics WHERE title=?".Replace(' ', ':');

			return Utils.runPython(Data.Python.QUERY_FILE, query, title.Replace(':', '/')).Replace(':', '\n');
		}

		public string addPokemon(int uid, string urlIcon, string name, int catchRate, int rarityTier)
		{
			string query = "INSERT INTO pokemons (uid, urlIcon, name, catchRate, rarityTier) VALUES (?,?,?,?,?)".Replace(' ', ':');
			string values = uid + ":" + urlIcon.Replace(':', '/') + ":" + name + ":" + catchRate + ":" + rarityTier;

			return Utils.runPython(Data.Python.QUERY_FILE, query, values).Replace(':', '\n');
		}

		public string subTo(string uid, string manga)
		{
			string mangaId = String.Empty;
			try {
				mangaId = makeQuery("SELECT id FROM mangas WHERE titre=?", manga);
				if (mangaId.Equals(String.Empty))
					return "Le manga '" + manga + "' n'existe pas :/";
			}
			catch (Exception e) {
				Utils.displayException(e, "subTo");
				return "Le manga '" + manga + "' n'existe pas :/";
			}

			string userId = makeQuery("SELECT id FROM users WHERE uid=?", uid);
			if (userId.Equals(String.Empty))
				return "L'utilisateur n'est pas dans la base de données :/";

			mangaId = Utils.onlyKeepDigits(mangaId);
			userId = Utils.onlyKeepDigits(userId);
			string alreadySub = makeQuery("SELECT id FROM subs WHERE user=? and manga=?", userId + ":" + mangaId);
			if (!alreadySub.Equals(String.Empty))
				return "Tu es déjà abonné à ce manga ! :)";

			addSub(userId, mangaId);

			return "Vous vous êtes bien abonné au manga '" + manga + "'.";
		}

		public string unsubTo(string uid, string manga)
		{
			string mangaId = String.Empty;
			try {
				mangaId = makeQuery("SELECT id FROM mangas WHERE titre=?", manga);
				if (mangaId.Equals(String.Empty))
					return "Le manga '" + manga + "' n'existe pas :/";
			}
			catch (Exception e) {
				Utils.displayException(e, "unsubTo");
				return "Le manga '" + manga + "' n'existe pas :/";
			}

			string userId = makeQuery("SELECT id FROM users WHERE uid=?", uid);
			if (userId.Equals(String.Empty))
				return "L'utilisateur n'est pas dans la base de données :/";

			mangaId = Utils.onlyKeepDigits(mangaId);
			userId = Utils.onlyKeepDigits(userId);
			string alreadySub = makeQuery("SELECT id FROM subs WHERE user=? and manga=?", userId + ":" + mangaId);
			if (alreadySub.Equals(String.Empty))
				return "Tu n'es pas abonné à ce manga ! :)";

			makeQuery("DELETE FROM subs WHERE user=? and manga=?", userId + ":" + mangaId);

			return "Vous vous êtes bien désabonné du manga '" + manga + "'.";
		}

		public string subList(string uid, string user = "")
		{
			string result = String.Empty;
			string userId = makeQuery("SELECT id FROM users WHERE uid=?", uid);
			if (userId.Equals(String.Empty))
				return "L'utilisateur n'est pas dans la base de données :/";

			if (user.Equals(String.Empty)) {
				List<string> users = new List<string>();
				string usersFlat = makeQuery("SELECT DISTINCT user FROM subs ORDER BY user");

				if (usersFlat.Contains('\n'))
					users = usersFlat.Split('\n').Select(elem => Utils.onlyKeepDigits(elem)).ToList();
				else
					users.Add(Utils.onlyKeepDigits(usersFlat));

				foreach (string usrId in users) {
					string usr = Utils.onlyKeepLetters(makeQuery("SELECT pseudo FROM users WHERE id=?", usrId));
					result += "Abonnements de **" + usr + "** : \n";
					var mangas = Utils.onlyKeepLetters(
						makeQuery("SELECT titre FROM subs JOIN mangas ON(subs.manga=mangas.id) JOIN users ON(subs.user=users.id) WHERE subs.user=?", usrId),
						new List<char>() { '-', '\n' }
						).Split('\n');
					foreach (var manga in mangas)
						result += "\t - " + manga + "\n";
				}

				return result;
			}
			else {
				return "pas encore implémenté";
			}
		}

		public bool isAdmin(string uid)
		{
			string userId = makeQuery("SELECT id FROM users WHERE uid=?", uid);
			if (userId.Equals(String.Empty))
				return false;

			userId = Utils.onlyKeepDigits(userId);
			string result = makeQuery("SELECT admin FROM users WHERE id=?", userId);
			result = Utils.onlyKeepDigits(result);
			if (result == "1")
				return true;

			return false;
		}

		public string display()
		{
			return Utils.splitBodies(Utils.runPython(Data.Python.DISPLAY_FILE).Replace(':', '\n'));
		}

		public List<ulong> getSubs(string manga)
		{
			string mangaId = makeQuery("SELECT id FROM mangas WHERE titre=?", manga);
			if (mangaId.Equals(String.Empty))
				throw new Exception("getSub(string manga) : Le manga '" + manga + "' n'existe pas :/");

			List<ulong> users = new List<ulong>();
			mangaId = Utils.onlyKeepDigits(mangaId);

			string result = makeQuery("SELECT uid FROM subs JOIN users ON(subs.user=users.id) WHERE subs.manga=?", mangaId);
			if (result == String.Empty)
				return users;

			foreach (string uid in result.Split('\n')) {
				users.Add(Convert.ToUInt64(Utils.onlyKeepDigits(uid)));
			}

			return users;
		}

		public string getPokemonInfo(string info, Data.PokemonInfo pokemonInfoWhere, Data.PokemonInfo pokemonInfoReturn)
		{
			string select = String.Empty; //return
			string where = String.Empty; //where

			switch (pokemonInfoReturn) {
				case Data.PokemonInfo.id:
					select = "uid";
					break;
				case Data.PokemonInfo.urlIcon:
					select = "urlIcon";
					break;
				case Data.PokemonInfo.name:
					select = "name";
					break;
				case Data.PokemonInfo.catchRate:
					select = "catchRate";
					break;
				case Data.PokemonInfo.rarityTier:
					select = "rarityTier";
					break;
			}

			switch (pokemonInfoWhere) {
				case Data.PokemonInfo.id:
					where = "uid";
					break;
				case Data.PokemonInfo.urlIcon:
					where = "urlIcon";
					break;
				case Data.PokemonInfo.name:
					where = "name";
					break;
				case Data.PokemonInfo.catchRate:
					where = "catchRate";
					break;
				case Data.PokemonInfo.rarityTier:
					where = "rarityTier";
					break;
			}

			string result = makeQuery("SELECT " + select + " FROM pokemons WHERE " + where + "=?", info);
			if (result == String.Empty) {
				return "L'info '" + info + "' n'existe pas.";
			}

			switch (pokemonInfoReturn) {
				case Data.PokemonInfo.id:
					result = Utils.onlyKeepDigits(result);
					break;
				case Data.PokemonInfo.urlIcon:
					result = Utils.removeChars(result, new List<char>() { '(', '\'', ')', ',' });
					result = result.Replace("///", "://");
					break;
				case Data.PokemonInfo.name:
					result = Utils.onlyKeepLetters(result);
					break;
				case Data.PokemonInfo.catchRate:
					result = Utils.onlyKeepDigits(result);
					break;
				case Data.PokemonInfo.rarityTier:
					result = Utils.onlyKeepDigits(result);
					break;
			}

			return result;
		}

		public string getPokemonInfos(string pokemonName)
		{
			string result = makeQuery("SELECT * FROM pokemons WHERE name=?", pokemonName);
			if (result == String.Empty) {
				result = makeQuery("SELECT * FROM pokemons WHERE name_fr=?", pokemonName);
			}
			return result;
		}


		public string getLine(string table, string id)
		{
			try {
				return makeQuery("SELECT * FROM " + table + " WHERE id=?", id);
			}
			catch (Exception e) {
				Utils.displayException(e, "getLine");
				return "";
			}
		}

		public string getLineColumn(string table, string column, string id)
		{
			try {
				return makeQuery("SELECT " + column + " FROM " + table + " WHERE id=?", id);
			}
			catch (Exception e) {
				Utils.displayException(e, "getLine");
				return "";
			}
		}

		public string getMaxId(string table)
		{
			return Utils.onlyKeepDigits(makeQuery("SELECT id FROM " + table + " ORDER BY id DESC LIMIT 1"));
		}

		public string get(string table)
		{
			string query = ("SELECT * FROM " + table).Replace(' ', ':');

			return "Table [" + table + "] : \n" + Utils.runPython(Data.Python.QUERY_FILE, query).Replace(':', '\n');
		}

		private string makeQuery(string query, string values = "")
		{
			return Utils.runPython(Data.Python.QUERY_FILE, query.Replace(' ', ':'), values).Replace(':', '\n');
		}
	}
}
