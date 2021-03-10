using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;


namespace Stats.Commands.Settings
{
    public class AddSettings : CommandSettings
    {
        [CommandArgument(0, "[Activity]")]
        public string? Activity { get; set; }
    }
}
