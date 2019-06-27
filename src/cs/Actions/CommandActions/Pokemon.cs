
using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Actions.CommandActions
{
	public class Pokemon : ACommandAction
	{
		public Pokemon() : base()
		{
			Name = Prefix + "pokemon";
			Description = "Permet d'afficher les infos d'un pokemon.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			var words = message.Content.ToLower().Split(' ');

			if (words.Length != 2) {
				await message.Channel.SendMessageAsync("This command can be used like this : " + Name + " Charizard");
			}
			else {
				var embed = GetAllPokemonInfo(words[1]);

				if (embed != null) {
					await message.Channel.SendMessageAsync("", false, embed);
				}
				else {
					await message.Channel.SendMessageAsync("Le pokemon '" + words[1] + "' n'existe pas.");
				}
			}
		}

		private Embed GetAllPokemonInfo(string pokemonName)
		{
			string pokemonInfos = Data.DataManager.database.getPokemonInfos(pokemonName);
			if (pokemonInfos == string.Empty) {
				return null;
			}

			pokemonInfos = pokemonInfos.Replace("(", "")
				.Replace(")", "")
				.Replace("'", "")
				.Replace("///", "://");
			string[] infos = pokemonInfos.Split(", ");

			int id = int.Parse(infos[1]);
			string urlIcon = infos[2];
			//https://img.pokemondb.net/sprites/black-white/anim/normal/unown-a.gif
			//http://www.pokestadium.com/sprites/xy/unown.gif
			//http://www.pokestadium.com/assets/img/sprites/official-art/unown.png
			urlIcon = "http://www.pokestadium.com/sprites/xy/" + infos[3] + ".gif";
			string nameEn = infos[3]; nameEn = char.ToUpper(nameEn[0]) + nameEn.Substring(1);
			string nameFr = infos[4]; nameFr = char.ToUpper(nameFr[0]) + nameFr.Substring(1);
			int catchRate = int.Parse(infos[5]);
			int rarityTier = int.Parse(infos[6]);

			string pokepediaUrl = "https://www.pokepedia.fr/" + nameFr;
			string prowikiUrl = "https://prowiki.info/index.php?title=" + nameEn;

			return new EmbedBuilder()
				.WithTitle("-- " + nameEn + " / " + nameFr + " --")
				.WithDescription("[Pokepedia](" + pokepediaUrl + ") - [ProWiki](" + prowikiUrl + ")")
				.WithColor(new Color(89, 181, 255))
				.WithThumbnailUrl(urlIcon)
				.AddField("ID", id, true)
				.AddField("Catch rate", catchRate, true)
				.AddField("Rarity tier", rarityTier, true)
				.Build();
		}
	}
}
