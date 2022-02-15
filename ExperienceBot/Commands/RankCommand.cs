namespace ExperienceBot.Commands;

using System;
using System.IO;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using global::ExperienceBot.Utils;

using Newtonsoft.Json;

[Description("Displays the user's stats.")]
public class RankCommand : BaseCommandModule
{
	[Priority(0)]
	[Command("rank")]
	[Aliases("me")]
	public async Task Rank(CommandContext ctx)
	{
		String path = $"./data/levels/{ctx.Member.Id}.json";

		StreamReader sr = new(path);

		String json = sr.ReadToEnd();
		sr.Close();

		Levels levels = JsonConvert.DeserializeObject<Levels>(json)!;

		DiscordEmbed embed = new DiscordEmbedBuilder
		{
			Title = $"{ctx.Member.DisplayName}",
			Description = $"Current level: `{levels.Lvls!.Lvl}`\nExperience: `{levels.Lvls.Xp}`/`{levels.Lvls.ReqXp}`",
			Color = ctx.Member.Color,
			Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
			{
				Url = ctx.Member.AvatarUrl
			},
			Footer = new DiscordEmbedBuilder.EmbedFooter
			{
				Text = DateTime.Now.ToString()
			}
		};

		await ctx.RespondAsync(embed: embed);
	}

	[Priority(1)]
	[Command("rank")]
	[Aliases("stats")]
	public async Task Rank(CommandContext ctx, DiscordMember member)
	{
		String path = $"./data/levels/{member.Id}.json";

		if(!File.Exists(path))
		{
			if(member.IsBot)
			{
				await ctx.RespondAsync($"{member.Mention} is a bot and therefore cannot be ranked.");
				return;
			}
			await ctx.RespondAsync($"{member.Username}#{member.Discriminator} is not ranked yet.");
			return;
		}

		StreamReader sr = new(path);

		String json = sr.ReadToEnd();
		sr.Close();

		Levels levels = JsonConvert.DeserializeObject<Levels>(json)!;

		DiscordEmbed embed = new DiscordEmbedBuilder
		{
			Title = $"{member.DisplayName}",
			Description = $"Current level: `{levels.Lvls!.Lvl}`\nExperience: `{levels.Lvls.Xp}`/`{levels.Lvls.ReqXp}`",
			Color = ctx.Member.Color,
			Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
			{
				Url = member.AvatarUrl
			}
		};

		await ctx.RespondAsync(embed: embed);
	}
}
