namespace ExperienceBot.Utils;

using System;

using Newtonsoft.Json;

public class Config
{
	[JsonProperty("token")]
	public String Token { get; set; } = "";

	[JsonProperty("prefixes")]
	public String[] Prefixes { get; set; } = { "--" };

	[JsonProperty("guildId")]
	public UInt64 GuildId { get; set; } = 0;

	[JsonProperty("modules")]
	public Modules Modules { get; set; } = new Modules();
}
public class Modules
{
	[JsonProperty("leveling")]
	public Leveling Leveling { get; set; } = new Leveling();
}
public class Leveling
{
	[JsonProperty("enabled")]
	public Boolean Enabled { get; set; } = false;

	[JsonProperty("minMessageLength")]
	public Int16 MinMessageLength { get; set; } = 0;

	[JsonProperty("xpRange")]
	public Range XpRange { get; set; } = new Range();

	[JsonProperty("noXpChannels")]
	public UInt64[]? NoXpChannels { get; set; } = Array.Empty<UInt64>();

	[JsonProperty("noXpRoles")]
	public UInt64[]? NoXpRoles { get; set; } = Array.Empty<UInt64>();

	[JsonProperty("levelUpAnnouncementsId")]
	public UInt64 LevelUpChannel { get; set; } = 0;

	[JsonProperty("levelRoleRewards")]
	public LevelRewards[]? LevelRoleRewards { get; set; } = Array.Empty<LevelRewards>();

	[JsonProperty("stackRewards")]
	public Boolean StackRewards { get; set; } = true;

	[JsonProperty("commands")]
	public Commands Commands { get; set; } = new Commands();
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
