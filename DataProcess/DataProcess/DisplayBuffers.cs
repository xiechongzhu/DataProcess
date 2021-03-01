﻿using DataProcess.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess
{
    public class DisplayBuffers
    {
        public DisplayBuffers()
        {
            SlowPacketList = new List<SlowPacket>();
            FastPacketList = new List<FastPacket>();
            TailPacketList = new List<TailPacketRs>();
            NavDataList = new List<NavData>();
            AngelDataList = new List<AngleData>();
            ProgramControlDataList = new List<ProgramControlData>();
            ServoDataList = new List<ServoData>();
        }

        public void Clear()
        {
            SlowPacketList.Clear();
            FastPacketList.Clear();
            TailPacketList.Clear();
            NavDataList.Clear();
            AngelDataList.Clear();
            ProgramControlDataList.Clear();
            ServoDataList.Clear();
        }

        public List<SlowPacket> SlowPacketList { get; }
        public List<FastPacket> FastPacketList { get; }
        public List<TailPacketRs> TailPacketList { get; }
        public List<NavData> NavDataList { get; }
        public List<AngleData> AngelDataList { get; }
        public List<ProgramControlData> ProgramControlDataList { get; }
        public List<ServoData> ServoDataList { get; }
    }

    public class yaoceDisplayBuffers
    {
        public yaoceDisplayBuffers()
        {
            XiTong_ZuoBiao_JingDu_buffer = new List<double>();
            XiTong_ZuoBiao_WeiDu_buffer = new List<double>();
            XiTong_ZuoBiao_GaoDu_buffer = new List<double>();
            XiTong_SuDu_Bei_buffer = new List<double>();
            XiTong_SuDu_Tian_buffer = new List<double>();
            XiTong_SuDu_Dong_buffer = new List<double>();
            XiTong_FaSheXi_ZXGZ_buffer = new List<double>();
            XiTong_FaSheXi_X_buffer = new List<double>();
            XiTong_FaSheXi_Y_buffer = new List<double>();
            XiTong_FaSheXi_Z_buffer = new List<double>();
            XiTong_YuShiLuoDian_SheCheng_buffer = new List<double>();
            XiTong_YuShiLuoDian_Z_buffer = new List<double>();
            XiTong_JiaoSuDu_Wx_buffer = new List<double>();
            XiTong_JiaoSuDu_Wy_buffer = new List<double>();
            XiTong_JiaoSuDu_Wz_buffer = new List<double>();

            DHKuaiSu_Ti_SuDu_Bei_buffer = new List<double>();
            DHKuaiSu_Ti_SuDu_Dong_buffer = new List<double>();
            DHKuaiSu_Ti_SuDu_Tian_buffer = new List<double>();
            DHKuaiSu_Ti_ZuoBiao_GaoDu_buffer = new List<double>();
            DHKuaiSu_Ti_ZuoBiao_JingDu_buffer = new List<double>();
            DHKuaiSu_Ti_ZuoBiao_WeiDu_buffer = new List<double>();
            DHKuaiSu_Tou_SuDu_Bei_buffer = new List<double>();
            DHKuaiSu_Tou_SuDu_Dong_buffer = new List<double>();
            DHKuaiSu_Tou_SuDu_Tian_buffer = new List<double>();
            DHKuaiSu_Tou_ZuoBiao_GaoDu_buffer = new List<double>();
            DHKuaiSu_Tou_ZuoBiao_JingDu_buffer = new List<double>();
            DHKuaiSu_Tou_ZuoBiao_WeiDu_buffer = new List<double>();

            DHManSu_Ti_SuDu_Bei_buffer = new List<double>();
            DHManSu_Ti_SuDu_Dong_buffer = new List<double>();
            DHManSu_Ti_SuDu_Tian_buffer = new List<double>();
            DHManSu_Ti_ZuoBiao_GaoDu_buffer = new List<double>();
            DHManSu_Ti_ZuoBiao_JingDu_buffer = new List<double>();
            DHManSu_Ti_ZuoBiao_WeiDu_buffer = new List<double>();
            DHManSu_Tou_SuDu_Bei_buffer = new List<double>();
            DHManSu_Tou_SuDu_Dong_buffer = new List<double>();
            DHManSu_Tou_SuDu_Tian_buffer = new List<double>();
            DHManSu_Tou_ZuoBiao_GaoDu_buffer = new List<double>();
            DHManSu_Tou_ZuoBiao_JingDu_buffer = new List<double>();
            DHManSu_Tou_ZuoBiao_WeiDu_buffer = new List<double>();

            XTJS_Tou_ZuoBiao_WeiDu_buffer = new List<double>();
            XTJS_Tou_ZuoBiao_JingDu_buffer = new List<double>();
            XTJS_Tou_ZuoBiao_GaoDu_buffer = new List<double>();
            XTJS_Tou_SuDu_Bei_buffer = new List<double>();
            XTJS_Tou_SuDu_Dong_buffer = new List<double>();
            XTJS_Tou_SuDu_Tian_buffer = new List<double>();
            XTJS_Tou_JiaoSuDu_Wx_buffer = new List<double>();
            XTJS_Tou_JiaoSuDu_Wy_buffer = new List<double>();
            XTJS_Tou_JiaoSuDu_Wz_buffer = new List<double>();
            XTJS_Tou_GuoZai_ZhouXiang_buffer = new List<double>();
            XTJS_Tou_GuoZai_FaXiang_buffer = new List<double>();
            XTJS_Tou_GuoZai_CeXiang_buffer = new List<double>();

            XTJS_Ti_ZuoBiao_WeiDu_buffer = new List<double>();
            XTJS_Ti_ZuoBiao_JingDu_buffer = new List<double>();
            XTJS_Ti_ZuoBiao_GaoDu_buffer = new List<double>();
            XTJS_Ti_SuDu_Bei_buffer = new List<double>();
            XTJS_Ti_SuDu_Dong_buffer = new List<double>();
            XTJS_Ti_SuDu_Tian_buffer = new List<double>();
            XTJS_Ti_JiaoSuDu_Wx_buffer = new List<double>();
            XTJS_Ti_JiaoSuDu_Wy_buffer = new List<double>();
            XTJS_Ti_JiaoSuDu_Wz_buffer = new List<double>();
            XTJS_Ti_GuoZai_ZhouXiang_buffer = new List<double>();
            XTJS_Ti_GuoZai_FaXiang_buffer = new List<double>();
            XTJS_Ti_GuoZai_CeXiang_buffer = new List<double>();

            XiTongPanJu15_buffer = new List<double>();
            XiTongPanJu16_buffer = new List<double>();
            DHK_Ti_buffer = new List<double>();
            DHK_Tou_buffer = new List<double>();
            DHM_Ti_buffer = new List<double>();
            DHM_Tou_buffer = new List<double>();
            XTJS_Tou_buffer = new List<double>();
            XTJS_Ti_buffer = new List<double>();
            HuiLuJianCe_buffer = new List<double>();
        }

        public void Clear()
        {
            XTJS_Tou_JiaoSuDu_Wx_buffer.Clear();
            XTJS_Tou_JiaoSuDu_Wy_buffer.Clear();
            XTJS_Tou_JiaoSuDu_Wz_buffer.Clear();
            XTJS_Tou_ZuoBiao_JingDu_buffer.Clear();
            XTJS_Tou_ZuoBiao_WeiDu_buffer.Clear();
            XTJS_Tou_ZuoBiao_GaoDu_buffer.Clear();
            XTJS_Tou_SuDu_Dong_buffer.Clear();
            XTJS_Tou_SuDu_Bei_buffer.Clear();
            XTJS_Tou_SuDu_Tian_buffer.Clear();
            XTJS_Tou_GuoZai_ZhouXiang_buffer.Clear();
            XTJS_Tou_GuoZai_CeXiang_buffer.Clear();
            XTJS_Tou_GuoZai_FaXiang_buffer.Clear();

            XTJS_Ti_JiaoSuDu_Wx_buffer.Clear();
            XTJS_Ti_JiaoSuDu_Wy_buffer.Clear();
            XTJS_Ti_JiaoSuDu_Wz_buffer.Clear();
            XTJS_Ti_ZuoBiao_JingDu_buffer.Clear();
            XTJS_Ti_ZuoBiao_WeiDu_buffer.Clear();
            XTJS_Ti_ZuoBiao_GaoDu_buffer.Clear();
            XTJS_Ti_SuDu_Dong_buffer.Clear();
            XTJS_Ti_SuDu_Bei_buffer.Clear();
            XTJS_Ti_SuDu_Tian_buffer.Clear();
            XTJS_Ti_GuoZai_ZhouXiang_buffer.Clear();
            XTJS_Ti_GuoZai_CeXiang_buffer.Clear();
            XTJS_Ti_GuoZai_FaXiang_buffer.Clear();

            XiTong_ZuoBiao_JingDu_buffer.Clear();
            XiTong_ZuoBiao_WeiDu_buffer.Clear();
            XiTong_ZuoBiao_GaoDu_buffer.Clear();
            XiTong_SuDu_Dong_buffer.Clear();
            XiTong_SuDu_Bei_buffer.Clear();
            XiTong_SuDu_Tian_buffer.Clear();
            XiTong_JiaoSuDu_Wx_buffer.Clear();
            XiTong_JiaoSuDu_Wy_buffer.Clear();
            XiTong_JiaoSuDu_Wz_buffer.Clear();
            XiTong_FaSheXi_ZXGZ_buffer.Clear();
            XiTong_FaSheXi_X_buffer.Clear();
            XiTong_FaSheXi_Y_buffer.Clear();
            XiTong_FaSheXi_Z_buffer.Clear();
            XiTong_YuShiLuoDian_SheCheng_buffer.Clear();
            XiTong_YuShiLuoDian_Z_buffer.Clear();

            DHKuaiSu_Ti_ZuoBiao_JingDu_buffer.Clear();
            DHKuaiSu_Ti_ZuoBiao_WeiDu_buffer.Clear();
            DHKuaiSu_Ti_ZuoBiao_GaoDu_buffer.Clear();
            DHKuaiSu_Ti_SuDu_Dong_buffer.Clear();
            DHKuaiSu_Ti_SuDu_Tian_buffer.Clear();
            DHKuaiSu_Ti_SuDu_Bei_buffer.Clear();
            DHKuaiSu_Tou_ZuoBiao_JingDu_buffer.Clear();
            DHKuaiSu_Tou_ZuoBiao_WeiDu_buffer.Clear();
            DHKuaiSu_Tou_ZuoBiao_GaoDu_buffer.Clear();
            DHKuaiSu_Tou_SuDu_Dong_buffer.Clear();
            DHKuaiSu_Tou_SuDu_Tian_buffer.Clear();
            DHKuaiSu_Tou_SuDu_Bei_buffer.Clear();

            DHManSu_Ti_ZuoBiao_JingDu_buffer.Clear();
            DHManSu_Ti_ZuoBiao_WeiDu_buffer.Clear();
            DHManSu_Ti_ZuoBiao_GaoDu_buffer.Clear();
            DHManSu_Ti_SuDu_Dong_buffer.Clear();
            DHManSu_Ti_SuDu_Tian_buffer.Clear();
            DHManSu_Ti_SuDu_Bei_buffer.Clear();
            DHManSu_Tou_ZuoBiao_JingDu_buffer.Clear();
            DHManSu_Tou_ZuoBiao_WeiDu_buffer.Clear();
            DHManSu_Tou_ZuoBiao_GaoDu_buffer.Clear();
            DHManSu_Tou_SuDu_Dong_buffer.Clear();
            DHManSu_Tou_SuDu_Tian_buffer.Clear();
            DHManSu_Tou_SuDu_Bei_buffer.Clear();

            XiTongPanJu15_buffer.Clear();
            XiTongPanJu16_buffer.Clear();
            DHK_Ti_buffer.Clear();
            DHK_Tou_buffer.Clear();
            DHM_Tou_buffer.Clear();
            DHM_Ti_buffer.Clear();
            XTJS_Ti_buffer.Clear();
            XTJS_Tou_buffer.Clear();
            HuiLuJianCe_buffer.Clear();
        }

        //系统判决
        public List<double> XiTong_ZuoBiao_JingDu_buffer { get; }
        public List<double> XiTong_ZuoBiao_WeiDu_buffer { get; }
        public List<double> XiTong_ZuoBiao_GaoDu_buffer { get; }

        public List<double> XiTong_SuDu_Dong_buffer { get; }
        public List<double> XiTong_SuDu_Bei_buffer { get; }
        public List<double> XiTong_SuDu_Tian_buffer { get; }

        public List<double> XiTong_JiaoSuDu_Wx_buffer { get; }
        public List<double> XiTong_JiaoSuDu_Wy_buffer { get; }
        public List<double> XiTong_JiaoSuDu_Wz_buffer { get; }

        public List<double> XiTong_FaSheXi_ZXGZ_buffer { get; }
        public List<double> XiTong_FaSheXi_X_buffer { get; }
        public List<double> XiTong_FaSheXi_Y_buffer { get; }
        public List<double> XiTong_FaSheXi_Z_buffer { get; }

        public List<double> XiTong_YuShiLuoDian_SheCheng_buffer { get; }
        public List<double> XiTong_YuShiLuoDian_Z_buffer { get; }

        //导航快
        public List<double> DHKuaiSu_Ti_ZuoBiao_JingDu_buffer { get; }
        public List<double> DHKuaiSu_Ti_ZuoBiao_WeiDu_buffer { get; }
        public List<double> DHKuaiSu_Ti_ZuoBiao_GaoDu_buffer { get; }

        public List<double> DHKuaiSu_Ti_SuDu_Dong_buffer { get; }
        public List<double> DHKuaiSu_Ti_SuDu_Bei_buffer { get; }
        public List<double> DHKuaiSu_Ti_SuDu_Tian_buffer { get; }

        public List<double> DHKuaiSu_Tou_ZuoBiao_JingDu_buffer { get; }
        public List<double> DHKuaiSu_Tou_ZuoBiao_WeiDu_buffer { get; }
        public List<double> DHKuaiSu_Tou_ZuoBiao_GaoDu_buffer { get; }

        public List<double> DHKuaiSu_Tou_SuDu_Dong_buffer { get; }
        public List<double> DHKuaiSu_Tou_SuDu_Bei_buffer { get; }
        public List<double> DHKuaiSu_Tou_SuDu_Tian_buffer { get; }

        //导航慢
        public List<double> DHManSu_Ti_ZuoBiao_JingDu_buffer { get; }
        public List<double> DHManSu_Ti_ZuoBiao_WeiDu_buffer { get; }
        public List<double> DHManSu_Ti_ZuoBiao_GaoDu_buffer { get; }

        public List<double> DHManSu_Ti_SuDu_Dong_buffer { get; }
        public List<double> DHManSu_Ti_SuDu_Bei_buffer { get; }
        public List<double> DHManSu_Ti_SuDu_Tian_buffer { get; }

        public List<double> DHManSu_Tou_ZuoBiao_JingDu_buffer { get; }
        public List<double> DHManSu_Tou_ZuoBiao_WeiDu_buffer { get; }
        public List<double> DHManSu_Tou_ZuoBiao_GaoDu_buffer { get; }

        public List<double> DHManSu_Tou_SuDu_Dong_buffer { get; }
        public List<double> DHManSu_Tou_SuDu_Bei_buffer { get; }
        public List<double> DHManSu_Tou_SuDu_Tian_buffer { get; }

        //系统即时
        public List<double> XTJS_Tou_ZuoBiao_JingDu_buffer { get; }
        public List<double> XTJS_Tou_ZuoBiao_WeiDu_buffer { get; }
        public List<double> XTJS_Tou_ZuoBiao_GaoDu_buffer { get; }

        public List<double> XTJS_Tou_SuDu_Dong_buffer { get; }
        public List<double> XTJS_Tou_SuDu_Bei_buffer { get; }
        public List<double> XTJS_Tou_SuDu_Tian_buffer { get; }

        public List<double> XTJS_Tou_JiaoSuDu_Wx_buffer { get; }
        public List<double> XTJS_Tou_JiaoSuDu_Wy_buffer { get; }
        public List<double> XTJS_Tou_JiaoSuDu_Wz_buffer { get; }

        public List<double> XTJS_Tou_GuoZai_ZhouXiang_buffer { get; }
        public List<double> XTJS_Tou_GuoZai_FaXiang_buffer { get; }
        public List<double> XTJS_Tou_GuoZai_CeXiang_buffer { get; }

        public List<double> XTJS_Ti_ZuoBiao_JingDu_buffer { get; }
        public List<double> XTJS_Ti_ZuoBiao_WeiDu_buffer { get; }
        public List<double> XTJS_Ti_ZuoBiao_GaoDu_buffer { get; }

        public List<double> XTJS_Ti_SuDu_Dong_buffer { get; }
        public List<double> XTJS_Ti_SuDu_Bei_buffer { get; }
        public List<double> XTJS_Ti_SuDu_Tian_buffer { get; }

        public List<double> XTJS_Ti_JiaoSuDu_Wx_buffer { get; }
        public List<double> XTJS_Ti_JiaoSuDu_Wy_buffer { get; }
        public List<double> XTJS_Ti_JiaoSuDu_Wz_buffer { get; }

        public List<double> XTJS_Ti_GuoZai_ZhouXiang_buffer { get; }
        public List<double> XTJS_Ti_GuoZai_FaXiang_buffer { get; }
        public List<double> XTJS_Ti_GuoZai_CeXiang_buffer { get; }

        //帧序号
        public List<double> XiTongPanJu15_buffer { get; }
        public List<double> XiTongPanJu16_buffer { get; }
        public List<double> HuiLuJianCe_buffer { get; }
        public List<double> DHK_Ti_buffer { get; }
        public List<double> DHK_Tou_buffer { get; }
        public List<double> DHM_Ti_buffer { get; }
        public List<double> DHM_Tou_buffer { get; }
        public List<double> XTJS_Ti_buffer { get; }
        public List<double> XTJS_Tou_buffer { get; }
    }
}
