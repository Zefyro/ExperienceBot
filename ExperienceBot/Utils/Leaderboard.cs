using Newtonsoft.Json;

namespace ExperienceBot.Utils
{
    public class Leaderboard
    {
        [JsonProperty("leaderboard")]
        public Ranked[] Ranked { get; set; }

        [JsonProperty("weekly")]
        public Weekly[] Weekly { get; set; }
    }
    public class Ranked
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
}
