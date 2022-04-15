namespace CommandService.Business.Entities
{
    public class Command : BaseModel
    {
        public string? HowTo { get; set; }
        public string? CommandLine { get; set; }
        public Guid PlatformID { get; set; }

#nullable disable
        public Platform Platform { get; set; }
    }
}
