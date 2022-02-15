namespace ExperienceBot;

using System;

using DSharpPlus.Entities;

using Microsoft.Extensions.Logging;

internal partial class ExperienceBot
{
	public readonly EventId BotEventId = new(42, "ExperienceBot");
	public static Random Random = new();
	public static DiscordGuild? Guild;
}
