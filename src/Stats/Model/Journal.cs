using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Stats.Model
{
    public class Journal
    {
        [JsonPropertyName("day")]
        public DateTime Day { get; init; }

        [JsonPropertyName("activities")]
        public List<Activity> Activities { get; init; } = new();
    }
}
