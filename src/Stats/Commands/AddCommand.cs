using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stats.Commands.Settings;
using Stats.Config;
using Stats.Repositories;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Stats.Commands
{
    public class AddCommand : AsyncCommand<AddSettings>
    {
        readonly ILogger<AddCommand> _logger;

        readonly IActivitiesRepository _activitiesRepository;

        readonly IJournalRepository _journalRepository;


        public AddCommand(ILogger<AddCommand> logger, IActivitiesRepository activitiesRepository, IJournalRepository journalRepository) =>
            (_logger, _activitiesRepository, _journalRepository) = (logger, activitiesRepository, journalRepository)
        ;


        public override async Task<int> ExecuteAsync(CommandContext context, AddSettings settings)
        {
            var supportedActivities = await _activitiesRepository.GetActivities();
            var requestedActivity = supportedActivities.Where(a => a.Name == settings?.Activity).FirstOrDefault();
            var isSupportedActivity = requestedActivity is not null;

            if(isSupportedActivity)
            {
                AnsiConsole.MarkupLine($"[green]Activity added to journal: {settings.Activity}[/]");
                await _journalRepository.AddEventToDay(requestedActivity!, DateTime.Now);
                return 0;
            }
            else
            {
                AnsiConsole.MarkupLine($"[red invert]Activity not supported: { settings.Activity ?? "<not supplied>" }[/]");
                return 1;
            }
        }
    }
}
