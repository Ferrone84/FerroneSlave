
using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Data;

namespace DiscordBot.Actions.CommandActions
{
	public class Quote : ACommandAction
	{
		public Quote() : base()
		{
			Name = Prefix + "quote";
			Description = "Permet de citer le message de quelqu'un.";
			ChannelAccessibility = ChannelAccessibilityType.Guild;
		}

		public override async Task Invoke(IUserMessage message)
		{
			string message_lower = message.ToString().ToLower();
			var args = message_lower.Split(' ');
			string error_message = "This command can be used like this : " + Name + " message_id OR " + Name + " message_id reply.";

			if (args.Length < 2) {
				await message.Channel.SendMessageAsync(error_message);
				return;
			}

			if (!(message.Channel is SocketGuildChannel)) {
				await message.Channel.SendMessageAsync("Channel MUST be from a guild.");
				return;
			}

			try {
				ulong messageId = ulong.Parse(args[1]);

				IMessage msg = await (message.Channel as SocketGuildChannel).Guild.GetMessageFromId(messageId);
				if (msg == null) {
					throw new ArgumentException("Le message '" + messageId + "' n'existe pas ou le bot n'y a pas accès.");
				}

				await message.DeleteAsync();
				await message.Channel.SendMessageAsync("", false, msg.Quote(message.Author));

				if (args.Length >= 3) {
					string quote_args = args[0] + ' ' + args[1] + ' ';
					string reply = message_lower.Split(quote_args)[1];

					await message.Channel.SendMessageAsync("**" + message.Author.Username + " replied : **" + reply);
				}
			}
			catch (ArgumentException e) {
				await message.Channel.SendMessageAsync(e.Message);
			}
			catch (Discord.Net.HttpException e) {
				e.Display(Name);
				await message.Channel.SendMessageAsync("Le bot n'as pas accès au channel du message.");
				await Channels.Debugs.SendMessageAsync(e.Message);
			}
			catch (Exception e) {
				await message.Channel.SendMessageAsync(error_message);
				e.Display(Name);
			}
		}
	}
}
