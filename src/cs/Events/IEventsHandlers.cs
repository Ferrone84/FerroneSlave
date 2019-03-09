using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Events
{
	public interface IEventHandler { }

	//ISelfEventsHandler
	public interface ISelfConnectedEventHandler : IEventHandler { Task Self_Connected(); }
	public interface ISelfReadyEventHandler : IEventHandler { Task Self_Ready(); }
	public interface ISelfDisconnectedEventHandler : IEventHandler { Task Self_Disconnected(Exception exception); }
	public interface ISelfJoinedGuildEventHandler : IEventHandler { Task Self_JoinedGuild(SocketGuild guild); }
	public interface ISelfLeftGuildEventHandler : IEventHandler { Task Self_LeftGuild(SocketGuild guild); }
	public interface ISelfCurrentUserUpdatedEventHandler : IEventHandler { Task Self_CurrentUserUpdated(SocketSelfUser old_user, SocketSelfUser new_user); }
	public interface ISelfLatencyUpdatedEventHandler : IEventHandler { Task Self_LatencyUpdated(int old_value, int new_value); }

	//IDMUserEventsHandler
	public interface IDMUserUpdatedEventHandler : IEventHandler { Task DM_User_Updated(SocketUser old_user, SocketUser new_user); }
	public interface IDMUserTypesEventHandler : IEventHandler { Task DM_User_Types(SocketUser user, ISocketPrivateChannel channel); }
	public interface IDMUserVoiceStateUpdatedEventHandler : IEventHandler { Task DM_User_VoiceStateUpdated(SocketUser user, SocketVoiceState old_voicestate, SocketVoiceState new_voicestate); }

	//IDMChannelEventsHandler
	public interface IDMRecipientAddedEventHandler : IEventHandler { Task DM_RecipientAdded(SocketGroupUser user); }
	public interface IDMRecipientRemovedEventHandler : IEventHandler { Task DM_RecipientRemoved(SocketGroupUser user); }

	//IDMMessageEventsHandler
	public interface IDMMessageReceivedEventHandler : IEventHandler { Task DM_Message_Received(SocketUserMessage message); }
	public interface IDMMessageUpdatedEventHandler : IEventHandler { Task DM_Message_Updated(Cacheable<IMessage, ulong> cache_message, SocketUserMessage new_message, ISocketMessageChannel channel); }
	public interface IDMMessageDeletedEventHandler : IEventHandler { Task DM_Message_Deleted(Cacheable<IMessage, ulong> cache_message, ISocketMessageChannel channel); }
	public interface IDMMessageReactionAddedEventHandler : IEventHandler { Task DM_Message_ReactionAdded(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction); }
	public interface IDMMessageReactionRemovedEventHandler : IEventHandler { Task DM_Message_ReactionRemoved(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction); }
	public interface IDMMessageReactionsClearedEventHandler : IEventHandler { Task DM_Message_ReactionsCleared(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel); }

	//IGuildEventsHandler
	public interface IGuildAvailableEventHandler : IEventHandler { Task Guild_Available(SocketGuild guild); }
	public interface IGuildUpdatedEventHandler : IEventHandler { Task Guild_Updated(SocketGuild old_guild, SocketGuild new_guild); }
	public interface IGuildUnavailableEventHandler : IEventHandler { Task Guild_Unavailable(SocketGuild guild); }
	public interface IGuildMembersDownloadedEventHandler : IEventHandler { Task Guild_MembersDownloaded(SocketGuild guild); }
	public interface IGuildMemberUpdatedEventHandler : IEventHandler { Task Guild_MemberUpdated(SocketGuildUser old_user, SocketGuildUser new_user); }

	//IGuildUserEventsHandler
	public interface IGuildUserJoinedEventHandler : IEventHandler { Task Guild_User_Joined(SocketGuildUser user); }
	public interface IGuildUserLeftEventHandler : IEventHandler { Task Guild_User_Left(SocketGuildUser user); }
	public interface IGuildUserBannedEventHandler : IEventHandler { Task Guild_User_Banned(SocketUser user, SocketGuild guild); }
	public interface IGuildUserUnbannedEventHandler : IEventHandler { Task Guild_User_Unbanned(SocketUser user, SocketGuild guild); }
	public interface IGuildUserUpdatedEventHandler : IEventHandler { Task Guild_User_Updated(SocketUser old_user, SocketUser new_user); }
	public interface IGuildUserTypesEventHandler : IEventHandler { Task Guild_User_Types(SocketUser user, SocketTextChannel channel); }

	//IGuildRoleEventsHandler
	public interface IGuildRoleCreatedEventHandler : IEventHandler { Task Guild_Role_Created(SocketRole role); }
	public interface IGuildRoleUpdatedEventHandler : IEventHandler { Task Guild_Role_Updated(SocketRole old_role, SocketRole new_role); }
	public interface IGuildRoleDeletedEventHandler : IEventHandler { Task Guild_Role_Deleted(SocketRole role); }

	//IGuildChannelEventsHandler
	public interface IGuildChannelCreatedEventHandler : IEventHandler { Task Guild_Channel_Created(SocketChannel channel); }
	public interface IGuildChannelUpdatedEventHandler : IEventHandler { Task Guild_Channel_Updated(SocketChannel old_channel, SocketChannel new_channel); }
	public interface IGuildChannelDestroyedEventHandler : IEventHandler { Task Guild_Channel_Destroyed(SocketChannel channel); }

	//IGuildMessageEventsHandler
	public interface IGuildMessageReceivedEventHandler : IEventHandler { Task Guild_Message_Received(SocketUserMessage message); }
	public interface IGuildMessageUpdatedEventHandler : IEventHandler { Task Guild_Message_Updated(Cacheable<IMessage, ulong> cache_message, SocketUserMessage new_message, ISocketMessageChannel channel); }
	public interface IGuildMessageDeletedEventHandler : IEventHandler { Task Guild_Message_Deleted(Cacheable<IMessage, ulong> cache_message, ISocketMessageChannel channel); }
	public interface IGuildMessageReactionAddedEventHandler : IEventHandler { Task Guild_Message_ReactionAdded(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction); }
	public interface IGuildMessageReactionRemovedEventHandler : IEventHandler { Task Guild_Message_ReactionRemoved(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel, SocketReaction reaction); }
	public interface IGuildMessageReactionsClearedEventHandler : IEventHandler { Task Guild_Message_ReactionsCleared(Cacheable<IUserMessage, ulong> cached_message, ISocketMessageChannel channel); }
}