/***************************************************************************** 
*        filename :IListener 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   IListener 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.ZooKeeper 
*        文件名:             IListener 
*        创建系统时间:       2015/9/10 14:27:50 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace Cl_Zookeeper.Registry
{
    internal interface IListener : IWatcher
    {
    }
}
