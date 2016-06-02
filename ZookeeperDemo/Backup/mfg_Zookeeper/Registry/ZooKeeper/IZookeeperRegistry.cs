namespace mfg_Zookeeper.Registry.ZooKeeper
{
    using mfg_Zookeeper.Registry.Listener;
    using System;
    using System.Collections.Generic;

    public interface IZookeeperRegistry
    {
        List<string> AddChildListener(string path, AbstractChildListener childListener);
        string AddDataListener(string path, AbstractDataListener dataListener);
        void AddStateListener(AbstractStateListener stateListener);
        void Close();
        void Create(string path, string data, bool persistent);
        void Delete(string path);
        bool Exists(string path);
        List<string> GetChildren(string path);
        string GetData(string path);
        bool IsConnected();
        void SetData(string path, string data);
    }
}

