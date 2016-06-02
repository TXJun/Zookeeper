namespace mfg_Zookeeper.Registry.Listener
{
    using mfg_Zookeeper.Registry.ZooKeeper;
    using System;
    using System.Collections.Generic;
    using ZooKeeperNet;

    public abstract class AbstractChildListener : IListener, IWatcher
    {
        private readonly IZookeeperRegistry _zookeeperRegistry;
        private static readonly object LockObject = new object();

        protected AbstractChildListener(IZookeeperRegistry zooKeeper)
        {
            this._zookeeperRegistry = zooKeeper;
        }

        public abstract void ChildrenChanged(string path, List<string> children);
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeChildrenChanged)
            {
                lock (LockObject)
                {
                    List<string> children = this._zookeeperRegistry.AddChildListener(@event.Path, this);
                    this.ChildrenChanged(@event.Path, children);
                }
            }
        }
    }
}

