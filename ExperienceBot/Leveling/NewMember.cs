namespace ExperienceBot.Leveling;

using System;
using System.IO;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.EventArgs;

using Newtonsoft.Json.Linq;

public class NewMember
{
	public static async Task NewMemberEvent(DiscordClient s, MessageCreateEventArgs e)
	{
		String path = $"./data/levels/{e.Author.Id}.json";

		if(!File.Exists(path))
		{
			JObject data = new()
			{
				["user"] = new JObject(),
				["levels"] = new JObject(),
				["preferences"] = new JObject()
			};

			StreamWriter sw = new(File.Create(path));
			sw.Write(data.ToString());
			sw.Close();
		}
	}
}
