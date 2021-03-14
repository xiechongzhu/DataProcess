using System;
using DevExpress.Xpo;

namespace DataProcess
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

        public ChartPointDataSource chart_XiTong_FaSheXi_ZXGZList = new ChartPointDataSource(200);
        public ChartPointDataSource chart_XiTong_FaSheXi_XList = new ChartPointDataSource(200);
        public ChartPointDataSource chart_XiTong_FaSheXi_YList = new ChartPointDataSource(200);
        public ChartPointDataSource chart_XiTong_FaSheXi_ZList = new ChartPointDataSource(200);

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

        //新增
        public ChartPointDataSource chart_DHKuaiSu_Ti_JiaoDu_FuYangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_JiaoDu_GunZhuanJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_JiaoDu_PianHangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHKuaiSu_Ti_TuoLuo_TuoLuoXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_TuoLuo_TuoLuoYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_TuoLuo_TuoLuoZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHKuaiSu_Ti_JiaSuDu_JiaJiXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_JiaSuDu_JiaJiYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Ti_JiaSuDu_JiaJiZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHKuaiSu_Tou_JiaoDu_FuYangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_JiaoDu_GunZhuanJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_JiaoDu_PianHangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHKuaiSu_Tou_TuoLuo_TuoLuoXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_TuoLuo_TuoLuoYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_TuoLuo_TuoLuoZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHKuaiSu_Tou_JiaSuDu_JiaJiXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_JiaSuDu_JiaJiYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHKuaiSu_Tou_JiaSuDu_JiaJiZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

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

        //新增
        public ChartPointDataSource chart_DHManSu_Ti_ZuoBiao_JingDuZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_ZuoBiao_WeiDuZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_ZuoBiao_GaoDuZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Ti_SuDu_DongZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_SuDu_BeiZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_SuDu_TianZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);


        public ChartPointDataSource chart_DHManSu_Tou_ZuoBiao_JingDuZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_ZuoBiao_WeiDuZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_ZuoBiao_GaoDuZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Tou_SuDu_DongZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_SuDu_BeiZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_SuDu_TianZuHeList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Ti_JiaoDu_FuYangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_JiaoDu_GunZhuanJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_JiaoDu_PianHangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Ti_TuoLuo_TuoLuoXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_TuoLuo_TuoLuoYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_TuoLuo_TuoLuoZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Ti_JiaSuDu_JiaJiXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_JiaSuDu_JiaJiYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Ti_JiaSuDu_JiaJiZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Tou_JiaoDu_FuYangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_JiaoDu_GunZhuanJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_JiaoDu_PianHangJiaoList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Tou_TuoLuo_TuoLuoXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_TuoLuo_TuoLuoYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_TuoLuo_TuoLuoZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DHManSu_Tou_JiaSuDu_JiaJiXList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_JiaSuDu_JiaJiYList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DHManSu_Tou_JiaSuDu_JiaJiZList = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);



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

        //弹头数据
        public ChartPointDataSource chart_DanTou_ZuHeJingDu = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_ZuHeWeiDu = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_ZuHeGaoDu = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_ZuHeDong = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_ZuHeBei = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_ZuHeTian = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DanTou_GNSSJingDu = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_GNSSWeiDu = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_GNSSGaoDu = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_GNSSDong = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_GNSSBei = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_GNSSTian = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DanTou_FuYangJiao = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_GunZhuanJiao = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_PianHangJiao = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DanTou_Wx = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_Wy = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_Wz = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);

        public ChartPointDataSource chart_DanTou_XBiLi = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_YBiLi = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);
        public ChartPointDataSource chart_DanTou_ZBiLi = new ChartPointDataSource(MainWindow.CHART_MAX_POINTS);



        //帧序号
        public ChartPointDataSource chart_XiTongPanJu15List = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_XiTongPanJu16List = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_HuiLuJianCeList = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_DHK_TiList = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_DHM_TiList = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_XTJS_TiList = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_DHK_TouList = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_DHM_TouList = new ChartPointDataSource(MainWindow.frame_MaxCount);
        public ChartPointDataSource chart_XTJS_TouList = new ChartPointDataSource(MainWindow.frame_MaxCount); 
        public ChartPointDataSource chart_DanTouList = new ChartPointDataSource(MainWindow.frame_MaxCount);
    }
}