using System.Text;

namespace DiscordBot.Utilities
{
	public class RegexUtils
	{
		public static string MatchAllWordsDisordered(params string[] words)
		{
			if (words.Length == 0) { return string.Empty; }
			
			StringBuilder result = new StringBuilder("^");
			foreach (string word in words) {
				if (word.Length == 0) { continue; }

				result.Append("(?=.*");
				foreach (char letter in word) {
					result.Append(letter+"+");
				}
				result.Append(")");
			}
			result.Append(".*$");

			return @result.ToString();
		}
	}
}
