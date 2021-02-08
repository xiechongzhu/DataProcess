using DataModels;
using DataProcess.Log;
using DataProcess.Parser;
using DataProcess.Parser.Fly;
using DataProcess.Protocol;
using DataProcess.Setting;
using DataProcess.Tools;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Editors;
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
using YaoCeProcess;
using ScoreTools.CustomControl;
using DataProcess.YaoCe;
using DataProcess.Controls;
using System.Timers;
using System.Threading;
using System.Diagnostics;

namespace DataProcess
{
    public class ChartDataSource
    {
        //缓变参数
        public ChartPointDataSource SlowHoodList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowInsAirList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowInsWallList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttAirList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList1 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList2 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList3 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList4 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList5 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttWallList6 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowInsPresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowAttPresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowLevel2PresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowPresureHighList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource SlowPresureLowList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //速变参数
        public List<ChartPointDataSource> FastShakeSeriesLists = new List<ChartPointDataSource>();
        public ChartPointDataSource FastLashT3SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource FastLashT2SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource FastLashT1SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource FastLashT0SeriesList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public List<ChartPointDataSource> FastLashSeriesLists1 = new List<ChartPointDataSource>();
        public ChartPointDataSource FastLashSeriesList2 = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public List<ChartPointDataSource> FastNoiseLists = new List<ChartPointDataSource>();

        //尾段参数
        public ChartPointDataSource TailPresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLevel1PresureList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailTemperature1List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailTemperature2List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash1XList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash1YList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash1ZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash2XList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash2YList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailLash2ZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource TailNoiseList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //导航数据
        public ChartPointDataSource NavLat = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavLon = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavHeight = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSpeedNorth = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSpeedSky = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSpeedEast = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavPitchAngle = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavCrabAngle = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavRollAngle = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //角速度数据
        public ChartPointDataSource AngleAccXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleAccYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleAccZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngleZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //伺服数据
        public ChartPointDataSource ServoVol28List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource ServoVol160List = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo1IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo2IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo3IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource Servo4IqList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        //帧计数
        public ChartPointDataSource TailSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource AngelSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource ServoSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource NavSequenceList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartDataSource()
        {
            for(int i = 0; i < 12; ++i)
            {
                FastShakeSeriesLists.Add(new ChartPointDataSource(MainWindow.CHART_MAX_POINTS));
            }
            for(int i = 0; i < 3; ++i)
            {
                FastLashSeriesLists1.Add(new ChartPointDataSource(MainWindow.CHART_MAX_POINTS));
            }
            for (int i = 0; i < 2; ++i)
            {
                FastNoiseLists.Add(new ChartPointDataSource(MainWindow.CHART_MAX_POINTS));
            }
        }

        public void Clear()
        {
            SlowHoodList.ClearPoints();
            SlowInsAirList.ClearPoints();
            SlowInsWallList.ClearPoints();
            SlowAttAirList.ClearPoints();
            SlowAttWallList1.ClearPoints();
            SlowAttWallList2.ClearPoints();
            SlowAttWallList3.ClearPoints();
            SlowAttWallList4.ClearPoints();
            SlowAttWallList5.ClearPoints();
            SlowAttWallList6.ClearPoints();
            SlowInsPresureList.ClearPoints();
            SlowAttPresureList.ClearPoints();
            SlowLevel2PresureList.ClearPoints();
            SlowPresureHighList.ClearPoints();
            SlowPresureLowList.ClearPoints(); ;

            FastShakeSeriesLists.ForEach(item => item.ClearPoints());
            FastLashT3SeriesList.ClearPoints();
            FastLashT2SeriesList.ClearPoints();
            FastLashT1SeriesList.ClearPoints();
            FastLashT0SeriesList.ClearPoints();
            FastLashSeriesLists1.ForEach(item => item.ClearPoints());
            FastLashSeriesList2.ClearPoints();
            FastNoiseLists.ForEach(item => item.ClearPoints());

            TailPresureList.ClearPoints();
            TailLevel1PresureList.ClearPoints();
            TailTemperature1List.ClearPoints();
            TailTemperature2List.ClearPoints();
            TailLash1XList.ClearPoints();
            TailLash1YList.ClearPoints();
            TailLash1ZList.ClearPoints();
            TailLash2XList.ClearPoints();
            TailLash2YList.ClearPoints();
            TailLash2ZList.ClearPoints();
            TailNoiseList.ClearPoints();

            NavLat.ClearPoints();
            NavLon.ClearPoints();
            NavHeight.ClearPoints();
            NavSpeedNorth.ClearPoints();
            NavSpeedSky.ClearPoints();
            NavSpeedEast.ClearPoints();
            NavPitchAngle.ClearPoints();
            NavCrabAngle.ClearPoints();
            NavRollAngle.ClearPoints();

            AngleAccXList.ClearPoints();
            AngleAccYList.ClearPoints();
            AngleAccZList.ClearPoints();
            AngleXList.ClearPoints();
            AngleYList.ClearPoints();
            AngleZList.ClearPoints();

            ServoVol28List.ClearPoints();
            ServoVol160List.ClearPoints();
            Servo1IqList.ClearPoints();
            Servo2IqList.ClearPoints();
            Servo3IqList.ClearPoints();
            Servo4IqList.ClearPoints();

            TailSequenceList.ClearPoints();
            AngelSequenceList.ClearPoints();
            ServoSequenceList.ClearPoints();
            NavSequenceList.ClearPoints();
        }

        public void SetMaxDisplayCount(int maxCount)
        {
            SlowHoodList.SetMaxCount(maxCount);
            SlowInsAirList.SetMaxCount(maxCount);
            SlowInsWallList.SetMaxCount(maxCount);
            SlowAttAirList.SetMaxCount(maxCount);
            SlowAttWallList1.SetMaxCount(maxCount);
            SlowAttWallList2.SetMaxCount(maxCount);
            SlowAttWallList3.SetMaxCount(maxCount);
            SlowAttWallList4.SetMaxCount(maxCount);
            SlowAttWallList5.SetMaxCount(maxCount);
            SlowAttWallList6.SetMaxCount(maxCount);
            SlowInsPresureList.SetMaxCount(maxCount);
            SlowAttPresureList.SetMaxCount(maxCount);
            SlowLevel2PresureList.SetMaxCount(maxCount);
            SlowPresureHighList.SetMaxCount(maxCount);
            SlowPresureLowList.SetMaxCount(maxCount) ;

            FastShakeSeriesLists.ForEach(item => item.SetMaxCount(maxCount));
            FastLashT3SeriesList.SetMaxCount(maxCount);
            FastLashT2SeriesList.SetMaxCount(maxCount);
            FastLashT1SeriesList.SetMaxCount(maxCount);
            FastLashT0SeriesList.SetMaxCount(maxCount);
            FastLashSeriesLists1.ForEach(item => item.SetMaxCount(maxCount));
            FastLashSeriesList2.SetMaxCount(maxCount);
            FastNoiseLists.ForEach(item => item.SetMaxCount(maxCount));

            TailPresureList.SetMaxCount(maxCount);
            TailLevel1PresureList.SetMaxCount(maxCount);
            TailTemperature1List.SetMaxCount(maxCount);
            TailTemperature2List.SetMaxCount(maxCount);
            TailLash1XList.SetMaxCount(maxCount);
            TailLash1YList.SetMaxCount(maxCount);
            TailLash1ZList.SetMaxCount(maxCount);
            TailLash2XList.SetMaxCount(maxCount);
            TailLash2YList.SetMaxCount(maxCount);
            TailLash2ZList.SetMaxCount(maxCount);
            TailNoiseList.SetMaxCount(maxCount);

            NavLat.SetMaxCount(maxCount);
            NavLon.SetMaxCount(maxCount);
            NavHeight.SetMaxCount(maxCount);
            NavSpeedNorth.SetMaxCount(maxCount);
            NavSpeedSky.SetMaxCount(maxCount);
            NavSpeedEast.SetMaxCount(maxCount);
            NavPitchAngle.SetMaxCount(maxCount);
            NavCrabAngle.SetMaxCount(maxCount);
            NavRollAngle.SetMaxCount(maxCount);

            AngleAccXList.SetMaxCount(maxCount);
            AngleAccYList.SetMaxCount(maxCount);
            AngleAccZList.SetMaxCount(maxCount);
            AngleXList.SetMaxCount(maxCount);
            AngleYList.SetMaxCount(maxCount);
            AngleZList.SetMaxCount(maxCount);

            ServoVol28List.SetMaxCount(maxCount);
            ServoVol160List.SetMaxCount(maxCount);
            Servo1IqList.SetMaxCount(maxCount);
            Servo2IqList.SetMaxCount(maxCount);
            Servo3IqList.SetMaxCount(maxCount);
            Servo4IqList.SetMaxCount(maxCount);

            TailSequenceList.SetMaxCount(maxCount);
            AngelSequenceList.SetMaxCount(maxCount);
            ServoSequenceList.SetMaxCount(maxCount);
            NavSequenceList.SetMaxCount(maxCount);
        }
    }

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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DevExpress.Xpf.Core.ThemedWindow
    {
#if true
        /// 每一个UDP帧固定长度651
        private const int UDPLENGTH = 651; 
                                          
        /// 自定义消息
       // private const int WM_USER = WinApi.WinApi.WM_USER; 
                                            
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
        private int xiTong_CHART_ITEM_INDEX = 0;

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


        // 是否收到数据
        bool bRecvStatusData_XiTong = false; //系统判决
                                              
        /// bRecvStatusData_HuiLuJianCe
        bool bRecvStatusData_HuiLuJianCe = false;  
       
        /// bRecvStatusData_UDP 
        bool bRecvStatusData_UDP = false;

        bool bRecvStatusData_DHK = false; //
        bool bRecvStatusData_DHM = false; //

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
                // 
                case E_STATUSTYPE_XiTong:
                    // 
                    if (bOn)
                    // 
                    {
                        // 
                        // pictureEdit_XiTong.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_green.png"); //
                        SetLedStatus( imageZhuangTai, LED_STATUS.LED_GREEN);
                    }
                    // 
                    else
                    // 
                    {
                        // 
                        //pictureEdit_XiTong.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_gray.png"); //
                        SetLedStatus( imageZhuangTai, LED_STATUS.LED_GRAY);
                    }
                    // 
                    break; //
                           // 
                case E_STATUSTYPE_HuiLuJianCe:
                    // 
                    if (bOn)
                    // 
                    {
                        // 
                        //pictureEdit_HuiLu.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_green.png"); //
                        SetLedStatus( imageJianCe, LED_STATUS.LED_GREEN);
                    }
                    // 
                    else
                    // 
                    {
                        // 
                        //pictureEdit_HuiLu.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_gray.png"); //
                        SetLedStatus( imageJianCe, LED_STATUS.LED_GRAY);
                    }
                    // 
                    break; //
                           // 
                case E_STATUSTYPE_DaoHangKuaiSu_Ti:
                // 
                case E_STATUSTYPE_DaoHangKuaiSu_Tou:
                    // 
                    if (bOn)
                    // 
                    {
                        // 
                        //pictureEdit_DHK.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_green.png"); //
                        SetLedStatus( imageKuaiSu, LED_STATUS.LED_GREEN);
                    }
                    // 
                    else
                    // 
                    {
                        // 
                        //pictureEdit_DHK.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_gray.png"); //
                        SetLedStatus( imageKuaiSu, LED_STATUS.LED_GRAY);
                    }
                    // 
                    break; //
                           // 
                case E_STATUSTYPE_DaoHangManSu_Ti:
                // 
                case E_STATUSTYPE_DaoHangManSu_Tou:
                    // 
                    if (bOn)
                    // 
                    {
                        // 
                        //pictureEdit_DHM.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_green.png"); //
                        SetLedStatus( imageManSu, LED_STATUS.LED_GREEN);
                    }
                    // 
                    else
                    // 
                    {
                        // 
                       // pictureEdit_DHM.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_gray.png"); //
                        SetLedStatus( imageManSu, LED_STATUS.LED_GRAY);
                    }
                    // 
                    break; //
                           // 
                           // TODO 20200219 新增
                           // 
                case E_STATUSTYPE_XiTongJiShi_Ti:
                // 
                case E_STATUSTYPE_XiTongJiShi_Tou:
                    // 
                    if (bOn)
                    // 
                    {
                        // 
                        //pictureEdit_XiTongJiShi.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_green.png"); //
                        SetLedStatus( imageJiShi, LED_STATUS.LED_GREEN);
                    }
                    // 
                    else
                    // 
                    {
                        // 
                        // pictureEdit_XiTongJiShi.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_gray.png"); //
                        SetLedStatus( imageJiShi, LED_STATUS.LED_GRAY);
                    }
                    // 
                    break; //
                           // 
                           // TODO 20200316 新增
                           // 
                case E_STATUSTYPE_dataConnect:
                    // 
                    if (bOn)
                    // 
                    {
                        // 
                        //pictureEdit_dataConnect.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_green.png"); //
                        SetLedStatus(ImageUDP, LED_STATUS.LED_GREEN);
                    }
                    // 
                    else
                    // 
                    {
                        // 
                        // pictureEdit_dataConnect.Image = Image.FromFile(Application.StartupPath + @"\Image\LED_gray.png"); //
                        SetLedStatus(ImageUDP, LED_STATUS.LED_GRAY);
                    }
                    // 
                    break; //
                           // 
                default:
                    // 
                    break; //
                           // 
            }
            // 
        }
#endif



        private enum LED_STATUS
        {
            LED_RED,
            LED_GREEN,
            LED_GRAY
        }

        private enum NETWORK_DATA_TYPE
        {
            SLOW,
            FAST,
            TAIL,
            FLY
        }

        Dictionary<LED_STATUS, BitmapImage> LedImages = new Dictionary<LED_STATUS, BitmapImage> {
            { LED_STATUS.LED_GRAY, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_gray.png")) },
            { LED_STATUS.LED_GREEN, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_green.png")) },
            { LED_STATUS.LED_RED, new BitmapImage(new Uri("pack://siteoforigin:,,,/Image/LED_red.png")) }
        };

        Dictionary<NETWORK_DATA_TYPE, DateTime> NetworkDateRecvTime = new Dictionary<NETWORK_DATA_TYPE, DateTime> {
            {NETWORK_DATA_TYPE.SLOW, DateTime.MinValue },
            {NETWORK_DATA_TYPE.FAST, DateTime.MinValue },
            {NETWORK_DATA_TYPE.TAIL, DateTime.MinValue },
            {NETWORK_DATA_TYPE.FLY, DateTime.MinValue },
        };

        private bool bRun;
        public static readonly int CHART_MAX_POINTS = 500;
        private TestInfo testInfo = null;
        private UdpClient udpClientEnv = null;
        private UdpClient udpClientFly = null;
        private EnvParser envParser = null;
        private FlyParser flyParser = null;
        private DispatcherTimer uiRefreshTimer = new DispatcherTimer();
        private DispatcherTimer ledTimer = new DispatcherTimer();
        private DisplayBuffers displayBuffers = new DisplayBuffers();
        private DataLogger dataLogger = null;
        ChartDataSource chartDataSource = new ChartDataSource();
        YaoCeChartDataSource yaoCeChartDataSource = new YaoCeChartDataSource();
        Ratios ratios;

        //
        private UdpClient udpClientYaoCe = null;
        private DataParser yaoceParser = null;

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

        //帧序号
        private DispatcherTimer timerChartUpdate_ZXH = new DispatcherTimer();

        //UDP
        private DispatcherTimer timerUpdateUDP = new DispatcherTimer();//
        private DispatcherTimer timerOffLineUDP = new DispatcherTimer();

        System.Timers.Timer readFileTimer = new System.Timers.Timer(); //读取文件定时器

        public MainWindow()
        {
            InitializeComponent();
            InitSeriesDataSource();
            InitYaoCeChartDataSource();
            uiRefreshTimer.Tick += UiRefreshTimer_Tick;
            uiRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            ledTimer.Tick += LedTimer_Tick;
            ledTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            UpdateSyncFireDisplay(Double.NaN);
            InitProgramDiagram();
            InitLedStatus();
            
            InitTimer_YaoCe();

            // 创建新的日志文件
            Logger.GetInstance().NewFile();
            Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_INFO, "程序开始启动！");
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
            //timerUpdateXiTongJiShiStatus.Tick += timerUpdateXiTongJiShiStatus_Tick;

            timerUpdateChart_XiTongJiShi.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            //this.timerUpdateChart_XiTongJiShi.Tick += timerUpdateChart_XiTongJiShi_Tick;

            timerOfflineXiTongJiShiStatus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            //this.timerOfflineXiTongJiShiStatus.Tick += timerOfflineXiTongJiShiStatus_Tick;
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

        //更新定时器状态
        private void setUpdateTimerStatus(bool bOpen)
        {
            if (bOpen)
            {
                UpdateXiTongStatusTimer.Start();  
                UpdateHuiLuJianCeTimer.Start(); 
                timerUpdateUDP.Start();  
            }
            else
            {
                UpdateXiTongStatusTimer.Stop();  
                UpdateHuiLuJianCeTimer.Stop();  
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
                        string fileName =  load.getLoadFileName();  
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
           // GenericFunction.reSetAllTextEdit(TabPage_XiTongPanJue); 
            //GenericFunction.reSetAllTextEdit(xtraTabPage_HuiLuJianCe);  

            // 是否收到数据
            bRecvStatusData_XiTong = false; 
            bRecvStatusData_HuiLuJianCe = false;  
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

        private void LedTimer_Tick(object sender, EventArgs e)
        {
            DateTime Now = DateTime.Now;
            if((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.SLOW]).TotalMilliseconds > 200)
            {
                SetLedStatus(ImageSlow, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.FAST]).TotalMilliseconds > 200)
            {
                SetLedStatus(ImageFast, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.TAIL]).TotalMilliseconds > 200)
            {
                SetLedStatus(ImageTail, LED_STATUS.LED_RED);
            }
            if ((Now - NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY]).TotalMilliseconds > 200)
            {
                SetLedStatus(ImageFly, LED_STATUS.LED_RED);
            }
        }

        private void InitLedStatus()
        {
            SetLedStatus(ImageSlow, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageFast, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageTail, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageFly, LED_STATUS.LED_GRAY);
            SetLedStatus(ImageUDP, LED_STATUS.LED_GRAY);
            SetLedStatus( imageJianCe, LED_STATUS.LED_GRAY);
            SetLedStatus( imageJiShi, LED_STATUS.LED_GRAY);
            SetLedStatus( imageKuaiSu, LED_STATUS.LED_GRAY);
            SetLedStatus( imageManSu, LED_STATUS.LED_GRAY);
            SetLedStatus( imageZhuangTai, LED_STATUS.LED_GRAY);
        }

        private void SetLedStatus(Image imageControl, LED_STATUS status)
        {
            if(!bRun)
            {
                imageControl.Source = LedImages[LED_STATUS.LED_GRAY];
                return;
            }
            if (imageControl.Source != LedImages[status])
            {
                imageControl.Source = LedImages[status];
            }
        }

        private void InitSeriesDataSource()
        {
            SeriesHood.DataSource = chartDataSource.SlowHoodList;
            SeriesInsAir.DataSource = chartDataSource.SlowInsAirList;
            SeriesInsWall.DataSource = chartDataSource.SlowInsWallList;
            SeriesAttAir.DataSource = chartDataSource.SlowAttAirList;
            SeriesAttWall1.DataSource = chartDataSource.SlowAttWallList1;
            SeriesAttWall2.DataSource = chartDataSource.SlowAttWallList2;
            SeriesAttWall3.DataSource = chartDataSource.SlowAttWallList3;
            SeriesAttWall4.DataSource = chartDataSource.SlowAttWallList4;
            SeriesAttWall5.DataSource = chartDataSource.SlowAttWallList5;
            SeriesAttWall6.DataSource = chartDataSource.SlowAttWallList6;
            SeriesInsPresure.DataSource = chartDataSource.SlowInsPresureList;
            SeriesAttPresure.DataSource = chartDataSource.SlowAttPresureList;
            SeriesLevel2Presure.DataSource = chartDataSource.SlowLevel2PresureList;
            SeriesAttPresureHigh.DataSource = chartDataSource.SlowPresureHighList;
            SeriesAttPresureLow.DataSource = chartDataSource.SlowPresureLowList;

            SeriesShake1.DataSource = chartDataSource.FastShakeSeriesLists[0];
            SeriesShake2.DataSource = chartDataSource.FastShakeSeriesLists[1];
            SeriesShake3.DataSource = chartDataSource.FastShakeSeriesLists[2];
            SeriesShake4.DataSource = chartDataSource.FastShakeSeriesLists[3];
            SeriesShake5.DataSource = chartDataSource.FastShakeSeriesLists[4];
            SeriesShake6.DataSource = chartDataSource.FastShakeSeriesLists[5];
            SeriesShake7.DataSource = chartDataSource.FastShakeSeriesLists[6];
            SeriesShake8.DataSource = chartDataSource.FastShakeSeriesLists[7];
            SeriesShake9.DataSource = chartDataSource.FastShakeSeriesLists[8];
            SeriesShake10.DataSource = chartDataSource.FastShakeSeriesLists[9];
            SeriesShake11.DataSource = chartDataSource.FastShakeSeriesLists[10];
            SeriesShake12.DataSource = chartDataSource.FastShakeSeriesLists[11];
            SeriesLash1_1.DataSource = chartDataSource.FastLashSeriesLists1[0];
            SeriesLash1_2.DataSource = chartDataSource.FastLashSeriesLists1[1];
            SeriesLash1_3.DataSource = chartDataSource.FastLashSeriesLists1[2];
            SeriesLash2.DataSource = chartDataSource.FastLashSeriesList2;
            SeriesNoise1.DataSource = chartDataSource.FastNoiseLists[0];
            SeriesNoise2.DataSource = chartDataSource.FastNoiseLists[1];

            SeriesPresure.DataSource = chartDataSource.TailPresureList;
            SeriesLevel1.DataSource = chartDataSource.TailLevel1PresureList;
            SeriesTemperature1.DataSource = chartDataSource.TailTemperature1List;
            SeriesTemperature2.DataSource = chartDataSource.TailTemperature2List;
            SeriesShake1X.DataSource = chartDataSource.TailLash1XList;
            SeriesShake1Y.DataSource = chartDataSource.TailLash1YList;
            SeriesShake1Z.DataSource = chartDataSource.TailLash1ZList;
            SeriesShake2X.DataSource = chartDataSource.TailLash2XList;
            SeriesShake2Y.DataSource = chartDataSource.TailLash2YList;
            SeriesShake2Z.DataSource = chartDataSource.TailLash2ZList;
            SeriesNoise.DataSource = chartDataSource.TailNoiseList;

            SeriesNavLat.DataSource = chartDataSource.NavLat;
            SeriesNavLon.DataSource = chartDataSource.NavLon;
            SeriesNavHeight.DataSource = chartDataSource.NavHeight;
            SeriesNavSpeedNorth.DataSource = chartDataSource.NavSpeedNorth;
            SeriesNavSpeedSky.DataSource = chartDataSource.NavSpeedSky;
            SeriesNavSpeedEast.DataSource = chartDataSource.NavSpeedEast;
            SeriesNavPitchAngle.DataSource = chartDataSource.NavPitchAngle;
            SeriesNavCrabAngle.DataSource = chartDataSource.NavCrabAngle;
            SeriesNavRollAngle.DataSource = chartDataSource.NavRollAngle;

            SeriesAccX.DataSource = chartDataSource.AngleAccXList;
            SeriesAccY.DataSource = chartDataSource.AngleAccYList;
            SeriesAccZ.DataSource = chartDataSource.AngleAccZList;
            SeriesAngelX.DataSource = chartDataSource.AngleXList;
            SeriesAngelY.DataSource = chartDataSource.AngleYList;
            SeriesAngelZ.DataSource = chartDataSource.AngleZList;

            SeriesServoVol28.DataSource = chartDataSource.ServoVol28List;
            SeriesServoVol160.DataSource = chartDataSource.ServoVol160List;
            SeriesServo1Iq.DataSource = chartDataSource.Servo1IqList;
            SeriesServo2Iq.DataSource = chartDataSource.Servo2IqList;
            SeriesServo3Iq.DataSource = chartDataSource.Servo3IqList;
            SeriesServo4Iq.DataSource = chartDataSource.Servo4IqList;

            SeriesTailSequence.DataSource = chartDataSource.TailSequenceList;
            SeriesAngelSequence.DataSource = chartDataSource.AngelSequenceList;
            SeriesServoSequence.DataSource = chartDataSource.ServoSequenceList;
            SeriesNavSequence.DataSource = chartDataSource.NavSequenceList;
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

        private void InitProgramDiagram()
        {
            ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(-1);
            GpsTime.Text = "--";
            programDigram.SetLinePoints(new Point(0.1, 0.9), new Point(0.5, -0.8), new Point(0.9, 0.9));
            FlyProtocol.GetPoints().ForEach(point => programDigram.AddPoint(point.Value, point.Key));
        }

        private void ResetProgramDiagram()
        {
            ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(-1);
            GpsTime.Text = "--";
            programDigram.Reset();
        }

        private void UiRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (displayBuffers.SlowPacketList.Count > 0)
            {
                DrawSlowPackets(displayBuffers.SlowPacketList);
                UpdateSyncFireDisplay(displayBuffers.SlowPacketList[displayBuffers.SlowPacketList.Count - 1].syncFire * ratios.Fire + ratios.FireFix);
            }

            if(displayBuffers.FastPacketList.Count > 0)
            {
                DrawFastPackets(displayBuffers.FastPacketList);
            }

            if(displayBuffers.TailPacketList.Count > 0)
            {
                DrawTailPackets(displayBuffers.TailPacketList);
            }

            if(displayBuffers.NavDataList.Count > 0)
            {
                DrawNavPackets(displayBuffers.NavDataList);
            }

            if(displayBuffers.AngelDataList.Count > 0)
            {
                DrawAngelPackets(displayBuffers.AngelDataList);
            }

            if(displayBuffers.ProgramControlDataList.Count > 0)
            {
                DrawProgramPackets(displayBuffers.ProgramControlDataList);
            }

            if(displayBuffers.ServoDataList.Count > 0)
            {
                DrawServoPackets(displayBuffers.ServoDataList);
            }

            displayBuffers.Clear();
        }

        private void UpdateSyncFireDisplay( double value)
        {
            if(value.Equals(Double.NaN))
            {
                labelSyncFire.Content = String.Format("点火同步信号:\t--");
            }
            else
            {
                labelSyncFire.Content = String.Format("点火同步信号:\t{0:F}", value);
            }
        }

        protected void DrawSlowPackets(List<SlowPacket> packets)
        {
            packets.ForEach(packet =>
            {
                for (int i = 0; i < 2; ++i)
                {
                    chartDataSource.SlowHoodList.AddPoint(packet.temperatureSensor.hood[i] * ratios.HoodTemp + ratios.HoodTempFix);
                    chartDataSource.SlowInsAirList.AddPoint(packet.temperatureSensor.insAir[i] * ratios.InsAirTemp + ratios.InsAirTempFix);
                    chartDataSource.SlowInsWallList.AddPoint(packet.temperatureSensor.insWall[i] * ratios.InsWallTemp + ratios.InsWallTempFix);
                    chartDataSource.SlowAttAirList.AddPoint(packet.temperatureSensor.attAir[i] * ratios.AttAirTemp + ratios.AttAirTempFix);
                }
                chartDataSource.SlowAttWallList1.AddPoint(packet.temperatureSensor.attWalls[0] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix);
                chartDataSource.SlowAttWallList1.AddPoint(packet.temperatureSensor.attWalls[1] * ratios.AttWalls1Temp + ratios.AttWalls1TempFix);
                chartDataSource.SlowAttWallList2.AddPoint(packet.temperatureSensor.attWalls[2] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix);
                chartDataSource.SlowAttWallList2.AddPoint(packet.temperatureSensor.attWalls[3] * ratios.AttWalls2Temp + ratios.AttWalls2TempFix);
                chartDataSource.SlowAttWallList3.AddPoint(packet.temperatureSensor.attWalls[4] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix);
                chartDataSource.SlowAttWallList3.AddPoint(packet.temperatureSensor.attWalls[5] * ratios.AttWalls3Temp + ratios.AttWalls3TempFix);
                chartDataSource.SlowAttWallList4.AddPoint(packet.temperatureSensor.attWalls[6] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix);
                chartDataSource.SlowAttWallList4.AddPoint(packet.temperatureSensor.attWalls[7] * ratios.AttWalls4Temp + ratios.AttWalls4TempFix);
                chartDataSource.SlowAttWallList5.AddPoint(packet.temperatureSensor.attWalls[8] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix);
                chartDataSource.SlowAttWallList5.AddPoint(packet.temperatureSensor.attWalls[9] * ratios.AttWalls5Temp + ratios.AttWalls5TempFix);
                chartDataSource.SlowAttWallList6.AddPoint(packet.temperatureSensor.attWalls[10] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix);
                chartDataSource.SlowAttWallList6.AddPoint(packet.temperatureSensor.attWalls[11] * ratios.AttWalls6Temp + ratios.AttWalls6TempFix);
                for (int i = 0; i < 2; ++i)
                {
                    chartDataSource.SlowInsPresureList.AddPoint(packet.pressureSensor.instrument[i] * ratios.InsPresure + ratios.InsPresureFix);
                    chartDataSource.SlowAttPresureList.AddPoint(packet.pressureSensor.attitudeControl[i] * ratios.AttiPresure + ratios.AttiPresureFix);
                    chartDataSource.SlowLevel2PresureList.AddPoint(packet.level2Transmitter[i] * ratios.Level2TransmitterPresure + ratios.Level2TransmitterPresureFix);
                    chartDataSource.SlowPresureHighList.AddPoint(packet.gestureControlHigh[i] * ratios.GestureControlHighPresure + ratios.GestureControlHighPresureFix);
                    chartDataSource.SlowPresureLowList.AddPoint(packet.gestureControlLow[i] * ratios.GestureControlLowPresure + ratios.GestureControlLowPresureFix);
                }
            });
            chartDataSource.SlowHoodList.NotifyDataChanged();
            chartDataSource.SlowInsAirList.NotifyDataChanged();
            chartDataSource.SlowInsWallList.NotifyDataChanged();
            chartDataSource.SlowAttAirList.NotifyDataChanged();
            chartDataSource.SlowAttWallList1.NotifyDataChanged();
            chartDataSource.SlowAttWallList2.NotifyDataChanged();
            chartDataSource.SlowAttWallList3.NotifyDataChanged();
            chartDataSource.SlowAttWallList4.NotifyDataChanged();
            chartDataSource.SlowAttWallList5.NotifyDataChanged();
            chartDataSource.SlowAttWallList6.NotifyDataChanged();
            chartDataSource.SlowInsPresureList.NotifyDataChanged();
            chartDataSource.SlowAttPresureList.NotifyDataChanged();
            chartDataSource.SlowLevel2PresureList.NotifyDataChanged();
            chartDataSource.SlowPresureHighList.NotifyDataChanged();
            chartDataSource.SlowPresureLowList.NotifyDataChanged();

            
        }

        void DrawFastPackets(List<FastPacket> packets)
        {
            packets.ForEach(packet =>
            {
                for (int idx = 0; idx < 80; ++idx)
                {
                    FastShakeSignal fastShakeSignal = packet.shakeSignals[idx];
                    /*for (int pos = 0; pos < 12; ++pos)
                    {
                        chartDataSource.FastShakeSeriesLists[pos].AddPoint(fastShakeSignal.signal[pos] * ratios.fastShake + ratios.fastShakeFix);
                    }*/
                    chartDataSource.FastShakeSeriesLists[0].AddPoint(fastShakeSignal.signal[0] * ratios.Shake1 + ratios.Shake1Fix);
                    chartDataSource.FastShakeSeriesLists[1].AddPoint(fastShakeSignal.signal[1] * ratios.Shake2 + ratios.Shake2Fix);
                    chartDataSource.FastShakeSeriesLists[2].AddPoint(fastShakeSignal.signal[2] * ratios.Shake3 + ratios.Shake3Fix);
                    chartDataSource.FastShakeSeriesLists[3].AddPoint(fastShakeSignal.signal[3] * ratios.Shake4 + ratios.Shake4Fix);
                    chartDataSource.FastShakeSeriesLists[4].AddPoint(fastShakeSignal.signal[4] * ratios.Shake5 + ratios.Shake5Fix);
                    chartDataSource.FastShakeSeriesLists[5].AddPoint(fastShakeSignal.signal[5] * ratios.Shake6 + ratios.Shake6Fix);
                    chartDataSource.FastShakeSeriesLists[6].AddPoint(fastShakeSignal.signal[6] * ratios.Shake7 + ratios.Shake7Fix);
                    chartDataSource.FastShakeSeriesLists[7].AddPoint(fastShakeSignal.signal[7] * ratios.Shake8 + ratios.Shake8Fix);
                    chartDataSource.FastShakeSeriesLists[8].AddPoint(fastShakeSignal.signal[8] * ratios.Shake9 + ratios.Shake9Fix);
                    chartDataSource.FastShakeSeriesLists[9].AddPoint(fastShakeSignal.signal[9] * ratios.Shake10 + ratios.Shake10Fix);
                    chartDataSource.FastShakeSeriesLists[10].AddPoint(fastShakeSignal.signal[10] * ratios.Shake11 + ratios.Shake11Fix);
                    chartDataSource.FastShakeSeriesLists[11].AddPoint(fastShakeSignal.signal[11] * ratios.Shake12 + ratios.Shake12Fix);
                }
                chartDataSource.FastLashT3SeriesList.AddPoint(packet.lashT3);
                chartDataSource.FastLashT2SeriesList.AddPoint(packet.lashT2);
                chartDataSource.FastLashT1SeriesList.AddPoint(packet.lashT1);
                chartDataSource.FastLashT0SeriesList.AddPoint(packet.lashT0);

                foreach(FastLashSignal fastLashSignal in packet.lashSignal)
                {
                    chartDataSource.FastLashSeriesLists1[0].AddPoint(fastLashSignal.signal[0] * ratios.Lash1_1 + ratios.Lash1_1Fix);
                    chartDataSource.FastLashSeriesLists1[1].AddPoint(fastLashSignal.signal[1] * ratios.Lash1_2 + ratios.Lash1_2Fix);
                    chartDataSource.FastLashSeriesLists1[2].AddPoint(fastLashSignal.signal[2] * ratios.Lash1_3 + ratios.Lash1_3Fix);
                    chartDataSource.FastLashSeriesList2.AddPoint(fastLashSignal.signal[3] * ratios.Lash2 + ratios.Lash2Fix);
                }
                for (int pos = 0; pos < 400; ++pos)
                {
                    chartDataSource.FastNoiseLists[0].AddPoint(packet.noiseSignal[pos].signal[0] * ratios.Noise1 + ratios.Noise1Fix);
                    chartDataSource.FastNoiseLists[1].AddPoint(packet.noiseSignal[pos].signal[1] * ratios.Noise2 + ratios.Noise2Fix);
                }
            });
            chartDataSource.FastShakeSeriesLists.ForEach(source => source.NotifyDataChanged());
            chartDataSource.FastLashT3SeriesList.NotifyDataChanged();
            chartDataSource.FastLashT2SeriesList.NotifyDataChanged();
            chartDataSource.FastLashT1SeriesList.NotifyDataChanged();
            chartDataSource.FastLashT0SeriesList.NotifyDataChanged();
            chartDataSource.FastLashSeriesLists1.ForEach(source => source.NotifyDataChanged());
            chartDataSource.FastLashSeriesList2.NotifyDataChanged();
            chartDataSource.FastNoiseLists.ForEach(source => source.NotifyDataChanged());
        }

        private void DrawTailPackets(List<TailPacketRs> tailPackets)
        {
            List<double>[] seriesPoints = new List<double>[(uint)ChannelType.ChannelMax];
            for (int i = 0; i < seriesPoints.Length; ++i)
            {
                seriesPoints[i] = new List<double>();
            }
            tailPackets.ForEach(packet =>
            {
                chartDataSource.TailSequenceList.AddPoint(packet.sequence);
                foreach (ushort data in packet.channels)
                {
                    uint channel = data.Channel();
                    if (channel < (uint)ChannelType.ChannelMax)
                    {
                        double value = 0;
                        switch ((ChannelType)channel)
                        {
                            case ChannelType.ChannelPresure:
                                value = data.Data() * ratios.TailPresure + ratios.TailPresureFix;
                                break;
                            case ChannelType.ChannelLevel1Presure:
                                value = data.Data() * ratios.Level1Presure + ratios.Level1PresureFix;
                                break;
                            case ChannelType.ChannelTemperature1:
                                value = data.Data() * ratios.Temperature1Temp + ratios.Temperature1TempFix;
                                break;
                            case ChannelType.ChannelTemperature2:
                                value = data.Data() * ratios.Temperature2Temp + ratios.Temperature2TempFix;
                                break;
                            case ChannelType.Channel1ShakeX:
                                value = data.Data() * ratios.Shake1X + ratios.Shake1XFix;
                                break;
                            case ChannelType.Channel1ShakeY:
                                value = data.Data() * ratios.Shake1Y + ratios.Shake1YFix;
                                break;
                            case ChannelType.Channel1ShakeZ:
                                value = data.Data() * ratios.Shake1Z + ratios.Shake1ZFix;
                                break;
                            case ChannelType.Channel2ShakeX:
                                value = data.Data() * ratios.Shake2X + ratios.Shake2XFix;
                                break;
                            case ChannelType.Channel2ShakeY:
                                value = data.Data() * ratios.Shake2Y + ratios.Shake2YFix;
                                break;
                            case ChannelType.Channel2ShakeZ:
                                value = data.Data() * ratios.Shake2Z + ratios.Shake2ZFix;
                                break;
                            case ChannelType.ChannelNoise:
                                value = data.Data() * ratios.Noise + ratios.NoiseFix;
                                break;
                            default:
                                break;
                        }
                        seriesPoints[channel].Add(value);
                    }
                }
            });

            chartDataSource.TailPresureList.AddPoints(seriesPoints[(int)ChannelType.ChannelPresure]);
            chartDataSource.TailLevel1PresureList.AddPoints(seriesPoints[(int)ChannelType.ChannelLevel1Presure]);
            chartDataSource.TailTemperature1List.AddPoints(seriesPoints[(int)ChannelType.ChannelTemperature1]);
            chartDataSource.TailTemperature2List.AddPoints(seriesPoints[(int)ChannelType.ChannelTemperature2]);
            chartDataSource.TailLash1XList.AddPoints(seriesPoints[(int)ChannelType.Channel1ShakeX]);
            chartDataSource.TailLash1YList.AddPoints(seriesPoints[(int)ChannelType.Channel1ShakeY]);
            chartDataSource.TailLash1ZList.AddPoints(seriesPoints[(int)ChannelType.Channel1ShakeZ]);
            chartDataSource.TailLash2XList.AddPoints(seriesPoints[(int)ChannelType.Channel2ShakeX]);
            chartDataSource.TailLash2YList.AddPoints(seriesPoints[(int)ChannelType.Channel2ShakeY]);
            chartDataSource.TailLash2ZList.AddPoints(seriesPoints[(int)ChannelType.Channel2ShakeZ]);
            chartDataSource.TailNoiseList.AddPoints(seriesPoints[(int)ChannelType.ChannelNoise]);
            chartDataSource.TailPresureList.NotifyDataChanged();
            chartDataSource.TailLevel1PresureList.NotifyDataChanged();
            chartDataSource.TailTemperature1List.NotifyDataChanged();
            chartDataSource.TailTemperature2List.NotifyDataChanged();
            chartDataSource.TailLash1XList.NotifyDataChanged();
            chartDataSource.TailLash1YList.NotifyDataChanged();
            chartDataSource.TailLash1ZList.NotifyDataChanged();
            chartDataSource.TailLash2XList.NotifyDataChanged();
            chartDataSource.TailLash2YList.NotifyDataChanged();
            chartDataSource.TailLash2ZList.NotifyDataChanged();
            chartDataSource.TailNoiseList.NotifyDataChanged();
            chartDataSource.TailSequenceList.NotifyDataChanged();
        }

        private void DrawNavPackets(List<NavData> navDataList)
        {
            navDataList.ForEach(packet =>
            {
                programDigram.AddNavData(packet);
                chartDataSource.NavLat.AddPoint(packet.latitude);
                chartDataSource.NavLon.AddPoint(packet.longitude);
                chartDataSource.NavHeight.AddPoint(packet.height);
                chartDataSource.NavSpeedNorth.AddPoint(packet.northSpeed);
                chartDataSource.NavSpeedSky.AddPoint(packet.skySpeed);
                chartDataSource.NavSpeedEast.AddPoint(packet.eastSpeed);
                chartDataSource.NavPitchAngle.AddPoint(packet.pitchAngle);
                chartDataSource.NavCrabAngle.AddPoint(packet.crabAngle);
                chartDataSource.NavRollAngle.AddPoint(packet.rollAngle);
                chartDataSource.NavSequenceList.AddPoint(packet.sequence);
                mapControl.AddTrackPoint(packet.longitude, packet.latitude);
            });

            chartDataSource.NavLat.NotifyDataChanged();
            chartDataSource.NavLon.NotifyDataChanged();
            chartDataSource.NavHeight.NotifyDataChanged();
            chartDataSource.NavSpeedNorth.NotifyDataChanged();
            chartDataSource.NavSpeedSky.NotifyDataChanged();
            chartDataSource.NavSpeedEast.NotifyDataChanged();
            chartDataSource.NavPitchAngle.NotifyDataChanged();
            chartDataSource.NavCrabAngle.NotifyDataChanged();
            chartDataSource.NavRollAngle.NotifyDataChanged();
            chartDataSource.NavSequenceList.NotifyDataChanged();

            NavData lastNavData = navDataList[navDataList.Count - 1];
            GpsTime.Text = String.Format("{0:F}S", lastNavData.gpsTime);
        }

        private void DrawAngelPackets(List<AngleData> angleDataList)
        {
            angleDataList.ForEach(packet =>
            {
                programDigram.AddAngleData(packet);
                chartDataSource.AngleAccXList.AddPoint(packet.ax);
                chartDataSource.AngleAccYList.AddPoint(packet.ay);
                chartDataSource.AngleAccZList.AddPoint(packet.az);
                chartDataSource.AngleXList.AddPoint(packet.angleX);
                chartDataSource.AngleYList.AddPoint(packet.angleY);
                chartDataSource.AngleZList.AddPoint(packet.angleZ);
                chartDataSource.AngelSequenceList.AddPoint(packet.sequence);
            });
            chartDataSource.AngleAccXList.NotifyDataChanged();
            chartDataSource.AngleAccYList.NotifyDataChanged();
            chartDataSource.AngleAccZList.NotifyDataChanged();
            chartDataSource.AngleXList.NotifyDataChanged();
            chartDataSource.AngleYList.NotifyDataChanged();
            chartDataSource.AngleZList.NotifyDataChanged();
            chartDataSource.AngelSequenceList.NotifyDataChanged();
        }

        private void DrawProgramPackets(List<ProgramControlData> programDataList)
        {
            programDataList.ForEach(packet =>
            {
                ProgramControlStatus.Text = FlyProtocol.GetProgramControlStatusDescription(packet.controlStatus) + String.Format("({0})", packet.controlStatus);
                programDigram.AddProgramData(packet);
            });
        }

        private void DrawServoPackets(List<ServoData> servoDataList)
        {
            servoDataList.ForEach(packet =>
            {
                programDigram.AddServoData(packet);
                chartDataSource.ServoVol28List.AddPoint(FlyDataConvert.GetVoltage28(packet.vol28));
                chartDataSource.ServoVol160List.AddPoint(FlyDataConvert.GetVoltage160(packet.vol160));
                chartDataSource.Servo1IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq1));
                chartDataSource.Servo2IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq2));
                chartDataSource.Servo3IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq3));
                chartDataSource.Servo4IqList.AddPoint(FlyDataConvert.GetElectricity(packet.Iq4));
                chartDataSource.ServoSequenceList.AddPoint(packet.sequence);
            });

            chartDataSource.ServoVol28List.NotifyDataChanged();
            chartDataSource.ServoVol160List.NotifyDataChanged();
            chartDataSource.Servo1IqList.NotifyDataChanged();
            chartDataSource.Servo2IqList.NotifyDataChanged();
            chartDataSource.Servo3IqList.NotifyDataChanged();
            chartDataSource.Servo4IqList.NotifyDataChanged();
            chartDataSource.ServoSequenceList.NotifyDataChanged();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            NETWORK_DATA_TYPE[] keys = NetworkDateRecvTime.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                NetworkDateRecvTime[keys[i]] = DateTime.MinValue;
            }

            if (envParser == null)
            {
                envParser = new EnvParser(new WindowInteropHelper(this).Handle);
            }
            if(flyParser == null)
            {
                flyParser = new FlyParser(new WindowInteropHelper(this).Handle);
            }
            if (yaoceParser == null)
            {
                yaoceParser = new DataParser(new WindowInteropHelper(this).Handle);
            }

            testInfo = new TestInfo
            {
                TestName = String.Empty,
                Operator = String.Empty,
                Comment = String.Empty,
                TestTime = DateTime.Now
            };

            SettingManager settingManager = new SettingManager();
            if(!settingManager.LoadRatios(out ratios))
            {
                MessageBox.Show("加载系数配置文件失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int maxDisplayPoint;
            try
            {
                
                if (settingManager.LoadNetworkSetting(out String envIpAddr, out int envPort, 
                                                      out String flyIpAddr, out int flyPort, 
                                                      out String yaoceIpAddr,out int yaocePort,
                                                      out maxDisplayPoint))
                {
                    udpClientEnv = new UdpClient(envPort);
                    udpClientEnv.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientEnv.JoinMulticastGroup(IPAddress.Parse(envIpAddr));
                    udpClientFly = new UdpClient(flyPort);
                    udpClientFly.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientFly.JoinMulticastGroup(IPAddress.Parse(flyIpAddr));

                    //
                    udpClientYaoCe = new UdpClient(yaocePort);
                    udpClientYaoCe.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024 * 1024 * 200);
                    udpClientYaoCe.JoinMulticastGroup(IPAddress.Parse(yaoceIpAddr));
                }
                else
                {
                    MessageBox.Show("加载网络配置文件失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                udpClientEnv?.Close();
                udpClientFly?.Close();
                udpClientYaoCe?.Close();
                MessageBox.Show("加入组播组失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            btnSetting.IsEnabled = false;
            btnHistory.IsEnabled = false;
            btnOpenData.IsEnabled = false;
            btnData.IsEnabled = true;
            ResetDisplay(maxDisplayPoint);
            displayBuffers.Clear();
            dataLogger = new DataLogger(testInfo.TestTime);
            envParser.dataLogger = dataLogger;
            flyParser.dataLogger = dataLogger;

            yaoceDataLogger.Start();
            envParser.Start();
            flyParser.Start();
            yaoceParser.Start();

            udpClientEnv.BeginReceive(EndEnvReceive, null);
            udpClientFly.BeginReceive(EndFlyReceive, null);
            udpClientYaoCe.BeginReceive(EndYaoCeReceive, null);
            uiRefreshTimer.Start();
            ledTimer.Start();
            mapControl.Clean();
            bRun = true;
        }

        private void EndEnvReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] recvBuffer = udpClientEnv?.EndReceive(ar, ref endPoint);
                if (recvBuffer != null)
                {
                    envParser.Enqueue(recvBuffer);
                }
                udpClientEnv?.BeginReceive(EndEnvReceive, null);
            }
            catch (Exception)
            { }
        }

        private void EndFlyReceive(IAsyncResult ar)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] recvBuffer = udpClientFly?.EndReceive(ar, ref endPoint);
                if(recvBuffer != null)
                {
                    flyParser.Enqueue(recvBuffer);
                    if (recvBuffer.Length >= Marshal.SizeOf(typeof(FlyHeader)))
                    {
                        for (int i = 0; i < recvBuffer.Length - Marshal.SizeOf(typeof(FlyHeader)); ++i)
                        {
                            if (recvBuffer[i] == FlyProtocol.syncHeader[0] && recvBuffer[i + 1] == FlyProtocol.syncHeader[1]
                                && recvBuffer[i + 2] == FlyProtocol.syncHeader[2])
                            {
                                NetworkDateRecvTime[NETWORK_DATA_TYPE.FLY] = DateTime.Now;
                                Dispatcher.Invoke(new Action<Image, LED_STATUS>(SetLedStatus), ImageFly, LED_STATUS.LED_GREEN);
                            }
                        }
                    }
                }
                udpClientFly?.BeginReceive(EndFlyReceive, null);
            }
            catch (Exception)
            { }
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
                udpClientYaoCe.BeginReceive(EndYaoCeReceive, null); //
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

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            bRun = false;
            try
            {
                udpClientEnv?.Close();
                udpClientFly?.Close();
                udpClientYaoCe?.Close();
            }
            catch (Exception) { }
            udpClientEnv = null;
            udpClientFly = null;
            udpClientYaoCe = null;
            if(envParser != null)
            {
                envParser.IsStartLogData = false;
                envParser.Stop();
            }
            if(flyParser != null)
            {
                flyParser.IsStartLogData = false;
                flyParser.Stop();
            }
            if(yaoceParser != null)
            {
                yaoceParser.Stop();
            }
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            btnSetting.IsEnabled = true;
            btnHistory.IsEnabled = true;
            btnOpenData.IsEnabled = true;
            btnData.IsChecked = false;
            btnData.IsEnabled = false;
            btnData.Content = "开始存储数据";
            uiRefreshTimer.Stop();
            dataLogger?.Close();
            yaoceDataLogger.Stop();//

            SaveTestInfo();
            InitLedStatus();
            ledTimer.Stop();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                IntPtr handle = hwndSource.Handle;
                hwndSource.AddHook(new HwndSourceHook(WndProc));
                hwndSource.AddHook(new HwndSourceHook(DefWndProc));
            }
        }

        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

#if true
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
                if(yaoceParser != null)
                {
                    yaoceParser.Stop();
                }
                // 关闭绘图定时器刷新数据
                //setTimerUpdateChartStatus(false); 

                // 停止加载文件进度
                UpdateLoadFileProgressTimer.Stop();
            }
#endif

            switch (msg)
            {
                case WinApi.WM_SLOW_DATA:
                    if (bRun) 
                    {
                        NetworkDateRecvTime[NETWORK_DATA_TYPE.SLOW] = DateTime.Now;
                        SetLedStatus(ImageSlow, LED_STATUS.LED_GREEN);
                    }
                    if (lParam != IntPtr.Zero)
                    {
                        ProcessSlowDataMessage(lParam);
                    }
                    break;
                case WinApi.WM_FAST_DATA:
                    if (bRun)
                    {
                        NetworkDateRecvTime[NETWORK_DATA_TYPE.FAST] = DateTime.Now;
                        SetLedStatus(ImageFast, LED_STATUS.LED_GREEN);
                    }
                    if (lParam != IntPtr.Zero)
                    {
                        ProcessFastDataMessage(lParam);
                    }
                    break;
                case WinApi.WM_TAIL_DATA:
                    if (bRun)
                    {
                        NetworkDateRecvTime[NETWORK_DATA_TYPE.TAIL] = DateTime.Now;
                        SetLedStatus(ImageTail, LED_STATUS.LED_GREEN);
                    }
                    if (lParam != IntPtr.Zero)
                    {
                        ProcessTailDataMessage(lParam);
                    }
                    break;
                case WinApi.WM_NAV_DATA:
                    ProcessNavMessage(lParam);
                    break;
                case WinApi.WM_ANGLE_DATA:
                    ProcessAngelData(lParam);
                    break;
                case WinApi.WM_PROGRAM_DATA:
                    ProcessProgramData(lParam);
                    break;
                case WinApi.WM_SERVO_DATA:
                    ProcessServoData(lParam);
                    break;
                default:
                    break;
            }

            return hwnd;
        }

        protected IntPtr DefWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
#if true
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
                        //xiTongJiShiSubForm_Ti.SetXiTongJiShiStatus(ref sObject); 
 
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
                       // xiTongJiShiSubForm_Tou.SetXiTongJiShiStatus(ref sObject); 

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
#endif
            return hwnd;
        }

        protected void ProcessSlowDataMessage(IntPtr msg)
        {
            SlowPacket packet = Marshal.PtrToStructure<SlowPacket>(msg);
            displayBuffers.SlowPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessFastDataMessage(IntPtr msg)
        {
            FastPacket packet = Marshal.PtrToStructure<FastPacket>(msg);
            displayBuffers.FastPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessTailDataMessage(IntPtr msg)
        {
            TailPacketRs packet = Marshal.PtrToStructure<TailPacketRs>(msg);
            displayBuffers.TailPacketList.Add(packet);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessNavMessage(IntPtr msg)
        {
            NavData data = Marshal.PtrToStructure<NavData>(msg);
            displayBuffers.NavDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessAngelData(IntPtr msg)
        {
            AngleData data = Marshal.PtrToStructure<AngleData>(msg);
            displayBuffers.AngelDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessProgramData(IntPtr msg)
        {
            ProgramControlData data = Marshal.PtrToStructure<ProgramControlData>(msg);
            displayBuffers.ProgramControlDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        protected void ProcessServoData(IntPtr msg)
        {
            ServoData data = Marshal.PtrToStructure<ServoData>(msg);
            displayBuffers.ServoDataList.Add(data);
            Marshal.FreeHGlobal(msg);
        }

        private void ResetDisplay(int maxDisplayPoint)
        {
            UpdateSyncFireDisplay(Double.NaN);
            ResetProgramDiagram();
            chartDataSource.Clear();
            chartDataSource.SetMaxDisplayCount(maxDisplayPoint);
        }

        private void SaveTestInfo()
        {
            if (testInfo != null)
            {
                try
                {
                    var db = new DatabaseDB();
                    db.Insert<DataModels.TestInfo>(new DataModels.TestInfo
                    {
                        TestName = testInfo.TestName,
                        Operator = testInfo.Operator,
                        Comment = testInfo.Comment,
                        Time = testInfo.TestTime
                    });
                    db.CommitTransaction();
                    File.Copy("params", Path.Combine("Log", testInfo.TestTime.ToString("yyyyMMddHHmmssfff"), "params"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存试验信息失败:" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            testInfo = null;
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.Owner = this;
            historyWindow.ShowDialog();
        }

        private void btnOpenData_Click(object sender, RoutedEventArgs e)
        {
            OpenDataWindow openDataWindow = new OpenDataWindow();
            openDataWindow.Owner = this;
            if((bool)openDataWindow.ShowDialog())
            {
                String flyFileName, slowFileName, fastFileName, tailFileName;
                openDataWindow.GetFileNames(out flyFileName, out slowFileName, out fastFileName, out tailFileName);
                HistoryDetailWindow historyDetailWindow = new HistoryDetailWindow(flyFileName, slowFileName, fastFileName, tailFileName);
                historyDetailWindow.ShowDialog();
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Owner = this;
            settingWindow.ShowDialog();
        }

        private void ThemedWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            btnStop_Click(this, new RoutedEventArgs());
        }

        private void ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            if((bool)toggleSwitch.IsChecked)
            {
                HideZeroLevel(ChartHood);
                HideZeroLevel(ChartInsAir);
                HideZeroLevel(ChartInsWall);
                HideZeroLevel(ChartAttAir);
                HideZeroLevel(ChartTemperature1);
                HideZeroLevel(ChartTemperature2);
                HideZeroLevel(ChartAttWalls1);
                HideZeroLevel(ChartAttWalls2);
                HideZeroLevel(ChartAttWalls3);
                HideZeroLevel(ChartAttWalls4);
                HideZeroLevel(ChartAttWalls5);
                HideZeroLevel(ChartAttWalls6);
                HideZeroLevel(ChartInsPresure);
                HideZeroLevel(ChartAttiPresure);
                HideZeroLevel(ChartPresure);
                HideZeroLevel(ChartLevel1Presure);
                HideZeroLevel(ChartLevel2Transmitter);
                HideZeroLevel(ChartGestureControlHigh);
                HideZeroLevel(ChartGestureControlLow);
                HideZeroLevel(ChartShake1);
                HideZeroLevel(ChartShake2);
                HideZeroLevel(ChartShake3);
                HideZeroLevel(ChartShake4);
                HideZeroLevel(ChartShake5);
                HideZeroLevel(ChartShake6);
                HideZeroLevel(ChartShake7);
                HideZeroLevel(ChartShake8);
                HideZeroLevel(ChartShake9);
                HideZeroLevel(ChartShake10);
                HideZeroLevel(ChartShake11);
                HideZeroLevel(ChartShake12);
                HideZeroLevel(ChartShake1X);
                HideZeroLevel(ChartShake1Y);
                HideZeroLevel(ChartShake1Z);
                HideZeroLevel(ChartShake2X);
                HideZeroLevel(ChartShake2Y);
                HideZeroLevel(ChartShake2Z);
                HideZeroLevel(ChartLash1_1);
                HideZeroLevel(ChartLash1_2);
                HideZeroLevel(ChartLash1_3);
                HideZeroLevel(ChartLash2);
                HideZeroLevel(ChartNoise1);
                HideZeroLevel(ChartNoise2);
                HideZeroLevel(ChartNoise);
            }
            else
            {
                SetFixedRange(ChartHood);
                SetFixedRange(ChartInsAir);
                SetFixedRange(ChartInsWall);
                SetFixedRange(ChartAttAir);
                SetFixedRange(ChartTemperature1);
                SetFixedRange(ChartTemperature2);
                SetFixedRange(ChartAttWalls1);
                SetFixedRange(ChartAttWalls2);
                SetFixedRange(ChartAttWalls3);
                SetFixedRange(ChartAttWalls4);
                SetFixedRange(ChartAttWalls5);
                SetFixedRange(ChartAttWalls6);
                SetFixedRange(ChartInsPresure);
                SetFixedRange(ChartAttiPresure);
                SetFixedRange(ChartPresure);
                SetFixedRange(ChartLevel1Presure);
                SetFixedRange(ChartLevel2Transmitter);
                SetFixedRange(ChartGestureControlHigh);
                SetFixedRange(ChartGestureControlLow);
                SetFixedRange(ChartShake1);
                SetFixedRange(ChartShake2);
                SetFixedRange(ChartShake3);
                SetFixedRange(ChartShake4);
                SetFixedRange(ChartShake5);
                SetFixedRange(ChartShake6);
                SetFixedRange(ChartShake7);
                SetFixedRange(ChartShake8);
                SetFixedRange(ChartShake9);
                SetFixedRange(ChartShake10);
                SetFixedRange(ChartShake11);
                SetFixedRange(ChartShake12);
                SetFixedRange(ChartShake1X);
                SetFixedRange(ChartShake1Y);
                SetFixedRange(ChartShake1Z);
                SetFixedRange(ChartShake2X);
                SetFixedRange(ChartShake2Y);
                SetFixedRange(ChartShake2Z);
                SetFixedRange(ChartLash1_1);
                SetFixedRange(ChartLash1_2);
                SetFixedRange(ChartLash1_3);
                SetFixedRange(ChartLash2);
                SetFixedRange(ChartNoise1);
                SetFixedRange(ChartNoise2);
                SetFixedRange(ChartNoise);
            }
        }

        private void HideZeroLevel(ChartControl chartControl)
        {
            XYDiagram2D diag = (XYDiagram2D)chartControl.Diagram;
            AxisY2D axis = diag.AxisY;
            axis.WholeRange = new DevExpress.Xpf.Charts.Range();
            axis.WholeRange.SetAuto();
            AxisY2D.SetAlwaysShowZeroLevel(axis.WholeRange, false);
        }

        private void SetFixedRange(ChartControl chartControl)
        {
            XYDiagram2D diag = (XYDiagram2D)chartControl.Diagram;
            AxisY2D axis = diag.AxisY;
            axis.WholeRange = new DevExpress.Xpf.Charts.Range()
            {
                MinValue = 0,
                MaxValue = 5
            };
            AxisY2D.SetAlwaysShowZeroLevel(axis.WholeRange, true);
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)btnData.IsChecked)
            {
                envParser.IsStartLogData = flyParser.IsStartLogData = true;
                btnData.Content = "停止存储数据";
            }
            else
            {
                envParser.IsStartLogData = flyParser.IsStartLogData = false;
                btnData.Content = "开始存储数据";
            }
            btnData.IsChecked = !btnData.IsChecked;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            yaoceParser = new DataParser(new WindowInteropHelper(this).Handle);
            if (load == null)
            {
                load = new LoadDataForm(); 
                load.setPlayStatus = setOffLineFilePlayStatus; 
            }
            load.Show(); 
            return; 
        }

        private void btnVideo_Click(object sender, RoutedEventArgs e)
        {
            SettingManager settingManager = new SettingManager();
            if(!settingManager.LoadVideoSetting(out VideoSetting videoSetting))
            {
                MessageBox.Show("读取视频软件配置文件失败!", "错误", MessageBoxButton.OK);
            }
            else
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(videoSetting.ExePath, videoSetting.ExeParam);
                processStartInfo.WorkingDirectory = Path.GetDirectoryName(videoSetting.ExePath);
                Process process = new Process
                {
                    StartInfo = processStartInfo
                };
                try
                {
                    process.Start();
                }
                catch(Exception exception)
                {
                    MessageBox.Show("视频软件启动失败:" + exception.Message, "错误", MessageBoxButton.OK);
                }
            }
        }

        private void clearAllChart()
        {
            xiTong_CHART_ITEM_INDEX = 0; //
        }
    }
}
