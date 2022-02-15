namespace ExperienceBot.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSharpPlus.Entities;

using Newtonsoft.Json;

public static class LeaderboardUtils
{
	public static void Update(Levels level)
	{
		Leaderboard leaderboard = Deserialize();

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
		Serialize(leaderboard);
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

	public static Leaderboard Deserialize()
	{
		String path = $"./data/leaderboard.json";

		StreamReader sr = new(path);

		String json = sr.ReadToEnd();
		sr.Close();

		return JsonConvert.DeserializeObject<Leaderboard>(json)!;
	}

	public static void Serialize(Leaderboard leaderboard)
	{
		String path = $"./data/leaderboard.json";

		StreamWriter sw = new(path);
		String output = JsonConvert.SerializeObject(leaderboard, Formatting.None);
		sw.Write(output);
		sw.Close();
	}

	public static async Task<String> PrettyPrint(Int32 startIndex, Int32 count)
	{
		Leaderboard leaderboard = Deserialize();
		leaderboard = Sort(leaderboard);

		IEnumerable<RankedUser> users = leaderboard.Ranked.Skip(startIndex).Take(count);

		StringBuilder builder = new();

		for(Int32 i = 0; i < users.Count(); i++)
		{
			String displayName = (await ExperienceBot.Guild.GetMemberAsync(users.ElementAt(i).Id)).DisplayName
				?? $"User missing [{users.ElementAt(i).Id}]";

			builder.Append($"**#{i} - {displayName} - Level {users.ElementAt(i).Level}\n");
			builder.Append($"{users.ElementAt(i).XP} XP -- {users.ElementAt(i).Messages} Messages\n\n");
		}

		return builder.ToString().TrimEnd('\n');
	}
}
