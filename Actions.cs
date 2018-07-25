using System;
using System.Collections.Generic;

using Discord.WebSocket;


namespace DiscordBot
{
	public class Actions
	{
		private List<Tuple<string, Func<SocketMessage, string>>> actions;

		public Actions()
		{
			actions = new List<Tuple<string, Func<SocketMessage, string>>>();

			add("!ping", ping);
			add("!date", date);
			add("!flip", flip);
			add(Utils.unflip, flip);
			add("!unflip", unflip);
			add(Utils.flip, unflip);
			add("!clean", clean);
			add("!mangas", mangas);
			add("!scans", scans);
			add("!lastchapter", lastChapter);
			add("!addmanga", addManga);
			add("!subto", subTo);
			add("!unsubto", unsubTo);
			add("!sublist", subList);
			add("!help", help);

			add("!!adduser", addUser);
			add("!!d", displayBdd);

			add("$fs", fs);
			add("$lenny", lenny);
			add("$popopo", popopo);
			add("$ken", ken);
			add("$amaury meme", amauryMeme);
			add("$mytho ultime", mythoUltime);
			add("$los", los);
			add("$boulot", boulot);
			add("$pp", pp);
			add("$fap", fap);

			add("bite", bite);
			add("musique de génie", musiqueGenie);
			add("gamabunta", gamabunta);
			add("invocation", invocation);
			add("welcome", welcome);
			add("évidemment", evidemment);
			add("evidemment", evidemment);
			add("omae", omae);
			add("sancho", sancho);
			add("detroit", detroitSmash);
			add("smash", detroitSmash);
		}

		private void add(string command, Func<SocketMessage, string> method)
		{
			this.actions.Add(new Tuple<string, Func<SocketMessage, string>>(command, method));
		}

		public List<Tuple<string, Func<SocketMessage, string>>> getActions
		{
			get { return actions; }
		}



		///////////////////////////////////////////////////////////////////
		//							Les commandes
		///////////////////////////////////////////////////////////////////

		private string ping(SocketMessage message)
		{
			return "Pong! Mon ping est de : " + Program._client.Latency.ToString() + "ms.";
		}

		private string date(SocketMessage message)
		{
			string _t = String.Empty;
			string message_lower = message.Content.ToLower();

			if (message_lower.Contains("day"))
			{
				_t = DateTime.Now.Day.ToString();
			}
			else if (message_lower.Contains("month"))
			{
				_t = DateTime.Now.Month.ToString();
			}
			else if (message_lower.Contains("year"))
			{
				_t = DateTime.Now.Year.ToString();
			}
			else if (message_lower.Contains("time"))
			{
				_t = DateTime.Now.TimeOfDay.ToString();
				_t = _t.Remove(8, _t.Length - 8);
			}
			else
				_t = DateTime.Now.ToString();
			return _t;
		}

		private string flip(SocketMessage message)
		{
			return Utils.flip;
		}

		private string unflip(SocketMessage message)
		{
			return Utils.unflip;
		}

		private string clean(SocketMessage message)
		{
			string msg = "Clean en cours...";
			for (int i = 0; i < 60; i++)
				msg += "\n";
			msg += "\nClean terminé.";
			return msg;
		}

		private string mangas(SocketMessage message)
		{
			return Utils.displayMangasList();
		}

		private string scans(SocketMessage message)
		{
			try
			{
				return Utils.displayCompleteMangasList();
			}
			catch (Exception e)
			{
				Utils.displayException(e, "!scans");
				return e.Message;
			}
		}

		private string lastChapter(SocketMessage message)
		{
			return Utils.lastChapter(message.Content.ToLower());
		}

		private string addManga(SocketMessage message)
		{
			return Utils.addManga(message.Content.ToLower());
		}

		private string subTo(SocketMessage message)
		{
			string msg = String.Empty;
			string message_lower = message.Content.ToLower();
			if (message_lower.Length > 6)
				msg = Program.database.subTo(message.Author.Id.ToString(), message_lower.Split(' ')[1]);
			else
				msg = "Il faut mettre un titre de manga. Ex : !subto one-piece";

			return msg;
		}

		private string unsubTo(SocketMessage message)
		{
			string msg = String.Empty;
			string message_lower = message.Content.ToLower();
			if (message_lower.Length > 8)
				msg = Program.database.unsubTo(message.Author.Id.ToString(), message_lower.Split(' ')[1]);
			else
				msg = "Il faut mettre un titre de manga. Ex : !unsubto one-piece";

			return msg;
		}

		private string subList(SocketMessage message)
		{
			string msg = String.Empty;
			string message_lower = message.Content.ToLower();
			if (message_lower.Length > 8)
				msg = Program.database.subList(message.Author.Id.ToString(), message_lower.Split(' ')[1]);
			else
				msg = Program.database.subList(message.Author.Id.ToString());

			return msg;
		}

		private string help(SocketMessage message)
		{
			return Utils.displayAllActions();
		}
		

		///////////////////////////////////////////////////////////////////
		//							  Partie Admin
		///////////////////////////////////////////////////////////////////

		private string addUser(SocketMessage message)
		{
			string msg = String.Empty;
			if (message.Content.ToLower().Length <= "!!adduser".Length)
			{
				return "Il faut rentrer des arguments. Ex : !!adduser 293780484822138881 ferrone nico";
			}
			try
			{
				var args = message.Content.Split(' ');
				Program.database.addUser(args[1], args[2], args[3]);
			}
			catch (Exception e)
			{
				Utils.displayException(e, "!!adduser");
				return "Il faut rentrer des arguments. Ex : !!adduser 293780484822138881 ferrone nico";
			}

			return String.Empty;
		}

		private string displayBdd(SocketMessage message)
		{
			return Program.database.display();
		}


		///////////////////////////////////////////////////////////////////
		//							Les deletes
		///////////////////////////////////////////////////////////////////

		private string fs(SocketMessage message)
		{
			string msg = message.Content.ToLower().Substring(3).ToLower();

			if (msg == String.Empty)
				return "$fs vide !";

			if (message.Author.Id == 150338863234154496)
				msg = Utils.FormateSentence(msg) + " by FlutterShy / Blossom / Pupute";
			else
				msg = Utils.FormateSentence(msg) + " by " + message.Author.Username;

			return msg;
		}

		private string lenny(SocketMessage message)
		{
			return "( ͡° ͜ʖ ͡°)";
		}

		private string popopo(SocketMessage message)
		{
			return "https://cdn.discordapp.com/attachments/346760327540506643/353847873458274304/popopo.png";
		}

		private string ken(SocketMessage message)
		{
			return "OMAE WA MOU ... SHINDEIRU !";
		}

		private string amauryMeme(SocketMessage message)
		{
			return "https://cdn.discordapp.com/attachments/309407896070782976/353833262273134592/Sans_titre.png";
		}

		private string mythoUltime(SocketMessage message)
		{
			return "https://cdn.discordapp.com/attachments/346760327540506643/402253939573129217/amaury_ultime.jpg";
		}

		private string los(SocketMessage message)
		{			
			if (message.Author.Id == 150338863234154496)
				return "Luc qui casse les couilles à vouloir trigger le LOS !";

			string msg = "LOS ? ";
			string message_lower = message.Content.ToLower();

			foreach (var user in Program.guild.Users)
			{
				foreach (var role in user.Roles)
				{
					if (role.Id == 471428502621650947)
					{
						msg += "<@" + user.Id + "> ";
					}
				}
			}
			msg += " - " + message.Author.Username + " veut jouer !";

			if (message_lower != "$los")
			{
				msg += " [" + message_lower.Substring(5) + "]";
			}

			return msg;
		}

		private string boulot(SocketMessage message)
		{
			return Utils.traffic();
		}

		private string pp(SocketMessage message)
		{
			return Utils.randomPpSong();
		}

		private string fap(SocketMessage message)
		{
			return "https://giphy.com/gifs/fap-Bk2NzCbwFH6sE";
		}


		///////////////////////////////////////////////////////////////////
		//							  Autres
		///////////////////////////////////////////////////////////////////

		private string bite(SocketMessage message)
		{
			int it = Utils.CountIterations(message.ToString().ToLower(), "bite");
			string pepe = "<:pepe:329281047730585601> ";
			string msg = String.Empty;
			for (int i = 0; i < it; i++)
				msg += pepe;

			return msg;
		}

		private string musiqueGenie(SocketMessage message)
		{
			return "https://www.youtube.com/watch?v=kXYiU_JCYtU&index=146&list=PLi7ipd_Aw87Xv1s_L8p1NZ6cg9Ny0dQEi&ab_channel=LinkinPark";
		}

		private string gamabunta(SocketMessage message)
		{
			return "Kuchiyose no jutsu ! https://vignette3.wikia.nocookie.net/naruto/images/8/84/Gamabunta.png/revision/latest?cb=20160623114719";
		}

		private string invocation(SocketMessage message)
		{
			return "Kuchiyose no jutsu !";
		}

		private string welcome(SocketMessage message)
		{
			return "https://www.youtube.com/watch?v=o0kGvgXmmgk&ab_channel=DiegoSousa";
		}

		private string evidemment(SocketMessage message)
		{
			return "https://www.youtube.com/watch?v=A4-MlnvXo2I&ab_channel=FansDesRoisD%27Internet";
		}

		private string omae(SocketMessage message)
		{
			return "NANIIII !!??";
		}

		private string sancho(SocketMessage message)
		{
			return "https://cdn.discordapp.com/attachments/346760327540506643/397856433145905156/yahazu_giri.png";
		}

		private string detroitSmash(SocketMessage message)
		{
			return "https://giphy.com/gifs/hero-smash-boku-XE8j547LpglrO";
		}
	}
}
