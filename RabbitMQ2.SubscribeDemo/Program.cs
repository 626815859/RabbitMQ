using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ2.SubscribeDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            String queueName = "myQ2";
            String exchangeName = "wytExchange";
            String routeKeyName = "wytRouteKey";


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
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable:false, autoDelete: false, arguments: null);

                    //声明队列
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //将队列和交换机绑定
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKeyName, arguments: null);

                    //定义接收消息的消费者逻辑
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>              //事件处理
                    {
                        Byte[] body = ea.Body;
                        String message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] {0}", message);
                    };

                    //将消费者和队列绑定
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }
            }

        }
    }
}
