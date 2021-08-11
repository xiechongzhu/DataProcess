using DataProcess;
using DataProcess.Tools;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DataProcess.CustomControl;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using YaoCeProcess;
using System.Reflection;
using System.Windows.Resources;
using DataProcess.Protocol;

namespace DataProcess.Controls
{
    public partial class YaoCeShuJuXianShi : UserControl
    {
        private Dictionary<Priority, bool> ParserStatus = new Dictionary<Priority, bool>();


        /// 每一个UDP帧固定长度651
        public const int UDPLENGTH = 651;

        /// 系统判决状态数据标识
        public const int WM_YAOCE_SystemStatus_DATA = WinApi.WM_USER + 107;

        /// 导航数据（快速——弹体）标识
        public const int WM_YAOCE_daoHangKuaiSu_Ti_DATA = WinApi.WM_USER + 108;

        /// 导航数据（快速——弹头）标识  改弹头导航数据
        public const int WM_YAOCE_danTouDaoHang_DATA = WinApi.WM_USER + 109;

        /// 导航数据（慢速——弹体）标识 
        public const int WM_YAOCE_daoHangManSu_Ti_DATA = WinApi.WM_USER + 110;

        /// 导航数据（慢速——弹头）标识
        public const int WM_YAOCE_daoHangManSu_Tou_DATA = WinApi.WM_USER + 111;

        /// 回路检测数据标识
        public const int WM_YAOCE_HuiLuJianCe_DATA = WinApi.WM_USER + 112;
        // 
        // TODO 20200219 新增
        // 
        /// 系统状态即时反馈数据（弹体）标识
        public const int WM_YAOCE_XiTongJiShi_Ti_DATA = WinApi.WM_USER + 113;

        /// 系统状态即时反馈（弹头）标识
        public const int WM_YAOCE_XiTongJiShi_Tou_DATA = WinApi.WM_USER + 114;

        /// 数据帧信息
        public const int WM_YAOCE_FRAMEPROPERTY_DATA = WinApi.WM_USER + 115;

        /// UDP包状态
        public const int WM_YAOCE_UDPPROPERTY_DATA = WinApi.WM_USER + 116;

        /// 读取完成后休眠时间
        private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(500);

        //// 读取文件动作
        /// E_LOADFILE_START
        public const int E_LOADFILE_START = 0;

        /// E_LOADFILE_PAUSE
        public const int E_LOADFILE_PAUSE = 1;

        /// E_LOADFILE_CONTINUE
        public const int E_LOADFILE_CONTINUE = 2;

        /// E_LOADFILE_STOP
        public const int E_LOADFILE_STOP = 3;

        /// E_LOADFILE_SKIPPROGRAM
        public const int E_LOADFILE_SKIPPROGRAM = 4;

        //--------成员变量-----------------//
        /// 是否开启Socket接收网络数据
        bool bStartRecvNetworkData = false;

        //文件转换
        bool dataConversion = false;

        public LoadDataForm load;//

        /// 直接读取二进制文件
        FileStream srFileRead = null;

        /// 文件大小
        long loadFileLength = 0;

        /// 已经读取的文件大小
        long alreadReadFileLength = 0;

        /// 创建曲线X轴索引值
        public int xiTong_CHART_ITEM_INDEX = 0;

        //------------------------状态数据缓存-----------------------------//
        //      系统判决状态
        SYSTEMPARSE_STATUS sObject_XiTong;

        //弹头导航数据
        DANTOUDAOHANGDATA sObject_DanTou;

        //      回路检测
        HUILUJIANCE_STATUS sObject_huiLuJianCe;

        //导航快
        DAOHANGSHUJU_KuaiSu sObject_DHK_Ti;
        DAOHANGSHUJU_KuaiSu sObject_DHK_Tou;

        //导航慢
        DAOHANGSHUJU_ManSu sObject_DHM_Ti;
        DAOHANGSHUJU_ManSu sObject_DHM_Tou;

        //系统即时
        SYSTEMImmediate_STATUS sObject_XTJS_Ti;
        SYSTEMImmediate_STATUS sObject_XTJS_Tou;

        //帧序号


        /// 是否收到数据
        bool bRecvStatusData_XiTong = false; //系统判决

        /// bRecvStatusData_HuiLuJianCe
        bool bRecvStatusData_HuiLuJianCe = false;//回路检测

        /// bRecvStatusData_UDP 
        bool bRecvStatusData_UDP = false;

        bool bRecvStatusData_DHK = false; //
        bool bRecvStatusData_DHM = false; //
        bool bRecvStatusData_DHM_Ti = false; //
        bool bRecvStatusData_DHM_Tou = false; //

        bool bRecvStatusData_XTJS = false; //系统即时反馈
        bool bRecvStatusData_XTJS_Ti = false; //
        bool bRecvStatusData_XTJS_Tou = false; //

        bool bReceStatusData_DANTOU = false; //弹头导航数据

        /// 码流记录
        private _DataLogger yaoceDataLoggerHigh = new _DataLogger(Priority.HighPriority); //
        private _DataLogger yaoceDataLoggerMiddle = new _DataLogger(Priority.MiddlePriority); //
        private _DataLogger yaoceDataLoggerLow = new _DataLogger(Priority.LowPriority); //

        //-----------------------------------------------------//

        const byte frameType_systemStatus_1 = 0x15; //       // 系统判据状态
        /// 系统判据状态
        const byte frameType_systemStatus_2 = 0x16; //       // 系统判据状态

        /// 回路检测反馈状态
        const byte frameType_HuiLuJianCe = 0x16; //          // 回路检测反馈状态

        /// 导航快速（弹体）
        const byte frameType_daoHangKuaiSu_Ti = 0x21; //     // 导航快速（弹体）

        /// 导航快速（弹头）
        const byte frameType_daoHangKuaiSu_Tou = 0x31; //    // 导航快速（弹头)

        /// 导航慢速（弹体）
        const byte frameType_daoHangManSu_Ti = 0x25; //      // 导航慢速（弹体）

        /// 导航慢速（弹头）
        const byte frameType_daoHangManSu_Tou = 0x35; //     // 导航慢速（弹头）//弹头导航数据

        /// 系统状态即时反馈（弹体）
        const byte frameType_XiTongJiShi_Ti = 0x26; //       // 系统状态即时反馈（弹体）   帧总长11  数据段总长度64 帧类型0x0B

        /// 系统状态即时反馈（弹头）
        const byte frameType_XiTongJiShi_Tou = 0x36; //      // 系统状态即时反馈（弹头）


        /// frameType_XTPJZT
        const byte frameType_XTPJZT = 0x01;

        /// frameType_XTPJFK
        const byte frameType_XTPJFK = 0x05;

        /// frameType_HLJCFK
        const byte frameType_HLJCFK = 0x06; //

        //-----------------------------------------------------//
        /// E_STATUSTYPE_XiTong
        public const uint E_STATUSTYPE_XiTong = 0x01;

        /// E_STATUSTYPE_HuiLuJianCe
        public const uint E_STATUSTYPE_HuiLuJianCe = 0x02;

        /// E_STATUSTYPE_DaoHangKuaiSu_Ti
        public const uint E_STATUSTYPE_DaoHangKuaiSu_Ti = 0x03;

        /// E_STATUSTYPE_DaoHangKuaiSu_Tou
        public const uint E_STATUSTYPE_DaoHangKuaiSu_Tou = 0x04;

        /// E_STATUSTYPE_DaoHangManSu_Ti
        public const uint E_STATUSTYPE_DaoHangManSu_Ti = 0x05;

        /// E_STATUSTYPE_DaoHangManSu_Tou
        public const uint E_STATUSTYPE_DaoHangManSu_Tou = 0x06;

        /// E_STATUSTYPE_XiTongJiShi_Ti
        public const uint E_STATUSTYPE_XiTongJiShi_Ti = 0x07;

        /// E_STATUSTYPE_XiTongJiShi_Tou
        public const uint E_STATUSTYPE_XiTongJiShi_Tou = 0x08;

        /// E_STATUSTYPE_dataConnect
        public const uint E_STATUSTYPE_dataConnect = 0x09;

        /// bDaoHangKuaiSuOnLine_Ti
        bool bDaoHangKuaiSuOnLine_Ti = false;

        /// bDaoHangKuaiSuOnLine_Tou
        bool bDaoHangKuaiSuOnLine_Tou = false;

        /// bDaoHangManSuOnLine_Ti
        bool bDaoHangManSuOnLine_Ti = false;

        /// bDaoHangManSuOnLine_Tou
        bool bDaoHangManSuOnLine_Tou = false;

        /// bXiTongJiShiOnLine_Ti
        bool bXiTongJiShiOnLine_Ti = false;

        /// bXiTongJiShiOnLine_Tou
        bool bXiTongJiShiOnLine_Tou = false;

        bool isConvertXiTongPanJue = false;
        bool isConvertDHK = false;
        bool isConvertDHM = false;
        bool isConvertXiTongJiShi = false;
        bool isConvertDanTou = false;


        /// setDaoHangStatusOnOffLine
        public void setDaoHangStatusOnOffLine(uint statusType, bool bOn)
        {
            switch (statusType)
            {
                case E_STATUSTYPE_DaoHangKuaiSu_Ti:
                    bDaoHangKuaiSuOnLine_Ti = bOn;
                    break;

                case E_STATUSTYPE_DaoHangKuaiSu_Tou:
                    bDaoHangKuaiSuOnLine_Tou = bOn;
                    break;

                case E_STATUSTYPE_DaoHangManSu_Ti:
                    bDaoHangManSuOnLine_Ti = bOn;
                    break;

                case E_STATUSTYPE_DaoHangManSu_Tou:
                    bDaoHangManSuOnLine_Tou = bOn;
                    break;
                // 
                // TODO 20200219 新增
                // 
                case E_STATUSTYPE_XiTongJiShi_Ti:
                    bXiTongJiShiOnLine_Ti = bOn;
                    break;

                case E_STATUSTYPE_XiTongJiShi_Tou:
                    bXiTongJiShiOnLine_Tou = bOn;
                    break;

                default:
                    break;
            }
        }

        public void setStatusOnOffLine(uint statusType, bool bOn)
        {
            switch (statusType)
            {
                case E_STATUSTYPE_XiTong:
                    if (bOn)
                    {
                        SetLedStatus(imageZhuangTai, LED_STATUS.LED_GREEN);
                    }
                    else
                    {
                        SetLedStatus(imageZhuangTai, LED_STATUS.LED_GRAY);
                    }
                    break;
                //case E_STATUSTYPE_HuiLuJianCe:
                //    if (bOn)
                //    {
                //        SetLedStatus(imageJianCe, LED_STATUS.LED_GREEN);
                //    }
                //    else
                //    {
                //        SetLedStatus(imageJianCe, LED_STATUS.LED_GRAY);
                //    }
                //    break;
                case E_STATUSTYPE_DaoHangKuaiSu_Ti:
                case E_STATUSTYPE_DaoHangKuaiSu_Tou:
                    if (bOn)
                    {
                        SetLedStatus(imageKuaiSu, LED_STATUS.LED_GREEN);
                    }
                    else
                    {
                        SetLedStatus(imageKuaiSu, LED_STATUS.LED_GRAY);
                    }
                    break;

                case E_STATUSTYPE_DaoHangManSu_Ti:
                    if (bOn)
                    {
                        SetLedStatus(imageManSu, LED_STATUS.LED_GREEN);
                    }
                    else
                    {
                        SetLedStatus(imageManSu, LED_STATUS.LED_GRAY);
                    }
                    break;
                case E_STATUSTYPE_DaoHangManSu_Tou:
                    if (bOn)
                    {
                        SetLedStatus(imageDanTou, LED_STATUS.LED_GREEN);
                    }
                    else
                    {
                        SetLedStatus(imageDanTou, LED_STATUS.LED_GRAY);
                    }
                    break;
                //
                // 
                // TODO 20200219 新增
                // 
                case E_STATUSTYPE_XiTongJiShi_Ti:
                case E_STATUSTYPE_XiTongJiShi_Tou:
                    if (bOn)
                    {
                        SetLedStatus(imageJiShi, LED_STATUS.LED_GREEN);
                    }
                    else
                    {
                        SetLedStatus(imageJiShi, LED_STATUS.LED_GRAY);
                    }
                    break;
                //
                // 
                // TODO 20200316 新增
                // 
                case E_STATUSTYPE_dataConnect:
                    if (bOn)
                    {
                        SetLedStatus(ImageUDP, LED_STATUS.LED_GREEN);
                    }
                    else
                    {
                        SetLedStatus(ImageUDP, LED_STATUS.LED_GRAY);
                    }
                    break;
                default:
                    break;
            }
        }
        private enum LED_STATUS
        {
            LED_RED,
            LED_GREEN,
            LED_GRAY
        }

        Dictionary<LED_STATUS, BitmapImage> LedImages = new Dictionary<LED_STATUS, BitmapImage> {
            { LED_STATUS.LED_GRAY, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_gray.png")) },
            { LED_STATUS.LED_GREEN, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_green.png")) },
            { LED_STATUS.LED_RED, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_red.png")) }
        };


        public LoadDataForm loadWindow;
        public MainWindow m = null;
        public Image ImageUDP = null;
        public int maxDisplayPoint = 0;

        private UdpClient udpClientYaoCeHigh = null;
        private UdpClient udpClientYaoCeMiddle = null;
        private UdpClient udpClientYaoCeLow = null;
        private DataParser yaoceParserHigh = null;
        private DataParser yaoceParserMiddle = null;
        private DataParser yaoceParserLow = null;
        private DataParser yaoceParserFile = null;
        private YaoCeChartDataSource yaoCeChartDataSource = new YaoCeChartDataSource();
        private yaoceDisplayBuffers yaoceDisplay = new yaoceDisplayBuffers();

        private DispatcherTimer UpdateLoadFileProgressTimer = new DispatcherTimer(); //文件加载进度定时器

        //系统判决
        private DispatcherTimer UpdateXiTongStatusTimer = new DispatcherTimer();//系统判决数据定时器    
        private DispatcherTimer timerOffLineXiTongStatus = new DispatcherTimer();//系统判决状态离线
        private DispatcherTimer timerUpdateChart = new DispatcherTimer(); // 更新曲线

        //导航快速弹体
        private DispatcherTimer timerUpdateDHKStatus = new DispatcherTimer();
        private DispatcherTimer timerOfflineDHKStatus = new DispatcherTimer();

        //导航快速弹头
        private DispatcherTimer timerUpdateDHKStatus_Tou = new DispatcherTimer();
        private DispatcherTimer timerOfflineDHKStatus_Tou = new DispatcherTimer();

        //导航慢速弹体
        private DispatcherTimer timerUpdateDHMStatus = new DispatcherTimer();
        private DispatcherTimer timerOfflineDHMStatus = new DispatcherTimer();

        //导航慢速弹头
        private DispatcherTimer timerUpdateDHMStatus_Tou = new DispatcherTimer();
        private DispatcherTimer timerOfflineDHMStatus_Tou = new DispatcherTimer();

        //回路检测
        private DispatcherTimer UpdateHuiLuJianCeTimer = new DispatcherTimer();//回路检测数据定时器
        private DispatcherTimer timerOffLineHuiLuJianCe = new DispatcherTimer();

        //系统即时反馈弹体
        private DispatcherTimer timerUpdateXiTongJiShiStatus = new DispatcherTimer();
        private DispatcherTimer timerOfflineXiTongJiShiStatus = new DispatcherTimer();

        //系统即时反馈弹头
        private DispatcherTimer timerUpdateXiTongJiShiStatus_Tou = new DispatcherTimer();
        private DispatcherTimer timerOfflineXiTongJiShiStatus_Tou = new DispatcherTimer();

        //帧序号


        //UDP
        private DispatcherTimer timerUpdateUDP = new DispatcherTimer();//
        private DispatcherTimer timerOffLineUDP = new DispatcherTimer();

        System.Timers.Timer readFileTimer = new System.Timers.Timer(); //读取文件定时器

        public List<ChartPointDataSource> chartPointDataSources = new List<ChartPointDataSource>();
        public List<List<double>> buffers = new List<List<double>>();

        public List<string> stringBuilder_XiTongPanJue = new List<string>();



        /*数据转换*/
        public DispatcherTimer timerConvertBar = new DispatcherTimer(); //数据转换进度
        public string ParaseFileName = String.Empty; //数据转换文件
        public string ParaseFileToCSVPath = String.Empty; //转换后CSV文件路径

        public StreamWriter streamXiTongPanJue = null;
        public StreamWriter streamDHK = null;
        public StreamWriter streamDHM = null;
        public StreamWriter streamXTJS = null;
        public StreamWriter streamDanTou = null;
        public bool ConvertResult = false;//转换完成
        static int i = 0;

        public List<StringBuilder> stringBuilders = new List<StringBuilder>();
        public List<StringBuilder> stringBuilders_DHK = new List<StringBuilder>();
        public List<StringBuilder> stringBuilders_DHM = new List<StringBuilder>();
        public List<StringBuilder> stringBuilders_XTJS = new List<StringBuilder>();
        public List<StringBuilder> stringBuilders_DanTou = new List<StringBuilder>();
        TransformationProgress w;

        public YaoCeShuJuXianShi()
        {
            InitializeComponent();
            InitLedStatus();
            InitTimer_YaoCe();
            InitYaoCeChartDataSource();

            // 创建新的日志文件
            Logger.GetInstance().NewFile();
            Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "程序开始启动！");

            AddVariable();
            ResetAllTextEdit();

            //string path = "./YaoCeConfigFile/系统实时判决.txt";
            //WriteCSV.ReadTxT(path);

    }

        public  void setDataConversion(bool i,string fileName,string fileCSVPath)
        {
            dataConversion = i;
            ParaseFileName = fileName;
            ParaseFileToCSVPath = fileCSVPath;
            //Console.WriteLine("{0},{1},{2}", i,ParaseFileName,ParaseFileToCSVPath);
        }

        private void ResetAllTextEdit()
        {
            GenericFunction.reSetAllTextEdit(this.XiTong);
            GenericFunction.reSetAllTextEdit(this.HuiLu);
            GenericFunction.reSetAllTextEdit(this.FanKui_Ti);
            //GenericFunction.reSetAllTextEdit(this.FanKui_Tou);
            GenericFunction.reSetAllTextEdit(this.DaoHangKuai_Ti);
            //GenericFunction.reSetAllTextEdit(this.DaoHangKuai_Tou);
            //GenericFunction.reSetAllTextEdit(this.DaoHangMan_Tou);
            //GenericFunction.reSetAllTextEdit(this.DaoHangMan_Tou);
        }

        private void ClearAllChart()
        {
            for (int i = 0; i < chartPointDataSources.Count(); i++)
            {
                chartPointDataSources[i].ClearPoints();
            }
        }

        public void EnableLoadButton(bool e)
        {
            btnLoad.IsEnabled = e;
        }

        public void ClearChart()
        {
            this.ClearAllChart();
        }

        public void setImageUDP(ref Image image)
        {
            ImageUDP = image;
        }

        public void setMainWindow(MainWindow main)
        {
            m = main;
        }

        private void SetLedStatus(Image imageControl, LED_STATUS status)
        {
            if (imageControl.Source != LedImages[status])
            {
                imageControl.Source = LedImages[status];
            }
        }

        private void AddVariable()
        {
            //系统
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_ZuoBiao_JingDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_ZuoBiao_WeiDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_ZuoBiao_GaoDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_SuDu_DongList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_SuDu_BeiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_SuDu_TianList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_JiaoSuDu_WxList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_JiaoSuDu_WyList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_JiaoSuDu_WzList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_FaSheXi_ZXGZList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_FaSheXi_XList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_FaSheXi_YList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_FaSheXi_ZList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_YuShiLuoDian_SheChengList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTong_YuShiLuoDian_ZList);

            //DHK
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_ZuoBiao_JingDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_ZuoBiao_WeiDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_ZuoBiao_GaoDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_SuDu_DongList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_SuDu_BeiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_SuDu_TianList);
#if false
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_ZuoBiao_JingDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_ZuoBiao_WeiDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_ZuoBiao_GaoDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_DongList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_BeiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_TianList);
#endif
            //新增
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaoDu_FuYangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaoDu_GunZhuanJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaoDu_PianHangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_TuoLuo_TuoLuoXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_TuoLuo_TuoLuoYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_TuoLuo_TuoLuoZList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaSuDu_JiaJiXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaSuDu_JiaJiYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaSuDu_JiaJiZList);

#if false
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_JiaoDu_FuYangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_JiaoDu_GunZhuanJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_JiaoDu_PianHangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_TuoLuo_TuoLuoXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_TuoLuo_TuoLuoYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_TuoLuo_TuoLuoZList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_JiaSuDu_JiaJiXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_JiaSuDu_JiaJiYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHKuaiSu_Tou_JiaSuDu_JiaJiZList);
#endif

            //DHM
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_JingDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_WeiDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_GaoDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_DongList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_BeiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_TianList);

#if false
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_JingDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_WeiDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_GaoDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_DongList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_BeiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_TianList);
#endif
            //新增
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_JingDuZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_WeiDuZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_GaoDuZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_DongZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_BeiZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_TianZuHeList);

#if false
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_JingDuZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_WeiDuZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_GaoDuZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_DongZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_BeiZuHeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_TianZuHeList);
#endif
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_JiaoDu_FuYangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_JiaoDu_GunZhuanJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_JiaoDu_PianHangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_TuoLuo_TuoLuoXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_TuoLuo_TuoLuoYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_TuoLuo_TuoLuoZList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_JiaSuDu_JiaJiXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_JiaSuDu_JiaJiYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Ti_JiaSuDu_JiaJiZList);
#if false
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_JiaoDu_FuYangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_JiaoDu_GunZhuanJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_JiaoDu_PianHangJiaoList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_TuoLuo_TuoLuoXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_TuoLuo_TuoLuoYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_TuoLuo_TuoLuoZList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_JiaSuDu_JiaJiXList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_JiaSuDu_JiaJiYList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHManSu_Tou_JiaSuDu_JiaJiZList);
#endif

//系统即时
#if false
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_ZuoBiao_JingDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_ZuoBiao_WeiDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_ZuoBiao_GaoDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_SuDu_DongList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_SuDu_BeiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_SuDu_TianList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_JiaoSuDu_WxList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_JiaoSuDu_WyList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_JiaoSuDu_WzList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_GuoZai_ZhouXiangList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_GuoZai_FaXiangList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Tou_GuoZai_CeXiangList);
#endif
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_JingDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_WeiDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_GaoDuList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_SuDu_DongList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_SuDu_BeiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_SuDu_TianList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_JiaoSuDu_WxList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_JiaoSuDu_WyList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_JiaoSuDu_WzList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_ZhouXiangList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_FaXiangList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_CeXiangList);

            //帧序号
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTongPanJu15List);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XiTongPanJu16List);
            //chartPointDataSources.Add(yaoCeChartDataSource.chart_HuiLuJianCeList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHK_TiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DHM_TiList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_TiList);
            //chartPointDataSources.Add(yaoCeChartDataSource.chart_DHK_TouList);
            //chartPointDataSources.Add(yaoCeChartDataSource.chart_DHM_TouList);
            //chartPointDataSources.Add(yaoCeChartDataSource.chart_XTJS_TouList);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTouList);

            //弹头导航
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_ZuHeJingDu);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_ZuHeWeiDu);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_ZuHeGaoDu);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_ZuHeDong);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_ZuHeBei);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_ZuHeTian);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_GNSSJingDu);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_GNSSWeiDu);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_GNSSGaoDu);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_GNSSDong);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_GNSSBei);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_GNSSTian);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_FuYangJiao);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_GunZhuanJiao);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_PianHangJiao);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_Wx);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_Wy);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_Wz);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_XBiLi);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_YBiLi);
            chartPointDataSources.Add(yaoCeChartDataSource.chart_DanTou_ZBiLi);

            //系统
            buffers.Add(yaoceDisplay.XiTong_ZuoBiao_JingDu_buffer);
            buffers.Add(yaoceDisplay.XiTong_ZuoBiao_WeiDu_buffer);
            buffers.Add(yaoceDisplay.XiTong_ZuoBiao_GaoDu_buffer);
            buffers.Add(yaoceDisplay.XiTong_SuDu_Dong_buffer);
            buffers.Add(yaoceDisplay.XiTong_SuDu_Bei_buffer);
            buffers.Add(yaoceDisplay.XiTong_SuDu_Tian_buffer);
            buffers.Add(yaoceDisplay.XiTong_JiaoSuDu_Wx_buffer);
            buffers.Add(yaoceDisplay.XiTong_JiaoSuDu_Wy_buffer);
            buffers.Add(yaoceDisplay.XiTong_JiaoSuDu_Wz_buffer);
            buffers.Add(yaoceDisplay.XiTong_FaSheXi_ZXGZ_buffer);
            buffers.Add(yaoceDisplay.XiTong_FaSheXi_X_buffer);
            buffers.Add(yaoceDisplay.XiTong_FaSheXi_Y_buffer);
            buffers.Add(yaoceDisplay.XiTong_FaSheXi_Z_buffer);
            buffers.Add(yaoceDisplay.XiTong_YuShiLuoDian_SheCheng_buffer);
            buffers.Add(yaoceDisplay.XiTong_YuShiLuoDian_Z_buffer);

            //DHK
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_ZuoBiao_JingDu_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_ZuoBiao_WeiDu_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_ZuoBiao_GaoDu_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_SuDu_Dong_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_SuDu_Bei_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_SuDu_Tian_buffer);
#if false
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_ZuoBiao_JingDu_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_ZuoBiao_WeiDu_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_ZuoBiao_GaoDu_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_SuDu_Dong_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_SuDu_Bei_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_SuDu_Tian_buffer);
#endif
            //新增
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_JiaoDu_FuYangJiao_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_JiaoDu_GunZhuanJiao_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_JiaoDu_PianHangJiao_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_TuoLuo_TuoLuoX_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_TuoLuo_TuoLuoY_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_TuoLuo_TuoLuoZ_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_JiaSuDu_JiaJiX_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_JiaSuDu_JiaJiY_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Ti_JiaSuDu_JiaJiZ_buffer);

#if false
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_JiaoDu_FuYangJiao_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_JiaoDu_GunZhuanJiao_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_JiaoDu_PianHangJiao_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_TuoLuo_TuoLuoX_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_TuoLuo_TuoLuoY_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_TuoLuo_TuoLuoZ_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_JiaSuDu_JiaJiX_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_JiaSuDu_JiaJiY_buffer);
            buffers.Add(yaoceDisplay.DHKuaiSu_Tou_JiaSuDu_JiaJiZ_buffer);
#endif

            //DHM
            buffers.Add(yaoceDisplay.DHManSu_Ti_ZuoBiao_JingDu_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_ZuoBiao_WeiDu_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_ZuoBiao_GaoDu_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_SuDu_Dong_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_SuDu_Bei_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_SuDu_Tian_buffer);

            //buffers.Add(yaoceDisplay.DHManSu_Tou_ZuoBiao_JingDu_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_ZuoBiao_WeiDu_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_ZuoBiao_GaoDu_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_SuDu_Dong_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_SuDu_Bei_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_SuDu_Tian_buffer);
            //新增
            buffers.Add(yaoceDisplay.DHManSu_Ti_ZuoBiao_JingDuZuHe_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_ZuoBiao_WeiDuZuHe_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_ZuoBiao_GaoDuZuHe_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_SuDu_DongZuHe_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_SuDu_BeiZuHe_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_SuDu_TianZuHe_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_ZuoBiao_JingDuZuHe_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_ZuoBiao_WeiDuZuHe_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_ZuoBiao_GaoDuZuHe_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_SuDu_DongZuHe_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_SuDu_BeiZuHe_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_SuDu_TianZuHe_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_JiaoDu_FuYangJiao_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_JiaoDu_GunZhuanJiao_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_JiaoDu_PianHangJiao_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_TuoLuo_TuoLuoX_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_TuoLuo_TuoLuoY_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_TuoLuo_TuoLuoZ_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_JiaSuDu_JiaJiX_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_JiaSuDu_JiaJiY_buffer);
            buffers.Add(yaoceDisplay.DHManSu_Ti_JiaSuDu_JiaJiZ_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_JiaoDu_FuYangJiao_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_JiaoDu_GunZhuanJiao_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_JiaoDu_PianHangJiao_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_TuoLuo_TuoLuoX_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_TuoLuo_TuoLuoY_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_TuoLuo_TuoLuoZ_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_JiaSuDu_JiaJiX_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_JiaSuDu_JiaJiY_buffer);
            //buffers.Add(yaoceDisplay.DHManSu_Tou_JiaSuDu_JiaJiZ_buffer);


            //系统即时
            //buffers.Add(yaoceDisplay.XTJS_Tou_ZuoBiao_JingDu_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_ZuoBiao_WeiDu_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_ZuoBiao_GaoDu_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_SuDu_Dong_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_SuDu_Bei_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_SuDu_Tian_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_JiaoSuDu_Wx_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_JiaoSuDu_Wy_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_JiaoSuDu_Wz_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_GuoZai_ZhouXiang_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_GuoZai_FaXiang_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_GuoZai_CeXiang_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_ZuoBiao_JingDu_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_ZuoBiao_WeiDu_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_ZuoBiao_GaoDu_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_SuDu_Dong_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_SuDu_Bei_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_SuDu_Tian_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_JiaoSuDu_Wx_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_JiaoSuDu_Wy_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_JiaoSuDu_Wz_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_GuoZai_ZhouXiang_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_GuoZai_FaXiang_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_GuoZai_CeXiang_buffer);

            //帧序号
            buffers.Add(yaoceDisplay.XiTongPanJu15_buffer);
            buffers.Add(yaoceDisplay.XiTongPanJu16_buffer);
            //buffers.Add(yaoceDisplay.HuiLuJianCe_buffer);
            buffers.Add(yaoceDisplay.DHK_Ti_buffer);
            buffers.Add(yaoceDisplay.DHM_Ti_buffer);
            buffers.Add(yaoceDisplay.XTJS_Ti_buffer);
            //buffers.Add(yaoceDisplay.DHK_Tou_buffer);
            //buffers.Add(yaoceDisplay.DHM_Tou_buffer);
            //buffers.Add(yaoceDisplay.XTJS_Tou_buffer);
            buffers.Add(yaoceDisplay.DanTou_buffer);

            //弹头导航
            buffers.Add(yaoceDisplay.DanTou_ZuHeJingDu_buffer);
            buffers.Add(yaoceDisplay.DanTou_ZuHeWeiDu_buffer);
            buffers.Add(yaoceDisplay.DanTou_ZuHeGaoDu_buffer);
            buffers.Add(yaoceDisplay.DanTou_ZuHeDong_buffer);
            buffers.Add(yaoceDisplay.DanTou_ZuHeBei_buffer);
            buffers.Add(yaoceDisplay.DanTou_ZuHeTian_buffer);
            buffers.Add(yaoceDisplay.DanTou_GNSSJingDu_buffer);
            buffers.Add(yaoceDisplay.DanTou_GNSSWeiDu_buffer);
            buffers.Add(yaoceDisplay.DanTou_GNSSGaoDu_buffer);
            buffers.Add(yaoceDisplay.DanTou_GNSSDong_buffer);
            buffers.Add(yaoceDisplay.DanTou_GNSSBei_buffer);
            buffers.Add(yaoceDisplay.DanTou_GNSSTian_buffer);
            buffers.Add(yaoceDisplay.DanTou_FuYangJiao_buffer);
            buffers.Add(yaoceDisplay.DanTou_GunZhuanJiao_buffer);
            buffers.Add(yaoceDisplay.DanTou_PianHangJiao_buffer);
            buffers.Add(yaoceDisplay.DanTou_Wx_buffer);
            buffers.Add(yaoceDisplay.DanTou_Wy_buffer);
            buffers.Add(yaoceDisplay.DanTou_Wz_buffer);
            buffers.Add(yaoceDisplay.DanTou_XBiLi_buffer);
            buffers.Add(yaoceDisplay.DanTou_YBiLi_buffer);
            buffers.Add(yaoceDisplay.DanTou_ZBiLi_buffer);



        }

        private void InitLedStatus()
        {
            //SetLedStatus(ImageUDP, LED_STATUS.LED_GRAY);
            SetLedStatus(imageDanTou, LED_STATUS.LED_GRAY);
            SetLedStatus(imageJiShi, LED_STATUS.LED_GRAY);
            SetLedStatus(imageKuaiSu, LED_STATUS.LED_GRAY);
            SetLedStatus(imageManSu, LED_STATUS.LED_GRAY);
            SetLedStatus(imageZhuangTai, LED_STATUS.LED_GRAY);
        }

        private void InitTimer_YaoCe()
        {
            //文件加载
            UpdateLoadFileProgressTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            UpdateLoadFileProgressTimer.Tick += timerUpdateLoadFileProgress_Tick;

            /*-------系统判决状态定时器--------------------------------------*/
            UpdateXiTongStatusTimer.Interval = TimeSpan.FromMilliseconds(500);
            UpdateXiTongStatusTimer.Tick += timerUpdateXiTongStatus_Tick;

            timerOffLineXiTongStatus.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            timerOffLineXiTongStatus.Tick += timerOffLineXiTongStatus_Tick;

            timerUpdateChart.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timerUpdateChart.Tick += timerUpdateChart_Tick;
            /*---------------------------------------------------------*/

            /*--------系统导航快速弹体定时器-------------------------------------*/
            timerUpdateDHKStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerUpdateDHKStatus.Tick += timerUpdateDHKStatus_Tick;

            timerOfflineDHKStatus.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            this.timerOfflineDHKStatus.Tick += timerOfflineDHKStatus_Tick;

            /*--------系统导航快速弹头定时器-------------------------------------*/
            timerUpdateDHKStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerUpdateDHKStatus_Tou.Tick += timerUpdateDHKStatus_Tou_Tick;

            timerOfflineDHKStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            this.timerOfflineDHKStatus_Tou.Tick += timerOfflineDHKStatus_Tou_Tick;

            /*---------------------------------------------------------*/



            /*--------系统导航慢速弹体定时器--------------------------------------*/
            timerUpdateDHMStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerUpdateDHMStatus.Tick += timerUpdateDHMStatus_Tick;

            timerOfflineDHMStatus.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            this.timerOfflineDHMStatus.Tick += timerOfflineDHMStatus_Tick;


            /*--------系统导航慢速弹头定时器--------------------------------------*/
            timerUpdateDHMStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timerUpdateDHMStatus_Tou.Tick += timerUpdateDHMStatus_Tou_Tick;

            timerOfflineDHMStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            this.timerOfflineDHMStatus_Tou.Tick += timerOfflineDHMStatus_Tou_Tick;

            /*---------------------------------------------------------*/


            /*------------回路检测定时器-------------------------------------*/
            UpdateHuiLuJianCeTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            UpdateHuiLuJianCeTimer.Tick += timerUpdateHuiLuJianCe_Tick;

            timerOffLineHuiLuJianCe.Tick += timerOffLineHuiLuJianCe_Tick;
            /*---------------------------------------------------------*/


            /*-------------系统状态即时反馈弹体定时器------------------------------*/
            timerUpdateXiTongJiShiStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timerUpdateXiTongJiShiStatus.Tick += timerUpdateXiTongJiShiStatus_Tick;

            timerOfflineXiTongJiShiStatus.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            this.timerOfflineXiTongJiShiStatus.Tick += timerOfflineXiTongJiShiStatus_Tick;


            /*-------------系统状态即时反馈弹头定时器------------------------------*/
            timerUpdateXiTongJiShiStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timerUpdateXiTongJiShiStatus_Tou.Tick += timerUpdateXiTongJiShiStatus_Tou_Tick;


            timerOfflineXiTongJiShiStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            this.timerOfflineXiTongJiShiStatus_Tou.Tick += timerOfflineXiTongJiShiStatus_Tou_Tick;
            /*---------------------------------------------------------*/


            /*-------------------帧序号--------------------*/
            /*---------------------------------------------------------*/


            /*-------UDP--------------------------*/
            timerUpdateUDP.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerUpdateUDP.Tick += timerUpdateUDP_Tick;

            timerOffLineUDP.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timerOffLineUDP.Tick += timerOffLineUDP_Tick;
            /*--------------------------------------------------
             */


            // 读取文件定时器
            readFileTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnReadFileTimedEvent);
            readFileTimer.Interval = 1;
            readFileTimer.Enabled = true;

            timerConvertBar.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timerConvertBar.Tick += TimerConvertBar_Tick;
        }

        private void TimerConvertBar_Tick(object sender, EventArgs e)
        {
            w.setProgressBarValue(0, loadFileLength, alreadReadFileLength);
        }

        private void TimerConvertDanTou_Tick(object sender, EventArgs e)
        {
            if (isConvertDanTou)
            {
                showDanTouDaoHangTimeStatus(ref sObject_DanTou);
            }
        }

        private void TimerConvertXiTongJiShi_Tick(object sender, EventArgs e)
        {
            if (isConvertXiTongJiShi)
            {
                showXiTongJiShiTimeStatus_Ti(ref sObject_XTJS_Ti);
            }
        }

        private void TimerConvertDHM_Tick(object sender, EventArgs e)
        {
            if (isConvertDHM)
            {
                showDHManSuTimeStatus_Ti(ref sObject_DHM_Ti);
            }
        }

        private void TimerConvertDHK_Tick(object sender, EventArgs e)
        {
            if (isConvertDHK)
            {
                showDHKuaiSuTimeStatus_Ti(ref sObject_DHK_Ti);
            }
        }

        private void TimerConvertXiTongPanJue_Tick(object sender, EventArgs e)
        {
            if(isConvertXiTongPanJue)
            {
                showSystemTimeStatus(ref sObject_XiTong);
            }
        }

        private void InitYaoCeChartDataSource()
        {
            //系统
            chart_XiTong_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_XiTong_ZuoBiao_WeiDuList;
            chart_XiTong_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_XiTong_ZuoBiao_JingDuList;
            chart_XiTong_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_XiTong_ZuoBiao_GaoDuList;

            chart_XiTong_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_XiTong_SuDu_DongList;
            chart_XiTong_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_XiTong_SuDu_DongList;
            chart_XiTong_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_XiTong_SuDu_DongList;

            chart_XiTong_JiaoSuDu_Wx.DataSource = yaoCeChartDataSource.chart_XiTong_JiaoSuDu_WxList;
            chart_XiTong_JiaoSuDu_Wy.DataSource = yaoCeChartDataSource.chart_XiTong_JiaoSuDu_WyList;
            chart_XiTong_JiaoSuDu_Wz.DataSource = yaoCeChartDataSource.chart_XiTong_JiaoSuDu_WzList;

            chart_XiTong_FaSheXi_X.DataSource = yaoCeChartDataSource.chart_XiTong_FaSheXi_XList;
            chart_XiTong_FaSheXi_Y.DataSource = yaoCeChartDataSource.chart_XiTong_FaSheXi_YList;
            chart_XiTong_FaSheXi_Z.DataSource = yaoCeChartDataSource.chart_XiTong_FaSheXi_ZList;
            chart_XiTong_FaSheXi_ZXGZ.DataSource = yaoCeChartDataSource.chart_XiTong_FaSheXi_ZXGZList;

            chart_XiTong_YuShiLuoDian_SheCheng.DataSource = yaoCeChartDataSource.chart_XiTong_YuShiLuoDian_SheChengList;
            chart_XiTong_YuShiLuoDian_Z.DataSource = yaoCeChartDataSource.chart_XiTong_YuShiLuoDian_ZList;

            //导航快
            chart_DHKuaiSu_Ti_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_ZuoBiao_WeiDuList;
            chart_DHKuaiSu_Ti_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_ZuoBiao_JingDuList;
            chart_DHKuaiSu_Ti_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_ZuoBiao_GaoDuList;

            chart_DHKuaiSu_Ti_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_SuDu_DongList;
            chart_DHKuaiSu_Ti_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_SuDu_BeiList;
            chart_DHKuaiSu_Ti_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_SuDu_TianList;

            chart_DHKuaiSu_Ti_JiaoDu_FuYangJiao.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaoDu_FuYangJiaoList;
            chart_DHKuaiSu_Ti_JiaoDu_GunZhuanJiao.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaoDu_GunZhuanJiaoList;
            chart_DHKuaiSu_Ti_JiaoDu_PianHangJiao.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaoDu_PianHangJiaoList;
            chart_DHKuaiSu_Ti_TuoLuo_TuoLuoX.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_TuoLuo_TuoLuoXList;
            chart_DHKuaiSu_Ti_TuoLuo_TuoLuoY.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_TuoLuo_TuoLuoYList;
            chart_DHKuaiSu_Ti_TuoLuo_TuoLuoZ.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_TuoLuo_TuoLuoZList;
            chart_DHKuaiSu_Ti_JiaSuDu_JiaJiX.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaSuDu_JiaJiXList;
            chart_DHKuaiSu_Ti_JiaSuDu_JiaJiY.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaSuDu_JiaJiYList;
            chart_DHKuaiSu_Ti_JiaSuDu_JiaJiZ.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Ti_JiaSuDu_JiaJiZList;


            //DHM
            chart_DHManSu_Ti_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_JingDuList;
            chart_DHManSu_Ti_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_GaoDuList;
            chart_DHManSu_Ti_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_WeiDuList;

            chart_DHManSu_Ti_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_DongList;
            chart_DHManSu_Ti_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_BeiList;
            chart_DHManSu_Ti_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_TianList;

          

            //新增
            chart_DHManSu_Ti_ZuoBiao_WeiDu_ZuHe.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_WeiDuZuHeList;
            chart_DHManSu_Ti_ZuoBiao_JingDu_ZuHe.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_JingDuZuHeList;
            chart_DHManSu_Ti_ZuoBiao_GaoDu_ZuHe.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_GaoDuZuHeList;

            chart_DHManSu_Ti_SuDu_Dong_ZuHe.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_DongZuHeList;
            chart_DHManSu_Ti_SuDu_Bei_ZuHe.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_BeiZuHeList;
            chart_DHManSu_Ti_SuDu_Tian_ZuHe.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_SuDu_TianZuHeList;

         

            chart_DHManSu_Ti_SuDu_FuYangJiao.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_JiaoDu_FuYangJiaoList;
            chart_DHManSu_Ti_SuDu_GunZhuanJiao.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_JiaoDu_GunZhuanJiaoList;
            chart_DHManSu_Ti_SuDu_PianHangJiao.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_JiaoDu_PianHangJiaoList;
            chart_DHManSu_Ti_SuDu_TuoLuoX.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_TuoLuo_TuoLuoXList;
            chart_DHManSu_Ti_SuDu_TuoLuoY.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_TuoLuo_TuoLuoYList;
            chart_DHManSu_Ti_SuDu_TuoLuoZ.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_TuoLuo_TuoLuoZList;
            chart_DHManSu_Ti_SuDu_JiaJiX.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_JiaSuDu_JiaJiXList;
            chart_DHManSu_Ti_SuDu_JiaJiY.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_JiaSuDu_JiaJiYList;
            chart_DHManSu_Ti_SuDu_JiaJiZ.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_JiaSuDu_JiaJiZList;
           


            //系统状态即时
            chart_XTJS_Ti_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_WeiDuList;
            chart_XTJS_Ti_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_JingDuList;
            chart_XTJS_Ti_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_GaoDuList;

            chart_XTJS_Ti_JiaoSuDu_Wx.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_JiaoSuDu_WxList;
            chart_XTJS_Ti_JiaoSuDu_Wy.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_JiaoSuDu_WyList;
            chart_XTJS_Ti_JiaoSuDu_Wz.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_JiaoSuDu_WzList;

            chart_XTJS_Ti_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_SuDu_DongList;
            chart_XTJS_Ti_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_SuDu_BeiList;
            chart_XTJS_Ti_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_SuDu_TianList;

            chart_XTJS_Ti_GuoZai_ZhouXiang.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_ZhouXiangList;
            chart_XTJS_Ti_GuoZai_FaXiang.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_FaXiangList;
            chart_XTJS_Ti_GuoZai_CeXiang.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_CeXiangList;

           
            //弹头导航数据
            

            //帧序号
            chart_XiTongPanJu15.DataSource = yaoCeChartDataSource.chart_XiTongPanJu15List;
            chart_XiTongPanJu16.DataSource = yaoCeChartDataSource.chart_XiTongPanJu16List;
            chart_DHM_Ti.DataSource = yaoCeChartDataSource.chart_DHM_TiList;
            chart_XTJS_Ti.DataSource = yaoCeChartDataSource.chart_XTJS_TiList;
            chart_DanTou.DataSource = yaoCeChartDataSource.chart_DanTouList;

            //弹头导航数据
            chart_DanTou_ZuoBiao_JingDu_ZuHe.DataSource = yaoCeChartDataSource.chart_DanTou_ZuHeJingDu;
            chart_DanTou_ZuoBiao_WeiDu_ZuHe.DataSource = yaoCeChartDataSource.chart_DanTou_ZuHeWeiDu;
            chart_DanTou_ZuoBiao_GaoDu_ZuHe.DataSource = yaoCeChartDataSource.chart_DanTou_ZuHeGaoDu;

            chart_DanTou_SuDu_Dong_ZuHe.DataSource = yaoCeChartDataSource.chart_DanTou_ZuHeDong;
            chart_DanTou_SuDu_Bei_ZuHe.DataSource = yaoCeChartDataSource.chart_DanTou_ZuHeBei;
            chart_DanTou_SuDu_tian_ZuHe.DataSource = yaoCeChartDataSource.chart_DanTou_ZuHeTian;

            chart_DanTou_ZuoBiao_JingDu_GNSS.DataSource = yaoCeChartDataSource.chart_DanTou_GNSSJingDu;
            chart_DanTou_ZuoBiao_WeiDu_GNSS.DataSource = yaoCeChartDataSource.chart_DanTou_GNSSWeiDu;
            chart_DanTou_ZuoBiao_GaoDu_GNSS.DataSource = yaoCeChartDataSource.chart_DanTou_GNSSGaoDu;

            chart_DanTou_SuDu_Dong_GNSS.DataSource = yaoCeChartDataSource.chart_DanTou_GNSSDong;
            chart_DanTou_SuDu_Bei_GNSS.DataSource = yaoCeChartDataSource.chart_DanTou_GNSSBei;
            chart_DanTou_SuDu_tian_GNSS.DataSource = yaoCeChartDataSource.chart_DanTou_GNSSTian;

            chart_DanTou_JiaoSuDu_Wx.DataSource = yaoCeChartDataSource.chart_DanTou_Wx;
            chart_DanTou_JiaoSuDu_Wy.DataSource = yaoCeChartDataSource.chart_DanTou_Wy;
            chart_DanTou_JiaoSuDu_Wz.DataSource = yaoCeChartDataSource.chart_DanTou_Wz;

            chart_DANTOU_JiaoDu_FuYangJiao.DataSource = yaoCeChartDataSource.chart_DanTou_FuYangJiao;
            chart_DANTOU_JiaoDu_GunZhuanJiao.DataSource = yaoCeChartDataSource.chart_DanTou_GunZhuanJiao;
            chart_DANTOU_JiaoDu_PianHangJiao.DataSource = yaoCeChartDataSource.chart_DanTou_PianHangJiao;

            chart_DanTou_XBiLi.DataSource = yaoCeChartDataSource.chart_DanTou_XBiLi;
            chart_DanTou_YBiLi.DataSource = yaoCeChartDataSource.chart_DanTou_YBiLi;
            chart_DanTou_ZBiLi.DataSource = yaoCeChartDataSource.chart_DanTou_ZBiLi;


        }

        //曲线定时器
        private void timerUpdateChart_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < buffers.Count(); i++)
            {
                if (buffers[i].Count > 0)
                {
                    //TemplateDraw(buffers.ElementAt(i), chartPointDataSources.ElementAt(i));
                    buffers[i].ForEach(packet =>
                    {
                        chartPointDataSources[i].AddPoint(packet);
                    });
                    chartPointDataSources[i].NotifyDataChanged();
                }
            }
            yaoceDisplay.Clear();
        }

        delegate void OnReadFileTimedEventCallBack(Object source, ElapsedEventArgs e);

        public void OnReadFileTimedEvent(Object source, ElapsedEventArgs e)
        {
            
            //判断当前线程是否是主线程
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (srFileRead == null)
                    {
                        return;
                    }

                     int fsLen = 0;
                    // 按字节读取数据
                    if (dataConversion)
                    {
                        fsLen = 10240;
                    }
                    else
                    {
                         fsLen = UDPLENGTH;
                    }

                    //Console.WriteLine("{0}, {1}", i++,fsLen);

                    byte[] heByte = new byte[fsLen];
                    int readLength = 0;
                    if ((readLength = srFileRead.Read(heByte, 0, heByte.Length)) > 0)
                    {
                        //处理数据
                        if (readLength < fsLen)
                        {
                            byte[] byteArray = new byte[readLength];
                            Array.Copy(heByte, 0, byteArray, 0, readLength);
                            yaoceParserFile.Enqueue(byteArray);
                        }
                        else
                        {
                            yaoceParserFile.Enqueue(heByte);
                        }
                        // 已经读取的文件大小
                        alreadReadFileLength += readLength;
                    }
                    else
                    {
                        if (!dataConversion)
                        {
                            // 关闭文件
                            srFileRead.Close();

                            // 关闭文件读取定时器
                            readFileTimer.Stop();

                            // 文件置空// 
                            srFileRead = null;

                            // 禁用按钮
                            // btnLoadFile.Enabled = true; 

                            // 停止加载文件进度
                            UpdateLoadFileProgressTimer.Stop();

                            // 更新进度条
                            load.setProgressBarValue(0, loadFileLength, loadFileLength, "100%");
                            load.loadFileFinish();

                            // 日志打印
                            Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "历史数据加载完成！");

                            MessageBox.Show("文件读取完成！");

                            // 线程休眠使用间隔时间(等待数据处理完成，而不是读取完毕，立即关闭定时器刷新)
                            Thread.Sleep(Interval);

                            // 关闭数据解析
                            yaoceParserFile.Stop();

                            // 停止绘图定时器刷新数据
                            setTimerUpdateChartStatus(false);

                            // 关闭状态刷新定时器
                            setUpdateTimerStatus(false);
                        }
                        else
                        {
                            // 关闭文件
                            srFileRead.Close();

                            // 关闭文件读取定时器
                            readFileTimer.Stop();

                            // 文件置空// 
                            srFileRead = null;

                            // 线程休眠使用间隔时间(等待数据处理完成，而不是读取完毕，立即关闭定时器刷新)
                            Thread.Sleep(Interval);

                            // 关闭数据解析
                            yaoceParserFile.Stop();
                            timerConvertBar.Stop();
                            w.setProgressBarValue(0, 100, 100);
                            w.Close();

                            dataConversion = false; //转换文件停止

                            foreach (StringBuilder s in stringBuilders)
                            {
                                streamXiTongPanJue.WriteLine(s);
                            }
                            stringBuilders.Clear();

                            foreach (StringBuilder s in stringBuilders_DHK)
                            {
                                streamDHK.WriteLine(s);
                            }
                            stringBuilders_DHK.Clear();

                            foreach (StringBuilder s in stringBuilders_DHM)
                            {
                                streamDHM.WriteLine(s);
                            }
                            stringBuilders_DHM.Clear();

                            foreach (StringBuilder s in stringBuilders_XTJS)
                            {
                                streamXTJS.WriteLine(s);
                            }
                            stringBuilders_XTJS.Clear();

                            foreach (StringBuilder s in stringBuilders_DanTou)
                            {
                                streamDanTou.WriteLine(s);
                            }
                            stringBuilders_DanTou.Clear();

                            streamXiTongPanJue?.Close();
                            streamDHK?.Close();
                            streamDHM?.Close();
                            streamXTJS?.Close();
                            streamDanTou?.Close();
                            System.Windows.MessageBox.Show("数据文件转换成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }), null);
            }
            else
            {
                if (srFileRead == null)
                {
                    return;
                }

                //Console.WriteLine("{0}", i++);
                // 按字节读取数据
                int fsLen = 0;
                // 按字节读取数据
                if (dataConversion)
                {
                    fsLen = 10240;
                }
                else
                {
                    fsLen = UDPLENGTH;
                }
                byte[] heByte = new byte[fsLen];
                int readLength = 0;
                if ((readLength = srFileRead.Read(heByte, 0, heByte.Length)) > 0)
                {
                    // 处理数据
                    if (readLength < fsLen)// 
                    {
                        byte[] byteArray = new byte[readLength];
                        Array.Copy(heByte, 0, byteArray, 0, readLength);
                        yaoceParserFile.Enqueue(byteArray);

                    }
                    else
                    {
                        yaoceParserFile.Enqueue(heByte);
                    }
                    // 已经读取的文件大小
                    alreadReadFileLength += readLength;
                }
                else
                {
                    if (!dataConversion)
                    {
                        // 关闭文件
                        srFileRead.Close();

                        // 关闭文件读取定时器
                        readFileTimer.Stop();

                        // 文件置空// 
                        srFileRead = null;

                        // 禁用按钮
                        // btnLoadFile.Enabled = true; 

                        // 停止加载文件进度
                        UpdateLoadFileProgressTimer.Stop();

                        // 更新进度条
                        load.setProgressBarValue(0, loadFileLength, loadFileLength, "100%");
                        load.loadFileFinish();

                        // 日志打印
                        Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "历史数据加载完成！");

                        MessageBox.Show("文件读取完成！");

                        // 线程休眠使用间隔时间(等待数据处理完成，而不是读取完毕，立即关闭定时器刷新)
                        Thread.Sleep(Interval);

                        // 关闭数据解析
                        yaoceParserFile.Stop();

                        // 停止绘图定时器刷新数据
                        setTimerUpdateChartStatus(false);

                        // 关闭状态刷新定时器
                        setUpdateTimerStatus(false);
                    }
                    else
                    {
                        // 关闭文件
                        srFileRead.Close();

                        // 关闭文件读取定时器
                        readFileTimer.Stop();

                        // 文件置空// 
                        srFileRead = null;

                        // 线程休眠使用间隔时间(等待数据处理完成，而不是读取完毕，立即关闭定时器刷新)
                        Thread.Sleep(Interval);

                        // 关闭数据解析
                        yaoceParserFile.Stop();

                        timerConvertBar.Stop();
                        w.setProgressBarValue(0, 100, 100);
                        w.Close();

                        foreach (StringBuilder s in stringBuilders)
                        {
                            streamXiTongPanJue.WriteLine(s);
                        }
                        stringBuilders.Clear();

                        foreach (StringBuilder s in stringBuilders_DHK)
                        {
                            streamDHK.WriteLine(s);
                        }
                        stringBuilders_DHK.Clear();

                        foreach (StringBuilder s in stringBuilders_DHM)
                        {
                            streamDHM.WriteLine(s);
                        }
                        stringBuilders_DHM.Clear();

                        foreach (StringBuilder s in stringBuilders_XTJS)
                        {
                            streamXTJS.WriteLine(s);
                        }
                        stringBuilders_XTJS.Clear();

                        foreach (StringBuilder s in stringBuilders_DanTou)
                        {
                            streamDanTou.WriteLine(s);
                        }
                        stringBuilders_DanTou.Clear();

                        dataConversion = false;
                        streamXiTongPanJue?.Close();
                        streamDHK?.Close();
                        streamDHM?.Close();
                        streamXTJS?.Close();
                        streamDanTou?.Close();
                        System.Windows.MessageBox.Show("数据文件转换成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void timerUpdateLoadFileProgress_Tick(object sender, EventArgs e)
        {
            double percent = (double)alreadReadFileLength / (double)loadFileLength;
            percent *= 100;

            // 日志打印
            Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "数据加载：" + percent.ToString("f2") + "%");

            // 更新进度条
            load.setProgressBarValue(0, loadFileLength, alreadReadFileLength, percent.ToString("f2") + "%");
        }


        /*---------通过定时器定时刷新数据-----------*/
        //系统判决数据
        private void timerUpdateXiTongStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_XiTong)
            {
                // 填充实时数据
                showSystemTimeStatus(ref sObject_XiTong);
                setStatusOnOffLine(E_STATUSTYPE_XiTong, true);
                GenericFunction.changeAllTextEditColor(this.XiTong);
                
            }
        }

        //系统判决离线状态
        private void timerOffLineXiTongStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            bRecvStatusData_XiTong = false;
            setStatusOnOffLine(E_STATUSTYPE_XiTong, false);
        }

        /*--------------------------------导航快速--------------------------------------*/
        //导航快速弹体数据
        private void timerUpdateDHKStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_DHK)
            {

                setStatusOnOffLine(E_STATUSTYPE_DaoHangKuaiSu_Ti, true);
                // 填充实时数据
                showDHKuaiSuTimeStatus_Ti(ref sObject_DHK_Ti);
                GenericFunction.changeAllTextEditColor(this.DaoHangKuai_Ti);
            }
        }

        //导航快速弹体离线状态
        private void timerOfflineDHKStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            bRecvStatusData_DHK = false;
            setStatusOnOffLine(E_STATUSTYPE_DaoHangKuaiSu_Ti, false);
        }

        //导航快速弹头数据
        private void timerUpdateDHKStatus_Tou_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            //if (bReceStatusData_DANTOU)
            //{
            //    // 填充实时数据
            //    setStatusOnOffLine(E_STATUSTYPE_DaoHangKuaiSu_Tou, true);
            //    showDanTouDaoHangTimeStatus(ref sObject_DanTou);
            //    GenericFunction.changeAllTextEditColor(this.DanTou);
            //}
        }

        //导航快速弹头离线状态
        private void timerOfflineDHKStatus_Tou_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            //bReceStatusData_DANTOU = false;
            //setStatusOnOffLine(E_STATUSTYPE_DaoHangKuaiSu_Tou, false);
        }
        /*------------------------------------------------------------------*/


        /*-------------------------------导航慢速-----------------------------------*/
        //导航慢速弹体数据
        private void timerUpdateDHMStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_DHM)
            {

                setStatusOnOffLine(E_STATUSTYPE_DaoHangManSu_Ti, true);

                if (bRecvStatusData_DHM_Ti)
                {
                    // 填充实时数据
                    showDHManSuTimeStatus_Ti(ref sObject_DHM_Ti);
                    GenericFunction.changeAllTextEditColor(this.DaoHangMan_Ti);
                    GenericFunction.changeAllTextEditColor(this.DaoHangMan_Ti2);
                }
            }
        }

        //导航慢速弹体离线状态
        private void timerOfflineDHMStatus_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            bRecvStatusData_DHM = false;
            setStatusOnOffLine(E_STATUSTYPE_DaoHangManSu_Ti, false);
        }

        //导航慢速弹头数据
        private void timerUpdateDHMStatus_Tou_Tick(object sender, EventArgs e)
        {
                if (bReceStatusData_DANTOU)
                {
                // 填充实时数据
                setStatusOnOffLine(E_STATUSTYPE_DaoHangManSu_Tou, true);
                showDanTouDaoHangTimeStatus(ref sObject_DanTou);
                GenericFunction.changeAllTextEditColor(DanTou);
                }
        }

        //导航慢速弹头离线状态
        private void timerOfflineDHMStatus_Tou_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            bReceStatusData_DANTOU = false;
            setStatusOnOffLine(E_STATUSTYPE_DaoHangManSu_Tou, false);
        }
        /*------------------------------------------------------------------*/


        /*----------------------------------回路检测--------------------------------*/
        //回路检测数据
        private void timerUpdateHuiLuJianCe_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            //if (bRecvStatusData_HuiLuJianCe)
            //{
            //    // 填充实时数据
            //    showHuiLuJianCeStatus(ref sObject_huiLuJianCe);
            //    setStatusOnOffLine(E_STATUSTYPE_HuiLuJianCe, true);
            //}
        }

        //回路检测离线状态
        private void timerOffLineHuiLuJianCe_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            //bRecvStatusData_HuiLuJianCe = false;
            //setStatusOnOffLine(E_STATUSTYPE_HuiLuJianCe, false);
        }
        /*----------------------------------------------------------------------*/


        /*------------------------------------------------------------------*/
        //更新UDP状态
        private void timerUpdateUDP_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_UDP)
            {
                setStatusOnOffLine(E_STATUSTYPE_dataConnect, true);
            }
        }

        //UDP连接离线
        private void timerOffLineUDP_Tick(object sender, EventArgs e)
        {
            bRecvStatusData_UDP = false;
            setStatusOnOffLine(E_STATUSTYPE_dataConnect, false);
        }
        /*------------------------------------------------------------------*/


        /*-----------------------------------系统即时反馈-----------------------------------*/
        //系统即时反馈数据弹体
        private void timerUpdateXiTongJiShiStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_XTJS)
            {

                setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, true);

                if (bRecvStatusData_XTJS_Ti)
                {
                    // 填充实时数据
                    showXiTongJiShiTimeStatus_Ti(ref sObject_XTJS_Ti);
                    GenericFunction.changeAllTextEditColor(this.FanKui_Ti);
                    GenericFunction.changeAllTextEditColor(this.FanKui_Ti2);
                }
            }
        }

        //系统即时离线状态弹体
        private void timerOfflineXiTongJiShiStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            bRecvStatusData_XTJS = false;
            setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, false);
        }

        //系统即时反馈数据弹头
        private void timerUpdateXiTongJiShiStatus_Tou_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            //if (bRecvStatusData_XTJS)
            //{

            //    setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, true);

            //    if (bRecvStatusData_XTJS_Tou)
            //    {
            //        // 填充实时数据
            //        showXiTongJiShiTimeStatus_Tou(ref sObject_XTJS_Tou);
            //        GenericFunction.changeAllTextEditColor(this.FanKui_Tou);
            //        GenericFunction.changeAllTextEditColor(this.FanKui_Tou2);
            //    }
            //}
        }

        //系统即时离线状态弹头
        private void timerOfflineXiTongJiShiStatus_Tou_Tick(object sender, EventArgs e)
        {
            //// 是否收到数据
            //bRecvStatusData_XTJS = false;
            //setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, false);
        }
        /*------------------------------------------------------------------------*/



        //更新定时器状态
        public void setUpdateTimerStatus(bool bOpen)
        {
            if (bOpen)
            {
                UpdateXiTongStatusTimer.Start();//系统判决数据
                UpdateHuiLuJianCeTimer.Start();//回路检测数据
                timerUpdateXiTongJiShiStatus.Start();//系统即时反馈弹体数据
                timerUpdateXiTongJiShiStatus_Tou.Start();//系统即时反馈弹头数据
                timerUpdateDHKStatus.Start();//导航快速弹体数据
                timerUpdateDHKStatus_Tou.Start();//导航快速弹头数据
                timerUpdateDHMStatus.Start();//导航慢速弹体数据
                timerUpdateDHMStatus_Tou.Start();//导航慢速弹头数据
                timerUpdateUDP.Start();//UDP
            }
            else
            {
                UpdateXiTongStatusTimer.Stop();
                UpdateHuiLuJianCeTimer.Stop();
                timerUpdateXiTongJiShiStatus.Stop();
                timerUpdateXiTongJiShiStatus_Tou.Stop();
                timerUpdateDHKStatus.Stop();
                timerUpdateDHKStatus_Tou.Stop();
                timerUpdateDHMStatus.Stop();
                timerUpdateDHMStatus_Tou.Stop();
                timerUpdateUDP.Stop();
            }
        }
        //更新曲线定时器
        public void setTimerUpdateChartStatus(bool bOpen)
        {
            if (bOpen)
            {
                timerUpdateChart.Start();
            }
            else
            {
                timerUpdateChart.Stop();
            }
        }

        //通过加载离线文件界面控制文件的读取与播放
        public void setOffLineFilePlayStatus(int action, int param1 = 0)
        {
            switch (action)
            {
                case E_LOADFILE_START:
                    {
                        if (!dataConversion)
                        {
                            string fileName = load.getLoadFileName();
                            if (System.IO.File.Exists(fileName))
                            {
                                startLoadOffLineFile(fileName);
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists(ParaseFileName))
                            {
                                initYaoCeParser();
                                string path = "./YaoCeConfigFile/系统实时判决.txt";
                                string pathDHK = "./YaoCeConfigFile/导航快速.txt";
                                string pathDHM = "./YaoCeConfigFile/导航慢速.txt";
                                string pathXTJS = "./YaoCeConfigFile/系统状态即时反馈.txt";
                                string pathDanTou = "./YaoCeConfigFile/弹头导航.txt";

                                string XiTongPanJuePath = Path.Combine(ParaseFileToCSVPath, "系统判决状态数据.csv");
                                string DHKPath = Path.Combine(ParaseFileToCSVPath, "导航快速数据.csv");
                                string DHMPath = Path.Combine(ParaseFileToCSVPath, "导航慢速数据.csv");
                                string XTJSPath = Path.Combine(ParaseFileToCSVPath, "系统状态即时反馈数据.csv");
                                string DanTouPath = Path.Combine(ParaseFileToCSVPath, "弹头导航数据.csv");

                             #if false
                                streamXiTongPanJue = new StreamWriter(XiTongPanJuePath, false, Encoding.GetEncoding("GB2312"));
                                streamDHK = new StreamWriter(DHKPath, false, Encoding.GetEncoding("GB2312"));
                                streamDHM = new StreamWriter(DHMPath, false, Encoding.GetEncoding("GB2312"));
                                streamXTJS = new StreamWriter(XTJSPath, false, Encoding.GetEncoding("GB2312"));
                                streamDanTou = new StreamWriter(DanTouPath, false, Encoding.GetEncoding("GB2312"));
                             #endif
                                try
                                {
                                    streamXiTongPanJue = new StreamWriter(XiTongPanJuePath, false, Encoding.GetEncoding("GB2312"));
                                    streamDHK = new StreamWriter(DHKPath, false, Encoding.GetEncoding("GB2312"));
                                    streamDHM = new StreamWriter(DHMPath, false, Encoding.GetEncoding("GB2312"));
                                    streamXTJS = new StreamWriter(XTJSPath, false, Encoding.GetEncoding("GB2312"));
                                    streamDanTou = new StreamWriter(DanTouPath, false, Encoding.GetEncoding("GB2312"));
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "警告");
                                    streamXiTongPanJue?.Close();
                                    streamDHK?.Close();
                                    streamDHM?.Close();
                                    streamXTJS?.Close();
                                    streamDanTou?.Close();
                                    return;
                                }

                                streamXiTongPanJue.WriteLine(WriteCSV.ReadTxT(path).ToString());
                                streamDHK.WriteLine(WriteCSV.ReadTxT(pathDHK).ToString());
                                streamDHM.WriteLine(WriteCSV.ReadTxT(pathDHM).ToString());
                                streamXTJS.WriteLine(WriteCSV.ReadTxT(pathXTJS).ToString());
                                streamDanTou.WriteLine(WriteCSV.ReadTxT(pathDanTou).ToString());
                                startLoadOffLineFile(ParaseFileName);
                                w = new TransformationProgress();
                                w.ShowDialog();
                                timerConvertBar.Start();
                            }
                        }

                    }
                    break;
                case E_LOADFILE_PAUSE:
                    {
                        Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "暂停加载历史数据！");

                        // 停止文件读取定时器
                        readFileTimer.Stop();

                        // 刷新加载文件进度
                        UpdateLoadFileProgressTimer.Stop();
                    }
                    break;
                case E_LOADFILE_CONTINUE: // 
                    {
                        Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "启动加载历史数据！");

                        // 打开文件读取定时器                         
                        readFileTimer.Start();

                        // 刷新加载文件进度  // 
                        UpdateLoadFileProgressTimer.Start();
                    }
                    // 
                    break;
                case E_LOADFILE_STOP:
                    {
                        Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "停止加载历史数据！");

                        // 关闭文件
                        srFileRead.Close();

                        // 文件置空
                        srFileRead = null;

                        // 停止文件读取定时器 
                        readFileTimer.Stop();

                        // 刷新加载文件进度
                        UpdateLoadFileProgressTimer.Stop();

                        // 线程休眠使用间隔时间(等待数据处理完成，而不是读取完毕，立即关闭定时器刷新)
                        Thread.Sleep(Interval);

                        // 关闭数据解析
                        yaoceParserFile.Stop();

                        // 关闭绘图定时器刷新数据
                        setTimerUpdateChartStatus(false);

                        // 关闭状态刷新定时器
                        setUpdateTimerStatus(false);


                    }
                    break;
                // 进度跳转
                case E_LOADFILE_SKIPPROGRAM:
                    {
                        // 停止文件读取定时器
                        readFileTimer.Stop();

                        // 取UDP长度的整数倍
                        long skipValue = (long)((double)loadFileLength / 100.0f * param1);
                        skipValue = (long)(skipValue / UDPLENGTH) * UDPLENGTH;

                        // 文件读取指针偏移
                        srFileRead.Seek(skipValue, 0);

                        // 更改已经读取的文件大小
                        alreadReadFileLength = skipValue;

                        // 开启文件读取定时器
                        readFileTimer.Start();
                    }
                    break;
                default:
                    break;
            }
        }

        //离线加载文件
        private void startLoadOffLineFile(string filePath)
        {

            // 打开文件
            srFileRead = new FileStream(filePath, FileMode.Open);

            // 文件大小 
            FileInfo fileInfo = new FileInfo(filePath);
            loadFileLength = fileInfo.Length;

            // 已经读取的文件大小
            alreadReadFileLength = 0;

            // 打开文件读取定时器
            readFileTimer.Start();

            // 开启数据解析
            yaoceParserFile.Start();

            if (!dataConversion) //文件转换，不显示数据
            {
                //清空所有的曲线
                ClearAllChart();

                // 启动绘图定时器刷新数据
                setTimerUpdateChartStatus(true);

                // 刷新加载文件进度
                UpdateLoadFileProgressTimer.Start();

                // 开启状态刷新定时器
                setUpdateTimerStatus(true);

                // NOTE 20200525 每次重新回放重置数据显示界面 
                ResetAllTextEdit();

                // 是否收到数据
                bRecvStatusData_XiTong = false;
                bRecvStatusData_HuiLuJianCe = false;
                bRecvStatusData_XTJS = false;
                bRecvStatusData_DHK = false;
                bRecvStatusData_DHM = false;
                bRecvStatusData_DHM_Ti = false;
                bRecvStatusData_DHM_Tou = false;
                bRecvStatusData_XTJS_Ti = false;
                bRecvStatusData_XTJS_Tou = false;
                bReceStatusData_DANTOU = false;
            }
            else
            {
                timerConvertBar.Start();
                isConvertXiTongPanJue = false;
                isConvertXiTongJiShi = false;
                isConvertDHK = false;
                isConvertDHM = false;
                isConvertDanTou = false;
            }

        }

        //字符串转16进制字节数组
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        //回路检测反馈数据显示(弃用)
        private void showHuiLuJianCeStatus(ref HUILUJIANCE_STATUS sObject)
        {
            HuiLu_ShuChuHuiLuDianZu1.Text = sObject.shuChu1HuiLuDianZu.ToString("2f"); //    // 电机驱动输出1回路电阻
                                                                                       // 
            HuiLu_ShuChuHuiLuDianZu2.Text = sObject.shuChu2HuiLuDianZu.ToString("2f"); //    // 电机驱动输出2回路电阻
                                                                                       // 
            HuiLu_QiHuoHuiLuDianZu1A.Text = sObject.QBDH1AHuiLuDianZu.ToString("2f"); //      // 起爆点火1A回路电阻
                                                                                      // 
            HuiLu_QiHuoHuiLuDianZu1B.Text = sObject.QBDH1BHuiLuDianZu.ToString("2f"); //      // 起爆点火1B回路电阻
                                                                                      // 
            HuiLu_QiHuoHuiLuDianZu2A.Text = sObject.QBDH2AHuiLuDianZu.ToString("2f"); //      // 起爆点火2A回路电阻
                                                                                      // 
            HuiLu_QiHuoHuiLuDianZu2B.Text = sObject.QBDH2BHuiLuDianZu.ToString("2f"); //      // 起爆点火2B回路电阻
                                                                                      // 
        }

#if true
        //系统判决实时状态显示
        private void showSystemTimeStatus(ref SYSTEMPARSE_STATUS sObject)
        {
            if (!dataConversion)
            {
                // 经度
                XiTong_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString();

                // 纬度
                XiTong_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString();

                // 海拔高度
                XiTong_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString();

                // 东向速度
                XiTong_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString();

                // 北向速度
                XiTong_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString();

                // 天向速度
                XiTong_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString();

                // Wx角速度
                XiTong_WxJiaoSuDuValue.Text = sObject.WxJiaoSuDu.ToString();

                // Wy角速度
                XiTong_WyJiaoSuDuValue.Text = sObject.WyJiaoSuDu.ToString();

                // Wz角速度
                XiTong_WzJiaoSuDuValue.Text = sObject.WzJiaoSuDu.ToString();

                // 当前发射系X
                XiTong_XFaSheXi.Text = sObject.curFaSheXi_X.ToString();

                // 当前发射系Y
                XiTong_YFaSheXi.Text = sObject.curFaSheXi_Y.ToString();

                // 当前发射系Z
                XiTong_ZFaSheXi.Text = sObject.curFaSheXi_Z.ToString();

                // GNSS时间
                XiTong_GNSSTime.Text = ((double)(sObject.GNSSTime * Math.Pow(10, -3))).ToString();

                // 飞行总时间
                XiTong_ZongFeiXingTime.Text = sObject.feiXingZongShiJian.ToString();

                //参试状态 0x00:无意义，0x01:正式实验，0x10：测试1，数据输出状态
                string canShiZhuangTai = "";
                switch (sObject.canShiZhuangTai)
                {
                    case 0:
                        canShiZhuangTai = "无意义";
                        break;

                    case 1:
                        canShiZhuangTai = "正式试验";
                        break;

                    case 2:
                        canShiZhuangTai = "测试1,数据输出状态";
                        break;

                    default:
                        canShiZhuangTai = "错误数据";
                        break;
                }
                XiTong_CanShiZhuangTai.Text = canShiZhuangTai;

                // 策略阶段(0-准备 1-起飞 2-一级 3-二级 4-结束)
                string ceLueJieDuanValue = "";
                switch (sObject.ceLueJieDuan)
                {
                    case 0:
                        ceLueJieDuanValue = "准备";
                        break;
                    case 1:
                        ceLueJieDuanValue = "起飞";
                        break;
                    case 2:
                        ceLueJieDuanValue = "一级";
                        break;
                    case 3:
                        ceLueJieDuanValue = "二级";
                        break;
                    case 4:
                        ceLueJieDuanValue = "结束";
                        break;
                    default:
                        break;
                }
                XiTong_CeLueJieDuan.Text = ceLueJieDuanValue;

                // 
                // 策略判决结果1
                // 
                byte jueCePanJueJieGuo1 = sObject.jueCePanJueJieGuo1;

                // bit0 总飞行时间（1：有效                                                     
                int celv10 = (jueCePanJueJieGuo1 >> 0 & 0x1) == 1 ? 1 : 0; //

                // bit1 侧向（1：有效）
                //XiTong_CeXiang.Text = (jueCePanJueJieGuo1 >> 1 & 0x1) == 1 ? "有效" : "无效";
                int celv11 = (jueCePanJueJieGuo1 >> 1 & 0x1) == 1 ? 1 : 0;

                // bit2 Wx角速度（1：有效）
                //XiTong_WxJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 2 & 0x1) == 1 ? "有效" : "无效";
                int celv12 = (jueCePanJueJieGuo1 >> 2 & 0x1) == 1 ? 1 : 0;

                // bit3 Wy角速度（1：有效）
                //XiTong_WyJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 3 & 0x1) == 1 ? "有效" : "无效";
                int celv13 = (jueCePanJueJieGuo1 >> 3 & 0x1) == 1 ? 1 : 0;

                // bit4 Wz角速度（1：有效）
                //XiTong_WzJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 4 & 0x1) == 1 ? "有效" : "无效"; //
                int celv14 = (jueCePanJueJieGuo1 >> 4 & 0x1) == 1 ? 1 : 0;

                // bit5 后向（1：有效）
                //XiTong_HouXiang.Text = (jueCePanJueJieGuo1 >> 5 & 0x1) == 1 ? "有效" : "无效"; //
                int celv15 = (jueCePanJueJieGuo1 >> 5 & 0x1) == 1 ? 1 : 0;

                // bit6 坠落（1：有效）
                //XiTong_ZhuiLuo.Text = (jueCePanJueJieGuo1 >> 6 & 0x1) == 1 ? "有效" : "无效"; //
                int celv16 = (jueCePanJueJieGuo1 >> 6 & 0x1) == 1 ? 1 : 0;

                // bit7 分离时间（1：有效） 
                //XiTong_FenLiShiTian.Text = (jueCePanJueJieGuo1 >> 7 & 0x1) == 1 ? "有效" : "无效"; //
                int celv17 = (jueCePanJueJieGuo1 >> 7 & 0x1) == 1 ? 1 : 0;

                // 策略判决结果2
                //
                byte jueCePanJueJieGuo2 = sObject.jueCePanJueJieGuo2;

                // bit0 控制区下限（1：有效）                                                        
                int celv_2_0 = (jueCePanJueJieGuo2 >> 0 & 0x1) == 1 ? 1 : 0;

                // bit1 控制区上限（1：有效）
                //XiTong_KongZhiQuShangXian.Text = (jueCePanJueJieGuo2 >> 1 & 0x1) == 1 ? "有效" : "无效"; //
                int celv_2_1 = (jueCePanJueJieGuo2 >> 1 & 0x1) == 1 ? 1 : 0;

                //bit2 解保
                int celv_2_2 = (jueCePanJueJieGuo2 >> 2 & 0x1) == 1 ? 1 : 0;

                //bit3 起爆
                int celv_2_3 = (jueCePanJueJieGuo2 >> 3 & 0x1) == 1 ? 1 : 0;

                //bit4 模拟解保
                int celv_2_4 = (jueCePanJueJieGuo2 >> 4 & 0x1) == 1 ? 1 : 0;

                //bit5模拟起爆
                int celv_2_5 = (jueCePanJueJieGuo2 >> 5 & 0x1) == 1 ? 1 : 0;

                int celv1 = celv10 + celv11 + celv12 + celv13 + celv14 + celv15 + celv16 + celv17 + celv_2_0 + celv_2_1;
                string celuepanjuejieguo1 = "";
                StringBuilder stringBuilder_CeLv1 = new StringBuilder();
                stringBuilder_CeLv1.Append("总飞行时间：");
                stringBuilder_CeLv1.Append(celv10.ToString());
                stringBuilder_CeLv1.Append(";");
                stringBuilder_CeLv1.Append("侧向：");
                stringBuilder_CeLv1.Append(celv11.ToString());
                stringBuilder_CeLv1.Append(";\n");
                stringBuilder_CeLv1.Append("Wx角速度：");
                stringBuilder_CeLv1.Append(celv12.ToString());
                stringBuilder_CeLv1.Append(";");
                stringBuilder_CeLv1.Append("Wy角速度：");
                stringBuilder_CeLv1.Append(celv13.ToString());
                stringBuilder_CeLv1.Append(";");
                stringBuilder_CeLv1.Append("Wz角速度：");
                stringBuilder_CeLv1.Append(celv14.ToString());
                stringBuilder_CeLv1.Append(";\n");
                stringBuilder_CeLv1.Append("后向：");
                stringBuilder_CeLv1.Append(celv15.ToString());
                stringBuilder_CeLv1.Append(";");
                stringBuilder_CeLv1.Append("坠落：");
                stringBuilder_CeLv1.Append(celv16.ToString());
                stringBuilder_CeLv1.Append(";");
                stringBuilder_CeLv1.Append("分离时间：");
                stringBuilder_CeLv1.Append(celv17.ToString());
                stringBuilder_CeLv1.Append(";\n");
                stringBuilder_CeLv1.Append("控制区上限：");
                stringBuilder_CeLv1.Append(celv_2_0.ToString());
                stringBuilder_CeLv1.Append(";");
                stringBuilder_CeLv1.Append("控制区下限：");
                stringBuilder_CeLv1.Append(celv_2_1.ToString());
                stringBuilder_CeLv1.Append(".");
                if (celv1 == 0)
                {
                    celuepanjuejieguo1 = "无异常";
                    XiTong_CeLuePanJue1_Text.Text = stringBuilder_CeLv1.ToString();
                }
                else
                {
                    celuepanjuejieguo1 = "异常";
                    XiTong_CeLuePanJue1_Text.Text = stringBuilder_CeLv1.ToString();
                }
                XiTong_CeLuePanJue1.Text = celuepanjuejieguo1;

                int celv2 = celv_2_2 + celv_2_3 + celv_2_4 + celv_2_5;
                string celuepanjuejieguo2 = "";
                StringBuilder stringBuilder_CeLv2 = new StringBuilder();
                stringBuilder_CeLv2.Append("解保：");
                stringBuilder_CeLv2.Append(celv_2_2.ToString());
                stringBuilder_CeLv2.Append(";");
                stringBuilder_CeLv2.Append("起爆：");
                stringBuilder_CeLv2.Append(celv_2_3.ToString());
                stringBuilder_CeLv2.Append(";");
                stringBuilder_CeLv2.Append("模拟解保：");
                stringBuilder_CeLv2.Append(celv_2_4.ToString());
                stringBuilder_CeLv2.Append(";");
                stringBuilder_CeLv2.Append("模拟起爆：");
                stringBuilder_CeLv2.Append(celv_2_5.ToString());
                stringBuilder_CeLv2.Append(".");
                if (celv2 == 0)
                {
                    celuepanjuejieguo2 = "无动作";
                    XiTong_CeLuePanJue2_Text.Text = stringBuilder_CeLv2.ToString();
                }
                else
                {
                    celuepanjuejieguo2 = "有动作";
                    XiTong_CeLuePanJue2_Text.Text = stringBuilder_CeLv2.ToString();

                }
                XiTong_CeLuePanJue2.Text = celuepanjuejieguo2;



                /*--------------------输入采集1--------------------------- */
                byte shuRuCaiJi1 = sObject.shuRuCaiJi1;

                //bit0 解保1时序状态
                int CJ1_jieBaoShiXu1 = (shuRuCaiJi1 >> 0 & 0x1) == 1 ? 1 : 0;

                //bit1 解保2时序状态
                int CJ1_jieBaoShiXu2 = (shuRuCaiJi1 >> 1 & 0x1) == 1 ? 1 : 0;

                //bit2 起爆控制状态
                int CJ1_qiBaoKongZhi = (shuRuCaiJi1 >> 2 & 0x1) == 1 ? 1 : 0;

                //bit3 解保控制状态
                int CJ1_jieBaoKongZhi = (shuRuCaiJi1 >> 3 & 0x1) == 1 ? 1 : 0;

                //bit4 火工品配电自保持状态
                int CJ1_huoGongPeiDian = (shuRuCaiJi1 >> 4 & 0x1) == 1 ? 1 : 0;

                //bit5 紧急断电状态
                //int CJ1_jinJiDuanDian = (shuRuCaiJi1 >> 5 & 0x1) == 1 ? 1 : 0;
                XiTong_JinJiDuanDian.Text = (shuRuCaiJi1 >> 5 & 0x1) == 1 ? "未断电" : "断电";

                //bit6 火工品BF配电指令状态
                int CJ1_huoGongBFPeiDian = (shuRuCaiJi1 >> 6 & 0x1) == 1 ? 1 : 0;

                //bit7 起爆点火指令1状态
                int CJ1_qiBaoDianHuoZhiLing1 = (shuRuCaiJi1 >> 7 & 0x1) == 1 ? 1 : 0;

                /*--------------------输入采集2--------------------------- */
                byte shuRuCaiJi2 = sObject.shuRuCaiJi2;

                //bit0 起爆点火指令2状态
                int CJ2_qiBaoDianHuoZhiLing2 = (shuRuCaiJi2 >> 0 & 0x1) == 1 ? 1 : 0;

                //bit1 解保指令1+状态
                int CJ2_jieBaoZhiLing1Jia = (shuRuCaiJi2 >> 1 & 0x1) == 1 ? 1 : 0;

                //bit2 解保指令1-状态
                int CJ2_jieBaoZhiLing1Jian = (shuRuCaiJi2 >> 2 & 0x1) == 1 ? 1 : 0;

                //bit3 解保指令2+状态
                int CJ2_jieBaoZhiLing2Jia = (shuRuCaiJi2 >> 3 & 0x1) == 1 ? 1 : 0;

                //bit4 解保指令2-状态
                int CJ2_jieBaoZhiLing2Jian = (shuRuCaiJi2 >> 4 & 0x1) == 1 ? 1 : 0;

                //bit5 解保控制指令+状态
                int CJ2_jieBaoKongZhiZhiLingJia = (shuRuCaiJi2 >> 5 & 0x1) == 1 ? 1 : 0;

                //bit6 解保控制指令-状态
                int CJ2_jieBaoKongZhiZhiLingJian = (shuRuCaiJi2 >> 6 & 0x1) == 1 ? 1 : 0;

                //bit7 起爆点火时序1，A通道状态
                int CJ2_qiBaoDianShiXu1A = (shuRuCaiJi2 >> 7 & 0x1) == 1 ? 1 : 0;

                /*--------------------输入采集3--------------------------- */
                byte shuRuCaiJi3 = sObject.shuRuCaiJi3;

                //bit0 起爆点火时序1，B通道状态
                int CJ3_qiBaoDianHuoShiXu1B = (shuRuCaiJi3 >> 0 & 0x1) == 1 ? 1 : 0;

                //bit1 起爆点火时序2，A通道状态
                int CJ3_qiBaoDianHuoShiXu2A = (shuRuCaiJi3 >> 1 & 0x1) == 1 ? 1 : 0;

                //bit2 起爆点火时序2，B通道状态
                int CJ3_qiBaoDianHuoShiXu2B = (shuRuCaiJi3 >> 2 & 0x1) == 1 ? 1 : 0;

                //bit3 起爆控制指令+状态
                int CJ3_qiBaoKongZhiJia = (shuRuCaiJi3 >> 3 & 0x1) == 1 ? 1 : 0;

                //bit4 起爆控制指令-状态
                int CJ3_qiBaoKongZhiJian = (shuRuCaiJi3 >> 4 & 0x1) == 1 ? 1 : 0;

                //bit5 触点1（起飞，0已起飞）
                //int CJ3_chuDian1 = (shuRuCaiJi3 >> 5 & 0x1) == 1 ? 1 : 0;
                //20210319(最新协议，1已起飞)
                XiTong_QiFei.Text = (shuRuCaiJi3 >> 5 & 0x1) == 0 ? "未起飞" : "已起飞";

                //bit6 触点2（预令，1有效）(最新协议更改 0有效)
                //int CJ3_chuDian2 = (shuRuCaiJi3 >> 6 & 0x1) == 1 ? 1 : 0;
                //20210312
                XiTong_YuLing.Text = (shuRuCaiJi3 >> 6 & 0x1) == 0 ? "有效" : "无效";

                //bit7 触点3（动令，1有效）(最新协议更改 0有效)
                //int CJ3_chuDian3 = (shuRuCaiJi3 >> 7 & 0x1) == 1 ? 1 : 0;
                //20210312
                XiTong_DongLing.Text = (shuRuCaiJi3 >> 7 & 0x1) == 0 ? "有效" : "无效";

                /*--------------------输入采集4--------------------------- */
                byte shuRuCaiJi4 = sObject.shuRuCaiJi4;

                //bit0 触点4（一级分离，0已分离）
                /*20210319 最新协议 1已分离*/
                //int CJ4_chuDian4 = (shuRuCaiJi4 >> 0 & 0x1) == 1 ? 1 : 0;
                XiTong_YiJiFeiLi.Text = (shuRuCaiJi4 >> 0 & 0x1) == 0 ? "未分离" : "已分离";

                //bit1 触点5
                int CJ4_chuDian5 = (shuRuCaiJi4 >> 1 & 0x1) == 1 ? 1 : 0;

                //bit2 触点6
                int CJ4_chuDian6 = (shuRuCaiJi4 >> 2 & 0x1) == 1 ? 1 : 0;

                /*触点7~10 协议更改：0有效*/
                //bit3 触点7（一级自毁工作状态A,1有效）
                int CJ4_chuDian7 = (shuRuCaiJi4 >> 3 & 0x1) == 1 ? 1 : 0;

                //bit4 触点8（一级自毁工作状态B，1有效）
                int CJ4_chuDian8 = (shuRuCaiJi4 >> 4 & 0x1) == 1 ? 1 : 0;

                //bit5 触点9（二级自毁工作状态A，1有效）
                int CJ4_chuDian9 = (shuRuCaiJi4 >> 5 & 0x1) == 1 ? 1 : 0;

                //bit6 触点10（二级自毁工作状态B,1有效）
                int CJ4_chuDian10 = (shuRuCaiJi4 >> 6 & 0x1) == 1 ? 1 : 0;

                //bit7 恒流源至火工品回路状态
                int CJ4_hengLiuYuan = (shuRuCaiJi4 >> 7 & 0x1) == 1 ? 1 : 0;

                int CJ1 = CJ1_jieBaoKongZhi + CJ2_jieBaoKongZhiZhiLingJia + CJ2_jieBaoKongZhiZhiLingJian +
                    CJ1_jieBaoShiXu1 + CJ2_jieBaoZhiLing1Jia + CJ2_jieBaoZhiLing1Jian + CJ1_jieBaoShiXu2 +
                    CJ2_jieBaoZhiLing2Jia + CJ2_jieBaoZhiLing2Jian;
                StringBuilder stringBuilder_ShuRuCaiJi1 = new StringBuilder();
                List<string> ShuRuCaiJi1Content = new List<string>();
                List<int> ShuRuCaiJi1Value = new List<int>();
                ShuRuCaiJi1Content.Add("解保控制状态：");
                ShuRuCaiJi1Content.Add("解保控制指令+：");
                ShuRuCaiJi1Content.Add("解保控制指令-：");
                ShuRuCaiJi1Content.Add("解保时序1状态：");
                ShuRuCaiJi1Content.Add("解保指令1+：");
                ShuRuCaiJi1Content.Add("解保指令1-：");
                ShuRuCaiJi1Content.Add("解保时序2状态：");
                ShuRuCaiJi1Content.Add("解保指令2+：");
                ShuRuCaiJi1Content.Add("解保指令2-：");

                ShuRuCaiJi1Value.Add(CJ1_jieBaoKongZhi);
                ShuRuCaiJi1Value.Add(CJ2_jieBaoKongZhiZhiLingJia);
                ShuRuCaiJi1Value.Add(CJ2_jieBaoKongZhiZhiLingJian);
                ShuRuCaiJi1Value.Add(CJ1_jieBaoShiXu1);
                ShuRuCaiJi1Value.Add(CJ2_jieBaoZhiLing1Jia);
                ShuRuCaiJi1Value.Add(CJ2_jieBaoZhiLing1Jian);
                ShuRuCaiJi1Value.Add(CJ1_jieBaoShiXu2);
                ShuRuCaiJi1Value.Add(CJ2_jieBaoZhiLing2Jia);
                ShuRuCaiJi1Value.Add(CJ2_jieBaoZhiLing2Jian);

                for (int i = 1; i < ShuRuCaiJi1Content.Count() + 1; i++)
                {
                    stringBuilder_ShuRuCaiJi1.Append(ShuRuCaiJi1Content[i - 1]);
                    stringBuilder_ShuRuCaiJi1.Append(ShuRuCaiJi1Value[i - 1]);
                    stringBuilder_ShuRuCaiJi1.Append(";");
                    if (i % 3 == 0)
                    {
                        stringBuilder_ShuRuCaiJi1.Append("\n");
                    }

                }

 
                if (CJ1 == 0)
                {
                    XiTong_ShuRuCaiJi1.Text = "已解保发出";
                }
                else if (CJ1 == 9)
                {
                    XiTong_ShuRuCaiJi1.Text = "未解保";
                }
                else
                {
                    XiTong_ShuRuCaiJi1.Text = "解保状态异常";               
                }
                XiTong_ShuRuCaiJi1_Text.Text = stringBuilder_ShuRuCaiJi1.ToString();


                int CJ2 = CJ1_qiBaoKongZhi + CJ3_qiBaoKongZhiJia + CJ3_qiBaoKongZhiJian +
                    CJ1_qiBaoDianHuoZhiLing1 + CJ2_qiBaoDianShiXu1A + CJ3_qiBaoDianHuoShiXu1B +
                    CJ2_qiBaoDianHuoZhiLing2 + CJ3_qiBaoDianHuoShiXu2A + CJ3_qiBaoDianHuoShiXu2B;
                StringBuilder stringBuilder_ShuRuCaiJi2 = new StringBuilder();
                List<string> ShuRuCaiJi2Content = new List<string>();
                List<int> ShuRuCaiJi2Value = new List<int>();
                ShuRuCaiJi2Content.Add("起爆控制状态：");
                ShuRuCaiJi2Content.Add("起爆控制指令+：");
                ShuRuCaiJi2Content.Add("起爆控制指令-：");
                ShuRuCaiJi2Content.Add("起爆点火指令1：");
                ShuRuCaiJi2Content.Add("起爆点火时序1A：");
                ShuRuCaiJi2Content.Add("起爆点火时序1B：");
                ShuRuCaiJi2Content.Add("起爆点火指令2：");
                ShuRuCaiJi2Content.Add("起爆点火时序2A：");
                ShuRuCaiJi2Content.Add("起爆点火时序2B：");

                ShuRuCaiJi2Value.Add(CJ1_qiBaoKongZhi);
                ShuRuCaiJi2Value.Add(CJ3_qiBaoKongZhiJia);
                ShuRuCaiJi2Value.Add(CJ3_qiBaoKongZhiJian);
                ShuRuCaiJi2Value.Add(CJ1_qiBaoDianHuoZhiLing1);
                ShuRuCaiJi2Value.Add(CJ2_qiBaoDianShiXu1A);
                ShuRuCaiJi2Value.Add(CJ3_qiBaoDianHuoShiXu1B);
                ShuRuCaiJi2Value.Add(CJ2_qiBaoDianHuoZhiLing2);
                ShuRuCaiJi2Value.Add(CJ3_qiBaoDianHuoShiXu2A);
                ShuRuCaiJi2Value.Add(CJ3_qiBaoDianHuoShiXu2B);

                for (int i = 1; i < ShuRuCaiJi2Content.Count() + 1; i++)
                {
                    stringBuilder_ShuRuCaiJi2.Append(ShuRuCaiJi2Content[i - 1]);
                    stringBuilder_ShuRuCaiJi2.Append(ShuRuCaiJi2Value[i - 1]);
                    stringBuilder_ShuRuCaiJi2.Append(";");
                    if (i % 3 == 0)
                    {
                        stringBuilder_ShuRuCaiJi2.Append("\n");
                    }
                }

                if (CJ2 == 0)
                {
                    XiTong_ShuRuCaiJi2.Text = "已起爆";
                }
                else if (CJ2 == 9)
                {
                    XiTong_ShuRuCaiJi2.Text = "未起爆";
                }
                else
                {
                    XiTong_ShuRuCaiJi2.Text = "解保状态异常";      
                }
                XiTong_ShuRuCaiJi2_Text.Text = stringBuilder_ShuRuCaiJi2.ToString();



                int CJ3 = CJ1_huoGongPeiDian + CJ1_huoGongBFPeiDian;
                StringBuilder stringBuilder_ShuRuCaiJi3 = new StringBuilder();
                stringBuilder_ShuRuCaiJi3.Append("火工品配电自保持状态：");
                stringBuilder_ShuRuCaiJi3.Append(CJ1_huoGongPeiDian.ToString());
                stringBuilder_ShuRuCaiJi3.Append(";");
                stringBuilder_ShuRuCaiJi3.Append("火工品BF配电自保持状态：");
                stringBuilder_ShuRuCaiJi3.Append(CJ1_huoGongBFPeiDian.ToString());
                stringBuilder_ShuRuCaiJi3.Append(";");


                if (CJ3 == 0)
                {
                    XiTong_ShuRuCaiJi3.Text = "已配电";
                }
                else if (CJ3 == 2)
                {
                    XiTong_ShuRuCaiJi3.Text = "未配电";
                }
                else
                {
                    XiTong_ShuRuCaiJi3.Text = "配电异常";           
                }
                XiTong_ShuRuCaiJi3_Text.Text = stringBuilder_ShuRuCaiJi3.ToString();

                int CJ4 = CJ4_chuDian7 + CJ4_chuDian8 + CJ4_chuDian9 + CJ4_chuDian10;
                StringBuilder stringBuilder_ShuRuCaiJi4 = new StringBuilder();

                /*协议更改:0有效*/
                stringBuilder_ShuRuCaiJi4.Append("一级自毁工作状态A：");
                stringBuilder_ShuRuCaiJi4.Append(CJ4_chuDian7 == 1 ? "无效" : "有效");
                stringBuilder_ShuRuCaiJi4.Append(";");
                stringBuilder_ShuRuCaiJi4.Append("一级自毁工作状态B：");
                stringBuilder_ShuRuCaiJi4.Append(CJ4_chuDian8 == 1 ? "无效" : "有效");
                stringBuilder_ShuRuCaiJi4.Append(";\n");
                stringBuilder_ShuRuCaiJi4.Append("二级自毁工作状态A：");
                stringBuilder_ShuRuCaiJi4.Append(CJ4_chuDian9 == 1 ? "无效" : "有效");
                stringBuilder_ShuRuCaiJi4.Append(";");
                stringBuilder_ShuRuCaiJi4.Append("二级自毁工作状态B：");
                stringBuilder_ShuRuCaiJi4.Append(CJ4_chuDian10 == 1 ? "无效" : "有效");
                stringBuilder_ShuRuCaiJi4.Append(".");
                if (CJ4 == 4)
                {
                    XiTong_ShuRuCaiJi4.Text = "弹体保险未解除";
                }
                else if (CJ4 == 0)
                {
                    XiTong_ShuRuCaiJi4.Text = "弹体保险解除";
                }
                else
                {
                    XiTong_ShuRuCaiJi4.Text = "弹体保险异常";     
                }
                XiTong_ShuRuCaiJi4_Text.Text = stringBuilder_ShuRuCaiJi4.ToString();

                //弹头解保
                float danTouJieBao = sObject.danTouJieBaoXinHao;
                int flag = 102;
                List<string> interval = new List<string>();
                interval.Add("[0.5,1.5]");
                interval.Add("[4.5,5.5]");
                interval.Add("[3.5,4.5]");
                interval.Add("[2.5,3.5]");
                interval.Add("[1.5,2.5]");
                Dictionary<int, string> danTou = new Dictionary<int, string>();
                danTou.Add(0, "遥测加电");
                danTou.Add(1, "初始化");
                danTou.Add(2, "一级保险解除");
                danTou.Add(3, "二级过载保险解除");
                danTou.Add(4, "三级保险解除");
                for (int i = 0; i < interval.Count(); i++)
                {
                    flag = DetermineDataInterval.InRange(interval[i], danTouJieBao);
                    if (flag == 100)
                    {
                        XiTong_DanTouJieBao.Text = danTou[i];
                        break;
                    }
                }
                if (flag == 102)
                {
                    XiTong_DanTouJieBao.Text = danTouJieBao.ToString() + "V-异常";
                }

                //起爆状态
                float qiBaoZhuangTai = sObject.qiBaoXinHao;
                int flagQiBao = 102;
                List<string> intervalQiBao = new List<string>();
                intervalQiBao.Add("[0.5,1.5]");
                intervalQiBao.Add("[4.5,5.5]");
                intervalQiBao.Add("[2.5,3.5]");
                intervalQiBao.Add("[1.5,2.5]");
                Dictionary<int, string> qiBao = new Dictionary<int, string>();
                qiBao.Add(0, "遥测加电");
                qiBao.Add(1, "保险控制信号");
                qiBao.Add(2, "高压充电完成");
                qiBao.Add(3, "起爆");
                for (int i = 0; i < qiBao.Count(); i++)
                {
                    flagQiBao = DetermineDataInterval.InRange(intervalQiBao[i], qiBaoZhuangTai);
                    if (flagQiBao == 100)
                    {
                        XiTong_QiBaoZhuangTai.Text = qiBao[i];
                        break;
                    }
                }

                if (flag == 102)
                {
                    XiTong_QiBaoZhuangTai.Text = danTouJieBao.ToString() + "V-异常";
                }

                //内部控制电压
                if (DetermineDataInterval.InRange("[20,35]", sObject.neiBuKongZhiDianYa) == 100)
                {
                    XiTong_NeiBuKongZhiDian.Text = sObject.neiBuKongZhiDianYa.ToString();
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 0, 255, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_NeiBuKongZhiDian.Background = brushes;
                }
                else
                {
                    XiTong_NeiBuKongZhiDian.Text = sObject.neiBuKongZhiDianYa.ToString();
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 0, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_NeiBuKongZhiDian.Background = brushes;
                }

                //功率电电压
                // //20210312
                if (DetermineDataInterval.InRange("[20,35]", sObject.gongLvDianDianYa) == 100)
                {

                    XiTong_GongLvDian.Text = sObject.gongLvDianDianYa.ToString();
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 0, 255, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_GongLvDian.Background = brushes;
                }
                else
                {
                    XiTong_GongLvDian.Text = sObject.gongLvDianDianYa.ToString();
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 0, 0);

                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_GongLvDian.Background = brushes;
                }

                // 导航状态指示1
                byte daoHangTip1 = sObject.daoHangTip1;

                // 导航数据选择                        // 
                XiTong_DaoHangShuJuXuanZe.Text = (daoHangTip1 & 0x1) == 0 ? "数据不可用" : "数据可用";

                // 陀螺数据融合结果（0：所有数据不可用 1：数据可用） 
                XiTong_TuoLuoShuJuRongHe.Text = ((daoHangTip1 >> 1) & 0x1) == 0 ? "所有数据不可用" : "数据可用";

                // bit2 bit3 数据未更新标志（00：均无数据; // 01：1号输入无数据，2号输入有数据; // 10：1号输入有数据，2号输入无数据; // 11：均有数据）
                byte tempValue = (byte)((daoHangTip1 >> 2) & 0x3);
                string tempSTR = "";
                switch (tempValue)
                {
                    case 0:
                        tempSTR = "均无数据";
                        break;
                    case 1:
                        // tempSTR = "1号输入无数据，2号输入有数据";  
                        tempSTR = "1号无数据，2号有数据";
                        break;
                    case 2:
                        // tempSTR = "1号输入有数据，2号输入无数据";
                        tempSTR = "1号有数据，2号无数据";
                        break;
                    case 3:
                        tempSTR = "均有数据";
                        break;
                    default:
                        break;
                }
                XiTong_ShuJuWeiGengXin.Text = tempSTR;

                //bit4 bit5 优选结果（00：优选结果无输出;   01：优选结果为1号, 10：优选结果为2号, 11:优选结果为1号和2号）
                //0x3 0011
                tempValue = (byte)((daoHangTip1 >> 4) & 0x3);
                tempSTR = "";
                switch (tempValue)
                {
                    case 0:
                        tempSTR = "优选结果无输出";
                        break;
                    case 1:
                        // tempSTR = "1号异常，2号正常"; 
                        tempSTR = "优选结果为1号";
                        break;
                    case 2:
                        // tempSTR = "1号正常，2号异常"; 
                        tempSTR = "优选结果为2号";
                        break;
                    case 3:
                        // tempSTR = "时间间隔均不正常"; 
                        tempSTR = "优选结果为1号和2号";
                        break;
                    default:
                        break;
                }
                XiTong_YouXuanJieGuo.Text = tempSTR;

                // bit6 弹头组合无效标志（1表示无效）
                XiTong_DanTouZuHe.Text = (daoHangTip1 >> 6 & 0x1) == 1 ? "无效" : "有效";

                // bit7 弹体组合无效标志（1表示无效）
                XiTong_DanTiZuHe.Text = (daoHangTip1 >> 7 & 0x1) == 1 ? "无效" : "有效";

                // -------------------导航状态指示2
                byte daoHangTip2 = sObject.daoHangTip2;
                List<string> data1Text = new List<string>(); //1号数据集合
                List<string> data2Text = new List<string>(); //2号数据集合
                Dictionary<byte, string> dicTip = new Dictionary<byte, string>();
                dicTip.Add(0, "不是野值");
                dicTip.Add(1, "无数据");
                dicTip.Add(2, "数据用于初始化");
                dicTip.Add(3, "是野值");

                // bit0 bit1 1号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data1Text.Add(dicTip[(byte)(daoHangTip2 & 0x03)]);

                // bit2 bit3 1号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data1Text.Add(dicTip[(byte)(daoHangTip2 >> 2 & 0x03)]);

                // bit4 bit5 1号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data1Text.Add(dicTip[(byte)(daoHangTip2 >> 4 & 0x03)]);

                // bit6 bit7 1号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data1Text.Add(dicTip[(byte)(daoHangTip2 >> 6 & 0x03)]);

                // ---------------------导航状态指示3
                byte daoHangTip3 = sObject.daoHangTip3;

                // bit0 bit1 1号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data1Text.Add(dicTip[(byte)(daoHangTip3 & 0x03)]);

                // bit2 bit3 1号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data1Text.Add(dicTip[(byte)(daoHangTip3 >> 2 & 0x03)]);

                // bit4 bit5 2号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data2Text.Add(dicTip[(byte)(daoHangTip3 >> 4 & 0x03)]);

                // bit6 bit7 2号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data2Text.Add(dicTip[(byte)(daoHangTip3 >> 6 & 0x03)]);

                // -------------------------导航状态指示4
                byte daoHangTip4 = sObject.daoHangTip4;

                // bit0 bit1 2号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data2Text.Add(dicTip[(byte)(daoHangTip4 & 0x03)]);

                // bit2 bit3 2号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data2Text.Add(dicTip[(byte)(daoHangTip4 >> 2 & 0x03)]);

                // bit4 bit5 2号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data2Text.Add(dicTip[(byte)(daoHangTip4 >> 4 & 0x03)]);

                // bit6 bit7 2号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                data2Text.Add(dicTip[(byte)(daoHangTip4 >> 6 & 0x03)]);

                StringBuilder stringBuilder_1Hao = new StringBuilder();
                List<string> No1 = new List<string>();
                No1.Add("1号数据经度:");
                No1.Add("1号数据纬度:");
                No1.Add("1号数据高度:");
                No1.Add("1号数据东向速度:");
                No1.Add("1号数据北向速度:");
                No1.Add("1号数据天向速度:");

                StringBuilder stringBuilder_2Hao = new StringBuilder();
                List<string> No2 = new List<string>();
                No2.Add("2号数据经度:");
                No2.Add("2号数据纬度:");
                No2.Add("2号数据高度:");
                No2.Add("2号数据东向速度:");
                No2.Add("2号数据北向速度:");
                No2.Add("2号数据天向速度:");

                for (int j = 0; j < No1.Count(); j++)
                {
                    stringBuilder_1Hao.Append(No1[j]);
                    stringBuilder_1Hao.Append(data1Text[j]);
                    stringBuilder_1Hao.Append(";");
                    if ((j + 1) % 3 == 0)
                    {
                        stringBuilder_1Hao.Append("\n");
                    }
                }

                for (int j = 0; j < No2.Count(); j++)
                {
                    stringBuilder_2Hao.Append(No2[j]);
                    stringBuilder_2Hao.Append(data2Text[j]);
                    stringBuilder_2Hao.Append(";");
                    if ((j + 1) % 3 == 0)
                    {
                        stringBuilder_2Hao.Append("\n");
                    }
                }


                int flagData1Green = 0;
                int flagData1Red = 0;
                for (int i = 0; i < data1Text.Count(); i++)
                {
                    if (data1Text[i] == "不是野值")
                    {
                        flagData1Green++;
                    }
                    if (data1Text[i] == "是野值")
                    {
                        flagData1Red++;
                    }
                }
                if (flagData1Green == 6) //全为0时显示为绿色
                {
                    XiTong_ShuJu1Hao.Text = "正常";
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 0, 255, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_ShuJu1Hao.Background = brushes;
                }
                else if (flagData1Red == 6) //全为1时显示为红色
                {
                    XiTong_ShuJu1Hao.Text = "错误";
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 0, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_ShuJu1Hao.Background = brushes;
                }
                else //其他值为黄色
                {
                    XiTong_ShuJu1Hao.Text = "警告";
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 0, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_ShuJu1Hao.Background = brushes; 
                }
                XiTong_ShuJu1Hao_Text.Text = stringBuilder_1Hao.ToString();

                int flagData2Green = 0;
                int flagData2Red = 0;
                for (int i = 0; i < data2Text.Count(); i++)
                {
                    if (data2Text[i] == "不是野值")
                    {
                        flagData2Green++;
                    }
                    if (data2Text[i] == "是野值")
                    {
                        flagData2Red++;
                    }
                }
                if (flagData2Green == 6)  //全为0时显示为绿色
                {
                    XiTong_ShuJu2Hao.Text = "正常";
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 0, 255, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_ShuJu2Hao.Background = brushes;
                }
                else if (flagData2Red == 6)  //全为1时显示为红色
                {
                    XiTong_ShuJu2Hao.Text = "错误";
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 0, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_ShuJu2Hao.Background = brushes;
                }
                else  //其他值为黄色
                {
                    XiTong_ShuJu2Hao.Text = "警告";
                    System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(30, 255, 255, 0);
                    System.Windows.Media.SolidColorBrush solidColorBrush = new System.Windows.Media.SolidColorBrush(color);
                    System.Windows.Media.Brush brushes = solidColorBrush;
                    XiTong_ShuJu2Hao.Background = brushes;
                }
                XiTong_ShuJu2Hao_Text.Text = stringBuilder_2Hao.ToString();

                // 
                // 预示落点Z
                // 
                XiTong_YuShiLuoDianZ.Text = sObject.yuShiLuoDianZ.ToString(); //
                                                                              // 
                                                                              // 预示落点射程
                                                                              // 
                XiTong_YuShiLuoDianSheCheng.Text = sObject.yuShiLuoDianSheCheng.ToString(); //
                                                                                            // 
                                                                                            // 轴向过载
                                                                                            // 
                XiTong_ZhouXiangBiLi.Text = sObject.zhouXiangGuoZai.ToString();

            }
            else
            {
                StringBuilder s = new StringBuilder(); 

                /* 修改时间：2021年4月15日19:18:44
                 * 修改说明：经度纬度的位置反了，应该是先纬度在经度
                 */
                // 纬度
                s.Append(((double)(sObject.weiDu * Math.Pow(10, -7))).ToString());
                s.Append(",");

                // 经度
                s.Append(((double)(sObject.jingDu * Math.Pow(10, -7))).ToString());
                s.Append(",");

                // 海拔高度
                s.Append(((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString());
                s.Append(",");
                // 东向速度
                s.Append(((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");
                // 北向速度
                s.Append(((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");
                // 天向速度
                s.Append(((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");
                // Wx角速度
                s.Append(sObject.WxJiaoSuDu.ToString());
                s.Append(",");
                // Wy角速度
                s.Append(sObject.WyJiaoSuDu.ToString());
                s.Append(",");
                // Wz角速度
                s.Append(sObject.WzJiaoSuDu.ToString());
                s.Append(",");

                // 轴向过载
                s.Append(sObject.zhouXiangGuoZai.ToString());
                s.Append(",");
                // GNSS时间
                s.Append(((double)(sObject.GNSSTime * Math.Pow(10, -3))).ToString());
                s.Append(",");
                // 当前发射系X
                s.Append(sObject.curFaSheXi_X.ToString());
                s.Append(",");
                // 当前发射系Y
                s.Append(sObject.curFaSheXi_Y.ToString());
                s.Append(",");
                // 当前发射系Z
                s.Append(sObject.curFaSheXi_Z.ToString());
                s.Append(",");

                // 预示落点射程
                s.Append(sObject.yuShiLuoDianSheCheng.ToString());
                s.Append(",");
                // 预示落点Z
                s.Append(sObject.yuShiLuoDianZ.ToString());
                s.Append(",");

                // 飞行总时间
                s.Append(sObject.feiXingZongShiJian.ToString());
                s.Append(",");
                //参试状态 0x00:无意义，0x01:正式实验，0x10：测试1，数据输出状态
                string canShiZhuangTai = "";
                switch (sObject.canShiZhuangTai)
                {
                    case 0:
                        canShiZhuangTai = "无意义";
                        break;

                    case 1:
                        canShiZhuangTai = "正式试验";
                        break;

                    case 2:
                        canShiZhuangTai = "测试1,数据输出状态";
                        break;

                    default:
                        canShiZhuangTai = "错误数据";
                        break;
                }
                s.Append(canShiZhuangTai);
                s.Append(",");
                // 策略阶段(0-准备 1-起飞 2-一级 3-二级 4-结束)
                string ceLueJieDuanValue = "";
                switch (sObject.ceLueJieDuan)
                {
                    case 0:
                        ceLueJieDuanValue = "准备";
                        break;
                    case 1:
                        ceLueJieDuanValue = "起飞";
                        break;
                    case 2:
                        ceLueJieDuanValue = "一级";
                        break;
                    case 3:
                        ceLueJieDuanValue = "二级";
                        break;
                    case 4:
                        ceLueJieDuanValue = "结束";
                        break;
                    default:
                        break;
                }
                s.Append(ceLueJieDuanValue);
                s.Append(",");
                // 
                // 策略判决结果1
                // 
                byte jueCePanJueJieGuo1 = sObject.jueCePanJueJieGuo1;

                // bit0 总飞行时间（1：有效                                                     
                s.Append((jueCePanJueJieGuo1 >> 0 & 0x1) == 1 ? 1 : 0); //
                s.Append(",");
                // bit1 侧向（1：有效）
                //XiTong_CeXiang.Text = (jueCePanJueJieGuo1 >> 1 & 0x1) == 1 ? "有效" : "无效";
                s.Append((jueCePanJueJieGuo1 >> 1 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // bit2 Wx角速度（1：有效）
                //XiTong_WxJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 2 & 0x1) == 1 ? "有效" : "无效";
                s.Append((jueCePanJueJieGuo1 >> 2 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // bit3 Wy角速度（1：有效）
                //XiTong_WyJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 3 & 0x1) == 1 ? "有效" : "无效";
                s.Append((jueCePanJueJieGuo1 >> 3 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // bit4 Wz角速度（1：有效）
                //XiTong_WzJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 4 & 0x1) == 1 ? "有效" : "无效"; //
                s.Append((jueCePanJueJieGuo1 >> 4 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // bit5 后向（1：有效）
                //XiTong_HouXiang.Text = (jueCePanJueJieGuo1 >> 5 & 0x1) == 1 ? "有效" : "无效"; //
                s.Append((jueCePanJueJieGuo1 >> 5 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // bit6 坠落（1：有效）
                //XiTong_ZhuiLuo.Text = (jueCePanJueJieGuo1 >> 6 & 0x1) == 1 ? "有效" : "无效"; //
                s.Append((jueCePanJueJieGuo1 >> 6 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // bit7 分离时间（1：有效） 
                //XiTong_FenLiShiTian.Text = (jueCePanJueJieGuo1 >> 7 & 0x1) == 1 ? "有效" : "无效"; //
                s.Append((jueCePanJueJieGuo1 >> 7 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // 策略判决结果2
                //
                byte jueCePanJueJieGuo2 = sObject.jueCePanJueJieGuo2;

                // bit0 控制区下限（1：有效）                                                        
                s.Append((jueCePanJueJieGuo2 >> 0 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                // bit1 控制区上限（1：有效）
                //XiTong_KongZhiQuShangXian.Text = (jueCePanJueJieGuo2 >> 1 & 0x1) == 1 ? "有效" : "无效"; //
                s.Append((jueCePanJueJieGuo2 >> 1 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit2 解保
                s.Append((jueCePanJueJieGuo2 >> 2 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit3 起爆
                s.Append((jueCePanJueJieGuo2 >> 3 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit4 模拟解保
                s.Append((jueCePanJueJieGuo2 >> 4 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit5模拟起爆
                s.Append((jueCePanJueJieGuo2 >> 5 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                /*--------------------输入采集1--------------------------- */
                byte shuRuCaiJi1 = sObject.shuRuCaiJi1;

                //bit0 解保1时序状态
                s.Append((shuRuCaiJi1 >> 0 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit1 解保2时序状态
                s.Append((shuRuCaiJi1 >> 1 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit2 起爆控制状态
                s.Append((shuRuCaiJi1 >> 2 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit3 解保控制状态
                s.Append((shuRuCaiJi1 >> 3 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit4 火工品配电自保持状态
                s.Append((shuRuCaiJi1 >> 4 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit5 紧急断电状态
                //int CJ1_jinJiDuanDian = (shuRuCaiJi1 >> 5 & 0x1) == 1 ? 1 : 0;
                s.Append((shuRuCaiJi1 >> 5 & 0x1) == 1 ? "未断电" : "断电");
                s.Append(",");
                //bit6 火工品BF配电指令状态
                s.Append((shuRuCaiJi1 >> 6 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit7 起爆点火指令1状态
                s.Append((shuRuCaiJi1 >> 7 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                /*--------------------输入采集2--------------------------- */
                byte shuRuCaiJi2 = sObject.shuRuCaiJi2;

                //bit0 起爆点火指令2状态
                s.Append((shuRuCaiJi2 >> 0 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit1 解保指令1+状态
                s.Append((shuRuCaiJi2 >> 1 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit2 解保指令1-状态
                s.Append((shuRuCaiJi2 >> 2 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit3 解保指令2+状态
                s.Append((shuRuCaiJi2 >> 3 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit4 解保指令2-状态
                s.Append((shuRuCaiJi2 >> 4 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit5 解保控制指令+状态
                s.Append((shuRuCaiJi2 >> 5 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit6 解保控制指令-状态
                s.Append((shuRuCaiJi2 >> 6 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit7 起爆点火时序1，A通道状态
                s.Append((shuRuCaiJi2 >> 7 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                /*--------------------输入采集3--------------------------- */
                byte shuRuCaiJi3 = sObject.shuRuCaiJi3;
                //bit0 起爆点火时序1，B通道状态
                s.Append((shuRuCaiJi3 >> 0 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit1 起爆点火时序2，A通道状态
                s.Append((shuRuCaiJi3 >> 1 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit2 起爆点火时序2，B通道状态
                s.Append((shuRuCaiJi3 >> 2 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit3 起爆控制指令+状态
                s.Append((shuRuCaiJi3 >> 3 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit4 起爆控制指令-状态
                s.Append((shuRuCaiJi3 >> 4 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit5 触点1（起飞，0已起飞）
                //int CJ3_chuDian1 = (shuRuCaiJi3 >> 5 & 0x1) == 1 ? 1 : 0;
                //20210319(最新协议，1已起飞)
                s.Append((shuRuCaiJi3 >> 5 & 0x1) == 0 ? "未起飞" : "已起飞");
                s.Append(",");
                //bit6 触点2（预令，1有效）(最新协议更改 0有效)
                //int CJ3_chuDian2 = (shuRuCaiJi3 >> 6 & 0x1) == 1 ? 1 : 0;
                //20210312
                s.Append((shuRuCaiJi3 >> 6 & 0x1) == 0 ? "有效" : "无效");
                s.Append(",");
                //bit7 触点3（动令，1有效）(最新协议更改 0有效)
                //int CJ3_chuDian3 = (shuRuCaiJi3 >> 7 & 0x1) == 1 ? 1 : 0;
                //20210312
                s.Append((shuRuCaiJi3 >> 7 & 0x1) == 0 ? "有效" : "无效");
                s.Append(",");
                /*--------------------输入采集4--------------------------- */
                byte shuRuCaiJi4 = sObject.shuRuCaiJi4;

                //bit0 触点4（一级分离，0已分离）
                /*20210319 最新协议 1已分离*/
                //int CJ4_chuDian4 = (shuRuCaiJi4 >> 0 & 0x1) == 1 ? 1 : 0;
                s.Append((shuRuCaiJi4 >> 0 & 0x1) == 0 ? "未分离" : "已分离");
                s.Append(",");
                //bit1 触点5
                s.Append((shuRuCaiJi4 >> 1 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit2 触点6
                s.Append((shuRuCaiJi4 >> 2 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                /*触点7~10 协议更改：0有效*/
                //bit3 触点7（一级自毁工作状态A,1有效）
                s.Append((shuRuCaiJi4 >> 3 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit4 触点8（一级自毁工作状态B，1有效）
                s.Append((shuRuCaiJi4 >> 4 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit5 触点9（二级自毁工作状态A，1有效）
                s.Append((shuRuCaiJi4 >> 5 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit6 触点10（二级自毁工作状态B,1有效）
                s.Append((shuRuCaiJi4 >> 6 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //bit7 恒流源至火工品回路状态
                s.Append((shuRuCaiJi4 >> 7 & 0x1) == 1 ? 1 : 0);
                s.Append(",");
                //弹头解保
                float danTouJieBao = sObject.danTouJieBaoXinHao;
                int flag = 102;
                List<string> interval = new List<string>();
                interval.Add("[0.5,1.5]");
                interval.Add("[4.5,5.5]");
                interval.Add("[3.5,4.5]");
                interval.Add("[2.5,3.5]");
                interval.Add("[1.5,2.5]");
                Dictionary<int, string> danTou = new Dictionary<int, string>();
                danTou.Add(0, "遥测加电");
                danTou.Add(1, "初始化");
                danTou.Add(2, "一级保险解除");
                danTou.Add(3, "二级过载保险解除");
                danTou.Add(4, "三级保险解除");
                for (int i = 0; i < interval.Count(); i++)
                {
                    flag = DetermineDataInterval.InRange(interval[i], danTouJieBao);
                    if (flag == 100)
                    {
                        s.Append(danTou[i]);
                        s.Append(",");
                        break;
                    }
                }
                if (flag == 102)
                {
                    s.Append(danTouJieBao.ToString() + "V-异常");
                    s.Append(",");
                }

                //起爆状态
                float qiBaoZhuangTai = sObject.qiBaoXinHao;
                int flagQiBao = 102;
                List<string> intervalQiBao = new List<string>();
                intervalQiBao.Add("[0.5,1.5]");
                intervalQiBao.Add("[4.5,5.5]");
                intervalQiBao.Add("[2.5,3.5]");
                intervalQiBao.Add("[1.5,2.5]");
                Dictionary<int, string> qiBao = new Dictionary<int, string>();
                qiBao.Add(0, "遥测加电");
                qiBao.Add(1, "保险控制信号");
                qiBao.Add(2, "高压充电完成");
                qiBao.Add(3, "起爆");
                for (int i = 0; i < qiBao.Count(); i++)
                {
                    flagQiBao = DetermineDataInterval.InRange(intervalQiBao[i], qiBaoZhuangTai);
                    if (flagQiBao == 100)
                    {
                        s.Append(qiBao[i]);
                        s.Append(",");
                        break;
                    }
                }

                if (flag == 102)
                {
                    s.Append(danTouJieBao.ToString() + "V-异常");
                    s.Append(",");
                }


                s.Append(sObject.neiBuKongZhiDianYa.ToString());
                s.Append(",");

                s.Append(sObject.gongLvDianDianYa.ToString());
                s.Append(",");
                // 导航状态指示1
                byte daoHangTip1 = sObject.daoHangTip1;

                // 导航数据选择                        // 
                s.Append((daoHangTip1 & 0x1) == 0 ? "数据不可用" : "数据可用");
                s.Append(",");
                // 陀螺数据融合结果（0：所有数据不可用 1：数据可用） 
                s.Append(((daoHangTip1 >> 1) & 0x1) == 0 ? "所有数据不可用" : "数据可用");
                s.Append(",");
                // bit2 bit3 数据未更新标志（00：均无数据; // 01：1号输入无数据，2号输入有数据; // 10：1号输入有数据，2号输入无数据; // 11：均有数据）
                byte tempValue = (byte)((daoHangTip1 >> 2) & 0x3);
                string tempSTR = "";
                switch (tempValue)
                {
                    case 0:
                        tempSTR = "均无数据";
                        break;
                    case 1:
                        // tempSTR = "1号输入无数据，2号输入有数据";  
                        tempSTR = "1号无数据，2号有数据";
                        break;
                    case 2:
                        // tempSTR = "1号输入有数据，2号输入无数据";
                        tempSTR = "1号有数据，2号无数据";
                        break;
                    case 3:
                        tempSTR = "均有数据";
                        break;
                    default:
                        break;
                }
                s.Append(tempSTR);
                s.Append(",");
                //bit4 bit5 优选结果（00：优选结果无输出;   01：优选结果为1号, 10：优选结果为2号, 11:优选结果为1号和2号）
                //0x3 0011
                tempValue = (byte)((daoHangTip1 >> 4) & 0x3);
                tempSTR = "";
                switch (tempValue)
                {
                    case 0:
                        tempSTR = "优选结果无输出";
                        break;
                    case 1:
                        // tempSTR = "1号异常，2号正常"; 
                        tempSTR = "优选结果为1号";
                        break;
                    case 2:
                        // tempSTR = "1号正常，2号异常"; 
                        tempSTR = "优选结果为2号";
                        break;
                    case 3:
                        // tempSTR = "时间间隔均不正常"; 
                        tempSTR = "优选结果为1号和2号";
                        break;
                    default:
                        break;
                }
                s.Append(tempSTR);
                s.Append(",");
                // bit6 弹头组合无效标志（1表示无效）
                s.Append((daoHangTip1 >> 6 & 0x1) == 1 ? "无效" : "有效");
                s.Append(",");
                // bit7 弹体组合无效标志（1表示无效）
                s.Append((daoHangTip1 >> 7 & 0x1) == 1 ? "无效" : "有效");
                s.Append(",");
                // -------------------导航状态指示2
                byte daoHangTip2 = sObject.daoHangTip2;
                Dictionary<byte, string> dicTip = new Dictionary<byte, string>();
                dicTip.Add(0, "不是野值");
                dicTip.Add(1, "无数据");
                dicTip.Add(2, "数据用于初始化");
                dicTip.Add(3, "是野值");

                // bit0 bit1 1号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip2 & 0x03)]);
                s.Append(",");
                // bit2 bit3 1号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip2 >> 2 & 0x03)]);
                s.Append(",");
                // bit4 bit5 1号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip2 >> 4 & 0x03)]);
                s.Append(",");
                // bit6 bit7 1号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip2 >> 6 & 0x03)]);
                s.Append(",");
                // ---------------------导航状态指示3
                byte daoHangTip3 = sObject.daoHangTip3;

                // bit0 bit1 1号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip3 & 0x03)]);
                s.Append(",");
                // bit2 bit3 1号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip3 >> 2 & 0x03)]);
                s.Append(",");
                // bit4 bit5 2号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip3 >> 4 & 0x03)]);
                s.Append(",");
                // bit6 bit7 2号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip3 >> 6 & 0x03)]);
                s.Append(",");
                // -------------------------导航状态指示4
                byte daoHangTip4 = sObject.daoHangTip4;

                // bit0 bit1 2号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip4 & 0x03)]);
                s.Append(",");
                // bit2 bit3 2号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip4 >> 2 & 0x03)]);
                s.Append(",");
                // bit4 bit5 2号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip4 >> 4 & 0x03)]);
                s.Append(",");
                // bit6 bit7 2号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
                s.Append(dicTip[(byte)(daoHangTip4 >> 6 & 0x03)]);
                s.Append(",");

                //WriteAKDataCSVFile(s.ToString());
                stringBuilders.Add(s);
            }
        }
#endif

        //系统快速弹体数据显示
        private void showDHKuaiSuTimeStatus_Ti(ref DAOHANGSHUJU_KuaiSu sObject)
        // 
        {
            if (!dataConversion)
            {
                // 
                // 导航系统时间
                // 
                DHKuaiSu_Ti_DaoHangXiTongShiJian.Text = ((double)(sObject.daoHangXiTongShiJian * 0.005)).ToString(); //
                                                                                                                     // 

                // 
                // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
                // 
                DHKuaiSu_Ti_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString(); //
                                                                                                    // 
                                                                                                    // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
                                                                                                    // 
                DHKuaiSu_Ti_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString(); //
                                                                                                  // 
                                                                                                  // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                                                                                                  // 
                DHKuaiSu_Ti_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString(); //
                                                                                                       // 

                // 
                //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
                // 
                DHKuaiSu_Ti_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                                  // 
                                                                                                                  //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                                                                                                                  // 
                DHKuaiSu_Ti_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                                // 
                                                                                                                //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                                                                                                                // 
                DHKuaiSu_Ti_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                                  // 

                // 
                // GNSS时间 单位s,UTC秒部
                // 
                DHKuaiSu_Ti_GNSSTime.Text = ((double)(sObject.GNSSTime * Math.Pow(10, -3))).ToString();//
                                                                                                       // 
                                                                                                       // 俯仰角
                                                                                                       // 
                DHKuaiSu_Ti_FuYangJiao.Text = sObject.fuYangJiao.ToString(); //
                                                                             // 
                                                                             // 滚转角
                                                                             // 
                DHKuaiSu_Ti_GunZhuanJiao.Text = sObject.gunZhuanJiao.ToString(); //
                                                                                 // 
                                                                                 // 偏航角
                                                                                 // 
                DHKuaiSu_Ti_PianHangJiao.Text = sObject.pianHangJiao.ToString(); //
                                                                                 // 

                // 
                // 陀螺X数据
                // 
                DHKuaiSu_Ti_TuoLuoXShuJu.Text = sObject.tuoLuoShuJu_X.ToString(); //
                                                                                  // 
                                                                                  // 陀螺Y数据
                                                                                  // 
                DHKuaiSu_Ti_TuoLuoYShuJu.Text = sObject.tuoLuoShuJu_Y.ToString(); //
                                                                                  // 
                                                                                  // 陀螺Z数据
                                                                                  // 
                DHKuaiSu_Ti_TuoLuoZShuJu.Text = sObject.tuoLuoShuJu_Z.ToString(); //
                                                                                  // 

                // 
                // 加速度计X数据
                // 
                DHKuaiSu_Ti_JiaSuDuJiX.Text = sObject.jiaSuDuJiShuJu_X.ToString(); //
                                                                                   // 
                                                                                   // 加速度计Y数据
                                                                                   // 
                DHKuaiSu_Ti_JiaSuDuJiY.Text = sObject.jiaSuDuJiShuJu_Y.ToString(); //
                                                                                   // 
                                                                                   // 加速度计Z数据
                                                                                   // 
                DHKuaiSu_Ti_JiaSuDuJiZ.Text = sObject.jiaSuDuJiShuJu_Z.ToString(); //

                // 
                // 状态标志位
                // 
                byte zhuangTaiBiaoZhiWei = sObject.zhuangTaiBiaoZhiWei; //
                                                                        // 
                                                                        // bit0 点火标志（0：未点火 1：已点火）
                                                                        // 
                DHKuaiSu_Ti_DianHuo.Text = (zhuangTaiBiaoZhiWei >> 0 & 0x1) == 1 ? "已点火" : "未点火"; //
                                                                                                  // 
                                                                                                  // bit1 分离标志（0：已分离 1：未分离）
                                                                                                  // 
                DHKuaiSu_Ti_FenLi.Text = (zhuangTaiBiaoZhiWei >> 1 & 0x1) == 1 ? "未分离" : "已分离"; //
                                                                                                // 
                                                                                                // bit2 bit3 00:准备阶段 01：对准阶段 10：导航阶段
                                                                                                // 
                byte tempValue = (byte)(zhuangTaiBiaoZhiWei >> 2 & 0x3); //
                                                                         // 
                string tempSTR = ""; //
                                     // 
                switch (tempValue)
                // 
                {
                    // 
                    case 0:
                        // 
                        tempSTR = "准备阶段"; //
                                          // 
                        break; //
                               // 
                    case 1:
                        // 
                        tempSTR = "对准阶段"; //
                                          // 
                        break; //
                               // 
                    case 2:
                        // 
                        tempSTR = "导航阶段"; //
                                          // 
                        break; //
                               // 
                    default:
                        // 
                        break; //
                               // 
                }
                // 
                DHKuaiSu_Ti_GongZuoJieDuan.Text = tempSTR; //
                                                           // 
                                                           // bit4 bit5 00:GPS无更新 01：GPS有更新 10：GPS更新过
                                                           // 
                tempValue = (byte)(zhuangTaiBiaoZhiWei >> 4 & 0x3); //
                                                                    // 
                tempSTR = ""; //
                              // 
                switch (tempValue)
                // 
                {
                    // 
                    case 0:
                        // 
                        tempSTR = "GPS无更新"; //
                                            // 
                        break; //
                               // 
                    case 1:
                        // 
                        tempSTR = "GPS有更新"; //
                                            // 
                        break; //
                               // 
                    case 2:
                        // 
                        tempSTR = "GPS更新过"; //
                                            // 
                        break; //
                               // 
                    default:
                        // 
                        break; //
                               // 
                }
                // 
                DHKuaiSu_Ti_GPSShuJuGengXin.Text = tempSTR; //
                                                            // 
                                                            // GPS组合标志 (00：上5ms惯导，本5ms惯导; // 01：上5ms惯导，本5ms组合; // 10：上5ms组合，本5ms组合; // 11：上5ms组合，本5ms惯导; //)
                                                            // 
                tempValue = (byte)(zhuangTaiBiaoZhiWei >> 6 & 0x3); //
                                                                    // 
                tempSTR = ""; //
                              // 
                switch (tempValue)
                // 
                {
                    // 
                    case 0:
                        // 
                        tempSTR = "上5ms惯导，本5ms惯导"; //
                                                   // 
                        break; //
                               // 
                    case 1:
                        // 
                        tempSTR = "上5ms惯导，本5ms组合"; //
                                                   // 
                        break; //
                               // 
                    case 2:
                        // 
                        tempSTR = "上5ms组合，本5ms组合"; //
                                                   // 
                        break; //
                               // 
                    case 3:
                        // 
                        tempSTR = "上5ms组合，本5ms惯导"; //
                                                   // 
                        break; //
                               // 
                    default:
                        // 
                        break; //
                               // 
                }
                // 
                DHKuaiSu_Ti_GPSZuHe.Text = tempSTR; //
                                                    // 

                // 
                // 陀螺故障标志
                //
                byte tuoLuoGuZhangBiaoZhi = sObject.tuoLuoGuZhangBiaoZhi;

                //bit0 导航组合完成标识（0：未完成,1：已组合）
                DHKuaiSu_Ti_DaoHangZuHe.Text = (tuoLuoGuZhangBiaoZhi >> 0 & 0x1) == 0 ? "未完成" : "已组合";

                //bit1 加计x故障标识（0：正常，1：故障）
                int JiaJiXGuZhang = (tuoLuoGuZhangBiaoZhi >> 1 & 0x1) == 0 ? 0 : 1;

                //bit2 加计Y故障标识（0：正常，1：故障）
                int JiaJiYGuZhang = (tuoLuoGuZhangBiaoZhi >> 2 & 0x1) == 0 ? 0 : 1;

                //bit3 加计z故障标识（0：正常，1：故障）
                int JiaJiZGuZhang = (tuoLuoGuZhangBiaoZhi >> 3 & 0x1) == 0 ? 0 : 1;


                // bit5 陀螺x故障标志（0：正常）
                int TuoLuoXGuZhang = (tuoLuoGuZhangBiaoZhi >> 5 & 0x1) == 0 ? 0 : 1;

                // bit6 陀螺y故障标志（0：正常）
                int TuoLuoYGuZhang = (tuoLuoGuZhangBiaoZhi >> 6 & 0x1) == 0 ? 0 : 1;

                // bit7 陀螺z故障标志（0：正常）                                                         
                int TuoLuoZGuZhang = (tuoLuoGuZhangBiaoZhi >> 7 & 0x1) == 0 ? 0 : 1;

                int JiaJi = JiaJiXGuZhang + JiaJiYGuZhang + JiaJiZGuZhang;
                int TuoLuo = TuoLuoXGuZhang + TuoLuoYGuZhang + TuoLuoZGuZhang;
                StringBuilder stringBuilder_JiaJi = new StringBuilder();
                StringBuilder stringBuilder_TuoLuo = new StringBuilder();

                stringBuilder_JiaJi.Append("加计X故障：");
                stringBuilder_JiaJi.Append(JiaJiXGuZhang == 0 ? "正常" : "故障");
                stringBuilder_JiaJi.Append(";");
                stringBuilder_JiaJi.Append("加计Y故障：");
                stringBuilder_JiaJi.Append(JiaJiYGuZhang == 0 ? "正常" : "故障");
                stringBuilder_JiaJi.Append(";");
                stringBuilder_JiaJi.Append("加计Z故障：");
                stringBuilder_JiaJi.Append(JiaJiZGuZhang == 0 ? "正常" : "故障");
                stringBuilder_JiaJi.Append(".");

                stringBuilder_TuoLuo.Append("陀螺X故障：");
                stringBuilder_TuoLuo.Append(TuoLuoXGuZhang == 0 ? "正常" : "故障");
                stringBuilder_TuoLuo.Append(";");
                stringBuilder_TuoLuo.Append("陀螺Y故障：");
                stringBuilder_TuoLuo.Append(TuoLuoYGuZhang == 0 ? "正常" : "故障");
                stringBuilder_TuoLuo.Append(";");
                stringBuilder_TuoLuo.Append("陀螺Z故障：");
                stringBuilder_TuoLuo.Append(TuoLuoZGuZhang == 0 ? "正常" : "故障");
                stringBuilder_TuoLuo.Append(".");
                if (JiaJi == 0) //加计X,Y,Z 全为0显示正常，其他显示异常
                {
                    DHKuaiSu_Ti_JiaJiGuZhang.Text = "正常";
                }
                else
                {
                    DHKuaiSu_Ti_JiaJiGuZhang.Text = "异常";

                }
                DHKuaiSu_Ti_JiaJiGuZhang_Text.Text = stringBuilder_JiaJi.ToString();

                if (TuoLuo == 0) //陀螺X,Y,Z 全为0显示正常，其他显示异常
                {
                    DHKuaiSu_Ti_TuoLuoGuZhang.Text = "正常";
                }
                else
                {
                    DHKuaiSu_Ti_TuoLuoGuZhang.Text = "异常";

                }
                DHKuaiSu_Ti_TuoLuoGuZhang_Text.Text = stringBuilder_TuoLuo.ToString();
            }
            else
            {
                StringBuilder s = new StringBuilder();
                // 
                // 导航系统时间
                // 
                 s.Append(((double)(sObject.daoHangXiTongShiJian * 0.005)).ToString());
                s.Append(",");


                // 
                // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
                // 
                s.Append(((double)(sObject.jingDu * Math.Pow(10, -7))).ToString()); //
                s.Append(",");                                                            // 
                                                                                          // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
                                                                                          // 
                s.Append(((double)(sObject.weiDu * Math.Pow(10, -7))).ToString()); //
                s.Append(",");                                                             // 
                                                                                           // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                                                                                           // 
                s.Append(((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString()); //
                s.Append(",");                                                                 // 

                // 
                //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
                // 
                s.Append(((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString()); //
                s.Append(",");                                                                      // 
                                                                                                    //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                                                                                                    // 
                s.Append(((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString()); //
                s.Append(",");                                                                      // 
                                                                                                    //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                                                                                                    // 
                s.Append(((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString()); //
                s.Append(",");                                                                     // 

                // 
                // GNSS时间 单位s,UTC秒部
                // 
                s.Append(((double)(sObject.GNSSTime * Math.Pow(10, -3))).ToString());//
                s.Append(",");                                                               // 
                                                                                             // 俯仰角
                                                                                             // 
                s.Append(sObject.fuYangJiao.ToString()); //
                s.Append(",");                                   // 
                                                                 // 滚转角
                                                                 // 
                s.Append(sObject.gunZhuanJiao.ToString()); //
                s.Append(",");                                     // 
                                                                   // 偏航角
                                                                   // 
                s.Append(sObject.pianHangJiao.ToString()); //
                s.Append(",");                                     // 

                // 
                // 陀螺X数据
                // 
                s.Append(sObject.tuoLuoShuJu_X.ToString()); //
                s.Append(",");                                       // 
                                                                     // 陀螺Y数据
                                                                     // 
                s.Append(sObject.tuoLuoShuJu_Y.ToString()); //
                s.Append(",");                                       // 
                                                                     // 陀螺Z数据
                                                                     // 
                s.Append(sObject.tuoLuoShuJu_Z.ToString()); //
                s.Append(",");                                       // 

                // 
                // 加速度计X数据
                // 
                s.Append(sObject.jiaSuDuJiShuJu_X.ToString()); //
                s.Append(",");                                         // 
                                                                       // 加速度计Y数据
                                                                       // 
                s.Append(sObject.jiaSuDuJiShuJu_Y.ToString()); //
                s.Append(",");                                        // 
                                                                      // 加速度计Z数据
                                                                      // 
                s.Append(sObject.jiaSuDuJiShuJu_Z.ToString()); //
                s.Append(",");
                // 
                // 状态标志位
                // 
                byte zhuangTaiBiaoZhiWei = sObject.zhuangTaiBiaoZhiWei; //
                                                                        // 
                                                                        // bit0 点火标志（0：未点火 1：已点火）
                                                                        // 
                s.Append((zhuangTaiBiaoZhiWei >> 0 & 0x1) == 1 ? "已点火" : "未点火"); //
                s.Append(",");                                                           // 
                                                                                         // bit1 分离标志（0：已分离 1：未分离）
                                                                                         // 
                s.Append((zhuangTaiBiaoZhiWei >> 1 & 0x1) == 1 ? "未分离" : "已分离"); //
                s.Append(",");                                                                         // 
                                                                                                       // bit2 bit3 00:准备阶段 01：对准阶段 10：导航阶段
                                                                                                       // 
                byte tempValue = (byte)(zhuangTaiBiaoZhiWei >> 2 & 0x3); //
                                                                         // 
                string tempSTR = ""; //
                                     // 
                switch (tempValue)
                // 
                {
                    // 
                    case 0:
                        // 
                        tempSTR = "准备阶段"; //
                                          // 
                        break; //
                               // 
                    case 1:
                        // 
                        tempSTR = "对准阶段"; //
                                          // 
                        break; //
                               // 
                    case 2:
                        // 
                        tempSTR = "导航阶段"; //
                                          // 
                        break; //
                               // 
                    default:
                        // 
                        break; //
                               // 
                }
                // 
                s.Append(tempSTR); //
                s.Append(",");                                    // 
                                                                  // bit4 bit5 00:GPS无更新 01：GPS有更新 10：GPS更新过
                                                                  // 
                tempValue = (byte)(zhuangTaiBiaoZhiWei >> 4 & 0x3); //
                                                                    // 
                tempSTR = ""; //
                              // 
                switch (tempValue)
                // 
                {
                    // 
                    case 0:
                        // 
                        tempSTR = "GPS无更新"; //
                                            // 
                        break; //
                               // 
                    case 1:
                        // 
                        tempSTR = "GPS有更新"; //
                                            // 
                        break; //
                               // 
                    case 2:
                        // 
                        tempSTR = "GPS更新过"; //
                                            // 
                        break; //
                               // 
                    default:
                        // 
                        break; //
                               // 
                }
                // 
                s.Append(tempSTR); //
                s.Append(",");                                   // 
                                                                 // GPS组合标志 (00：上5ms惯导，本5ms惯导; // 01：上5ms惯导，本5ms组合; // 10：上5ms组合，本5ms组合; // 11：上5ms组合，本5ms惯导; //)
                                                                 // 
                tempValue = (byte)(zhuangTaiBiaoZhiWei >> 6 & 0x3); //
                                                                    // 
                tempSTR = ""; //
                              // 
                switch (tempValue)
                // 
                {
                    // 
                    case 0:
                        // 
                        tempSTR = "上5ms惯导，本5ms惯导"; //
                                                   // 
                        break; //
                               // 
                    case 1:
                        // 
                        tempSTR = "上5ms惯导，本5ms组合"; //
                                                   // 
                        break; //
                               // 
                    case 2:
                        // 
                        tempSTR = "上5ms组合，本5ms组合"; //
                                                   // 
                        break; //
                               // 
                    case 3:
                        // 
                        tempSTR = "上5ms组合，本5ms惯导"; //
                                                   // 
                        break; //
                               // 
                    default:
                        // 
                        break; //
                               // 
                }
                // 
                s.Append(tempSTR); //
                s.Append(",");                          // 

                // 
                // 陀螺故障标志
                //
                byte tuoLuoGuZhangBiaoZhi = sObject.tuoLuoGuZhangBiaoZhi;

                //bit0 导航组合完成标识（0：未完成,1：已组合）
                s.Append((tuoLuoGuZhangBiaoZhi >> 0 & 0x1) == 0 ? "未完成" : "已组合");
                s.Append(",");
                //bit1 加计x故障标识（0：正常，1：故障）
                s.Append((tuoLuoGuZhangBiaoZhi >> 1 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");
                //bit2 加计Y故障标识（0：正常，1：故障）
                s.Append((tuoLuoGuZhangBiaoZhi >> 2 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");
                //bit3 加计z故障标识（0：正常，1：故障）
                s.Append((tuoLuoGuZhangBiaoZhi >> 3 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // bit5 陀螺x故障标志（0：正常）
                s.Append((tuoLuoGuZhangBiaoZhi >> 5 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");
                // bit6 陀螺y故障标志（0：正常）
                s.Append((tuoLuoGuZhangBiaoZhi >> 6 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");
                // bit7 陀螺z故障标志（0：正常）                                                         
                s.Append((tuoLuoGuZhangBiaoZhi >> 7 & 0x1) == 0 ? "正常" : "故障");
                stringBuilders_DHK.Add(s);
                
            }
        }

#if false
        //系统快速弹头数据显示(弃用)
        private void showDHKuaiSuTimeStatus_Tou(ref DAOHANGSHUJU_KuaiSu sObject)
        // 
        {
            // 
            // 导航系统时间
            // 
            DHKuaiSu_Tou_DaoHangXiTongShiJian.Text = sObject.daoHangXiTongShiJian.ToString(); //
                                                                                              // 

            // 
            // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
            // 
            DHKuaiSu_Tou_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString(); //
                                                                                                 // 
                                                                                                 // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
                                                                                                 // 
            DHKuaiSu_Tou_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString(); //
                                                                                               // 
                                                                                               // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                                                                                               // 
            DHKuaiSu_Tou_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString(); //
                                                                                                    // 

            // 
            //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
            // 
            DHKuaiSu_Tou_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                               // 
                                                                                                               //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                                                                                                               // 
            DHKuaiSu_Tou_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                             // 
                                                                                                             //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                                                                                                             // 
            DHKuaiSu_Tou_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                               // 

            // 
            // GNSS时间 单位s,UTC秒部
            // 
            DHKuaiSu_Tou_GNSSTime.Text = ((double)(sObject.GNSSTime * Math.Pow(10, -3))).ToString(); //
                                                                                                     // 
                                                                                                     // 俯仰角
                                                                                                     // 
            DHKuaiSu_Tou_FuYangJiao.Text = sObject.fuYangJiao.ToString(); //
                                                                          // 
                                                                          // 滚转角
                                                                          // 
            DHKuaiSu_Tou_GunZhuanJiao.Text = sObject.gunZhuanJiao.ToString(); //
                                                                              // 
                                                                              // 偏航角
                                                                              // 
            DHKuaiSu_Tou_PianHangJiao.Text = sObject.pianHangJiao.ToString(); //
                                                                              // 

            // 
            // 陀螺X数据
            // 
            DHKuaiSu_Tou_TuoLuoXShuJu.Text = sObject.tuoLuoShuJu_X.ToString(); //
                                                                               // 
                                                                               // 陀螺Y数据
                                                                               // 
            DHKuaiSu_Tou_TuoLuoYShuJu.Text = sObject.tuoLuoShuJu_Y.ToString(); //
                                                                               // 
                                                                               // 陀螺Z数据
                                                                               // 
            DHKuaiSu_Tou_TuoLuoZShuJu.Text = sObject.tuoLuoShuJu_Z.ToString(); //
                                                                               // 

            // 
            // 加速度计X数据
            // 
            DHKuaiSu_Tou_JiaSuDuJiX.Text = sObject.jiaSuDuJiShuJu_X.ToString(); //
                                                                                // 
                                                                                // 加速度计Y数据
                                                                                // 
            DHKuaiSu_Tou_JiaSuDuJiY.Text = sObject.jiaSuDuJiShuJu_Y.ToString(); //
                                                                                // 
                                                                                // 加速度计Z数据
                                                                                // 
            DHKuaiSu_Tou_JiaSuDuJiZ.Text = sObject.jiaSuDuJiShuJu_Z.ToString(); //

            // 
            // 状态标志位
            // 
            byte zhuangTaiBiaoZhiWei = sObject.zhuangTaiBiaoZhiWei; //
                                                                    // 
                                                                    // bit0 点火标志（0：未点火 1：已点火）
                                                                    // 
            DHKuaiSu_Tou_DianHuo.Text = (zhuangTaiBiaoZhiWei >> 0 & 0x1) == 1 ? "已点火" : "未点火"; //
                                                                                               // 
                                                                                               // bit1 分离标志（0：已分离 1：未分离）
                                                                                               // 
            DHKuaiSu_Tou_FenLi.Text = (zhuangTaiBiaoZhiWei >> 1 & 0x1) == 1 ? "未分离" : "已分离"; //
                                                                                             // 
                                                                                             // bit2 bit3 00:准备阶段 01：对准阶段 10：导航阶段
                                                                                             // 
            byte tempValue = (byte)(zhuangTaiBiaoZhiWei >> 2 & 0x3); //
                                                                     // 
            string tempSTR = ""; //
                                 // 
            switch (tempValue)
            // 
            {
                // 
                case 0:
                    // 
                    tempSTR = "准备阶段"; //
                                      // 
                    break; //
                           // 
                case 1:
                    // 
                    tempSTR = "对准阶段"; //
                                      // 
                    break; //
                           // 
                case 2:
                    // 
                    tempSTR = "导航阶段"; //
                                      // 
                    break; //
                           // 
                default:
                    // 
                    break; //
                           // 
            }
            // 
            DHKuaiSu_Tou_GongZuoJieDuan.Text = tempSTR; //
                                                        // 
                                                        // bit4 bit5 00:GPS无更新 01：GPS有更新 10：GPS更新过
                                                        // 
            tempValue = (byte)(zhuangTaiBiaoZhiWei >> 4 & 0x3); //
                                                                // 
            tempSTR = ""; //
                          // 
            switch (tempValue)
            // 
            {
                // 
                case 0:
                    // 
                    tempSTR = "GPS无更新"; //
                                        // 
                    break; //
                           // 
                case 1:
                    // 
                    tempSTR = "GPS有更新"; //
                                        // 
                    break; //
                           // 
                case 2:
                    // 
                    tempSTR = "GPS更新过"; //
                                        // 
                    break; //
                           // 
                default:
                    // 
                    break; //
                           // 
            }
            // 
            DHKuaiSu_Tou_GPSShuJuGengXin.Text = tempSTR; //
                                                         // 
                                                         // GPS组合标志 (00：上5ms惯导，本5ms惯导; // 01：上5ms惯导，本5ms组合; // 10：上5ms组合，本5ms组合; // 11：上5ms组合，本5ms惯导; //)
                                                         // 
            tempValue = (byte)(zhuangTaiBiaoZhiWei >> 6 & 0x3); //
                                                                // 
            tempSTR = ""; //
                          // 
            switch (tempValue)
            // 
            {
                // 
                case 0:
                    // 
                    tempSTR = "上5ms惯导，本5ms惯导"; //
                                               // 
                    break; //
                           // 
                case 1:
                    // 
                    tempSTR = "上5ms惯导，本5ms组合"; //
                                               // 
                    break; //
                           // 
                case 2:
                    // 
                    tempSTR = "上5ms组合，本5ms组合"; //
                                               // 
                    break; //
                           // 
                case 3:
                    // 
                    tempSTR = "上5ms组合，本5ms惯导"; //
                                               // 
                    break; //
                           // 
                default:
                    // 
                    break; //
                           // 
            }
            // 
            DHKuaiSu_Tou_GPSZuHe.Text = tempSTR; //
                                                 // 

            // 
            // 陀螺故障标志
            //
            byte tuoLuoGuZhangBiaoZhi = sObject.tuoLuoGuZhangBiaoZhi;

            //bit0 导航组合完成标识（0：未完成,1：已组合）
            DHKuaiSu_Tou_DaoHangZuHe.Text = (tuoLuoGuZhangBiaoZhi >> 0 & 0x1) == 0 ? "未完成" : "已组合";

            //bit1 加计x故障标识（0：正常，1：故障）
            int JiaJiXGuZhang = (tuoLuoGuZhangBiaoZhi >> 1 & 0x1) == 0 ? 0 : 1;

            //bit2 加计Y故障标识（0：正常，1：故障）
            int JiaJiYGuZhang = (tuoLuoGuZhangBiaoZhi >> 2 & 0x1) == 0 ? 0 : 1;

            //bit3 加计z故障标识（0：正常，1：故障）
            int JiaJiZGuZhang = (tuoLuoGuZhangBiaoZhi >> 3 & 0x1) == 0 ? 0 : 1;


            // bit5 陀螺x故障标志（0：正常）
            int TuoLuoXGuZhang = (tuoLuoGuZhangBiaoZhi >> 5 & 0x1) == 0 ? 0 : 1;

            // bit6 陀螺y故障标志（0：正常）
            int TuoLuoYGuZhang = (tuoLuoGuZhangBiaoZhi >> 6 & 0x1) == 0 ? 0 : 1;

            // bit7 陀螺z故障标志（0：正常）                                                         
            int TuoLuoZGuZhang = (tuoLuoGuZhangBiaoZhi >> 7 & 0x1) == 0 ? 0 : 1;

            int JiaJi = JiaJiXGuZhang + JiaJiYGuZhang + JiaJiZGuZhang;
            int TuoLuo = TuoLuoXGuZhang + TuoLuoYGuZhang + TuoLuoZGuZhang;
            StringBuilder stringBuilder_JiaJi = new StringBuilder();
            StringBuilder stringBuilder_TuoLuo = new StringBuilder();

            stringBuilder_JiaJi.Append("加计X故障：");
            stringBuilder_JiaJi.Append(JiaJiXGuZhang == 0 ? "正常" : "故障");
            stringBuilder_JiaJi.Append(";");
            stringBuilder_JiaJi.Append("加计Y故障：");
            stringBuilder_JiaJi.Append(JiaJiYGuZhang == 0 ? "正常" : "故障");
            stringBuilder_JiaJi.Append(";");
            stringBuilder_JiaJi.Append("加计Z故障：");
            stringBuilder_JiaJi.Append(JiaJiZGuZhang == 0 ? "正常" : "故障");
            stringBuilder_JiaJi.Append(".");

            stringBuilder_TuoLuo.Append("陀螺X故障：");
            stringBuilder_TuoLuo.Append(TuoLuoXGuZhang == 0 ? "正常" : "故障");
            stringBuilder_TuoLuo.Append(";");
            stringBuilder_TuoLuo.Append("陀螺Y故障：");
            stringBuilder_TuoLuo.Append(TuoLuoYGuZhang == 0 ? "正常" : "故障");
            stringBuilder_TuoLuo.Append(";");
            stringBuilder_TuoLuo.Append("陀螺Z故障：");
            stringBuilder_TuoLuo.Append(TuoLuoZGuZhang == 0 ? "正常" : "故障");
            stringBuilder_TuoLuo.Append(".");
            if (JiaJi == 0) //加计X,Y,Z 全为0显示正常，其他显示异常
            {
                DHKuaiSu_Tou_JiaJiGuZhang.Text = "正常";
            }
            else
            {
                DHKuaiSu_Tou_JiaJiGuZhang.Text = "异常";
                ToolTip ttprogbar = new ToolTip();
                ttprogbar.Content = stringBuilder_JiaJi.ToString();
                DHKuaiSu_Tou_JiaJiGuZhang.ToolTip = (ttprogbar);
            }

            if (TuoLuo == 0) //陀螺X,Y,Z 全为0显示正常，其他显示异常
            {
                DHKuaiSu_Tou_TuoLuoGuZhang.Text = "正常";
            }
            else
            {
                DHKuaiSu_Tou_TuoLuoGuZhang.Text = "异常";
                ToolTip ttprogbar = new ToolTip();
                ttprogbar.Content = stringBuilder_TuoLuo.ToString();
                DHKuaiSu_Tou_TuoLuoGuZhang.ToolTip = (ttprogbar);
            }                                                                 // 
        }
#endif

        //系统慢速弹体数据显示
        private void showDHManSuTimeStatus_Ti(ref DAOHANGSHUJU_ManSu sObject)
        // 
        {
            if (!dataConversion)
            {
                // 
                // GPS时间 单位s,UTC秒部
                // 
                DHManSu_Ti_GPSTime.Text = ((double)(sObject.GPSTime * Math.Pow(10, -3))).ToString(); //
                                                                                                     // 
                                                                                                     // GPS定位模式
                                                                                                     // 
                byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi; //
                                                                // 
                string tempValueSTR = ""; //
                                          // 

                // 
                // bit0 (1:采用GPS定位 0:没有采用GPS定位)
                // 
                tempValueSTR = (GPSDingWeiMoShi >> 0 & 0x01) == 1 ? "采用GPS定位" : "没有采用GPS定位"; //
                                                                                             // 
                                                                                             //DHManSu_Ti_GPSDingWeiZhuangTai_GPS.Text = tempValueSTR; //
                                                                                             // 
                                                                                             // bit1 (1:采用BD2定位 0:没有采用BD2定位)
                                                                                             // 
                tempValueSTR = (GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位"; //
                                                                                             // 
                DHManSu_Ti_GPSDingWeiZhuangTai_BD2.Text = tempValueSTR; //
                                                                        // 
                                                                        // bit2 1：采用GLONASS定位 0：没有采用GLONASS定位
                                                                        // 
                tempValueSTR = (GPSDingWeiMoShi >> 2 & 0x01) == 1 ? "采用GLONASS定位" : "没有采用GLONASS定位"; //
                                                                                                     // 
                                                                                                     // DHManSu_Ti_GPSDingWeiZhuangTai_GLONASS.Text = tempValueSTR; //
                                                                                                     // 
                                                                                                     // bit3 0:没有DGNSS可用 1：DGNSS可用
                                                                                                     // 
                tempValueSTR = (GPSDingWeiMoShi >> 3 & 0x01) == 1 ? "DGNSS可用" : "没有DGNSS可用"; //
                                                                                             // 
                                                                                             //DHManSu_Ti_GPSDingWeiZhuangTai_DGNSS.Text = tempValueSTR; //
                                                                                             // 
                                                                                             // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
                                                                                             // 
                byte tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03); //
                                                                      // 
                tempValueSTR = tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : "")); //
                                                                                                                          // 
                DHManSu_Ti_GPSDingWeiZhuangTai_Fix.Text = tempValueSTR; //
                                                                        // 
                                                                        // bit6 0:GNSS修正无效 1：GNSS修正有效
                                                                        // 
                tempValueSTR = (GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效"; //
                                                                                             // 
                DHManSu_Ti_GPSDingWeiZhuangTai_GNSSXiuZheng.Text = tempValueSTR; //
                                                                                 // 
                                                                                 // bit7 0:BD2修正无效 1：BD2修正有效
                                                                                 // 
                tempValueSTR = (GPSDingWeiMoShi >> 7 & 0x01) == 1 ? "BD2修正有效" : "BD2修正无效"; //
                                                                                           // 
                                                                                           // DHManSu_Ti_GPSDingWeiZhuangTai_BD2XiuZheng.Text = tempValueSTR; //
                                                                                           // 
                                                                                           // DHManSu_Ti_GPSDingWeiZhuangTai.Text = tempValueSTR; //
                                                                                           // 

                // 

                // 
                // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                // 
                tempValue = sObject.GPS_SV; //
                                            // 
                DHManSu_Ti_GPSSVKeYong.Text = ((byte)(tempValue & 0xF)).ToString(); //
                                                                                    // 
                DHManSu_Ti_GPSCanYuDingWei.Text = ((byte)(tempValue >> 4 & 0xF)).ToString(); //
                                                                                             // 
                                                                                             // BD2 SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                                                                                             // 
                tempValue = sObject.BD2_SV; //
                                            // 
                DHManSu_Ti_BD2KeYong.Text = ((byte)(tempValue & 0xF)).ToString(); //
                                                                                  // 
                DHManSu_Ti_BD2CanYuDingWei.Text = ((byte)(tempValue >> 4 & 0xF)).ToString(); //
                                                                                             // 

                // 
                // sObject.jingDu; //// 经度（组合结果）当量：1e-7
                DHManSu_Ti_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString();

                // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
                DHManSu_Ti_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString();

                // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                DHManSu_Ti_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString();

                //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
                DHManSu_Ti_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString();

                //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                DHManSu_Ti_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString();

                //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                DHManSu_Ti_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString();


                // 
                // PDOP 当量0.01
                // 
                DHManSu_Ti_PDOP.Text = ((double)(sObject.PDOP * 0.01)).ToString(); //
                                                                                   // 
                                                                                   // HDOP 当量0.01
                                                                                   // 
                DHManSu_Ti_HDOP.Text = ((double)(sObject.HDOP * 0.01)).ToString(); //
                                                                                   // 
                                                                                   // VDOP 当量0.01
                                                                                   // 
                DHManSu_Ti_VDOP.Text = ((double)(sObject.VDOP * 0.01)).ToString(); //
                                                                                   // 

                // 
                // X陀螺温度
                string XTuoLuoWenDu = sObject.tuoLuoWenDu_X.ToString();
                // 
                // Y陀螺温度
                string YTuoLuoWenDu = sObject.tuoLuoWenDu_Y.ToString();
                // 
                // Z陀螺温度
                string ZTuoLuoWenDu = sObject.tuoLuoWenDu_Z.ToString();
                // 

                // 
                // X加计温度
                string XJiaJiWenDu = sObject.jiaJiWenDu_X.ToString();


                // Y加计温度
                string YJiaJiWenDu = sObject.jiaJiWenDu_Y.ToString();

                // Z加计温度
                string ZJiaJiWenDu = sObject.jiaJiWenDu_Z.ToString();
                // 

                // 
                // +5V电压值     当量0.05
                string Zheng5VDianYa = ((double)(sObject.dianYaZhi_zheng5V * 0.05)).ToString();
                // 
                // -5V电压值     当量0.05
                string Fu5VDianYa = ((double)(sObject.dianYaZhi_fu5V * 0.05)).ToString();
                // 

                // 
                // +15V电压值    当量0.02
                string Zheng15VDianYa = ((double)(sObject.dianYaZhi_zheng15V * 0.2)).ToString();

                // 
                // -15V电压值    当量0.02
                string Fu15VDianYa = ((double)(sObject.dianYaZhi_fu15V * 0.2)).ToString();
                // 

                // 
                // X陀螺+5V电压值     当量0.05
                string XTuoLuoZheng5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_X_zheng5V * 0.05)).ToString();

                // X陀螺-5V电压值     当量0.05
                string XTuoLuoFu5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_X_fu5V * 0.05)).ToString();

                // 
                // Y陀螺+5V电压值     当量0.05
                string YTuoLuoZheng5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Y_zheng5V * 0.05)).ToString();

                // Y陀螺-5V电压值     当量0.05 
                string YTuoLuoFu5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Y_fu5V * 0.05)).ToString();

                // Z陀螺+5V电压值     当量0.05
                string ZTuoLuoZheng5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Z_zheng5V * 0.05)).ToString();

                // Z陀螺-5V电压值     当量0.05
                string ZTuoLuoFu5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Z_fu5V * 0.05)).ToString();

                // 
                // 与X陀螺通信错误计数（一直循环计数）
                string XTuoLuoTongXinError = sObject.yuTuoLuoTongXingCuoWuJiShu_X.ToString();

                // 与Y陀螺通信错误计数（一直循环计数）
                string YTuoLuoTongXinError = sObject.yuTuoLuoTongXingCuoWuJiShu_Y.ToString();

                // 与Z陀螺通信错误计数（一直循环计数）
                string ZTuoLuoTongXinError = sObject.yuTuoLuoTongXingCuoWuJiShu_Z.ToString();

                // 与GPS接收机通信错误计数（一直循环计数）
                string GPSJieShouJiTongXinError = sObject.yuGPSJieShouJiTongXingCuoWuJiShu.ToString();

                // IMU进入中断次数（每800次+1 循环计数
                string IMUZhongDuan = sObject.IMUJinRuZhongDuanCiShu.ToString();

                // GPS中断次数（每10次+1 循环计数
                string GPSZhongDuan = sObject.GPSZhongDuanCiShu.ToString();

                string QiTaZhuangTaiDataTuoLuo = "";
                QiTaZhuangTaiDataTuoLuo = string.Concat("X陀螺温度：", XTuoLuoWenDu, ";", "Y陀螺温度：", YTuoLuoWenDu, ";", "Z陀螺温度：", ZTuoLuoWenDu, ";\n");

                string QiTaZhuangTaiDataJiaJi = "";
                QiTaZhuangTaiDataJiaJi = string.Concat("X加计温度：", XJiaJiWenDu, ";", "Y加计温度：", YJiaJiWenDu, ";", "Z加计温度：", ZJiaJiWenDu, ";\n");

                string QiTaZhuangTaiData5VDianYa = "";
                QiTaZhuangTaiData5VDianYa = string.Concat("+5V电压值：", Zheng5VDianYa, ";", "-5V电压值：", Fu5VDianYa, ";\n");

                string QiTaZhuangTaiData15VDianYa = "";
                QiTaZhuangTaiData15VDianYa = string.Concat("+15V电压值：", Zheng15VDianYa, ";", "-15V电压值：", Fu15VDianYa, ";\n");

                string QiTaZhuangTaiDataXTuoLuoDianYa = "";
                QiTaZhuangTaiDataXTuoLuoDianYa = string.Concat("X陀螺+5V电压：", XTuoLuoZheng5VDianYa, ";", "X陀螺-5V电压：", XTuoLuoFu5VDianYa, ";\n");

                string QiTaZhuangTaiDataYTuoLuoDianYa = "";
                QiTaZhuangTaiDataYTuoLuoDianYa = string.Concat("Y陀螺+5V电压：", YTuoLuoZheng5VDianYa, ";", "Y陀螺-5V电压：", YTuoLuoFu5VDianYa, ";\n");

                string QiTaZhuangTaiDataZTuoLuoDianYa = "";
                QiTaZhuangTaiDataZTuoLuoDianYa = string.Concat("Z陀螺+5V电压：", ZTuoLuoZheng5VDianYa, ";", "Z陀螺-5V电压：", ZTuoLuoFu5VDianYa, ";\n");

                string QiTaZhuangTaiDataTuoLuoError = "";
                QiTaZhuangTaiDataTuoLuoError = string.Concat("与X陀螺通信错误计：", XTuoLuoTongXinError, ";", "与Y陀螺通信错误计：", YTuoLuoTongXinError, ";", "与Z陀螺通信错误计：", ZTuoLuoTongXinError, ";\n");

                string QiTaZhuangTaiDataCount = "";
                QiTaZhuangTaiDataCount = string.Concat("与GPS接收机通信错误计数：", GPSJieShouJiTongXinError, ";\n", "IMU进入中断次数：", IMUZhongDuan, ";\n", "GPS中断次数：", GPSZhongDuan, ".");

                string QiTaZhuangTaiData = "";
                QiTaZhuangTaiData = string.Concat(QiTaZhuangTaiDataTuoLuo, QiTaZhuangTaiDataJiaJi, QiTaZhuangTaiData5VDianYa,
                                                  QiTaZhuangTaiData15VDianYa, QiTaZhuangTaiDataXTuoLuoDianYa, QiTaZhuangTaiDataYTuoLuoDianYa,
                                                  QiTaZhuangTaiDataZTuoLuoDianYa, QiTaZhuangTaiDataTuoLuoError, QiTaZhuangTaiDataCount);
                //ToolTip tip = new ToolTip();
                //tip.Content = QiTaZhuangTaiData;
                //DHManSu_Ti_QiTaZhuangTaiShuJu.ToolTip = tip;
                DHManSu_Ti_QiTaZhuangTaiShuJu_Text.Text = QiTaZhuangTaiData;

                // 
                // sObject.jingDuZuHe; //// 经度（组合结果）当量：1e-7
                DHManSu_Ti_JingDuZuHe.Text = ((double)(sObject.jingDu_ZuHe * Math.Pow(10, -7))).ToString();

                // sObject.weiDuZuHe; //               // 纬度（组合结果）当量：1e-7
                DHManSu_Ti_WeiDuZuHe.Text = ((double)(sObject.weiDu_ZuHe * Math.Pow(10, -7))).ToString();

                // sObject.haiBaGaoDuZuHe; //          // 海拔高度（组合结果）当量：1e-2
                DHManSu_Ti_GaoDuZuHe.Text = ((double)(sObject.haiBaGaoDu_ZuHe * Math.Pow(10, -2))).ToString();

                //sObject.dongXiangSuDuZuHe; //        // 东向速度（组合结果）当量：1e-2
                DHManSu_Ti_DongXiangSuDuZuHe.Text = ((double)(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

                //sObject.beiXiangSuDuZuHe; //         // 北向速度（组合结果）当量：1e-2
                DHManSu_Ti_BeiXiangSuDuZuHe.Text = ((double)(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

                //sObject.tianXiangSuDuZuHe; //        // 天向速度（组合结果）当量：1e-2
                DHManSu_Ti_TianXiangSuDuZuHe.Text = ((double)(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

                // 俯仰角
                DHManSu_Ti_FuYangJiao.Text = sObject.fuYangJiao.ToString();

                // 滚转角
                DHManSu_Ti_GunZhuanJiao.Text = sObject.gunZhuanJiao.ToString();

                // 偏航角
                DHManSu_Ti_PianHangJiao.Text = sObject.pianHangJiao.ToString();

                // 陀螺X数据
                DHManSu_Ti_TuoLuoXShuJu.Text = sObject.tuoLuoShuJu_X.ToString();

                // 陀螺Y数据
                DHManSu_Ti_TuoLuoYShuJu.Text = sObject.tuoLuoShuJu_Y.ToString();

                // 陀螺Z数据
                DHManSu_Ti_TuoLuoZShuJu.Text = sObject.tuoLuoShuJu_Z.ToString();

                // 加速度计X数据
                DHManSu_Ti_JiaSuDuJiX.Text = sObject.jiaSuDuJiShuJu_X.ToString();

                // 加速度计Y数据
                DHManSu_Ti_JiaSuDuJiY.Text = sObject.jiaSuDuJiShuJu_Y.ToString();

                // 加速度计Z数据
                DHManSu_Ti_JiaSuDuJiZ.Text = sObject.jiaSuDuJiShuJu_Z.ToString();

                // 
                // 标志位1
                // 
                byte biaoZhiWei1 = sObject.biaoZhiWei1;
                // bit0 导航初始值装订标志（0:未装订 1：已装订）
                DHManSu_Ti_DaoHangChuZhiZhuangDing.Text = (biaoZhiWei1 >> 0 & 0x1) == 0 ? "未装订" : "已装订";

                // bit1 发送1553数据标志（0：未发送 1：已发送）
                DHManSu_Ti_1553ShuJuFaSong.Text = (biaoZhiWei1 >> 1 & 0x1) == 0 ? "未发送" : "已发送";

                // bit2 导航标志（0：未导航 1：已导航）
                DHManSu_Ti_DaoHangBiaoZhi.Text = (biaoZhiWei1 >> 2 & 0x1) == 0 ? "未导航" : "已导航";

                // bit3 对准完成标志(0:未对准 1：已对准)
                DHManSu_Ti_DuiZhunWanCheng.Text = (biaoZhiWei1 >> 3 & 0x1) == 0 ? "未对准" : "已对准";

                // bit4 飞行参数读取标志(0:未装订 1：已装订)
                DHManSu_Ti_FeiXingCanShu.Text = (biaoZhiWei1 >> 4 & 0x1) == 0 ? "未装订" : "已装订";

                //bit5 加计x故障标识（0：正常 1：故障）
                int JiaJiXGZ = (biaoZhiWei1 >> 5 & 0x1) == 0 ? 0 : 1;

                //bit 6 加计y故障标志（0：正常 1：故障）
                int JiaJiYGZ = (biaoZhiWei1 >> 6 & 0x1) == 0 ? 0 : 1;

                //bit7 加计z故障标志 （0：正常 1：故障）
                int JiaJiZGZ = (biaoZhiWei1 >> 7 & 0x1) == 0 ? 0 : 1;

                // 
                // 标志位2
                // 
                byte biaoZhiWei2 = sObject.biaoZhiWei2;

                // bit0 bit1 工作模式（00：飞行模式 01：仿真模式1 10：仿真模式2 11：调试模式）
                tempValue = (byte)(biaoZhiWei2 >> 0 & 0x3);
                string tempSTR = "";
                switch (tempValue)
                {
                    case 0:
                        tempSTR = "飞行模式";
                        break;

                    case 1:
                        tempSTR = "仿真模式1";
                        break;

                    case 2:
                        tempSTR = "仿真模式2";
                        break;

                    case 3:
                        tempSTR = "调试模式";
                        break;

                    default:
                        break;
                }
                DHManSu_Ti_GongZuoMoShi.Text = tempSTR;

                //bit2 陀螺X故障标志（0：正常 1：故障）
                int TuoLuoXGZ = (biaoZhiWei2 >> 2 & 0x1) == 0 ? 0 : 1;

                //bit3 陀螺Y故障标志（0：正常 1：故障）
                int TuoLuoYGZ = (biaoZhiWei2 >> 3 & 0x1) == 0 ? 0 : 1;

                //bit4 陀螺Z故障标志（0：正常 1：故障）
                int TuoLuoZGZ = (biaoZhiWei2 >> 4 & 0x1) == 0 ? 0 : 1;

                // bit5 GPS组合标志（0：惯性 1：组合）
                DHManSu_Ti_GPSZuHe.Text = (biaoZhiWei2 >> 5 & 0x1) == 0 ? "惯性" : "组合";

                // bit6 点火标志(0：未点火 1：已点火)
                DHManSu_Ti_DianHuo.Text = (biaoZhiWei2 >> 6 & 0x1) == 0 ? "未点火" : "已点火";

                // bit7 分离标志（0：已分离 1：未分离）
                DHManSu_Ti_FenLi.Text = (biaoZhiWei2 >> 7 & 0x1) == 0 ? "已分离" : "未分离";

                int TuoLuoGZ = TuoLuoXGZ + TuoLuoYGZ + TuoLuoZGZ;
                int JiaJiGZ = JiaJiXGZ + JiaJiYGZ + JiaJiZGZ;

                StringBuilder stringBuilder_JiaJiXGZ = new StringBuilder();
                stringBuilder_JiaJiXGZ.Append("加计X故障：");
                stringBuilder_JiaJiXGZ.Append(JiaJiXGZ.ToString());
                stringBuilder_JiaJiXGZ.Append(";");
                stringBuilder_JiaJiXGZ.Append("加计Y故障：");
                stringBuilder_JiaJiXGZ.Append(JiaJiYGZ.ToString());
                stringBuilder_JiaJiXGZ.Append(";");
                stringBuilder_JiaJiXGZ.Append("加计Z故障：");
                stringBuilder_JiaJiXGZ.Append(JiaJiZGZ.ToString());
                stringBuilder_JiaJiXGZ.Append(";");

                StringBuilder stringBuilder_TuoLuoXGZ = new StringBuilder();
                stringBuilder_TuoLuoXGZ.Append("陀螺X故障：");
                stringBuilder_TuoLuoXGZ.Append(TuoLuoXGZ);
                stringBuilder_TuoLuoXGZ.Append(";");
                stringBuilder_TuoLuoXGZ.Append("陀螺Y故障：");
                stringBuilder_TuoLuoXGZ.Append(TuoLuoYGZ);
                stringBuilder_TuoLuoXGZ.Append(";");
                stringBuilder_TuoLuoXGZ.Append("陀螺Z故障：");
                stringBuilder_TuoLuoXGZ.Append(TuoLuoZGZ);
                stringBuilder_TuoLuoXGZ.Append(";");

                /* 修改时间：2021年4月15日19:09:25
                 * 修改说明: 之前应该是修改过，1是正常；现改为0是正常
                 */
                if (TuoLuoGZ == 0)
                {
                    DHManSu_Ti_TuoLuoGuZhang.Text = "正常";
                }
                else
                {
                    DHManSu_Ti_TuoLuoGuZhang.Text = "异常";
                }
                DHManSu_Ti_TuoLuoGuZhang_Text.Text = stringBuilder_TuoLuoXGZ.ToString();

                if (JiaJiGZ == 0)
                {
                    DHManSu_Ti_JiaJiGuZhang.Text = "正常";
                }
                else
                {
                    DHManSu_Ti_JiaJiGuZhang.Text = "异常";
                }
                DHManSu_Ti_JiaJiGuZhang_Text.Text = stringBuilder_JiaJiXGZ.ToString();
            }
            else
            {
                StringBuilder s = new StringBuilder();
                // 
                // GPS时间 单位s,UTC秒部
                // 
               s.Append(((double)(sObject.GPSTime * Math.Pow(10, -3))).ToString()); //
                s.Append(",");
                                                                                                     // 
                                                                                                     // GPS定位模式
                                                                                                     // 
                byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi; //
                                                               

                // 
                // bit0 (1:采用GPS定位 0:没有采用GPS定位)
                // 
                s.Append((GPSDingWeiMoShi >> 0 & 0x01) == 1 ? "采用GPS定位" : "没有采用GPS定位"); //
                s.Append(",");                                                                     // 
                                                                                                   //DHManSu_Ti_GPSDingWeiZhuangTai_GPS.Text = tempValueSTR; //
                                                                                                   // 
                                                                                                   // bit1 (1:采用BD2定位 0:没有采用BD2定位)
                                                                                                   // 
                s.Append((GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位");
                s.Append(",");// 
                // bit2 1：采用GLONASS定位 0：没有采用GLONASS定位
                // 
                s.Append((GPSDingWeiMoShi >> 2 & 0x01) == 1 ? "采用GLONASS定位" : "没有采用GLONASS定位"); //
                s.Append(",");                                                                                // 
                                                                                                              // DHManSu_Ti_GPSDingWeiZhuangTai_GLONASS.Text = tempValueSTR; //
                                                                                                              // 
                                                                                                              // bit3 0:没有DGNSS可用 1：DGNSS可用
                                                                                                              // 
                s.Append((GPSDingWeiMoShi >> 3 & 0x01) == 1 ? "DGNSS可用" : "没有DGNSS可用"); //
                s.Append(",");                                                                           // 
                                                                                                         //DHManSu_Ti_GPSDingWeiZhuangTai_DGNSS.Text = tempValueSTR; //
                                                                                                         // 
                                                                                                         // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
                                                                                                         // 
                byte tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03); //
                                                                      // 
                s.Append(tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : ""))); //
                s.Append(",");                                                                                                     // 
                                                                                                                                   // 
                                                                                                                                   // bit6 0:GNSS修正无效 1：GNSS修正有效
                                                                                                                                   // 
                s.Append((GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效");
                s.Append(",");// 
                // bit7 0:BD2修正无效 1：BD2修正有效
                // 
                s.Append((GPSDingWeiMoShi >> 7 & 0x01) == 1 ? "BD2修正有效" : "BD2修正无效"); //
                s.Append(",");                                                                          // 
                                                                                                        // DHManSu_Ti_GPSDingWeiZhuangTai_BD2XiuZheng.Text = tempValueSTR; //
                                                                                                        // 
                                                                                                        // DHManSu_Ti_GPSDingWeiZhuangTai.Text = tempValueSTR; //
                                                                                                        // 

                // 

                // 
                // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                // 
                tempValue = sObject.GPS_SV; //
                                            // 
                s.Append(((byte)(tempValue & 0xF)).ToString()); //
                s.Append(",");                                               // 
                s.Append(((byte)(tempValue >> 4 & 0xF)).ToString()); //
                s.Append(",");                                                                           // 
                                                                                                         // BD2 SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                                                                                                         // 
                tempValue = sObject.BD2_SV; //
                                            // 
                s.Append(((byte)(tempValue & 0xF)).ToString()); //
                s.Append(",");                                              // 
                s.Append(((byte)(tempValue >> 4 & 0xF)).ToString()); //
                s.Append(",");                                                 // 

                // 
                // sObject.jingDu; //// 经度（组合结果）当量：1e-7
                s.Append(((double)(sObject.jingDu * Math.Pow(10, -7))).ToString());
                s.Append(",");
                // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
                s.Append(((double)(sObject.weiDu * Math.Pow(10, -7))).ToString());
                s.Append(",");
                // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                s.Append(((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString());
                s.Append(",");
                //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");
                //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");
                //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");

                // 
                // PDOP 当量0.01
                // 
                s.Append(((double)(sObject.PDOP * 0.01)).ToString()); //
                s.Append(",");                                                 // 
                                                                               // HDOP 当量0.01
                                                                               // 
                s.Append(((double)(sObject.HDOP * 0.01)).ToString()); //
                s.Append(",");                                                  // 
                                                                                // VDOP 当量0.01
                                                                                // 
                s.Append(((double)(sObject.VDOP * 0.01)).ToString()); //
                s.Append(",");                                                 // 

                // 
                // X陀螺温度
                s.Append(sObject.tuoLuoWenDu_X.ToString());
                s.Append(",");// 
                // Y陀螺温度
                s.Append(sObject.tuoLuoWenDu_Y.ToString());
                s.Append(",");
                // 
                // Z陀螺温度
                s.Append(sObject.tuoLuoWenDu_Z.ToString());
                s.Append(",");
                // 

                // 
                // X加计温度
                s.Append(sObject.jiaJiWenDu_X.ToString());
                s.Append(",");


                // Y加计温度
                s.Append(sObject.jiaJiWenDu_Y.ToString());
                s.Append(",");
                // Z加计温度
                s.Append(sObject.jiaJiWenDu_Z.ToString());
                s.Append(",");
                // 

                // 
                // +5V电压值     当量0.05
                s.Append(((double)(sObject.dianYaZhi_zheng5V * 0.05)).ToString());
                s.Append(",");
                // 
                // -5V电压值     当量0.05
                s.Append(((double)(sObject.dianYaZhi_fu5V * 0.05)).ToString());
                s.Append(",");
                // 

                // 
                // +15V电压值    当量0.02
                s.Append(((double)(sObject.dianYaZhi_zheng15V * 0.2)).ToString());
                s.Append(",");

                // 
                // -15V电压值    当量0.02
                s.Append(((double)(sObject.dianYaZhi_fu15V * 0.2)).ToString());
                s.Append(",");
                // 

                // 
                // X陀螺+5V电压值     当量0.05
                s.Append(((double)(sObject.tuoLuoDianYaZhi_X_zheng5V * 0.05)).ToString());
                s.Append(",");

                // X陀螺-5V电压值     当量0.05
                s.Append(((double)(sObject.tuoLuoDianYaZhi_X_fu5V * 0.05)).ToString());
                s.Append(",");

                // 
                // Y陀螺+5V电压值     当量0.05
                s.Append(((double)(sObject.tuoLuoDianYaZhi_Y_zheng5V * 0.05)).ToString());
                s.Append(",");

                // Y陀螺-5V电压值     当量0.05 
                s.Append(((double)(sObject.tuoLuoDianYaZhi_Y_fu5V * 0.05)).ToString());
                s.Append(",");

                // Z陀螺+5V电压值     当量0.05
                s.Append(((double)(sObject.tuoLuoDianYaZhi_Z_zheng5V * 0.05)).ToString());
                s.Append(",");

                // Z陀螺-5V电压值     当量0.05
                s.Append(((double)(sObject.tuoLuoDianYaZhi_Z_fu5V * 0.05)).ToString());
                s.Append(",");

                // 
                // 与X陀螺通信错误计数（一直循环计数）
                s.Append(sObject.yuTuoLuoTongXingCuoWuJiShu_X.ToString());
                s.Append(",");

                // 与Y陀螺通信错误计数（一直循环计数）
                s.Append(sObject.yuTuoLuoTongXingCuoWuJiShu_Y.ToString());
                s.Append(",");

                // 与Z陀螺通信错误计数（一直循环计数）
                s.Append(sObject.yuTuoLuoTongXingCuoWuJiShu_Z.ToString());
                s.Append(",");

                // 与GPS接收机通信错误计数（一直循环计数）
                s.Append(sObject.yuGPSJieShouJiTongXingCuoWuJiShu.ToString());
                s.Append(",");

                // IMU进入中断次数（每800次+1 循环计数
                s.Append(sObject.IMUJinRuZhongDuanCiShu.ToString());
                s.Append(",");

                // GPS中断次数（每10次+1 循环计数
                s.Append(sObject.GPSZhongDuanCiShu.ToString());
                s.Append(",");


                // 
                // 标志位1
                // 
                byte biaoZhiWei1 = sObject.biaoZhiWei1;
                // bit0 导航初始值装订标志（0:未装订 1：已装订）
                s.Append((biaoZhiWei1 >> 0 & 0x1) == 0 ? "未装订" : "已装订");
                s.Append(",");

                // bit1 发送1553数据标志（0：未发送 1：已发送）
                s.Append((biaoZhiWei1 >> 1 & 0x1) == 0 ? "未发送" : "已发送");
                s.Append(",");

                // bit2 导航标志（0：未导航 1：已导航）
                s.Append((biaoZhiWei1 >> 2 & 0x1) == 0 ? "未导航" : "已导航");
                s.Append(",");

                // bit3 对准完成标志(0:未对准 1：已对准)
                s.Append((biaoZhiWei1 >> 3 & 0x1) == 0 ? "未对准" : "已对准");
                s.Append(",");

                // bit4 飞行参数读取标志(0:未装订 1：已装订)
                s.Append((biaoZhiWei1 >> 4 & 0x1) == 0 ? "未装订" : "已装订");
                s.Append(",");

                //bit5 加计x故障标识（0：正常 1：故障）
                s.Append((biaoZhiWei1 >> 5 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                //bit 6 加计y故障标志（0：正常 1：故障）
                s.Append((biaoZhiWei1 >> 6 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                //bit7 加计z故障标志 （0：正常 1：故障）
                s.Append((biaoZhiWei1 >> 7 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // 
                // 标志位2
                // 
                byte biaoZhiWei2 = sObject.biaoZhiWei2;

                // bit0 bit1 工作模式（00：飞行模式 01：仿真模式1 10：仿真模式2 11：调试模式）
                tempValue = (byte)(biaoZhiWei2 >> 0 & 0x3);
                string tempSTR = "";
                switch (tempValue)
                {
                    case 0:
                        tempSTR = "飞行模式";
                        break;

                    case 1:
                        tempSTR = "仿真模式1";
                        break;

                    case 2:
                        tempSTR = "仿真模式2";
                        break;

                    case 3:
                        tempSTR = "调试模式";
                        break;

                    default:
                        break;
                }
                s.Append(tempSTR);
                s.Append(",");


                /***********************************************************
                 * 2021年8月11日15:09:08 代码错误，应该是 “&” 符号，误写为“>>”
                 * 
                 * ********************************************************/
                //bit2 陀螺X故障标志（0：正常 1：故障）
                s.Append((biaoZhiWei2 >> 2 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                //bit3 陀螺Y故障标志（0：正常 1：故障）
                s.Append((biaoZhiWei2 >> 3 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                //bit4 陀螺Z故障标志（0：正常 1：故障）
                s.Append((biaoZhiWei2 >> 4 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // bit5 GPS组合标志（0：惯性 1：组合）
                s.Append((biaoZhiWei2 >> 5 & 0x1) == 0 ? "惯性" : "组合");
                s.Append(",");
                // bit6 点火标志(0：未点火 1：已点火)
                s.Append((biaoZhiWei2 >> 6 & 0x1) == 0 ? "未点火" : "已点火");
                s.Append(",");
                // bit7 分离标志（0：已分离 1：未分离）
                s.Append((biaoZhiWei2 >> 7 & 0x1) == 0 ? "已分离" : "未分离");
                s.Append(",");

                // sObject.jingDuZuHe; //// 经度（组合结果）当量：1e-7
                s.Append(((double)(sObject.jingDu_ZuHe * Math.Pow(10, -7))).ToString());
                s.Append(",");

                // sObject.weiDuZuHe; //               // 纬度（组合结果）当量：1e-7
                s.Append(((double)(sObject.weiDu_ZuHe * Math.Pow(10, -7))).ToString());
                s.Append(",");

                // sObject.haiBaGaoDuZuHe; //          // 海拔高度（组合结果）当量：1e-2
                s.Append(((double)(sObject.haiBaGaoDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //sObject.dongXiangSuDuZuHe; //        // 东向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //sObject.beiXiangSuDuZuHe; //         // 北向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //sObject.tianXiangSuDuZuHe; //        // 天向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                // 俯仰角
                s.Append(sObject.fuYangJiao.ToString());
                s.Append(",");

                // 滚转角
                s.Append(sObject.gunZhuanJiao.ToString());
                s.Append(",");

                // 偏航角
                s.Append(sObject.pianHangJiao.ToString());
                s.Append(",");

                // 陀螺X数据
                s.Append(sObject.tuoLuoShuJu_X.ToString());
                s.Append(",");

                // 陀螺Y数据
                s.Append(sObject.tuoLuoShuJu_Y.ToString());
                s.Append(",");

                // 陀螺Z数据
                s.Append(sObject.tuoLuoShuJu_Z.ToString());
                s.Append(",");

                // 加速度计X数据
                s.Append(sObject.jiaSuDuJiShuJu_X.ToString());
                s.Append(",");

                // 加速度计Y数据
                s.Append(sObject.jiaSuDuJiShuJu_Y.ToString());
                s.Append(",");

                // 加速度计Z数据
                s.Append(sObject.jiaSuDuJiShuJu_Z.ToString());
                s.Append(",");

                stringBuilders_DHM.Add(s);
            }
        }

#if false
        //系统慢速弹头数据显示(弃用)
        private void showDHManSuTimeStatus_Tou(ref DAOHANGSHUJU_ManSu sObject)
        // 
        {
            // 
            // GPS时间 单位s,UTC秒部
            // 
            DHManSu_Tou_GPSTime.Text = /*ConvertIntDatetime(sObject.GPSTime).ToString()*/sObject.GPSTime.ToString(); //
                                                                                                                     // 
                                                                                                                     // GPS定位模式
                                                                                                                     // 
            byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi; //
                                                            // 
            string tempValueSTR = ""; //
                                      // 

            // 
            // bit0 (1:采用GPS定位 0:没有采用GPS定位)
            // 
            tempValueSTR = (GPSDingWeiMoShi >> 0 & 0x01) == 1 ? "采用GPS定位" : "没有采用GPS定位"; //
                                                                                         // 
            DHManSu_Tou_GPSDingWeiZhuangTai_GPS.Text = tempValueSTR; //
                                                                     // 
                                                                     // bit1 (1:采用BD2定位 0:没有采用BD2定位)
                                                                     // 
            tempValueSTR = (GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位"; //
                                                                                         // 
            DHManSu_Tou_GPSDingWeiZhuangTai_BD2.Text = tempValueSTR; //
                                                                     // 
                                                                     // bit2 1：采用GLONASS定位 0：没有采用GLONASS定位
                                                                     // 
            tempValueSTR = (GPSDingWeiMoShi >> 2 & 0x01) == 1 ? "采用GLONASS定位" : "没有采用GLONASS定位"; //
                                                                                                 // 
            DHManSu_Tou_GPSDingWeiZhuangTai_GLONASS.Text = tempValueSTR; //
                                                                         // 
                                                                         // bit3 0:没有DGNSS可用 1：DGNSS可用
                                                                         // 
            tempValueSTR = (GPSDingWeiMoShi >> 3 & 0x01) == 1 ? "DGNSS可用" : "没有DGNSS可用"; //
                                                                                         // 
            DHManSu_Tou_GPSDingWeiZhuangTai_DGNSS.Text = tempValueSTR; //
                                                                       // 
                                                                       // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
                                                                       // 
            byte tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03); //
                                                                  // 
            tempValueSTR = tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : "")); //
                                                                                                                      // 
            DHManSu_Tou_GPSDingWeiZhuangTai_Fix.Text = tempValueSTR; //
                                                                     // 
                                                                     // bit6 0:GNSS修正无效 1：GNSS修正有效
                                                                     // 
            tempValueSTR = (GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效"; //
                                                                                         // 
            DHManSu_Tou_GPSDingWeiZhuangTai_GNSSXiuZheng.Text = tempValueSTR; //
                                                                              // 
                                                                              // bit7 0:BD2修正无效 1：BD2修正有效
                                                                              // 
            tempValueSTR = (GPSDingWeiMoShi >> 7 & 0x01) == 1 ? "BD2修正有效" : "BD2修正无效"; //
                                                                                       // 
            DHManSu_Tou_GPSDingWeiZhuangTai_BD2XiuZheng.Text = tempValueSTR; //
                                                                             // 
                                                                             // DHManSu_Tou_GPSDingWeiZhuangTai.Text = tempValueSTR; //
                                                                             // 

            // 

            // 
            // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
            // 
            tempValue = sObject.GPS_SV; //
                                        // 
            DHManSu_Tou_GPSSVKeYong.Text = ((byte)(tempValue & 0xF)).ToString(); //
                                                                                 // 
            DHManSu_Tou_GPSCanYuDingWei.Text = ((byte)(tempValue >> 4 & 0xF)).ToString(); //
                                                                                          // 
                                                                                          // BD2 SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                                                                                          // 
            tempValue = sObject.BD2_SV; //
                                        // 
            DHManSu_Tou_BD2KeYong.Text = ((byte)(tempValue & 0xF)).ToString(); //
                                                                               // 
            DHManSu_Tou_BD2CanYuDingWei.Text = ((byte)(tempValue >> 4 & 0xF)).ToString(); //
                                                                                          // 

            // 
            // sObject.jingDu; //// 经度（组合结果）当量：1e-7
            DHManSu_Tou_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString();

            // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
            DHManSu_Tou_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString();

            // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
            DHManSu_Tou_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString();

            //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
            DHManSu_Tou_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString();

            //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
            DHManSu_Tou_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString();

            //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
            DHManSu_Tou_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString();


            // 
            // PDOP 当量0.01
            // 
            DHManSu_Tou_PDOP.Text = ((double)(sObject.PDOP * 0.01)).ToString(); //
                                                                                // 
                                                                                // HDOP 当量0.01
                                                                                // 
            DHManSu_Tou_HDOP.Text = ((double)(sObject.HDOP * 0.01)).ToString(); //
                                                                                // 
                                                                                // VDOP 当量0.01
                                                                                // 
            DHManSu_Tou_VDOP.Text = ((double)(sObject.VDOP * 0.01)).ToString(); //
                                                                                // 

            // 
            // X陀螺温度
            string XTuoLuoWenDu = sObject.tuoLuoWenDu_X.ToString();
            // 
            // Y陀螺温度
            string YTuoLuoWenDu = sObject.tuoLuoWenDu_Y.ToString();
            // 
            // Z陀螺温度
            string ZTuoLuoWenDu = sObject.tuoLuoWenDu_Z.ToString();
            // 

            // 
            // X加计温度
            string XJiaJiWenDu = sObject.jiaJiWenDu_X.ToString();


            // Y加计温度
            string YJiaJiWenDu = sObject.jiaJiWenDu_Y.ToString();

            // Z加计温度
            string ZJiaJiWenDu = sObject.jiaJiWenDu_Z.ToString();
            // 

            // 
            // +5V电压值     当量0.05
            string Zheng5VDianYa = ((double)(sObject.dianYaZhi_zheng5V * 0.05)).ToString();
            // 
            // -5V电压值     当量0.05
            string Fu5VDianYa = ((double)(sObject.dianYaZhi_fu5V * 0.05)).ToString();
            // 

            // 
            // +15V电压值    当量0.02
            string Zheng15VDianYa = ((double)(sObject.dianYaZhi_zheng15V * 0.2)).ToString();

            // 
            // -15V电压值    当量0.02
            string Fu15VDianYa = ((double)(sObject.dianYaZhi_fu15V * 0.2)).ToString();
            // 

            // 
            // X陀螺+5V电压值     当量0.05
            string XTuoLuoZheng5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_X_zheng5V * 0.05)).ToString();

            // X陀螺-5V电压值     当量0.05
            string XTuoLuoFu5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_X_fu5V * 0.05)).ToString();

            // 
            // Y陀螺+5V电压值     当量0.05
            string YTuoLuoZheng5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Y_zheng5V * 0.05)).ToString();

            // Y陀螺-5V电压值     当量0.05 
            string YTuoLuoFu5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Y_fu5V * 0.05)).ToString();

            // Z陀螺+5V电压值     当量0.05
            string ZTuoLuoZheng5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Z_zheng5V * 0.05)).ToString();

            // Z陀螺-5V电压值     当量0.05
            string ZTuoLuoFu5VDianYa = ((double)(sObject.tuoLuoDianYaZhi_Z_fu5V * 0.05)).ToString();

            // 
            // 与X陀螺通信错误计数（一直循环计数）
            string XTuoLuoTongXinError = sObject.yuTuoLuoTongXingCuoWuJiShu_X.ToString();

            // 与Y陀螺通信错误计数（一直循环计数）
            string YTuoLuoTongXinError = sObject.yuTuoLuoTongXingCuoWuJiShu_Y.ToString();

            // 与Z陀螺通信错误计数（一直循环计数）
            string ZTuoLuoTongXinError = sObject.yuTuoLuoTongXingCuoWuJiShu_Z.ToString();

            // 与GPS接收机通信错误计数（一直循环计数）
            string GPSJieShouJiTongXinError = sObject.yuGPSJieShouJiTongXingCuoWuJiShu.ToString();

            // IMU进入中断次数（每800次+1 循环计数
            string IMUZhongDuan = sObject.IMUJinRuZhongDuanCiShu.ToString();

            // GPS中断次数（每10次+1 循环计数
            string GPSZhongDuan = sObject.GPSZhongDuanCiShu.ToString();

            string QiTaZhuangTaiDataTuoLuo = "";
            QiTaZhuangTaiDataTuoLuo = string.Concat("X陀螺温度：", XTuoLuoWenDu, ";", "Y陀螺温度：", YTuoLuoWenDu, ";", "Z陀螺温度：", ZTuoLuoWenDu, ";\n");

            string QiTaZhuangTaiDataJiaJi = "";
            QiTaZhuangTaiDataJiaJi = string.Concat("X加计温度：", XJiaJiWenDu, ";", "Y加计温度：", YJiaJiWenDu, ";", "Z加计温度：", ";\n");

            string QiTaZhuangTaiData5VDianYa = "";
            QiTaZhuangTaiData5VDianYa = string.Concat("+5V电压值：", Zheng5VDianYa, ";", "-5V电压值：", Fu5VDianYa, ";\n");

            string QiTaZhuangTaiData15VDianYa = "";
            QiTaZhuangTaiData15VDianYa = string.Concat("+15V电压值：", Zheng15VDianYa, ";", "-15V电压值：", Fu15VDianYa, ";\n");

            string QiTaZhuangTaiDataXTuoLuoDianYa = "";
            QiTaZhuangTaiDataXTuoLuoDianYa = string.Concat("X陀螺+5V电压：", XTuoLuoZheng5VDianYa, ";", "X陀螺-5V电压：", XTuoLuoFu5VDianYa, ";\n");

            string QiTaZhuangTaiDataYTuoLuoDianYa = "";
            QiTaZhuangTaiDataYTuoLuoDianYa = string.Concat("Y陀螺+5V电压：", YTuoLuoZheng5VDianYa, ";", "Y陀螺-5V电压：", YTuoLuoFu5VDianYa, ";\n");

            string QiTaZhuangTaiDataZTuoLuoDianYa = "";
            QiTaZhuangTaiDataZTuoLuoDianYa = string.Concat("Z陀螺+5V电压：", ZTuoLuoZheng5VDianYa, ";", "Z陀螺-5V电压：", ZTuoLuoFu5VDianYa, ";\n");

            string QiTaZhuangTaiDataTuoLuoError = "";
            QiTaZhuangTaiDataTuoLuoError = string.Concat("与X陀螺通信错误计：", XTuoLuoTongXinError, ";", "与Y陀螺通信错误计：", YTuoLuoTongXinError, ";", "与Z陀螺通信错误计：", ZTuoLuoTongXinError, ";");

            string QiTaZhuangTaiDataCount = "";
            QiTaZhuangTaiDataCount = string.Concat("与GPS接收机通信错误计数：", GPSJieShouJiTongXinError, ";", "IMU进入中断次数：", IMUZhongDuan, ";", "GPS中断次数：", GPSZhongDuan, ".");

            string QiTaZhuangTaiData = "";
            QiTaZhuangTaiData = string.Concat(QiTaZhuangTaiDataTuoLuo, QiTaZhuangTaiDataJiaJi, QiTaZhuangTaiData5VDianYa,
                                              QiTaZhuangTaiData15VDianYa, QiTaZhuangTaiDataXTuoLuoDianYa, QiTaZhuangTaiDataYTuoLuoDianYa,
                                              QiTaZhuangTaiDataZTuoLuoDianYa, QiTaZhuangTaiDataTuoLuoError, QiTaZhuangTaiDataCount);
            ToolTip tip = new ToolTip();
            tip.Content = QiTaZhuangTaiData;
            DHManSu_Tou_QiTaZhuangTaiShuJu.ToolTip = tip;

            // 
            // sObject.jingDuZuHe; //// 经度（组合结果）当量：1e-7
            DHManSu_Tou_JingDuZuHe.Text = ((double)(sObject.jingDu_ZuHe * Math.Pow(10, -7))).ToString();

            // sObject.weiDuZuHe; //               // 纬度（组合结果）当量：1e-7
            DHManSu_Tou_WeiDuZuHe.Text = ((double)(sObject.weiDu_ZuHe * Math.Pow(10, -7))).ToString();

            // sObject.haiBaGaoDuZuHe; //          // 海拔高度（组合结果）当量：1e-2
            DHManSu_Tou_GaoDuZuHe.Text = ((double)(sObject.haiBaGaoDu_ZuHe * Math.Pow(10, -2))).ToString();

            //sObject.dongXiangSuDuZuHe; //        // 东向速度（组合结果）当量：1e-2
            DHManSu_Tou_DongXiangSuDuZuHe.Text = ((double)(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

            //sObject.beiXiangSuDuZuHe; //         // 北向速度（组合结果）当量：1e-2
            DHManSu_Tou_BeiXiangSuDuZuHe.Text = ((double)(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

            //sObject.tianXiangSuDuZuHe; //        // 天向速度（组合结果）当量：1e-2
            DHManSu_Tou_TianXiangSuDuZuHe.Text = ((double)(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

            // 俯仰角
            DHManSu_Tou_FuYangJiao.Text = sObject.fuYangJiao.ToString();

            // 滚转角
            DHManSu_Tou_GunZhuanJiao.Text = sObject.gunZhuanJiao.ToString();

            // 偏航角
            DHManSu_Tou_PianHangJiao.Text = sObject.pianHangJiao.ToString();

            // 陀螺X数据
            DHManSu_Tou_TuoLuoXShuJu.Text = sObject.tuoLuoShuJu_X.ToString();

            // 陀螺Y数据
            DHManSu_Tou_TuoLuoYShuJu.Text = sObject.tuoLuoShuJu_Y.ToString();

            // 陀螺Z数据
            DHManSu_Tou_TuoLuoZShuJu.Text = sObject.tuoLuoShuJu_Z.ToString();

            // 加速度计X数据
            DHManSu_Tou_JiaSuDuJiX.Text = sObject.jiaSuDuJiShuJu_X.ToString();

            // 加速度计Y数据
            DHManSu_Tou_JiaSuDuJiY.Text = sObject.jiaSuDuJiShuJu_Y.ToString();

            // 加速度计Z数据
            DHManSu_Tou_JiaSuDuJiZ.Text = sObject.jiaSuDuJiShuJu_Z.ToString();

            // 
            // 标志位1
            // 
            byte biaoZhiWei1 = sObject.biaoZhiWei1;
            // bit0 导航初始值装订标志（0:未装订 1：已装订）
            DHManSu_Tou_DaoHangChuZhiZhuangDing.Text = (biaoZhiWei1 >> 0 & 0x1) == 0 ? "未装订" : "已装订";

            // bit1 发送1553数据标志（0：未发送 1：已发送）
            DHManSu_Tou_1553ShuJuFaSong.Text = (biaoZhiWei1 >> 1 & 0x1) == 0 ? "未发送" : "已发送";

            // bit2 导航标志（0：未导航 1：已导航）
            DHManSu_Tou_DaoHangBiaoZhi.Text = (biaoZhiWei1 >> 2 & 0x1) == 0 ? "未导航" : "已导航";

            // bit3 对准完成标志(0:未对准 1：已对准)
            DHManSu_Tou_DuiZhunWanCheng.Text = (biaoZhiWei1 >> 3 & 0x1) == 0 ? "未对准" : "已对准";

            // bit4 飞行参数读取标志(0:未装订 1：已装订)
            DHManSu_Tou_FeiXingCanShu.Text = (biaoZhiWei1 >> 4 & 0x1) == 0 ? "未装订" : "已装订";

            //bit5 加计x故障标识（0：正常 1：故障）
            int JiaJiXGZ = (biaoZhiWei1 >> 5 & 0x1) == 0 ? 0 : 1;

            //bit 6 加计y故障标志（0：正常 1：故障）
            int JiaJiYGZ = (biaoZhiWei1 >> 6 & 0x1) == 0 ? 0 : 1;

            //bit7 加计z故障标志 （0：正常 1：故障）
            int JiaJiZGZ = (biaoZhiWei1 >> 7 & 0x1) == 0 ? 0 : 1;

            // 
            // 标志位2
            // 
            byte biaoZhiWei2 = sObject.biaoZhiWei2;

            // bit0 bit1 工作模式（00：飞行模式 01：仿真模式1 10：仿真模式2 11：调试模式）
            tempValue = (byte)(biaoZhiWei2 >> 0 & 0x3);
            string tempSTR = "";
            switch (tempValue)
            {
                case 0:
                    tempSTR = "飞行模式";
                    break;

                case 1:
                    tempSTR = "仿真模式1";
                    break;

                case 2:
                    tempSTR = "仿真模式2";
                    break;

                case 3:
                    tempSTR = "调试模式";
                    break;

                default:
                    break;
            }
            DHManSu_Tou_GongZuoMoShi.Text = tempSTR;

            //bit2 陀螺X故障标志（0：正常 1：故障）
            int TuoLuoXGZ = (biaoZhiWei2 >> 2 >> 0x1) == 0 ? 0 : 1;

            //bit3 陀螺Y故障标志（0：正常 1：故障）
            int TuoLuoYGZ = (biaoZhiWei2 >> 3 >> 0x1) == 0 ? 0 : 1;

            //bit4 陀螺Z故障标志（0：正常 1：故障）
            int TuoLuoZGZ = (biaoZhiWei2 >> 4 >> 0x1) == 0 ? 0 : 1;

            // bit5 GPS组合标志（0：惯性 1：组合）
            DHManSu_Tou_GPSZuHe.Text = (biaoZhiWei2 >> 5 & 0x1) == 0 ? "惯性" : "组合";

            // bit6 点火标志(0：未点火 1：已点火)
            DHManSu_Tou_DianHuo.Text = (biaoZhiWei2 >> 6 & 0x1) == 0 ? "未点火" : "已点火";

            // bit7 分离标志（0：已分离 1：未分离）
            DHManSu_Tou_FenLi.Text = (biaoZhiWei2 >> 7 & 0x1) == 0 ? "已分离" : "未分离";

            int TuoLuoGZ = TuoLuoXGZ + TuoLuoYGZ + TuoLuoZGZ;
            int JiaJiGZ = JiaJiXGZ + JiaJiYGZ + JiaJiZGZ;

            StringBuilder stringBuilder_JiaJiXGZ = new StringBuilder();
            stringBuilder_JiaJiXGZ.Append("加计X故障：");
            stringBuilder_JiaJiXGZ.Append(JiaJiXGZ.ToString());
            stringBuilder_JiaJiXGZ.Append(";");
            stringBuilder_JiaJiXGZ.Append("加计Y故障：");
            stringBuilder_JiaJiXGZ.Append(JiaJiYGZ.ToString());
            stringBuilder_JiaJiXGZ.Append(";");
            stringBuilder_JiaJiXGZ.Append("加计Z故障：");
            stringBuilder_JiaJiXGZ.Append(JiaJiZGZ.ToString());
            stringBuilder_JiaJiXGZ.Append(";");

            StringBuilder stringBuilder_TuoLuoXGZ = new StringBuilder();
            stringBuilder_TuoLuoXGZ.Append("陀螺X故障：");
            stringBuilder_TuoLuoXGZ.Append(TuoLuoXGZ);
            stringBuilder_TuoLuoXGZ.Append(";");
            stringBuilder_TuoLuoXGZ.Append("陀螺Y故障：");
            stringBuilder_TuoLuoXGZ.Append(TuoLuoYGZ);
            stringBuilder_TuoLuoXGZ.Append(";");
            stringBuilder_TuoLuoXGZ.Append("陀螺Z故障：");
            stringBuilder_TuoLuoXGZ.Append(TuoLuoZGZ);
            stringBuilder_TuoLuoXGZ.Append(";");

            if (TuoLuoGZ == 0)
            {
                DHManSu_Tou_TuoLuoGuZhang.Text = "正常";
            }
            else
            {
                DHManSu_Tou_TuoLuoGuZhang.Text = "异常";
                ToolTip ttprogbar1 = new ToolTip();
                ttprogbar1.Content = stringBuilder_TuoLuoXGZ.ToString();
                DHManSu_Tou_TuoLuoGuZhang.ToolTip = ttprogbar1;

            }

            if (JiaJiGZ == 0)
            {
                DHManSu_Tou_JiaJiGuZhang.Text = "正常";
            }
            else
            {
                DHManSu_Tou_JiaJiGuZhang.Text = "异常";
                ToolTip ttprogbar1 = new ToolTip();
                ttprogbar1.Content = stringBuilder_JiaJiXGZ.ToString();
                DHManSu_Tou_JiaJiGuZhang.ToolTip = ttprogbar1;
            }
        }
#endif

        //系统即时反馈弹体数据显示
        private void showXiTongJiShiTimeStatus_Ti(ref SYSTEMImmediate_STATUS sObject)
        {
            if (!dataConversion)
            {
                // 
                // 故障标志位
                // 
                byte guZhangBiaoZhi = sObject.guZhangBiaoZhi;

                // bit0 陀螺x故障标志（0：正常；1：故障）
                int XTuoLuoGuZhang = (guZhangBiaoZhi & 0x1) == 0 ? 0 : 1;

                // bit1 陀螺y故障标志（0：正常；1：故障）
                int YTuoLuoGuZhang = (guZhangBiaoZhi >> 1 & 0x1) == 0 ? 0 : 1;

                // bit2 陀螺z故障标志（0：正常；1：故障）
                int ZTuoLuoGuZhang = (guZhangBiaoZhi >> 2 & 0x1) == 0 ? 0 : 1;

                // bit3 RS422故障标志（0：正常；1：故障） 
                int RS422GuZhang = (guZhangBiaoZhi >> 3 & 0x1) == 0 ? 0 : 1;

                // bit4 1553B故障标志（0：正常；1：故障）
                int _1553BGuZhang = (guZhangBiaoZhi >> 4 & 0x1) == 0 ? 0 : 1;

                //bit5 加计X故障标志（0：正常；1：故障）
                int JiaJiXGZ = (guZhangBiaoZhi >> 5 & 0x1) == 0 ? 0 : 1;

                //bit6 加计Y故障标志（0：正常；1：故障）
                int JiaJiYGZ = (guZhangBiaoZhi >> 6 & 0x1) == 0 ? 0 : 1;

                //bit7 加计Z故障标志（0：正常；1：故障）
                int JiaJiZGZ = (guZhangBiaoZhi >> 7 & 0x1) == 0 ? 0 : 1;

                StringBuilder guZhangBiaoZhiWei = new StringBuilder();
                guZhangBiaoZhiWei.Append("陀螺x故障标志：");
                guZhangBiaoZhiWei.Append(XTuoLuoGuZhang == 0 ? "正常;" : "故障;");
                guZhangBiaoZhiWei.Append("陀螺y故障标志：");
                guZhangBiaoZhiWei.Append(YTuoLuoGuZhang == 0 ? "正常;" : "故障;");
                guZhangBiaoZhiWei.Append("陀螺z故障标志：");
                guZhangBiaoZhiWei.Append(ZTuoLuoGuZhang == 0 ? "正常;\n" : "故障;\n");
                guZhangBiaoZhiWei.Append("RS422通信状态：");
                guZhangBiaoZhiWei.Append(RS422GuZhang == 0 ? "正常;" : "故障;");
                guZhangBiaoZhiWei.Append("1553B通信状态：");
                guZhangBiaoZhiWei.Append(_1553BGuZhang == 0 ? "正常;\n" : "故障;\n");
                guZhangBiaoZhiWei.Append("加计X故障标志：");
                guZhangBiaoZhiWei.Append(JiaJiXGZ == 0 ? "正常;" : "故障;");
                guZhangBiaoZhiWei.Append("加计Y故障标志：");
                guZhangBiaoZhiWei.Append(JiaJiYGZ == 0 ? "正常;" : "故障;");
                guZhangBiaoZhiWei.Append("加计Z故障标志：");
                guZhangBiaoZhiWei.Append(JiaJiZGZ == 0 ? "正常;" : "故障;");

                int GZBiaoZhi = XTuoLuoGuZhang + YTuoLuoGuZhang + ZTuoLuoGuZhang
                              + RS422GuZhang + _1553BGuZhang + JiaJiXGZ +
                              JiaJiYGZ + JiaJiZGZ;
                if (GZBiaoZhi == 0)
                {
                    XTJS_Ti_GuZhangBiaoZhi.Text = "正常";
                }
                else
                {
                    XTJS_Ti_GuZhangBiaoZhi.Text = "异常";

                }
                XTJS_Ti_GuZhangBiaoZhi_Text.Text = guZhangBiaoZhiWei.ToString();

                // 
                // X陀螺温度
                // 
                // Y陀螺温度
                // 
                // Z陀螺温度
                // 
                XTJS_Ti_XTuoLuoWenDu.Text = sObject.tuoLuoWenDu_X.ToString();
                XTJS_Ti_YTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Y.ToString();
                XTJS_Ti_ZTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Z.ToString();

                // 
                // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                // 
                byte tempValueGPS = sObject.GPS_SV;
                XTJS_Ti_GPSSVKeYong.Text = ((byte)(tempValueGPS & 0xF)).ToString();
                XTJS_Ti_GPSCanYuDingWei.Text = ((byte)(tempValueGPS >> 4 & 0xF)).ToString();

                //BD2SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                //
                byte tempVauleBD2 = sObject.BD2SV;
                XTJS_Ti_BD2SV.Text = ((byte)(tempVauleBD2 & 0xF)).ToString();
                XTJS_Ti_BD2CanYuDingWei.Text = ((byte)(tempVauleBD2 >> 4 & 0xF)).ToString();

                // 
                // GPS定位模式
                // 
                byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi;
                string tempValueSTR = "";

                // bit1 (1:采用BD2定位 0:没有采用BD2定位)
                tempValueSTR = (GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位";
                XTJS_Ti_ShiFouCaiYongBeiDou.Text = tempValueSTR;


                // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
                byte tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03);
                tempValueSTR = tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : ""));
                XTJS_Ti_DingWeiZhuangTai.Text = tempValueSTR;

                // bit6 0:GNSS修正无效 1：GNSS修正有效
                tempValueSTR = (GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效";
                XTJS_Ti_GNSSXiuZhengZhuangTai.Text = tempValueSTR;


                // 
                // PDOP 当量0.01
                // 
                XTJS_Ti_PDOP.Text = ((double)(sObject.PDOP)).ToString();

                // HDOP 当量0.01 
                XTJS_Ti_HDOP.Text = ((double)(sObject.HDOP)).ToString();

                // VDOP 当量0.01
                XTJS_Ti_VDOP.Text = ((double)(sObject.VDOP)).ToString();

                // 
                // GPS时间 单位s,UTC秒部
                XTJS_Ti_GPSTime.Text = ((double)(sObject.GPSTime * 0.1)).ToString();

                // 
                // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
                // 
                XTJS_Ti_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString();

                // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7 
                XTJS_Ti_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString();

                // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                XTJS_Ti_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString();

                //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
                XTJS_Ti_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString();

                //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                XTJS_Ti_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString();

                //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                XTJS_Ti_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString();

                // 
                // 轴向过载
                // 
                // 法向过载
                // 
                // 侧向过载
                // 
                XTJS_Ti_GuoZhai_ZhouXiang.Text = sObject.zhouXiangGuoZai.ToString();
                XTJS_Ti_GuoZhai_FaXiang.Text = sObject.faXiangGuoZai.ToString();
                XTJS_Ti_GuoZhai_CeXiang.Text = sObject.ceXiangGuoZai.ToString();

                // 
                // Wx角速度
                // 
                // Wy角速度
                // 
                // Wz角速度
                // 
                XTJS_Ti_JiaoSuDu_X.Text = sObject.WxJiaoSuDu.ToString();
                XTJS_Ti_JiaoSuDu_Y.Text = sObject.WyJiaoSuDu.ToString();
                XTJS_Ti_JiaoSuDu_Z.Text = sObject.WzJiaoSuDu.ToString();
            }
            else
            {
                StringBuilder s = new StringBuilder();
                // 
                // 故障标志位
                // 
                byte guZhangBiaoZhi = sObject.guZhangBiaoZhi;

                // bit0 陀螺x故障标志（0：正常；1：故障）
                s.Append((guZhangBiaoZhi & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // bit1 陀螺y故障标志（0：正常；1：故障）
                s.Append((guZhangBiaoZhi >> 1 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // bit2 陀螺z故障标志（0：正常；1：故障）
                s.Append((guZhangBiaoZhi >> 2 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // bit3 RS422故障标志（0：正常；1：故障） 
                s.Append((guZhangBiaoZhi >> 3 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // bit4 1553B故障标志（0：正常；1：故障）
                s.Append((guZhangBiaoZhi >> 4 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                //bit5 加计X故障标志（0：正常；1：故障）
                s.Append((guZhangBiaoZhi >> 5 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                //bit6 加计Y故障标志（0：正常；1：故障）
                s.Append((guZhangBiaoZhi >> 6 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                //bit7 加计Z故障标志（0：正常；1：故障）
                s.Append((guZhangBiaoZhi >> 7 & 0x1) == 0 ? "正常" : "故障");
                s.Append(",");

                // X陀螺温度
                // 
                // Y陀螺温度
                // 
                // Z陀螺温度
                // 
                s.Append(sObject.tuoLuoWenDu_X.ToString());
                s.Append(",");
                s.Append(sObject.tuoLuoWenDu_Y.ToString());
                s.Append(",");
                s.Append(sObject.tuoLuoWenDu_Z.ToString());
                s.Append(",");

                // 
                // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                // 
                byte tempValueGPS = sObject.GPS_SV;
                s.Append(((byte)(tempValueGPS & 0xF)).ToString());
                s.Append(",");
                s.Append(((byte)(tempValueGPS >> 4 & 0xF)).ToString());
                s.Append(",");



                // 
                // GPS定位模式
                // 
                byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi;
                string tempValueSTR = "";
                //bit0
                s.Append((GPSDingWeiMoShi & 0x01) == 1 ? "采用GPS定位" : "没有采用GPS定位");
                s.Append(",");

                // bit1 (1:采用BD2定位 0:没有采用BD2定位)
                s.Append((GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位");
                s.Append(",");

                //bit2
                s.Append((GPSDingWeiMoShi >> 2 & 0x01) == 1 ? "采用GLONASS定位" : "没有采用GLONASS定位");
                s.Append(",");

                //bit3
                s.Append((GPSDingWeiMoShi >> 3 & 0x01) == 1 ? "DGNSS可用" : "没有DGNSS可用");
                s.Append(",");

                // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
                byte tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03);
                tempValueSTR = tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : ""));
                s.Append(tempValueSTR);
                s.Append(",");

                // bit6 0:GNSS修正无效 1：GNSS修正有效
                tempValueSTR = (GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效";
                s.Append(tempValueSTR);
                s.Append(",");

                //bit7
                s.Append((GPSDingWeiMoShi >> 7 & 0x01) == 1 ? "BD2修正有效" : "BD2修正无效");
                s.Append(",");


                // 
                // PDOP 当量0.01
                // 
                s.Append(((double)(sObject.PDOP)).ToString());
                s.Append(",");

                // HDOP 当量0.01 
                s.Append(((double)(sObject.HDOP)).ToString());
                s.Append(",");

                // VDOP 当量0.01
                s.Append(((double)(sObject.VDOP)).ToString());
                s.Append(",");

                // 
                // GPS时间 单位s,UTC秒部
                s.Append(((double)(sObject.GPSTime * 0.1)).ToString());
                s.Append(",");

                // 
                // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
                // 
                s.Append(((double)(sObject.jingDu * Math.Pow(10, -7))).ToString());
                s.Append(",");

                // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7 
                s.Append(((double)(sObject.weiDu * Math.Pow(10, -7))).ToString());
                s.Append(",");

                // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                s.Append(((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                s.Append(((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString());
                s.Append(",");

                // 
                // 轴向过载
                // 
                // 法向过载
                // 
                // 侧向过载
                // 
                s.Append(sObject.zhouXiangGuoZai.ToString());
                s.Append(",");
                s.Append(sObject.faXiangGuoZai.ToString());
                s.Append(",");
                s.Append(sObject.ceXiangGuoZai.ToString());
                s.Append(",");

                // 
                // Wx角速度
                // 
                // Wy角速度
                // 
                // Wz角速度
                // 
                s.Append(sObject.WxJiaoSuDu.ToString());
                s.Append(",");
                s.Append(sObject.WyJiaoSuDu.ToString());
                s.Append(",");
                s.Append(sObject.WzJiaoSuDu.ToString());
                s.Append(",");

                //BD2SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                //
                byte tempVauleBD2 = sObject.BD2SV;
                s.Append(((byte)(tempVauleBD2 & 0xF)).ToString());
                s.Append(",");
                s.Append(((byte)(tempVauleBD2 >> 4 & 0xF)).ToString());
                s.Append(",");

                stringBuilders_XTJS.Add(s);
            }
        }

#if false
        //系统即时反馈弹头数据显示(弃用)
        private void showXiTongJiShiTimeStatus_Tou(ref SYSTEMImmediate_STATUS sObject)
        {
            // 
            // 故障标志位
            // 
            byte guZhangBiaoZhi = sObject.guZhangBiaoZhi;

            // bit0 陀螺x故障标志（0：正常；1：故障）
            int XTuoLuoGuZhang = (guZhangBiaoZhi & 0x1) == 0 ? 0 : 1;

            // bit1 陀螺y故障标志（0：正常；1：故障）
            int YTuoLuoGuZhang = (guZhangBiaoZhi >> 1 & 0x1) == 0 ? 0 : 1;

            // bit2 陀螺z故障标志（0：正常；1：故障）
            int ZTuoLuoGuZhang = (guZhangBiaoZhi >> 2 & 0x1) == 0 ? 0 : 1;

            // bit3 RS422故障标志（0：正常；1：故障） 
            int RS422GuZhang = (guZhangBiaoZhi >> 3 & 0x1) == 0 ? 0 : 1;

            // bit4 1553B故障标志（0：正常；1：故障）
            int _1553BGuZhang = (guZhangBiaoZhi >> 4 & 0x1) == 0 ? 0 : 1;

            //bit5 加计X故障标志（0：正常；1：故障）
            int JiaJiXGZ = (guZhangBiaoZhi >> 5 & 0x1) == 0 ? 0 : 1;

            //bit6 加计Y故障标志（0：正常；1：故障）
            int JiaJiYGZ = (guZhangBiaoZhi >> 6 & 0x1) == 0 ? 0 : 1;

            //bit7 加计Z故障标志（0：正常；1：故障）
            int JiaJiZGZ = (guZhangBiaoZhi >> 7 & 0x1) == 0 ? 0 : 1;

            StringBuilder guZhangBiaoZhiWei = new StringBuilder();
            guZhangBiaoZhiWei.Append("陀螺x故障标志：");
            guZhangBiaoZhiWei.Append(XTuoLuoGuZhang == 0 ? "正常;" : "故障;");
            guZhangBiaoZhiWei.Append("陀螺y故障标志：");
            guZhangBiaoZhiWei.Append(YTuoLuoGuZhang == 0 ? "正常;" : "故障;");
            guZhangBiaoZhiWei.Append("陀螺z故障标志：");
            guZhangBiaoZhiWei.Append(ZTuoLuoGuZhang == 0 ? "正常;\n" : "故障;\n");
            guZhangBiaoZhiWei.Append("RS422通信状态：");
            guZhangBiaoZhiWei.Append(RS422GuZhang == 0 ? "正常;" : "故障;");
            guZhangBiaoZhiWei.Append("1553B通信状态：");
            guZhangBiaoZhiWei.Append(_1553BGuZhang == 0 ? "正常;\n" : "故障;\n");
            guZhangBiaoZhiWei.Append("加计X故障标志：");
            guZhangBiaoZhiWei.Append(JiaJiXGZ == 0 ? "正常;" : "故障;");
            guZhangBiaoZhiWei.Append("加计Y故障标志：");
            guZhangBiaoZhiWei.Append(JiaJiYGZ == 0 ? "正常;" : "故障;");
            guZhangBiaoZhiWei.Append("加计Z故障标志：");
            guZhangBiaoZhiWei.Append(JiaJiZGZ == 0 ? "正常;" : "故障;");

            int GZBiaoZhi = XTuoLuoGuZhang + YTuoLuoGuZhang + ZTuoLuoGuZhang
                          + RS422GuZhang + _1553BGuZhang + JiaJiXGZ +
                          JiaJiYGZ + JiaJiZGZ;
            if (GZBiaoZhi == 0)
            {
                XTJS_Tou_GuZhangBiaoZhi.Text = "正常";
            }
            else
            {
                XTJS_Tou_GuZhangBiaoZhi.Text = "异常";
                ToolTip ttprogbar1 = new ToolTip();
                ttprogbar1.Content = guZhangBiaoZhiWei.ToString();
                XTJS_Tou_GuZhangBiaoZhi.ToolTip = ttprogbar1;
            }

            // 
            // X陀螺温度
            // 
            // Y陀螺温度
            // 
            // Z陀螺温度
            // 
            XTJS_Tou_XTuoLuoWenDu.Text = sObject.tuoLuoWenDu_X.ToString();
            XTJS_Tou_YTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Y.ToString();
            XTJS_Tou_ZTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Z.ToString();

            // 
            // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
            // 
            byte tempValueGPS = sObject.GPS_SV;
            XTJS_Tou_GPSSVKeYong.Text = ((byte)(tempValueGPS & 0xF)).ToString();
            XTJS_Tou_GPSCanYuDingWei.Text = ((byte)(tempValueGPS >> 4 & 0xF)).ToString();

            //BD2SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
            //
            byte tempVauleBD2 = sObject.BD2SV;
            XTJS_Tou_BD2SV.Text = ((byte)(tempVauleBD2 & 0xF)).ToString();
            XTJS_Tou_BD2CanYuDingWei.Text = ((byte)(tempVauleBD2 >> 4 & 0xF)).ToString();

            // 
            // GPS定位模式
            // 
            byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi;
            string tempValueSTR = "";

            // bit1 (1:采用BD2定位 0:没有采用BD2定位)
            tempValueSTR = (GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位";
            XTJS_Tou_ShiFouCaiYongBeiDou.Text = tempValueSTR;


            // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
            byte tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03);
            tempValueSTR = tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : ""));
            XTJS_Tou_DingWeiZhuangTai.Text = tempValueSTR;

            // bit6 0:GNSS修正无效 1：GNSS修正有效
            tempValueSTR = (GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效";
            XTJS_Tou_GNSSXiuZhengZhuangTai.Text = tempValueSTR;


            // 
            // PDOP 当量0.01
            // 
            XTJS_Tou_PDOP.Text = ((double)(sObject.PDOP)).ToString();

            // HDOP 当量0.01 
            XTJS_Tou_HDOP.Text = ((double)(sObject.HDOP)).ToString();

            // VDOP 当量0.01
            XTJS_Tou_VDOP.Text = ((double)(sObject.VDOP)).ToString();

            // 
            // GPS时间 单位s,UTC秒部
            XTJS_Tou_GPSTime.Text = ((double)(sObject.GPSTime * 0.1)).ToString();

            // 
            // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
            // 
            XTJS_Tou_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString();

            // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7 
            XTJS_Tou_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString();

            // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
            XTJS_Tou_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString();

            //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
            XTJS_Tou_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString();

            //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
            XTJS_Tou_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString();

            //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
            XTJS_Tou_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString();

            // 
            // 轴向过载
            // 
            // 法向过载
            // 
            // 侧向过载
            // 
            XTJS_Tou_GuoZhai_ZhouXiang.Text = sObject.zhouXiangGuoZai.ToString();
            XTJS_Tou_GuoZhai_FaXiang.Text = sObject.faXiangGuoZai.ToString();
            XTJS_Tou_GuoZhai_CeXiang.Text = sObject.ceXiangGuoZai.ToString();

            // 
            // Wx角速度
            // 
            // Wy角速度
            // 
            // Wz角速度
            // 
            XTJS_Tou_JiaoSuDu_X.Text = sObject.WxJiaoSuDu.ToString();
            XTJS_Tou_JiaoSuDu_Y.Text = sObject.WyJiaoSuDu.ToString();
            XTJS_Tou_JiaoSuDu_Z.Text = sObject.WzJiaoSuDu.ToString();
        }
#endif

        //弹头导航数据
        private void showDanTouDaoHangTimeStatus(ref DANTOUDAOHANGDATA sObject)
        {
            if (!dataConversion)
            {
                //GNSS时间
                danTouDaoHang_GNSSTime.Text = ((double)(sObject.GNSSTime * Math.Pow(10, -3))).ToString();

                //组合后经度
                danTouDaoHang_ZuHeJingDu.Text = ((double)(sObject.jingDu_ZuHe * Math.Pow(10, -7))).ToString();

                //组合后纬度
                danTouDaoHang_ZuHeWeiDu.Text = ((double)(sObject.weiDu_ZuHe * Math.Pow(10, -7))).ToString();

                //组合后高度
                danTouDaoHang_ZuHeGaoDu.Text = ((double)(sObject.gaoDu_ZuHe * Math.Pow(10, -2))).ToString();

                //组合后东向速度
                danTouDaoHang_ZuHeDong.Text = ((double)(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

                //组合后北向速度
                danTouDaoHang_ZuHeBei.Text = ((double)(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

                //组合后天向速度
                danTouDaoHang_ZuHeTian.Text = ((double)(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString();

                //GNSS经度
                danTouDaoHang_GNSSJingDu.Text = ((double)(sObject.jingDu_GNSS * Math.Pow(10, -7))).ToString();

                //GNSS纬度
                danTouDaoHang_GNSSWeiDu.Text = ((double)(sObject.weiDu_GNSS * Math.Pow(10, -7))).ToString();

                //GNSS高度
                danTouDaoHang_GNSSGaoDu.Text = ((double)(sObject.gaoDu_GNSS * Math.Pow(10, -2))).ToString();

                //GNSS东向速度
                danTouDaoHang_GNSSDong.Text = ((double)(sObject.dongXiangSuDu_GNSS * Math.Pow(10, -2))).ToString();

                //GNSS北向速度
                danTouDaoHang_GNSSBei.Text = ((double)(sObject.beiXiangSuDu_GNSS * Math.Pow(10, -2))).ToString();

                //GNSS天向速度
                danTouDaoHang_GNSSTian.Text = ((double)(sObject.tianXiangSuDu_GNSS * Math.Pow(10, -2))).ToString();

                //俯仰角
                danTouDaoHang_FuYangJiao.Text = sObject.fuYangJiao.ToString();

                //滚转角
                danTouDaoHang_GunZhuanJiao.Text = sObject.gunZhuanJiao.ToString();

                //偏航角
                danTouDaoHang_PianHangJiao.Text = sObject.pianHangJiao.ToString();

                //角速度Wx
                danTouDaoHang_Wx.Text = sObject.WxJiaoSuDu.ToString();

                //角速度Wy
                danTouDaoHang_Wy.Text = sObject.WyJiaoSuDu.ToString();

                //角速度Wz
                danTouDaoHang_Wz.Text = sObject.WzJiaoSuDu.ToString();

                //x比力
                danTouDaoHang_XBiLi.Text = sObject.xBiLi.ToString();

                //y比力
                danTouDaoHang_YBiLi.Text = sObject.yBiLi.ToString();

                //z比力
                danTouDaoHang_ZBiLi.Text = sObject.zBiLi.ToString();

                /* 修改时间：2021年4月15日18:58:13 
                 * 修改说明：HDOP、VDOP没有乘以当量 0.01：确认修改
                 */
                //HDOP
                danTouDaoHang_HDOP.Text = ((double)(sObject.HDOP * Math.Pow(10,-2))).ToString();

                //VDOP
                danTouDaoHang_VDOP.Text = ((double)(sObject.VDOP * Math.Pow(10, -2))).ToString();

                //可视卫星数
                danTouDaoHang_KeShiHuaWeiXingShu.Text = sObject.keShiWeiXingShu.ToString();

                //使用卫星数
                danTouDaoHang_ShiYongWeiXingShu.Text = sObject.shiYongWeiXingShu.ToString();

                //陀螺故障标识
                byte tuoLuoGuZhang = sObject.tuoLuoGuZhangBiaoShi;
                string tempSTR = String.Empty;
#if false
            //bit 0 陀螺故障标识 0正常
            danTouDaoHang_TuoLuoGuZhang.Text = (tuoLuoGuZhang >> 0 & 0x1) == 0 ? "正常" : "异常";
            //bit 1 加计故障标识 0正常
            danTouDaoHang_JiaJiGuZhang.Text = (tuoLuoGuZhang >> 1 & 0x1) == 0 ? "正常" : "异常";
            //bit 2 组合解算正常标识 0未组合 1组合
            danTouDaoHang_ZuHeJieSuan.Text = (tuoLuoGuZhang >> 2 & 0x1) == 0 ? "未组合" : "组合";
#endif
                switch (tuoLuoGuZhang)
                {
                    case 0:
                        tempSTR = "陀螺无效、加计无效、未组合";
                        break;
                    case 1:
                        tempSTR = "陀螺正常、加计正常、未组合";
                        break;
                    case 2:
                        tempSTR = "陀螺正常、加计正常、未组合";
                        break;
                    case 3:
                        tempSTR = "陀螺正常、加计正常、组合";
                        break;
                    case 4:
                        tempSTR = "陀螺异常、加计异常、未组合";
                        break;
                    default:
                        break;
                }
                danTouDaoHang_BiaoShiWei.Text = tempSTR;
            }
            else
            {
                StringBuilder s = new StringBuilder();
                //GNSS时间
                s.Append(((double)(sObject.GNSSTime * Math.Pow(10, -3))).ToString());
                s.Append(",");

                //组合后经度
                s.Append(((double)(sObject.jingDu_ZuHe * Math.Pow(10, -7))).ToString());
                s.Append(",");

                //组合后纬度
                s.Append(((double)(sObject.weiDu_ZuHe * Math.Pow(10, -7))).ToString());
                s.Append(",");

                //组合后高度
                s.Append(((double)(sObject.gaoDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //组合后东向速度
                s.Append(((double)(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //组合后北向速度
                s.Append(((double)(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //组合后天向速度
                s.Append(((double)(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //GNSS经度
                s.Append(((double)(sObject.jingDu_GNSS * Math.Pow(10, -7))).ToString());
                s.Append(",");

                //GNSS纬度
                s.Append(((double)(sObject.weiDu_GNSS * Math.Pow(10, -7))).ToString());
                s.Append(",");

                //GNSS高度
                s.Append(((double)(sObject.gaoDu_GNSS * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //GNSS东向速度
                s.Append(((double)(sObject.dongXiangSuDu_GNSS * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //GNSS北向速度
                s.Append(((double)(sObject.beiXiangSuDu_GNSS * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //GNSS天向速度
                s.Append(((double)(sObject.tianXiangSuDu_GNSS * Math.Pow(10, -2))).ToString());
                s.Append(",");

                //俯仰角
                s.Append(sObject.fuYangJiao.ToString());
                s.Append(",");

                //滚转角
                s.Append(sObject.gunZhuanJiao.ToString());
                s.Append(",");

                //偏航角
                s.Append(sObject.pianHangJiao.ToString());
                s.Append(",");

                //角速度Wx
                s.Append(sObject.WxJiaoSuDu.ToString());
                s.Append(",");

                //角速度Wy
                s.Append(sObject.WyJiaoSuDu.ToString());
                s.Append(",");

                //角速度Wz
                s.Append(sObject.WzJiaoSuDu.ToString());
                s.Append(",");

                //x比力
                s.Append(sObject.xBiLi.ToString());
                s.Append(",");

                //y比力
                s.Append(sObject.yBiLi.ToString());
                s.Append(",");

                //z比力
                s.Append(sObject.zBiLi.ToString());
                s.Append(",");

                /* 修改时间：2021年4月15日18:58:13 
                 * 修改说明：HDOP、VDOP没有乘以当量 0.01：确认修改
                 */
                //HDOP
                s.Append(((double)(sObject.HDOP * 0.01)).ToString());
                s.Append(",");

                //VDOP
                s.Append(((double)(sObject.VDOP * 0.01)).ToString());
                s.Append(",");

                //可视卫星数
                s.Append(sObject.keShiWeiXingShu.ToString());
                s.Append(",");

                //使用卫星数
                s.Append(sObject.shiYongWeiXingShu.ToString());
                s.Append(",");

                //陀螺故障标识
                byte tuoLuoGuZhang = sObject.tuoLuoGuZhangBiaoShi;
                string tempSTR = String.Empty;
#if false
            //bit 0 陀螺故障标识 0正常
            danTouDaoHang_TuoLuoGuZhang.Text = (tuoLuoGuZhang >> 0 & 0x1) == 0 ? "正常" : "异常";
            //bit 1 加计故障标识 0正常
            danTouDaoHang_JiaJiGuZhang.Text = (tuoLuoGuZhang >> 1 & 0x1) == 0 ? "正常" : "异常";
            //bit 2 组合解算正常标识 0未组合 1组合
            danTouDaoHang_ZuHeJieSuan.Text = (tuoLuoGuZhang >> 2 & 0x1) == 0 ? "未组合" : "组合";
#endif
                /*协议更改20210321 0："陀螺无效、加计无效、未组合" 
                 * 1："陀螺正常、加计正常、未组合" 
                 * 2："陀螺正常、加计正常、未组合"
                 * 3: "陀螺正常、加计正常、组合"
                 * 4: "陀螺异常、加计异常、未组合"
                 * */
                switch (tuoLuoGuZhang)
                {
                    case 0:
                        tempSTR = "陀螺无效、加计无效、未组合";
                        break;
                    case 1:
                        tempSTR = "陀螺正常、加计正常、未组合";
                        break;
                    case 2:
                        tempSTR = "陀螺正常、加计正常、未组合";
                        break;
                    case 3:
                        tempSTR = "陀螺正常、加计正常、组合";
                        break;
                    case 4:
                        tempSTR = "陀螺异常、加计异常、未组合";
                        break;
                    default:
                        break;
                }
                s.Append(tempSTR);
                s.Append(",");

                stringBuilders_DanTou.Add(s);
            }
        }

        //加载文件按钮
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            initYaoCeParser();

            if (load == null)
            {
                load = new LoadDataForm();
                load.setPlayStatus = setOffLineFilePlayStatus;
            }
            load.Show();
            load.Activate();
            return;
        }

        //初始化数据解析
        public void initYaoCeParser()
        {
            if (yaoceParserHigh == null)
            {
                yaoceParserHigh = new DataParser(new WindowInteropHelper(m).Handle, Protocol.Priority.HighPriority);
            }
            if (yaoceParserMiddle == null)
            {
                yaoceParserMiddle = new DataParser(new WindowInteropHelper(m).Handle, Protocol.Priority.MiddlePriority);
            }
            if (yaoceParserLow== null)
            {
                yaoceParserLow = new DataParser(new WindowInteropHelper(m).Handle, Protocol.Priority.LowPriority);
            }
            if (yaoceParserFile == null)
            {
                yaoceParserFile = new DataParser(new WindowInteropHelper(m).Handle, Protocol.Priority.HighPriority);
            }
        }

        //开始数据解析
        public void startYaoCeParser()
        {
            ParserStatus[Priority.HighPriority] = ParserStatus[Priority.MiddlePriority] = ParserStatus[Priority.LowPriority] = true;
            yaoceParserHigh.Start();
            yaoceParserMiddle.Start();
            yaoceParserLow.Start();
        }

        //停止数据解析
        public void stopYaoCeParser()
        {
            if (yaoceParserHigh != null)
            {
                yaoceParserHigh.Stop();
            }
            if (yaoceParserMiddle != null)
            {
                yaoceParserMiddle.Stop();
            }
            if (yaoceParserLow != null)
            {
                yaoceParserLow.Stop();
            }
        }

        //开始数据日志记录
        public void startYaoCeDataLogger()
        {
            yaoceDataLoggerHigh.Start();
            yaoceDataLoggerMiddle.Start();
            yaoceDataLoggerLow.Start();
        }

        //停止数据日志记录
        public void stopYaoCeDataLogger()
        {
            yaoceDataLoggerHigh.Stop();//
            yaoceDataLoggerMiddle.Stop();//
            yaoceDataLoggerLow.Stop();//
        }

        //初始化UDP套接字
        public void initYaoCeUdpClient(int portHigh, string ipAddrHigh, int portMiddle, string ipAddrMiddle,
            int portLow, string ipAddrLow, int idleTime)
        {
            udpClientYaoCeHigh = new UdpClient(portHigh);
            udpClientYaoCeHigh.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
            udpClientYaoCeHigh.JoinMulticastGroup(IPAddress.Parse(ipAddrHigh));
            udpClientYaoCeMiddle = new UdpClient(portMiddle);
            udpClientYaoCeMiddle.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
            udpClientYaoCeMiddle.JoinMulticastGroup(IPAddress.Parse(ipAddrMiddle));
            udpClientYaoCeLow = new UdpClient(portLow);
            udpClientYaoCeLow.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
            udpClientYaoCeLow.JoinMulticastGroup(IPAddress.Parse(ipAddrLow));

            yaoceParserHigh.IdleTimeout = yaoceParserMiddle.IdleTimeout = yaoceParserLow.IdleTimeout = idleTime;
            yaoceParserHigh.IdleHandler = yaoceParserMiddle.IdleHandler = yaoceParserLow.IdleHandler = IdleHandler;
        }

        private void IdleHandler(Priority priority, bool bActive)
        {
            ParserStatus[priority] = bActive;
            if(ParserStatus[Priority.HighPriority])
            {
                yaoceParserHigh.PostMessageEnable = true;
                yaoceParserMiddle.PostMessageEnable = yaoceParserLow.PostMessageEnable = false;
                return;
            }
            if (ParserStatus[Priority.MiddlePriority])
            {
                yaoceParserMiddle.PostMessageEnable = true;
                yaoceParserHigh.PostMessageEnable = yaoceParserLow.PostMessageEnable = false;
                return;
            }
            if(ParserStatus[Priority.LowPriority])
            {
                yaoceParserLow.PostMessageEnable = true;
                yaoceParserHigh.PostMessageEnable = yaoceParserMiddle.PostMessageEnable = false;
                return;
            }
        }

        //开启UDP接收
        public void startUDPReceive()
        {
            udpClientYaoCeHigh.BeginReceive(EndYaoCeReceive, udpClientYaoCeHigh);
            udpClientYaoCeMiddle.BeginReceive(EndYaoCeReceive, udpClientYaoCeMiddle);
            udpClientYaoCeLow.BeginReceive(EndYaoCeReceive, udpClientYaoCeLow);
        }

        //关闭UDP套接字
        public void closeYaoCeUdp()
        {
            udpClientYaoCeHigh?.Close();
            udpClientYaoCeMiddle?.Close();
            udpClientYaoCeLow?.Close();
        }

        //置空UDP套接字
        public void emptyYaoCeUdp()
        {
            udpClientYaoCeHigh = null;
            udpClientYaoCeMiddle = null;
            udpClientYaoCeLow = null;
        }

        private void EndYaoCeReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                UdpClient socket = (UdpClient)ar.AsyncState;
                byte[] recvBuffer = socket?.EndReceive(ar, ref endPoint);
                if (recvBuffer != null)
                {
                    if (socket == udpClientYaoCeHigh)
                    { 
                        yaoceParserHigh.Enqueue(recvBuffer);
                        yaoceDataLoggerHigh.Enqueue(recvBuffer);
                    }
                    else if (socket == udpClientYaoCeMiddle)
                    {
                        yaoceParserMiddle.Enqueue(recvBuffer);
                        yaoceDataLoggerMiddle.Enqueue(recvBuffer);
                    }
                    else if (socket == udpClientYaoCeLow)
                    {
                        yaoceParserLow.Enqueue(recvBuffer);
                        yaoceDataLoggerLow.Enqueue(recvBuffer);
                    }
                }
                socket?.BeginReceive(EndYaoCeReceive, socket);
            }
            catch (Exception)
            { }
        }

        public bool getLoadFormClose()
        {
            return load.IsLoaded;
        }

        //窗口关闭处理
        public void closeWindow(int msg, IntPtr wParam)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            // 捕捉关闭窗体消息(用户点击关闭窗体控制按钮) 
            if (msg == WM_SYSCOMMAND && (int)wParam == SC_CLOSE)
            {
                // 读取文件定时器
                readFileTimer.Stop();

                // 关闭日志文件
                Logger.GetInstance().closeFile();

                // 关闭码流记录
                yaoceDataLoggerHigh.Stop();
                yaoceDataLoggerMiddle.Stop();
                yaoceDataLoggerLow.Stop();

                // 关闭消息处理
                if (yaoceParserHigh != null)
                {
                    yaoceParserHigh.Stop();
                }
                if (yaoceParserMiddle != null)
                {
                    yaoceParserMiddle.Stop();
                }
                if (yaoceParserLow != null)
                {
                    yaoceParserLow.Stop();
                }
                if (yaoceParserFile != null)
                {
                    yaoceParserFile.Stop();
                }
                // 关闭绘图定时器刷新数据
                setTimerUpdateChartStatus(false); 

                // 停止加载文件进度
                UpdateLoadFileProgressTimer.Stop();


                Application.Current.Shutdown();


            }
        }

        //窗口消息处理
        public IntPtr handleMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_YAOCE_SystemStatus_DATA:
                    {
                        IntPtr ptr = lParam;
                        SYSTEMPARSE_STATUS sObject = Marshal.PtrToStructure<SYSTEMPARSE_STATUS>(ptr);

                        // 缓存状态数据
                        sObject_XiTong = sObject;
                        if (!dataConversion)
                        {
                            // 重新启动离线定时器
                            timerOffLineXiTongStatus.Stop();
                            timerOffLineXiTongStatus.Start();

                            // 是否收到数据
                            bRecvStatusData_XiTong = true;


                            // 添加系统坐标点集
                            double jingDu = Convert.ToDouble(sObject.jingDu * Math.Pow(10, -7));
                            double WeiDu = Convert.ToDouble(sObject.weiDu * Math.Pow(10, -7));
                            double GaoDu = Convert.ToDouble(sObject.haiBaGaoDu * Math.Pow(10, -2));
                            AddXiTongZuoBiao(jingDu, WeiDu, GaoDu);

                            // 添加系统速度点集
                            double dong = Convert.ToDouble(sObject.dongXiangSuDu * Math.Pow(10, -2));
                            double bei = Convert.ToDouble(sObject.beiXiangSuDu * Math.Pow(10, -2));
                            double tian = Convert.ToDouble(sObject.tianXiangSuDu * Math.Pow(10, -2));
                            AddXiTongSuDu(dong, bei, tian);

                            // 添加系统角速度点集
                            AddXiTongJiaoSuDu(sObject.WxJiaoSuDu, sObject.WyJiaoSuDu, sObject.WzJiaoSuDu);

                            // 添加系统发射系点集                                                          
                            AddXiTongFaSheXi(sObject.zhouXiangGuoZai, sObject.curFaSheXi_X, sObject.curFaSheXi_Y, sObject.curFaSheXi_Z);

                            // 添加系统预示落点点集                                                                                      
                            AddXiTongYuShiLuoDian(sObject.yuShiLuoDianSheCheng, sObject.yuShiLuoDianZ);
                        }
                        else
                        {
                            showSystemTimeStatus(ref sObject_XiTong);
                        }

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_daoHangKuaiSu_Ti_DATA:
                    {
                        IntPtr ptr = lParam;
                        DAOHANGSHUJU_KuaiSu sObject = Marshal.PtrToStructure<DAOHANGSHUJU_KuaiSu>(ptr);

                        // 缓存状态数据
                        sObject_DHK_Ti = sObject;
                        if(!dataConversion)
                        {
                            // 重新启动离线定时器
                            timerOfflineDHKStatus.Stop();
                            timerOfflineDHKStatus.Start();

                            // 是否收到数据
                            bRecvStatusData_DHK = true;

                            // 添加导航数据快速坐标点集
                            double jingDu = Convert.ToDouble(sObject.jingDu * Math.Pow(10, -7));
                            double WeiDu = Convert.ToDouble(sObject.weiDu * Math.Pow(10, -7));
                            double GaoDu = Convert.ToDouble(sObject.haiBaGaoDu * Math.Pow(10, -2));
                            AddDHKTiZuoBiao(jingDu, WeiDu, GaoDu);

                            // 添加导航数据快速速度点集
                            double dong = Convert.ToDouble(sObject.dongXiangSuDu * Math.Pow(10, -2));
                            double bei = Convert.ToDouble(sObject.beiXiangSuDu * Math.Pow(10, -2));
                            double tian = Convert.ToDouble(sObject.tianXiangSuDu * Math.Pow(10, -2));
                            AddDHKTiSuDu(dong, bei, tian);

                            //添加导航数据快速角度坐标集
                            double fuYang = Convert.ToDouble(sObject.fuYangJiao);
                            double gunZhuan = Convert.ToDouble(sObject.gunZhuanJiao);
                            double pianHang = Convert.ToDouble(sObject.pianHangJiao);
                            AddDHKTiJiaoDu(fuYang, gunZhuan, pianHang);

                            //添加导航数据快速陀螺坐标集
                            double x = Convert.ToDouble(sObject.tuoLuoShuJu_X);
                            double y = Convert.ToDouble(sObject.tuoLuoShuJu_Y);
                            double z = Convert.ToDouble(sObject.tuoLuoShuJu_Z);
                            AddDHKTiTuoLuo(x, y, z);

                            //添加导航数据快速加速度坐标集
                            double jiaJiX = Convert.ToDouble(sObject.jiaSuDuJiShuJu_X);
                            double jiaJiY = Convert.ToDouble(sObject.jiaSuDuJiShuJu_Y);
                            double jiaJiZ = Convert.ToDouble(sObject.jiaSuDuJiShuJu_Z);
                            AddDHKTiJiaSuDu(jiaJiX, jiaJiY, jiaJiZ);
                        }
                        else
                        {
                            showDHKuaiSuTimeStatus_Ti(ref sObject_DHK_Ti);
                        }

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                //弹头导航
                case WM_YAOCE_danTouDaoHang_DATA:
                    {
                        IntPtr ptr = lParam;
                        DANTOUDAOHANGDATA sObject = Marshal.PtrToStructure<DANTOUDAOHANGDATA>(ptr);

                        // 缓存状态数据
                        sObject_DanTou = sObject;
                        if(!dataConversion)
                        {
                            // 重新启动离线定时器
                            timerOfflineDHMStatus_Tou.Stop();
                            timerOfflineDHMStatus_Tou.Start();

                            // 是否收到数据
                            bReceStatusData_DANTOU = true;

                            // 添加坐标点集
                            double jingDu_ZuHe = Convert.ToDouble(sObject.jingDu_ZuHe * Math.Pow(10, -7));
                            double WeiDu_ZuHe = Convert.ToDouble(sObject.weiDu_ZuHe * Math.Pow(10, -7));
                            double GaoDu_ZuHe = Convert.ToDouble(sObject.gaoDu_ZuHe * Math.Pow(10, -2));
                            AddDANTOUZuoBiaoZuHe(jingDu_ZuHe, WeiDu_ZuHe, GaoDu_ZuHe);

                            // 添加速度点集
                            double dong_ZuHe = Convert.ToDouble(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2));
                            double bei_ZuHe = Convert.ToDouble(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2));
                            double tian_ZuHe = Convert.ToDouble(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2));
                            AddDANTOUSuDuZuHe(dong_ZuHe, bei_ZuHe, tian_ZuHe);

                            // 添加坐标点集
                            double jingDu_GNSS = Convert.ToDouble(sObject.jingDu_GNSS * Math.Pow(10, -7));
                            double WeiDu_GNSS = Convert.ToDouble(sObject.weiDu_GNSS * Math.Pow(10, -7));
                            double GaoDu_GNSS = Convert.ToDouble(sObject.gaoDu_GNSS * Math.Pow(10, -2));
                            AddDANTOUZuoBiaoGNSS(jingDu_GNSS, WeiDu_GNSS, GaoDu_GNSS);

                            // 添加速度点集
                            double dong_GNSS = Convert.ToDouble(sObject.dongXiangSuDu_GNSS * Math.Pow(10, -2));
                            double bei_GNSS = Convert.ToDouble(sObject.beiXiangSuDu_GNSS * Math.Pow(10, -2));
                            double tian_GNSS = Convert.ToDouble(sObject.tianXiangSuDu_GNSS * Math.Pow(10, -2));
                            AddDANTOUSuDuGNSS(dong_GNSS, bei_GNSS, tian_GNSS);

                            //添加角度
                            double fuYang = Convert.ToDouble(sObject.fuYangJiao);
                            double gunZhuan = Convert.ToDouble(sObject.gunZhuanJiao);
                            double pianHang = Convert.ToDouble(sObject.pianHangJiao);
                            AddDANTOUJiaoDu(fuYang, gunZhuan, pianHang);

                            AddDANTOUJiaoSuDu(sObject.WxJiaoSuDu, sObject.WyJiaoSuDu, sObject.WzJiaoSuDu);
                            AddDANTOUBiLi(sObject.xBiLi, sObject.yBiLi, sObject.zBiLi);
                        }
                        else
                        {
                            showDanTouDaoHangTimeStatus(ref sObject_DanTou);
                        }
                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_daoHangManSu_Ti_DATA:
                    {
                        IntPtr ptr = lParam;
                        DAOHANGSHUJU_ManSu sObject = Marshal.PtrToStructure<DAOHANGSHUJU_ManSu>(ptr);

                        // 缓存状态数据
                        sObject_DHM_Ti = sObject;
                        if(!dataConversion)
                        {
                            // 重新启动离线定时器
                            timerOfflineDHMStatus.Stop();
                            timerOfflineDHMStatus.Start();

                            // 是否收到数据
                            bRecvStatusData_DHM = true;
                            bRecvStatusData_DHM_Ti = true;

                            // 添加导航数据慢速坐标点集
                            double jingDu = Convert.ToDouble(sObject.jingDu * Math.Pow(10, -7));
                            double WeiDu = Convert.ToDouble(sObject.weiDu * Math.Pow(10, -7));
                            double GaoDu = Convert.ToDouble(sObject.haiBaGaoDu * Math.Pow(10, -2));
                            AddDHMTiZuoBiao(jingDu, WeiDu, GaoDu);

                            // 添加导航数据慢速速度点集
                            double dong = Convert.ToDouble(sObject.dongXiangSuDu * Math.Pow(10, -2));
                            double bei = Convert.ToDouble(sObject.beiXiangSuDu * Math.Pow(10, -2));
                            double tian = Convert.ToDouble(sObject.tianXiangSuDu * Math.Pow(10, -2));
                            AddDHMTiSuDu(dong, bei, tian);

                            //添加导航数据慢速坐标（组合结果）点集
                            double jingDuZH = Convert.ToDouble(sObject.jingDu_ZuHe * Math.Pow(10, -7));
                            double weiDuZH = Convert.ToDouble(sObject.weiDu_ZuHe * Math.Pow(10, -7));
                            double gaoDuZH = Convert.ToDouble(sObject.haiBaGaoDu_ZuHe * Math.Pow(10, -2));
                            AddDHMTiZuoBiaoZuHe(jingDuZH, weiDuZH, gaoDuZH);

                            //添加导航数据慢速速度（组合结果）点集
                            double dongZH = Convert.ToDouble(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2));
                            double beiZH = Convert.ToDouble(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2));
                            double tianZH = Convert.ToDouble(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2));
                            AddDHMTiSuDuZuHe(dongZH, beiZH, tianZH);

                            //添加导航数据慢速角度坐标集
                            double fuYang = Convert.ToDouble(sObject.fuYangJiao);
                            double gunZhuan = Convert.ToDouble(sObject.gunZhuanJiao);
                            double pianHang = Convert.ToDouble(sObject.pianHangJiao);
                            AddDHMTiJiaoDu(fuYang, gunZhuan, pianHang);

                            //添加导航数据慢速陀螺坐标集
                            double x = Convert.ToDouble(sObject.tuoLuoShuJu_X);
                            double y = Convert.ToDouble(sObject.tuoLuoShuJu_Y);
                            double z = Convert.ToDouble(sObject.tuoLuoShuJu_Z);
                            AddDHMTiTuoLuo(x, y, z);

                            //添加导航数据慢速加速度坐标集
                            double jiaJiX = Convert.ToDouble(sObject.jiaSuDuJiShuJu_X);
                            double jiaJiY = Convert.ToDouble(sObject.jiaSuDuJiShuJu_Y);
                            double jiaJiZ = Convert.ToDouble(sObject.jiaSuDuJiShuJu_Z);
                            AddDHMTiJiaSuDu(jiaJiX, jiaJiY, jiaJiZ);
                        }
                        else
                        {
                            showDHManSuTimeStatus_Ti(ref sObject_DHM_Ti);
                        }

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_daoHangManSu_Tou_DATA:
                    {
                        IntPtr ptr = lParam;
                        DAOHANGSHUJU_ManSu sObject = Marshal.PtrToStructure<DAOHANGSHUJU_ManSu>(ptr);

                        // 缓存状态数据
                        sObject_DHM_Tou = sObject;

                        // 重新启动离线定时器
                        timerOfflineDHMStatus_Tou.Stop();
                        timerOfflineDHMStatus_Tou.Start();

                        // 是否收到数据
                        bRecvStatusData_DHM = true;
                        bRecvStatusData_DHM_Tou = true;

                        // 添加导航数据慢速坐标点集
                        double jingDu = Convert.ToDouble(sObject.jingDu * Math.Pow(10, -7));
                        double WeiDu = Convert.ToDouble(sObject.weiDu * Math.Pow(10, -7));
                        double GaoDu = Convert.ToDouble(sObject.haiBaGaoDu * Math.Pow(10, -2));
                        AddDHMTouZuoBiao(jingDu, WeiDu, GaoDu);

                        // 添加导航数据慢速速度点集                                                         
                        double dong = Convert.ToDouble(sObject.dongXiangSuDu * Math.Pow(10, -2));
                        double bei = Convert.ToDouble(sObject.beiXiangSuDu * Math.Pow(10, -2));
                        double tian = Convert.ToDouble(sObject.tianXiangSuDu * Math.Pow(10, -2));
                        AddDHMTouSuDu(dong, bei, tian);

                        //添加导航数据慢速坐标（组合结果）点集
                        double jingDuZH = Convert.ToDouble(sObject.jingDu_ZuHe * Math.Pow(10, -7));
                        double weiDuZH = Convert.ToDouble(sObject.weiDu_ZuHe * Math.Pow(10, -7));
                        double gaoDuZH = Convert.ToDouble(sObject.haiBaGaoDu_ZuHe * Math.Pow(10, -2));
                        AddDHMTouZuoBiaoZuHe(jingDuZH, weiDuZH, gaoDuZH);

                        //添加导航数据慢速速度（组合结果）点集
                        double dongZH = Convert.ToDouble(sObject.dongXiangSuDu_ZuHe * Math.Pow(10, -2));
                        double beiZH = Convert.ToDouble(sObject.beiXiangSuDu_ZuHe * Math.Pow(10, -2));
                        double tianZH = Convert.ToDouble(sObject.tianXiangSuDu_ZuHe * Math.Pow(10, -2));
                        AddDHMTouSuDuZuHe(dongZH, beiZH, tianZH);

                        //添加导航数据慢速角度坐标集
                        double fuYang = Convert.ToDouble(sObject.fuYangJiao);
                        double gunZhuan = Convert.ToDouble(sObject.gunZhuanJiao);
                        double pianHang = Convert.ToDouble(sObject.pianHangJiao);
                        AddDHMTiJiaoDu(fuYang, gunZhuan, pianHang);

                        //添加导航数据慢速陀螺坐标集
                        double x = Convert.ToDouble(sObject.tuoLuoShuJu_X);
                        double y = Convert.ToDouble(sObject.tuoLuoShuJu_Y);
                        double z = Convert.ToDouble(sObject.tuoLuoShuJu_Z);
                        AddDHMTouTuoLuo(x, y, z);

                        //添加导航数据慢速加速度坐标集
                        double jiaJiX = Convert.ToDouble(sObject.jiaSuDuJiShuJu_X);
                        double jiaJiY = Convert.ToDouble(sObject.jiaSuDuJiShuJu_Y);
                        double jiaJiZ = Convert.ToDouble(sObject.jiaSuDuJiShuJu_Z);
                        AddDHMTouJiaSuDu(jiaJiX, jiaJiY, jiaJiZ);

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_HuiLuJianCe_DATA:
                    {
                        IntPtr ptr = lParam;
                        HUILUJIANCE_STATUS sObject = Marshal.PtrToStructure<HUILUJIANCE_STATUS>(ptr);

                        // 缓存状态数据
                        sObject_huiLuJianCe = sObject;

                        // 重新启动离线定时器
                        UpdateHuiLuJianCeTimer.Stop();
                        UpdateHuiLuJianCeTimer.Start();

                        // 是否收到数据
                        bRecvStatusData_HuiLuJianCe = true;


                        Marshal.FreeHGlobal(ptr);
                    }
                    break;

                // TODO 20200219 新增
                case WM_YAOCE_XiTongJiShi_Ti_DATA:
                    {
                        IntPtr ptr = lParam;
                        SYSTEMImmediate_STATUS sObject = Marshal.PtrToStructure<SYSTEMImmediate_STATUS>(ptr);

                        // 缓存状态数据
                        sObject_XTJS_Ti = sObject;
                        if (!dataConversion)
                        {
                            //重新启动离线定时器
                            timerOfflineXiTongJiShiStatus.Stop();
                            timerOfflineXiTongJiShiStatus.Start();

                            //是否接收数据
                            bRecvStatusData_XTJS = true;
                            bRecvStatusData_XTJS_Ti = true;

                            double jingDu = Convert.ToDouble(sObject.jingDu * Math.Pow(10, -7));
                            double WeiDu = Convert.ToDouble(sObject.weiDu * Math.Pow(10, -7));
                            double GaoDu = Convert.ToDouble(sObject.haiBaGaoDu * Math.Pow(10, -2));
                            double dong = Convert.ToDouble(sObject.dongXiangSuDu * Math.Pow(10, -2));
                            double bei = Convert.ToDouble(sObject.beiXiangSuDu * Math.Pow(10, -2));
                            double tian = Convert.ToDouble(sObject.tianXiangSuDu * Math.Pow(10, -2));

                            AddXTJSTiZuoBiao(jingDu, WeiDu, GaoDu); //
                            AddXTJSTiSuDu(dong, bei, tian); //
                            AddXTJSTiJiaoSuDu(sObject.WxJiaoSuDu, sObject.WyJiaoSuDu, sObject.WzJiaoSuDu); //
                            AddXTJSTiGuoZai(sObject.zhouXiangGuoZai, sObject.faXiangGuoZai, sObject.ceXiangGuoZai); //
                        }
                        else
                        {
                            showXiTongJiShiTimeStatus_Ti(ref sObject_XTJS_Ti);
                        }

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_XiTongJiShi_Tou_DATA:
                    {
                        IntPtr ptr = lParam;
                        SYSTEMImmediate_STATUS sObject = Marshal.PtrToStructure<SYSTEMImmediate_STATUS>(ptr);

                        // 缓存状态数据
                        sObject_XTJS_Tou = sObject;

                        //重新启动离线定时器
                        timerOfflineXiTongJiShiStatus.Stop();
                        timerOfflineXiTongJiShiStatus.Start();

                        //是否接收数据
                        bRecvStatusData_XTJS = true;
                        bRecvStatusData_XTJS_Tou = true;

                        double jingDu = Convert.ToDouble(sObject.jingDu * Math.Pow(10, -7));
                        double WeiDu = Convert.ToDouble(sObject.weiDu * Math.Pow(10, -7));
                        double GaoDu = Convert.ToDouble(sObject.haiBaGaoDu * Math.Pow(10, -2));
                        double dong = Convert.ToDouble(sObject.dongXiangSuDu * Math.Pow(10, -2));
                        double bei = Convert.ToDouble(sObject.beiXiangSuDu * Math.Pow(10, -2));
                        double tian = Convert.ToDouble(sObject.tianXiangSuDu * Math.Pow(10, -2));

                        AddXTJSTouZuoBiao(jingDu, WeiDu, GaoDu); //
                        AddXTJSTouSuDu(dong, bei, tian); //
                        AddXTJSTouJiaoSuDu(sObject.WxJiaoSuDu, sObject.WyJiaoSuDu, sObject.WzJiaoSuDu); //
                        AddXTJSTouGuoZai(sObject.zhouXiangGuoZai, sObject.faXiangGuoZai, sObject.ceXiangGuoZai); //

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_FRAMEPROPERTY_DATA:
                    {
                        IntPtr ptr = lParam;
                        FRAME_PROPERTY sObject = Marshal.PtrToStructure<FRAME_PROPERTY>(ptr);

                        // 缓存状态数据
                        this.AddFRAMEINFO(ref sObject);

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_UDPPROPERTY_DATA:
                    {
                        IntPtr ptr = lParam;
                        UDP_PROPERTY sObject = Marshal.PtrToStructure<UDP_PROPERTY>(ptr);

                        // 重新启动离线定时器
                        timerOffLineUDP.Stop();
                        timerOffLineUDP.Start();

                        // 更改状态灯颜色 
                        bRecvStatusData_UDP = true;
                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                default:
                    return hwnd;
            }
            return hwnd;
        }

        private void AddXiTongZuoBiao(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.XiTong_ZuoBiao_JingDu_buffer.Add(JingDu);
            yaoceDisplay.XiTong_ZuoBiao_WeiDu_buffer.Add(WeiDu);
            yaoceDisplay.XiTong_ZuoBiao_GaoDu_buffer.Add(GaoDu);
        }

        private void AddXiTongSuDu(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.XiTong_SuDu_Dong_buffer.Add(Dong);
            yaoceDisplay.XiTong_SuDu_Bei_buffer.Add(Bei);
            yaoceDisplay.XiTong_SuDu_Tian_buffer.Add(Tian);
        }

        private void AddXiTongJiaoSuDu(double Wx, double Wy, double Wz)
        {
            yaoceDisplay.XiTong_JiaoSuDu_Wx_buffer.Add(Wx);
            yaoceDisplay.XiTong_JiaoSuDu_Wy_buffer.Add(Wy);
            yaoceDisplay.XiTong_JiaoSuDu_Wz_buffer.Add(Wz);

        }

        private void AddXiTongFaSheXi(double ZXGZ, double X, double Y, double Z)
        {
            yaoceDisplay.XiTong_FaSheXi_ZXGZ_buffer.Add(ZXGZ);
            yaoceDisplay.XiTong_FaSheXi_X_buffer.Add(X);
            yaoceDisplay.XiTong_FaSheXi_Y_buffer.Add(Y);
            yaoceDisplay.XiTong_FaSheXi_Z_buffer.Add(Z);
        }

        private void AddXiTongYuShiLuoDian(double SheCheng, double Z)
        {
            yaoceDisplay.XiTong_YuShiLuoDian_SheCheng_buffer.Add(SheCheng);
            yaoceDisplay.XiTong_YuShiLuoDian_Z_buffer.Add(Z);

        }

        private void AddDHKTiZuoBiao(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DHKuaiSu_Ti_ZuoBiao_JingDu_buffer.Add(JingDu);
            yaoceDisplay.DHKuaiSu_Ti_ZuoBiao_WeiDu_buffer.Add(WeiDu);
            yaoceDisplay.DHKuaiSu_Ti_ZuoBiao_GaoDu_buffer.Add(GaoDu);
        }

        private void AddDHKTiSuDu(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DHKuaiSu_Ti_SuDu_Dong_buffer.Add(Dong);
            yaoceDisplay.DHKuaiSu_Ti_SuDu_Bei_buffer.Add(Bei);
            yaoceDisplay.DHKuaiSu_Ti_SuDu_Tian_buffer.Add(Tian);
        }

        private void AddDHKTouZuoBiao(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DHKuaiSu_Tou_ZuoBiao_JingDu_buffer.Add(JingDu);
            yaoceDisplay.DHKuaiSu_Tou_ZuoBiao_WeiDu_buffer.Add(WeiDu);
            yaoceDisplay.DHKuaiSu_Tou_ZuoBiao_GaoDu_buffer.Add(GaoDu);
        }

        private void AddDHKTouSuDu(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DHKuaiSu_Tou_SuDu_Dong_buffer.Add(Dong);
            yaoceDisplay.DHKuaiSu_Tou_SuDu_Bei_buffer.Add(Bei);
            yaoceDisplay.DHKuaiSu_Tou_SuDu_Tian_buffer.Add(Tian);
        }

        private void AddDHKTiJiaoDu(double FuYang, double GunZhuan, double PianHang)
        {
            yaoceDisplay.DHKuaiSu_Ti_JiaoDu_FuYangJiao_buffer.Add(FuYang);
            yaoceDisplay.DHKuaiSu_Ti_JiaoDu_GunZhuanJiao_buffer.Add(GunZhuan);
            yaoceDisplay.DHKuaiSu_Ti_JiaoDu_PianHangJiao_buffer.Add(PianHang);
        }

        private void AddDHKTouJiaoDu(double FuYang, double GunZhuan, double PianHang)
        {
            yaoceDisplay.DHKuaiSu_Tou_JiaoDu_FuYangJiao_buffer.Add(FuYang);
            yaoceDisplay.DHKuaiSu_Tou_JiaoDu_GunZhuanJiao_buffer.Add(GunZhuan);
            yaoceDisplay.DHKuaiSu_Tou_JiaoDu_PianHangJiao_buffer.Add(PianHang);
        }

        private void AddDHKTiTuoLuo(double X, double Y, double Z)
        {
            yaoceDisplay.DHKuaiSu_Ti_TuoLuo_TuoLuoX_buffer.Add(X);
            yaoceDisplay.DHKuaiSu_Ti_TuoLuo_TuoLuoY_buffer.Add(Y);
            yaoceDisplay.DHKuaiSu_Ti_TuoLuo_TuoLuoZ_buffer.Add(Z);
        }

        private void AddDHKTouTuoLuo(double X, double Y, double Z)
        {
            yaoceDisplay.DHKuaiSu_Tou_TuoLuo_TuoLuoX_buffer.Add(X);
            yaoceDisplay.DHKuaiSu_Tou_TuoLuo_TuoLuoY_buffer.Add(Y);
            yaoceDisplay.DHKuaiSu_Tou_TuoLuo_TuoLuoZ_buffer.Add(Z);
        }

        private void AddDHKTiJiaSuDu(double X, double Y, double Z)
        {
            yaoceDisplay.DHKuaiSu_Ti_JiaSuDu_JiaJiX_buffer.Add(X);
            yaoceDisplay.DHKuaiSu_Ti_JiaSuDu_JiaJiY_buffer.Add(Y);
            yaoceDisplay.DHKuaiSu_Ti_JiaSuDu_JiaJiZ_buffer.Add(Z);
        }

        private void AddDHKTouJiaSuDu(double X, double Y, double Z)
        {
            yaoceDisplay.DHKuaiSu_Tou_JiaSuDu_JiaJiX_buffer.Add(X);
            yaoceDisplay.DHKuaiSu_Tou_JiaSuDu_JiaJiY_buffer.Add(Y);
            yaoceDisplay.DHKuaiSu_Tou_JiaSuDu_JiaJiZ_buffer.Add(Z);
        }

        private void AddDHMTiZuoBiao(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DHManSu_Ti_ZuoBiao_JingDu_buffer.Add(JingDu);
            yaoceDisplay.DHManSu_Ti_ZuoBiao_WeiDu_buffer.Add(WeiDu);
            yaoceDisplay.DHManSu_Ti_ZuoBiao_GaoDu_buffer.Add(GaoDu);
        }

        private void AddDHMTiSuDu(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DHManSu_Ti_SuDu_Dong_buffer.Add(Dong);
            yaoceDisplay.DHManSu_Ti_SuDu_Bei_buffer.Add(Bei);
            yaoceDisplay.DHManSu_Ti_SuDu_Tian_buffer.Add(Tian);
        }

        private void AddDHMTouZuoBiao(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DHManSu_Tou_ZuoBiao_JingDu_buffer.Add(JingDu);
            yaoceDisplay.DHManSu_Tou_ZuoBiao_WeiDu_buffer.Add(WeiDu);
            yaoceDisplay.DHManSu_Tou_ZuoBiao_GaoDu_buffer.Add(GaoDu);
        }

        private void AddDHMTouSuDu(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DHManSu_Tou_SuDu_Dong_buffer.Add(Dong);
            yaoceDisplay.DHManSu_Tou_SuDu_Bei_buffer.Add(Bei);
            yaoceDisplay.DHManSu_Tou_SuDu_Tian_buffer.Add(Tian);
        }

        private void AddDHMTiZuoBiaoZuHe(double jingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DHManSu_Ti_ZuoBiao_JingDuZuHe_buffer.Add(jingDu);
            yaoceDisplay.DHManSu_Ti_ZuoBiao_WeiDuZuHe_buffer.Add(WeiDu);
            yaoceDisplay.DHManSu_Ti_ZuoBiao_GaoDuZuHe_buffer.Add(GaoDu);
        }

        private void AddDHMTouZuoBiaoZuHe(double jingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DHManSu_Tou_ZuoBiao_JingDuZuHe_buffer.Add(jingDu);
            yaoceDisplay.DHManSu_Tou_ZuoBiao_WeiDuZuHe_buffer.Add(WeiDu);
            yaoceDisplay.DHManSu_Tou_ZuoBiao_GaoDuZuHe_buffer.Add(GaoDu);
        }

        private void AddDHMTiSuDuZuHe(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DHManSu_Ti_SuDu_DongZuHe_buffer.Add(Dong);
            yaoceDisplay.DHManSu_Ti_SuDu_BeiZuHe_buffer.Add(Bei);
            yaoceDisplay.DHManSu_Ti_SuDu_TianZuHe_buffer.Add(Tian);
        }

        private void AddDHMTouSuDuZuHe(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DHManSu_Tou_SuDu_DongZuHe_buffer.Add(Dong);
            yaoceDisplay.DHManSu_Tou_SuDu_BeiZuHe_buffer.Add(Bei);
            yaoceDisplay.DHManSu_Tou_SuDu_TianZuHe_buffer.Add(Tian);
        }

        private void AddDHMTiJiaoDu(double FuYang, double GunZhuan, double PianHang)
        {
            yaoceDisplay.DHManSu_Ti_JiaoDu_FuYangJiao_buffer.Add(FuYang);
            yaoceDisplay.DHManSu_Ti_JiaoDu_GunZhuanJiao_buffer.Add(GunZhuan);
            yaoceDisplay.DHManSu_Ti_JiaoDu_PianHangJiao_buffer.Add(PianHang);
        }

        private void AddDHMTouJiaoDu(double FuYang, double GunZhuan, double PianHang)
        {
            yaoceDisplay.DHManSu_Tou_JiaoDu_FuYangJiao_buffer.Add(FuYang);
            yaoceDisplay.DHManSu_Tou_JiaoDu_GunZhuanJiao_buffer.Add(GunZhuan);
            yaoceDisplay.DHManSu_Tou_JiaoDu_PianHangJiao_buffer.Add(PianHang);
        }

        private void AddDHMTiTuoLuo(double X, double Y, double Z)
        {
            yaoceDisplay.DHManSu_Ti_TuoLuo_TuoLuoX_buffer.Add(X);
            yaoceDisplay.DHManSu_Ti_TuoLuo_TuoLuoY_buffer.Add(Y);
            yaoceDisplay.DHManSu_Ti_TuoLuo_TuoLuoZ_buffer.Add(Z);
        }

        private void AddDHMTouTuoLuo(double X, double Y, double Z)
        {
            yaoceDisplay.DHManSu_Tou_TuoLuo_TuoLuoX_buffer.Add(X);
            yaoceDisplay.DHManSu_Tou_TuoLuo_TuoLuoY_buffer.Add(Y);
            yaoceDisplay.DHManSu_Tou_TuoLuo_TuoLuoZ_buffer.Add(Z);
        }

        private void AddDHMTiJiaSuDu(double X, double Y, double Z)
        {
            yaoceDisplay.DHManSu_Ti_JiaSuDu_JiaJiX_buffer.Add(X);
            yaoceDisplay.DHManSu_Ti_JiaSuDu_JiaJiY_buffer.Add(Y);
            yaoceDisplay.DHManSu_Ti_JiaSuDu_JiaJiZ_buffer.Add(Z);
        }

        private void AddDHMTouJiaSuDu(double X, double Y, double Z)
        {
            yaoceDisplay.DHManSu_Tou_JiaSuDu_JiaJiX_buffer.Add(X);
            yaoceDisplay.DHManSu_Tou_JiaSuDu_JiaJiY_buffer.Add(Y);
            yaoceDisplay.DHManSu_Tou_JiaSuDu_JiaJiZ_buffer.Add(Z);
        }

        private void AddXTJSTiZuoBiao(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.XTJS_Ti_ZuoBiao_JingDu_buffer.Add(JingDu);
            yaoceDisplay.XTJS_Ti_ZuoBiao_WeiDu_buffer.Add(WeiDu);
            yaoceDisplay.XTJS_Ti_ZuoBiao_GaoDu_buffer.Add(GaoDu);
        }

        private void AddXTJSTiSuDu(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.XTJS_Ti_SuDu_Dong_buffer.Add(Dong);
            yaoceDisplay.XTJS_Ti_SuDu_Bei_buffer.Add(Bei);
            yaoceDisplay.XTJS_Ti_SuDu_Tian_buffer.Add(Tian);
        }

        private void AddXTJSTiJiaoSuDu(double Wx, double Wy, double Wz)
        {
            yaoceDisplay.XTJS_Ti_JiaoSuDu_Wx_buffer.Add(Wx);
            yaoceDisplay.XTJS_Ti_JiaoSuDu_Wy_buffer.Add(Wy);
            yaoceDisplay.XTJS_Ti_JiaoSuDu_Wz_buffer.Add(Wz);
        }

        private void AddXTJSTiGuoZai(double ZX, double FX, double CX)
        {
            yaoceDisplay.XTJS_Ti_GuoZai_ZhouXiang_buffer.Add(ZX);
            yaoceDisplay.XTJS_Ti_GuoZai_FaXiang_buffer.Add(FX);
            yaoceDisplay.XTJS_Ti_GuoZai_CeXiang_buffer.Add(CX);
        }

        private void AddXTJSTouZuoBiao(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.XTJS_Tou_ZuoBiao_JingDu_buffer.Add(JingDu);
            yaoceDisplay.XTJS_Tou_ZuoBiao_WeiDu_buffer.Add(WeiDu);
            yaoceDisplay.XTJS_Tou_ZuoBiao_GaoDu_buffer.Add(GaoDu);
        }

        private void AddXTJSTouSuDu(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.XTJS_Tou_SuDu_Dong_buffer.Add(Dong);
            yaoceDisplay.XTJS_Tou_SuDu_Bei_buffer.Add(Bei);
            yaoceDisplay.XTJS_Tou_SuDu_Tian_buffer.Add(Tian);
        }

        private void AddXTJSTouJiaoSuDu(double Wx, double Wy, double Wz)
        {
            yaoceDisplay.XTJS_Tou_JiaoSuDu_Wx_buffer.Add(Wx);
            yaoceDisplay.XTJS_Tou_JiaoSuDu_Wy_buffer.Add(Wy);
            yaoceDisplay.XTJS_Tou_JiaoSuDu_Wz_buffer.Add(Wz);
        }

        private void AddXTJSTouGuoZai(double ZX, double FX, double CX)
        {
            yaoceDisplay.XTJS_Tou_GuoZai_ZhouXiang_buffer.Add(ZX);
            yaoceDisplay.XTJS_Tou_GuoZai_FaXiang_buffer.Add(FX);
            yaoceDisplay.XTJS_Tou_GuoZai_CeXiang_buffer.Add(CX);
        }

        private void AddDANTOUZuoBiaoZuHe(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DanTou_ZuHeJingDu_buffer.Add(JingDu);
            yaoceDisplay.DanTou_ZuHeWeiDu_buffer.Add(WeiDu);
            yaoceDisplay.DanTou_ZuHeGaoDu_buffer.Add(GaoDu);
        }

        private void AddDANTOUZuoBiaoGNSS(double JingDu, double WeiDu, double GaoDu)
        {
            yaoceDisplay.DanTou_GNSSJingDu_buffer.Add(JingDu);
            yaoceDisplay.DanTou_GNSSWeiDu_buffer.Add(WeiDu);
            yaoceDisplay.DanTou_GNSSGaoDu_buffer.Add(GaoDu);
        }

        private void AddDANTOUSuDuZuHe(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DanTou_ZuHeDong_buffer.Add(Dong);
            yaoceDisplay.DanTou_ZuHeBei_buffer.Add(Bei);
            yaoceDisplay.DanTou_ZuHeTian_buffer.Add(Tian);
        }

        private void AddDANTOUSuDuGNSS(double Dong, double Bei, double Tian)
        {
            yaoceDisplay.DanTou_GNSSDong_buffer.Add(Dong);
            yaoceDisplay.DanTou_GNSSBei_buffer.Add(Bei);
            yaoceDisplay.DanTou_GNSSTian_buffer.Add(Tian);
        }

        private void AddDANTOUJiaoSuDu(double Wx, double Wy, double Wz)
        {
            yaoceDisplay.DanTou_Wx_buffer.Add(Wx);
            yaoceDisplay.DanTou_Wy_buffer.Add(Wy);
            yaoceDisplay.DanTou_Wz_buffer.Add(Wz);
        }

        private void AddDANTOUJiaoDu(double FuYang, double GunZhuan, double PianHang)
        {
            yaoceDisplay.DanTou_FuYangJiao_buffer.Add(FuYang);
            yaoceDisplay.DanTou_GunZhuanJiao_buffer.Add(GunZhuan);
            yaoceDisplay.DanTou_PianHangJiao_buffer.Add(PianHang);
        }

        private void AddDANTOUBiLi(double x,double y,double z)
        {
            yaoceDisplay.DanTou_XBiLi_buffer.Add(x);
            yaoceDisplay.DanTou_YBiLi_buffer.Add(y);
            yaoceDisplay.DanTou_ZBiLi_buffer.Add(z);
        }

        private void AddFRAMEINFO(ref FRAME_PROPERTY sObject)
        {
            switch (sObject.CanId)
            {
                // 系统判据状态
                case frameType_systemStatus_1:
                    {
                        yaoceDisplay.XiTongPanJu15_buffer.Add(sObject.frameNo);
                    }
                    break;

                // 系统判据状态 0x16(中间存在两种情况，需要通过帧类型来做进一步的区分)
                case frameType_systemStatus_2:
                    {
                        if (sObject.frameType == frameType_XTPJFK)
                        {
                            yaoceDisplay.XiTongPanJu16_buffer.Add(sObject.frameNo); //
                                                                                    // 
                        }
                        else if (sObject.frameType == frameType_HLJCFK)
                        {
                            yaoceDisplay.HuiLuJianCe_buffer.Add(sObject.frameNo); //                                                                                        // 
                        }
                    }
                    break;
                case frameType_daoHangKuaiSu_Ti:
                    yaoceDisplay.DHK_Ti_buffer.Add(sObject.frameNo);
                    break;

                case frameType_daoHangKuaiSu_Tou:
                    yaoceDisplay.DHK_Tou_buffer.Add(sObject.frameNo);
                    break;

                case frameType_daoHangManSu_Ti:          
                    if (yaoceDisplay.DHM_Ti_buffer.Count() > MainWindow.frame_MaxCount)
                    {
                        yaoceDisplay.DHM_Ti_buffer.RemoveAt(0);
                    }
                    yaoceDisplay.DHM_Ti_buffer.Add(sObject.frameNo);
                    break;

                case frameType_daoHangManSu_Tou:
                    ///yaoceDisplay.DHM_Tou_buffer.Add(sObject.frameNo);
                    yaoceDisplay.DanTou_buffer.Add(sObject.frameNo);
                    break;

                case frameType_XiTongJiShi_Ti:
                    yaoceDisplay.XTJS_Ti_buffer.Add(sObject.frameNo);
                    break;

                case frameType_XiTongJiShi_Tou:
                    yaoceDisplay.XTJS_Tou_buffer.Add(sObject.frameNo);
                    break;

                default:
                    break;
            }
        }

        private void TemplateDraw(List<double> packets, ChartPointDataSource Points)
        {
            packets.ForEach(packet =>
            {
                Points.AddPoint(packet);
            });
        }


        private void XiTong_CeLuePanJue1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_CeLuePanJue1_PoPup.IsOpen = false;
            XiTong_CeLuePanJue1_PoPup.IsOpen = true;
        }

        private void XiTong_CeLuePanJue2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_CeLuePanJue2_PoPup.IsOpen = false;
            XiTong_CeLuePanJue2_PoPup.IsOpen = true;
        }

        private void XiTong_ShuRuCaiJi1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_ShuRuCaiJi1_PoPup.IsOpen = false;
            XiTong_ShuRuCaiJi1_PoPup.IsOpen = true;
        }

        private void XiTong_ShuRuCaiJi2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_ShuRuCaiJi2_PoPup.IsOpen = false;
            XiTong_ShuRuCaiJi2_PoPup.IsOpen = true;
        }

        private void XiTong_ShuRuCaiJi3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_ShuRuCaiJi3_PoPup.IsOpen = false;
            XiTong_ShuRuCaiJi3_PoPup.IsOpen = true;
        }

        private void XiTong_ShuRuCaiJi4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_ShuRuCaiJi4_PoPup.IsOpen = false;
            XiTong_ShuRuCaiJi4_PoPup.IsOpen = true;
        }

        private void XiTong_ShuJu1Hao_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_ShuJu1Hao_PoPup.IsOpen = false;
            XiTong_ShuJu1Hao_PoPup.IsOpen = true;
        }

        private void XiTong_ShuJu2Hao_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XiTong_ShuJu2Hao_PoPup.IsOpen = false;
            XiTong_ShuJu2Hao_PoPup.IsOpen = true;
        }

        private void XTJS_Ti_GuZhangBiaoZhi_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            XTJS_Ti_GuZhangBiaoZhi_PoPup.IsOpen = false;
            XTJS_Ti_GuZhangBiaoZhi_PoPup.IsOpen = true;
        }

        private void DHKuaiSu_Ti_JiaJiGuZhang_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DHKuaiSu_Ti_JiaJiGuZhang_PoPup.IsOpen = false;
            DHKuaiSu_Ti_JiaJiGuZhang_PoPup.IsOpen = true;
        }

        private void DHKuaiSu_Ti_TuoLuoGuZhang_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DHKuaiSu_Ti_TuoLuoGuZhang_PoPup.IsOpen = false;
            DHKuaiSu_Ti_TuoLuoGuZhang_PoPup.IsOpen = true;
        }

        private void DHManSu_Ti_QiTaZhuangTaiShuJu_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DHManSu_Ti_QiTaZhuangTaiShuJu_PoPup.IsOpen = false;
            DHManSu_Ti_QiTaZhuangTaiShuJu_PoPup.IsOpen = true;
        }

        private void DHManSu_Ti_JiaJiGuZhang_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DHManSu_Ti_JiaJiGuZhang_PoPup.IsOpen = false;
            DHManSu_Ti_JiaJiGuZhang_PoPup.IsOpen = true;
        }

        private void DHManSu_Ti_TuoLuoGuZhang_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DHManSu_Ti_TuoLuoGuZhang_PoPup.IsOpen = false;
            DHManSu_Ti_TuoLuoGuZhang_PoPup.IsOpen = true;
        }
    }
}
