using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stats.Commands.Settings;
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


        public AddCommand(ILogger<AddCommand> logger) => (_logger) = (logger);


        public override async Task<int> ExecuteAsync(CommandContext context, AddSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
