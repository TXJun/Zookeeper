/***************************************************************************** 
*        filename :AbstractChildListener 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   AbstractChildListener 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Listener 
*        文件名:             AbstractChildListener 
*        创建系统时间:       2015/9/10 14:29:06 
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
    /// <summary>
    /// 该节点的子节点改变监听事件
    /// </summary>
    public abstract class AbstractChildListener : IListener,IWatcher
    {
        private readonly IZookeeperRegistry _zookeeperRegistry;
        private static readonly object LockObject = new object();

        /// <summary>
        /// 初始化Zookeeper操作类
        /// </summary>
        /// <param name="zooKeeper"></param>
        protected AbstractChildListener(IZookeeperRegistry zooKeeper)
        {
            this._zookeeperRegistry = zooKeeper;
        }

        /// <summary>
        /// 子节点改变的抽象方法，继承方法需要重新实现
        /// </summary>
        /// <param name="path"></param>
        /// <param name="children"></param>
        public abstract void ChildrenChanged(string path, IEnumerable<string> children);

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="@event"></param>
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeChildrenChanged)
            {
                lock (LockObject)
                {
                    //重新向该节点注册监听事件
                    IEnumerable<string> children = this._zookeeperRegistry.AddChildListener(@event.Path, this);
                    //通知方法
                    this.ChildrenChanged(@event.Path, children);
                }
            }
        }
    }
}
