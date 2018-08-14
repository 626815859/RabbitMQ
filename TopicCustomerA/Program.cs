using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace TopicCustomerA
{
    class Program
    {
        static void Main(string[] args)
        {
            String exchangeName = "wytExchangeTopic";
            String routeKeyName1 = "black.critical.high";

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "192.168.0.47";
            factory.Port = 5672;
            factory.VirtualHost = "/";
            factory.UserName = "wangpiao";
            factory.Password = "Wp15228211255";

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchangeName, type: "topic", durable: true, autoDelete: false, arguments: null);

                    String queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKeyName1, arguments: null);

                    //定义接收消息的消费者逻辑
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) => //事件
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();


                }
            }
        }
    }
}