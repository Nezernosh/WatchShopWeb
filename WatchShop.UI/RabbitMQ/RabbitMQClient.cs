using System.Text;
using RabbitMQ.Client;

namespace WatchShop.UI.RabbitMQ
{
    public class RabbitMQClient
    {
        string QUEUE;
        string rabbitHost;
        ConnectionFactory rabbitFactory;
        public RabbitMQClient()
        {
            QUEUE = "NOTIFICATION_QUEUE";
            rabbitHost = "rabbitmq";
            rabbitFactory = new ConnectionFactory()
            {
                HostName = rabbitHost,
                UserName = "guest",
                Password = "guest",
            };

        }
        public void Send(string message)
        {
            using (var rabbitConnection = rabbitFactory.CreateConnection())
            using (var rabbitChannel = rabbitConnection.CreateModel())
            {
                rabbitChannel.QueueDeclare(queue: QUEUE,
                                              durable: true,
                                              exclusive: false,
                                              autoDelete: false,
                                              arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                rabbitChannel.BasicPublish(exchange: "",
                               routingKey: QUEUE,
                               basicProperties: null,
                               body: body);
            }
        }
    }
}
