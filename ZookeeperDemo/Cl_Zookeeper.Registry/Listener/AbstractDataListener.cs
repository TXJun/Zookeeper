/***************************************************************************** 
*        filename :AbstractDataListener 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   AbstractDataListener 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Listener 
*        文件名:             AbstractDataListener 
*        创建系统时间:       2015/9/10 14:29:21 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;
using Cl_Zookeeper.Registry;

namespace Cl_Zookeeper.Registry
{
    public abstract class AbstractDataListener : IListener
    {
        private readonly IZookeeperRegistry _zookeeperRegistry;

        protected AbstractDataListener(IZookeeperRegistry zooKeeper)
        {
            this._zookeeperRegistry = zooKeeper;
        }

        public abstract void DataChanged(string path, string data);
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeDataChanged)
            {
                string data = this._zookeeperRegistry.AddDataListener(@event.Path, this);
                if (data != null)
                {
                    this.DataChanged(@event.Path, data);
                }
            }
        }
    }
}
