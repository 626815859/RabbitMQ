using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Sender2
{
  public  class ProduceMQ
    {
        public static void Producer()
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
                    Endpoint = new AmqpTcpEndpoint(uri)
                };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //设置交换器的类型
                        channel.ExchangeDeclare(exchangeName, exchangeType);
                        //声明一个队列，设置队列是否持久化，排他性，与自动删除
                        channel.QueueDeclare(qName, true, false, false, null);
                        //绑定消息队列，交换器，routingkey
                        channel.QueueBind(qName, exchangeName, routingKey);
                        var properties = channel.CreateBasicProperties();
                        //队列持久化
                        properties.Persistent = true;
                        var m = new QMessage() { CreatDateTime = DateTime.Now, Name = "测试", IdCard = 123456 };
                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(m));   //序列化
                        //发送信息
                        channel.BasicPublish(exchangeName, routingKey, properties, body);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
