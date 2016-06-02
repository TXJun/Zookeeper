/***************************************************************************** 
*        filename :HostInfo 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   HostInfo 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Entity 
*        文件名:             HostInfo 
*        创建系统时间:       2015/9/13 14:01:17 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry.Entity
{
    /// <summary>集群单个服务器对象
    /// </summary>
    public class HostInfo
    {
        /// <summary>主机信息 
        /// </summary>
        public string Host { get; set; }

        /// <summary>权重 
        /// </summary>
        public int Weight { get; set; }

        /// <summary>主机的连接池 
        /// </summary>
        //public PooledRedisClientManager PooledRedisClient { get; set; }
    }
}
