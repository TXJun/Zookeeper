/***************************************************************************** 
*        filename :RootCustomerService 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   RootCustomerService 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service 
*        文件名:             RootCustomerService 
*        创建系统时间:       2015/9/11 10:42:53 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cl_Zookeeper.Registry.Entity;

namespace Cl_Zookeeper.Service
{
    /// <summary>
    /// 服务消费者
    /// /{CompanyName}/{ClusterType}/{Category}/{ServiceName}
    /// 1个应该下可能连接多个服务 例:MfgApp下连接教材、出题、评测等
    /// </summary>
    public class RootCustomerService : ZkCustomerService
    {
        private static readonly object LockObject = new object();

        /// <summary>
        /// 获取系统名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override string GetCategory(string path)
        {
            return path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[2];
        }
        /// <summary>
        /// 获取集群服务名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override string GetClusterName(string path)
        {
            return path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[3];
        }

        protected override object GetLockObject()
        {
            return LockObject;
        }

        /// <summary>
        /// 获取集群结点
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override string GetNodeName(string path)
        {
            return path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[4];
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="category"></param>
        /// <param name="clusterName"></param>
        /// <returns></returns>
        protected override string GetPath(Category category, string clusterName)
        {
            return string.Format(NodePath.DClusterString, ClusterType.Rpc, category, clusterName);
        }
    }
}
