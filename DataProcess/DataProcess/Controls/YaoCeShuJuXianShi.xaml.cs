using DataProcess;
using DataProcess.Tools;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ScoreTools.CustomControl;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using YaoCeProcess;

namespace DataProcess.Controls
{
    public class YaoCeChartDataSource
    {
        //系统
        public ChartPointDataSource chart_XiTong_ZuoBiao_JingDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_ZuoBiao_WeiDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_ZuoBiao_GaoDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XiTong_SuDu_DongList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_SuDu_BeiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_SuDu_TianList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XiTong_JiaoSuDu_WxList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_JiaoSuDu_WyList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_JiaoSuDu_WzList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XiTong_FaSheXi_ZXGZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_FaSheXi_XList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_FaSheXi_YList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_FaSheXi_ZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XiTong_YuShiLuoDian_SheChengList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTong_YuShiLuoDian_ZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //导航快
        public ChartPointDataSource chart_DHKuaiSu_Ti_ZuoBiao_JingDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_ZuoBiao_WeiDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_ZuoBiao_GaoDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHKuaiSu_Ti_SuDu_DongList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_SuDu_BeiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_SuDu_TianList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);


        public ChartPointDataSource chart_DHKuaiSu_Tou_ZuoBiao_JingDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_ZuoBiao_WeiDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_ZuoBiao_GaoDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHKuaiSu_Tou_SuDu_DongList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_SuDu_BeiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_SuDu_TianList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //导航慢
        public ChartPointDataSource chart_DHManSu_Ti_ZuoBiao_JingDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_ZuoBiao_WeiDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_ZuoBiao_GaoDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Ti_SuDu_DongList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_SuDu_BeiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_SuDu_TianList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);


        public ChartPointDataSource chart_DHManSu_Tou_ZuoBiao_JingDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_ZuoBiao_WeiDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_ZuoBiao_GaoDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Tou_SuDu_DongList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_SuDu_BeiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_SuDu_TianList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //系统即时
        public ChartPointDataSource chart_XTJS_Tou_ZuoBiao_JingDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_ZuoBiao_WeiDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_ZuoBiao_GaoDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XTJS_Tou_SuDu_DongList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_SuDu_BeiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_SuDu_TianList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XTJS_Tou_JiaoSuDu_WxList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_JiaoSuDu_WyList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_JiaoSuDu_WzList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XTJS_Tou_GuoZai_ZhouXiangList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_GuoZai_FaXiangList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Tou_GuoZai_CeXiangList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XTJS_Ti_ZuoBiao_JingDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_ZuoBiao_WeiDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_ZuoBiao_GaoDuList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XTJS_Ti_SuDu_DongList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_SuDu_BeiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_SuDu_TianList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XTJS_Ti_JiaoSuDu_WxList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_JiaoSuDu_WyList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_JiaoSuDu_WzList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_XTJS_Ti_GuoZai_ZhouXiangList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_GuoZai_FaXiangList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_Ti_GuoZai_CeXiangList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);


        //帧序号
        public ChartPointDataSource chart_XiTongPanJu15List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XiTongPanJu16List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_HuiLuJianCeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHK_TiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHM_TiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_TiList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHK_TouList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHM_TouList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_XTJS_TouList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
    }

    public partial class YaoCeShuJuXianShi : UserControl
    {
        /// 每一个UDP帧固定长度651
        public const int UDPLENGTH = 651;
 
        /// 系统判决状态数据标识
        public const int WM_YAOCE_SystemStatus_DATA = WinApi.WM_USER + 107;

        /// 导航数据（快速——弹体）标识
        public const int WM_YAOCE_daoHangKuaiSu_Ti_DATA = WinApi.WM_USER + 108;

        /// 导航数据（快速——弹头）标识
        public const int WM_YAOCE_daoHangKuaiSu_Tou_DATA = WinApi.WM_USER + 109;

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

        bool bRecvStatusData_XTJS = false; //系统即时反馈

        /// 网络消息处理
        //private DataParser dataParser;

        /// 码流记录
        private _DataLogger yaoceDataLogger = new _DataLogger(); //


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
                case E_STATUSTYPE_HuiLuJianCe:
                    if (bOn)
                    {
                        SetLedStatus(imageJianCe, LED_STATUS.LED_GREEN);
                    } 
                    else
                    {
                        SetLedStatus(imageJianCe, LED_STATUS.LED_GRAY);
                    }
                    break;
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
                case E_STATUSTYPE_DaoHangManSu_Tou:
                    if (bOn)
                    {
                        SetLedStatus(imageManSu, LED_STATUS.LED_GREEN);
                    }
                    else
                    {
                        SetLedStatus(imageManSu, LED_STATUS.LED_GRAY);
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

        private UdpClient udpClientYaoCe = null;
        private DataParser yaoceParser = null;
        YaoCeChartDataSource yaoCeChartDataSource = new YaoCeChartDataSource();

        private DispatcherTimer UpdateLoadFileProgressTimer = new DispatcherTimer(); //文件加载进度定时器

        //系统判决
        private DispatcherTimer UpdateXiTongStatusTimer = new DispatcherTimer();//系统判决数据定时器    
        private DispatcherTimer timerOffLineXiTongStatus = new DispatcherTimer();//系统判决状态离线
        private DispatcherTimer timerUpdateChart = new DispatcherTimer(); // 更新曲线

        //导航快速
        private DispatcherTimer timerUpdateDHKStatus = new DispatcherTimer();
        private DispatcherTimer timerOfflineDHKStatus = new DispatcherTimer();
        private DispatcherTimer timerUpdateChart_DHK = new DispatcherTimer();

        //导航慢速
        private DispatcherTimer timerUpdateChart_DHM = new DispatcherTimer();
        private DispatcherTimer timerUpdateDHMStatus = new DispatcherTimer();
        private DispatcherTimer timerOfflineDHMStatus = new DispatcherTimer();

        //回路检测
        private DispatcherTimer UpdateHuiLuJianCeTimer = new DispatcherTimer();//回路检测数据定时器
        private DispatcherTimer timerOffLineHuiLuJianCe = new DispatcherTimer();

        //系统即时反馈
        private DispatcherTimer timerUpdateChart_XiTongJiShi = new DispatcherTimer();
        private DispatcherTimer timerUpdateXiTongJiShiStatus = new DispatcherTimer();
        private DispatcherTimer timerOfflineXiTongJiShiStatus = new DispatcherTimer();

        //系统即时反馈
        private DispatcherTimer timerUpdateChart_XiTongJiShi_Tou = new DispatcherTimer();
        private DispatcherTimer timerUpdateXiTongJiShiStatus_Tou = new DispatcherTimer();
        private DispatcherTimer timerOfflineXiTongJiShiStatus_Tou = new DispatcherTimer();

        //帧序号
        private DispatcherTimer timerChartUpdate_ZXH = new DispatcherTimer();

        //UDP
        private DispatcherTimer timerUpdateUDP = new DispatcherTimer();//
        private DispatcherTimer timerOffLineUDP = new DispatcherTimer();

        System.Timers.Timer readFileTimer = new System.Timers.Timer(); //读取文件定时器

        public YaoCeShuJuXianShi()
        {
            InitializeComponent();

            InitLedStatus();
            InitTimer_YaoCe();
            InitYaoCeChartDataSource();

            // 创建新的日志文件
            Logger.GetInstance().NewFile();
            Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "程序开始启动！");
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

        private void InitLedStatus()
        {
            //SetLedStatus(ImageUDP, LED_STATUS.LED_GRAY);
            SetLedStatus(imageJianCe, LED_STATUS.LED_GRAY);
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
            UpdateXiTongStatusTimer.Interval = TimeSpan.FromMilliseconds(1000);
            UpdateXiTongStatusTimer.Tick += timerUpdateXiTongStatus_Tick;

            timerOffLineXiTongStatus.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            timerOffLineXiTongStatus.Tick += timerOffLineXiTongStatus_Tick;

            timerUpdateChart.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerUpdateChart.Tick += timerUpdateChart_Tick;
            /*---------------------------------------------------------*/

            /*--------系统导航快速定时器-------------------------------------*/
            timerUpdateDHKStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            //timerUpdateDHKStatus.Tick += timerUpdateDHKStatus_Tick;

            timerOfflineDHKStatus.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            //this.timerOfflineDHKStatus.Tick += timerOfflineDHKStatus_Tick;

            timerUpdateChart_DHK.Interval = new TimeSpan(0, 0, 0, 0, 500);
            //this.timerUpdateChart_DHK.Tick += timerUpdateChart_DHK_Tick;
            /*---------------------------------------------------------*/



            /*--------系统导航慢速定时器--------------------------------------*/
            timerUpdateDHMStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            //timerUpdateDHMStatus.Tick += timerUpdateDHKStatus_Tick;

            timerOfflineDHMStatus.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            //this.timerOfflineDHMStatus.Tick += timerOfflineDHKStatus_Tick;

            timerUpdateChart_DHM.Interval = new TimeSpan(0, 0, 0, 0, 500);
            //this.timerUpdateChart_DHK.Tick += timerUpdateChart_Tick_DHK;
            /*---------------------------------------------------------*/


            /*------------回路检测定时器-------------------------------------*/
            UpdateHuiLuJianCeTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            UpdateHuiLuJianCeTimer.Tick += timerUpdateHuiLuJianCe_Tick;

            timerOffLineHuiLuJianCe.Tick += timerOffLineHuiLuJianCe_Tick;
            /*---------------------------------------------------------*/


            /*-------------系统状态即时反馈定时器------------------------------*/
            timerUpdateXiTongJiShiStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timerUpdateXiTongJiShiStatus.Tick += timerUpdateXiTongJiShiStatus_Tick;

            timerUpdateChart_XiTongJiShi.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            //this.timerUpdateChart_XiTongJiShi.Tick += timerUpdateChart_XiTongJiShi_Tick;

            timerOfflineXiTongJiShiStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timerOfflineXiTongJiShiStatus.Tick += timerOfflineXiTongJiShiStatus_Tick;


            timerUpdateXiTongJiShiStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timerUpdateXiTongJiShiStatus_Tou.Tick += timerUpdateXiTongJiShiStatus_Tou_Tick;

            timerUpdateChart_XiTongJiShi_Tou.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            //this.timerUpdateChart_XiTongJiShi_Tou.Tick += timerUpdateChart_XiTongJiShi_Tou_Tick;

            timerOfflineXiTongJiShiStatus_Tou.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timerOfflineXiTongJiShiStatus_Tou.Tick += timerOfflineXiTongJiShiStatus_Tou_Tick;
            /*---------------------------------------------------------*/


            /*-------------------帧序号--------------------*/
            timerChartUpdate_ZXH.Interval = new TimeSpan(0, 0, 0, 0, 100);
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

            chart_DHKuaiSu_Ti_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_DongList;
            chart_DHKuaiSu_Ti_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_BeiList;
            chart_DHKuaiSu_Ti_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_TianList;

            chart_DHKuaiSu_Tou_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_ZuoBiao_WeiDuList;
            chart_DHKuaiSu_Tou_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_ZuoBiao_JingDuList;
            chart_DHKuaiSu_Tou_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_ZuoBiao_GaoDuList;

            chart_DHKuaiSu_Tou_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_DongList;
            chart_DHKuaiSu_Tou_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_BeiList;
            chart_DHKuaiSu_Tou_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_DHKuaiSu_Tou_SuDu_TianList;

            //导航慢
            chart_DHManSu_Ti_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_WeiDuList;
            chart_DHManSu_Ti_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_JingDuList;
            chart_DHManSu_Ti_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Ti_ZuoBiao_GaoDuList;

            chart_DHManSu_Ti_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_DongList;
            chart_DHManSu_Ti_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_BeiList;
            chart_DHManSu_Ti_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_TianList;

            chart_DHManSu_Tou_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_WeiDuList;
            chart_DHManSu_Tou_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_JingDuList;
            chart_DHManSu_Tou_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_ZuoBiao_GaoDuList;

            chart_DHManSu_Tou_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_DongList;
            chart_DHManSu_Tou_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_BeiList;
            chart_DHManSu_Tou_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_DHManSu_Tou_SuDu_TianList;

            //系统状态即时
            chart_XTJS_Ti_ZuoBiao_WeiDu.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_WeiDuList;
            chart_XTJS_Ti_ZuoBiao_JingDu.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_JingDuList;
            chart_XTJS_Ti_ZuoBiao_GaoDu.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_ZuoBiao_GaoDuList;

            chart_XTJS_Ti_JiaoSuDu_Wx.DataSource = yaoCeChartDataSource.chart_XTJS_Tou_JiaoSuDu_WxList;
            chart_XTJS_Ti_JiaoSuDu_Wy.DataSource = yaoCeChartDataSource.chart_XTJS_Tou_JiaoSuDu_WyList;
            chart_XTJS_Ti_JiaoSuDu_Wz.DataSource = yaoCeChartDataSource.chart_XTJS_Tou_JiaoSuDu_WzList;

            chart_XTJS_Ti_SuDu_Dong.DataSource = yaoCeChartDataSource.chart_XTJS_Tou_SuDu_DongList;
            chart_XTJS_Ti_SuDu_Bei.DataSource = yaoCeChartDataSource.chart_XTJS_Tou_SuDu_BeiList;
            chart_XTJS_Ti_SuDu_Tian.DataSource = yaoCeChartDataSource.chart_XTJS_Tou_SuDu_TianList;

            chart_XTJS_Tou_GuoZai_ZhouXiang.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_ZhouXiangList;
            chart_XTJS_Tou_GuoZai_FaXiang.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_FaXiangList;
            chart_XTJS_Tou_GuoZai_CeXiang.DataSource = yaoCeChartDataSource.chart_XTJS_Ti_GuoZai_CeXiangList;

            //帧序号
            chart_XiTongPanJu15.DataSource = yaoCeChartDataSource.chart_XiTongPanJu15List;
            chart_XiTongPanJu16.DataSource = yaoCeChartDataSource.chart_XiTongPanJu16List;
            chart_HuiLuJianCe.DataSource = yaoCeChartDataSource.chart_HuiLuJianCeList;
            chart_DHK_Ti.DataSource = yaoCeChartDataSource.chart_DHK_TiList;
            chart_DHM_Ti.DataSource = yaoCeChartDataSource.chart_DHM_TiList;
            chart_XTJS_Ti.DataSource = yaoCeChartDataSource.chart_XTJS_TiList;
            chart_DHK_Tou.DataSource = yaoCeChartDataSource.chart_DHK_TouList;
            chart_DHM_Tou.DataSource = yaoCeChartDataSource.chart_DHM_TouList;
            chart_XTJS_Tou.DataSource = yaoCeChartDataSource.chart_XTJS_TouList;
        }

        private void timerUpdateChart_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

                    // 按字节读取数据
                    const int fsLen = UDPLENGTH;
                    byte[] heByte = new byte[fsLen];
                    int readLength = 0;
                    if ((readLength = srFileRead.Read(heByte, 0, heByte.Length)) > 0)
                    {
                        // 处理数据
                        if (readLength < fsLen)// 
                        {
                            byte[] byteArray = new byte[readLength];
                            Array.Copy(heByte, 0, byteArray, 0, readLength);
                            yaoceParser.Enqueue(byteArray);
                        }
                        else
                        {
                            yaoceParser.Enqueue(heByte);
                        }
                        // 已经读取的文件大小
                        alreadReadFileLength += readLength;
                    }
                    else
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
                        load.setProgressBarValue(0, loadFileLength, loadFileLength);
                        load.loadFileFinish();

                        // 日志打印
                        Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "历史数据加载完成！");

                        MessageBox.Show("文件读取完成！");

                        // 线程休眠使用间隔时间(等待数据处理完成，而不是读取完毕，立即关闭定时器刷新)
                        Thread.Sleep(Interval);

                        // 关闭数据解析
                        yaoceParser.Stop();

                        // 停止绘图定时器刷新数据
                        // setTimerUpdateChartStatus(false);  

                        // 关闭状态刷新定时器
                        setUpdateTimerStatus(false);
                    }
                }), null);
            }
            else
            {
                if (srFileRead == null)
                {
                    return;
                }

                // 按字节读取数据
                const int fsLen = UDPLENGTH;
                byte[] heByte = new byte[fsLen];
                int readLength = 0;
                if ((readLength = srFileRead.Read(heByte, 0, heByte.Length)) > 0)
                {
                    // 处理数据
                    if (readLength < fsLen)// 
                    {
                        byte[] byteArray = new byte[readLength];
                        Array.Copy(heByte, 0, byteArray, 0, readLength);
                        yaoceParser.Enqueue(byteArray);

                    }
                    else
                    {
                        yaoceParser.Enqueue(heByte);
                    }
                    // 已经读取的文件大小
                    alreadReadFileLength += readLength;
                }
                else
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
                    load.setProgressBarValue(0, loadFileLength, loadFileLength);
                    load.loadFileFinish();

                    // 日志打印
                    Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "历史数据加载完成！");

                    MessageBox.Show("文件读取完成！");

                    // 线程休眠使用间隔时间(等待数据处理完成，而不是读取完毕，立即关闭定时器刷新)
                    Thread.Sleep(Interval);

                    // 关闭数据解析
                    yaoceParser.Stop();

                    // 停止绘图定时器刷新数据
                    // setTimerUpdateChartStatus(false);  

                    // 关闭状态刷新定时器
                    setUpdateTimerStatus(false);
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
            load.setProgressBarValue(0, loadFileLength, alreadReadFileLength);

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
            }
        }

        //系统判决离线状态
        private void timerOffLineXiTongStatus_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            bRecvStatusData_XiTong = false;
            setStatusOnOffLine(E_STATUSTYPE_XiTong, false);
        }

        /*----------------------------------------------------------------------*/

        //回路检测数据
        private void timerUpdateHuiLuJianCe_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_HuiLuJianCe)
            {
                // 填充实时数据
                showHuiLuJianCeStatus(ref sObject_huiLuJianCe);
                setStatusOnOffLine(E_STATUSTYPE_HuiLuJianCe, true);
            }
        }

        //回路检测离线状态
        private void timerOffLineHuiLuJianCe_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            bRecvStatusData_HuiLuJianCe = false;
            setStatusOnOffLine(E_STATUSTYPE_HuiLuJianCe, false);
        }

        /*----------------------------------------------------------------------*/


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


        /*----------------------------------------------------------------------*/

        //系统即时反馈数据弹体
        private void timerUpdateXiTongJiShiStatus_Tick(object sender,EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_XTJS)
            {
                // 填充实时数据
                showXiTongJiShiTimeStatus_Ti(ref sObject_XTJS_Ti);
                setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, true);
            }
        }

        //系统即时离线状态弹体
        private void timerOfflineXiTongJiShiStatus_Tick(object sender,EventArgs e)
        {
            // 是否收到数据
            bRecvStatusData_XTJS = false;
            setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, false);
        }

        /*------------------------------------------------------------------------*/

        //系统即时反馈数据弹头
        private void timerUpdateXiTongJiShiStatus_Tou_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            if (bRecvStatusData_XTJS)
            {
                // 填充实时数据
                showXiTongJiShiTimeStatus_Tou(ref sObject_XTJS_Tou);
                setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, true);
            }
        }

        //系统即时离线状态弹头
        private void timerOfflineXiTongJiShiStatus_Tou_Tick(object sender, EventArgs e)
        {
            // 是否收到数据
            bRecvStatusData_XTJS = false;
            setStatusOnOffLine(E_STATUSTYPE_XiTongJiShi_Ti, false);
        }

        /*------------------------------------------------------------------------*/



        //更新定时器状态
        private void setUpdateTimerStatus(bool bOpen)
        {
            if (bOpen)
            {
                UpdateXiTongStatusTimer.Start();
                UpdateHuiLuJianCeTimer.Start();
                timerUpdateXiTongJiShiStatus.Start();
                timerUpdateXiTongJiShiStatus_Tou.Start();
                timerUpdateUDP.Start();
            }
            else
            {
                UpdateXiTongStatusTimer.Stop();
                UpdateHuiLuJianCeTimer.Stop();
                timerUpdateXiTongJiShiStatus.Stop();
                timerUpdateXiTongJiShiStatus_Tou.Stop();
                timerUpdateUDP.Stop();
            }
        }

        //更新曲线定时器
        private void setTimerUpdateChartStatus(bool bOpen)
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
                        string fileName = load.getLoadFileName();
                        if (System.IO.File.Exists(fileName))
                        {
                            startLoadOffLineFile(fileName);
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
                        yaoceParser.Stop();

                        // 关闭绘图定时器刷新数据
                        //setTimerUpdateChartStatus(false); 

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
            yaoceParser.Start();

            //清空所有的曲线
            //clearAllChart(); 

            // 启动绘图定时器刷新数据
            //setTimerUpdateChartStatus(true);  

            // 刷新加载文件进度
            UpdateLoadFileProgressTimer.Start();

            // 开启状态刷新定时器
            setUpdateTimerStatus(true);


            // NOTE 20200525 每次重新回放重置数据显示界面 
            GenericFunction.reSetAllTextEdit(this.XiTong);
            GenericFunction.reSetAllTextEdit(this.HuiLu);
            GenericFunction.reSetAllTextEdit(this.FanKui_Ti);
            GenericFunction.reSetAllTextEdit(this.FanKui_Tou);



            // 是否收到数据
            bRecvStatusData_XiTong = false;
            bRecvStatusData_HuiLuJianCe = false;
            bRecvStatusData_XTJS = false;
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

        //回路检测反馈数据显示
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

        //系统判决实时状态显示
        private void showSystemTimeStatus(ref SYSTEMPARSE_STATUS sObject)
        {
            // 经度
            XiTong_JingDu.Text = sObject.jingDu.ToString();

            // 纬度
            XiTong_WeiDu.Text = sObject.weiDu.ToString();

            // 海拔高度
            XiTong_GaoDu.Text = sObject.haiBaGaoDu.ToString();

            // 东向速度
            XiTong_DongXiangSuDu.Text = sObject.dongXiangSuDu.ToString();

            // 北向速度
            XiTong_BeiXiangSuDu.Text = sObject.beiXiangSuDu.ToString();

            // 天向速度
            XiTong_TianXiangSuDu.Text = sObject.tianXiangSuDu.ToString();

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
            XiTong_GNSSTime.Text = sObject.GNSSTime.ToString();

            // 飞行总时间
            XiTong_ZongFeiXingTime.Text = sObject.feiXingZongShiJian.ToString();

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

            // 弹头状态(0-状态异常 1-产品遥测上电正常 2-初始化正常 3-一级保险解除
            // 4-二级保险解除 5-收到保险解除信号 6-三级保险解除 7-充电 8-起爆
            string danTouZhuangTaiValue = "";
            switch (sObject.danTouZhuangTai)
            {
                case 0:
                    danTouZhuangTaiValue = "状态异常";
                    break;
                case 1:
                    danTouZhuangTaiValue = "产品遥测上电正常";
                    break;
                case 2:
                    danTouZhuangTaiValue = "初始化正常";
                    break;
                case 3:
                    danTouZhuangTaiValue = "一级保险解除";
                    break;
                case 4:
                    danTouZhuangTaiValue = "二级保险解除";
                    break;
                case 5:
                    danTouZhuangTaiValue = "收到保险解除信号";
                    break;
                case 6:
                    danTouZhuangTaiValue = "三级保险解除";
                    break;
                case 7:
                    danTouZhuangTaiValue = "充电";
                    break;
                case 8:
                    danTouZhuangTaiValue = "起爆";
                    break;
                default:
                    danTouZhuangTaiValue = "未知";
                    break;
            }
            XiTong_DanTouZhuangTai.Text = danTouZhuangTaiValue;

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

            // bit4 bit5 时间间隔异常标志（00：时间间隔均正常; // 01：1号时间间隔异常，2号时间间隔正常； 10：1号时间间隔正常，2号时间间隔异常； 00：时间间隔均不正常） 
            tempValue = (byte)((daoHangTip1 >> 4) & 0x3);
            tempSTR = "";
            switch (tempValue)
            {
                case 0:
                    // tempSTR = "时间间隔均正常";
                    tempSTR = "无输出";
                    break;
                case 1:
                    // tempSTR = "1号异常，2号正常"; 
                    tempSTR = "1号";
                    break;
                case 2:
                    // tempSTR = "1号正常，2号异常"; 
                    tempSTR = "2号";
                    break;
                case 3:
                    // tempSTR = "时间间隔均不正常"; 
                    tempSTR = "1号和2号";
                    break;
                default:
                    break;
            }
            XiTong_ShiJianJianGeYiChang.Text = tempSTR;

            // bit6 弹头组合无效标志（1表示无效）
            XiTong_DanTouZuHe.Text = (daoHangTip1 >> 6 & 0x1) == 1 ? "无效" : "有效";

            // bit7 弹体组合无效标志（1表示无效）
            XiTong_DanTiZuHe.Text = (daoHangTip1 >> 7 & 0x1) == 1 ? "无效" : "有效";

            // 导航状态指示2
            byte daoHangTip2 = sObject.daoHangTip2;
            Dictionary<byte, string> dicTip = new Dictionary<byte, string>();
            dicTip.Add(0, "不是野值");
            dicTip.Add(1, "无数据");
            dicTip.Add(2, "数据用于初始化");
            dicTip.Add(3, "是野值");

            // bit0 bit1 1号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_1HaoShuJuJingDu.Text = dicTip[(byte)(daoHangTip2 & 0x2)];

            // bit2 bit3 1号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）// 
            XiTong_1HaoShuJuWeiDu.Text = dicTip[(byte)(daoHangTip2 >> 2 & 0x2)];

            // bit4 bit5 1号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_1HaoShuJuGaoDu.Text = dicTip[(byte)(daoHangTip2 >> 4 & 0x2)];

            // bit6 bit7 1号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_1HaoShuJuDongXiangSuDu.Text = dicTip[(byte)(daoHangTip2 >> 6 & 0x2)];

            // 导航状态指示3
            byte daoHangTip3 = sObject.daoHangTip3;

            // bit0 bit1 1号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_1HaoShuJuBeiXiangSuDu.Text = dicTip[(byte)(daoHangTip3 & 0x2)];

            // bit2 bit3 1号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_1HaoShuJuTianXiangSuDu.Text = dicTip[(byte)(daoHangTip3 >> 2 & 0x2)];

            // bit4 bit5 2号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值） 
            XiTong_2HaoShuJuJingDu.Text = dicTip[(byte)(daoHangTip3 >> 4 & 0x2)];

            // bit6 bit7 2号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_2HaoShuJuWeiDu.Text = dicTip[(byte)(daoHangTip3 >> 6 & 0x2)];

            // 导航状态指示3
            byte daoHangTip4 = sObject.daoHangTip4;

            // bit0 bit1 2号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_2HaoShuJuGaoDu.Text = dicTip[(byte)(daoHangTip4 & 0x2)];

            // bit2 bit3 2号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_2HaoShuJuDongXiangSuDu.Text = dicTip[(byte)(daoHangTip4 >> 2 & 0x2)];

            // bit4 bit5 2号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_2HaoShuJuBeiXiangSuDu.Text = dicTip[(byte)(daoHangTip4 >> 4 & 0x2)];

            // bit6 bit7 2号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
            XiTong_2HaoShuJuTianXiangSuDu.Text = dicTip[(byte)(daoHangTip4 >> 6 & 0x2)];

            // 系统状态指示
            byte sysyemStatusTip = sObject.sysyemStatusTip;

            // bit0 功率输出闭合（1有效）
            XiTong_GongLvShuChuBiHe.Text = (sysyemStatusTip & 0x1) == 1 ? "有效" : "无效";

            // bit1 解保指令发出（1有效）
            XiTong_JieBaoZhiLingFaChu.Text = (sysyemStatusTip >> 1 & 0x1) == 1 ? "有效" : "无效";

            // bit2 自毁指令发出（1有效）                                      
            XiTong_ZiHuiZhiLingFaChu.Text = (sysyemStatusTip >> 2 & 0x1) == 1 ? "有效" : "无效";

            // bit3 复位信号（1有效）
            XiTong_FuWeiXinHao.Text = (sysyemStatusTip >> 3 & 0x1) == 1 ? "有效" : "无效";

            // bit4 对外供电（1有效）                                             
            XiTong_DuiWaiGongDian.Text = (sysyemStatusTip >> 4 & 0x1) == 1 ? "有效" : "无效";

            // bit5 模拟自毁指令1（1有效）                                                  
            XiTong_MoNiZiHui1.Text = (sysyemStatusTip >> 5 & 0x1) == 1 ? "有效" : "无效";

            // bit6 模拟自毁指令2（1有效）                                             
            XiTong_MoNiZiHui2.Text = (sysyemStatusTip >> 6 & 0x1) == 1 ? "有效" : "无效";

            // bit7 回路检测 ?? 待定
            XiTong_HuiLuJianCe.Text = (sysyemStatusTip >> 7 & 0x1) == 1 ? "数据可用" : "数据不可用";

            // 触点状态指示
            byte chuDianZhuangTai = sObject.chuDianZhuangTai;

            // bit0 起飞分离脱插信号（0有效）
            XiTong_QiFeiFenLiTuoCha.Text = (chuDianZhuangTai >> 0 & 0x1) == 0 ? "有效" : "无效";

            // bit1 一级分离脱插信号（0有效）                                           // 
            XiTong_YiJiFenLiTuoCha.Text = (chuDianZhuangTai >> 1 & 0x1) == 0 ? "有效" : "无效";

            // bit2 安控接收机预令（1有效）
            XiTong_AnKongJieShouJiYuLing.Text = (chuDianZhuangTai >> 2 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                                  // 
                                                                                                  // bit3 安控接收机动令（1有效）
                                                                                                  // 
            XiTong_AnKongJieShouJiDongLing.Text = (chuDianZhuangTai >> 3 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                                    // 
                                                                                                    // bit4 一级自毁工作状态A（1有效）
                                                                                                    // 
            XiTong_1ZiHuiWorkA.Text = (chuDianZhuangTai >> 4 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                        // 
                                                                                        // bit5 一级自毁工作状态B（1有效）
                                                                                        // 
            XiTong_1ZiHuiWorkB.Text = (chuDianZhuangTai >> 5 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                        // 
                                                                                        // bit6 二级自毁工作状态A（1有效）
                                                                                        // 
            XiTong_2ZiHuiWorkA.Text = (chuDianZhuangTai >> 6 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                        // 
                                                                                        // bit7 二级自毁工作状态B（1有效）
                                                                                        // 
            XiTong_2ZiHuiWorkB.Text = (chuDianZhuangTai >> 7 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                        // 

            // 
            //----------------------------------------------------------------------//
            // 

            // 
            // 策略判决结果1
            // 
            byte jueCePanJueJieGuo1 = sObject.jueCePanJueJieGuo1; //
                                                                  // 
                                                                  // bit0 总飞行时间（1：有效
                                                                  // 
            XiTong_ZongFeiXingShiJian.Text = (jueCePanJueJieGuo1 >> 0 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                                 // 
                                                                                                 // bit1 侧向（1：有效）
                                                                                                 // 
            XiTong_CeXiang.Text = (jueCePanJueJieGuo1 >> 1 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                      // 
                                                                                      // bit2 Wx角速度（1：有效）
                                                                                      // 
            XiTong_WxJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 2 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                               // 
                                                                                               // bit3 Wy角速度（1：有效）
                                                                                               // 
            XiTong_WyJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 3 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                               // 
                                                                                               // bit4 Wz角速度（1：有效）
                                                                                               // 
            XiTong_WzJiaoSuDuStatus.Text = (jueCePanJueJieGuo1 >> 4 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                               // 
                                                                                               // bit5 后向（1：有效）
                                                                                               // 
            XiTong_HouXiang.Text = (jueCePanJueJieGuo1 >> 5 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                       // 
                                                                                       // bit6 坠落（1：有效）
                                                                                       // 
            XiTong_ZhuiLuo.Text = (jueCePanJueJieGuo1 >> 6 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                      // 
                                                                                      // bit7 分离时间（1：有效）
                                                                                      // 
            XiTong_FenLiShiTian.Text = (jueCePanJueJieGuo1 >> 7 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                           // 

            // 
            //----------------------------------------------------------------------//
            // 

            // 
            // 策略判决结果2
            // 
            byte jueCePanJueJieGuo2 = sObject.jueCePanJueJieGuo2; //
                                                                  // 
                                                                  // bit0 控制区下限（1：有效）
                                                                  // 
            XiTong_KongZhiQuXiaXian.Text = (jueCePanJueJieGuo2 >> 0 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                               // 
                                                                                               // bit1 控制区上限（1：有效）
                                                                                               // 
            XiTong_KongZhiQuShangXian.Text = (jueCePanJueJieGuo2 >> 1 & 0x1) == 1 ? "有效" : "无效"; //
                                                                                                 // 

            // 
            //----------------------------------------------------------------------//
            // 

            // 
            // 输出开关状态1
            // 
            byte shuChuKaiGuanStatus1 = sObject.shuChuKaiGuanStatus1; //
                                                                      // 
                                                                      // bit0 弹头保险（1：闭合）
                                                                      // 
            XiTong_DanTouBaoXian.Text = (jueCePanJueJieGuo2 >> 0 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                            // 
                                                                                            // bit1 弹头起爆（1：闭合）
                                                                                            // 
            XiTong_DanTouQiBao.Text = (jueCePanJueJieGuo2 >> 1 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                          // 
                                                                                          // bit2 一级保险1（1：闭合）
                                                                                          // 
            XiTong_1JiBaoXian1.Text = (jueCePanJueJieGuo2 >> 2 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                          // 
                                                                                          // bit3 一级保险2（1：闭合）
                                                                                          // 
            XiTong_1JiBaoXian2.Text = (jueCePanJueJieGuo2 >> 3 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                          // 
                                                                                          // bit4 一级起爆1（1：闭合）
                                                                                          // 
            XiTong_1JiQiBao1.Text = (jueCePanJueJieGuo2 >> 4 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                        // 
                                                                                        // bit5 一级起爆2（1：闭合）
                                                                                        // 
            XiTong_1JiQiBao2.Text = (jueCePanJueJieGuo2 >> 5 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                        // 
                                                                                        // bit6 二级保险1（1：闭合）
                                                                                        // 
            XiTong_2JiBaoXian1.Text = (jueCePanJueJieGuo2 >> 6 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                          // 
                                                                                          // bit7 二级保险2（1：闭合）
                                                                                          // 
            XiTong_2JiBaoXian2.Text = (jueCePanJueJieGuo2 >> 7 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                          // 

            // 
            //----------------------------------------------------------------------//
            // 

            // 
            // 输出开关状态2
            // 
            byte shuChuKaiGuanStatus2 = sObject.shuChuKaiGuanStatus2; //
                                                                      // 
                                                                      // bit0 二级起爆1（1：闭合）
                                                                      // 
            XiTong_2JiQiBao1.Text = (shuChuKaiGuanStatus2 >> 0 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                          // 
                                                                                          // bit1 二级起爆2（1：闭合）
                                                                                          // 
            XiTong_2JiQiBao2.Text = (shuChuKaiGuanStatus2 >> 1 & 0x1) == 1 ? "闭合" : "断开"; //
                                                                                          // 
                                                                                          // bit2 bit3 参试状态（00：测试1，数据输出状态；01：测试2，低压输出状态；10：保留状态；11：正式实验）
                                                                                          // 
            tempValue = (byte)(shuChuKaiGuanStatus2 >> 2 & 0x3); //
                                                                 // 
            tempSTR = ""; //
                          // 
            switch (tempValue)
            // 
            {
                // 
                case 0:
                    // 
                    tempSTR = "测试1，数据输出状态"; //
                                            // 
                    break; //
                           // 
                case 1:
                    // 
                    tempSTR = "测试2，低压输出状态"; //
                                            // 
                    break; //
                           // 
                case 2:
                    // 
                    tempSTR = "保留状态"; //
                                      // 
                    break; //
                           // 
                case 3:
                    // 
                    tempSTR = "正式实验"; //
                                      // 
                    break; //
                           // 
                default:
                    // 
                    break; //
                           // 
            }
            // 
            XiTong_CanShiZhuangTai.Text = tempSTR; //
                                                   // 

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
            XiTong_ZhouXiangGuoZai.Text = sObject.zhouXiangGuoZai.ToString(); //
                                                                              // 
        }

        //系统即时反馈弹体数据显示
        private void showXiTongJiShiTimeStatus_Ti(ref SYSTEMImmediate_STATUS sObject)
        {
            // 
            // 故障标志位
            // 
            byte guZhangBiaoZhi = sObject.guZhangBiaoZhi; //
                                                          // 
                                                          // bit0 陀螺x故障标志（0：正常；1：故障）
                                                          // 
            XTJS_Ti_XTuoLuoGuZhang.Text = (guZhangBiaoZhi & 0x1) == 0 ? "正常" : "故障"; //
                                                                                        // 
                                                                                        // bit1 陀螺y故障标志（0：正常；1：故障）
                                                                                        // 
            XTJS_Ti_YTuoLuoGuZhang.Text = (guZhangBiaoZhi >> 1 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                             // 
                                                                                             // bit2 陀螺z故障标志（0：正常；1：故障）
                                                                                             // 
            XTJS_Ti_ZTuoLuoGuZhang.Text = (guZhangBiaoZhi >> 2 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                             // 
                                                                                             // bit3 RS422故障标志（0：正常；1：故障）
                                                                                             // 
            XTJS_Ti_RS422GuZhang.Text = (guZhangBiaoZhi >> 3 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                           // 
                                                                                           // bit4 1553B故障标志（0：正常；1：故障）
                                                                                           // 
            XTJS_Ti_1553BGuZhang.Text = (guZhangBiaoZhi >> 4 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                           // 

            // 
            // X陀螺温度
            // 
            // Y陀螺温度
            // 
            // Z陀螺温度
            // 
            XTJS_Ti_XTuoLuoWenDu.Text = sObject.tuoLuoWenDu_X.ToString(); //
                                                                             // 
            XTJS_Ti_YTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Y.ToString(); //
                                                                             // 
            XTJS_Ti_ZTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Z.ToString(); //
                                                                             // 

            // 
            // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
            // 
            byte tempValue = sObject.GPS_SV; //
                                             // 
            XTJS_Ti_GPSSVKeYong.Text = ((byte)(tempValue & 0xF)).ToString(); //
                                                                             // 
            XTJS_Ti_GPSCanYuDingWei.Text = ((byte)(tempValue >> 4 & 0xF)).ToString(); //
                                                                                      // 

            // 
            // GPS定位模式
            // 
            byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi; //
                                                            // 
            string tempValueSTR = ""; //
                                      // 
                                      // bit0 (1:采用GPS定位 0:没有采用GPS定位)
                                      // 
            tempValueSTR = (GPSDingWeiMoShi >> 0 & 0x01) == 1 ? "采用GPS定位" : "没有采用GPS定位"; //
                                                                                         // 
            XTJS_Ti_GPSDingWeiZhuangTai_GPS.Text = tempValueSTR; //
                                                                 // 
                                                                 // bit1 (1:采用BD2定位 0:没有采用BD2定位)
                                                                 // 
            tempValueSTR = (GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位"; //
                                                                                         // 
            XTJS_Ti_GPSDingWeiZhuangTai_BD2.Text = tempValueSTR; //
                                                                 // 
                                                                 // bit2 1：采用GLONASS定位 0：没有采用GLONASS定位
                                                                 // 
            tempValueSTR = (GPSDingWeiMoShi >> 2 & 0x01) == 1 ? "采用GLONASS定位" : "没有采用GLONASS定位"; //
                                                                                                 // 
            XTJS_Ti_GPSDingWeiZhuangTai_GLONASS.Text = tempValueSTR; //
                                                                     // 
                                                                     // bit3 0:没有DGNSS可用 1：DGNSS可用
                                                                     // 
            tempValueSTR = (GPSDingWeiMoShi >> 3 & 0x01) == 1 ? "DGNSS可用" : "没有DGNSS可用"; //
                                                                                         // 
            XTJS_Ti_GPSDingWeiZhuangTai_DGNSS.Text = tempValueSTR; //
                                                                   // 
                                                                   // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
                                                                   // 
            tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03); //
                                                             // 
            tempValueSTR = tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : "")); //
                                                                                                                      // 
            XTJS_Ti_GPSDingWeiZhuangTai_Fix.Text = tempValueSTR; //
                                                                 // 
                                                                 // bit6 0:GNSS修正无效 1：GNSS修正有效
                                                                 // 
            tempValueSTR = (GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效"; //
                                                                                         // 
            XTJS_Ti_GPSDingWeiZhuangTai_GNSSXiuZheng.Text = tempValueSTR; //
                                                                          // 
                                                                          // bit7 0:BD2修正无效 1：BD2修正有效
                                                                          // 
            tempValueSTR = (GPSDingWeiMoShi >> 7 & 0x01) == 1 ? "BD2修正有效" : "BD2修正无效"; //
                                                                                       // 
            XTJS_Ti_GPSDingWeiZhuangTai_BD2XiuZheng.Text = tempValueSTR; //
                                                                         // 
                                                                         // DHManSu_GPSDingWeiZhuangTai.Text = tempValueSTR; //
                                                                         // 

            // 
            // PDOP 当量0.01
            // 
            XTJS_Ti_PDOP.Text = ((double)(sObject.PDOP)).ToString(); //
                                                                     // 
                                                                     // HDOP 当量0.01
                                                                     // 
            XTJS_Ti_HDOP.Text = ((double)(sObject.HDOP)).ToString(); //
                                                                     // 
                                                                     // VDOP 当量0.01
                                                                     // 
            XTJS_Ti_VDOP.Text = ((double)(sObject.VDOP)).ToString(); //
                                                                     // 

            // 
            // GPS时间 单位s,UTC秒部
            // 
            XTJS_Ti_GPSTime.Text = ((double)(sObject.GPSTime * 0.1)).ToString(); //
                                                                                 // 

            // 
            // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
            // 
            XTJS_Ti_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString(); //
                                                                                            // 
                                                                                            // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
                                                                                            // 
            XTJS_Ti_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString(); //
                                                                                          // 
                                                                                          // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                                                                                          // 
            XTJS_Ti_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString(); //
                                                                                               // 

            // 
            //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
            // 
            XTJS_Ti_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                          // 
                                                                                                          //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                                                                                                          // 
            XTJS_Ti_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                        // 
                                                                                                        //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                                                                                                        // 
            XTJS_Ti_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                          // 

            // 
            // 轴向过载
            // 
            // 法向过载
            // 
            // 侧向过载
            // 
            XTJS_Ti_GuoZhai_ZhouXiang.Text = sObject.zhouXiangGuoZai.ToString(); //
                                                                                 // 
            XTJS_Ti_GuoZhai_FaXiang.Text = sObject.faXiangGuoZai.ToString(); //
                                                                             // 
            XTJS_Ti_GuoZhai_CeXiang.Text = sObject.ceXiangGuoZai.ToString(); //
                                                                             // 

            // 
            // Wx角速度
            // 
            // Wy角速度
            // 
            // Wz角速度
            // 
            XTJS_Ti_JiaoSuDu_X.Text = sObject.WxJiaoSuDu.ToString(); //
                                                                     // 
            XTJS_Ti_JiaoSuDu_Y.Text = sObject.WyJiaoSuDu.ToString(); //
                                                                     // 
            XTJS_Ti_JiaoSuDu_Z.Text = sObject.WzJiaoSuDu.ToString(); //
                                                                     // 
        }

        //系统即时反馈弹头数据显示
        private void showXiTongJiShiTimeStatus_Tou(ref SYSTEMImmediate_STATUS sObject)
        {
            // 
            {
                // 
                // 故障标志位
                // 
                byte guZhangBiaoZhi = sObject.guZhangBiaoZhi; //
                                                              // 
                                                              // bit0 陀螺x故障标志（0：正常；1：故障）
                                                              // 
                XTJS_Tou_XTuoLuoGuZhang.Text = (guZhangBiaoZhi & 0x1) == 0 ? "正常" : "故障"; //
                                                                                          // 
                                                                                          // bit1 陀螺y故障标志（0：正常；1：故障）
                                                                                          // 
                XTJS_Tou_YTuoLuoGuZhang.Text = (guZhangBiaoZhi >> 1 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                               // 
                                                                                               // bit2 陀螺z故障标志（0：正常；1：故障）
                                                                                               // 
                XTJS_Tou_ZTuoLuoGuZhang.Text = (guZhangBiaoZhi >> 2 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                               // 
                                                                                               // bit3 RS422故障标志（0：正常；1：故障）
                                                                                               // 
                XTJS_Tou_RS422GuZhang.Text = (guZhangBiaoZhi >> 3 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                             // 
                                                                                             // bit4 1553B故障标志（0：正常；1：故障）
                                                                                             // 
                XTJS_Tou_1553BGuZhang.Text = (guZhangBiaoZhi >> 4 & 0x1) == 0 ? "正常" : "故障"; //
                                                                                             // 

                // 
                // X陀螺温度
                // 
                // Y陀螺温度
                // 
                // Z陀螺温度
                // 
                XTJS_Tou_XTuoLuoWenDu.Text = sObject.tuoLuoWenDu_X.ToString(); //
                                                                               // 
                XTJS_Tou_YTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Y.ToString(); //
                                                                               // 
                XTJS_Tou_ZTuoLuoWenDu.Text = sObject.tuoLuoWenDu_Z.ToString(); //
                                                                               // 

                // 
                // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
                // 
                byte tempValue = sObject.GPS_SV; //
                                                 // 
                XTJS_Tou_GPSSVKeYong.Text = ((byte)(tempValue & 0xF)).ToString(); //
                                                                                  // 
                XTJS_Tou_GPSCanYuDingWei.Text = ((byte)(tempValue >> 4 & 0xF)).ToString(); //
                                                                                           // 

                // 
                // GPS定位模式
                // 
                byte GPSDingWeiMoShi = sObject.GPSDingWeiMoShi; //
                                                                // 
                string tempValueSTR = ""; //
                                          // 
                                          // bit0 (1:采用GPS定位 0:没有采用GPS定位)
                                          // 
                tempValueSTR = (GPSDingWeiMoShi >> 0 & 0x01) == 1 ? "采用GPS定位" : "没有采用GPS定位"; //
                                                                                             // 
                XTJS_Tou_GPSDingWeiZhuangTai_GPS.Text = tempValueSTR; //
                                                                      // 
                                                                      // bit1 (1:采用BD2定位 0:没有采用BD2定位)
                                                                      // 
                tempValueSTR = (GPSDingWeiMoShi >> 1 & 0x01) == 1 ? "采用BD2定位" : "没有采用BD2定位"; //
                                                                                             // 
                XTJS_Tou_GPSDingWeiZhuangTai_BD2.Text = tempValueSTR; //
                                                                      // 
                                                                      // bit2 1：采用GLONASS定位 0：没有采用GLONASS定位
                                                                      // 
                tempValueSTR = (GPSDingWeiMoShi >> 2 & 0x01) == 1 ? "采用GLONASS定位" : "没有采用GLONASS定位"; //
                                                                                                     // 
                XTJS_Tou_GPSDingWeiZhuangTai_GLONASS.Text = tempValueSTR; //
                                                                          // 
                                                                          // bit3 0:没有DGNSS可用 1：DGNSS可用
                                                                          // 
                tempValueSTR = (GPSDingWeiMoShi >> 3 & 0x01) == 1 ? "DGNSS可用" : "没有DGNSS可用"; //
                                                                                             // 
                XTJS_Tou_GPSDingWeiZhuangTai_DGNSS.Text = tempValueSTR; //
                                                                        // 
                                                                        // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
                                                                        // 
                tempValue = (byte)(GPSDingWeiMoShi >> 4 & 0x03); //
                                                                 // 
                tempValueSTR = tempValue == 0 ? "No Fix" : (tempValue == 1 ? "2DFix" : (tempValue == 3 ? "3D Fix" : "")); //
                                                                                                                          // 
                XTJS_Tou_GPSDingWeiZhuangTai_Fix.Text = tempValueSTR; //
                                                                      // 
                                                                      // bit6 0:GNSS修正无效 1：GNSS修正有效
                                                                      // 
                tempValueSTR = (GPSDingWeiMoShi >> 6 & 0x01) == 1 ? "GNSS修正有效" : "GNSS修正无效"; //
                                                                                             // 
                XTJS_Tou_GPSDingWeiZhuangTai_GNSSXiuZheng.Text = tempValueSTR; //
                                                                               // 
                                                                               // bit7 0:BD2修正无效 1：BD2修正有效
                                                                               // 
                tempValueSTR = (GPSDingWeiMoShi >> 7 & 0x01) == 1 ? "BD2修正有效" : "BD2修正无效"; //
                                                                                           // 
                XTJS_Tou_GPSDingWeiZhuangTai_BD2XiuZheng.Text = tempValueSTR; //
                                                                              // 
                                                                              // DHManSu_GPSDingWeiZhuangTai.Text = tempValueSTR; //
                                                                              // 

                // 
                // PDOP 当量0.01
                // 
                XTJS_Tou_PDOP.Text = ((double)(sObject.PDOP)).ToString(); //
                                                                          // 
                                                                          // HDOP 当量0.01
                                                                          // 
                XTJS_Tou_HDOP.Text = ((double)(sObject.HDOP)).ToString(); //
                                                                          // 
                                                                          // VDOP 当量0.01
                                                                          // 
                XTJS_Tou_VDOP.Text = ((double)(sObject.VDOP)).ToString(); //
                                                                          // 

                // 
                // GPS时间 单位s,UTC秒部
                // 
                XTJS_Tou_GPSTime.Text = ((double)(sObject.GPSTime * 0.1)).ToString(); //
                                                                                      // 

                // 
                // sObject.jingDu; //              // 经度（组合结果）当量：1e-7
                // 
                XTJS_Tou_JingDu.Text = ((double)(sObject.jingDu * Math.Pow(10, -7))).ToString(); //
                                                                                                 // 
                                                                                                 // sObject.weiDu; //               // 纬度（组合结果）当量：1e-7
                                                                                                 // 
                XTJS_Tou_WeiDu.Text = ((double)(sObject.weiDu * Math.Pow(10, -7))).ToString(); //
                                                                                               // 
                                                                                               // sObject.haiBaGaoDu; //          // 海拔高度（组合结果）当量：1e-2
                                                                                               // 
                XTJS_Tou_GaoDu.Text = ((double)(sObject.haiBaGaoDu * Math.Pow(10, -2))).ToString(); //
                                                                                                    // 

                // 
                //sObject.dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-2
                // 
                XTJS_Tou_DongXiangSuDu.Text = ((double)(sObject.dongXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                               // 
                                                                                                               //sObject.beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-2
                                                                                                               // 
                XTJS_Tou_BeiXiangSuDu.Text = ((double)(sObject.beiXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                             // 
                                                                                                             //sObject.tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-2
                                                                                                             // 
                XTJS_Tou_TianXiangSuDu.Text = ((double)(sObject.tianXiangSuDu * Math.Pow(10, -2))).ToString(); //
                                                                                                               // 
                // 
                // 轴向过载
                // 
                // 法向过载
                // 
                // 侧向过载
                // 
                XTJS_Tou_GuoZhai_ZhouXiang.Text = sObject.zhouXiangGuoZai.ToString(); //
                                                                                      // 
                XTJS_Tou_GuoZhai_FaXiang.Text = sObject.faXiangGuoZai.ToString(); //
                                                                                  // 
                XTJS_Tou_GuoZhai_CeXiang.Text = sObject.ceXiangGuoZai.ToString(); //
                                                                                  // 

                // 
                // Wx角速度
                // 
                // Wy角速度
                // 
                // Wz角速度
                // 
                XTJS_Tou_JiaoSuDu_X.Text = sObject.WxJiaoSuDu.ToString(); //
                                                                          // 
                XTJS_Tou_JiaoSuDu_Y.Text = sObject.WyJiaoSuDu.ToString(); //
                                                                          // 
                XTJS_Tou_JiaoSuDu_Z.Text = sObject.WzJiaoSuDu.ToString(); //
                                                                          // 
            }
        }

        //加载文件按钮
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //yaoceParser = new DataParser(new WindowInteropHelper(m).Handle);
            initYaoCeParser();
            if (load == null)
            {
                load = new LoadDataForm();
                load.setPlayStatus = setOffLineFilePlayStatus;
            }

            //LoadDataForm.Instance().setPlayStatus = setOffLineFilePlayStatus;
            //LoadDataForm.Instance().Show();
            load.Show();
            return;
        }

        //初始化数据解析
        public void initYaoCeParser()
        {
            if (yaoceParser == null)
            {
                yaoceParser = new DataParser(new WindowInteropHelper(m).Handle);
            }
        }

        //开始数据解析
        public void startYaoCeParser()
        {
            yaoceParser.Start();
        }

        //停止数据解析
        public void stopYaoCeParser()
        {
            if (yaoceParser != null)
            {
                yaoceParser.Stop();
            }
        }

        //开始数据日志记录
        public void startYaoCeDataLogger()
        {
            yaoceDataLogger.Start();
        }

        //停止数据日志记录
        public void stopYaoCeDataLogger()
        {
            yaoceDataLogger.Stop();//
        }

        //初始化UDP套接字
        public void initYaoCeUdpClient(int port,string ipAddr)
        {
            udpClientYaoCe = new UdpClient(port);
            udpClientYaoCe.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
            udpClientYaoCe.JoinMulticastGroup(IPAddress.Parse(ipAddr));
        }

        //开启UDP接收
        public void startUDPReceive()
        {
            udpClientYaoCe.BeginReceive(EndYaoCeReceive, null);
        }

        //关闭UDP套接字
        public void closeYaoCeUdp()
        {
            udpClientYaoCe?.Close();
        }

        //置空UDP套接字
        public void emptyYaoCeUdp()
        {
            udpClientYaoCe = null;
        }

        private void EndYaoCeReceive(IAsyncResult ar)
        {
            // 
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0); //
                                                                    // 
            try
            // 
            {
                // 
                byte[] recvBuffer = udpClientYaoCe?.EndReceive(ar, ref endPoint); //
                                                                                  // 
                yaoceParser.Enqueue(recvBuffer); //
                                                 // 
                yaoceDataLogger.Enqueue(recvBuffer); //
                                                     // 
                udpClientYaoCe?.BeginReceive(EndYaoCeReceive, null); //
                                                                     // 
            }
            // 
            catch (Exception)
            // 
            {
                // 
            }
            // 
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
                yaoceDataLogger.Stop();

                // 关闭消息处理
                if (yaoceParser != null)
                {
                    yaoceParser.Stop();
                }
                // 关闭绘图定时器刷新数据
                //setTimerUpdateChartStatus(false); 

                // 停止加载文件进度
                UpdateLoadFileProgressTimer.Stop();
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

                        // 重新启动离线定时器
                        timerOffLineXiTongStatus.Stop();
                        timerOffLineXiTongStatus.Start();

                        // 是否收到数据
                        bRecvStatusData_XiTong = true;

                        // 绘图
                        xiTong_CHART_ITEM_INDEX++;

                        // 添加系统坐标点集
                        //AddXiTongZuoBiao(sObject.jingDu, sObject.weiDu, sObject.haiBaGaoDu); 

                        // 添加系统速度点集
                        //AddXiTongSuDu(sObject.dongXiangSuDu, sObject.beiXiangSuDu, sObject.tianXiangSuDu); 

                        // 添加系统角速度点集
                        //AddXiTongJiaoSuDu(sObject.WxJiaoSuDu, sObject.WyJiaoSuDu, sObject.WzJiaoSuDu); 

                        // 添加系统发射系点集                                                          
                        //AddXiTongFaSheXi(sObject.zhouXiangGuoZai, sObject.curFaSheXi_X, sObject.curFaSheXi_Y, sObject.curFaSheXi_Z);

                        // 添加系统预示落点点集                                                                                      
                        //AddXiTongYuShiLuoDian(sObject.yuShiLuoDianSheCheng, sObject.yuShiLuoDianZ); 

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_daoHangKuaiSu_Ti_DATA:
                    {
                        IntPtr ptr = lParam;
                        DAOHANGSHUJU_KuaiSu sObject = Marshal.PtrToStructure<DAOHANGSHUJU_KuaiSu>(ptr);

                        // 缓存状态数据
                        //dHKSubForm_Ti.SetDHKStatus(ref sObject);

                        // 绘图 
                        //dHKSubForm_Ti.setCHARTITEMINDEXAdd();

                        // 添加导航数据快速坐标点集
                        //dHKSubForm_Ti.AddDHKuaiSuZuoBiao(sObject.jingDu, sObject.weiDu, sObject.haiBaGaoDu); 

                        // 添加导航数据快速速度点集
                        //dHKSubForm_Ti.AddDHKuaiSuSuDu(sObject.dongXiangSuDu, sObject.beiXiangSuDu, sObject.tianXiangSuDu);

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_daoHangKuaiSu_Tou_DATA:
                    {
                        IntPtr ptr = lParam;
                        DAOHANGSHUJU_KuaiSu sObject = Marshal.PtrToStructure<DAOHANGSHUJU_KuaiSu>(ptr);

                        // 缓存状态数据
                        //dHKSubForm_Tou.SetDHKStatus(ref sObject); 

                        // 绘图 
                        //dHKSubForm_Tou.setCHARTITEMINDEXAdd();

                        // 添加导航数据快速坐标点集
                        // dHKSubForm_Tou.AddDHKuaiSuZuoBiao(sObject.jingDu, sObject.weiDu, sObject.haiBaGaoDu); 

                        // 添加导航数据快速速度点集
                        //dHKSubForm_Tou.AddDHKuaiSuSuDu(sObject.dongXiangSuDu, sObject.beiXiangSuDu, sObject.tianXiangSuDu);  

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_daoHangManSu_Ti_DATA:
                    {
                        IntPtr ptr = lParam;
                        DAOHANGSHUJU_ManSu sObject = Marshal.PtrToStructure<DAOHANGSHUJU_ManSu>(ptr);

                        // 缓存状态数据
                        //dHMSubForm_Ti.SetDHMStatus(ref sObject); 

                        // 绘图
                        //dHMSubForm_Ti.setCHARTITEMINDEXAdd(); 

                        // 添加导航数据慢速坐标点集
                        //dHMSubForm_Ti.AddDHManSuZuoBiao(sObject.jingDu, sObject.weiDu, sObject.haiBaGaoDu); 

                        // 添加导航数据慢速速度点集
                        //dHMSubForm_Ti.AddDHManSuSuDu(sObject.dongXiangSuDu, sObject.beiXiangSuDu, sObject.tianXiangSuDu);

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_daoHangManSu_Tou_DATA:
                    {
                        IntPtr ptr = lParam;
                        DAOHANGSHUJU_ManSu sObject = Marshal.PtrToStructure<DAOHANGSHUJU_ManSu>(ptr);

                        // 缓存状态数据
                        //dHMSubForm_Tou.SetDHMStatus(ref sObject);

                        // 绘图
                        //dHMSubForm_Tou.setCHARTITEMINDEXAdd(); 

                        // 添加导航数据慢速坐标点集
                        //dHMSubForm_Tou.AddDHManSuZuoBiao(sObject.jingDu, sObject.weiDu, sObject.haiBaGaoDu); 

                        // 添加导航数据慢速速度点集                                                         
                        //dHMSubForm_Tou.AddDHManSuSuDu(sObject.dongXiangSuDu, sObject.beiXiangSuDu, sObject.tianXiangSuDu); 

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

                        //重新启动离线定时器
                        timerOfflineXiTongJiShiStatus.Stop();
                        timerOfflineXiTongJiShiStatus.Start();

                        //是否接收数据
                        bRecvStatusData_XTJS = true;

                        // 绘图
                        //xiTongJiShiSubForm_Ti.setCHARTITEMINDEXAdd();  

                        // xiTongJiShiSubForm_Ti.AddXiTongJiShiZuoBiao(sObject.jingDu, sObject.weiDu, sObject.haiBaGaoDu); //
                        // 
                        // xiTongJiShiSubForm_Ti.AddXiTongJiShiSuDu(sObject.dongXiangSuDu, sObject.beiXiangSuDu, sObject.tianXiangSuDu); //
                        // 
                        //xiTongJiShiSubForm_Ti.AddXiTongJiShiJiaoSuDu(sObject.WxJiaoSuDu, sObject.WyJiaoSuDu, sObject.WzJiaoSuDu); //
                        // 
                        // xiTongJiShiSubForm_Ti.AddXiTongJiShiGuoZai(sObject.zhouXiangGuoZai, sObject.faXiangGuoZai, sObject.ceXiangGuoZai); //

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

                        // 绘图
                        //xiTongJiShiSubForm_Tou.setCHARTITEMINDEXAdd(); 


                        //xiTongJiShiSubForm_Tou.AddXiTongJiShiZuoBiao(sObject.jingDu, sObject.weiDu, sObject.haiBaGaoDu); //
                        //                                                                                                 // 
                        //xiTongJiShiSubForm_Tou.AddXiTongJiShiSuDu(sObject.dongXiangSuDu, sObject.beiXiangSuDu, sObject.tianXiangSuDu); //
                        //                                                                                                               // 
                        //xiTongJiShiSubForm_Tou.AddXiTongJiShiJiaoSuDu(sObject.WxJiaoSuDu, sObject.WyJiaoSuDu, sObject.WzJiaoSuDu); //
                        //                                                                                                           // 
                        //xiTongJiShiSubForm_Tou.AddXiTongJiShiGuoZai(sObject.zhouXiangGuoZai, sObject.faXiangGuoZai, sObject.ceXiangGuoZai); 

                        Marshal.FreeHGlobal(ptr);
                    }
                    break;
                case WM_YAOCE_FRAMEPROPERTY_DATA:
                    {
                        IntPtr ptr = lParam;
                        FRAME_PROPERTY sObject = Marshal.PtrToStructure<FRAME_PROPERTY>(ptr);

                        // 缓存状态数据
                        //frameInfoForm.addFrameInfo(ref sObject); 

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

        //清除曲线内容
        private void clearAllChart()
        {
            xiTong_CHART_ITEM_INDEX = 0; //
        }

    }
}
