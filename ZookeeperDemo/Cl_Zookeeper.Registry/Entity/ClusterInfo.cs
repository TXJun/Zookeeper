/***************************************************************************** 
*        filename :ClusterInfo 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ClusterInfo 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Entity 
*        文件名:             ClusterInfo 
*        创建系统时间:       2015/9/13 14:00:42 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry.Entity
{
    /// <summary>
    /// 集群对象类
    /// </summary>
    public class ClusterInfo
    {
        /// <summary>集群的节点列表 
        /// </summary>
        public List<HostInfo> HostList { get; set; }

        /// <summary>一致性哈希算法
        /// </summary>
        public KetamaNodeLocator NodeLocator { get; set; }

        /// <summary>加权轮询算法
        /// </summary>
        public RoundRobinLocator RoundRobin { get; set; }

        /// <summary>
        /// 机器名称及权重
        /// </summary>
        public Dictionary<string, int> HostDic { get; set; }
    }
}
