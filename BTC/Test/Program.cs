using BTC.API.Services;
using BTC.API.Services.QueueServices;
using QueueServices.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var rabbitFactory = new RabbitConnectionFactory("localhost", "amqp://guest:guest@localhost:15672/");
            var rabbit = new RabbitQueue(rabbitFactory);
            rabbit.DeclareExchange("logs", "fanout");
            rabbit.CreateQueue("logs", "logs");
            rabbit.SetBasicConsume("logs", true);
            rabbit.OnReceived += (m, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine(message);
            };

            Console.ReadKey();
        }
    }
}
