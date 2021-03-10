using System.Text.Json.Serialization;


namespace Stats.Model
{
    public class Activity
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("emoji")]
        public string Emoji { get; init; } = string.Empty;

        // todo: is daily goal a better name?
        [JsonPropertyName("goal")]
        public int Goal { get; init; }
    }
}
