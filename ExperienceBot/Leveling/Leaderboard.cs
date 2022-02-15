namespace ExperienceBot.Leveling;

using System;
using System.IO;
using System.Linq;

using global::ExperienceBot.Utils;

using Newtonsoft.Json;

public class Leaderboard
{
	public static void Update(Levels level)
	{
		Boolean match = false;
		Utils.Leaderboard leaderboard = Get();

		Ranked filtered = leaderboard.Ranked.Where(x => x.Id == level.User.Id).FirstOrDefault();

		Ranked newEntry = new();

		if(filtered == default)
		{
			newEntry.XP = level.Lvls!.TotalXp;
			newEntry.Level = level.Lvls.Lvl;
			newEntry.Messages = level.Lvls.Messages;
			newEntry.Id = level.User!.Id;
			newEntry.Rank = 0;
		}
		else
		{
			filtered.XP = level.Lvls!.TotalXp;
			filtered.Level = level.Lvls.Lvl;
			filtered.Messages = level.Lvls.Messages;
			for(Int32 i = 0; i < leaderboard.Ranked.Length; i++)
			{
				if(leaderboard.Ranked[i].Id == level.User!.Id)
				{
					match = true;
					leaderboard.Ranked[i] = filtered;
				}
			}
		}
		if(!match)
		{
			leaderboard.Ranked = leaderboard.Ranked.Append(newEntry).ToArray();
		}
		leaderboard.Ranked = leaderboard.Ranked.Append(newEntry).ToArray();
		//leaderboard = Sort(leaderboard);
		Save(leaderboard);
	}

	public static Utils.Leaderboard Sort(Utils.Leaderboard leaderboard)
	{
		leaderboard.Ranked = leaderboard.Ranked!.OrderBy(x => x.XP).ToArray();

		for(Int16 i = 1; i <= leaderboard.Ranked.Length; i++)
		{
			leaderboard.Ranked[i].Rank = i;
		}

		return leaderboard;
	}
	public static Utils.Leaderboard Get()
	{
		String path = $"./data/leaderboard.json";

		StreamReader sr = new(path);

		String json = sr.ReadToEnd();
		sr.Close();

		return JsonConvert.DeserializeObject<Utils.Leaderboard>(json)!;
	}
	public static void Save(Utils.Leaderboard leaderboard)
	{
		String path = $"./data/leaderboard.json";

		StreamWriter sw = new(path);
		String output = JsonConvert.SerializeObject(leaderboard, Formatting.None);
		sw.Write(output);
		sw.Close();
	}
}
