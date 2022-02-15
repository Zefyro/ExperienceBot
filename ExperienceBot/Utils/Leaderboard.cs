namespace ExperienceBot.Utils;

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

public class Leaderboard
{
	[JsonProperty("leaderboard")]
	public List<RankedUser> Ranked { get; set; } = default!;

	[JsonProperty("weekly")]
	public List<Weekly> Weekly { get; set; } = default!;
}
public class RankedUser
{
	[JsonProperty("id")]
	public UInt64 Id { get; set; }

	[JsonProperty("rank")]
	public Int16 Rank { get; set; }

	[JsonProperty("level")]
	public Int16 Level { get; set; }

	[JsonProperty("totalXP")]
	public Int32 XP { get; set; }

	[JsonProperty("messages")]
	public Int32 Messages { get; set; }
}
public class Weekly
{
	[JsonProperty("id")]
	public UInt64 Id { get; set; }

	[JsonProperty("messages")]
	public Int32 Messages { get; set; }
}
