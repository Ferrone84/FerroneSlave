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
	class Utils
	{
		public static string PYTHON_EXE = @"C:\Users\utilisateur\AppData\Local\Programs\Python\Python37\python.exe";
		public static string PYTHON_DIR_PATH = @"resources/python/";
		public static string TOKEN_FILE_NAME = @"resources/token.txt";
		public static string MANGASDATA_FILE_NAME = @"resources/data.txt";
		public static string TRAJETS_FILE_NAME = @"resources/trajets.txt";
		public static string LOGS_FILE_NAME = @"resources/logs.txt";
		public static string PP_FILE_NAME = @"resources/pp.txt";

		public static void displayException(Exception e, string message = "Error")
		{
			Console.WriteLine(message + " : \n" + e.Message + "\n");
			Console.WriteLine(e.StackTrace);
		}


		/*
		 * runPython("filename").aff();
		 * runPython("filename", "a", "b").aff();
		 * runPython("filename", new string[] { "c", "d" }).aff();
		*/
		public static string runPython(string fileName, params string[] args)
		{
			string result = "";
			string arguments = "";
			string file = PYTHON_DIR_PATH + fileName;

			if (args.Length != 0)
			{
				arguments += " ";
				for (int i = 0; i < args.Length; i++) //un Select serait mieux, mais flemme
				{
					arguments += args[i];
					if (i != args.Length - 1)
						arguments += " ";
				}
			}

			// Create new process start info 
			ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(PYTHON_EXE);

			// make sure we can read the output from stdout 
			myProcessStartInfo.UseShellExecute = false;
			myProcessStartInfo.RedirectStandardOutput = true;
			myProcessStartInfo.Arguments = file + arguments;

			Process myProcess = new Process();
			// assign start information to the process 
			myProcess.StartInfo = myProcessStartInfo;

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

		public static string onlyKeepDigits(string str)
		{
			return new string((from c in str
							   where char.IsDigit(c)
							   select c
			).ToArray());
		}

		public static string onlyKeepLetters(string str)
		{
			return new string((from c in str
							   where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)
							   select c
			).ToArray());
		}

		public static string onlyKeepLetters(string str, List<char> chars)
		{
			return new string((from c in str
							   where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || chars.Contains(c)
							   select c
			).ToArray());
		}

		public static List<string> moreThanTwoThousandsChars(string str)
		{
			List<string> result = new List<string>();

			int start = 0;
			while (str.Length >= 2000)
			{
				result.Add(str.Substring(start, 1999));
				start += 1999;
				str = str.Remove(0, 1999);
			}
			result.Add(str);
			return result;
			/*if (str == String.Empty)
				return new List<string>();

			str.debug();
			var a = (List<string>) Split(str, 1999);
			foreach (var b in a)
				b.aff();
			return a;*/
		}

		public static IEnumerable<string> Split(string str, int chunkSize)
		{
			return Enumerable.Range(0, str.Length / chunkSize)
				.Select(i => str.Substring(i * chunkSize, chunkSize));
		}
	}

	static class Extensions
	{
		public static void aff(this string str)
		{
			Console.WriteLine(str);
		}
		public static void debug(this string str)
		{
			Console.WriteLine("/"+str+"/");
		}

		public static void aff(this int entier)
		{
			Console.WriteLine(entier);
		}

		public static void aff(this bool var)
		{
			Console.WriteLine(var);
		}
	}
}
