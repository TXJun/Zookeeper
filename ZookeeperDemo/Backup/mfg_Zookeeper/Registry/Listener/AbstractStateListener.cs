namespace mfg_Zookeeper.Registry.Listener
{
    using mfg_Zookeeper.Registry.ZooKeeper;
    using System;
    using ZooKeeperNet;

    public abstract class AbstractStateListener : IListener, IWatcher
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

