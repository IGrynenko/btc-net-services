using RabbitMQ.Client;

namespace QueueServices.Interfaces
{
    public interface IRabbitConnectionFactory
    {
        string HostName { get; }
        string Url { get; }

        IConnection CreateConnection();
    }
}