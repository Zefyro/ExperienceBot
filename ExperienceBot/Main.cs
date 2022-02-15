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

			Console.WriteLine("Please enter a valid discord bot token:");
			Config.Token = Console.ReadLine()!;

			Console.WriteLine("Please enter a valid Guild Id:");
			Config.Guild = Convert.ToUInt64(Console.ReadLine());

			Console.WriteLine("Default prefix is (--). Would you like to add a prefix? (Y/N)");
			String A1 = Console.ReadLine()!;

			if(A1.ToLower() == ("y" ?? "yes"))
			{
				Console.WriteLine("Enter your prefix:");
				Config.Prefixes = Config.Prefixes.Append(Console.ReadLine()).ToArray()!;
			}
			else if(A1.ToLower() == ("n" ?? "no"))
			{
				Console.WriteLine("Alright, just know that you can add one from the config at a later date.");
			}
			else
			{
				Console.WriteLine("I take that as a no...");
			}

			Console.WriteLine("Would you like to enable module 'Leveling'? (Y/N)");
			String A2 = Console.ReadLine()!;
			if(A2.ToLower() == ("y" ?? "yes"))
			{
				Utils.Leveling.Enabled = true;
				Console.WriteLine("What should the minimum message length be?");
				Utils.Leveling.MinMessageLenght = Convert.ToInt16(Console.ReadLine());
				Console.WriteLine("What should the xp range be? (Min,Max)");
				String[] xpRange = Console.ReadLine()!.Trim().Split(',');
				Utils.Leveling.XpRange.Min = Convert.ToInt16(xpRange[0]);
				Utils.Leveling.XpRange.Max = Convert.ToInt16(xpRange[1]);
				Console.WriteLine("What should the level up announcement channel be? (Channel Id)");
				Utils.Leveling.LevelUpChannel = Convert.ToUInt64(Console.ReadLine());
			}
			else if(A2.ToLower() == ("n" ?? "no"))
			{
				Console.WriteLine("Alright, just know that you can enable it from the config at a later date.");
			}
			else
			{
				Console.WriteLine("I take that as a no...");
			}

			Console.WriteLine("You can edit more stuff from the config.json.\n\nPress any key to continue...");
			Console.ReadKey();

			using StreamWriter sw = new("./data/config.json");
			Config config = new();
			sw.Write(JsonConvert.SerializeObject(config, Formatting.Indented));
		}

		using(StreamReader sr = new("./data/config.json"))
		{
			JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
		}

		DiscordClient? discord = new(new DiscordConfiguration()
		{
			AutoReconnect = true,
			Token = Config.Token,
			TokenType = TokenType.Bot,
			MinimumLogLevel = LogLevel.Information
		});

		CommandsNextConfiguration? commandsConfig = new()
		{
			StringPrefixes = Config.Prefixes,
			DmHelp = false,
			EnableMentionPrefix = false
		};

		CommandsNextExtension? commands = discord.UseCommandsNext(commandsConfig);

		Guild = await discord.GetGuildAsync(Config.Guild);

		if(Utils.Leveling.Enabled)
		{
			discord.MessageCreated += (s, e) =>
			{
				if(!e.Author.IsBot && e.Message.Content.Length >= Utils.Leveling.MinMessageLenght)
				{
					if(!Utils.Leveling.NoXpChannels!.Contains(e.Channel.Id))
					{
						_ = Task.Run(() => NewMember.NewMemberEvent(s, e));
						_ = Task.Run(() => MessageSent.MessageSentEvent(s, e));
					}
				}

				return System.Threading.Tasks.Task.CompletedTask;
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
