using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
{
   public  class ConsumerMQ
    {
        public static void Consumer()
        {
            try
            {
                var qName = "lhtest1";
                var exchangeName = "fanoutchange1";
                var exchangeType = "fanout";//topic、fanout
                var routingKey = "*";
                var uri = new Uri("amqp://192.168.0.40:5672/");
                var factory = new ConnectionFactory
                {
                    UserName = "wangpiao",
                    Password = "Wp15228211255",
                    RequestedHeartbeat = 0,
                    Endpoint = new AmqpTcpEndpoint(uri),
                   // VirtualHost="Myhost" //默认为"/"
                };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchangeName, exchangeType);
                        channel.QueueDeclare(qName, true, false, false, null);
                        channel.QueueBind(qName, exchangeName, routingKey);
                        //定义这个队列的消费者
                        QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                        //false为手动应答，true为自动应答
                        channel.BasicConsume(qName, true, consumer);
                        while (true)
                        {
                            BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                            byte[] bytes = ea.Body;
                            var messageStr = Encoding.UTF8.GetString(bytes);
                            var message = DeserializeJsonToObj<QMessage>(messageStr);
                            Console.WriteLine("Receive a Message, DateTime:" + message.CreatDateTime.ToString("yyyy-MM-dd HH:mm:ss") + " Title:" + message.Name+message.IdCard);
                            ////如果是自动应答，下下面这句代码不用写啦。
                            //if ((Convert.ToInt32(message.Title) % 2) == 1)
                            //{
                            //    channel.BasicAck(ea.DeliveryTag, false);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static T DeserializeJsonToObj<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            T resault = o as T;
            return resault;
        }

    }
}
