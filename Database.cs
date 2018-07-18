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
using System.Net;
using Newtonsoft.Json.Linq;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;


namespace DiscordBot
{
	class Database
	{
		public Database()
		{
			try
			{

			}
			catch (Exception e)
			{
				Utils.displayException(e, "Database()");
			}
		}

		public void init()
		{
			Utils.runPython("init_tables.py");
		}

		public string addUser(string uid, string pseudo, string prenom = "", short admin = 0)
		{
			string query = "INSERT INTO users (uid, pseudo, prenom, admin) VALUES (?,?,?,?)".Replace(' ', ':');
			string values = uid + ":" + pseudo + ":" + prenom + ":" + admin;

			return Utils.runPython("query_executor.py", query, values).Replace(':', '\n');
		}

		public string get(string table)
		{
			string query = ("SELECT * FROM "+table).Replace(' ', ':');

			return Utils.runPython("query_executor.py", query).Replace(':', '\n');
		}
	}
}
