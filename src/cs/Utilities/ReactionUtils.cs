using Discord;
using DiscordBot.Data;
using System;

namespace DiscordBot.Utilities
{
	public class ReactionUtils
	{
		public static IUserMessage IsThisNsfwInProgress(IUserMessage message)
		{
			foreach (var msg in DataManager.nsfw_content_inprocess) {
				if (msg.Key.Id == message.Id) {
					return msg.Value;
				}
			}
			return null;
		}

		public static void RemoveNsfwMessage(IUserMessage message)
		{
			foreach (var msg in DataManager.nsfw_content_inprocess) {
				if (msg.Key.Id == message.Id) {
					DataManager.nsfw_content_inprocess.Remove(message);
				}
			}
		}

		public static async void NsfwProcessing(IUserMessage message)
		{
			//renvoyer dans le meme channel un embed qui met l'icone NSFW + qui dit qui a commit la faute
			var embed = GetNsfwEmbed(message.Author);

			if (embed != null) {
				await message.Channel.SendMessageAsync("", false, embed);
			}
			else {
				await message.Channel.SendMessageAsync("What do fock.");
			}

			//renvoyer le message dans le channel nsfw
			if (message.Attachments.Count == 0) {
				await Channels.Nsfw.SendMessageAsync("*--Content proposed by " + message.Author.Mention + "--*\n" + message.Content);
			}
			else {
				foreach (IAttachment attachment in message.Attachments) {
					await Channels.Nsfw.SendMessageAsync("*--Content proposed by " + message.Author.Mention + "--*\n" + message.Content + "\n" + attachment.Url);
				}
			}

			//supprimer le message originel
			try {
				await message.DeleteAsync();
			}
			catch (Exception) { }
		}

		private static Embed GetNsfwEmbed(IUser author)
		{
			return new EmbedBuilder()
				.WithTitle("NSFW POLICE FORCE")
				.WithDescription("Report to this field : <#389537278671978497>")
				.WithColor(new Color(200, 25, 25))
				.WithThumbnailUrl("https://cdn.discordapp.com/emojis/539905759580782602.png?v=1")
				.AddField("\u200B", "You " + author.Mention + " sir, are DISGUSTING !", false)
				.Build();
		}
	}
}
