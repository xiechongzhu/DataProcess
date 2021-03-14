// 
using System; //
// 
using System.Collections.Generic; //
// 
using System.Linq; //
// 
using System.Text; //
// 
using System.Threading.Tasks; //
// 

// 
/// <summary>
// 
/// YaoCeProcess
// 

// 
namespace YaoCeProcess
// 
{
    // 
    /// <summary>
    // 
    /// 文件名:CRC/
    // 
    /// 文件功能描述:CRC校验/
    // 
    /// 创建人:yangy
    // 
    /// 版权所有:Copyright (C) ZGM/
    // 
    /// 创建标识:2020.03.12/     
    // 
    /// 修改描述:/
    // 
    
    // 
    class CRC
    // 
    {
        // 
        #region  CRC16
        // 
        /// <summary>
        // 
        /// CRC16
        // 
        
        // 
        /// <param name="data"></param>
        // 
        /// <returns></returns>
        // 
        public static byte[] CRC16(byte[] data)
        // 
        {
            // 
            int len = data.Length; //
                                   // 
            if (len > 0)
            // 
            {
                // 
                ushort crc = 0xFFFF; //
                                     // 

                // 
                for (int i = 0; i < len; i++)
                // 
                {
                    // 
                    crc = (ushort)(crc ^ (data[i])); //
                                                     // 
                    for (int j = 0; j < 8; j++)
                    // 
                    {
                        // 
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1); //
                                                                                                   // 
                    }
                    // 
                }
                // 
                byte hi = (byte)((crc & 0xFF00) >> 8); //  //高位置
                                                       // 
                byte lo = (byte)(crc & 0x00FF); //         //低位置
                                                // 

                // 
                return new byte[] { hi, lo }; //
                                              // 
            }
            // 
            return new byte[] { 0, 0 }; //
                                        // 
        }
        // 
        #endregion
        // 

        // 
        #region  ToCRC16
        // 
        /// <summary>
        // 
        /// ToCRC16
        // 
        
        // 
        /// <param name="content"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToCRC16(string content)
        // 
        {
            // 
            return ToCRC16(content, Encoding.UTF8); //
                                                    // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToCRC16
        // 
        
        // 
        /// <param name="content"></param>
        // 
        /// <param name="isReverse"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToCRC16(string content, bool isReverse)
        // 
        {
            // 
            return ToCRC16(content, Encoding.UTF8, isReverse); //
                                                               // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToCRC16
        // 
        
        // 
        /// <param name="content"></param>
        // 
        /// <param name="encoding"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToCRC16(string content, Encoding encoding)
        // 
        {
            // 
            return ByteToString(CRC16(encoding.GetBytes(content)), true); //
                                                                          // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToCRC16
        // 
        
        // 
        /// <param name="content"></param>
        // 
        /// <param name="encoding"></param>
        // 
        /// <param name="isReverse"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToCRC16(string content, Encoding encoding, bool isReverse)
        // 
        {
            // 
            return ByteToString(CRC16(encoding.GetBytes(content)), isReverse); //
                                                                               // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToCRC16
        // 
        
        // 
        /// <param name="data"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToCRC16(byte[] data)
        // 
        {
            // 
            return ByteToString(CRC16(data), true); //
                                                    // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToCRC16
        // 
        
        // 
        /// <param name="data"></param>
        // 
        /// <param name="isReverse"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToCRC16(byte[] data, bool isReverse)
        // 
        {
            // 
            return ByteToString(CRC16(data), isReverse); //
                                                         // 
        }
        // 
        #endregion
        // 

        // 
        #region  ToModbusCRC16
        // 
        /// <summary>
        // 
        /// ToModbusCRC16
        // 
        
        // 
        /// <param name="s"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToModbusCRC16(string s)
        // 
        {
            // 
            return ToModbusCRC16(s, true); //
                                           // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToModbusCRC16
        // 
        
        // 
        /// <param name="s"></param>
        // 
        /// <param name="isReverse"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToModbusCRC16(string s, bool isReverse)
        // 
        {
            // 
            return ByteToString(CRC16(StringToHexByte(s)), isReverse); //
                                                                       // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToModbusCRC16
        // 
        
        // 
        /// <param name="data"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToModbusCRC16(byte[] data)
        // 
        {
            // 
            return ToModbusCRC16(data, true); //
                                              // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ToModbusCRC16
        // 
        
        // 
        /// <param name="data"></param>
        // 
        /// <param name="isReverse"></param>
        // 
        /// <returns></returns>
        // 
        public static string ToModbusCRC16(byte[] data, bool isReverse)
        // 
        {
            // 
            return ByteToString(CRC16(data), isReverse); //
                                                         // 
        }
        // 
        #endregion
        // 

        // 
        #region  ByteToString
        // 
        /// <summary>
        // 
        /// ByteToString
        // 
        
        // 
        /// <param name="arr"></param>
        // 
        /// <param name="isReverse"></param>
        // 
        /// <returns></returns>
        // 
        public static string ByteToString(byte[] arr, bool isReverse)
        // 
        {
            // 
            try
            // 
            {
                // 
                byte hi = arr[0], lo = arr[1]; //
                                               // 
                return Convert.ToString(isReverse ? hi + lo * 0x100 : hi * 0x100 + lo, 16).ToUpper().PadLeft(4, '0'); //
                                                                                                                      // 
            }
            // 
            catch (Exception ex) { throw (ex); }
            // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ByteToString
        // 
        
        // 
        /// <param name="arr"></param>
        // 
        /// <returns></returns>
        // 
        public static string ByteToString(byte[] arr)
        // 
        {
            // 
            try
            // 
            {
                // 
                return ByteToString(arr, true); //
                                                // 
            }
            // 
            catch (Exception ex) { throw (ex); }
            // 
        }
        // 
        #endregion
        // 

        // 
        #region  StringToHexString
        // 
        /// <summary>
        // 
        /// StringToHexString
        // 
        
        // 
        /// <param name="str"></param>
        // 
        /// <returns></returns>
        // 
        public static string StringToHexString(string str)
        // 
        {
            // 
            StringBuilder s = new StringBuilder(); //
                                                   // 
            foreach (short c in str.ToCharArray())
            // 
            {
                // 
                s.Append(c.ToString("X4")); //
                                            // 
            }
            // 
            return s.ToString(); //
                                 // 
        }
        // 
        #endregion
        // 

        // 
        #region  StringToHexByte
        // 
        /// <summary>
        // 
        /// ConvertChinese
        // 
        
        // 
        /// <param name="str"></param>
        // 
        /// <returns></returns>
        // 
        private static string ConvertChinese(string str)
        // 
        {
            // 
            StringBuilder s = new StringBuilder(); //
                                                   // 
            foreach (short c in str.ToCharArray())
            // 
            {
                // 
                if (c <= 0 || c >= 127)
                // 
                {
                    // 
                    s.Append(c.ToString("X4")); //
                                                // 
                }
                // 
                else
                // 
                {
                    // 
                    s.Append((char)c); //
                                       // 
                }
                // 
            }
            // 
            return s.ToString(); //
                                 // 
        }
        // 

        // 
        /// <summary>
        // 
        /// FilterChinese
        // 
        
        // 
        /// <param name="str"></param>
        // 
        /// <returns></returns>
        // 
        private static string FilterChinese(string str)
        // 
        {
            // 
            StringBuilder s = new StringBuilder(); //
                                                   // 
            foreach (short c in str.ToCharArray())
            // 
            {
                // 
                if (c > 0 && c < 127)
                // 
                {
                    // 
                    s.Append((char)c); //
                                       // 
                }
                // 
            }
            // 
            return s.ToString(); //
                                 // 
        }
        // 

        // 
        /// <summary>
        // 
        /// 字符串转16进制字符数组
        // 
        
        // 
        /// <param name="hex"></param>
        // 
        /// <returns></returns>
        // 
        public static byte[] StringToHexByte(string str)
        // 
        {
            // 
            return StringToHexByte(str, false); //
                                                // 
        }
        // 

        // 
        /// <summary>
        // 
        /// 字符串转16进制字符数组
        // 
        
        // 
        /// <param name="str"></param>
        // 
        /// <param name="isFilterChinese">是否过滤掉中文字符</param>
        // 
        /// <returns></returns>
        // 
        public static byte[] StringToHexByte(string str, bool isFilterChinese)
        // 
        {
            // 
            string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str); //
                                                                                     // 

            // 
            //清除所有空格
            // 
            hex = hex.Replace(" ", ""); //
                                        // 
                                        //若字符个数为奇数，补一个0
                                        // 
            hex += hex.Length % 2 != 0 ? "0" : ""; //
                                                   // 

            // 
            byte[] result = new byte[hex.Length / 2]; //
                                                      // 
            for (int i = 0, c = result.Length; i < c; i++)
            // 
            {
                // 
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16); //
                                                                         // 
            }
            // 
            return result; //
                           // 
        }
        // 
        #endregion
        // 

        // 
        //------------------------------------------------------------------//
        // 

        // 
        /*
// 
         #region  StringToHexByte
// 
        /// <summary>
// 
        /// ConvertChinese
// 
        
// 
        /// <param name="str"></param>
// 
        /// <returns></returns>
// 
        private static string ConvertChinese(string str)
// 
        {
// 
            StringBuilder s = new StringBuilder(); //
// 
            foreach (short c in str.ToCharArray())
// 
            {
// 
                if (c <= 0 || c >= 127)
// 
                {
// 
                    s.Append(c.ToString("X4")); //
// 
                }
// 
                else
// 
                {
// 
                    s.Append((char)c); //
// 
                }
// 
            }
// 
            return s.ToString(); //
// 
        }
// 

// 
        /// <summary>
// 
        /// FilterChinese
// 
        
// 
        /// <param name="str"></param>
// 
        /// <returns></returns>
// 
        private static string FilterChinese(string str)
// 
        {
// 
            StringBuilder s = new StringBuilder(); //
// 
            foreach (short c in str.ToCharArray())
// 
            {
// 
                if (c > 0 && c < 127)
// 
                {
// 
                    s.Append((char)c); //
// 
                }
// 
            }
// 
            return s.ToString(); //
// 
        }
// 

// 
        /// <summary>
// 
        /// 字符串转16进制字符数组
// 
        
// 
        /// <param name="hex"></param>
// 
        /// <returns></returns>
// 
        public static byte[] StringToHexByte(string str)
// 
        {
// 
            return StringToHexByte(str, false); //
// 
        }
// 

// 
        /// <summary>
// 
        /// 字符串转16进制字符数组
// 
        
// 
        /// <param name="str"></param>
// 
        /// <param name="isFilterChinese">是否过滤掉中文字符</param>
// 
        /// <returns></returns>
// 
        public static byte[] StringToHexByte(string str, bool isFilterChinese)
// 
        {
// 
            string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str); //
// 

// 
            //清除所有空格
// 
            hex = hex.Replace(" ", ""); //
// 
            //若字符个数为奇数，补一个0
// 
            hex += hex.Length % 2 != 0 ? "0" : ""; //
// 

// 
            byte[] result = new byte[hex.Length / 2]; //
// 
            for (int i = 0, c = result.Length; // i < c; // i++)
// 
            {
// 
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16); //
// 
            }
// 
            return result; //
// 
        }
// 
        #endregion
// 
         */
        // 
    }
    // 
}
// 
