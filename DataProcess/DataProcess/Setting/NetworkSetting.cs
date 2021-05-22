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
        public String EnvIpAddressHeigh { get; set; }
        public int EnvPortHeigh { get; set; }
        public String FlyIpAddressHeigh { get; set; }
        public int FlyPortHeigh { get; set; }
        public String YaoCeIpAddressHeigh { get; set; }
        public int YaoCePortHeigh { get; set; }

        public String EnvIpAddressMiddle { get; set; }
        public int EnvPortMiddle { get; set; }
        public String FlyIpAddressMiddle { get; set; }
        public int FlyPortMiddle { get; set; }
        public String YaoCeIpAddressMiddle { get; set; }
        public int YaoCePortMiddle { get; set; }

        public String EnvIpAddressLow { get; set; }
        public int EnvPortLow { get; set; }
        public String FlyIpAddressLow { get; set; }
        public int FlyPortLow { get; set; }
        public String YaoCeIpAddressLow { get; set; }
        public int YaoCePortLow { get; set; }

        public int idleTime { get; set; }
        public int MaxDisplayPoint { get; set; }
    }
}
