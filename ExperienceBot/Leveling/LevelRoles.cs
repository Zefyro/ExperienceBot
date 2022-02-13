using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using ExperienceBot.Utils;

using Newtonsoft.Json;

namespace ExperienceBot.Leveling
{
    public class LevelRoles
    {
        public static async Task GrantRewards(DiscordMember member, Levels? level)
        {
            DiscordRole RoleReward;

            String path = $"./data/levels/{member.Id}.json";
            StreamReader sr = new StreamReader(path);

            String json = sr.ReadToEnd();
            sr.Close();

            Levels levels = JsonConvert.DeserializeObject<Levels>(json);

            if (level != null)
                levels = level;

            foreach (var reward in Utils.Leveling.LevelRoleRewards)
            {
                if (levels.Lvls.Lvl >= reward.RequiredLevel)
                {
                    RoleReward = ExperienceBot.Guild.GetRole(reward.RoleId);
                    await member.GrantRoleAsync(RoleReward);
                }
            }
        }
    }
}
