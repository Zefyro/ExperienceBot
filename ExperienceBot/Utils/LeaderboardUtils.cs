namespace ExperienceBot.Utils;

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Newtonsoft.Json;

public static class LeaderboardUtils
{
	public static void Update(Levels level)
	{
		Leaderboard leaderboard = Get();

		RankedUser filtered = leaderboard.Ranked.Where(x => x.Id == level.User.Id).FirstOrDefault()!;

		RankedUser newEntry = new();

		if(filtered == default)
		{
			newEntry.XP = level.Lvls!.TotalXp;
			newEntry.Level = level.Lvls.Lvl;
			newEntry.Messages = level.Lvls.Messages;
			newEntry.Id = level.User!.Id;
			newEntry.Rank = 0;

			leaderboard.Ranked.Add(newEntry);
		}
		else
		{
			Int32 index = leaderboard.Ranked.IndexOf(filtered);

			filtered.XP = level.Lvls!.TotalXp;
			filtered.Level = level.Lvls.Lvl;
			filtered.Messages = level.Lvls.Messages;

			leaderboard.Ranked[index] = filtered;
		}

		//leaderboard = Sort(leaderboard);
		Save(leaderboard);
	}

	public static Leaderboard Sort(Leaderboard leaderboard)
	{
		leaderboard.Ranked = leaderboard.Ranked!.OrderBy(x => x.XP).ToList();

		for(Int16 i = 1; i <= leaderboard.Ranked.Count; i++)
		{
			leaderboard.Ranked[i].Rank = i;
		}

		return leaderboard;
	}

	public static Leaderboard Get()
	{
		String path = $"./data/leaderboard.json";

		StreamReader sr = new(path);

		String json = sr.ReadToEnd();
		sr.Close();

		return JsonConvert.DeserializeObject<Leaderboard>(json)!;
	}

	public static void Save(Leaderboard leaderboard)
	{
		String path = $"./data/leaderboard.json";

		StreamWriter sw = new(path);
		String output = JsonConvert.SerializeObject(leaderboard, Formatting.None);
		sw.Write(output);
		sw.Close();
	}
}
