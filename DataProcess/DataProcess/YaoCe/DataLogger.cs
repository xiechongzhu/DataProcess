// 
using DataProcess.Protocol;
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
using System.Threading.Tasks; 

/// YaoCeProcess
namespace YaoCeProcess
{
    /// 文件名:DataLogger
    /// 文件功能描述:数据记录
    /// 创建人:yangy
    /// 版权所有:Copyright (C) ZGM
    /// 创建标识:2020.03.12
    /// 修改描述:
    public class _DataLogger
    {
        /// strLogDir
        private static String strLogDir = AppDomain.CurrentDomain.BaseDirectory + "Log"; 

        /// queue
        private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>(); 

        /// isRuning
        private bool isRuning = false; 

        /// thread
        Thread thread; 

        /// 二进制数据格式存储
        protected FileStream logWriter; 

        /// 以文本的格式存储
        // protected StreamWriter logWriter; 

        /// strLogFolder
        public String strLogFolder;

        /// strDataFile
        public String strDataFile; 

        /// DataLogger
        public _DataLogger(Priority priority)
        {
            strLogFolder = strLogDir + @"\Bitstream"; 
            switch(priority)
            {
                case Priority.HighPriority:
                    strLogFolder += @"\高优先级";
                    break;
                case Priority.MiddlePriority:
                    strLogFolder += @"\中优先级";
                    break;
                case Priority.LowPriority:
                    strLogFolder += @"\低优先级";
                    break;
            }
            Directory.CreateDirectory(strLogFolder);  
        }

        /// Enqueue
        public void Enqueue(byte[] data)
        {
            queue.Enqueue(data);
        }

        /// Start
        public void Start()
        {
            while (queue.TryDequeue(out byte[] dropBuffer)) ;  
            logWriter?.Close(); 

            // 生成码流日志文件名称
            DateTime dateTime = DateTime.Now; 
            String strDate = dateTime.ToString("yyyy_MM_dd_HH_mm_ss"); 

           // 注意修改文件后缀名
            strDataFile = strLogFolder + @"\" + strDate + @"_data.dat"; 

            // 二进制数据格式存储
            logWriter = new FileStream(strDataFile, FileMode.Append); 

            // 文本数据格式存储 
            // logWriter = new StreamWriter(strDataFile);

            isRuning = true; 
            thread = new Thread(new ThreadStart(ThreadFunction));  
            thread.Start(); 
        }

        /// Stop
        public void Stop()
        {
            isRuning = false;  
            thread?.Join(); 
            logWriter?.Close(); 
        }

        /// ThreadFunction
        private void ThreadFunction()
        {
            while (isRuning)
            {
                byte[] dataBuffer; 
                if (queue.TryDequeue(out dataBuffer))
                {
                    LogData(dataBuffer);
                }
                else
                {
                    Thread.Sleep(5); 
                }
            } 
        }

        /// LogData
        private void LogData(byte[] buffer)
        {
#if false
            // 按文本格式存储
            StringBuilder sb = new StringBuilder(buffer.Length * 3); 
            foreach (byte b in buffer)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0') + " "); 
            }
            String strData = sb.ToString().ToUpper(); 
            logWriter.WriteLine(strData);
            logWriter.Flush();
#endif
           // 以二进制格式进行数据存储
            // 20200207 直接原数据存储
            if (buffer != null && buffer.Length > 0)
            {
                logWriter.Write(buffer, 0, buffer.Length); 
                logWriter.Flush();
            }
        } 
    }
}
