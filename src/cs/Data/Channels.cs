
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Data
{
	public class Channels
	{
		public static IMessageChannel General => DataManager._client.GetChannel(309407896070782976) as SocketTextChannel ?? Tests;
		public static IMessageChannel Mangas => DataManager._client.GetChannel(439960408703369217) as SocketTextChannel ?? Tests;
		public static IMessageChannel Musique => DataManager._client.GetChannel(472354528948387857) as SocketTextChannel ?? Tests;
		public static IMessageChannel Debug => DataManager._client.GetChannel(353262627880697868) as SocketTextChannel ?? Tests;
		public static IMessageChannel Debugs => DataManager._client.GetChannel(456443420378923010) as SocketTextChannel ?? Tests;
		public static IMessageChannel Zone51 => DataManager._client.GetChannel(346760327540506643) as SocketTextChannel ?? Tests;
		public static IMessageChannel Nsfw => DataManager._client.GetChannel(389537278671978497) as SocketTextChannel ?? Tests;
		public static IMessageChannel Problems => DataManager._client.GetChannel(554978094117814272) as SocketTextChannel ?? Tests;
		public static IMessageChannel Logs => DataManager._client.GetChannel(553882320856416256) as SocketTextChannel ?? Tests_Logs;
		public static IMessageChannel Qwertee => DataManager._client.GetChannel(648216097404878889) as SocketTextChannel ?? Tests;

		public static IMessageChannel Tests => DataManager._client.GetChannel(543925483650416650) as SocketTextChannel;
		public static IMessageChannel Tests_Logs => DataManager._client.GetChannel(550798135165976578) as SocketTextChannel;
	}
}
