using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ2.PublishDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string exchangeName = "wytExchange";
            string routeKey = "wytRouteKey";
            string message = "Hello World!";
            string qName = "myQ2";

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "192.168.0.47";
            factory.Port = 5672;
            factory.VirtualHost = "/";
            factory.UserName = "wangpiao";
            factory.Password = "Wp15228211255";
            

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //声明交换机（名称：log，类型：fanout（扇出））
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: false, autoDelete: false, arguments: null);


                    channel.QueueDeclare(qName, true, false, false, null);

                    channel.QueueBind(qName, exchangeName, routeKey);
                    Byte[] body = Encoding.UTF8.GetBytes(message);

                    //消息推送
                    channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, body: body);



                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
           
        }

    }
}
