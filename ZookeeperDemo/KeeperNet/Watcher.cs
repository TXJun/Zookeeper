/***************************************************************************** 
*        filename :Watcher 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   Watcher 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       KeeperNet 
*        文件名:             Watcher 
*        创建系统时间:       2015/8/25 15:29:25 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace KeeperNet
{
    public class Watcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            switch (@event.Type)
            {
                case EventType.NodeChildrenChanged:
                    Console.WriteLine("已经触发了 [子结点改变]事件！");
                    break;
                case EventType.NodeCreated:
                    Console.WriteLine("已经触发了[结点创建]事件！");
                    break;
                case EventType.NodeDataChanged:
                    Console.WriteLine("已经触发了【结点数据变化】事件！");
                    break;
                case EventType.NodeDeleted:
                    Console.WriteLine("已经触发了结点删除事件！");
                    break;
                case EventType.None:
                    Console.WriteLine("已经触发了【None】事件！");
                    break;
                default:
                    break;
            }
            KeeperHelper.Exists(KeeperHelper.ROOT_MFGAPP, new Watcher());
        }
    }

    public class RootWatcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("Root结点已经触发了" + @event.Type + "事件！");
            KeeperHelper.Exists(KeeperHelper.ROOT, new Watcher());
        }
    }
}
