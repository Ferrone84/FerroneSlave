
using System;
using System.Collections.Generic;
using System.IO;

using DiscordBot.Data;

namespace DiscordBot
{
	public class Utils
	{
		public static void Init()
		{
			if (IsLinux) {
				DataManager.Python.PYTHON_EXE = @"/usr/bin/python3";
			}
			if (IsTestBot) {
				//content for the testBot
			}
		}

		public static bool IsLinux {
			get {
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}

		public static bool IsTestBot => File.ReadAllText(DataManager.Text.ISBOT_FILE) == "1";

		public static string Token => File.ReadAllLines(DataManager.Text.TOKEN_FILE)[0];

		public static string GoogleMapsApiKey => File.ReadAllLines(DataManager.Text.TOKEN_FILE)[1];

		//----- Setups -----
		public static SortedDictionary<string, string> InitMangasData()
		{
			SortedDictionary<string, string> result = new SortedDictionary<string, string>();
			try {
				string[] lines = File.ReadAllLines(DataManager.Text.MANGASDATA_FILE);
				foreach (string manga in lines) {
					result.Add(manga, "");
				}
				/*foreach (string line in lines) {
					var data = line.Split("|");
					var manga = data[0];
					var content = data[1];

					result.Add(manga, content);
				}*/
			}
			catch (Exception e) {
				e.DisplayException(System.Reflection.MethodBase.GetCurrentMethod().ToString());
			}
			return result;
		}

		public static List<string> InitPpSong()
		{
			List<string> result = new List<string>();
			string[] lines = File.ReadAllLines(DataManager.Text.PP_FILE);
			foreach (string line in lines) {
				result.Add(line);
			}
			return result;
		}
	}
}
