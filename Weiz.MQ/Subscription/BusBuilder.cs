using System;
using System.Configuration;
using EasyNetQ;

namespace Weiz.MQ
{
    /// <summary>
    /// ��Ϣ������������
    /// </summary>
    public class BusBuilder
    {
        public static IBus CreateMessageBus()
        {
            // ��Ϣ�����������ַ���
            // var connectionString = ConfigurationManager.ConnectionStrings["RabbitMQ"];
            string connString = "host=192.168.0.40:5672;virtualHost=OrderQueue;username=wangpiao;password=Wp15228211255";
            if (connString == null || connString == string.Empty)
            {
                throw new Exception("messageserver connection string is missing or empty");
            }
            
            return RabbitHutch.CreateBus(connString);
        }

       
    }

}