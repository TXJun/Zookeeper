using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace KeeperNet
{
    public class KeeperHelper
    {
        private static ZooKeeper zk = null;

        public static string ROOT = "/root";
        public static string ROOT_MFGAPP = "/root/mfgapp";

        static KeeperHelper()
        {
            
            zk = new ZooKeeper("192.168.160.49:2181", new TimeSpan(0, 0, 1, 50000), new RootWatcher());
        }

        /// <summary>
        /// 创建结点
        /// </summary>
        /// <param name="path">结点路径</param>
        /// <param name="data">内容</param>
        /// <returns></returns>
        public static bool Create(string path, string data)
        {
            var isExists = zk.Exists(path, true);
            var tmpPath = string.Empty;
            if (isExists == null)
            {
                //创建一个节点,不进行ACL权限控制,节点为永久性的(即客户端shutdown了也不会消失) 
                tmpPath = zk.Create(path, data.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                zk.Exists(path, true);
                return tmpPath.Equals(path);
            }
            return true;
        }

        public static bool Create(string path, string data, IWatcher watcher)
        {
            var isExists = zk.Exists(path, watcher);
            var tmpPath = string.Empty;
            if (isExists == null)
            {
                //创建一个节点,不进行ACL权限控制,节点为永久性的(即客户端shutdown了也不会消失) 
                tmpPath = zk.Create(path, data.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                zk.Exists(path, watcher);
                return tmpPath.Equals(path);
            }
            return true;
        }

        public static bool Exists(string path, IWatcher watcher)
        {
            return zk.Exists(path, watcher) != null;
        }
    }
}
