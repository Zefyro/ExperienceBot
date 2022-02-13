using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperienceBot.Leveling
{
    public class Leaderboard
    { 
        public static void Update(Utils.Levels level)
        {
            Boolean match = false;
            Utils.Leaderboard leaderboard = Get();

            Utils.Ranked filtered = leaderboard.Ranked.Where(x => x.Id == level.User.Id).FirstOrDefault();

            Utils.Ranked newEntry = new Utils.Ranked();

            if (filtered == default)
            {
                newEntry.XP = level.Lvls.TotalXp;
                newEntry.Level = level.Lvls.Lvl;
                newEntry.Messages = level.Lvls.Messages;
                newEntry.Id = level.User.Id;
                newEntry.Rank = 0;
            }
            else
            {
                filtered.XP = level.Lvls.TotalXp;
                filtered.Level = level.Lvls.Lvl;
                filtered.Messages = level.Lvls.Messages;
                for (Int32 i = 0; i < leaderboard.Ranked.Count(); i++)
                {
                    if (leaderboard.Ranked[i].Id == level.User.Id)
                    {
                        match = true;
                        leaderboard.Ranked[i] = filtered;
                    }
                }
            }
            if (!match)
            {
                leaderboard.Ranked.Append(newEntry);
            }
            leaderboard.Ranked.Append(newEntry);
            //leaderboard = Sort(leaderboard);
            Save(leaderboard);
        }

        public static Utils.Leaderboard Sort(Utils.Leaderboard leaderboard)
        {
            leaderboard.Ranked.OrderBy(x => x.XP);

            for (Int16 i = 1; i <= leaderboard.Ranked.Count(); i++)
            {
                leaderboard.Ranked[i].Rank = i;
            }

            return leaderboard;
        }
        public static Utils.Leaderboard Get()
        {
            String path = $"./data/leaderboard.json";

            StreamReader sr = new StreamReader(path);

            String json = sr.ReadToEnd();
            sr.Close();

            return JsonConvert.DeserializeObject<Utils.Leaderboard>(json);
        }
        public static void Save(Utils.Leaderboard leaderboard)
        {
            String path = $"./data/leaderboard.json";

            StreamWriter sw = new StreamWriter(path);
            String output = JsonConvert.SerializeObject(leaderboard, Formatting.None);
            sw.Write(output);
            sw.Close();
        }
    }
}
