using Cl_Zookeeper.Registry.Entity;
/***************************************************************************** 
*        filename :ZkRedisProviderService 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ZkRedisProviderService 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service 
*        文件名:             ZkRedisProviderService 
*        创建系统时间:       2015/9/13 15:02:59 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Service
{
    /// <summary>
    /// 根据实际情况改动
    /// </summary>
    public class ZkRedisProviderService : ZkProviderService
    {
        public void Delete(int port)
        {
            this.Delete(ZkProviderService.GetLocalIp(), port);
        }

        public void Delete(string ip, int port)
        {
            string str = string.Format(NodePath.VServiceString, ClusterType.Redis);
            base._zooKeeper.Delete(string.Concat(new object[] { str, "/", ip, ":", port }));
        }

        public void Regist(int port, int weight)
        {
            this.Regist(ZkProviderService.GetLocalIp(), port, weight);
        }

        public void Regist(string ip, int port, int weight)
        {
            string str = string.Format(NodePath.VServiceString, ClusterType.Redis);
            base.Regist(port, weight, string.Concat(new object[] { str, "/", ip, ":", port }), null);
        }
    }
}
