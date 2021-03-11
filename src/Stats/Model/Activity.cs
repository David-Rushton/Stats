using System.Text.Json.Serialization;


namespace Stats.Model
{
    public class Activity
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("emoji")]
        public string Emoji { get; init; } = string.Empty;

        [JsonPropertyName("prompt")]
        public string Prompt { get; init; } = string.Empty;

        [JsonPropertyName("dailyGoal")]
        public decimal DailyGoal { get; init; }


        public ActivityEvent CreateEvent(decimal value) =>
            new ActivityEvent
            {
                Activity = this,
                Value = value
            }
        ;
    }
}
