
namespace DiscordBot
{
	public static class UserExtentions
	{
		public static bool IsAdmin(this Discord.IUser user)
		{
			return Data.DataManager.database.isAdmin(user.Id.ToString());
		}
	}
}
