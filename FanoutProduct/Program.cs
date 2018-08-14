using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanoutProduct
{
    class Program
    {
        static void Main(string[] args)
        {
            string exchangeName = "wytExchange-Fanout";
            //string routeKey = "wytRouteKey";
            string message = "Hello World!";
           // string qName = "myQ2";

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
                    //声明exchange   设置为fanout模式不像direct模式通过routingkey来进行匹配，而是会把消息发送到所以的已经绑定的队列中。
                    channel.ExchangeDeclare(exchange: exchangeName, type: "fanout", durable: true, autoDelete: false, arguments: null);


                    IBasicProperties properties = channel.CreateBasicProperties();

                    properties.Persistent = true;

                    Task.Run(() =>
                    {
                        while (true)
                        {
                            for (int i = 0; i < 10000; i++)
                            {
                                Byte[] body = Encoding.UTF8.GetBytes(message + i);
                                channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: properties, body: body);
                            }
                            Thread.Sleep(100);
                        }
                    }).Wait();

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();


        }
    }
}
