﻿using Newtonsoft.Json;

namespace ExperienceBot.Utils
{
    public class Config
    {
        [JsonProperty("token")]
        public static String Token { get; set; } = "";

        [JsonProperty("prefixes")]
        public static String[] Prefixes { get; set; } = { "--" };

        [JsonProperty("guildId")]
        public static UInt64 Guild { get; set; } = 0;

        [JsonProperty("modules")]
        public static Modules Modules { get; set; } = new Modules();
    }
    public class Modules
    {
        [JsonProperty("leveling")]
        public static Leveling Leveling { get; set; } = new Leveling();
    }
    public class Leveling
    {
        [JsonProperty("enabled")]
        public static Boolean Enabled { get; set; } = false;

        [JsonProperty("minMessageLength")]
        public static Int16 MinMessageLenght { get; set; } = 0;

        [JsonProperty("xpRange")]
        public static Range XpRange { get; set; } = new Range();

        [JsonProperty("noXpChannels")]
        public static UInt64[]? NoXpChannels { get; set; } = { };

        [JsonProperty("noXpRoles")]
        public static UInt64[]? NoXpRoles { get; set; } = { };

        [JsonProperty("levelUpAnnouncementsId")]
        public static UInt64 LevelUpChannel { get; set; } = 0;

        [JsonProperty("levelRoleRewards")]
        public static LevelRewards[]? LevelRoleRewards { get; set; } = { };

        [JsonProperty("stackRewards")]
        public static Boolean StackRewards { get; set; } = true;

        [JsonProperty("commands")]
        public static Commands Commands { get; set; } = new Commands();
    }
    public class Range
    {
        [JsonProperty("min")]
        public Int16 Min { get; set; } = 0;

        [JsonProperty("max")]
        public Int16 Max { get; set; } = 1;
    }
    public class LevelRewards
    {
        [JsonProperty("roleId")]
        public UInt64 RoleId { get; set; } = 0;

        [JsonProperty("requiredLevel")]
        public Int16 RequiredLevel { get; set; } = 999;
    }

    public class Commands
    {
        [JsonProperty("rank")]
        public Boolean Rank { get; set; } = true;

        [JsonProperty("leaderboard")]
        public Boolean Leaderboard { get; set; } = true;

        [JsonProperty("weekly")]

        public Boolean Weekly { get; set; } = false;
    }
}