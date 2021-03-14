// 
using System; //
// 
using System.Collections.Generic; //
// 
using System.Linq; //
// 
using System.Runtime.InteropServices; //
// 
using System.Text; //
// 
using System.Threading.Tasks; //
// 

// 
namespace YaoCeProcess
// 
{
// 
    // UDP包头
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    struct UDPHead
// 
    {
// 
        public byte header1; //            // 固定0xAA
// 
        public byte header2; //            // 固定0x00
// 
        public byte header3; //            // 固定0x55
// 
        public byte header4; //            // 固定0x77
// 

// 
        public UInt16 dataLength; //         // 数据长度L
// 
        // 后面直接跟<=645个字节数据
// 
    }
// 

// 
    //----------------------------------------------------------------------------//
// 
    // CAN数据帧协议头
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    struct CANHead
// 
    {
// 
        public byte frameInfo1; //          // 
// 
        public byte frameInfo2; //          // 
// 
        // 这两个字节包括了仲裁场（数据类型ID），数据长度
// 
        // 后面直接跟数据段第1-8个字节(不足则填充)
// 
    }
// 

// 
    // 第一子帧帧结构
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    struct CANFirst
// 
    {
// 
        public byte xuHao; //              // 子帧序号 第一帧固定为0x00
// 
        public byte zhenZongChang; //      // 帧总长度
// 
        public UInt16 zhenBianHao; //        // 帧编号（0-65535循环计数）??使用待定
// 
        public byte shuJuDuanZongChangDu; //   // 数据段总长度
// 
        public byte zhenLeiXing; //        // 帧类型
// 
    }
// 

// 
    // 中间帧帧结构
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    struct CANCenter
// 
    {
// 
        public byte xuHao; //              // 子帧序号 依次递增
// 
        // 后面直接跟数据段7个字节
// 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
// 
        public byte[] data; //
// 
    }
// 

// 
    // 结束帧帧结构
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    struct CANEnd
// 
    {
// 
        public byte xuHao; //              // 子帧序号 依次递增
// 
        // 后面直接跟数据段1-5个字节数据
// 
        // 跟两个字节的CRC16校验结果
// 
    }
// 

// 
    //----------------------------------------------------------------------------//
// 

// 
    // 系统判据状态数据
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    public struct SYSTEMPARSE_STATUS
// 
    {
// 
        // NOTE 20200506 协议弄反
// 
        public int weiDu; //             // 纬度
// 
        public int jingDu; //            // 经度
// 
        public int haiBaGaoDu; //         // 海拔高度
// 

// 
        public int dongXiangSuDu; //      // 东向速度
// 
        public int beiXiangSuDu; //       // 北向速度
// 
        public int tianXiangSuDu; //      // 天向速度
// 

// 
        public float WxJiaoSuDu; //         // Wx角速度
// 
        public float WyJiaoSuDu; //         // Wy角速度
// 
        public float WzJiaoSuDu; //         // Wz角速度
// 

// 
        public float zhouXiangGuoZai; //    // 轴向过载
// 
        public UInt32 GNSSTime; //           // GNSS时间
// 

// 
        public float curFaSheXi_X; //       // 当前发射系X
// 
        public float curFaSheXi_Y; //       // 当前发射系Y
// 
        public float curFaSheXi_Z; //       // 当前发射系Z
// 

// 
        public float yuShiLuoDianSheCheng; //   // 预示落点射程
// 
        public float yuShiLuoDianZ; //      // 预示落点Z
// 
        public float feiXingZongShiJian; // // 飞行总时间
// 
        public byte canShiZhuangTai;    // 参试状态
// 
        public byte ceLueJieDuan; //       // 策略阶段(0-准备 1-起飞 2-一级 3-二级 4-结束)

        public byte jueCePanJueJieGuo1;    // 策略结果1

        public byte jueCePanJueJieGuo2;    // 策略结果2

        public byte shuRuCaiJi1;    // 输入采集1

        public byte shuRuCaiJi2;    // 输入采集2

        public byte shuRuCaiJi3;    // 输入采集3

        public byte shuRuCaiJi4;    // 输入采集4

        public float danTouJieBaoXinHao;    // 弹头解保信号

        public float qiBaoXinHao;    // 起爆状态遥测信号

        public float neiBuKongZhiDianYa;    // 内部控制电压

        public float gongLvDianDianYa;  // 功率电电压
        // 
        // public byte danTouZhuangTai; //    // 弹头状态(0-状态异常 1-产品遥测上电正常 2-初始化正常 3-一级保险解除
// 
                                        // 4-二级保险解除 5-收到保险解除信号 6-三级保险解除 7-充电 8-起爆)
// 

// 
        public byte daoHangTip1; //        // 导航状态提示1
// 
                                        // bit0 导航数据选择结果（0：数据不可用 1：数据可用）
// 
                                        // bit1 陀螺数据融合结果（0：所有数据不可用 1：数据可用）
// 
                                        // bit2 bit3 数据未更新标志（00：均无数据; // 01：1号输入无数据，2号输入有数据; //
// 
                                        //                           10：1号输入有数据，2号输入无数据; // 11：均有数据）
// 
                                        // bit4 bit5 时间间隔异常标志（00：时间间隔均正常; // 01：1号时间间隔异常，2号时间间隔正常； 10：1号时间间隔正常，2号时间间隔异常； 00：时间间隔均不正常）
// 
                                        // bit6 弹头组合无效标志（1表示无效）
// 
                                        // bit7 弹体组合无效标志（1表示无效）
// 

// 
        public byte daoHangTip2; //        // 导航状态提示2
// 
                                        // bit0 bit1 1号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit2 bit3 1号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit4 bit5 1号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit6 bit7 1号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 

// 
        public byte daoHangTip3; //        // 导航状态提示3
// 
                                        // bit0 bit1 1号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit2 bit3 1号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit4 bit5 2号数据经度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit6 bit7 2号数据纬度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 

// 
        public byte daoHangTip4; //        // 导航状态提示4
// 
                                        // bit0 bit1 2号数据高度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit2 bit3 2号数据东向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit4 bit5 2号数据北向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 
                                        // bit6 bit7 2号数据天向速度标志（00：不是野值；01：无数据；10：数据用于初始化；11：是野值）
// 

// 
        // public byte sysyemStatusTip; //    // 系统状态指示
// 
                                        // bit0 功率输出闭合（1有效）
// 
                                        // bit1 解保指令发出（1有效）
// 
                                        // bit2 自毁指令发出（1有效）
// 
                                        // bit3 复位信号（1有效）
// 
                                        // bit4 对外供电（1有效）
// 
                                        // bit5 模拟自毁指令1（1有效）
// 
                                        // bit6 模拟自毁指令2（1有效）
// 
                                        // bit7 回路检测 ?? 待定
// 

// 
        // public byte chuDianZhuangTai; //   // 触点状态指示
// 
                                        // bit0 起飞分离脱插信号（0有效）
// 
                                        // bit1 一级分离脱插信号（0有效）
// 
                                        // bit2 安控接收机预令（1有效）
// 
                                        // bit3 安控接收机动令（1有效）
// 
                                        // bit4 一级自毁工作状态A（1有效）
// 
                                        // bit5 一级自毁工作状态B（1有效）
// 
                                        // bit6 二级自毁工作状态A（1有效）
// 
                                        // bit7 二级自毁工作状态B（1有效）
// 

// 
        // public byte jueCePanJueJieGuo1; // // 策略判决结果1
// 
                                        // bit0 总飞行时间（1：有效）
// 
                                        // bit1 侧向（1：有效）
// 
                                        // bit2 Wx角速度（1：有效）
// 
                                        // bit3 Wy角速度（1：有效）
// 
                                        // bit4 Wz角速度（1：有效）
// 
                                        // bit5 后向（1：有效）
// 
                                        // bit6 坠落（1：有效）
// 
                                        // bit7 分离时间（1：有效）
// 

// 
        // public byte jueCePanJueJieGuo2; // // 策略判决结果2
// 
                                        // bit0 控制区下限（1：有效）
// 
                                        // bit1 控制区上限（1：有效）
// 

// 
        // public byte shuChuKaiGuanStatus1; // // 输出开关状态1
// 
                                          // bit0 弹头保险（1：闭合）
// 
                                          // bit1 弹头起爆（1：闭合）
// 
                                          // bit2 一级保险1（1：闭合）
// 
                                          // bit3 一级保险2（1：闭合）
// 
                                          // bit4 一级起爆1（1：闭合）
// 
                                          // bit5 一级起爆2（1：闭合）
// 
                                          // bit6 二级保险1（1：闭合）
// 
                                          // bit7 二级保险2（1：闭合）
// 

// 
        // public byte shuChuKaiGuanStatus2; // // 输出开关状态2
// 
                                          // bit0 二级起爆1（1：闭合）
// 
                                          // bit1 二级起爆2（1：闭合）
// 
                                          // bit2 bit3 参试状态（00：测试1，数据输出状态；01：测试2，低压输出状态；10：保留状态；11：正式实验）
// 
    }
// 

// 
    // 导航数据(快速)
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    public struct DAOHANGSHUJU_KuaiSu
// 
    {
// 
        // ?? 当量是什么意思
// 
        public uint daoHangXiTongShiJian; // // 导航系统时间
// 
        public int jingDu; //               // 经度（组合结果）当量：1e-7
// 
        public int weiDu; //                // 纬度（组合结果）当量：1e-7
// 
        public int haiBaGaoDu; //           // 海拔高度（组合结果）当量：1e-7
// 

// 
        public int dongXiangSuDu; //        // 东向速度（组合结果）当量：1e-7
// 
        public int beiXiangSuDu; //         // 北向速度（组合结果）当量：1e-7
// 
        public int tianXiangSuDu; //        // 天向速度（组合结果）当量：1e-7
// 

// 
        public uint GNSSTime; //             // GNSS时间 单位s,UTC秒部
// 
        public float fuYangJiao; //           // 俯仰角
// 
        public float gunZhuanJiao; //         // 滚转角
// 
        public float pianHangJiao; //         // 偏航角
// 

// 
        // 上5ms速度
// 
        public float tuoLuoShuJu_X; //        // 陀螺X数据
// 
        public float tuoLuoShuJu_Y; //        // 陀螺Y数据
// 
        public float tuoLuoShuJu_Z; //        // 陀螺Z数据
// 

// 
        // 上5ms加速度
// 
        public float jiaSuDuJiShuJu_X; //     // 加速度计X数据
// 
        public float jiaSuDuJiShuJu_Y; //     // 加速度计Y数据
// 
        public float jiaSuDuJiShuJu_Z; //     // 加速度计Z数据
// 

// 
        // 本5ms速度
// 
        //public float tuoLuoShuJu_X2; //       // 陀螺X数据2
// 
        //public float tuoLuoShuJu_Y2; //       // 陀螺Y数据2
// 
        //public float tuoLuoShuJu_Z2; //       // 陀螺Z数据2
// 

// 
        // 本5ms加速度
// 
        //public float jiaSuDuJiShuJu_X2; //    // 加速度计X数据2
// 
        //public float jiaSuDuJiShuJu_Y2; //    // 加速度计Y数据2
// 
        //public float jiaSuDuJiShuJu_Z2; //    // 加速度计Z数据2
// 

// 
        public byte zhuangTaiBiaoZhiWei; //  // 状态标志位
// 
                                          // bit0 点火标志（0：未点火 1：已点火）
// 
                                          // bit1 分离标志（0：已分离 1：未分离）
// 
                                          // bit2 bit3 00:准备阶段 01：对准阶段 10：导航阶段
// 
                                          // bit4 bit5 00:GPS无更新 01：GPS有更新 10：GPS更新过
// 
                                          // GPS组合标志 (00：上5ms惯导，本5ms惯导; // 01：上5ms惯导，本5ms组合; // 10：上5ms组合，本5ms组合; // 11：上5ms组合，本5ms惯导; //)
// 

// 
        public byte tuoLuoGuZhangBiaoZhi; // // 陀螺故障标志
// 
                                          // bit5 陀螺x故障标志（0：正常）
// 
                                          // bit6 陀螺y故障标志（0：正常）
// 
                                          // bit7 陀螺z故障标志（0：正常）
// 
    }
// 

// 
    // 导航数据(慢速)
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    public struct DAOHANGSHUJU_ManSu
// 
    {
// 
        public uint GPSTime; //              // GPS时间 单位s,UTC秒部
// 
        public byte GPSDingWeiMoShi; //      // GPS定位模式
// 
                                          // bit0 (1:采用GPS定位 0:没有采用GPS定位)
// 
                                          // bit1 (1:采用BD2定位 0:没有采用BD2定位)
// 
                                          // bit2 1：采用GLONASS定位 0：没有采用GLONASS定位
// 
                                          // bit3 0:没有DGNSS可用 1：DGNSS可用
// 
                                          // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
// 
                                          // bit6 0:GNSS修正无效 1：GNSS修正有效
// 
                                          // bit7 0:BD2修正无效 1：BD2修正有效
// 

// 
        public byte GPS_SV; //               // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 
        public byte BD2_SV; //               // BD2 SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 

// 
        public int jingDu; //               // 经度（GPS测量）当量：1e-7
// 
        public int weiDu; //                // 纬度（GPS测量）当量：1e-7
// 
        public int haiBaGaoDu; //           // 海拔高度（GPS测量）当量：1e-2
// 

// 
        public int dongXiangSuDu; //        // 东向速度（GPS测量）当量：1e-2
// 
        public int beiXiangSuDu; //         // 北向速度（GPS测量）当量：1e-2
// 
        public int tianXiangSuDu; //        // 天向速度（GPS测量）当量：1e-2
// 

// 
        public ushort PDOP; //                 // PDOP 当量0.01
// 
        public ushort HDOP; //                 // HDOP 当量0.01
// 
        public ushort VDOP; //                 // VDOP 当量0.01
// 

// 
        //----------------------------------------------------//
// 
        // 原协议类型为char
// 
        public byte tuoLuoWenDu_X; //        // X陀螺温度
// 
        public byte tuoLuoWenDu_Y; //        // Y陀螺温度
// 
        public byte tuoLuoWenDu_Z; //        // Z陀螺温度
// 

// 
        public byte jiaJiWenDu_X; //         // X加计温度
// 
        public byte jiaJiWenDu_Y; //         // Y加计温度
// 
        public byte jiaJiWenDu_Z; //         // Z加计温度
// 
        //----------------------------------------------------//
// 

// 
        public sbyte dianYaZhi_zheng5V; //    // +5V电压值     当量0.05
// 
        public sbyte dianYaZhi_fu5V; //       // -5V电压值     当量0.05
// 

// 
        public sbyte dianYaZhi_zheng15V; //   // +15V电压值    当量0.02
// 
        public sbyte dianYaZhi_fu15V; //      // -15V电压值    当量0.02
// 

// 
        public sbyte tuoLuoDianYaZhi_X_zheng5V; //    // X陀螺+5V电压值     当量0.05
// 
        public sbyte tuoLuoDianYaZhi_X_fu5V; //       // X陀螺-5V电压值     当量0.05
// 

// 
        public sbyte tuoLuoDianYaZhi_Y_zheng5V; //    // Y陀螺+5V电压值     当量0.05
// 
        public sbyte tuoLuoDianYaZhi_Y_fu5V; //       // Y陀螺-5V电压值     当量0.05
// 

// 
        public sbyte tuoLuoDianYaZhi_Z_zheng5V; //    // Z陀螺+5V电压值     当量0.05
// 
        public sbyte tuoLuoDianYaZhi_Z_fu5V; //       // Z陀螺-5V电压值     当量0.05
// 

// 
        public byte yuTuoLuoTongXingCuoWuJiShu_X; // // 与X陀螺通信错误计数（一直循环计数）
// 
        public byte yuTuoLuoTongXingCuoWuJiShu_Y; // // 与Y陀螺通信错误计数（一直循环计数）
// 
        public byte yuTuoLuoTongXingCuoWuJiShu_Z; // // 与Z陀螺通信错误计数（一直循环计数）
// 
        public byte yuGPSJieShouJiTongXingCuoWuJiShu; // // 与GPS接收机通信错误计数（一直循环计数）
// 

// 
        public byte IMUJinRuZhongDuanCiShu; // // IMU进入中断次数（每800次+1 循环计数）
// 
        public byte GPSZhongDuanCiShu; //      // GPS中断次数（每10次+1 循环计数）
// 

// 
        public byte biaoZhiWei1; //            // 标志位1
// 
                                            // bit0 导航初始值装订标志（0:未装订 1：已装订）
// 
                                            // bit1 发送1553数据标志（0：未发送 1：已发送）
// 
                                            // bit2 导航标志（0：未导航 1：已导航）
// 
                                            // bit3 对准完成标志(0:未对准 1：已对准)
// 
                                            // bit4 装订参数读取标志(0:未装订 1：已装订)
// 

// 
        public byte biaoZhiWei2; //            // 标志位2
                                 // 
                                 // bit0 bit1 工作模式（00：飞行模式 01：仿真模式1 10：仿真模式2 11：调试模式）
                                 // 
                                 // bit5 GPS组合标志（0：惯性 1：组合）
                                 // 
                                 // bit6 点火标志(0：未点火 1：已点火)
                                 // 
                                 // bit7 分离标志（0：已分离 1：未分离）


        public int jingDu_ZuHe; //               // 经度（GPS测量）当量：1e-7
                           // 
        public int weiDu_ZuHe; //                // 纬度（GPS测量）当量：1e-7
                          // 
        public int haiBaGaoDu_ZuHe; //           // 海拔高度（GPS测量）当量：1e-2
                               // 

        // 
        public int dongXiangSuDu_ZuHe; //        // 东向速度（GPS测量）当量：1e-2
                                  // 
        public int beiXiangSuDu_ZuHe; //         // 北向速度（GPS测量）当量：1e-2
                                 // 
        public int tianXiangSuDu_ZuHe; //        // 天向速度（GPS测量）当量：1e-2
                                       // 

        public float fuYangJiao; //           // 俯仰角
                                 // 
        public float gunZhuanJiao; //         // 滚转角
                                   // 
        public float pianHangJiao; //         // 偏航角
                                   // 

        // 
        // 上5ms速度
        // 
        public float tuoLuoShuJu_X; //        // 陀螺X数据
                                    // 
        public float tuoLuoShuJu_Y; //        // 陀螺Y数据
                                    // 
        public float tuoLuoShuJu_Z; //        // 陀螺Z数据
                                    // 

        // 
        // 上5ms加速度
        // 
        public float jiaSuDuJiShuJu_X; //     // 加速度计X数据
                                       // 
        public float jiaSuDuJiShuJu_Y; //     // 加速度计Y数据
                                       // 
        public float jiaSuDuJiShuJu_Z; //     // 加速度计Z数据
    }
// 

// 
    // 回路检测反馈数据
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    struct HUILUJIANCE_STATUS
// 
    {
// 
        public float shuChu1HuiLuDianZu; //    // 电机驱动输出1回路电阻
// 
        public UInt32 reserve1; //              // 保留1
// 
        public float shuChu2HuiLuDianZu; //    // 电机驱动输出2回路电阻
// 
        public UInt32 reserve2; //              // 保留2
// 
        public float QBDH1AHuiLuDianZu; //     // 起爆点火1A回路电阻
// 
        public float QBDH1BHuiLuDianZu; //     // 起爆点火1B回路电阻
// 
        public float QBDH2AHuiLuDianZu; //     // 起爆点火2A回路电阻
// 
        public float QBDH2BHuiLuDianZu; //     // 起爆点火2B回路电阻
// 
    }
// 

// 
    // 系统状态即时反馈
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    public struct SYSTEMImmediate_STATUS
// 
    {
// 
        public byte guZhangBiaoZhi; //         // 故障标志位
// 
                                            // bit0 陀螺x故障标志（0：正常；1：故障）
// 
                                            // bit1 陀螺y故障标志（0：正常；1：故障）
// 
                                            // bit2 陀螺z故障标志（0：正常；1：故障）
// 
                                            // bit3 RS422故障标志（0：正常；1：故障）
// 
                                            // bit4 1553B故障标志（0：正常；1：故障）
// 

// 
        public byte tuoLuoWenDu_X; //          // X陀螺温度
// 
        public byte tuoLuoWenDu_Y; //          // Y陀螺温度
// 
        public byte tuoLuoWenDu_Z; //          // Z陀螺温度
// 

// 
        public byte GPS_SV; //                 // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 
        public byte GPSDingWeiMoShi; //        // GPS定位模式
// 
                                            // bit0 (1:采用GPS定位 0:没有采用GPS定位)
// 
                                            // bit1 (1:采用BD2定位 0:没有采用BD2定位)
// 
                                            // bit2 1：采用GLONASS定位 0：没有采用GLONASS定位
// 
                                            // bit3 0:没有DGNSS可用 1：DGNSS可用
// 
                                            // bit4 bit5 (00:No Fix 01:2DFix 11:3D Fix)
// 
                                            // bit6 0:GNSS修正无效 1：GNSS修正有效
// 
                                            // bit7 0:BD2修正无效 1：BD2修正有效
// 

// 
        public ushort PDOP; //                 // PDOP 
// 
        public ushort HDOP; //                 // HDOP 
// 
        public ushort VDOP; //                 // VDOP 
// 

// 
        public uint GPSTime; //                // GPS时间 单位s,UTC秒，当量：0.1
// 

// 
        public Int32 jingDu; //                // 经度           当量：1e-7
// 
        public Int32 weiDu; //                 // 纬度           当量：1e-7
// 
        public Int32 haiBaGaoDu; //            // 海拔高度       当量：1e-2
// 

// 
        public Int32 dongXiangSuDu; //         // 东向速度       当量：1e-2
// 
        public Int32 beiXiangSuDu; //          // 北向速度       当量：1e-2
// 
        public Int32 tianXiangSuDu; //         // 天向速度       当量：1e-2
// 

// 
        public float zhouXiangGuoZai; //       // 轴向过载
// 
        public float faXiangGuoZai; //         // 法向过载
// 
        public float ceXiangGuoZai; //         // 侧向过载
// 

// 
        public float WxJiaoSuDu; //            // Wx角速度
// 
        public float WyJiaoSuDu; //            // Wy角速度
// 
        public float WzJiaoSuDu; //            // Wz角速度
                                 // 
        public byte BD2SV; //        // BD2SV 可用/参与定位
    }

    //弹头导航数据
    [Serializable]
    // 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    
    public struct DANTOUDAOHANGDATA
    {

        public uint GNSSTime; //GNSS时间  当量:0.001

        public int jingDu_ZuHe; //组合后经度 当量:1e-7
        public int weiDu_ZuHe; //组合后纬度 当量:1e-7
        public int gaoDu_ZuHe; //组合后高度 当量:1e-2

        public int dongXiangSuDu_ZuHe; //组合后东向速度 当量:1e-2
        public int beiXiangSuDu_ZuHe; //组合后北向速度 当量:1e-2
        public int tianXiangSuDu_ZuHe; //组合后天向速度 当量:1e-2

        public int jingDu_GNSS; //GNSS测量经度 当量:1e-7
        public int weiDu_GNSS; //GNSS测量纬度 当量:1e-7
        public int gaoDu_GNSS; //GNSS测量高度 当量:1e-2

        public int dongXiangSuDu_GNSS; //GNSS测量东向速度 当量:1e-2
        public int beiXiangSuDu_GNSS; //GNSS测量北向速度 当量:1e-2
        public int tianXiangSuDu_GNSS;//GNSS测量天向速度 当量:1e-2

        public float fuYangJiao; //俯仰角
        public float gunZhuanJiao; //滚转角
        public float pianHangJiao; // 偏航角

        public float WxJiaoSuDu; //角速度Wx
        public float WyJiaoSuDu; //角速度Wy
        public float WzJiaoSuDu; //角速度Wz

        public float xBiLi; //x比力
        public float yBiLi; //y比力
        public float zBiLi; //z比力

        public ushort HDOP; //HDOP 当量：0.01
        public ushort VDOP; //VDOP 当量: 0.01

        public byte keShiWeiXingShu; //可视卫星数
        public byte shiYongWeiXingShu; //使用卫星数
        public byte tuoLuoGuZhangBiaoShi; //陀螺故障标识
    }

    // 
    // 帧属性
    // 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    public struct FRAME_PROPERTY
// 
    {
// 
        public byte CanId; //                  // 帧ID
// 
        public byte frameType; //              // 帧类型
// 
        public UInt16 frameNo; //              // 帧序号
// 
    }
// 

// 
    // UDP包状态
// 
    [Serializable]
// 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
// 
    public struct UDP_PROPERTY
// 
    {
// 
        public bool ret; //                    // UDP包状态
// 
    }
// 
}
// 
