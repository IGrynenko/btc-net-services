using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTC.API.Services.QueueServices
{
    public class QueueService
    {
        //private readonly IConfiguration _configuration;

        //public QueueService(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public void Post()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                    // if we don't want to lost
                    //channel.QueueDeclare(queue: "test",
                    //                     durable: false, //set
                    //                     exclusive: false,
                    //                     autoDelete: false,
                    //                     arguments: null);

                    var message = "HELLO";
                    var body = Encoding.UTF8.GetBytes(message);

                    //num of messages to send for ... 
                    //channel.BasicQos(0, 1, false);

                    //persistent messages
                    //var properties = channel.CreateBasicProperties();
                    //properties.Persistent = true;

                    channel.BasicPublish(exchange: "logs",
                                         routingKey: "",
                                         basicProperties: null, //properties
                                         body: body);
                    Console.WriteLine("POST");
                }
            }
        }

        public void Consume()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                    //channel.QueueDeclare(queue: "test",
                    //                     durable: false, //set
                    //                     exclusive: false,
                    //                     autoDelete: false,
                    //                     arguments: null);

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange: "logs",
                                      routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        //Thread.Sleep(5000);

                        //notify we have processed
                        //channel.BasicAck(ea.DeliveryTag, false);

                        Console.WriteLine(message);
                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: false,
                                         consumer: consumer);

                    Console.WriteLine("CONSUME");
                    Console.ReadKey();
                }
            }
        }
    }
}
