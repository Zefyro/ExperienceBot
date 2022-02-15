namespace ExperienceBot;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;

using global::ExperienceBot.Commands;
using global::ExperienceBot.Leveling;
using global::ExperienceBot.Utils;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

internal partial class ExperienceBot
{
	private static async Task Main()
	{
		if(!Directory.Exists("./data") || !File.Exists("./data/config.json"))
		{
			Directory.CreateDirectory("./data");
			Config config = new();

			Console.WriteLine("Please enter a valid discord bot token:");
			config.Token = Console.ReadLine()!;

			Console.WriteLine("Please enter a valid GuildId Id:");
			config.GuildId = Convert.ToUInt64(Console.ReadLine());

			Console.WriteLine("The default prefix is --. Would you like to add a prefix? (Y/N)");
			String answer = Console.ReadLine()!;

			if(answer.ToLower() == "y" || answer.ToLower() == "yes")
			{
				Console.WriteLine("Enter your prefix:");
				config.Prefixes = config.Prefixes.Append(Console.ReadLine()).ToArray()!;
			}
			else if(answer.ToLower() == "n" || answer.ToLower() == "no")
			{
				Console.WriteLine("Alright, just know that you can add one from the config at a later date.");
			}
			else
			{
				Console.WriteLine("I'll take that as a no...");
			}

			Console.WriteLine("Would you like to enable the leveling module? (Y/N)");
			answer = Console.ReadLine()!;

			if(answer.ToLower() == "y" || answer.ToLower() == "yes")
			{
				config.Modules.Leveling.Enabled = true;

				Console.WriteLine("What should the minimum message length be?");
				config.Modules.Leveling.MinMessageLength = Convert.ToInt16(Console.ReadLine());

				Console.WriteLine("What should the xp range be? (Min,Max)");
				String[] xpRange = Console.ReadLine()!.Trim().Split(',');

				config.Modules.Leveling.XpRange.Min = Convert.ToInt16(xpRange[0]);
				config.Modules.Leveling.XpRange.Max = Convert.ToInt16(xpRange[1]);

				Console.WriteLine("What should the level up announcement channel be? (Channel Id)");
				config.Modules.Leveling.LevelUpChannel = Convert.ToUInt64(Console.ReadLine());
			}
			else if(answer.ToLower() == "n" || answer.ToLower() == "no")
			{
				Console.WriteLine("Alright, just know that you can enable it from the config at a later date.");
			}
			else
			{
				Console.WriteLine("I'll take that as a no...");
			}

			Console.WriteLine("You can edit more stuff in the config.json file.\n\nPress any key to continue...");

			StreamWriter sw;

			if(File.Exists("./data/config.json"))
			{
				sw = new(File.Create("./data/config.json"));
			}
			else
			{
				sw = new("./data/config.json");
			}

			sw.Write(JsonConvert.SerializeObject(config, Formatting.Indented));
			Console.ReadKey();
		}

		using StreamReader sr = new("./data/config.json");
		Configuration = JsonConvert.DeserializeObject<Config>(sr.ReadToEnd())!;
		

		DiscordClient? discord = new(new DiscordConfiguration()
		{
			AutoReconnect = true,
			Token = Configuration.Token,
			TokenType = TokenType.Bot,
			MinimumLogLevel = LogLevel.Information
		});

		CommandsNextConfiguration? commandsConfig = new()
		{
			StringPrefixes = Configuration.Prefixes,
			DmHelp = false,
			EnableMentionPrefix = false
		};

		CommandsNextExtension? commands = discord.UseCommandsNext(commandsConfig);

		Guild = await discord.GetGuildAsync(Configuration.GuildId);

		if(Configuration.Modules.Leveling.Enabled)
		{
			LevelUpChannel = Guild.GetChannel(Configuration.Modules.Leveling.LevelUpChannel);

#pragma warning disable CS1998
			discord.MessageCreated += async (s, e) =>
#pragma warning restore CS1998
			{
				_ = Task.Run( async () =>
				{
					if(!e.Author.IsBot && e.Message.Content.Length >= Configuration.Modules.Leveling.MinMessageLength)
					{
						if(!Configuration.Modules.Leveling.NoXpChannels!.Contains(e.Channel.Id))
						{
							// these HAVE to be in order
							await Task.Run(() => NewMember.NewMemberEvent(s, e));
							await Task.Run(() => MessageSent.MessageSentEvent(s, e));
						}
					}
				});
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
