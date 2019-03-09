
namespace DiscordBot
{
	public static class StringExtension
	{
		public static string[] Split(this string str, string splitter)
		{
			return str.Split(new[] { splitter }, System.StringSplitOptions.None);
		}
	}
}