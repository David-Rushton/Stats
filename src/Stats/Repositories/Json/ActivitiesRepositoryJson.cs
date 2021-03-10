using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stats.Config;
using Stats.Repositories;
using Stats.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Stats.Repositories.Json
{
    public class ActivitiesRepositoryJson : IActivitiesRepository
    {
        readonly ILogger<ActivitiesRepositoryJson> _logger;

        readonly JsonDbConfig _jsonDbConfig;



        public ActivitiesRepositoryJson(ILogger<ActivitiesRepositoryJson> logger, IOptions<JsonDbConfig> jsonDbConfig) =>
            (_logger, _jsonDbConfig) = (logger, jsonDbConfig.Value)
        ;


        public async Task<List<Model.Activity>> GetActivities()
        {
            var activitiesDb = Path.Join(_jsonDbConfig.Path, "Activities.json");
            using var stream = File.OpenRead(activitiesDb);
            var activities = await JsonSerializer.DeserializeAsync<Model.Activity[]>(stream);

            // todo: json db requires proper validation
            Debug.Assert(activities is not null, "Could not read from Json db");


            return activities.ToList();
        }
    }
}
