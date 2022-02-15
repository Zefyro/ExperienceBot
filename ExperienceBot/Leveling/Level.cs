namespace ExperienceBot.Leveling;

using System;
using System.Threading.Tasks;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;

using global::ExperienceBot.Utils;

public class Level
{
	public static async Task LevelUp(Levels level, MessageCreateEventArgs e)
	{
		DiscordMember member = await ExperienceBot.Guild!.GetMemberAsync(e.Author.Id);

		level.Lvls!.Lvl++;
		level.Lvls.Xp -= level.Lvls.ReqXp;
		level.Lvls.ReqXp = (Int32)(50 + (10 * level.Lvls.Lvl) + 1.1 * level.Lvls.Lvl * (1.1 * level.Lvls.Lvl));

		_ = LevelRoles.GrantRewards(member, level);

		await Announce(level, member);
	}
	public static async Task Announce(Levels level, DiscordMember member)
	{
		await ExperienceBot.LevelUpChannel.SendMessageAsync($"Level Up! {member.Mention} current level is: {level.Lvls!.Lvl}");

		if(level.Preferences.LevelUpDm)
		{
			try
			{
				await member.SendMessageAsync($"{ExperienceBot.Guild.Name}\nLevel Up! Your current level is: {level.Lvls!.Lvl}");
			}
			catch(UnauthorizedException)
			{
				// i was so tempted to put
				// Discord user. You fool. You buffoon. You must have realized that you have failed to enabled your DMs, and that 
				// therefore the Bot will not be able to satisfy your request for a DM alert.

				await ExperienceBot.LevelUpChannel.SendMessageAsync($"Failed to send a DM to {member.Mention} since they turned their DMs off.");
			}
		}
	}
}
