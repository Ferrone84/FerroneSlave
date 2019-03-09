using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace DiscordBot.Data
{
	public class Database
	{
		public Database()
		{

		}

		public void init()
		{
			RunPython(DataManager.Python.INIT_TABLES_FILE);
		}
		
		public void loadMangas()
		{
			foreach (KeyValuePair<string, string> kvp in DataManager.mangasData) {
				addManga(kvp.Key);
			}
		}

		public void loadUsers()
		{
			var users = DataManager.guild.Users;

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
					if (user.Id == Users.Fluttershy.Id) {
						username = "Fluttershy";
					}

					try {
						addUser(user.Id.ToString(), username, "a", admin);
					}
					catch (Exception e) {
						e.DisplayException("Database adduser");
					}
				}
			}
		}

		public string addUser(string uid, string pseudo, string prenom = "", short admin = 0)
		{
			string query = "INSERT INTO users (uid, pseudo, prenom, admin) VALUES (?,?,?,?)".Replace(' ', ':');
			string values = uid + ":" + pseudo + ":" + prenom + ":" + admin;

			return RunPython(DataManager.Python.QUERY_FILE, query, values).Replace(':', '\n');
		}

		public string addManga(string manga, string scan = " ")
		{
			string query = "INSERT INTO mangas (titre, scan) VALUES (?,?)".Replace(' ', ':');
			string values = manga + ":" + scan;

			return RunPython(DataManager.Python.QUERY_FILE, query, values).Replace(':', '\n');
		}

		public string addSub(string userId, string mangaId)
		{
			string query = "INSERT INTO subs (user, manga) VALUES (?,?)".Replace(' ', ':');
			string values = userId + ":" + mangaId;

			return RunPython(DataManager.Python.QUERY_FILE, query, values).Replace(':', '\n');
		}

		public string addMusic(string title)
		{
			string query = "INSERT INTO musics (title) VALUES (?)".Replace(' ', ':');

			return RunPython(DataManager.Python.QUERY_FILE, query, title.Replace(':', '/')).Replace(':', '\n');
		}

		public string removeMusic(string title)
		{
			string query = "DELETE FROM musics WHERE title=?".Replace(' ', ':');

			return RunPython(DataManager.Python.QUERY_FILE, query, title.Replace(':', '/')).Replace(':', '\n');
		}

		public string addPokemon(int uid, string urlIcon, string name, int catchRate, int rarityTier)
		{
			string query = "INSERT INTO pokemons (uid, urlIcon, name, catchRate, rarityTier) VALUES (?,?,?,?,?)".Replace(' ', ':');
			string values = uid + ":" + urlIcon.Replace(':', '/') + ":" + name + ":" + catchRate + ":" + rarityTier;

			return RunPython(DataManager.Python.QUERY_FILE, query, values).Replace(':', '\n');
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
				e.DisplayException(System.Reflection.MethodBase.GetCurrentMethod().ToString());
				return "Le manga '" + manga + "' n'existe pas :/";
			}

			string userId = makeQuery("SELECT id FROM users WHERE uid=?", uid);
			if (userId.Equals(String.Empty))
				return "L'utilisateur n'est pas dans la base de données :/";

			mangaId = mangaId.OnlyKeepDigits();
			userId = userId.OnlyKeepDigits();
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
				e.DisplayException(System.Reflection.MethodBase.GetCurrentMethod().ToString());
				return "Le manga '" + manga + "' n'existe pas :/";
			}

			string userId = makeQuery("SELECT id FROM users WHERE uid=?", uid);
			if (userId.Equals(String.Empty))
				return "L'utilisateur n'est pas dans la base de données :/";

			mangaId = mangaId.OnlyKeepDigits();
			userId = userId.OnlyKeepDigits();
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
					users = usersFlat.Split('\n').Select(elem => elem.OnlyKeepDigits()).ToList();
				else
					users.Add(usersFlat.OnlyKeepDigits());

				foreach (string usrId in users) {
					string usr = makeQuery("SELECT pseudo FROM users WHERE id=?", usrId).OnlyKeepLetters();
					result += "Abonnements de **" + usr + "** : \n";

					string[] mangas = 
						makeQuery("SELECT titre FROM subs JOIN mangas ON(subs.manga=mangas.id) JOIN users ON(subs.user=users.id) WHERE subs.user=?", usrId)
						.OnlyKeepLetters( new List<char>() { '-', '\n' })
						.Split('\n');

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

			userId = userId.OnlyKeepDigits();
			string result = makeQuery("SELECT admin FROM users WHERE id=?", userId);
			result = result.OnlyKeepDigits();
			if (result == "1")
				return true;

			return false;
		}

		public List<ulong> getSubs(string manga)
		{
			string mangaId = makeQuery("SELECT id FROM mangas WHERE titre=?", manga);
			if (mangaId.Equals(String.Empty))
				throw new Exception("getSub(string manga) : Le manga '" + manga + "' n'existe pas :/");

			List<ulong> users = new List<ulong>();
			mangaId = mangaId.OnlyKeepDigits();

			string result = makeQuery("SELECT uid FROM subs JOIN users ON(subs.user=users.id) WHERE subs.manga=?", mangaId);
			if (result == String.Empty)
				return users;

			foreach (string uid in result.Split('\n')) {
				users.Add(Convert.ToUInt64(uid.OnlyKeepDigits()));
			}

			return users;
		}

		public string getPokemonInfo(string info, DataManager.PokemonInfo pokemonInfoWhere, DataManager.PokemonInfo pokemonInfoReturn)
		{
			string select = pokemonInfoReturn.ToString(); //return
			string where = pokemonInfoWhere.ToString(); //where

			string result = makeQuery("SELECT " + select + " FROM pokemons WHERE " + where + "=?", info);
			if (result == String.Empty) {
				return "L'info '" + info + "' n'existe pas.";
			}

			switch (pokemonInfoReturn) {
				case DataManager.PokemonInfo.uid:
					result = result.OnlyKeepDigits();
					break;
				case DataManager.PokemonInfo.urlIcon:
					result = result.RemoveChars(new List<char>() { '(', '\'', ')', ',' });
					result = result.Replace("///", "://");
					break;
				case DataManager.PokemonInfo.name:
					result = result.OnlyKeepLetters();
					break;
				case DataManager.PokemonInfo.catchRate:
					result = result.OnlyKeepDigits();
					break;
				case DataManager.PokemonInfo.rarityTier:
					result = result.OnlyKeepDigits();
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

		public string GetDisplayedTables()
		{
			return RunPython(Data.DataManager.Python.DISPLAY_FILE).Replace(':', '\n');
		}


		public string getLine(string table, string id)
		{
			try {
				return makeQuery("SELECT * FROM " + table + " WHERE id=?", id);
			}
			catch (Exception e) {
				e.DisplayException(System.Reflection.MethodBase.GetCurrentMethod().ToString());
				return "";
			}
		}

		public string getLineColumn(string table, string column, string id)
		{
			try {
				return makeQuery("SELECT " + column + " FROM " + table + " WHERE id=?", id);
			}
			catch (Exception e) {
				e.DisplayException(System.Reflection.MethodBase.GetCurrentMethod().ToString());
				return "";
			}
		}

		public string getMaxId(string table)
		{
			return makeQuery("SELECT id FROM " + table + " ORDER BY id DESC LIMIT 1").OnlyKeepDigits();
		}

		public string get(string table)
		{
			string query = ("SELECT * FROM " + table).Replace(' ', ':');

			return "Table [" + table + "] : \n" + RunPython(DataManager.Python.QUERY_FILE, query).Replace(':', '\n');
		}

		private string makeQuery(string query, string values = "")
		{
			return RunPython(DataManager.Python.QUERY_FILE, query.Replace(' ', ':'), values).Replace(':', '\n');
		}


		/// <summary>
		/// Allow to start python process.
		/// Use :
		/// RunPython("filename").aff();
		/// RunPython("filename", "a", "b").aff();
		/// RunPython("filename", new string[] { "c", "d" }).aff();
		/// </summary>
		/// <param name="fileName">Name of the python file.</param>
		/// <param name="args">Arguments for the process.</param>
		/// <returns></returns>
		private string RunPython(string fileName, params string[] args)
		{
			string result = String.Empty;
			string arguments = String.Empty;

			if (args.Length != 0) {
				arguments += " ";
				for (int i = 0; i < args.Length; i++) {
					arguments += args[i];
					if (i != args.Length - 1)
						arguments += " ";
				}
			}

			// Create new process start info 
			ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(DataManager.Python.PYTHON_EXE) {
				// make sure we can read the output from stdout 
				UseShellExecute = false,
				RedirectStandardOutput = true,
				Arguments = fileName + arguments
			};

			Process myProcess = new Process {
				// assign start information to the process 
				StartInfo = myProcessStartInfo
			};

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
	}
}
