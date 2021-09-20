using BTC.API.Interfaces;
using Microsoft.Extensions.Configuration;
using QueueServices.Interfaces;
using QueueServices.Services;
using System;
using System.Text;

namespace BTC.API.Services
{
    public class RabbitService : IQueueService, IDisposable
    {
        private readonly string _exchange;
        private IRabbitQueue _iRabbitQueue;

        public RabbitService(IConfiguration configuration)
            : this(configuration.GetSection("Rabbit:Host").Value, configuration.GetSection("Rabbit:Url").Value, configuration.GetSection("Rabbit:Exchange").Value)
        { }

        public RabbitService(string host, string url, string exchange)
        {
            _exchange = exchange;
            var rabbitFactory = new RabbitConnectionFactory(host, url);
            _iRabbitQueue = new RabbitQueue(rabbitFactory);
            _iRabbitQueue.DeclareExchange(_exchange, "fanout");
        }

        public virtual void Publish(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException();

            if (_iRabbitQueue == null)
                throw new Exception("Initialize first"); //custom

            var body = Encoding.UTF8.GetBytes(message);

            _iRabbitQueue.SetBasicPublish(_exchange, body);         
        }

        public void Dispose()
        {
            _iRabbitQueue.Dispose();
        }
    }
}
