using System.Collections.Generic;


namespace Stats.Config
{
    public class ActivitiesConfig
    {
        List<Activity> Activities { get; init; } = new();
    }


    public class Activity
    {
        public string Name { get; init; } = string.Empty;

        public string Emoji { get; init; } = string.Empty;

        public int Goal { get; init; }
    }
}
