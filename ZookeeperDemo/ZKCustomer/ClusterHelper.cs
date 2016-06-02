using Cl_Zookeeper.Registry;
using Cl_Zookeeper.Registry.Entity;
using Cl_Zookeeper.Service;
/***************************************************************************** 
*        filename :ServiceCluster 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ServiceCluster 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       ZKCustomer 
*        文件名:             ServiceCluster 
*        创建系统时间:       2015/9/11 10:25:47 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZKCustomer
{
    public class ClusterHelper
    {
        /// <summary>并行字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, ClusterInfo> ClusterInfoDictionary;

        /// <summary>读写共享线程锁  
        /// </summary>
        private static readonly ReaderWriterLockSlim ReaderWriterLock = new ReaderWriterLockSlim();

        /// <summary>zookeeper Client 
        /// </summary>
        private static readonly ZkCustomerService ZkCustomer;

        static ClusterHelper()
        {
            ClusterInfoDictionary = new ConcurrentDictionary<string, ClusterInfo>();
            ZkCustomer = new RootCustomerService()
            {
                AddDelegate = AddHost,
                RemoveDelegate = RemoveHost,
                UpdateDelegate = DataChange
            };
        }

        /// <summary>
        /// 根据分布式算法获取提供服务的节点
        /// </summary>
        /// <param name="category"></param>
        /// <param name="clusterName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public HostInfo GetService(Category category, string clusterName, string key)
        {
            ClusterInfo clusterInfo;
            ReaderWriterLock.EnterReadLock();
            //获取集群Key
            string clusterKey = GetClusterKey(category.ToString(), clusterName);
            bool exit = ClusterInfoDictionary.TryGetValue(clusterKey, out clusterInfo);
            ReaderWriterLock.ExitReadLock();
            if (!exit)
            {
                //未加载集群，需加载
                clusterInfo = InitCluster(category, clusterName);
                if (clusterInfo == null)
                    return null;
            }
            try
            {
                if (!clusterInfo.HostList.Any())
                    return null;
                //string host = clusterInfo.NodeLocator.GetPrimary(key);//一致性哈希算法
                string host = clusterInfo.RoundRobin.Start();
                HostInfo hostInfo = clusterInfo.HostList.SingleOrDefault(m => m.Host == host);
                if (hostInfo == null)
                    return null;
                return hostInfo;
            }
            catch (Exception exception)
            {
                //LogHelper.Log.Debug("RedisHelper-AddHost异常---" + exception.Message);
                return null;
            }
        }

        #region 私有方法
        static string GetClusterKey(string category, string clusterName)
        {
            return category.ToString() + "/" + clusterName;
        }

        /// <summary>
        /// 初始化集群 /root/Rpc/MfgApp/Txj_Test
        /// </summary>
        /// <param name="category">服务名称</param>
        /// <param name="clusterName">集群实例</param>
        /// <returns></returns>
        private ClusterInfo InitCluster(Category category, string clusterName)
        {
            string clusterKey = GetClusterKey(category.ToString(), clusterName);
            ReaderWriterLock.EnterWriteLock();
            try
            {
                ClusterInfo clusterInfo;
                if (!ClusterInfoDictionary.TryGetValue(clusterKey, out clusterInfo))
                {
                    clusterInfo = new ClusterInfo();
                    IEnumerable<ProviderEntity> providerEntities = ZkCustomer.GetHostList(category, clusterName);
                    clusterInfo.HostList = new List<HostInfo>();
                    Dictionary<string, int> dic = new Dictionary<string, int>();
                    foreach (ProviderEntity providerEntity in providerEntities)
                    {
                        HostInfo hostInfo = new HostInfo
                        {
                            Host = providerEntity.Address,
                            Weight = providerEntity.Weight,
                            //PooledRedisClient = new PooledRedisClientManager(new[] { providerEntity.Address },new[] { providerEntity.Address })
                        };
                        clusterInfo.HostList.Add(hostInfo);
                        dic.Add(providerEntity.Address, providerEntity.Weight);
                    }
                    clusterInfo.NodeLocator = new KetamaNodeLocator(dic);
                    clusterInfo.RoundRobin = new RoundRobinLocator(dic);
                    clusterInfo.HostDic = dic;
                    ClusterInfoDictionary.TryAdd(clusterKey, clusterInfo);
                }

                return clusterInfo;
            }
            catch (Exception exception)
            {
                //LogHelper.Log.Debug("RedisHelper-InitCluster异常---" + exception.Message);
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
            return null;
        }
        #endregion

        #region 回调事件
        /// <summary>增加节点 
        /// </summary>
        /// <param name="providerEntity">服务器信息</param>
        private static void AddHost(ProviderEntity providerEntity)
        {
            if (providerEntity == null)
                return;
            ReaderWriterLock.EnterWriteLock();
            string clusterKey = GetClusterKey(providerEntity.Categorys, providerEntity.ServiceName);
            try
            {
                ClusterInfo clusterInfo;
                if (ClusterInfoDictionary.TryGetValue(clusterKey, out clusterInfo))
                {
                    HostInfo hostInfo = new HostInfo
                    {
                        Host = providerEntity.Address,
                        //PooledRedisClient = new PooledRedisClientManager(new[] { providerEntity.Address },new[] { providerEntity.Address }),
                        Weight = providerEntity.Weight
                    };
                    clusterInfo.HostList.Add(hostInfo);
                    clusterInfo.HostDic.Add(providerEntity.Address, providerEntity.Weight);
                    clusterInfo.NodeLocator = new KetamaNodeLocator(clusterInfo.HostDic);
                    clusterInfo.RoundRobin = new RoundRobinLocator(clusterInfo.HostDic);
                    Console.WriteLine(string.Format("新增结节\r\n{0}", providerEntity.ToString()));
                }
            }
            catch (Exception exception)
            {
                //LogHelper.Log.Debug("RedisHelper-AddHost异常---" + exception.Message);
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
        }

        /// <summary>移出节点 
        /// </summary>
        /// <param name="providerEntity">服务器信息</param>
        private static void RemoveHost(ProviderEntity providerEntity)
        {
            string clusterKey = GetClusterKey(providerEntity.Categorys, providerEntity.ServiceName);
            ReaderWriterLock.EnterWriteLock();
            try
            {
                ClusterInfo clusterInfo;
                if (ClusterInfoDictionary.TryGetValue(clusterKey, out clusterInfo))
                {
                    if (clusterInfo.HostDic.Remove(providerEntity.Address))
                    {
                        HostInfo host = clusterInfo.HostList.Single(m => m.Host == providerEntity.Address);
                        //host.PooledRedisClient.Dispose();
                        clusterInfo.HostList.Remove(host);
                        clusterInfo.NodeLocator = new KetamaNodeLocator(clusterInfo.HostDic);
                        clusterInfo.RoundRobin = new RoundRobinLocator(clusterInfo.HostDic);
                    }
                }
                Console.WriteLine(string.Format("断开连接\r\n{0}", providerEntity.ToString()));
            }
            catch (Exception exception)
            {
                //LogHelper.Log.Debug("RedisHelper-RemoveHost异常---" + exception.Message);
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
        }

        /// <summary>节点数据改变 
        /// </summary>
        /// <param name="providerEntity">服务器信息</param>
        private static void DataChange(ProviderEntity providerEntity)
        {
            string clusterKey = GetClusterKey(providerEntity.Categorys, providerEntity.ServiceName);
            ReaderWriterLock.EnterWriteLock();
            try
            {
                ClusterInfo clusterInfo;
                if (ClusterInfoDictionary.TryGetValue(clusterKey, out clusterInfo))
                {
                    clusterInfo.HostDic[providerEntity.Address] = providerEntity.Weight;
                    HostInfo host = clusterInfo.HostList.Single(m => m.Host == providerEntity.Address);
                    host.Weight = providerEntity.Weight;
                    clusterInfo.NodeLocator = new KetamaNodeLocator(clusterInfo.HostDic);
                    clusterInfo.RoundRobin = new RoundRobinLocator(clusterInfo.HostDic);
                }
                Console.WriteLine(string.Format("节点数据改变\r\n{0}", providerEntity.ToString()));
            }
            catch (Exception exception)
            {
                //LogHelper.Log.Debug("RedisHelper-RemoveHost异常---" + exception.Message);
            }
            finally
            {
                ReaderWriterLock.ExitWriteLock();
            }
        }
        #endregion
    }
}
