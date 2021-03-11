using System.Text.Json.Serialization;


namespace Stats.Model
{
    public class ActivityEvent
    {
        [JsonPropertyName("activity")]
        public Activity Activity { get; init; } = new();

        [JsonPropertyName("value")]
        public decimal Value { get; init; }
    }
}
