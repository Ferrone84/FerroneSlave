using System.Collections.Generic;

using Discord;
using Discord.WebSocket;
using System.Threading;

namespace DiscordBot.Data
{
	public class DataManager
	{
		public static string SOURCES_DIR = @"src/";
		public static string PYTHON_DIR = $@"{SOURCES_DIR}python/";
		public static string CSHARP_DIR = $@"{SOURCES_DIR}cs/";

		public static string RESOURCES_DIR = @"resources/";
		public static string BIN_DIR = $@"{RESOURCES_DIR}binaries/";
		public static string TEXT_DIR = $@"{RESOURCES_DIR}text/";

		public struct Python
		{
			public static string DISPLAY_FILE = $@"{PYTHON_DIR}display.py";
			public static string QUERY_FILE = $@"{PYTHON_DIR}query_executor.py";
			public static string INIT_TABLES_FILE = $@"{PYTHON_DIR}init_tables.py";

			//Must remain full path
			public static string PYTHON_EXE = @"C:\Users\utilisateur\AppData\Local\Programs\Python\Python37\python.exe";
		}

		public struct Binary
		{
			public static string DB_FILE = $@"{BIN_DIR}bdd.db";
			public static string DB_FILE_SAVE = $@"{BIN_DIR}bdd_save.db";
			public static string BANNED_FILE = $@"{BIN_DIR}banned.bin";
			public static string POP_ACTIONS_FILE = $@"{BIN_DIR}pop_actions.bin";
		}
		
		public struct Text
		{
			public static string PP_FILE = $@"{TEXT_DIR}pp.txt";
			public static string SAY_FILE = $@"{TEXT_DIR}say.txt";
			public static string TOKEN_FILE = $@"{TEXT_DIR}token.txt";
			public static string ISBOT_FILE = $@"{TEXT_DIR}is_test.txt";
			public static string TRAJETS_FILE = $@"{TEXT_DIR}trajets.txt";
			public static string POKEMONS_FILE = $@"{TEXT_DIR}pokemons.p";
			public static string ERRORSLOG_FILE = $@"{TEXT_DIR}errors.txt";
			public static string MANGASDATA_FILE = $@"{TEXT_DIR}mangas_list.txt";
			public static string MANGASDATA_RSS_FILE = $@"{TEXT_DIR}feed_data.txt";
		}

		public static DiscordSocketClient _client;
		public static CancellationTokenSource delay_controller;
		public static ulong master_id = 293780484822138881;
		
		public static Database database;
		public static SocketGuild guild;
		public static List<string> pp_songs;
		public static List<ulong> baned_people;
		public static Dictionary<ulong, int> people_spam;
		public static Dictionary<string, int> actions_used;
		public static SortedDictionary<string, string> mangasData;
		public static Dictionary<IUserMessage, IUserMessage> nsfw_content_inprocess;
		public static Dictionary<string, Actions.Action> actions;
		public static List<Actions.OtherActions.AOtherAction> otherActions;

		public enum PokemonInfo
		{
			uid,
			urlIcon,
			name,
			catchRate,
			rarityTier
		}
	}
}
