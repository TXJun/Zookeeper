/***************************************************************************** 
*        filename :ConsumeBase 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ConsumeBase 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Entity 
*        文件名:             ConsumeBase 
*        创建系统时间:       2015/9/13 14:03:06 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry.Entity
{
    public class ConsumeBase : EntityBase
    {
        public string Address { get; set; }
    }
}
