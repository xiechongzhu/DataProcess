using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Setting
{
    public class SettingManager
    {
        private readonly String NetworkSettingFile = "network";

        public bool LoadNetworkSetting(out String envIpAddr, out int envPort, out String flyIpAddr, out int flyPort)
        {
            try
            {
                using(FileStream file = File.Open(NetworkSettingFile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    NetworkSetting networkSetting = (NetworkSetting)formatter.Deserialize(file);
                    envIpAddr = networkSetting.EnvIpAddress;
                    envPort = networkSetting.EnvPort;
                    flyIpAddr = networkSetting.FlyIpAddress;
                    flyPort = networkSetting.FlyPort;
                }
            }
            catch(Exception)
            {
                envIpAddr = flyIpAddr = String.Empty;
                envPort = flyPort = 0;
                return false;
            }
            return true;
        }

        public bool SaveNetworkSetting(String envIpAddr, int envPort, String flyIpAddr, int flyPort)
        {
            NetworkSetting networkSetting = new NetworkSetting
            {
                EnvIpAddress = envIpAddr,
                EnvPort = envPort,
                FlyIpAddress = flyIpAddr,
                FlyPort = flyPort
            };

            try
            {
                using (FileStream file = File.Create(NetworkSettingFile))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, networkSetting);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
    }
}
