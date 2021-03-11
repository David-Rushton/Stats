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

            // todo: break into separate method
            if(settings.ListActivities)
            {
                var table = new Table();
                table.Title("Activities");
                table.Border(TableBorder.Rounded);
                table.AddColumn("Name");
                table.AddColumn(new TableColumn("Goal").RightAligned());

                foreach(var activity in supportedActivities)
                    table.AddRow
                    (
                        $"{activity.Emoji} {activity.Name}",
                        activity.Goal.ToString()
                    );


                AnsiConsole.Render(table);

                return 0;
            }


            // todo: move below into own method
            // todo: render progress after add!  show the journal
            if(isSupportedActivity)
            {
                AnsiConsole.MarkupLine($"[green]Activity added to journal: {settings.Activity}[/]");
                var journal = await _journalRepository.AddEventToDay(requestedActivity!, DateTime.Now);

                // todo: own method?
                var tree = new Tree(journal.Day.ToString("dd MMMM"));
                var activityTotals = journal.Activities.GroupBy(k => $"{k.Emoji} {k.Name}");
                foreach(var activity in activityTotals)
                    tree.AddNode($"[blue]{activity.Key} {activity.Count()}[/]");


                AnsiConsole.Render(tree);
                return 0;
            }

            AnsiConsole.MarkupLine($"[red invert]Activity not supported: { settings.Activity ?? "<not supplied>" }[/]");
            return 1;
        }
    }
}
