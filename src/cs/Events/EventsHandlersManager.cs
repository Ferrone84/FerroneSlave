using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Events
{
	class EventHandlersManager :
		ISelfConnectedEventHandler, ISelfReadyEventHandler, ISelfDisconnectedEventHandler, ISelfJoinedGuildEventHandler, ISelfLeftGuildEventHandler, ISelfCurrentUserUpdatedEventHandler, ISelfLatencyUpdatedEventHandler,
		IDMUserUpdatedEventHandler, IDMUserTypesEventHandler, IDMUserVoiceStateUpdatedEventHandler,
		IDMRecipientAddedEventHandler, IDMRecipientRemovedEventHandler,
		IDMMessageReceivedEventHandler, IDMMessageUpdatedEventHandler, IDMMessageDeletedEventHandler, IDMMessageReactionAddedEventHandler, IDMMessageReactionRemovedEventHandler, IDMMessageReactionsClearedEventHandler,
		IGuildAvailableEventHandler, IGuildUpdatedEventHandler, IGuildUnavailableEventHandler, IGuildMembersDownloadedEventHandler, IGuildMemberUpdatedEventHandler,
		IGuildUserJoinedEventHandler, IGuildUserLeftEventHandler, IGuildUserBannedEventHandler, IGuildUserUnbannedEventHandler, IGuildUserUpdatedEventHandler, IGuildUserTypesEventHandler,
		IGuildRoleCreatedEventHandler, IGuildRoleUpdatedEventHandler, IGuildRoleDeletedEventHandler,
		IGuildChannelCreatedEventHandler, IGuildChannelUpdatedEventHandler, IGuildChannelDestroyedEventHandler,
		IGuildMessageReceivedEventHandler, IGuildMessageUpdatedEventHandler, IGuildMessageDeletedEventHandler, IGuildMessageReactionAddedEventHandler, IGuildMessageReactionRemovedEventHandler, IGuildMessageReactionsClearedEventHandler
	{
		readonly List<ISelfConnectedEventHandler> SelfConnectedEventHandler;
		readonly List<ISelfReadyEventHandler> SelfReadyEventHandler;
		readonly List<ISelfDisconnectedEventHandler> SelfDisconnectedEventHandler;
		readonly List<ISelfJoinedGuildEventHandler> SelfJoinedGuildEventHandler;
		readonly List<ISelfLeftGuildEventHandler> SelfLeftGuildEventHandler;
		readonly List<ISelfCurrentUserUpdatedEventHandler> SelfCurrentUserUpdatedEventHandler;
		readonly List<ISelfLatencyUpdatedEventHandler> SelfLatencyUpdatedEventHandler;
		readonly List<IDMUserUpdatedEventHandler> DMUserUpdatedEventHandler;
		readonly List<IDMUserTypesEventHandler> DMUserTypesEventHandler;
		readonly List<IDMUserVoiceStateUpdatedEventHandler> DMUserVoiceStateUpdatedEventHandler;
		readonly List<IDMRecipientAddedEventHandler> DMRecipientAddedEventHandler;
		readonly List<IDMRecipientRemovedEventHandler> DMRecipientRemovedEventHandler;
		readonly List<IDMMessageReceivedEventHandler> DMMessageReceivedEventHandler;
		readonly List<IDMMessageUpdatedEventHandler> DMMessageUpdatedEventHandler;
		readonly List<IDMMessageDeletedEventHandler> DMMessageDeletedEventHandler;
		readonly List<IDMMessageReactionAddedEventHandler> DMMessageReactionAddedEventHandler;
		readonly List<IDMMessageReactionRemovedEventHandler> DMMessageReactionRemovedEventHandler;
		readonly List<IDMMessageReactionsClearedEventHandler> DMMessageReactionsClearedEventHandler;
		readonly List<IGuildAvailableEventHandler> GuildAvailableEventHandler;
		readonly List<IGuildUpdatedEventHandler> GuildUpdatedEventHandler;
		readonly List<IGuildUnavailableEventHandler> GuildUnavailableEventHandler;
		readonly List<IGuildMembersDownloadedEventHandler> GuildMembersDownloadedEventHandler;
		readonly List<IGuildMemberUpdatedEventHandler> GuildMemberUpdatedEventHandler;
		readonly List<IGuildUserJoinedEventHandler> GuildUserJoinedEventHandler;
		readonly List<IGuildUserLeftEventHandler> GuildUserLeftEventHandler;
		readonly List<IGuildUserBannedEventHandler> GuildUserBannedEventHandler;
		readonly List<IGuildUserUnbannedEventHandler> GuildUserUnbannedEventHandler;
		readonly List<IGuildUserUpdatedEventHandler> GuildUserUpdatedEventHandler;
		readonly List<IGuildUserTypesEventHandler> GuildUserTypesEventHandler;
		readonly List<IGuildRoleCreatedEventHandler> GuildRoleCreatedEventHandler;
		readonly List<IGuildRoleUpdatedEventHandler> GuildRoleUpdatedEventHandler;
		readonly List<IGuildRoleDeletedEventHandler> GuildRoleDeletedEventHandler;
		readonly List<IGuildChannelCreatedEventHandler> GuildChannelCreatedEventHandler;
		readonly List<IGuildChannelUpdatedEventHandler> GuildChannelUpdatedEventHandler;
		readonly List<IGuildChannelDestroyedEventHandler> GuildChannelDestroyedEventHandler;
		readonly List<IGuildMessageReceivedEventHandler> GuildMessageReceivedEventHandler;
		readonly List<IGuildMessageUpdatedEventHandler> GuildMessageUpdatedEventHandler;
		readonly List<IGuildMessageDeletedEventHandler> GuildMessageDeletedEventHandler;
		readonly List<IGuildMessageReactionAddedEventHandler> GuildMessageReactionAddedEventHandler;
		readonly List<IGuildMessageReactionRemovedEventHandler> GuildMessageReactionRemovedEventHandler;
		readonly List<IGuildMessageReactionsClearedEventHandler> GuildMessageReactionsClearedEventHandler;

		public EventHandlersManager(DiscordSocketClient client)
		{
			this.SelfConnectedEventHandler = new List<ISelfConnectedEventHandler>();
			this.SelfReadyEventHandler = new List<ISelfReadyEventHandler>();
			this.SelfDisconnectedEventHandler = new List<ISelfDisconnectedEventHandler>();
			this.SelfJoinedGuildEventHandler = new List<ISelfJoinedGuildEventHandler>();
			this.SelfLeftGuildEventHandler = new List<ISelfLeftGuildEventHandler>();
			this.SelfCurrentUserUpdatedEventHandler = new List<ISelfCurrentUserUpdatedEventHandler>();
			this.SelfLatencyUpdatedEventHandler = new List<ISelfLatencyUpdatedEventHandler>();
			this.DMUserUpdatedEventHandler = new List<IDMUserUpdatedEventHandler>();
			this.DMUserTypesEventHandler = new List<IDMUserTypesEventHandler>();
			this.DMUserVoiceStateUpdatedEventHandler = new List<IDMUserVoiceStateUpdatedEventHandler>();
			this.DMRecipientAddedEventHandler = new List<IDMRecipientAddedEventHandler>();
			this.DMRecipientRemovedEventHandler = new List<IDMRecipientRemovedEventHandler>();
			this.DMMessageReceivedEventHandler = new List<IDMMessageReceivedEventHandler>();
			this.DMMessageUpdatedEventHandler = new List<IDMMessageUpdatedEventHandler>();
			this.DMMessageDeletedEventHandler = new List<IDMMessageDeletedEventHandler>();
			this.DMMessageReactionAddedEventHandler = new List<IDMMessageReactionAddedEventHandler>();
			this.DMMessageReactionRemovedEventHandler = new List<IDMMessageReactionRemovedEventHandler>();
			this.DMMessageReactionsClearedEventHandler = new List<IDMMessageReactionsClearedEventHandler>();
			this.GuildAvailableEventHandler = new List<IGuildAvailableEventHandler>();
			this.GuildUpdatedEventHandler = new List<IGuildUpdatedEventHandler>();
			this.GuildUnavailableEventHandler = new List<IGuildUnavailableEventHandler>();
			this.GuildMembersDownloadedEventHandler = new List<IGuildMembersDownloadedEventHandler>();
			this.GuildMemberUpdatedEventHandler = new List<IGuildMemberUpdatedEventHandler>();
			this.GuildUserJoinedEventHandler = new List<IGuildUserJoinedEventHandler>();
			this.GuildUserLeftEventHandler = new List<IGuildUserLeftEventHandler>();
			this.GuildUserBannedEventHandler = new List<IGuildUserBannedEventHandler>();
			this.GuildUserUnbannedEventHandler = new List<IGuildUserUnbannedEventHandler>();
			this.GuildUserUpdatedEventHandler = new List<IGuildUserUpdatedEventHandler>();
			this.GuildUserTypesEventHandler = new List<IGuildUserTypesEventHandler>();
			this.GuildRoleCreatedEventHandler = new List<IGuildRoleCreatedEventHandler>();
			this.GuildRoleUpdatedEventHandler = new List<IGuildRoleUpdatedEventHandler>();
			this.GuildRoleDeletedEventHandler = new List<IGuildRoleDeletedEventHandler>();
			this.GuildChannelCreatedEventHandler = new List<IGuildChannelCreatedEventHandler>();
			this.GuildChannelUpdatedEventHandler = new List<IGuildChannelUpdatedEventHandler>();
			this.GuildChannelDestroyedEventHandler = new List<IGuildChannelDestroyedEventHandler>();
			this.GuildMessageReceivedEventHandler = new List<IGuildMessageReceivedEventHandler>();
			this.GuildMessageUpdatedEventHandler = new List<IGuildMessageUpdatedEventHandler>();
			this.GuildMessageDeletedEventHandler = new List<IGuildMessageDeletedEventHandler>();
			this.GuildMessageReactionAddedEventHandler = new List<IGuildMessageReactionAddedEventHandler>();
			this.GuildMessageReactionRemovedEventHandler = new List<IGuildMessageReactionRemovedEventHandler>();
			this.GuildMessageReactionsClearedEventHandler = new List<IGuildMessageReactionsClearedEventHandler>();

			/*  Self */
			client.Connected += Self_Connected;
			client.Ready += Self_Ready;
			client.Disconnected += Self_Disconnected;
			client.JoinedGuild += Self_JoinedGuild;
			client.LeftGuild += Self_LeftGuild;
			client.CurrentUserUpdated += Self_CurrentUserUpdated;
			client.LatencyUpdated += Self_LatencyUpdated;

			/* Guild */
			client.GuildAvailable += Guild_Available;
			client.GuildUpdated += Guild_Updated;
			client.GuildUnavailable += Guild_Unavailable;
			client.GuildMembersDownloaded += Guild_MembersDownloaded;
			client.GuildMemberUpdated += Guild_MemberUpdated;

			/* Guild > Role */
			client.RoleCreated += Guild_Role_Created;
			client.RoleUpdated += Guild_Role_Updated;
			client.RoleDeleted += Guild_Role_Deleted;

			/* Guild > Channel */
			client.ChannelCreated += Guild_Channel_Created;
			client.ChannelUpdated += Guild_Channel_Updated;
			client.ChannelDestroyed += Guild_Channel_Destroyed;

			/* Guild|DM > Message */
			client.MessageReceived += Message_Received;
			client.MessageUpdated += Message_Updated;
			client.MessageDeleted += Message_Deleted;

			/* Guild|DM > Message */
			client.ReactionAdded += Message_ReactionAdded;
			client.ReactionRemoved += Message_ReactionRemoved;
			client.ReactionsCleared += Message_ReactionsCleared;

			/* DM */
			client.RecipientAdded += DM_RecipientAdded; // DM Group gaining member
			client.RecipientRemoved += DM_RecipientRemoved; // DM Group losing member}

			/* Guild > User */
			client.UserBanned += Guild_User_Banned;
			client.UserUnbanned += Guild_User_Unbanned;
			client.UserJoined += Guild_User_Joined;
			client.UserLeft += Guild_User_Left;

			/* Guild|DM > User */
			client.UserIsTyping += User_Types;
			client.UserUpdated += User_Updated;
			client.UserVoiceStateUpdated += User_VoiceStateUpdated;
		}

		public void RemoveEvents(DiscordSocketClient client)
		{
			/*  Self */
			client.Connected -= Self_Connected;
			client.Ready -= Self_Ready;
			client.Disconnected -= Self_Disconnected;
			client.JoinedGuild -= Self_JoinedGuild;
			client.LeftGuild -= Self_LeftGuild;
			client.CurrentUserUpdated -= Self_CurrentUserUpdated;
			client.LatencyUpdated -= Self_LatencyUpdated;

			/* Guild */
			client.GuildAvailable -= Guild_Available;
			client.GuildUpdated -= Guild_Updated;
			client.GuildUnavailable -= Guild_Unavailable;
			client.GuildMembersDownloaded -= Guild_MembersDownloaded;
			client.GuildMemberUpdated -= Guild_MemberUpdated;

			/* Guild > Role */
			client.RoleCreated -= Guild_Role_Created;
			client.RoleUpdated -= Guild_Role_Updated;
			client.RoleDeleted -= Guild_Role_Deleted;

			/* Guild > Channel */
			client.ChannelCreated -= Guild_Channel_Created;
			client.ChannelUpdated -= Guild_Channel_Updated;
			client.ChannelDestroyed -= Guild_Channel_Destroyed;

			/* Guild|DM > Message */
			client.MessageReceived -= Message_Received;
			client.MessageUpdated -= Message_Updated;
			client.MessageDeleted -= Message_Deleted;

			/* Guild|DM > Message */
			client.ReactionAdded -= Message_ReactionAdded;
			client.ReactionRemoved -= Message_ReactionRemoved;
			client.ReactionsCleared -= Message_ReactionsCleared;

			/* DM */
			client.RecipientAdded -= DM_RecipientAdded; // DM Group gaining member
			client.RecipientRemoved -= DM_RecipientRemoved; // DM Group losing member}

			/* Guild > User */
			client.UserBanned -= Guild_User_Banned;
			client.UserUnbanned -= Guild_User_Unbanned;
			client.UserJoined -= Guild_User_Joined;
			client.UserLeft -= Guild_User_Left;

			/* Guild|DM > User */
			client.UserIsTyping -= User_Types;
			client.UserUpdated -= User_Updated;
			client.UserVoiceStateUpdated -= User_VoiceStateUpdated;
		}

		public void AddHandlers(params IEventHandler[] handlers)
		{
			foreach (IEventHandler handler in handlers) {
				if (handler is ISelfConnectedEventHandler) { this.SelfConnectedEventHandler.Add(handler as ISelfConnectedEventHandler); }
				if (handler is ISelfReadyEventHandler) { this.SelfReadyEventHandler.Add(handler as ISelfReadyEventHandler); }
				if (handler is ISelfDisconnectedEventHandler) { this.SelfDisconnectedEventHandler.Add(handler as ISelfDisconnectedEventHandler); }
				if (handler is ISelfJoinedGuildEventHandler) { this.SelfJoinedGuildEventHandler.Add(handler as ISelfJoinedGuildEventHandler); }
				if (handler is ISelfLeftGuildEventHandler) { this.SelfLeftGuildEventHandler.Add(handler as ISelfLeftGuildEventHandler); }
				if (handler is ISelfCurrentUserUpdatedEventHandler) { this.SelfCurrentUserUpdatedEventHandler.Add(handler as ISelfCurrentUserUpdatedEventHandler); }
				if (handler is ISelfLatencyUpdatedEventHandler) { this.SelfLatencyUpdatedEventHandler.Add(handler as ISelfLatencyUpdatedEventHandler); }
				if (handler is IDMUserUpdatedEventHandler) { this.DMUserUpdatedEventHandler.Add(handler as IDMUserUpdatedEventHandler); }
				if (handler is IDMUserTypesEventHandler) { this.DMUserTypesEventHandler.Add(handler as IDMUserTypesEventHandler); }
				if (handler is IDMUserVoiceStateUpdatedEventHandler) { this.DMUserVoiceStateUpdatedEventHandler.Add(handler as IDMUserVoiceStateUpdatedEventHandler); }
				if (handler is IDMRecipientAddedEventHandler) { this.DMRecipientAddedEventHandler.Add(handler as IDMRecipientAddedEventHandler); }
				if (handler is IDMRecipientRemovedEventHandler) { this.DMRecipientRemovedEventHandler.Add(handler as IDMRecipientRemovedEventHandler); }
				if (handler is IDMMessageReceivedEventHandler) { this.DMMessageReceivedEventHandler.Add(handler as IDMMessageReceivedEventHandler); }
				if (handler is IDMMessageUpdatedEventHandler) { this.DMMessageUpdatedEventHandler.Add(handler as IDMMessageUpdatedEventHandler); }
				if (handler is IDMMessageDeletedEventHandler) { this.DMMessageDeletedEventHandler.Add(handler as IDMMessageDeletedEventHandler); }
				if (handler is IDMMessageReactionAddedEventHandler) { this.DMMessageReactionAddedEventHandler.Add(handler as IDMMessageReactionAddedEventHandler); }
				if (handler is IDMMessageReactionRemovedEventHandler) { this.DMMessageReactionRemovedEventHandler.Add(handler as IDMMessageReactionRemovedEventHandler); }
				if (handler is IDMMessageReactionsClearedEventHandler) { this.DMMessageReactionsClearedEventHandler.Add(handler as IDMMessageReactionsClearedEventHandler); }
				if (handler is IGuildAvailableEventHandler) { this.GuildAvailableEventHandler.Add(handler as IGuildAvailableEventHandler); }
				if (handler is IGuildUpdatedEventHandler) { this.GuildUpdatedEventHandler.Add(handler as IGuildUpdatedEventHandler); }
				if (handler is IGuildUnavailableEventHandler) { this.GuildUnavailableEventHandler.Add(handler as IGuildUnavailableEventHandler); }
				if (handler is IGuildMembersDownloadedEventHandler) { this.GuildMembersDownloadedEventHandler.Add(handler as IGuildMembersDownloadedEventHandler); }
				if (handler is IGuildMemberUpdatedEventHandler) { this.GuildMemberUpdatedEventHandler.Add(handler as IGuildMemberUpdatedEventHandler); }
				if (handler is IGuildUserJoinedEventHandler) { this.GuildUserJoinedEventHandler.Add(handler as IGuildUserJoinedEventHandler); }
				if (handler is IGuildUserLeftEventHandler) { this.GuildUserLeftEventHandler.Add(handler as IGuildUserLeftEventHandler); }
				if (handler is IGuildUserBannedEventHandler) { this.GuildUserBannedEventHandler.Add(handler as IGuildUserBannedEventHandler); }
				if (handler is IGuildUserUnbannedEventHandler) { this.GuildUserUnbannedEventHandler.Add(handler as IGuildUserUnbannedEventHandler); }
				if (handler is IGuildUserUpdatedEventHandler) { this.GuildUserUpdatedEventHandler.Add(handler as IGuildUserUpdatedEventHandler); }
				if (handler is IGuildUserTypesEventHandler) { this.GuildUserTypesEventHandler.Add(handler as IGuildUserTypesEventHandler); }
				if (handler is IGuildRoleCreatedEventHandler) { this.GuildRoleCreatedEventHandler.Add(handler as IGuildRoleCreatedEventHandler); }
				if (handler is IGuildRoleUpdatedEventHandler) { this.GuildRoleUpdatedEventHandler.Add(handler as IGuildRoleUpdatedEventHandler); }
				if (handler is IGuildRoleDeletedEventHandler) { this.GuildRoleDeletedEventHandler.Add(handler as IGuildRoleDeletedEventHandler); }
				if (handler is IGuildChannelCreatedEventHandler) { this.GuildChannelCreatedEventHandler.Add(handler as IGuildChannelCreatedEventHandler); }
				if (handler is IGuildChannelUpdatedEventHandler) { this.GuildChannelUpdatedEventHandler.Add(handler as IGuildChannelUpdatedEventHandler); }
				if (handler is IGuildChannelDestroyedEventHandler) { this.GuildChannelDestroyedEventHandler.Add(handler as IGuildChannelDestroyedEventHandler); }
				if (handler is IGuildMessageReceivedEventHandler) { this.GuildMessageReceivedEventHandler.Add(handler as IGuildMessageReceivedEventHandler); }
				if (handler is IGuildMessageUpdatedEventHandler) { this.GuildMessageUpdatedEventHandler.Add(handler as IGuildMessageUpdatedEventHandler); }
				if (handler is IGuildMessageDeletedEventHandler) { this.GuildMessageDeletedEventHandler.Add(handler as IGuildMessageDeletedEventHandler); }
				if (handler is IGuildMessageReactionAddedEventHandler) { this.GuildMessageReactionAddedEventHandler.Add(handler as IGuildMessageReactionAddedEventHandler); }
				if (handler is IGuildMessageReactionRemovedEventHandler) { this.GuildMessageReactionRemovedEventHandler.Add(handler as IGuildMessageReactionRemovedEventHandler); }
				if (handler is IGuildMessageReactionsClearedEventHandler) { this.GuildMessageReactionsClearedEventHandler.Add(handler as IGuildMessageReactionsClearedEventHandler); }
			}
		}

		public Task Self_Connected()
		{
			foreach (ISelfConnectedEventHandler handler in this.SelfConnectedEventHandler) { new Thread(() => handler.Self_Connected()).Start(); }
			return Task.CompletedTask;
		}

		public Task Self_Ready()
		{
			foreach (ISelfReadyEventHandler handler in this.SelfReadyEventHandler) { new Thread(() => handler.Self_Ready()).Start(); }
			return Task.CompletedTask;
		}

		public Task Self_Disconnected(Exception exception)
		{
			foreach (ISelfDisconnectedEventHandler handler in this.SelfDisconnectedEventHandler) { new Thread(() => handler.Self_Disconnected(exception)).Start(); }
			return Task.CompletedTask;
		}

		public Task Self_JoinedGuild(SocketGuild guild)
		{
			foreach (ISelfJoinedGuildEventHandler handler in this.SelfJoinedGuildEventHandler) { new Thread(() => handler.Self_JoinedGuild(guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Self_LeftGuild(SocketGuild guild)
		{
			foreach (ISelfLeftGuildEventHandler handler in this.SelfLeftGuildEventHandler) { new Thread(() => handler.Self_LeftGuild(guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Self_CurrentUserUpdated(SocketSelfUser old_user, SocketSelfUser new_user)
		{
			foreach (ISelfCurrentUserUpdatedEventHandler handler in this.SelfCurrentUserUpdatedEventHandler) { new Thread(() => handler.Self_CurrentUserUpdated(old_user, new_user)).Start(); }
			return Task.CompletedTask;
		}

		public Task Self_LatencyUpdated(int old_value, int new_value)
		{
			foreach (ISelfLatencyUpdatedEventHandler handler in this.SelfLatencyUpdatedEventHandler) { new Thread(() => handler.Self_LatencyUpdated(old_value, new_value)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_User_Updated(SocketUser old_user, SocketUser new_user)
		{
			foreach (IDMUserUpdatedEventHandler handler in this.DMUserUpdatedEventHandler) { new Thread(() => handler.DM_User_Updated(old_user, new_user)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_User_Types(SocketUser user, ISocketPrivateChannel channel)
		{
			foreach (IDMUserTypesEventHandler handler in this.DMUserTypesEventHandler) { new Thread(() => handler.DM_User_Types(user, channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_User_VoiceStateUpdated(SocketUser user, SocketVoiceState old_voicestate, SocketVoiceState new_voicestate)
		{
			foreach (IDMUserVoiceStateUpdatedEventHandler handler in this.DMUserVoiceStateUpdatedEventHandler) { new Thread(() => handler.DM_User_VoiceStateUpdated(user, old_voicestate, new_voicestate)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_RecipientAdded(SocketGroupUser user)
		{
			foreach (IDMRecipientAddedEventHandler handler in this.DMRecipientAddedEventHandler) { new Thread(() => handler.DM_RecipientAdded(user)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_RecipientRemoved(SocketGroupUser user)
		{
			foreach (IDMRecipientRemovedEventHandler handler in this.DMRecipientRemovedEventHandler) { new Thread(() => handler.DM_RecipientRemoved(user)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_Message_Received(SocketUserMessage message)
		{
			foreach (IDMMessageReceivedEventHandler handler in this.DMMessageReceivedEventHandler) { new Thread(() => handler.DM_Message_Received(message)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_Message_Updated(Cacheable<IMessage, ulong> cache_message, SocketUserMessage new_message, ISocketMessageChannel channel)
		{
			foreach (IDMMessageUpdatedEventHandler handler in this.DMMessageUpdatedEventHandler) { new Thread(() => handler.DM_Message_Updated(cache_message, new_message, channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_Message_Deleted(Cacheable<IMessage, ulong> cache_message, ISocketMessageChannel channel)
		{
			foreach (IDMMessageDeletedEventHandler handler in this.DMMessageDeletedEventHandler) { new Thread(() => handler.DM_Message_Deleted(cache_message, channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_Message_ReactionAdded(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			foreach (IDMMessageReactionAddedEventHandler handler in this.DMMessageReactionAddedEventHandler) { new Thread(() => handler.DM_Message_ReactionAdded(cached_message, channel, reaction)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_Message_ReactionRemoved(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			foreach (IDMMessageReactionRemovedEventHandler handler in this.DMMessageReactionRemovedEventHandler) { new Thread(() => handler.DM_Message_ReactionRemoved(cached_message, channel, reaction)).Start(); }
			return Task.CompletedTask;
		}

		public Task DM_Message_ReactionsCleared(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel)
		{
			foreach (IDMMessageReactionsClearedEventHandler handler in this.DMMessageReactionsClearedEventHandler) { new Thread(() => handler.DM_Message_ReactionsCleared(cached_message, channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Available(SocketGuild guild)
		{
			foreach (IGuildAvailableEventHandler handler in this.GuildAvailableEventHandler) { new Thread(() => handler.Guild_Available(guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Updated(SocketGuild old_guild, SocketGuild new_guild)
		{
			foreach (IGuildUpdatedEventHandler handler in this.GuildUpdatedEventHandler) { new Thread(() => handler.Guild_Updated(old_guild, new_guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Unavailable(SocketGuild guild)
		{
			foreach (IGuildUnavailableEventHandler handler in this.GuildUnavailableEventHandler) { new Thread(() => handler.Guild_Unavailable(guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_MembersDownloaded(SocketGuild guild)
		{
			foreach (IGuildMembersDownloadedEventHandler handler in this.GuildMembersDownloadedEventHandler) { new Thread(() => handler.Guild_MembersDownloaded(guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_MemberUpdated(SocketGuildUser old_user, SocketGuildUser new_user)
		{
			foreach (IGuildMemberUpdatedEventHandler handler in this.GuildMemberUpdatedEventHandler) { new Thread(() => handler.Guild_MemberUpdated(old_user, new_user)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_User_Joined(SocketGuildUser user)
		{
			foreach (IGuildUserJoinedEventHandler handler in this.GuildUserJoinedEventHandler) { new Thread(() => handler.Guild_User_Joined(user)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_User_Left(SocketGuildUser user)
		{
			foreach (IGuildUserLeftEventHandler handler in this.GuildUserLeftEventHandler) { new Thread(() => handler.Guild_User_Left(user)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_User_Banned(SocketUser user, SocketGuild guild)
		{
			foreach (IGuildUserBannedEventHandler handler in this.GuildUserBannedEventHandler) { new Thread(() => handler.Guild_User_Banned(user, guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_User_Unbanned(SocketUser user, SocketGuild guild)
		{
			foreach (IGuildUserUnbannedEventHandler handler in this.GuildUserUnbannedEventHandler) { new Thread(() => handler.Guild_User_Unbanned(user, guild)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_User_Updated(SocketUser old_user, SocketUser new_user)
		{
			foreach (IGuildUserUpdatedEventHandler handler in this.GuildUserUpdatedEventHandler) { new Thread(() => handler.Guild_User_Updated(old_user, new_user)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_User_Types(SocketUser user, SocketTextChannel channel)
		{
			foreach (IGuildUserTypesEventHandler handler in this.GuildUserTypesEventHandler) { new Thread(() => handler.Guild_User_Types(user, channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Role_Created(SocketRole role)
		{
			foreach (IGuildRoleCreatedEventHandler handler in this.GuildRoleCreatedEventHandler) { new Thread(() => handler.Guild_Role_Created(role)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Role_Updated(SocketRole old_role, SocketRole new_role)
		{
			foreach (IGuildRoleUpdatedEventHandler handler in this.GuildRoleUpdatedEventHandler) { new Thread(() => handler.Guild_Role_Updated(old_role, new_role)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Role_Deleted(SocketRole role)
		{
			foreach (IGuildRoleDeletedEventHandler handler in this.GuildRoleDeletedEventHandler) { new Thread(() => handler.Guild_Role_Deleted(role)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Channel_Created(SocketChannel channel)
		{
			foreach (IGuildChannelCreatedEventHandler handler in this.GuildChannelCreatedEventHandler) { new Thread(() => handler.Guild_Channel_Created(channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Channel_Updated(SocketChannel old_channel, SocketChannel new_channel)
		{
			foreach (IGuildChannelUpdatedEventHandler handler in this.GuildChannelUpdatedEventHandler) { new Thread(() => handler.Guild_Channel_Updated(old_channel, new_channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Channel_Destroyed(SocketChannel channel)
		{
			foreach (IGuildChannelDestroyedEventHandler handler in this.GuildChannelDestroyedEventHandler) { new Thread(() => handler.Guild_Channel_Destroyed(channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Message_Received(SocketUserMessage message)
		{
			foreach (IGuildMessageReceivedEventHandler handler in this.GuildMessageReceivedEventHandler) { new Thread(() => handler.Guild_Message_Received(message)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Message_Updated(Cacheable<IMessage, ulong> cache_message, SocketUserMessage new_message, ISocketMessageChannel channel)
		{
			foreach (IGuildMessageUpdatedEventHandler handler in this.GuildMessageUpdatedEventHandler) { new Thread(() => handler.Guild_Message_Updated(cache_message, new_message, channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Message_Deleted(Cacheable<IMessage, ulong> cache_message, ISocketMessageChannel channel)
		{
			foreach (IGuildMessageDeletedEventHandler handler in this.GuildMessageDeletedEventHandler) { new Thread(() => handler.Guild_Message_Deleted(cache_message, channel)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Message_ReactionAdded(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			foreach (IGuildMessageReactionAddedEventHandler handler in this.GuildMessageReactionAddedEventHandler) { new Thread(() => handler.Guild_Message_ReactionAdded(cached_message, channel, reaction)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Message_ReactionRemoved(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			foreach (IGuildMessageReactionRemovedEventHandler handler in this.GuildMessageReactionRemovedEventHandler) { new Thread(() => handler.Guild_Message_ReactionRemoved(cached_message, channel, reaction)).Start(); }
			return Task.CompletedTask;
		}

		public Task Guild_Message_ReactionsCleared(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel)
		{
			foreach (IGuildMessageReactionsClearedEventHandler handler in this.GuildMessageReactionsClearedEventHandler) { new Thread(() => handler.Guild_Message_ReactionsCleared(cached_message, channel)).Start(); }
			return Task.CompletedTask;
		}


		public async Task Message_Received(SocketMessage message)
		{
			if (!(message is SocketUserMessage) || message.Author.Id == Data.Users.Bot.Id || Data.DataManager.baned_people.Contains(message.Author.Id)) { return; }
			SocketUserMessage user_message = message as SocketUserMessage;

			if (user_message.Channel is SocketGuildChannel) { await Guild_Message_Received(user_message); }
			else { await DM_Message_Received(user_message); }
		}

		public async Task Message_Updated(Cacheable<IMessage, ulong> cached_message, SocketMessage new_message, ISocketMessageChannel channel)
		{
			if (!(new_message is SocketUserMessage)) { return; }
			SocketUserMessage user_message = new_message as SocketUserMessage;
			if (user_message.Channel is SocketGuildChannel) { await Guild_Message_Updated(cached_message, user_message, channel); }
			else { await DM_Message_Updated(cached_message, user_message, channel); }
		}

		public async Task Message_Deleted(Cacheable<IMessage, ulong> cached_message, ISocketMessageChannel channel)
		{
			if (channel is SocketGuildChannel) { await Guild_Message_Deleted(cached_message, channel); }
			else { await DM_Message_Deleted(cached_message, channel); }
		}

		public async Task Message_ReactionAdded(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			if (Data.DataManager.baned_people.Contains(reaction.User.Value.Id) || reaction.User.Value.Id == Data.Users.Bot.Id) { return; }
			if (channel is SocketGuildChannel) { await Guild_Message_ReactionAdded(cached_message, channel, reaction); }
			else { await DM_Message_ReactionAdded(cached_message, channel, reaction); }
		}

		public async Task Message_ReactionRemoved(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			if (channel is SocketGuildChannel) { await Guild_Message_ReactionRemoved(cached_message, channel, reaction); }
			else { await DM_Message_ReactionRemoved(cached_message, channel, reaction); }
		}

		public async Task Message_ReactionsCleared(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel)
		{
			if (channel is SocketGuildChannel) { await Guild_Message_ReactionsCleared(cached_message, channel); }
			else { await DM_Message_ReactionsCleared(cached_message, channel); }
		}

		public async Task User_Updated(SocketUser old_user, SocketUser new_user)
		{
			if (new_user is SocketGuildUser) { await Guild_User_Updated(old_user, new_user); }
			else { await DM_User_Updated(old_user, new_user); }
		}

		public async Task User_VoiceStateUpdated(SocketUser user, SocketVoiceState old_voicestate, SocketVoiceState new_voicestate)
		{
			await DM_User_VoiceStateUpdated(user, old_voicestate, new_voicestate);
		}

		public async Task User_Types(SocketUser user, ISocketMessageChannel channel)
		{
			if (channel is SocketGuildChannel) { await Guild_User_Types(user, channel as SocketTextChannel); }
			else { await DM_User_Types(user, channel as ISocketPrivateChannel); }
		}
	}
}
