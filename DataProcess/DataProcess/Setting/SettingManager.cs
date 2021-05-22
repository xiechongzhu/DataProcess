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
        private readonly String RatioSettingFile = "params";
        private readonly String VideoSettingFile = "video";
        private readonly String MainSettingFile = "main";

        public bool LoadMainSetting(out MainSetting mainSetting)
        {
            try
            {
                using (FileStream file = File.Open(MainSettingFile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    mainSetting = (MainSetting)formatter.Deserialize(file);
                }
            }
            catch(Exception)
            {
                mainSetting = new MainSetting();
                return false;
            }
            return true;
        }

        public bool LoadVideoSetting(out VideoSetting videoSetting)
        {
            try
            {
                using(FileStream file = File.Open(VideoSettingFile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    videoSetting = (VideoSetting)formatter.Deserialize(file);
                }
            }
            catch(Exception)
            {
                videoSetting = null;
                return false;
            }
            return true;
        }

        public bool LoadNetworkSetting(out String envIpAddrHeigh, out int envPortHeigh,
                                       out String flyIpAddrHeigh, out int flyPortHeigh,
                                       out String yaoceIpAddrHeigh, out int yaocePortHeigh,
                                       out String envIpAddrMiddle, out int envPortMiddle,
                                       out String flyIpAddrMiddle, out int flyPortMiddle,
                                       out String yaoceIpAddrMiddle, out int yaocePortMiddle,
                                       out String envIpAddrLow, out int envPortLow,
                                       out String flyIpAddrLow, out int flyPortLow,
                                       out String yaoceIpAddrLow, out int yaocePortLow,
                                       out int idleTime, out int maxDisplayPoint)
        {
            try
            {
                using(FileStream file = File.Open(NetworkSettingFile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    NetworkSetting networkSetting = (NetworkSetting)formatter.Deserialize(file);

                    envIpAddrHeigh = networkSetting.EnvIpAddressHeigh;
                    envPortHeigh = networkSetting.EnvPortHeigh;
                    flyIpAddrHeigh = networkSetting.FlyIpAddressHeigh;
                    flyPortHeigh = networkSetting.FlyPortHeigh;
                    yaoceIpAddrHeigh = networkSetting.YaoCeIpAddressHeigh;
                    yaocePortHeigh = networkSetting.YaoCePortHeigh;

                    envIpAddrMiddle = networkSetting.EnvIpAddressMiddle;
                    envPortMiddle = networkSetting.EnvPortMiddle;
                    flyIpAddrMiddle = networkSetting.FlyIpAddressMiddle;
                    flyPortMiddle = networkSetting.FlyPortMiddle;
                    yaoceIpAddrMiddle = networkSetting.YaoCeIpAddressMiddle;
                    yaocePortMiddle = networkSetting.YaoCePortMiddle;

                    envIpAddrLow = networkSetting.EnvIpAddressLow;
                    envPortLow = networkSetting.EnvPortLow;
                    flyIpAddrLow = networkSetting.FlyIpAddressLow;
                    flyPortLow = networkSetting.FlyPortLow;
                    yaoceIpAddrLow = networkSetting.YaoCeIpAddressLow;
                    yaocePortLow = networkSetting.YaoCePortLow;

                    idleTime = networkSetting.idleTime;
                    maxDisplayPoint = networkSetting.MaxDisplayPoint;
                }
            }
            catch(Exception)
            {
                envIpAddrHeigh = flyIpAddrHeigh = yaoceIpAddrHeigh = envIpAddrMiddle = flyIpAddrMiddle = yaoceIpAddrMiddle
                   = envIpAddrLow = flyIpAddrLow = yaoceIpAddrLow = String.Empty;
                envPortHeigh = flyPortHeigh = yaocePortHeigh = envPortMiddle = flyPortMiddle = yaocePortMiddle
                    = envPortLow = flyPortLow = yaocePortLow = 0;
                idleTime = 1000;
                maxDisplayPoint = 1000;
                return false;
            }
            return true;
        }

        public bool LoadRatios(out Ratios ratios)
        {
            try
            {
                using(FileStream file = File.Open(RatioSettingFile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    ratios = (Ratios)formatter.Deserialize(file);
                }
            }
            catch(Exception)
            {
                ratios = new Ratios();
                return false;
            }
            return true;
        }

        public bool LoadRatios(String ratiosFile, out Ratios ratios)
        {
            try
            {
                using (FileStream file = File.Open(ratiosFile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    ratios = (Ratios)formatter.Deserialize(file);
                }
            }
            catch (Exception)
            {
                ratios = new Ratios();
                return false;
            }
            return true;
        }

        public bool SaveMainSetting(MainSetting mainSetting)
        {
            try
            {
                using(FileStream file = File.Create(MainSettingFile))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, mainSetting);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        public bool SaveVideoSetting(VideoSetting videoSetting)
        {
            try
            {
                using (FileStream file = File.Create(VideoSettingFile))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, videoSetting);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        public bool SaveNetworkSetting(String envIpAddrHeigh, int envPortHeigh,
                                       String flyIpAddrHeigh, int flyPortHeigh,
                                       String yaoceIpAddrHeigh, int yaocePortHeigh,
                                       String envIpAddrMiddle, int envPortMiddle,
                                       String flyIpAddrMiddle, int flyPortMiddle,
                                       String yaoceIpAddrMiddle, int yaocePortMiddle,
                                       String envIpAddrLow, int envPortLow,
                                       String flyIpAddrLow, int flyPortLow,
                                       String yaoceIpAddrLow, int yaocePortLow,
                                       int idleTime, int maxDisplayPoint)
        {
            NetworkSetting networkSetting = new NetworkSetting
            {
                EnvIpAddressHeigh = envIpAddrHeigh,
                EnvPortHeigh = envPortHeigh,
                FlyIpAddressHeigh = flyIpAddrHeigh,
                FlyPortHeigh = flyPortHeigh,
                YaoCeIpAddressHeigh = yaoceIpAddrHeigh,
                YaoCePortHeigh = yaocePortHeigh,

                EnvIpAddressMiddle = envIpAddrMiddle,
                EnvPortMiddle = envPortMiddle,
                FlyIpAddressMiddle = flyIpAddrMiddle,
                FlyPortMiddle = flyPortMiddle,
                YaoCeIpAddressMiddle = yaoceIpAddrMiddle,
                YaoCePortMiddle = yaocePortMiddle,

                EnvIpAddressLow = envIpAddrLow,
                EnvPortLow = envPortLow,
                FlyIpAddressLow = flyIpAddrLow,
                FlyPortLow = flyPortLow,
                YaoCeIpAddressLow = yaoceIpAddrLow,
                YaoCePortLow = yaocePortLow,

                idleTime = idleTime,
                MaxDisplayPoint = maxDisplayPoint
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

        public bool SaveRatios(Ratios ratios)
        {
            try
            {
                using (FileStream file = File.Create(RatioSettingFile))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(file, ratios);
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
