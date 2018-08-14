using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Sender2
{
    class Program
    {
        static void Main(string[] args)
        {
            ProduceMQ.Producer();
            Console.WriteLine("start");
        }
    }
}
