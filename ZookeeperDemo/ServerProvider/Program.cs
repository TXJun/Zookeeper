using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ServerProvider
{
    class Program
    {
        static void Main(string[] args)
        {
           

            Console.ReadLine();
        }

        static long ToInt(string addr)
        {
            return (long)(uint)IPAddress.NetworkToHostOrder(
                 (int)IPAddress.Parse(addr).Address);
        }

        static string ToAddr(long address)
        {
            return IPAddress.Parse(address.ToString()).ToString();
        }


        public static List<NetWork> GetNetWork()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            List<NetWork> nets = new List<NetWork>();
            List<string> macs = new List<string>();
            foreach (NetworkInterface adapter in nics)
            {
                Console.WriteLine("-----------------------------------");
                //判断是否是以太网连接  
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    //if (adapter.GetPhysicalAddress().ToString() != "")
                    //{
                    //    macs.Add(adapter.GetPhysicalAddress().ToString());
                    //}

                    NetWork net = new NetWork();
                    IPInterfaceProperties ip = adapter.GetIPProperties();     //IP配置信息  
                    //获取单播地址集
                    if (ip.UnicastAddresses.Count > 0)
                    {
                        UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                        foreach (UnicastIPAddressInformation ipadd in ipCollection)
                        {
                            if (ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                net.IP = ipadd.Address.ToString();//判断是否为ipv4
                                //net.SubMark = ipadd.IPv4Mask.ToString();
                            }
                            //if (ipadd.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            //{
                            //    net.IPV6 = ipadd.Address.ToString();//判断是否为ipv6
                            //    net.SubMarkV6 = ipadd.IPv4Mask.ToString();
                            //}                           
                        }

                        Console.WriteLine("IP地址 ：" + net.IP);
                        Console.WriteLine("子网掩码 ：" + net.SubMark);
                    }
                    if (ip.GatewayAddresses.Count > 0)
                    {
                        net.GateWay = ip.GatewayAddresses[0].Address.ToString();//默认网关  
                        Console.WriteLine("默认网关  ：" + net.GateWay);
                    }
                    int DnsCount = ip.DnsAddresses.Count;
                    if (DnsCount == 1)
                        net.DNS1 = ip.DnsAddresses[0].ToString(); //主DNS  
                    if (DnsCount == 2)
                        net.DNS2 = ip.DnsAddresses[1].ToString(); //备用DNS地址  
                    nets.Add(net);
                }
            }
            return nets;
        }
    }

    public class NetWork
    {
        public string DNS1 { get; set; }
        public string DNS2 { get; set; }

        public string GateWay { get; set; }

        public string IP { get; set; }
        //public string IPV6 { get; set; }

        public string SubMark { get; set; }
        //public string SubMarkV6 { get; set; }
    }
}
