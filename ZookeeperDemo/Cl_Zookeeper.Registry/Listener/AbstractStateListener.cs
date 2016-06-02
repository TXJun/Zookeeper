/***************************************************************************** 
*        filename :AbstractStateListener 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   AbstractStateListener 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Listener 
*        文件名:             AbstractStateListener 
*        创建系统时间:       2015/9/10 14:29:30 
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
    public abstract class AbstractStateListener : IListener
    {
        public int Connected = 1;
        public int Disconnected = 0;
        public int Reconnected = 2;

        protected AbstractStateListener()
        {
        }

        public void Process(WatchedEvent @event)
        {
            if (@event.State == KeeperState.Disconnected)
            {
                this.StateChanged(this.Disconnected);
            }
            else if (@event.State == KeeperState.SyncConnected)
            {
                this.StateChanged(this.Connected);
            }
        }

        public abstract void StateChanged(int connected);
    }
}
