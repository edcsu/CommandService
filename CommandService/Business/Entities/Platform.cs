namespace CommandService.Business.Entities
{
    public class Platform : BaseModel
    {
#nullable disable
        public Guid ExternalID { get; set; }

        public string Name { get; set; }

        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}
