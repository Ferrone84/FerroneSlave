using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Supremes;
using CloudflareSolverRe;
using System.Net;

namespace Crawler
{
	class Crawler
	{
		private string url;

		public Crawler(string url)
		{
			Url = url;
		}

		public string Url { get => url; set => url = value; }

		public string Crawl(string selecter = "", int timeout = 15000)
		{
			string result = string.Empty;
			Supremes.Nodes.Document document = null;
			try {
				document = Dcsoup.Parse(new Uri(url), timeout);

				if (selecter != string.Empty) {
					result = document.Select(selecter).ToString();
				}
				else {
					result = document.ToString();
				}
			}
			catch (Exception) {
				throw new TimeoutException("Timeout on : <" + url + ">");
			}

			return result;
		}

		public string CrawlCloudFlare()
		{
			return GetClient(url).DownloadString(url);
		}

		public WebClient GetClient(string url)
		{
			try {
				var target = new Uri(url);

				var cf = new CloudflareSolver {
					MaxTries = 3,
					ClearanceDelay = 3000
				};

				var result = cf.Solve(target).Result;

				if (!result.Success) {
					Console.WriteLine($"[Failed] Details: {result.FailReason}");
					throw new Exception("Failed but reloaded.");
				}

				var client = new WebClient();
				client.Headers.Add(HttpRequestHeader.Cookie, result.Cookies.AsHeaderString());
				client.Headers.Add(HttpRequestHeader.UserAgent, result.UserAgent);

				return client;
			}
			catch (Exception) {
				return GetClient(url);
			}
		}
	}
}
