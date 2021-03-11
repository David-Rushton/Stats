using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stats.Config;
using Stats.Repositories;
using Stats.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Stats.Repositories
{
    public class JournalRepositoryJson : IJournalRepository
    {
        readonly ILogger<JournalRepositoryJson> _logger;

        readonly JsonDbConfig _jsonDbConfig;


        public JournalRepositoryJson(ILogger<JournalRepositoryJson> logger, IOptions<JsonDbConfig> jsonDbConfig) =>
            (_logger, _jsonDbConfig) = (logger, jsonDbConfig.Value)
        ;


        public async Task<Journal> AddEventToDay(Model.Activity activity, DateTime day)
        {
            var journalPath = Path.Join(_jsonDbConfig.Path, $"journal.{day.ToString("yyyyMMdd")}.json");
            var journal = await ReadDay(day);

            // update the journal
            journal.Activities.Add(activity);

            // persist changes
            var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
            using var file = File.Open(journalPath, FileMode.Create);
            using var stream = new StreamWriter(file);
            stream.Write(JsonSerializer.Serialize<Journal>(journal, jsonOptions));

            // return updated
            return journal;
        }


        public async Task<Journal> ReadDay(DateTime day)
        {
            var journalPath = Path.Join(_jsonDbConfig.Path, $"journal.{day.ToString("yyyyMMdd")}.json");
            var isJournalPathValid = File.Exists(journalPath);

            if(isJournalPathValid)
            {
                using var stream = File.OpenRead(journalPath);
                var journal = await JsonSerializer.DeserializeAsync<Journal>(stream);

                // todo:
                Debug.Assert(journal is not null, "Journal is null.  Cannot");

                return journal;
            }


            return new Journal
            {
                Day = day
            };
        }
    }
}
