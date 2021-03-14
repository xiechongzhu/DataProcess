// 
using System; //
// 
using System.Collections.Generic; //
// 
using System.IO; //
// 
using System.Linq; //
// 
using System.Runtime.CompilerServices; //
// 
using System.Text; //
// 
using System.Threading.Tasks; //
// 
using DataProcess;

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
    /// 文件名:Logger/
    // 
    /// 文件功能描述:日志记录/
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
    public class Logger
    // 
    {
        // 
        /// <summary>
        // 
        /// strLogDir
        // 
        
        // 
        private static String strLogDir = AppDomain.CurrentDomain.BaseDirectory + "Log"; //
                                                                                         // 

        // 
        /// <summary>
        // 
        /// LOG_LEVEL
        // 
        
        // 
        public enum LOG_LEVEL
        // 
        {
            // 
            LOG_INFO,
            // 
            LOG_WARN,
            // 
            LOG_ERROR
            // 
        }
        // 

        // 
        /// <summary>
        // 
        /// logWriter
        // 
        
        // 
        protected StreamWriter logWriter; //
                                          // 

        // 
        /// <summary>
        // 
        /// Logger
        // 
        
        // 
        private Logger()
        // 
        {
            // 
            // 创建日志文件夹
            // 
            Directory.CreateDirectory(strLogDir); //
                                                  // 
        }
        // 

        // 
        /// <summary>
        // 
        /// NewFile
        // 
        
        // 
        public void NewFile()
        // 
        {
            // 
            logWriter?.Close(); //
                                // 

            // 
            // 生成日志文件
            // 
            dateTime = DateTime.Now; //
                                     // 
            String strDate = dateTime.ToString("yyyy_MM_dd_HH_mm_ss"); //
                                                                       // 
            strLogFile = strLogDir + @"\" + strDate + @"_Log.txt"; //
                                                                   // 
            logWriter = new StreamWriter(strLogFile);  
        }

        /// closeFile
        public void closeFile()
        { 
            logWriter?.Close();
        }

        /// mainForm
        private MainWindow mainForm; 

        /// __instance
        private static Logger __instance = new Logger();  


        /// dateTime
        public DateTime dateTime;  

        /// strLogFile
        public String strLogFile;  


        /// GetInstance
        public static Logger GetInstance()
        { 
            return __instance;  
        }

        /// SetMainForm
        public void SetMainForm(MainWindow _mainForm)
        { 
            mainForm = _mainForm;  
        }

        /// Log
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(LOG_LEVEL level, String msg)
        {
            DateTime dateTime = DateTime.Now;
            String strLevel; 
            switch (level)
            { 
                case LOG_LEVEL.LOG_INFO:
                    strLevel = "信息";
                    break; 
                case LOG_LEVEL.LOG_ERROR:
                    strLevel = "错误"; 
                    break;  
                default:
                    return;  
            }
            String strLog = String.Format("[{0}][{1}]{2}", dateTime.ToString("G"), strLevel, msg);
            logWriter.WriteLine(strLog);  
            logWriter.Flush();  
        } 
    }
}
