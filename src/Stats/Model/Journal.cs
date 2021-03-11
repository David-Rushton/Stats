using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Stats.Model
{
    public class Journal
    {
        [JsonPropertyName("day")]
        public DateTime Day { get; init; }

        [JsonPropertyName("activityEvents")]
        public List<ActivityEvent> ActivityEvents { get; init; } = new();
    }
}
