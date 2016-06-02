using Cl_Zookeeper.Registry;
/***************************************************************************** 
*        filename :DataListener 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   DataListener 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service.Listener 
*        文件名:             DataListener 
*        创建系统时间:       2015/9/10 14:51:58 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Service
{
    internal class DataListener : AbstractDataListener
    {
        private readonly ChildDataChange _childDataChange;
        private static DataListener _dataListener;
        private static readonly object ObjectLock = new object();

        private DataListener(IZookeeperRegistry zooKeeper, ChildDataChange childDataChange)
            : base(zooKeeper)
        {
            this._childDataChange = childDataChange;
        }

        public static DataListener CreateInstance(IZookeeperRegistry zooKeeper, ChildDataChange childDataChange)
        {
            if (_dataListener == null)
            {
                lock (ObjectLock)
                {
                    if (_dataListener == null)
                    {
                        _dataListener = new DataListener(zooKeeper, childDataChange);
                    }
                }
            }
            return _dataListener;
        }

        public override void DataChanged(string path, string data)
        {
            try
            {
                this._childDataChange(path, data);
            }
            catch (Exception exception)
            {
                //TODO 日志
                //Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("ChildDataChange：DataListener异常，异常信息：" + exception.Message);
            }
        }
    }
}
