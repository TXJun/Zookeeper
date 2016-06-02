/***************************************************************************** 
*        filename :IZookeeperRegistry 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   IZookeeperRegistry 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.ZooKeeper 
*        文件名:             IZookeeperRegistry 
*        创建系统时间:       2015/9/10 14:28:19 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cl_Zookeeper.Registry;

namespace Cl_Zookeeper.Registry
{
    /// <summary>
    /// Zookeeper操作接口
    /// </summary>
    public interface IZookeeperRegistry
    {
        IEnumerable<string> AddChildListener(string path, AbstractChildListener childListener);
        string AddDataListener(string path, AbstractDataListener dataListener);
        void AddStateListener(AbstractStateListener stateListener);
        void Close();
        void Create(string path, string data, bool persistent);
        void Delete(string path);
        bool Exists(string path);
        IEnumerable<string> GetChildren(string path);
        string GetData(string path);
        bool IsConnected();
        void SetData(string path, string data);
    }
}
