using Cl_Zookeeper.Registry.Entity;
/***************************************************************************** 
*        filename :ZkRpcProviderService 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ZkRpcProviderService 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service 
*        文件名:             ZkRpcProviderService 
*        创建系统时间:       2015/9/10 17:41:40 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Service
{
    public class ZkRpcProviderService : ZkProviderService
    {
        /// <summary>
        /// 向Zookeeper服务注册服务提供者IP信息
        /// </summary>
        /// <param name="port">服务提供Port</param>
        /// <param name="weight">权重</param>
        /// <param name="catelogs">服务类别</param>
        /// <param name="serviceName">服务名称</param>
        public void Regist(int port, int weight, Category catelogs, string serviceName)
        {
            this.Regist(ZkProviderService.GetLocalIp(), port, weight, catelogs, serviceName);
        }

        /// <summary>
        /// 向Zookeeper服务注册服务提供者IP信息
        /// </summary>
        /// <param name="ip">服务IP</param>
        /// <param name="port">服务提供Port</param>
        /// <param name="weight">权重</param>
        /// <param name="catelogs">服务类别</param>
        /// <param name="serviceName">服务名称</param>
        public void Regist(string ip, int port, int weight, Category catelogs, string serviceName)
        {
            if (string.IsNullOrEmpty(ip))
            {
                ip = ZkProviderService.GetLocalIp();
            }
            ProviderEntity providerEntity = new ProviderEntity
            {
                Status = 1,
                WorkStatus = 1,
                Address = ip + ":" + port,
                ServiceName = serviceName,
                Categorys = catelogs.ToString(),
                Modality = ClusterType.Rpc.ToString(),
                Version = "1.0.0.0",
                Weight = weight
            };
            base.Regist(port, weight, string.Format(NodePath.DClusterString, providerEntity.Modality, catelogs, serviceName) + "/" + providerEntity.Address, providerEntity);
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public string GetLocalIp()
        {
            return ZkProviderService.GetLocalIp();
        }
    }
}
