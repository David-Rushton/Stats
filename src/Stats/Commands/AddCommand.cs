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


        public AddCommand(ILogger<AddCommand> logger, IActivitiesRepository activitiesRepository) =>
            (_logger, _activitiesRepository) = (logger, activitiesRepository)
        ;


        public override async Task<int> ExecuteAsync(CommandContext context, AddSettings settings)
        {
            var supportedActivity = await _activitiesRepository.GetActivities();
            var isSupportedActivity = supportedActivity.Exists(a => a.Name == settings?.Activity);

            if(isSupportedActivity)
            {
                AnsiConsole.MarkupLine($"[green]Activity added to journal: {settings.Activity}[/]");
                return 0;
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Activity not supported: {settings.Activity}[/]");
                return 1;
            }
        }
    }
}
