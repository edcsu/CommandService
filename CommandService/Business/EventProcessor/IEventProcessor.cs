namespace CommandService.Business.EventProcessor
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
