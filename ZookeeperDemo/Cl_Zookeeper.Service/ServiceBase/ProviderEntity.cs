using Cl_Zookeeper.Registry.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cl_Zookeeper.Service
{
    public class ProviderEntity : ProviderBase
    {
        public int Status { get; set; }

        public int WorkStatus { get; set; }

        public override string ToString()
        {
            return string.Format("---Status：{0}\r\n---WorkStatus：{1}\r\n---Address：{2}\r\n---Weight：{3}\r\n---Categorys：{4}\r\n---Modality：{5}\r\n---ServiceName：{6}\r\n---Version:{7}",
                Status, WorkStatus, Address, Weight, Categorys, Modality, ServiceName, Version);
        }
    }
}
