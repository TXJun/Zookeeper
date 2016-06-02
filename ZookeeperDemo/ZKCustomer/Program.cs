using Cl_Zookeeper.Registry;
using Cl_Zookeeper.Registry.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZKCustomer
{
    class Program
    {
        private static System.Timers.Timer _timer = null;

        static void Main(string[] args)
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000 * 1;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            Console.ReadLine();
        }

        static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            HostInfo hostInfo = new ClusterHelper().GetService(Category.MfgApp, "Txj_Test", "abcde");
            if (hostInfo != null)
                Console.WriteLine(hostInfo.Host);
            else
                Console.WriteLine("集群中无可用节点");
        }
    }
}
