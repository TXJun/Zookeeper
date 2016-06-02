/***************************************************************************** 
*        filename :Category 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Category 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Entity 
*        文件名:             Category 
*        创建系统时间:       2015/9/10 14:25:22 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry.Entity
{
    /// <summary>
    /// 服务对象枚举
    /// </summary>
    public enum Category
    {
        Global,
        Usercenter,
        MfgApp,
        Zone,
        Bill
    }

    /// <summary>
    /// 集群方式枚举
    /// </summary>
    public enum ClusterType
    {
        Redis,
        Rpc
    }
}
