using Cl_Zookeeper.Registry.Entity;
using Cl_Zookeeper.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZKProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            var provider = new ZkRpcProviderService();
            provider.Regist(Port, 2, Category.MfgApp, "Txj_Test");

            Console.WriteLine(provider.GetLocalIp() + "：" + Port);

            Console.ReadLine();
        }
    }
}
