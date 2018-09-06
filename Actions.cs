﻿using System;
using System.Collections.Generic;

using System.IO;
using Discord.WebSocket;


namespace DiscordBot
{
	public class Actions
	{
		private List<Tuple<string, string, Func<SocketMessage, string>>> actions;

		public Actions()
		{
			actions = new List<Tuple<string, string, Func<SocketMessage, string>>>();

			add("!ping", "Affiche le ping du bot.", ping);
			add("!date", "Affiche la date.", date);
			add("!flip", "Lance une table.", flip);
			add(Utils.unflip, "", flip);
			add("!unflip", "Replace une table.", unflip);
			add(Utils.flip, "", unflip);
			add("!clean", "Clean l'espace de message.", clean);
			add("!mangas", "Affiche la liste des mangas traités.", mangas);
			add("!scans", "Affiche le dernier scan pour chaque mangas traités.", scans);
			add("!lastchapter", "Affiche le dernier scan pour le manga en paramètre. (!lastchapter one-piece)", lastChapter);
			add("!addmanga", "Ajoute le manga en paramètre à la liste. (!addmanga one-piece)", addManga);
			add("!subto", "Permet de s'abonner au manga précisé. (!subto one-piece)", subTo);
			add("!unsubto", "Permet de se désabonner du manga précisé. (!unsubto one-piece)", unsubTo);
			add("!sublist", "Affiche la liste de tous les abonnements aux mangas.", subList);
			add("!help", "Affiche toutes les options.", help);

			add("!!display", "Affiche la bdd.", displayBdd);
			add("!!adduser", "Ajoute un utilisateur à la bdd.", addUser);
			add("!!savebdd", "Sauvegarde la bdd.", saveBdd);
			add("!!restbdd", "Restaure la bdd.", restBdd);
			add("!!delete", "Supprime le nombre de messages spécifiés en partant de la fin.", delete);

			add("$fs", "Formate la phrase qui suit avec de jolies lettres.", fs);
			add("$lenny", "Affiche le meme 'lenny'.", lenny);
			add("$popopo", "Affiche le meme 'popopo'.", popopo);
			add("$ken", "Tu ne le sais pas, mais tu es déjà mort.", ken);
			add("$amaury meme", "Tout le monde sait ce que c'est.", amauryMeme);
			add("$mytho ultime", "El mytho ultima.", mythoUltime);
			add("$los", "Trigger la dreamteam de LOS !!", los);
			add("$boulot", "", boulot);
			add("$pp", "Random PP Song", pp);
			add("$rsong", "Random Song", rsong);
			add("$fap", "Si t'aime te fap ;)", fap);

			add("bite", "Si ta phrase contient une bite ou plusieurs alors PEPE*biteNumber.", bite);
			add("musique de génie", "Le jour où tu veux écouter de la vrai musique.", musiqueGenie);
			add("gamabunta", "Meme naruto du BOSS.", gamabunta);
			add("invocation", "Meme naruto.", invocation);
			add("welcome", "Meme Resident Evil 4.", welcome);
			add("évidemment", "Meme Antoine Daniel.", evidemment);
			add("evidemment", "Meme Antoine Daniel.", evidemment);
			add("omae", "NANI !?", omae);
			add("sancho", "Le génie de Brook.", sancho);
			add("detroit", "Cqfd.", detroitSmash);
			add("smash", "Cqfd.", detroitSmash);
		}

		private void add(string command, string description, Func<SocketMessage, string> method)
		{
			this.actions.Add(new Tuple<string, string, Func<SocketMessage, string>>(command, description, method));
		}

		public bool actionExist(string action)
		{
			foreach (var act in actions)
			{
				if (act.Item1 == action)
					return true;
			}
			return false;
		}

		public List<Tuple<string, string, Func<SocketMessage, string>>> getActions
		{
			get { return actions; }
		}

		public static string getActionType(string action)
		{
			string type = String.Empty;

			if (action.StartsWith("!!")) { type = "Les Commandes admin"; }
			else if (action.StartsWith("!")) { type = "Les Commandes"; }
			else if (action.StartsWith("$")) { type = "Les Deletes"; }
			else { type = "Autres"; }

			return type;
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

		private string displayBdd(SocketMessage message)
		{
			return Program.database.display();
		}

		private string addUser(SocketMessage message)
		{
			string msg = "Il faut rentrer des arguments. Ex : !!adduser 293780484822138881 ferrone nico";

			if (message.Content.ToLower().Length <= "!!adduser".Length)
			{
				return msg;
			}
			try
			{
				var args = message.Content.Split(' ');
				Program.database.addUser(args[1], args[2], args[3]);
			}
			catch (Exception e)
			{
				Utils.displayException(e, "!!adduser");
				return msg;
			}

			return String.Empty;
		}

		private string saveBdd(SocketMessage message)
		{
			try
			{
				File.Copy(Utils.DB_FILE_NAME, Utils.DB_FILE_SAVE, true);
			}
			catch (Exception e)
			{
				Utils.displayException(e, "saveBdd");
				return e.Message;
			}

			return "La bdd a bien été save.";
		}

		private string restBdd(SocketMessage message)
		{
			try
			{
				File.Copy(Utils.DB_FILE_SAVE, Utils.DB_FILE_NAME, true);
			}
			catch (Exception e)
			{
				Utils.displayException(e, "restBdd");
				return e.Message;
			}

			return "La bdd a bien été restaurée.";
		}

		private string delete(SocketMessage message) //trouver un moyen de rendre ça async sans vomir
		{
			try
			{
				int numberOfDelete = Convert.ToInt32(message.Content.Split(' ')[1]) + 1;
				var messages = Utils.getMessages(Utils.getChannel(message.Channel.Id), numberOfDelete);

				foreach (var msg in messages)
				{
					msg.DeleteAsync();
				}
			}
			catch (Exception e)
			{
				Utils.displayException(e, "delete");
				return "La commande doit être du type !!delete 10";
			}

			return String.Empty;
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
					if (role.Id == 471428502621650947 && user.Id != message.Author.Id)
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

		private string rsong(SocketMessage message)
		{
			return Utils.randomSong();
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