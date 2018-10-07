using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Worker
	{
		
        static void Main(string[] args)
        {
			var factory = new ConnectionFactory() { HostName = "localhost" };

			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(queue: "task_queue",
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);

					var consumer = new EventingBasicConsumer(channel);

					consumer.Received += (model, e) =>
					{
						var body = e.Body;
						string message = Encoding.UTF8.GetString(body);
						Console.WriteLine("=> Received : {0}", message);
						
						channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);

						int dots = 0;
						foreach (char c in message)
							if (c == '.') dots++;

						Console.WriteLine("=> Thread wait {0} secs", dots);
						Thread.Sleep(dots * 1000);

						Console.WriteLine("=> Done");

						
					};
					
					channel.BasicConsume(queue: "task_queue",
										 autoAck: false,
										 consumer: consumer);

					Console.WriteLine(" Press [enter] to exit.");
					Console.ReadLine();
				}
			}
		}
		
    }
}
