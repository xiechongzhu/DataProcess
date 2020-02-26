using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Setting
{
    [Serializable]
    public class NetworkSetting
    {
        public String EnvIpAddress { get; set; }
        public int EnvPort { get; set; }
        public String FlyIpAddress { get; set; }
        public int FlyPort { get; set; }
    }
}
