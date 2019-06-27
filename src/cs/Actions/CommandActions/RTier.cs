
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.CommandActions
{
	public class RTier : ACommandAction
	{
		public RTier() : base()
		{
			Name = Prefix + "rtier";
			Description = "Permet de savoir la rareté d'un pokemon (rarityTier).";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string msg = string.Empty;
			string message_lower = message.Content.ToLower();

			var words = message_lower.Split(' ');

			if (words.Length == 2) {
				string pokemonName = words[1];
				msg = DataManager.database.getPokemonInfo(pokemonName, DataManager.PokemonInfo.name, DataManager.PokemonInfo.rarityTier);
			}
			else {
				msg = "This command can be used like this : " + Name + " Charizard";
			}

			await message.Channel.SendMessagesAsync(msg);
		}
	}
}
