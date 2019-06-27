
using Discord;

namespace DiscordBot.Data
{
	public class Users
	{
		public static IUser Ferrone => DataManager._client.GetUser(293780484822138881);
		public static IUser Fluttershy => DataManager._client.GetUser(150338863234154496);
		public static IUser Sophie => DataManager._client.GetUser(536545704223703040);

		public static IUser Bot => DataManager._client.GetUser(342330092032098304) ?? TestBot;
		public static IUser TestBot => DataManager._client.GetUser(543559733035008010);
	}
}
