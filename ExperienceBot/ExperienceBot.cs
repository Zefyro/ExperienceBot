namespace ExperienceBot;

using System;

using DSharpPlus.Entities;

using global::ExperienceBot.Utils;

using Microsoft.Extensions.Logging;

internal sealed partial class ExperienceBot
{
	public EventId BotEventId { get; private set; } = new(42, "ExperienceBot");
    public static Random Random { get; private set; } = new();
    public static DiscordGuild Guild { get; private set; } = null!;
    public static Config Configuration { get; private set; } = null!;
    public static DiscordChannel LevelUpChannel { get; private set; } = null!;
}
