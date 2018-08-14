using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeadersProduct
{
    class Program
    {
        static void Main(string[] args)
        {
            string exchangeName = "wytExchangeHeaders";

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
                    channel.ExchangeDeclare(exchange: exchangeName, type: "headers", durable: true, autoDelete: false, arguments: null);

                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<String, Object>()
                    {
                        {"user","wangpiao" },
                        {"password","Wp15228211255"}
                    };

                    Byte[] body = Encoding.UTF8.GetBytes("Hello World");

                    channel.BasicPublish(exchange: exchangeName, routingKey: String.Empty, basicProperties: properties, body: body);
                }
            }

            Console.Write("发布成功！");

            Console.ReadKey();



        }
    }
}
