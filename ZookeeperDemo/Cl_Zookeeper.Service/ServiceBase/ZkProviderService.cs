using Cl_Zookeeper.Registry;
using Newtonsoft.Json;
/***************************************************************************** 
*        filename :ZkProviderService 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ZkProviderService 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service 
*        文件名:             ZkProviderService 
*        创建系统时间:       2015/9/10 15:07:33 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Cl_Zookeeper.Service
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public abstract class ZkProviderService
    {
        protected readonly IZookeeperRegistry _zooKeeper;

        /// <summary>
        /// Zookeeper监控服务提供者服务
        /// </summary>
        protected ZkProviderService()
        {
            string str = ConfigurationManager.AppSettings["zooKeeperAddress"];
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("未配置zookeeper集群地址：zooKeeperAddress");
            }
            this._zooKeeper = ZookeeperRegistryFactory.CreateRegistry(str);
        }

        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        protected static string GetLocalIp()
        {
            IPAddress[] addressArray = (from m in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                                        where m.AddressFamily == AddressFamily.InterNetwork
                                        select m).ToArray<IPAddress>();
            if (addressArray.Length == 1)
            {
                return addressArray[0].ToString();
            }
            foreach (IPAddress address in addressArray)
            {
                if (address.ToString().StartsWith("192."))
                {
                    return address.ToString();
                }
            }
            return addressArray[1].ToString();
        }

        /// <summary>
        /// 向Zookeeper注册服务
        /// </summary>
        /// <param name="port"></param>
        /// <param name="weight"></param>
        /// <param name="path"></param>
        /// <param name="providerEntity"></param>
        protected void Regist(int port, int weight, string path, ProviderEntity providerEntity)
        {         
            if (this._zooKeeper.Exists(path))
            {
                this._zooKeeper.Delete(path);
            }
            this._zooKeeper.Create(path, (providerEntity == null) ? string.Empty : JsonConvert.SerializeObject(providerEntity), false);
        }
    }
}
