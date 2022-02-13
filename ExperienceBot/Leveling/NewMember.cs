using DSharpPlus;
using DSharpPlus.EventArgs;

using Newtonsoft.Json.Linq;

namespace ExperienceBot.Leveling
{
    public class NewMember
    {
        public static async Task NewMemberEvent(DiscordClient s, MessageCreateEventArgs e)
        {
            String path = $"./data/levels/{e.Author.Id}.json";

            if (!File.Exists(path))
            {
                JObject data = new JObject();
                data["user"] = new JObject();
                data["levels"] = new JObject();
                data["preferences"] = new JObject();

                StreamWriter sw = new StreamWriter(File.Create(path));
                sw.Write(data.ToString());
                sw.Close();
            }
        }
    }
}
