namespace ExperienceBot.Commands;

using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

public class WeeklyTopCommand : BaseCommandModule
{
	[Command("weekly")]
	public Task Weekly(CommandContext ctx) => Task.CompletedTask;
}
