﻿namespace ExperienceBot.Leveling;

using System;
using System.IO;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.EventArgs;

using global::ExperienceBot.Utils;

using Newtonsoft.Json;

public class MessageSent
{
	public static async Task MessageSentEvent(DiscordClient s, MessageCreateEventArgs e)
	{
		Int32 xp = ExperienceBot.Random.Next(
			ExperienceBot.Configuration.Modules.Leveling.XpRange.Min,
			ExperienceBot.Configuration.Modules.Leveling.XpRange.Max);

		StreamReader sr = new($"./data/levels/{e.Author.Id}.json");

		String json = sr.ReadToEnd();
		sr.Close();

		Levels level = JsonConvert.DeserializeObject<Levels>(json)!;

		level.User!.Id = e.Author.Id;
		level.User.Name = $"{e.Author.Username}#{e.Author.Discriminator}";
		level.Lvls!.Messages++;
		level.Lvls.TotalXp += xp;
		level.Lvls.Xp += xp;

		if(level.Lvls.Xp >= level.Lvls.ReqXp)
		{
			await Level.LevelUp(level, e);
		}

		_ = Task.Run(() => LeaderboardUtils.Update(level));

		StreamWriter sw = new($"./data/levels/{e.Author.Id}.json");
		String output = JsonConvert.SerializeObject(level, Formatting.None);
		sw.Write(output);
		sw.Close();
	}
}
