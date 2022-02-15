namespace ExperienceBot.Commands;

using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using global::ExperienceBot.Utils;

public class LeaderboardCommand : BaseCommandModule
{
	[Command("leaderboard")]
	[Aliases("levels", "lb")]
	[Description("Displays the server's leaderboard.")]
	public async Task Leaderboard(CommandContext ctx) => await Leaderboard(ctx, 0);

	[Command("leaderboard")]
	public async Task Leaderboard(CommandContext ctx, Int32 page)
	{
		String formattedLeaderboard = await LeaderboardUtils.PrettyPrint((page - 1) * 20, 20);

		DiscordEmbed embed = new DiscordEmbedBuilder
		{
			Title = $"{ctx.Guild.Name} leaderboard",
			Description = $"{formattedLeaderboard}",
			Color = ctx.Guild.Owner.Color,
			Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
			{
				Url = ctx.Guild.IconUrl
			},
			Footer = new DiscordEmbedBuilder.EmbedFooter
			{
				Text = DateTime.Now.ToString()
			}
		};
		await ctx.RespondAsync(embed: embed);
	}
}
