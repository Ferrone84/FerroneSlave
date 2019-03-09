using System;
using System.IO;

namespace DiscordBot.Utilities
{
	public class SaveStateManager
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
			try {
				using (Stream stream = File.Open(filename, FileMode.Open)) {
					var binary_serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					return (T)binary_serializer.Deserialize(stream);
				}
			}
			catch (Exception) {
				("Le fichier "+filename+" n'existe pas. Valeur par défaut renvoyé dans l'instance.").Println();
				return default(T);
			}
		}
	}
}
