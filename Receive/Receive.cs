using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Receive
    {
        static void Main(string[] args)
        {
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(queue: "hello",
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);
					channel.QueueDeclare(queue: "hello2",
										durable: false,
										exclusive: false,
										autoDelete: false,
										arguments: null);

					var consumer1 = new EventingBasicConsumer(channel);
					var consumer2 = new EventingBasicConsumer(channel);

					consumer1.Received += (model, e) =>
					{
						var body = e.Body;
						var message = Encoding.UTF8.GetString(body);
						Console.WriteLine("=> Received 1: {0}", message);
					};
					
					
					consumer2.Received += (model, e) =>
					{
						var body = e.Body;
						var message = Encoding.UTF8.GetString(body);
						Console.WriteLine("=> Received 2: {0}", message);
					};

					channel.BasicConsume(queue: "hello",
										 autoAck: true,
										 consumer: consumer1);

					channel.BasicConsume(queue: "hello2",
										 autoAck: true,
										 consumer: consumer2);

					Console.WriteLine(" Press [enter] to exit.");
					Console.ReadLine();
				}
			}
		}
    }
}
