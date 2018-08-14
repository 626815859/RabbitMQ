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
                    //声明交换机  direct模式  消费者收到消息处理后 需要发送应答ack
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


                        //ack:RabbitMQ默认是使用ACK确认机制的。当Consumer接收到RabbitMQ发布的消息时需要在适当的时机发送一个ACK确认的包来告知RabbitMQ，自己接收到了消息并成功处理。某一个Consumer在收到消息后没有发送ACK确认包，RabbitMQ就会认为Consumer还在处理任务，当有1个消息都没有发送ACK确认包时，RabbitMQ就不会再发送消息给该Consumer
                        //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);   //手动发送ack应答,如果使用，需要将下面BasicConsume的autoAck设置为false
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
