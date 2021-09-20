using QueueServices.Interfaces;
using RabbitMQ.Client;
using System;

namespace QueueServices.Services
{
    public class RabbitConnectionFactory : IRabbitConnectionFactory
    {
        public string HostName { get; private set; }
        public string Url { get; private set; }

        public RabbitConnectionFactory(string hostName, string url)
        {
            HostName = hostName;
            Url = url;
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory { HostName = HostName };
            return factory.CreateConnection();
        }
    }
}
