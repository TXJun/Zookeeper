using Cl_Zookeeper.Registry;
using Cl_Zookeeper.Registry.Entity;
using Newtonsoft.Json;
/***************************************************************************** 
*        filename :ZkConsumerService 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ZkConsumerService 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service 
*        文件名:             ZkConsumerService 
*        创建系统时间:       2015/9/10 14:55:28 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Service
{
    /// <summary>
    /// 服务消费者基类
    /// </summary>
    public abstract class ZkCustomerService
    {
        private readonly Dictionary<string, List<ProviderEntity>> _clusterDictionary = new Dictionary<string, List<ProviderEntity>>();
        private readonly IZookeeperRegistry _zooKeeperClient;
        protected ZkCustomerService()
        {
            string str = ConfigurationManager.AppSettings["zooKeeperAddress"];
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("未配置zookeeper集群地址：zooKeeperAddress");
            }
            this._zooKeeperClient = ZookeeperRegistryFactory.CreateRegistry(str);
        }
        private ProviderEntity AddDataListener(string path)
        {
            string str = this._zooKeeperClient.AddDataListener(path, DataListener.CreateInstance(this._zooKeeperClient, new ChildDataChange(this.NodeDataChange)));
            if (!string.IsNullOrEmpty(str))
            {
                return JsonConvert.DeserializeObject<ProviderEntity>(str);
            }
            return null;
        }
        protected abstract string GetCategory(string path);
        protected abstract string GetClusterName(string path);
        public IList<ProviderEntity> GetHostList(Category category, string clusterName)
        {
            List<ProviderEntity> list;
            string key = category + "_" + clusterName;
            if (!this._clusterDictionary.TryGetValue(key, out list))
            {
                lock (this.GetLockObject())
                {
                    if (!this._clusterDictionary.TryGetValue(key, out list))
                    {
                        list = this.Subscribe(category, clusterName);
                    }
                }
            }
            if (list == null)
            {
                return null;
            }
            return (from m in list
                    where (m.Status == 1) && (m.WorkStatus == 1)
                    select m).ToList<ProviderEntity>();
        }
        protected abstract object GetLockObject();
        protected abstract string GetNodeName(string path);
        protected abstract string GetPath(Category category, string clusterName);
        private void NodeChange(string path, IEnumerable<string> children)
        {
            lock (this.GetLockObject())
            {
                string category = this.GetCategory(path);
                string clusterName = this.GetClusterName(path);
                if (!string.IsNullOrEmpty(clusterName))
                {
                    List<ProviderEntity> list;
                    string key = category + "_" + clusterName;
                    if (this._clusterDictionary.TryGetValue(key, out list))
                    {
                        using (IEnumerator<string> enumerator = children.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Predicate<ProviderEntity> match = null;
                                string child = enumerator.Current;
                                if (match == null)
                                {
                                    match = m => m.Address == child;
                                }
                                if (!list.Exists(match))
                                {
                                    ProviderEntity item = this.AddDataListener(path + "/" + child);
                                    if (item != null)
                                    {
                                        list.Add(item);
                                        if (this.AddDelegate != null)
                                        {
                                            this.AddDelegate(item);
                                        }
                                    }
                                }
                            }
                        }
                        int index = 0;
                        while (index < list.Count)
                        {
                            if (!children.Contains(list[index].Address))
                            {
                                if (this.RemoveDelegate != null)
                                {
                                    this.RemoveDelegate(list[index]);
                                }
                                list.RemoveAt(index);
                            }
                            else
                            {
                                index++;
                            }
                        }
                    }
                }
            }
        }
        private void NodeDataChange(string path, string data)
        {
            lock (this.GetLockObject())
            {
                string category = this.GetCategory(path);
                string clusterName = this.GetClusterName(path);
                string address = this.GetNodeName(path);
                if (!string.IsNullOrEmpty(clusterName) && !string.IsNullOrEmpty(address))
                {
                    string str3 = category + "_" + clusterName;
                    ProviderEntity providerEntity = JsonConvert.DeserializeObject<ProviderEntity>(data);
                    ProviderEntity entity2 = this._clusterDictionary[str3].SingleOrDefault<ProviderEntity>(m => m.Address == address);
                    if (entity2 != null)
                    {
                        if (((providerEntity.Status == 0) || (providerEntity.WorkStatus == 0)) && ((entity2.Status == 1) && (entity2.WorkStatus == 1)))
                        {
                            if (this.RemoveDelegate != null)
                            {
                                try
                                {
                                    this.RemoveDelegate(providerEntity);
                                }
                                catch
                                {
                                }
                            }
                        }
                        else if (((providerEntity.Status == 1) && (providerEntity.WorkStatus == 1)) && ((entity2.Status == 0) || (entity2.WorkStatus == 0)))
                        {
                            if (this.AddDelegate != null)
                            {
                                try
                                {
                                    this.AddDelegate(providerEntity);
                                }
                                catch
                                {
                                }
                            }
                        }
                        else if (((((providerEntity.Status == 1) && (providerEntity.WorkStatus == 1)) && ((entity2.Status == 1) && (entity2.WorkStatus == 1))) && (providerEntity.Weight != entity2.Weight)) && (this.UpdateDelegate != null))
                        {
                            try
                            {
                                this.UpdateDelegate(providerEntity);
                            }
                            catch
                            {
                            }
                        }
                        entity2.Modality = providerEntity.Modality;
                        entity2.Address = providerEntity.Address;
                        entity2.Categorys = providerEntity.Categorys;
                        entity2.ServiceName = providerEntity.ServiceName;
                        entity2.Status = providerEntity.Status;
                        entity2.Version = providerEntity.Version;
                        entity2.Weight = providerEntity.Weight;
                        entity2.WorkStatus = providerEntity.WorkStatus;
                    }
                }
            }
        }
        private List<ProviderEntity> Subscribe(Category category, string clusterName)
        {
            List<ProviderEntity> list;
            string key = category + "_" + clusterName;
            if (!this._clusterDictionary.TryGetValue(key, out list))
            {
                list = new List<ProviderEntity>();
                string path = this.GetPath(category, clusterName);
                if (!this._zooKeeperClient.Exists(path))
                {
                    this._zooKeeperClient.Create(path, string.Empty, true);
                }
                IEnumerable<string> list2 = this._zooKeeperClient.AddChildListener(path, ChildListener.CreateInstance(this._zooKeeperClient, new ChildChange(this.NodeChange)));
                if (list2 == null)
                {
                    return list;
                }
                foreach (string str3 in list2)
                {
                    ProviderEntity item = this.AddDataListener(path + "/" + str3);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
                this._clusterDictionary.Add(key, list);
            }
            return list;
        }
        public AddNodeDelegate AddDelegate { get; set; }
        public RemoveNodeDelegate RemoveDelegate { get; set; }
        public UpdateNodeDelegate UpdateDelegate { get; set; }
    }
}
