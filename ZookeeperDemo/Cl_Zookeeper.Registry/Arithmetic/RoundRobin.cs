/***************************************************************************** 
*        filename :RoundRobin 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.34014 
*        新建项输入的名称:   RoundRobin 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Cl_Zookeeper.Registry.Arithmetic 
*        文件名:             RoundRobin 
*        创建系统时间:       2015/9/11 15:05:33 
*        创建年份:           2015 
/*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Registry
{
    public class RoundRobin
    {
        private Dictionary<string, int> dictionary = new Dictionary<string, int>();

        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 服务器权重列表
        /// </summary>
        private static List<int> weightList = new List<int>();

        /// <summary>
        /// 当前索引
        /// </summary>
        private static int currentIndex;

        /// <summary>
        /// 当前权重
        /// </summary>
        private static int currentWeight;

        private static int maxWeight;

        /// <summary>
        /// 最大公约数
        /// </summary>
        private static int gcd;

        static RoundRobin()
        {
            currentIndex = -1;
            currentWeight = 0;

            //获取服务器权重列表,从配置文件
            //weightList = GetWeightList();
            //maxWeight = GetMaxWeight(weightList);
            //gcd = GetMaxGCD(weightList);
        }

        /// <summary>
        /// 获取最大公约数
        /// </summary>
        /// <param name="list">要查找的int集合</param>
        /// <returns>返回集合中所有数的最大公约数</returns>
        private static int GetMaxGCD(List<int> list)
        {
            list.Sort(new WeightCompare());

            int iMinWeight = weightList[0];

            int gcd = 1;

            for (int i = 1; i < iMinWeight; i++)
            {
                bool isFound = true;
                foreach (int iWeight in list)
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
        private static int GetMaxWeight(List<int> list)
        {
            int iMaxWeight = 0;
            foreach (int i in list)
            {
                if (iMaxWeight < i) iMaxWeight = i;
            }
            return iMaxWeight;
        }

        //private static int RoundRobin()
        //{
        //    while (true)
        //    {
        //        currentIndex = (currentIndex + 1) % weightList.Count;
        //        if (0 == currentIndex)
        //        {
        //            currentWeight = currentWeight - gcd;
        //            if (0 >= currentWeight)
        //            {
        //                currentWeight = maxWeight;
        //                if (currentWeight == 0) return null;
        //            }
        //        }

        //        if (weightList[currentIndex] >= currentWeight)
        //        {
        //            return weightList[currentIndex];
        //        }
        //    }
        //}



    }
}
