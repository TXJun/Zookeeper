using Cl_Zookeeper.Registry;
/***************************************************************************** 
*        filename :ChildListener 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   ChildListener 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Service.Listener 
*        文件名:             ChildListener 
*        创建系统时间:       2015/9/10 14:46:36 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Service
{
    internal class ChildListener : AbstractChildListener
    {
        private readonly ChildChange _childChange;
        private static ChildListener _childListener;
        private static readonly object LockObject = new object();

        private ChildListener(IZookeeperRegistry zooKeeper, ChildChange childChange)
            : base(zooKeeper)
        {
            this._childChange = childChange;
        }

        public override void ChildrenChanged(string path, IEnumerable<string> children)
        {
            try
            {
                this._childChange(path, children);
            }
            catch (Exception exception)
            {
                //TODO 日志
                //Mfg.Comm.Log.LogHelper.LogHelper.Log.Error("ChildListener-ChildrenChanged异常，异常信息：" + exception.Message);
            }
        }

        public static ChildListener CreateInstance(IZookeeperRegistry zooKeeper, ChildChange childChange)
        {
            if (_childListener == null)
            {
                lock (LockObject)
                {
                    if (_childListener == null)
                    {
                        _childListener = new ChildListener(zooKeeper, childChange);
                    }
                }
            }
            return _childListener;
        }
    }
}
