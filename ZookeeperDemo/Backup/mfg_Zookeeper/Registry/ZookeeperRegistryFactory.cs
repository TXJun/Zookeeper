namespace mfg_Zookeeper.Registry
{
    using mfg_Zookeeper.Registry.ZooKeeper;
    using System;

    public class ZookeeperRegistryFactory
    {
        private static IZookeeperRegistry _zookeeperRegistry;
        private static readonly object LockObject = new object();

        public static IZookeeperRegistry CreateRegistry(string address)
        {
            if (_zookeeperRegistry == null)
            {
                lock (LockObject)
                {
                    if (_zookeeperRegistry == null)
                    {
                        _zookeeperRegistry = new ZookeeperRegistry(address);
                    }
                }
            }
            return _zookeeperRegistry;
        }
    }
}

