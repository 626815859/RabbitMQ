using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weiz.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderProcessMessage order = new OrderProcessMessage();
            MQ.MyMessage msg = new MQ.MyMessage();
            msg.MessageID = "1";
            msg.MessageRouter = "order.notice.lisi";

            MQ.MQHelper.Subscribe(msg, order);   //接受消息

            Console.WriteLine("Listening for messages.");

        }
    }
}
