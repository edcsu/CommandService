namespace CommandService.Core
{
    public class PlatformEvents
    {
        public const string PlatFormPublished = "PlatForm_Published";
        public const string Undetermined = "Undetermined";
    }

    public enum EventType
    {
        PlatFormPublished,
        Undetermined,
    }
}
