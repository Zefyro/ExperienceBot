namespace ExperienceBot.Utils;

using System;

using Newtonsoft.Json;

public class Levels
{
    [JsonProperty("user")]
    public User User { get; set; } = default!;

    [JsonProperty("levels")]
    public Lvls Lvls { get; set; } = default!;

    [JsonProperty("preferences")]
    public Preferences Preferences { get; set; } = default!;
}
public class User
{
	[JsonProperty("id")]
	public UInt64 Id { get; set; } = 0;

	[JsonProperty("name")]
	public String Name { get; set; } = "";
}
public class Lvls
{
	[JsonProperty("level")]
	public Int16 Lvl { get; set; } = 0;

	[JsonProperty("totalXp")]
	public Int32 TotalXp { get; set; } = 0;

	[JsonProperty("xp")]
	public Int32 Xp { get; set; } = 0;

	[JsonProperty("reqXp")]
	public Int32 ReqXp { get; set; } = 50;

	[JsonProperty("messages")]
	public Int32 Messages { get; set; } = 0;
}
public class Preferences
{
	[JsonProperty("levelUpMention")]
	public Boolean LevelUpMention { get; set; } = true;

	[JsonProperty("levelUpDm")]
	public Boolean LevelUpDm { get; set; } = false;
}
