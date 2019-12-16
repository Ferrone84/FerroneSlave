
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Debug : ACommandAction
	{
		public Debug() : base()
		{
			Name = Prefix + "d";
			Description = "Utilisé pour le debug.";
			Accessibility = AccessibilityType.Invisible;
		}

		public override async Task Invoke(IUserMessage message)
		{
			try {
				/*
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&key={YOUR_API_KEY}");
				httpWebRequest.ContentType = "text/json";
				httpWebRequest.Method = "POST";

				string msg = "LOUL";

				using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
					string json = "{ \"method\": \"send\", " +
									"	\"params\": [ " +
									"	 \"IPutAGuidHere\", " +
									"	 \"msg@MyCompany.com\", " +
									"	 \"MyTenDigitNumberWasHere\", " +
									"	 \"" + msg + "\" " +
									"	 ] " +
									"}";

					streamWriter.Write(json);
				}
				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) {
					string responseText = streamReader.ReadToEnd();
				}
				*/
			}
			catch (System.Exception e) {
				e.Display(Name);
				await message.Channel.SendMessagesAsync(e.Message + "\n" + e.StackTrace);
			}
		}
	}
}
