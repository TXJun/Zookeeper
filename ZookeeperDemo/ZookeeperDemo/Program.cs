using KeeperNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace ZookeeperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var root="/root";

            KeeperHelper.Create(KeeperHelper.ROOT, "mf");

            ZkClient c = new ZkClient();
            Console.ReadLine();
            //ZooKeeper zk = KeeperHelper.zk;
            //var ac = zk.Exists("/root", true);
            //var acr = zk.Exists("/root/rt", true);
            //string str = zk.Create("/root/mfgapp/r5", "192.168.160.49:6397".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            //创建一个Zookeeper实例，第一个参数为目标服务器地址和端口，第二个参数为Session超时时间，第三个为节点变化时的回调方法 
            //zk.Exists("/root", true);

            //var stat = zk.GetChildren("/root/mfgapp", new KeeperNet.Watcher());



            //zk.SetData("/root", "a".GetBytes(), -1);
            ////创建一个节点root，数据是mydata,不进行ACL权限控制，节点为永久性的(即客户端shutdown了也不会消失) 
            //zk.Create("/root", "mydata".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            //zk.Create("/root/mfgapp", "mfgapp".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);

            //zk.Create("/root/mfgapp/r1", "192.168.160.49:6397".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            //zk.Create("/root/mfgapp/r2", "192.168.160.50:6397".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            //zk.Create("/root/mfgapp/r3", "192.168.160.51:6397".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            //zk.SetData("/root/mfgapp/r1", "childos".GetBytes(), -1);
            //zk.Exists("/root", true);

            // 这个方法的意思就是只要Task Factory与zookeeper断开连接后，这个节点就会被自动删除
            //WatchedEvent event=new WatchedEvent(EventType.NodeDeleted,KeeperState.SyncConnected,"/root");


            //var cd = zk.GetData("/root/mfgapp/r1", true, null); ;


            //Console.WriteLine(System.Text.Encoding.Default.GetString(cd));
            //var stat1 = zk.Exists("/root/mfgapp/r1", new RootWatcher());
            //var stat2 = zk.Exists("/root/mfgapp/r2", new RootWatcher());
            //var stat3 = zk.Exists("/root/mfgapp/r3", new RootWatcher());

            ////在root下面创建一个childone znode,数据为childone,不进行ACL权限控制，节点为永久性的 
            //zk.Create("/root/childone", "childone".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            ////取得/root节点下的子节点名称,返回List<String> 
            //zk.GetChildren("/root", true);
            ////取得/root/childone节点下的数据,返回byte[] 
            //zk.GetData("/root/childone", true, null);

            //修改节点/root/childone下的数据，第三个参数为版本，如果是-1，那会无视被修改的数据版本，直接改掉
            //zk.SetData("/root/childone", "childonemodify".GetBytes(), -1);
            //删除/root/childone这个节点，第二个参数为版本，－1的话直接删除，无视版本 
            //zk.Delete("/root/childone", -1);

            for (int i = 0; i < 100000; i++)
            {
                var cmd = Console.ReadLine();
                string[] conten = cmd.Split(' ');
                if (conten.Length < 2)
                {
                    Console.WriteLine("该命令不存在");
                    continue;
                }
                try
                {
                    var cmdHead = conten[0];
                    switch (cmdHead)
                    {
                        case "ls":
                            Ls(conten[1]);
                            break;
                        case "ls2":
                            Console.WriteLine("-->" + cmdHead);
                            break;
                        case "3":
                            Console.WriteLine("-->" + cmdHead);
                            break;
                        case "4":
                            Console.WriteLine("-->" + cmdHead);
                            break;
                        case "5":
                            Console.WriteLine("-->" + cmdHead);
                            break;
                        default:
                            Console.WriteLine("-->" + cmdHead);
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("该命令执行异常");
                    continue;
                }

            }
            Console.ReadLine();
        }

        public static string Ls(string path)
        {
            return "123123";
        }
    }
}