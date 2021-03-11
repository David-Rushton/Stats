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
                        activity.DailyGoal.ToString()
                    );


                AnsiConsole.Render(table);

                return 0;
            }


            // todo: move below into own method
            // todo: render progress after add!  show the journal
            if(isSupportedActivity)
            {
                var activityValue = AnsiConsole.Ask<decimal>(requestedActivity!.Prompt);
                var activityEvent = requestedActivity!.CreateEvent(activityValue);
                var journal = await _journalRepository.AddEventToDay(activityEvent, DateTime.Now);

                // todo: own method?
                var barChart = new BarChart()
                    .ShowValues()
                    .Label(journal.Day.ToString("dd MMMM"))
                ;

                var activityTotals = journal.ActivityEvents
                    .GroupBy(k => k.Activity.Name)
                    .ToDictionary(k => k.Key, v => new { Value = v.Sum(i => i.Value), Unit = v.First().Activity.Emoji })
                ;

                foreach(var (k, v) in activityTotals)
                {
                    barChart.AddItem($"{k} {v.Unit}", Convert.ToDouble(v.Value), GetColor(v.Value));
                }


                AnsiConsole.MarkupLine($"[green]Activity added to journal: {settings.Activity}[/]");
                AnsiConsole.Render(barChart);
                return 0;
            }

            AnsiConsole.MarkupLine($"[red invert]Activity not supported: { settings.Activity ?? "<not supplied>" }[/]");
            return 1;



            Color GetColor(decimal value) =>
                requestedActivity!.DailyGoal switch
                {
                    // bug: this is the goal for the added activity - not the reported one
                    decimal goal when value < goal    => Color.Red,
                    decimal goal when value > goal    => Color.Green,
                    _                                 => Color.Yellow
                }

            ;
        }
    }
}
