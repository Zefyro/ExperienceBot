using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using ExperienceBot.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExperienceBot
{
    partial class ExperienceBot
    {
        public readonly EventId BotEventId = new EventId(42, "ExperienceBot");
        public static Random Random = new Random();
        public static DiscordGuild? Guild;
    }
}