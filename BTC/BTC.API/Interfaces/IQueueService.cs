namespace BTC.API.Interfaces
{
    public interface IQueueService
    {
        void Publish(string message);
    }
}