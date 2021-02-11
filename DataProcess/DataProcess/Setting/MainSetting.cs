using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Setting
{
    [Serializable]
    public class MainSetting
    {
        public double StartLng;
        public double StartLat;
        public double EndLng;
        public double EndLat;
        public String MainText;
        public double BoomLineFront;
        public double BoomLineBack;
        public double BoomLineSide;
        public double PipeLength;
        public double PipeWidth;
    }
}
