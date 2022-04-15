namespace CommandService.Business.ViewModels
{
    public class CommandDetailsDto
    {
        public Guid Id { get; set; }

        public string? HowTo { get; set; }

        public string? CommandLine { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
