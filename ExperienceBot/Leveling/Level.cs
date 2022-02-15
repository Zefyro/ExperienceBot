namespace ExperienceBot.Leveling;

using System;
using System.Threading.Tasks;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using global::ExperienceBot.Utils;

public class Level
{
	public static async Task LevelUp(Levels level, MessageCreateEventArgs e)
	{
		DiscordMember member = await ExperienceBot.Guild!.GetMemberAsync(e.Author.Id);

		level.Lvls!.Lvl++;
		level.Lvls.Xp -= level.Lvls.ReqXp;
		level.Lvls.ReqXp = (Int32)(50 + (10 * level.Lvls.Lvl) + ((1.1 * level.Lvls.Lvl)) * (1.1 * level.Lvls.Lvl));

		_ = LevelRoles.GrantRewards(member, level);

		await Announce(level, member);
	}
	public static async Task Announce(Levels level, DiscordMember member)
	{
		DiscordChannel channel = ExperienceBot.Guild!.GetChannel(Leveling.LevelUpChannel);

		if(level.Preferences!.LevelUpMention)
		{
			await channel.SendMessageAsync($"Level Up! {member.Mention} current level is: {level.Lvls!.Lvl}");
		}
		else if(level.Preferences.LevelUpDm)
		{
			await member.SendMessageAsync($"{ExperienceBot.Guild.Name}\nLevel Up! Your current level is: {level.Lvls!.Lvl}");
			await channel.SendMessageAsync($"Level Up! {member.Username}#{member.Discriminator} current level is: {level.Lvls.Lvl}");
		}
		else
		{
			await channel.SendMessageAsync($"Level Up! {member.Username}#{member.Discriminator} current level is: {level.Lvls!.Lvl}");
		}
	}
}
