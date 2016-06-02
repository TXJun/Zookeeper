/***************************************************************************** 
*        filename :EntityBase 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   EntityBase 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Entity 
*        文件名:             EntityBase 
*        创建系统时间:       2015/9/13 14:02:24 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry.Entity
{
    public class EntityBase
    {
        /// <summary>
        /// 服务名称 Global、Usercenter、MfgApp、Zone、Bill
        /// </summary>
        public string Categorys { get; set; }
        /// <summary>
        /// 集群类别 Redis、Rpc
        /// </summary>
        public string Modality { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
