/***************************************************************************** 
*        filename :ProviderBase 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ProviderBase 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Entity 
*        文件名:             ProviderBase 
*        创建系统时间:       2015/9/13 14:02:45 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry.Entity
{
    public class ProviderBase : EntityBase
    {
        /// <summary>
        /// 服务提供者地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }
    }
}
