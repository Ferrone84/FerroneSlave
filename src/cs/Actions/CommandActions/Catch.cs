
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Data;

namespace DiscordBot.Actions.CommandActions
{
	public class Catch : ACommandAction
	{
		public Catch() : base()
		{
			Name = Prefix + "catch";
			Description = "Permet de savoir le % de chance de capture du pokemon ou d'obtenir son catchRate.";
		}

		public override async Task Invoke(IUserMessage message)
		{
			string result = "This command can be used by two diffents ways: \n1 - Just the pokemon name after the command, will send you his catch rate.\n2 - The second way will sent you his % chance of being catch : `!command life_percent catch_rate bonus_ball bonus_statut` => `!command 100 45 2 2`.";
			string message_lower = message.Content.ToLower();

			var words = message_lower.Split(' ');
			int words_length = words.Length;

			try {
				if (words_length == 2) {
					string pokemonName = words[1];
					result = DataManager.database.getPokemonInfo(pokemonName, DataManager.PokemonInfo.name, DataManager.PokemonInfo.catchRate);
				}
				else if (words_length == 5) {
					float hp_percent = float.Parse(words[1], CultureInfo.InvariantCulture.NumberFormat);
					float catch_rate = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
					float bonus_ball = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
					float bonus_statut = float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat);

					if (bonus_ball == 0.0) {
						throw new ArgumentException("Bonus_ball cannot be 0. Basics are : 1 for Pokeball, 1.5 for Superball and 2 for Hyperball.");
					}
					if (bonus_statut == 0.0) {
						throw new ArgumentException("Bonus_statut cannot be 0. Basics are : 1 for normal, 1.5 for paralyze and 2 for sleep.");
					}
					double formula = (1 - (2 / 3.0) * (hp_percent / 100.0)) * catch_rate * bonus_ball * bonus_statut;

					result = formula.ToString("0.00")+"%";
				}
				await message.Channel.SendMessagesAsync(result);
			}
			catch (ArgumentException e) {
				await message.Channel.SendMessageAsync(e.Message);
			}
			catch (Exception e) {
				e.Display(Name);
				await message.Channel.SendMessagesAsync(result);
			}
		}
	}
}
