namespace mfg_Zookeeper.Registry.Listener
{
    using mfg_Zookeeper.Registry.ZooKeeper;
    using System;
    using ZooKeeperNet;

    public abstract class AbstractDataListener : IListener, IWatcher
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

