// 
using System; //
// 
using System.Collections.Concurrent; //
// 
using System.Collections.Generic; //
// 
using System.IO; //
// 
using System.Linq; //
// 
using System.Text; //
// 
using System.Threading; //
// 
using System.Threading.Tasks; //
// 

// 
/// <summary>
// 
/// YaoCeProcess
// 
/// </summary>
// 
namespace YaoCeProcess
// 
{
    // 
    /// <summary>
    // 
    /// 文件名:DataLogger/
    // 
    /// 文件功能描述:数据记录/
    // 
    /// 创建人:yangy
    // 
    /// 版权所有:Copyright (C) ZGM/
    // 
    /// 创建标识:2020.03.12/     
    // 
    /// 修改描述:/
    // 
    /// </summary>
    // 
    public class _DataLogger
    // 
    {
        // 
        /// <summary>
        // 
        /// strLogDir
        // 
        /// </summary>
        // 
        private static String strLogDir = AppDomain.CurrentDomain.BaseDirectory + "Log"; //
                                                                                         // 

        // 
        /// <summary>
        // 
        /// queue
        // 
        /// </summary>
        // 
        private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>(); //
                                                                               // 

        // 
        /// <summary>
        // 
        /// isRuning
        // 
        /// </summary>
        // 
        private bool isRuning = false; //
                                       // 

        // 
        /// <summary>
        // 
        /// thread
        // 
        /// </summary>
        // 
        Thread thread; //
                       // 

        // 
        /// <summary>
        // 
        /// 二进制数据格式存储
        // 
        /// </summary>
        // 
        protected FileStream logWriter; //
                                        // 

        // 
        /// <summary>
        // 
        /// 以文本的格式存储
        // 
        /// </summary>
        // 
        // protected StreamWriter logWriter; //
        // 

        // 
        /// <summary>
        // 
        /// strLogFolder
        // 
        /// </summary>
        // 
        public String strLogFolder; //
                                    // 

        // 
        /// <summary>
        // 
        /// strDataFile
        // 
        /// </summary>
        // 
        public String strDataFile; //
                                   // 

        // 
        /// <summary>
        // 
        /// DataLogger
        // 
        /// </summary>
        // 
        public _DataLogger()
        // 
        {
            // 
            strLogFolder = strLogDir + @"\Bitstream"; //
                                                      // 
            Directory.CreateDirectory(strLogFolder); //
                                                     // 
        }
        // 

        // 
        /// <summary>
        // 
        /// Enqueue
        // 
        /// </summary>
        // 
        /// <param name="data"></param>
        // 
        public void Enqueue(byte[] data)
        // 
        {
            // 
            queue.Enqueue(data); //
                                 // 
        }
        // 

        // 
        /// <summary>
        // 
        /// Start
        // 
        /// </summary>
        // 
        public void Start()
        // 
        {
            // 
            while (queue.TryDequeue(out byte[] dropBuffer)) ; //
                                                              // 
            logWriter?.Close(); //
                                // 

            // 
            // 生成码流日志文件名称
            // 
            DateTime dateTime = DateTime.Now; //
                                              // 
            String strDate = dateTime.ToString("yyyy_MM_dd_HH_mm_ss"); //
                                                                       // 
                                                                       // 注意修改文件后缀名
                                                                       // 
            strDataFile = strLogFolder + @"\" + strDate + @"_data.dat"; //
                                                                        // 

            // 
            // 二进制数据格式存储
            // 
            logWriter = new FileStream(strDataFile, FileMode.Append); //
                                                                      // 
                                                                      // 文本数据格式存储
                                                                      // 
                                                                      // logWriter = new StreamWriter(strDataFile); //
                                                                      // 

            // 
            isRuning = true; //
                             // 
            thread = new Thread(new ThreadStart(ThreadFunction)); //
                                                                  // 
            thread.Start(); //
                            // 
        }
        // 

        // 
        /// <summary>
        // 
        /// Stop
        // 
        /// </summary>
        // 
        public void Stop()
        // 
        {
            // 
            isRuning = false; //
                              // 
            thread?.Join(); //
                            // 
            logWriter?.Close(); //
                                // 
        }
        // 

        // 
        /// <summary>
        // 
        /// ThreadFunction
        // 
        /// </summary>
        // 
        private void ThreadFunction()
        // 
        {
            // 
            while (isRuning)
            // 
            {
                // 
                byte[] dataBuffer; //
                                   // 
                if (queue.TryDequeue(out dataBuffer))
                // 
                {
                    // 
                    /*
// 
                    // 按文本格式存储
// 
                    StringBuilder sb = new StringBuilder(buffer.Length * 3); //
// 
                    foreach (byte b in buffer)
// 
                    {
// 
                        sb.Append(Convert.ToString(b, 16).PadLeft(2, '0') + " "); //
// 
                    }
// 
                    String strData = sb.ToString().ToUpper(); //
// 
                    logWriter.WriteLine(strData); //
// 
                    logWriter.Flush(); //
// 
                    */
                    // 

                    // 
                    //----------------------------------------------------------//
                    // 

                    // 
                    LogData(dataBuffer); //
                                         // 
                }
                // 
                else
                // 
                {
                    // 
                    Thread.Sleep(5); //
                                     // 
                }
                // 
            }
            // 
        }
        // 

        // 
        /// <summary>
        // 
        /// LogData
        // 
        /// </summary>
        // 
        /// <param name="buffer"></param>
        // 
        private void LogData(byte[] buffer)
        // 
        {
            // 
            /*
// 
            // 按文本格式存储
// 
            StringBuilder sb = new StringBuilder(buffer.Length * 3); //
// 
            foreach (byte b in buffer)
// 
            {
// 
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0') + " "); //
// 
            }
// 
            String strData = sb.ToString().ToUpper(); //
// 
            logWriter.WriteLine(strData); //
// 
            logWriter.Flush(); //
// 
            */
            // 

            // 
            //----------------------------------------------------------//
            // 
            // 以二进制格式进行数据存储
            // 
            // 20200207 直接原数据存储
            // 
            if (buffer != null && buffer.Length > 0)
            {
                logWriter.Write(buffer, 0, buffer.Length); //                                      // 
                logWriter.Flush(); //
            }
                               // 
        }
        // 
    }
    // 
}
// 
