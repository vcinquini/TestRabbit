using System;
using System.IO;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class ReceiveLogFile
	{
        static void Main(string[] args)
        {
			using (StreamWriter f = new StreamWriter(".\\log.txt"))
			{
				var factory = new ConnectionFactory() { HostName = "localhost" };
				using (var connection = factory.CreateConnection())
				{
					using (var channel = connection.CreateModel())
					{
						channel.ExchangeDeclare(exchange: "logs", type: "fanout");

						var queueName = channel.QueueDeclare().QueueName;

						channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

						Console.WriteLine("=> Waiting for logs.");

						var consumer = new EventingBasicConsumer(channel);

						consumer.Received += (model, e) =>
						{
							var body = e.Body;
							var message = Encoding.UTF8.GetString(body);


							f.WriteLine("=> Received: {0}", message);
							Console.WriteLine("=> Received: {0}", message);
						};

						channel.BasicConsume(queue: queueName,
											 autoAck: true,
											 consumer: consumer);

						Console.WriteLine(" Press [enter] to exit.");
						Console.ReadLine();

						f.Close();
					}
				}
			}
		}
    }
}
