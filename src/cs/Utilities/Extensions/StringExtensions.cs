
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiscordBot
{
	public static class StringExtensions
	{
		private const int SplitLength = 1980;

		public static string[] Split(this string str, string splitter)
		{
			return str.Split(new[] { splitter }, System.StringSplitOptions.None);
		}

		public static string[] FlatSplit(this string str, int maxLength = SplitLength)
		{
			List<string> result = new List<string>();

			int start = 0;
			while (str.Length >= maxLength) {
				int end = maxLength;
				result.Add(str.Substring(start, end));
				str = str.Remove(0, end);
			}
			result.Add(str);

			return result.ToArray();
		}

		public static string[] SplitBodies(this string content, string delim = "\n", int maxLength = SplitLength)
		{
			if (content.Length < maxLength) { return new string[] { content }; };
			if (!content.Contains(delim)) { return content.FlatSplit(maxLength); };

			int start = 0;
			string buffer = content;
			List<string> result = new List<string>();

			while (buffer.Length >= maxLength) {
				string tmp = content.Substring(start, maxLength);
				int lastDelim = tmp.LastIndexOf(delim);
				if (lastDelim == -1) { return content.FlatSplit(maxLength); };

				result.Add(content.Substring(start, lastDelim));
				lastDelim += delim.Length;
				buffer = buffer.Remove(0, lastDelim);
				start += lastDelim;
			}
			result.Add(buffer); //last token

			return result.ToArray();
		}

		public static (string id, string url) GetYoutubeLink(this string message)
		{
			string pattern = @"(https?:\/\/)?(www\.|m\.)?youtu(be.com\/watch\/?\?v=|\.be\/)(?<id>[-_A-Za-z0-9]+)";
			Match match = new Regex(pattern).Match(message);
			return (match.Success ? (match.Groups["id"].Value, match.Value) : (string.Empty, string.Empty));
		}

		public static string Brackets(this string str)
		{
			return "[" + str + "]";
		}

		public static string OnlyKeepDigits(this string str)
		{
			return new string((from c in str
							   where char.IsDigit(c)
							   select c
			).ToArray());
		}

		public static string OnlyKeepLetters(this string str)
		{
			return new string((from c in str
							   where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)
							   select c
			).ToArray());
		}

		public static string OnlyKeepLetters(this string str, List<char> chars)
		{
			return new string((from c in str
							   where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || chars.Contains(c)
							   select c
			).ToArray());
		}

		public static string OnlyKeep(this string str, List<char> chars)
		{
			return new string((from c in str
							   where chars.Contains(c)
							   select c
			).ToArray());
		}

		public static string RemoveChars(this string str, List<char> chars)
		{
			return new string((from c in str
							   where !chars.Contains(c)
							   select c
			).ToArray());
		}
	}
}
