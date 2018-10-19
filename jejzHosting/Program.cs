using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace jejzHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Wcfjejz.Service1)))
            {
                host.Open();
                Console.WriteLine("host started\r\n" + DateTime.Now.ToString());
                Console.ReadLine();
            }  
        }
    }
}
