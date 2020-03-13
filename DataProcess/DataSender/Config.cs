using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataSender
{
    [Serializable]
    public class Config
    {
        private static readonly String ConfigFileName = "Config.xml";
        public String IpAddr { get; set; }
        public int Port { get; set; }

        public void Load()
        {
            try
            {
                using (StreamReader reader = new StreamReader(ConfigFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));
                    Config _config = serializer.Deserialize(reader) as Config;
                    IpAddr = _config.IpAddr;
                    Port = _config.Port;    
                }
            }
            catch(Exception)
            { }
        }

        public void Save()
        {
            try
            {
                using (FileStream fs = File.Create(ConfigFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));
                    serializer.Serialize(fs, this);
                }
            }
            catch (Exception)
            { }
        }
    }
}
