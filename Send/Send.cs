using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    class Send
    {
        static void Main(string[] args)
        {
			string message = "";
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

					Console.WriteLine("Type [exit] to exit.");

					while ((message = Console.ReadLine()) != "exit")
					{
						if (message == string.Empty)
						{
							message = @"{'employees':[
								{ 'firstName':'John', 'lastName':'Doe' },
								{ 'firstName':'Anna', 'lastName':'Smith' },
								{ 'firstName':'Peter', 'lastName':'Jones' }
							]}";
						}
						var body = Encoding.UTF8.GetBytes(message);

						channel.BasicPublish(exchange: "",
												routingKey: "hello",
												basicProperties: null,
												body: body);
						
						channel.BasicPublish(exchange: "",
												routingKey: "hello2",
												basicProperties: null,
												body: body);

						Console.WriteLine("=> Sent {0}", message);
					}
				}
			}
        }
    }
}
