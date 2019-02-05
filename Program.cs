namespace DiscordBot
{
	public class Program
	{
		public static void Main(string[] args)
			=> new DiscordBot().MainAsync().GetAwaiter().GetResult();
	}
}
