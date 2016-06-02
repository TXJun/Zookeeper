/***************************************************************************** 
*        filename :ZookeeperRegistry 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ZookeeperRegistry 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry 
*        文件名:             ZookeeperRegistry 
*        创建系统时间:       2015/9/10 15:01:48 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace Cl_Zookeeper.Registry
{
    /// <summary>
    /// Zookeeper操作方法实现
    /// </summary>
    internal class ZookeeperRegistry : IZookeeperRegistry
    {
        private readonly ZooKeeperNet.ZooKeeper _zKClient;

        /// <summary>
        /// 初始化Zookeeper实例
        /// </summary>
        /// <param name="address">集群地址列表</param>
        public ZookeeperRegistry(string address)
        {
            try
            {
                this._zKClient = new ZooKeeperNet.ZooKeeper(address, TimeSpan.FromSeconds(5.0), null);
            }
            catch (Exception ex)
            {
                throw new Exception("获取子节点信息异常，异常信息：" + ex.Message);
            }
        }

        public IEnumerable<string> AddChildListener(string path, AbstractChildListener childListener)
        {
            IEnumerable<string> children = null;
            try
            {
                children = this._zKClient.GetChildren(path, childListener);
                // this._zKClient.GetChildren(path, childListener);
            }
            catch (Exception ex)
            {
                throw new Exception("获取子节点信息异常，异常信息：" + ex.Message);
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
            catch (Exception ex)
            {
                throw new Exception("设置节点数据发生异常，异常信息：" + ex.Message);
            }
        }

        public void AddStateListener(AbstractStateListener stateListener)
        {
            try
            {
                this._zKClient.Register(stateListener);
            }
            catch (Exception ex)
            {
                throw new Exception("设置状态监测发生异常，异常信息：" + ex.Message);
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
                    // Mfg.Comm.Log.LogHelper.LogHelper.Log.Debug("节点数据序列号失败，异常信息：" + exception.Message);
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
                //Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("创建节点发生异常，异常信息：" + exception.Message);
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
            catch (Exception ex)
            {
                throw new Exception("删除节点发生异常，异常信息：" + ex.Message);
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
                //Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("判断是否存在节点异常，异常信息：" + exception.Message);
                return false;
            }
        }

        public IEnumerable<string> GetChildren(string path)
        {
            return this.AddChildListener(path, null);
        }

        /// <summary>获取指定结点内容 无注册监听
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public string GetData(string path)
        {
            return this.AddDataListener(path, null);
        }

        /// <summary>ZooKeeper是否连接上
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return this._zKClient.State.IsAlive();
        }

        /// <summary>设置结点数据方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="data">内容</param>
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
                    //Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("节点数据序列号发生异常，异常信息：" + exception.Message);
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
                //Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("节点数据设置异常，异常信息：" + exception.Message);
            }
        }

    }
}
