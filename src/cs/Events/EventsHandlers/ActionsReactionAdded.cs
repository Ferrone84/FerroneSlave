using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Data;
using DiscordBot.Utilities;

namespace DiscordBot.Events.EventsHandlers
{
	public class ActionsReactionAdded : IGuildMessageReactionAddedEventHandler
	{
		//TODO A terme implémentera le system d'Actions lié => dossier Reactions
		public async Task Guild_Message_ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
		{
			try {
				IUserMessage message = channel.GetMessageAsync(cachedMessage.Id).Result as IUserMessage;

				if (reaction.User.Value.IsAdmin()) {
					IUserMessage nsfwMessage = ReactionUtils.IsThisNsfwInProgress(message);

					if (nsfwMessage != null) {
						if (reaction.Emote.Equals(EmoteManager.CheckMark)) {
							await message.DeleteAsync();
							ReactionUtils.NsfwProcessing(nsfwMessage);
							ReactionUtils.RemoveNsfwMessage(message);
						}
						else if (reaction.Emote.Equals(EmoteManager.CrossMark)) {
							await message.DeleteAsync();
							await nsfwMessage.RemoveAllReactionsAsync();
							ReactionUtils.RemoveNsfwMessage(message);
						}
					}
					else if (channel.Id == Channels.Musique.Id && reaction.Emote.Equals(EmoteManager.NegativeCrossMark)) {
						string result = DataManager.database.removeMusic(message.Content.GetYtLink());

						if (result == string.Empty) {
							await message.RemoveAllReactionsAsync();
							await message.AddReactionAsync(EmoteManager.Skull);
							await Channels.Debugs.SendMessageAsync("Message n°" + reaction.MessageId + " deleted from musique database. (" + message.Content + ")");
						}
					}
					else if (reaction.Emote.Equals(EmoteManager.Nsfw)) {
						ReactionUtils.NsfwProcessing(message);
					}
				}
				else {
					if (reaction.Emote.Equals(EmoteManager.Nsfw)) {
						int result = 0;
						var reactedUsers = await message.GetReactionUsersAsync(EmoteManager.Nsfw, 100).FlattenAsync();

						IUser user = null;
						using (IEnumerator<IUser> enumerator = reactedUsers.GetEnumerator()) {
							while (enumerator.MoveNext()) {
								if (result == 0) {
									user = enumerator.Current;
								}
								result++;
							}
						}

						if (result == 1) {
							await message.AddReactionAsync(EmoteManager.Nsfw);
							Embed embed = message.Quote(reaction.User.Value);
							IUserMessage messageSent = null;

							if (embed != null) {
								messageSent = await message.Channel.SendMessageAsync("<@&328899154887835678> Is this NSFW ? *reported by " + user.Mention + "*", false, embed);
							}
							else {
								messageSent = await message.Channel.SendMessageAsync("<@&328899154887835678> Is this NSFW ? *reported by " + user.Mention + "*");
							}
							await messageSent.AddReactionsAsync(new IEmote[] { EmoteManager.CheckMark, EmoteManager.CrossMark });
							DataManager.nsfw_content_inprocess.Add(messageSent, message);
						}
					}
				}
			}
			catch (Exception e) {
				e.Display(System.Reflection.MethodBase.GetCurrentMethod().ToString());
			}
		}
	}
}
