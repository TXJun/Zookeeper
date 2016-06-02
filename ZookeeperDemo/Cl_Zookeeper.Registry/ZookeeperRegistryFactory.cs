/***************************************************************************** 
*        filename :ZookeeperRegistryFactory 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ZookeeperRegistryFactory 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry 
*        文件名:             ZookeeperRegistryFactory 
*        创建系统时间:       2015/9/10 15:02:42 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry
{
    /// <summary>
    /// Zookeeper工厂类
    /// </summary>
    public class ZookeeperRegistryFactory
    {
        private static IZookeeperRegistry _zookeeperRegistry;
        private static readonly object LockObject = new object();
        /// <summary>
        /// 创建Zookeeper操作实例
        /// </summary>
        /// <param name="address">集群地址</param>
        /// <returns></returns>
        public static IZookeeperRegistry CreateRegistry(string address)
        {
            if (_zookeeperRegistry == null)
            {
                lock (LockObject)
                {
                    if (_zookeeperRegistry == null)
                    {
                        //实例化Zookeeper操作类
                        _zookeeperRegistry = new ZookeeperRegistry(address);
                    }
                }
            }
            return _zookeeperRegistry;
        }
    }
}
