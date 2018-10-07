using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    class SendLog
    {
        static void Main(string[] args)
        {
			string message = "";
			var factory = new ConnectionFactory() { HostName = "localhost" };

			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.ExchangeDeclare(exchange: "logs", type: "fanout");

					while((message = Console.ReadLine()) != String.Empty)
					{
						byte[] body = Encoding.UTF8.GetBytes(message);

						channel.BasicPublish(exchange: "logs",
												routingKey: "",
												basicProperties: null,
												body: body);

						Console.WriteLine("=> Sent {0}", message);
					}
				}
			}
        }
	}
}
