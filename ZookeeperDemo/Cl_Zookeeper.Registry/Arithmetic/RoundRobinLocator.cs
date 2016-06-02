/***************************************************************************** 
*        filename :LoadBalance 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   LoadBalance 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Arithmetic 
*        文件名:             LoadBalance 
*        创建系统时间:       2015/9/11 15:16:06 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cl_Zookeeper.Registry
{
    /// <summary>
    /// 加权轮询算法
    /// </summary>
    public class RoundRobinLocator
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object locker = new object();
        /// <summary>
        /// 服务器权重列表
        /// </summary>
        private static Dictionary<string, int> dictionary = new Dictionary<string, int>();
        /// <summary>
        /// 当前索引
        /// </summary>
        private static int currentIndex;
        /// <summary>
        /// 当前权重
        /// </summary>
        private static int currentWeight;
        /// <summary>
        /// 最大权重
        /// </summary>
        private static int maxWeight;
        /// <summary>
        /// 最大公约数
        /// </summary>
        private static int gcd;
        public RoundRobinLocator(Dictionary<string, int> _dictionary)
        {
            dictionary = _dictionary;
            currentIndex = -1;
            currentWeight = 0;
            maxWeight = GetMaxWeight(dictionary);
            gcd = GetMaxGCD(dictionary);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string Start()
        {
            lock (locker)
            {
                string serverHost = RoundRobin();
                if (serverHost != null)
                {
                    return serverHost;
                }
                return dictionary.Keys.ElementAt(0);
            }
        }

        /// <summary>
        /// 获取最大公约数
        /// </summary>
        /// <param name="list">要查找的int集合</param>
        /// <returns>返回集合中所有数的最大公约数</returns>
        private static int GetMaxGCD(Dictionary<string, int> dic)
        {
            Dictionary<string, int> list = dic.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            int iMinWeight = dictionary.Values.Min();
            int gcd = 1;
            for (int i = 1; i < iMinWeight; i++)
            {
                bool isFound = true;
                foreach (int iWeight in list.Values)
                {
                    if (iWeight % i != 0)
                    {
                        isFound = false;
                        break;
                    }
                }
                if (isFound) gcd = i;
            }
            return gcd;
        }
        /// <summary>
        /// 获取服务器权重集合中的最大权重
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static int GetMaxWeight(Dictionary<string, int> dic)
        {
            int iMaxWeight = 0;
            foreach (int i in dic.Values)
            {
                if (iMaxWeight < i) iMaxWeight = i;
            }
            return iMaxWeight;
        }
        /// <summary>
        /// 获取当前值
        /// </summary>
        /// <returns></returns>
        private static string RoundRobin()
        {
            while (true)
            {
                currentIndex = (currentIndex + 1) % dictionary.Count;
                if (0 == currentIndex)
                {
                    currentWeight = currentWeight - gcd;
                    if (0 >= currentWeight)
                    {
                        currentWeight = maxWeight;
                        if (currentWeight == 0) return null;
                    }
                }

                if (dictionary.Values.ElementAt(currentIndex) >= currentWeight)
                {
                    return dictionary.Keys.ElementAt(currentIndex);
                }
            }
        }
    }
}
