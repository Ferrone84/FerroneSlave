
using System;

namespace DiscordBot
{
	public static class ExceptionExtentions
	{
		public static void Display(this Exception e, string message = "Error")
		{
			("\n" + message + " : (" + e.GetType().Name + ") \n" + e.Message + "\n").Println();
			("[StackTrace]\n" + e.StackTrace + "\n").Println();
		}
	}
}
