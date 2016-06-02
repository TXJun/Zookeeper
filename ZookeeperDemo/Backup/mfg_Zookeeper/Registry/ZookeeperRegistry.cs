namespace mfg_Zookeeper.Registry
{
    using Mfg.Comm.Log.LogHelper;
    using mfg_Zookeeper.Registry.Listener;
    using mfg_Zookeeper.Registry.ZooKeeper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ZooKeeperNet;

    internal class ZookeeperRegistry : IZookeeperRegistry
    {
        private readonly ZooKeeper _zKClient;

        public ZookeeperRegistry(string address)
        {
            this._zKClient = new ZooKeeper(address, TimeSpan.FromSeconds(5.0), null);
        }

        public List<string> AddChildListener(string path, AbstractChildListener childListener)
        {
            List<string> children = null;
            try
            {
                children = this._zKClient.GetChildren(path, childListener);
            }
            catch (Exception exception)
            {
                Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("获取子节点信息异常，异常信息：" + exception.Message);
            }
            return children;
        }

        public string AddDataListener(string path, AbstractDataListener dataListener)
        {
            try
            {
                byte[] bytes = this._zKClient.GetData(path, dataListener, null);
                if ((bytes != null) && (bytes.Length > 0))
                {
                    return Encoding.UTF8.GetString(bytes);
                }
                return string.Empty;
            }
            catch (Exception exception)
            {
                Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("设置节点数据发生异常，异常信息：" + exception.Message);
                return null;
            }
        }

        public void AddStateListener(AbstractStateListener stateListener)
        {
            try
            {
                this._zKClient.Register(stateListener);
            }
            catch (Exception exception)
            {
                Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("设置状态监测发生异常，异常信息：" + exception.Message);
            }
        }

        public void Close()
        {
            this._zKClient.Dispose();
        }

        public void Create(string path, string data, bool persistent)
        {
            byte[] bytes = null;
            Exception exception;
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    bytes = Encoding.UTF8.GetBytes(data);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    Mfg.Comm.Log.LogHelper.LogHelper.Log.Debug("节点数据序列号失败，异常信息：" + exception.Message);
                }
            }
            try
            {
                string[] source = path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                int length = source.Length;
                for (int i = 0; i < (length - 1); i++)
                {
                    string str = "/" + string.Join("/", source.Take<string>(i + 1));
                    if (!this.Exists(str))
                    {
                        this.CreatePersistent(str, null);
                    }
                }
                if (!persistent)
                {
                    this.CreateEphemeral(path, bytes);
                }
                else
                {
                    this.CreatePersistent(path, bytes);
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("创建节点发生异常，异常信息：" + exception.Message);
            }
        }

        private void CreateEphemeral(string path, byte[] data)
        {
            this._zKClient.Create(path, data, Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
        }

        private void CreatePersistent(string path, byte[] data)
        {
            this._zKClient.Create(path, data, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
        }

        public void Delete(string path)
        {
            try
            {
                this._zKClient.Delete(path, -1);
            }
            catch (Exception exception)
            {
                Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("删除节点发生异常，异常信息：" + exception.Message);
            }
        }

        public bool Exists(string path)
        {
            try
            {
                return (this._zKClient.Exists(path, false) != null);
            }
            catch (Exception exception)
            {
                Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("判断是否存在节点异常，异常信息：" + exception.Message);
                return false;
            }
        }

        public List<string> GetChildren(string path)
        {
            return this.AddChildListener(path, null);
        }

        public string GetData(string path)
        {
            return this.AddDataListener(path, null);
        }

        public bool IsConnected()
        {
            return this._zKClient.State.IsAlive();
        }

        public void SetData(string path, string data)
        {
            byte[] bytes = null;
            Exception exception;
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    bytes = Encoding.UTF8.GetBytes(data);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("节点数据序列号发生异常，异常信息：" + exception.Message);
                    return;
                }
            }
            try
            {
                this._zKClient.SetData(path, bytes, -1);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("节点数据设置异常，异常信息：" + exception.Message);
            }
        }
    }
}

