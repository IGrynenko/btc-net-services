using QueueServices.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace QueueServices.Services
{
    public class RabbitQueue : IRabbitQueue, IDisposable
    {
        public IModel Channel { get; private set; }

        public event EventHandler<BasicDeliverEventArgs> OnReceived
        {
            add { _consumer.Received += value; }
            remove { _consumer.Received -= value; }
        }

        private readonly IConnection _connection;
        private readonly EventingBasicConsumer _consumer;
        private bool _disposed = false;

        public RabbitQueue(IRabbitConnectionFactory rabbitFactory)
        {
            _connection = rabbitFactory.CreateConnection();
            Channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(Channel);
        }

        public void DeclareExchange(string exchangeName, string exchangeType = "direct")
        {
            if (exchangeType != null && !ExchangeType.All().Contains(exchangeType))
                throw new ArgumentException("Incorrect type");

            Channel.ExchangeDeclare(exchangeName, exchangeType);
        }

        public void CreateQueue(string queueName, string exhangeName)
        {
            CreateQueue(queueName, exhangeName, string.Empty);
        }

        public void CreateQueue(string queueName, string exhangeName, string routingKey)
        {
            CreateQueue(queueName, exhangeName, routingKey, false);
        }

        public void CreateQueue(string queueName, string exhangeName, string routingKey, bool durable)
        {
            Channel.QueueDeclare(queue: queueName,
                                 durable: durable,
                                 autoDelete: false,
                                 exclusive: false);
            Channel.QueueBind(queue: queueName,
                              exchange: exhangeName,
                              routingKey: routingKey);
        }

        public void SetBasicConsume(string queueName, bool autoAck)
        {
            Channel.BasicConsume(queue: queueName,
                                 autoAck: autoAck,
                                 consumer: _consumer);
        }

        public void SetBasicPublish(string exchangeName, ReadOnlyMemory<byte> body)
        {
            SetBasicPublish(exchangeName, body);
        }

        public void SetBasicPublish(string exchangeName, ReadOnlyMemory<byte> body, string routingKey)
        {
            SetBasicPublish(exchangeName, body, routingKey, Channel.CreateBasicProperties());
        }

        public void SetBasicPublish(string exchangeName, ReadOnlyMemory<byte> body, string routingKey, IBasicProperties basicProperties)
        {
            Channel.BasicPublish(exchange: exchangeName,
                                 routingKey: routingKey,
                                 basicProperties: basicProperties,
                                 body: body);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Channel?.Dispose();
                _connection?.Dispose();
            }

            _disposed = true;
        }

        ~RabbitQueue() => Dispose(false);
    }
}
