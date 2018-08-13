using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weiz.Consumer
{
    public class OrderProcessMessage:MQ.IProcessMessage
    {
        public void ProcessMsg(MQ.MyMessage msg)
        {
            string str = string.Format("{0},{1},{2},{3}", msg.MessageID, msg.MessageTitle, msg.MessageRouter, msg.MessageBody);
            Console.WriteLine(str);
        }
    }
}