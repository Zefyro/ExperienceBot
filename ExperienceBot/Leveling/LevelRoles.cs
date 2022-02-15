namespace ExperienceBot.Leveling;

using System;
using System.IO;
using System.Threading.Tasks;

using DSharpPlus.Entities;

using global::ExperienceBot.Utils;

using Newtonsoft.Json;

public class LevelRoles
{
	public static async Task GrantRewards(DiscordMember member, Levels? level)
	{
		DiscordRole RoleReward;

		String path = $"./data/levels/{member.Id}.json";
		StreamReader sr = new(path);

		String json = sr.ReadToEnd();
		sr.Close();

		Levels levels = JsonConvert.DeserializeObject<Levels>(json)!;

		if(level != null)
		{
			levels = level;
		}

		foreach(LevelRewards? reward in ExperienceBot.Configuration.Modules.Leveling.LevelRoleRewards)
		{
			if(levels.Lvls.Lvl >= reward.RequiredLevel)
			{
				RoleReward = ExperienceBot.Guild!.GetRole(reward.RoleId);
				await member.GrantRoleAsync(RoleReward);
			}
		}
	}
}
