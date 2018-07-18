using System;
using System.IO;


namespace DiscordBot
{
	class BotStateManager
	{
		public static void Save<T>(string filename, T obj)
		{
			using (Stream stream = File.Open(filename, FileMode.Create))
			{
				var binary_serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				binary_serializer.Serialize(stream, obj);
			}
		}
		public static T Load<T>(string filename)
		{
			using (Stream stream = File.Open(filename, FileMode.Open))
			{
				var binary_serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				return (T)binary_serializer.Deserialize(stream);
			}
		}
	}
}