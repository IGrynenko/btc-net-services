using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace QueueServices.Interfaces
{
    public interface IRabbitQueue
    {
        IModel Channel { get; }

        event EventHandler<BasicDeliverEventArgs> OnReceived;

        void CreateQueue(string queueName, string exhangeName);
        void CreateQueue(string queueName, string exhangeName, string routingKey);
        void CreateQueue(string queueName, string exhangeName, string routingKey, bool durable);
        void DeclareExchange(string exchangeName, string exchangeType = "direct");
        void Dispose();
        void SetBasicConsume(string queueName, bool autoAck);
        void SetBasicPublish(string exchangeName, ReadOnlyMemory<byte> body);
        void SetBasicPublish(string exchangeName, ReadOnlyMemory<byte> body, string routingKey);
        void SetBasicPublish(string exchangeName, ReadOnlyMemory<byte> body, string routingKey, IBasicProperties basicProperties);
    }
}