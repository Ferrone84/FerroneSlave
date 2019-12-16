using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Data;
using DiscordBot.Utilities;

namespace DiscordBot.Events.EventsHandlers
{
	public class Logs :
		/*ISelfConnectedEventHandler,*/ ISelfReadyEventHandler, ISelfDisconnectedEventHandler, ISelfJoinedGuildEventHandler, ISelfLeftGuildEventHandler, ISelfCurrentUserUpdatedEventHandler, ISelfLatencyUpdatedEventHandler,
		//IDMUserUpdatedEventHandler, IDMUserTypesEventHandler, IDMUserVoiceStateUpdatedEventHandler,
		//IDMRecipientAddedEventHandler, IDMRecipientRemovedEventHandler,
		IDMMessageReceivedEventHandler, IDMMessageUpdatedEventHandler, IDMMessageDeletedEventHandler, //IDMMessageReactionAddedEventHandler, IDMMessageReactionRemovedEventHandler, IDMMessageReactionsClearedEventHandler,
		/*IGuildAvailableEventHandler,*/ IGuildUpdatedEventHandler, IGuildUnavailableEventHandler, IGuildMembersDownloadedEventHandler, IGuildMemberUpdatedEventHandler,
		IGuildUserJoinedEventHandler, IGuildUserLeftEventHandler, IGuildUserBannedEventHandler, IGuildUserUnbannedEventHandler, IGuildUserUpdatedEventHandler, /*IGuildUserTypesEventHandler,*/
		IGuildRoleCreatedEventHandler, IGuildRoleUpdatedEventHandler, IGuildRoleDeletedEventHandler,
		IGuildChannelCreatedEventHandler, IGuildChannelUpdatedEventHandler, IGuildChannelDestroyedEventHandler,
		IGuildMessageReceivedEventHandler, IGuildMessageUpdatedEventHandler, IGuildMessageDeletedEventHandler, /*IGuildMessageReactionAddedEventHandler, IGuildMessageReactionRemovedEventHandler,*/ IGuildMessageReactionsClearedEventHandler
	{
		private string breaker = " / ";

		public async Task DM_Message_Deleted(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel)
		{
			try {
				string messageInfos = (cachedMessage.Value is IUserMessage message) ? GetChannelInfos(channel) + " => " + message.Author.Username + " said : " + message.Content : "Message " + cachedMessage.Id + " doesn't exists.";
				await SendLogs("DM_Message_Deleted", messageInfos, EmoteManager.Guilds.Prod.Minus);
			}
			catch (Exception e) {
				e.Display();
			}
		}

		public async Task DM_Message_Received(SocketUserMessage message)
		{
			string logs = GetChannelInfos(message.Channel) + " => " + message.Author.Username + " say : " + message.Content;
			await SendLogs("DM_Message_Received", logs, EmoteManager.Guilds.Prod.Plus);
		}

		public async Task DM_Message_Updated(Cacheable<IMessage, ulong> cacheMessage, SocketUserMessage newMessage, ISocketMessageChannel channel)
		{
			string logs = GetChannelInfos(channel) + " => " + newMessage.Author.Username + " now say : " + newMessage.Content;
			await SendLogs("DM_Message_Updated", logs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Guild_Channel_Created(SocketChannel channel)
		{
			await SendLogs("Guild_Channel_Created", GetChannelInfos(channel), EmoteManager.Guilds.Prod.Plus);
		}

		public async Task Guild_Channel_Destroyed(SocketChannel channel)
		{
			await SendLogs("Guild_Channel_Destroyed", GetChannelInfos(channel), EmoteManager.Guilds.Prod.Minus);
		}

		public async Task Guild_Channel_Updated(SocketChannel oldChannel, SocketChannel newChannel)
		{
			string eventName = "Guild_Channel_Updated => ";
			StringBuilder logs = new StringBuilder("-> ");

			if (oldChannel is ISocketMessageChannel) {
				SocketTextChannel oldTextChannel = oldChannel as SocketTextChannel;
				SocketTextChannel newTextChannel = newChannel as SocketTextChannel;
				eventName += "(TextChannel)" + oldTextChannel.Guild.Name + "." + oldTextChannel.Name;

				if (oldTextChannel.Name != newTextChannel.Name) {
					logs.Append("(Name) [").Append(oldTextChannel.Name).Append(" => ").Append(newTextChannel.Name).Append("]").Append(breaker);
				}
				if (oldTextChannel.SlowModeInterval != newTextChannel.SlowModeInterval) {
					logs.Append("(SlowModeInterval) [").Append(oldTextChannel.SlowModeInterval).Append(" => ").Append(newTextChannel.SlowModeInterval).Append("]").Append(breaker);
				}
				if (oldTextChannel.Topic != newTextChannel.Topic) {
					logs.Append("(Topic) [").Append(oldTextChannel.Topic).Append(" => ").Append(newTextChannel.Topic).Append("]").Append(breaker);
				}
				if (oldTextChannel.IsNsfw != newTextChannel.IsNsfw) {
					logs.Append("(IsNsfw) [").Append(oldTextChannel.IsNsfw).Append(" => ").Append(newTextChannel.IsNsfw).Append("]").Append(breaker);
				}
				if (oldTextChannel.Position != newTextChannel.Position) {
					logs.Append("(Position) [").Append(oldTextChannel.Position).Append(" => ").Append(newTextChannel.Position).Append("]").Append(breaker);
				}
				if (!logs.ToString().Contains(breaker) && !oldTextChannel.PermissionOverwrites.Equals(newTextChannel.PermissionOverwrites)) {
					logs.Append("(PermissionOverwrites) [").Append(GetChangedPermissions(oldChannel, newChannel)).Append("]").Append(breaker);
				}
			}
			else if (oldChannel is ISocketAudioChannel) {
				SocketVoiceChannel oldVoiceChannel = oldChannel as SocketVoiceChannel;
				SocketVoiceChannel newVoiceChannel = newChannel as SocketVoiceChannel;
				eventName += "(VoiceChannel)" + oldVoiceChannel.Guild.Name + "." + oldVoiceChannel.Name;

				if (oldVoiceChannel.Name != newVoiceChannel.Name) {
					logs.Append("(Name) [").Append(oldVoiceChannel.Name).Append(" => ").Append(newVoiceChannel.Name).Append("]").Append(breaker);
				}
				if (oldVoiceChannel.Bitrate != newVoiceChannel.Bitrate) {
					logs.Append("(Bitrate) [").Append(oldVoiceChannel.Bitrate).Append(" => ").Append(newVoiceChannel.Bitrate).Append("]").Append(breaker);
				}
				if (oldVoiceChannel.UserLimit != newVoiceChannel.UserLimit) {
					logs.Append("(UserLimit) [").Append(oldVoiceChannel.UserLimit ?? 0).Append(" => ").Append(newVoiceChannel.UserLimit ?? 0).Append("]").Append(breaker);
				}
				if (oldVoiceChannel.Position != newVoiceChannel.Position) {
					logs.Append("(Position) [").Append(oldVoiceChannel.Position).Append(" => ").Append(newVoiceChannel.Position).Append("]").Append(breaker);
				}
				if (!oldVoiceChannel.PermissionOverwrites.Equals(newVoiceChannel.PermissionOverwrites)) {
					logs.Append("(PermissionOverwrites) [").Append(GetChangedPermissions(oldChannel, newChannel)).Append("]").Append(breaker);
				}
			}

			string resultLogs = logs.ToString();
			if (resultLogs.EndsWith(breaker)) {
				resultLogs = resultLogs.Substring(0, resultLogs.Length - breaker.Length);
			}

			await SendLogs(eventName, resultLogs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Guild_MembersDownloaded(SocketGuild guild)
		{
			await SendLogs("Guild_MembersDownloaded", "-> (Guild) => " + guild.Name.Brackets());
		}

		public async Task Guild_MemberUpdated(SocketGuildUser oldUser, SocketGuildUser newUser)
		{
			string eventName = "Guild_MemberUpdated => " + oldUser.Guild.Name + "." + oldUser.Username;
			StringBuilder logs = new StringBuilder("-> ");

			if (oldUser.Nickname != newUser.Nickname) {
				logs.Append("(Nickname) [").Append(oldUser.Nickname ?? oldUser.Username).Append(" => ").Append(newUser.Nickname ?? newUser.Username).Append("]").Append(breaker);
			}
			else if (!oldUser.Roles.Equals(newUser.Roles)) {
				var oldUserRoles = oldUser.Roles;
				var newUserRoles = newUser.Roles;

				bool roles = false;
				StringBuilder tmpBuilder = new StringBuilder("(AddedRoles) [");
				foreach (IRole role in newUserRoles) {
					if (!oldUserRoles.Contains(role)) {
						roles = true;
						tmpBuilder.Append(role.Name).Append(breaker);
					}
				}
				string tmpLogs = tmpBuilder.ToString();
				if (tmpLogs.EndsWith(breaker)) {
					tmpLogs = tmpLogs.Substring(0, tmpLogs.Length - breaker.Length);
				}
				if (roles) {
					logs.Append(tmpLogs).Append("]").Append(breaker);
				}

				roles = false;
				tmpBuilder = new StringBuilder("(DeletedRoles) [");
				foreach (IRole role in oldUserRoles) {
					if (!newUserRoles.Contains(role)) {
						roles = true;
						tmpBuilder.Append(role.Name).Append(breaker);
					}
				}
				tmpLogs = tmpBuilder.ToString();
				if (tmpLogs.EndsWith(breaker)) {
					tmpLogs = tmpLogs.Substring(0, tmpLogs.Length - breaker.Length);
				}
				if (roles) {
					logs.Append(tmpLogs).Append("]").Append(breaker);
				}
			}

			string resultLogs = logs.ToString();
			if (resultLogs.EndsWith(breaker)) {
				resultLogs = resultLogs.Substring(0, resultLogs.Length - breaker.Length);
			}

			if (resultLogs.Replace("-> ", "") == string.Empty) { return; }
			await SendLogs(eventName, resultLogs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Guild_Message_ReactionsCleared(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel)
		{
			IUserMessage message = cachedMessage.Value;
			string logs = GetChannelInfos(channel) + " => " + message.Author.Username + " cleared reactions on message " + message.Id + " with content : " + message.Content;
			await SendLogs("Guild_Message_ReactionsCleared", logs, EmoteManager.Guilds.Prod.Minus);
		}

		public async Task Guild_Message_Received(SocketUserMessage message)
		{
			if (message.Channel.Id == Channels.Logs.Id) { return; }
			string logs = GetChannelInfos(message.Channel) + " => " + message.Author.Username + " say : " + message.Content;
			await SendLogs("Guild_Message_Received", logs, EmoteManager.Guilds.Prod.Plus);
		}

		public async Task Guild_Message_Updated(Cacheable<IMessage, ulong> cacheMessage, SocketUserMessage newMessage, ISocketMessageChannel channel)
		{
			string logs = GetChannelInfos(channel) + " => " + newMessage.Author.Username + " now say : " + newMessage.Content;
			await SendLogs("Guild_Message_Updated", logs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Guild_Message_Deleted(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel)
		{
			try {
				string messageInfos = (cachedMessage.Value is IUserMessage message) ? GetChannelInfos(channel) + " => " + message.Author.Username + " said : " + message.Content : "Message " + cachedMessage.Id + " doesn't exists.";
				await SendLogs("Guild_Message_Deleted", messageInfos, EmoteManager.Guilds.Prod.Minus);
			}
			catch (Exception e) {
				e.Display();
			}
		}

		public async Task Guild_Role_Created(SocketRole role)
		{
			string logs = "Role [" + role.Id + " / " + role.Name + "] created in Guild " + role.Guild.Name.Brackets() + ".";
			await SendLogs("Guild_Role_Created", logs, EmoteManager.Guilds.Prod.Plus);
		}

		public async Task Guild_Role_Deleted(SocketRole role)
		{
			string logs = "Role [" + role.Id + " / " + role.Name + "] deleted in Guild " + role.Guild.Name.Brackets() + ".";
			await SendLogs("Guild_Role_Deleted", logs, EmoteManager.Guilds.Prod.Minus);
		}

		public async Task Guild_Role_Updated(SocketRole oldRole, SocketRole newRole)
		{
			string eventName = "Guild_Role_Updated => " + oldRole.Guild.Name + "." + oldRole.Name.Replace("@", "");
			StringBuilder logs = new StringBuilder("-> ");

			if (oldRole.Color.ToString() != newRole.Color.ToString()) {
				logs.Append("(Color) [").Append(oldRole.Color).Append(" => ").Append(newRole.Color).Append("]").Append(breaker);
			}
			if (oldRole.IsHoisted != newRole.IsHoisted) {
				logs.Append("(IsHoisted) [").Append(oldRole.IsHoisted).Append(" => ").Append(newRole.IsHoisted).Append("]").Append(breaker);
			}
			if (oldRole.IsMentionable != newRole.IsMentionable) {
				logs.Append("(IsMentionable) [").Append(oldRole.IsMentionable).Append(" => ").Append(newRole.IsMentionable).Append("]").Append(breaker);
			}
			if (oldRole.Name != newRole.Name) {
				logs.Append("(Name) [").Append(oldRole.Name).Append(" => ").Append(newRole.Name).Append("]").Append(breaker);
			}
			if (oldRole.Position != newRole.Position) {
				logs.Append("(Position) [").Append(oldRole.Position).Append(" => ").Append(newRole.Position).Append("]").Append(breaker);
			}
			if (!oldRole.Permissions.Equals(newRole.Permissions)) {
				logs.Append("(Permissions) [");
				GuildPermissions oldPermissions = oldRole.Permissions;
				GuildPermissions newPermissions = newRole.Permissions;

				//oldPermissions.RawValue;
				if (oldPermissions.AddReactions != newPermissions.AddReactions) { logs.Append("(AddReactions) [").Append(oldPermissions.AddReactions).Append(" => ").Append(newPermissions.AddReactions).Append("]").Append(breaker); }
				if (oldPermissions.Administrator != newPermissions.Administrator) { logs.Append("(Administrator) [").Append(oldPermissions.Administrator).Append(" => ").Append(newPermissions.Administrator).Append("]").Append(breaker); }
				if (oldPermissions.AttachFiles != newPermissions.AttachFiles) { logs.Append("(AttachFiles) [").Append(oldPermissions.AttachFiles).Append(" => ").Append(newPermissions.AttachFiles).Append("]").Append(breaker); }
				if (oldPermissions.BanMembers != newPermissions.BanMembers) { logs.Append("(BanMembers) [").Append(oldPermissions.BanMembers).Append(" => ").Append(newPermissions.BanMembers).Append("]").Append(breaker); }
				if (oldPermissions.ChangeNickname != newPermissions.ChangeNickname) { logs.Append("(ChangeNickname) [").Append(oldPermissions.ChangeNickname).Append(" => ").Append(newPermissions.ChangeNickname).Append("]").Append(breaker); }
				if (oldPermissions.Connect != newPermissions.Connect) { logs.Append("(Connect) [").Append(oldPermissions.Connect).Append(" => ").Append(newPermissions.Connect).Append("]").Append(breaker); }
				if (oldPermissions.CreateInstantInvite != newPermissions.CreateInstantInvite) { logs.Append("(CreateInstantInvite) [").Append(oldPermissions.CreateInstantInvite).Append(" => ").Append(newPermissions.CreateInstantInvite).Append("]").Append(breaker); }
				if (oldPermissions.DeafenMembers != newPermissions.DeafenMembers) { logs.Append("(DeafenMembers) [").Append(oldPermissions.DeafenMembers).Append(" => ").Append(newPermissions.DeafenMembers).Append("]").Append(breaker); }
				if (oldPermissions.EmbedLinks != newPermissions.EmbedLinks) { logs.Append("(EmbedLinks) [").Append(oldPermissions.EmbedLinks).Append(" => ").Append(newPermissions.EmbedLinks).Append("]").Append(breaker); }
				if (oldPermissions.KickMembers != newPermissions.KickMembers) { logs.Append("(KickMembers) [").Append(oldPermissions.KickMembers).Append(" => ").Append(newPermissions.KickMembers).Append("]").Append(breaker); }
				if (oldPermissions.ManageChannels != newPermissions.ManageChannels) { logs.Append("(ManageChannels) [").Append(oldPermissions.ManageChannels).Append(" => ").Append(newPermissions.ManageChannels).Append("]").Append(breaker); }
				if (oldPermissions.ManageEmojis != newPermissions.ManageEmojis) { logs.Append("(ManageEmojis) [").Append(oldPermissions.ManageEmojis).Append(" => ").Append(newPermissions.ManageEmojis).Append("]").Append(breaker); }
				if (oldPermissions.ManageGuild != newPermissions.ManageGuild) { logs.Append("(ManageGuild) [").Append(oldPermissions.ManageGuild).Append(" => ").Append(newPermissions.ManageGuild).Append("]").Append(breaker); }
				if (oldPermissions.ManageMessages != newPermissions.ManageMessages) { logs.Append("(ManageMessages) [").Append(oldPermissions.ManageMessages).Append(" => ").Append(newPermissions.ManageMessages).Append("]").Append(breaker); }
				if (oldPermissions.ManageNicknames != newPermissions.ManageNicknames) { logs.Append("(ManageNicknames) [").Append(oldPermissions.ManageNicknames).Append(" => ").Append(newPermissions.ManageNicknames).Append("]").Append(breaker); }
				if (oldPermissions.ManageRoles != newPermissions.ManageRoles) { logs.Append("(ManageRoles) [").Append(oldPermissions.ManageRoles).Append(" => ").Append(newPermissions.ManageRoles).Append("]").Append(breaker); }
				if (oldPermissions.ManageWebhooks != newPermissions.ManageWebhooks) { logs.Append("(ManageWebhooks) [").Append(oldPermissions.ManageWebhooks).Append(" => ").Append(newPermissions.ManageWebhooks).Append("]").Append(breaker); }
				if (oldPermissions.MentionEveryone != newPermissions.MentionEveryone) { logs.Append("(MentionEveryone) [").Append(oldPermissions.MentionEveryone).Append(" => ").Append(newPermissions.MentionEveryone).Append("]").Append(breaker); }
				if (oldPermissions.MoveMembers != newPermissions.MoveMembers) { logs.Append("(MoveMembers) [").Append(oldPermissions.MoveMembers).Append(" => ").Append(newPermissions.MoveMembers).Append("]").Append(breaker); }
				if (oldPermissions.MuteMembers != newPermissions.MuteMembers) { logs.Append("(MuteMembers) [").Append(oldPermissions.MuteMembers).Append(" => ").Append(newPermissions.MuteMembers).Append("]").Append(breaker); }
				if (oldPermissions.PrioritySpeaker != newPermissions.PrioritySpeaker) { logs.Append("(PrioritySpeaker) [").Append(oldPermissions.PrioritySpeaker).Append(" => ").Append(newPermissions.PrioritySpeaker).Append("]").Append(breaker); }
				if (oldPermissions.ReadMessageHistory != newPermissions.ReadMessageHistory) { logs.Append("(ReadMessageHistory) [").Append(oldPermissions.ReadMessageHistory).Append(" => ").Append(newPermissions.ReadMessageHistory).Append("]").Append(breaker); }
				if (oldPermissions.SendMessages != newPermissions.SendMessages) { logs.Append("(SendMessages) [").Append(oldPermissions.SendMessages).Append(" => ").Append(newPermissions.SendMessages).Append("]").Append(breaker); }
				if (oldPermissions.SendTTSMessages != newPermissions.SendTTSMessages) { logs.Append("(SendTTSMessages) [").Append(oldPermissions.SendTTSMessages).Append(" => ").Append(newPermissions.SendTTSMessages).Append("]").Append(breaker); }
				if (oldPermissions.Speak != newPermissions.Speak) { logs.Append("(Speak) [").Append(oldPermissions.Speak).Append(" => ").Append(newPermissions.Speak).Append("]").Append(breaker); }
				if (oldPermissions.UseExternalEmojis != newPermissions.UseExternalEmojis) { logs.Append("(UseExternalEmojis) [").Append(oldPermissions.UseExternalEmojis).Append(" => ").Append(newPermissions.UseExternalEmojis).Append("]").Append(breaker); }
				if (oldPermissions.UseVAD != newPermissions.UseVAD) { logs.Append("(UseVAD) [").Append(oldPermissions.UseVAD).Append(" => ").Append(newPermissions.UseVAD).Append("]").Append(breaker); }
				if (oldPermissions.ViewAuditLog != newPermissions.ViewAuditLog) { logs.Append("(ViewAuditLog) [").Append(oldPermissions.ViewAuditLog).Append(" => ").Append(newPermissions.ViewAuditLog).Append("]").Append(breaker); }
				if (oldPermissions.ViewChannel != newPermissions.ViewChannel) { logs.Append("(ViewChannel) [").Append(oldPermissions.ViewChannel).Append(" => ").Append(newPermissions.ViewChannel).Append("]").Append(breaker); }

				string tmpLogs = logs.ToString();
				if (tmpLogs.EndsWith(breaker)) {
					tmpLogs = tmpLogs.Substring(0, tmpLogs.Length - breaker.Length);
				}
				logs = new StringBuilder(tmpLogs);
				logs.Append("]").Append(breaker);
			}

			string resultLogs = logs.ToString();
			if (resultLogs.EndsWith(breaker)) {
				resultLogs = resultLogs.Substring(0, resultLogs.Length - breaker.Length);
			}

			if (resultLogs.Replace("-> ", "") == string.Empty) { return; }
			await SendLogs(eventName, resultLogs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Guild_Unavailable(SocketGuild guild)
		{
			await SendLogs("Guild_Unavailable", "-> (Guild) => " + guild.Name.Brackets(), EmoteManager.Guilds.Prod.Minus);
		}

		public async Task Guild_Updated(SocketGuild oldGuild, SocketGuild newGuild)
		{
			string eventName = "Guild_Updated (Guild) => " + oldGuild.Name;
			StringBuilder logs = new StringBuilder("-> ");

			if (oldGuild.Name != newGuild.Name) {
				logs.Append("(Name) [").Append(oldGuild.Name).Append(" => ").Append(newGuild.Name).Append("]").Append(breaker);
			}
			if (!oldGuild.VerificationLevel.Equals(newGuild.VerificationLevel)) {
				logs.Append("(VerificationLevel) [").Append(oldGuild.VerificationLevel).Append(" => ").Append(newGuild.VerificationLevel).Append("]").Append(breaker);
			}
			if (!oldGuild.ExplicitContentFilter.Equals(newGuild.ExplicitContentFilter)) {
				logs.Append("(ExplicitContentFilter) [").Append(oldGuild.ExplicitContentFilter).Append(" => ").Append(newGuild.ExplicitContentFilter).Append("]").Append(breaker);
			}
			if (!oldGuild.Emotes.Equals(newGuild.Emotes)) {
				var oldEmotes = oldGuild.Emotes;
				var newEmotes = newGuild.Emotes;

				bool emotes = false;
				StringBuilder tmpBuilder = new StringBuilder("(AddedEmotes) [");
				foreach (IEmote emote in newEmotes) {
					if (!oldEmotes.Contains(emote)) {
						emotes = true;
						tmpBuilder.Append(emote.ToString()).Append(breaker);
					}
				}
				string tmpLogs = tmpBuilder.ToString();
				if (tmpLogs.EndsWith(breaker)) {
					tmpLogs = tmpLogs.Substring(0, tmpLogs.Length - breaker.Length);
				}
				if (emotes) {
					logs.Append(tmpLogs).Append("]").Append(breaker);
				}

				emotes = false;
				tmpBuilder = new StringBuilder("(DeletedEmotes) [");
				foreach (IEmote emote in oldEmotes) {
					if (!newEmotes.Contains(emote)) {
						emotes = true;
						tmpBuilder.Append(emote.ToString()).Append(breaker);
					}
				}
				tmpLogs = tmpBuilder.ToString();
				if (tmpLogs.EndsWith(breaker)) {
					tmpLogs = tmpLogs.Substring(0, tmpLogs.Length - breaker.Length);
				}
				if (emotes) {
					logs.Append(tmpLogs).Append("]").Append(breaker);
				}
			}

			string resultLogs = logs.ToString();
			if (resultLogs.EndsWith(breaker)) {
				resultLogs = resultLogs.Substring(0, resultLogs.Length - breaker.Length);
			}

			await SendLogs(eventName, resultLogs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Guild_User_Banned(SocketUser user, SocketGuild guild)
		{
			string logs = "-> (Guild) => " + guild.Name.Brackets() + breaker + "(User) => " + GetUserInfos(user);
			await SendLogs("Guild_User_Banned", logs, EmoteManager.Guilds.Prod.Ban);
		}

		public async Task Guild_User_Joined(SocketGuildUser user)
		{
			string logs = "-> (Guild) => " + user.Guild.Name.Brackets() + breaker + "(User) => " + GetUserInfos(user);
			await SendLogs("Guild_User_Joined", logs, EmoteManager.Guilds.Prod.Plus);
		}

		public async Task Guild_User_Left(SocketGuildUser user)
		{
			string logs = "-> (Guild) => " + user.Guild.Name.Brackets() + breaker + "(User) => " + GetUserInfos(user);
			await SendLogs("Guild_User_Left", logs, EmoteManager.Guilds.Prod.Minus);
		}

		public async Task Guild_User_Unbanned(SocketUser user, SocketGuild guild)
		{
			string logs = "-> (Guild) => " + guild.Name.Brackets() + breaker + "(User) => " + GetUserInfos(user);
			await SendLogs("Guild_User_Banned", logs, EmoteManager.Guilds.Prod.Ban);
		}

		public async Task Guild_User_Updated(SocketUser oldUser, SocketUser newUser)
		{
			string logs = "-> (Guild) => " + (oldUser as IGuildUser).Guild.Name.Brackets() + breaker + "(User) => " + GetUserInfos(oldUser) + breaker + GetUserInfos(newUser);
			logs += " FINALLY FIRED [Guild_User_Updated] " + Users.Ferrone.Mention;
			await SendLogs("Guild_User_Updated", logs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Self_CurrentUserUpdated(SocketSelfUser oldUser, SocketSelfUser newUser)
		{
			string logs = "FINALLY FIRED [Self_CurrentUserUpdated] " + Users.Ferrone.Mention + breaker + "(User) => " + GetUserInfos(oldUser);

			breaker.Debug(oldUser.Username, newUser.Username);
			breaker.Debug(oldUser.IsVerified.ToString(), newUser.IsVerified.ToString());
			breaker.Debug(oldUser.IsMfaEnabled.ToString(), newUser.IsMfaEnabled.ToString());
			breaker.Debug(oldUser.Email, newUser.Email);
			breaker.Debug(oldUser.Discriminator, newUser.Discriminator);
			breaker.Debug(oldUser.Activity.Name, newUser.Activity.Name);
			breaker.Debug(oldUser.Activity.Type.ToString(), newUser.Activity.Type.ToString());
			
			await SendLogs("Self_CurrentUserUpdated", logs, EmoteManager.Guilds.Prod.Edit);
		}

		public async Task Self_Disconnected(Exception exception)
		{
			string logs = "Disconnected. " + exception?.Message;
			await SendLogs("Self_Disconnected", logs);
		}

		public async Task Self_JoinedGuild(SocketGuild guild)
		{
			string logs = "-> (Guild) => " + guild.Name.Brackets() + breaker + "(Owner) => " + GetUserInfos(guild.Owner) + breaker + "(CreatedAt) => " + guild.CreatedAt.Date.ToString();
			await SendLogs("Self_JoinedGuild", logs, EmoteManager.Guilds.Prod.Plus);
		}

		public async Task Self_LeftGuild(SocketGuild guild)
		{
			string logs = "-> (Guild) => " + guild.Name.Brackets() + breaker + "(Owner) => " + GetUserInfos(guild.Owner) + breaker + "(CreatedAt) => " + guild.CreatedAt.Date.ToString();
			await SendLogs("Self_LeftGuild", logs, EmoteManager.Guilds.Prod.Minus);
		}

		public async Task Self_LatencyUpdated(int old_value, int new_value)
		{
			if (new_value >= 300) {
				string logs = "Latency unusually very high : " + new_value.ToString().Brackets();
				await SendLogs("Self_LatencyUpdated", logs);
			}
		}

		public async Task Self_Ready()
		{
			string logs = "Bot Ready.";
			await SendLogs("Self_Ready", logs);
		}



		private async Task SendLogs(string eventName, string logs, IEmote logEmote = null)
		{
			string emote = (logEmote == null) ? string.Empty : logEmote.ToString() + " ";
			string fullLogs = (emote + "[" + eventName + "] (" + DateTime.Now.ToString() + ") " + logs).Replace("@", "|@|");
			fullLogs.Println();
			await Channels.Logs.SendMessagesAsync(fullLogs);
		}

		private string GetChannelInfos(IChannel channel)
		{
			return (channel is SocketGuildChannel) ? ((channel as SocketGuildChannel).Guild.Name + "." + channel.Name).Brackets() : "DM".Brackets();
		}

		private string GetUserInfos(IUser user)
		{
			return user.ToString() + user.Id.ToString().Brackets();
		}

		private string GetChangedPermissions(SocketChannel oldChannel, SocketChannel newChannel)
		{
			StringBuilder result = new StringBuilder();
			OverwritePermissions oldPermissions = (oldChannel as SocketGuildChannel).PermissionOverwrites.First().Permissions;
			OverwritePermissions newPermissions = (newChannel as SocketGuildChannel).PermissionOverwrites.First().Permissions;

			if (oldPermissions.AddReactions != newPermissions.AddReactions) { result.Append("(AddReactions) [").Append(oldPermissions.AddReactions).Append(" => ").Append(newPermissions.AddReactions).Append("]").Append(breaker); }
			if (oldPermissions.AttachFiles != newPermissions.AttachFiles) { result.Append("(AttachFiles) [").Append(oldPermissions.AttachFiles).Append(" => ").Append(newPermissions.AttachFiles).Append("]").Append(breaker); }
			if (oldPermissions.Connect != newPermissions.Connect) { result.Append("(Connect) [").Append(oldPermissions.Connect).Append(" => ").Append(newPermissions.Connect).Append("]").Append(breaker); }
			if (oldPermissions.CreateInstantInvite != newPermissions.CreateInstantInvite) { result.Append("(CreateInstantInvite) [").Append(oldPermissions.CreateInstantInvite).Append(" => ").Append(newPermissions.CreateInstantInvite).Append("]").Append(breaker); }
			if (oldPermissions.DeafenMembers != newPermissions.DeafenMembers) { result.Append("(DeafenMembers) [").Append(oldPermissions.DeafenMembers).Append(" => ").Append(newPermissions.DeafenMembers).Append("]").Append(breaker); }
			if (oldPermissions.EmbedLinks != newPermissions.EmbedLinks) { result.Append("(EmbedLinks) [").Append(oldPermissions.EmbedLinks).Append(" => ").Append(newPermissions.EmbedLinks).Append("]").Append(breaker); }
			if (oldPermissions.ManageChannel != newPermissions.ManageChannel) { result.Append("(ManageChannel) [").Append(oldPermissions.ManageChannel).Append(" => ").Append(newPermissions.ManageChannel).Append("]").Append(breaker); }
			if (oldPermissions.ManageMessages != newPermissions.ManageMessages) { result.Append("(ManageMessages) [").Append(oldPermissions.ManageMessages).Append(" => ").Append(newPermissions.ManageMessages).Append("]").Append(breaker); }
			if (oldPermissions.ManageRoles != newPermissions.ManageRoles) { result.Append("(ManageRoles) [").Append(oldPermissions.ManageRoles).Append(" => ").Append(newPermissions.ManageRoles).Append("]").Append(breaker); }
			if (oldPermissions.ManageWebhooks != newPermissions.ManageWebhooks) { result.Append("(ManageWebhooks) [").Append(oldPermissions.ManageWebhooks).Append(" => ").Append(newPermissions.ManageWebhooks).Append("]").Append(breaker); }
			if (oldPermissions.MentionEveryone != newPermissions.MentionEveryone) { result.Append("(MentionEveryone) [").Append(oldPermissions.MentionEveryone).Append(" => ").Append(newPermissions.MentionEveryone).Append("]").Append(breaker); }
			if (oldPermissions.MoveMembers != newPermissions.MoveMembers) { result.Append("(MoveMembers) [").Append(oldPermissions.MoveMembers).Append(" => ").Append(newPermissions.MoveMembers).Append("]").Append(breaker); }
			if (oldPermissions.MuteMembers != newPermissions.MuteMembers) { result.Append("(MuteMembers) [").Append(oldPermissions.MuteMembers).Append(" => ").Append(newPermissions.MuteMembers).Append("]").Append(breaker); }
			if (oldPermissions.ReadMessageHistory != newPermissions.ReadMessageHistory) { result.Append("(ReadMessageHistory) [").Append(oldPermissions.ReadMessageHistory).Append(" => ").Append(newPermissions.ReadMessageHistory).Append("]").Append(breaker); }
			if (oldPermissions.SendMessages != newPermissions.SendMessages) { result.Append("(SendMessages) [").Append(oldPermissions.SendMessages).Append(" => ").Append(newPermissions.SendMessages).Append("]").Append(breaker); }
			if (oldPermissions.SendTTSMessages != newPermissions.SendTTSMessages) { result.Append("(SendTTSMessages) [").Append(oldPermissions.SendTTSMessages).Append(" => ").Append(newPermissions.SendTTSMessages).Append("]").Append(breaker); }
			if (oldPermissions.Speak != newPermissions.Speak) { result.Append("(Speak) [").Append(oldPermissions.Speak).Append(" => ").Append(newPermissions.Speak).Append("]").Append(breaker); }
			if (oldPermissions.UseExternalEmojis != newPermissions.UseExternalEmojis) { result.Append("(UseExternalEmojis) [").Append(oldPermissions.UseExternalEmojis).Append(" => ").Append(newPermissions.UseExternalEmojis).Append("]").Append(breaker); }
			if (oldPermissions.UseVAD != newPermissions.UseVAD) { result.Append("(UseVAD) [").Append(oldPermissions.UseVAD).Append(" => ").Append(newPermissions.UseVAD).Append("]").Append(breaker); }
			if (oldPermissions.ViewChannel != newPermissions.ViewChannel) { result.Append("(ViewChannel) [").Append(oldPermissions.ViewChannel).Append(" => ").Append(newPermissions.ViewChannel).Append("]").Append(breaker); }

			string resultLogs = result.ToString();
			if (resultLogs.EndsWith(breaker)) {
				resultLogs = resultLogs.Substring(0, resultLogs.Length - breaker.Length);
			}

			return resultLogs;
		}
	}
}
