using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

using Newtonsoft.Json;

using ExperienceBot.Utils;
using ExperienceBot.Leveling;
using ExperienceBot.Commands;

using Microsoft.Extensions.Logging;

namespace ExperienceBot
{
    partial class ExperienceBot
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            if (!Directory.Exists("./data") || !File.Exists("./data/config.json"))
            {
                Directory.CreateDirectory("./data");

                Console.WriteLine("Please enter a valid discord bot token:");
                Config.Token = Console.ReadLine();

                Console.WriteLine("Please enter a valid Guild Id:");
                Config.Guild = Convert.ToUInt64(Console.ReadLine());

                Console.WriteLine("Default prefix is (--). Would you like to add a prefix? (Y/N)");
                String A1 = Console.ReadLine();

                if (A1.ToLower() == ("y" ?? "yes"))
                {
                    Console.WriteLine("Enter your prefix:");
                    Config.Prefixes.Append(Console.ReadLine());
                }
                else if (A1.ToLower() == ("n" ?? "no")) 
                    Console.WriteLine("Alright, just know that you can add one from the config at a later date.");
                else
                    Console.WriteLine("I take that as a no...");

                Console.WriteLine("Would you like to enable module 'Leveling'? (Y/N)");
                String A2 = Console.ReadLine();
                if (A2.ToLower() == ("y" ?? "yes"))
                {
                    Utils.Leveling.Enabled = true;
                    Console.WriteLine("What should the minimum message length be?");
                    Utils.Leveling.MinMessageLenght = Convert.ToInt16(Console.ReadLine());
                    Console.WriteLine("What should the xp range be? (Min,Max)");
                    String[] xpRange = Console.ReadLine().Trim().Split(',');
                    Utils.Leveling.XpRange.Min = Convert.ToInt16(xpRange[0]);
                    Utils.Leveling.XpRange.Max = Convert.ToInt16(xpRange[1]);
                    Console.WriteLine("What should the level up announcement channel be? (Channel Id)");
                    Utils.Leveling.LevelUpChannel = Convert.ToUInt64(Console.ReadLine());
                }
                else if (A2.ToLower() == ("n" ?? "no"))
                    Console.WriteLine("Alright, just know that you can enable it from the config at a later date.");
                else
                    Console.WriteLine("I take that as a no...");
                Console.WriteLine("You can edit more stuff from the config.json.\n\nPress any key to continue...");
                Console.ReadKey();

                using (StreamWriter sw = new StreamWriter("./data/config.json"))
                {
                    Config config = new Config();
                    sw.Write(JsonConvert.SerializeObject(config, Formatting.Indented));
                }
            }

            using (StreamReader sr = new StreamReader("./data/config.json"))
            {
                JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
            }

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                AutoReconnect = true,
                Token = Config.Token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.Information
            });

            var commandsConfig = new CommandsNextConfiguration(new CommandsNextConfiguration()
            {
                StringPrefixes = Config.Prefixes,
                DmHelp = false,
                EnableMentionPrefix = false
            });

            var commands = discord.UseCommandsNext(commandsConfig);

            Guild = await discord.GetGuildAsync(Config.Guild);

            if (Utils.Leveling.Enabled)
            {
                discord.MessageCreated += async (s, e) =>
                {
                    if (!e.Author.IsBot && e.Message.Content.Length >= Utils.Leveling.MinMessageLenght)
                    {
                        if (!Utils.Leveling.NoXpChannels.Contains(e.Channel.Id))
                        {
                            _ = Task.Run(() => NewMember.NewMemberEvent(s, e));
                            _ = Task.Run(() => MessageSent.MessageSentEvent(s, e));
                        }
                    }
                };

                commands.RegisterCommands<RankCommand>();
                commands.RegisterCommands<LeaderboardCommand>();

                // Not implemented yet
                //commands.RegisterCommands<WeeklyTopCommand>();
            }

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
