/***************************************************************************** 
*        filename :ChildChange 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ChildChange 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service 
*        文件名:             ChildChange 
*        创建系统时间:       2015/9/10 14:44:22 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Service
{
    internal delegate void ChildChange(string path, IEnumerable<string> children);
}
