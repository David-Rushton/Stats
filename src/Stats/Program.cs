using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stats.Commands;
using Stats.Config;
using Spectre.Cli.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.IO;
using System.Threading.Tasks;


namespace Stats
{
    class Program
    {
        static async Task<int> Main(string[] args) =>
            await Bootstrap().RunAsync(args)
        ;


        static CommandApp Bootstrap()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build()
            ;

            var serviceCollection = new ServiceCollection()
                .AddLogging
                (
                    config =>
                    {
                        config.AddSimpleConsole();
                    }
                )
                .Configure<ActivitiesConfig>(configuration.GetSection("activities"))
            ;


            using var registrar = new DependencyInjectionRegistrar(serviceCollection);
            var app = new CommandApp(registrar);

            app.Configure
            (
                config =>
                {
                    config.Settings.ApplicationName = "Stats";
                    config.ValidateExamples();

                    config.AddCommand<AddCommand>("add")
                        .WithDescription("Add an activity to the journal")
                        .WithExample(new[] { "add" })
                    ;
                }
            );

            return app;
        }
    }
}
