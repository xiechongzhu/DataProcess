using DataProcess.Controls;
using DataProcess.Protocol;
using System;
using System.Collections.Concurrent;
using System.IO; 
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;

namespace YaoCeProcess
{
    /// 文件名:DataParser/
    /// 文件功能描述:数据分析/
    /// 创建人:yangy
    /// 版权所有:Copyright (C) ZGM/
    /// 创建标识:2020.03.12/
    /// 修改描述:
    /// 
    public class DataParser
    {
// 
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        /// PostMessage
        private static extern int PostMessage(IntPtr hwnd, int Msg, int wParam, IntPtr lParam);

        private Priority ParserPriority;

        private DispatcherTimer idleTimer = new DispatcherTimer();

        public int IdleTimeout { get; set; }

        public Action<Priority, bool> IdleHandler;

        private DateTime LastRecvDataTime;

        public bool PostMessageEnable = true;


        /// DataParser
        public DataParser(IntPtr mainFormHandle, Priority priority)
        {
            this.mainFormHandle = mainFormHandle;
            ParserPriority = priority;
            idleTimer.Tick += IdleTimer_Tick;
        }

        private void IdleTimer_Tick(object sender, EventArgs e)
        {
            if(DateTime.Now - LastRecvDataTime > new TimeSpan(0, 0, 0, 0, IdleTimeout))
            {
                IdleHandler?.Invoke(ParserPriority, false);
            }
        }

        //------------------------------------------------------------------------------------//

        /// mainFormHandle
        private IntPtr mainFormHandle; 


        /// queue
        private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>(); 

        /// isRuning
        private bool isRuning = false;
 

        /// thread
        Thread thread;


        /// 每一个UDP帧固定长度651
        private const int UDPLENGTH = 651; 

        /// 每一个状态长帧结尾的校验
        private const int CRCLENGTH = 2; 

        //------------------------------------------------------------------------------------//
        //---------------缓存的CAN长帧数据---------------//

        /// 系统判据状态
        const byte frameType_systemStatus_1 = 0x15; //帧类别：0x01

        /// 系统判据状态
        const byte frameType_systemStatus_2 = 0x16; //帧类别：0x05


        /// 导航快速（弹体）
        const byte frameType_daoHangKuaiSu = 0x25; //帧类别：0x01

        //20210313新增 弹头导航数据
        const byte frameType_danTouDaoHang = 0x35; //帧类别：0x03

        /// 导航慢速（弹体）
        const byte frameType_daoHangManSu = 0x21; //帧类别:0x02

        /// 系统状态即时反馈（弹体）
        const byte frameType_XiTongJiShi = 0x26; //帧总长11  数据段总长度64 帧类型：0x0B

        // 系统判决状态15
        /// bRecvHeader_XiTong15
        bool bRecvHeader_XiTong15 = false;

        /// 状态数据 
        byte[] statusBuffer_XiTong15 = null;

        /// 帧总长度
        byte totalCountCan_XiTong15 = 0;


        /// 数据段总长度
        byte frameLength_XiTong15 = 0;

        /// 帧编号
        UInt16 frameNO_XiTong15 = 0;

        // 系统判决状态16
        /// bRecvHeader_XiTong16
        bool bRecvHeader_XiTong16 = false; 

        /// 状态数据
        byte[] statusBuffer_XiTong16 = null;

        /// 帧总长度
        byte totalCountCan_XiTong16 = 0; 

        /// 数据段总长度
        byte frameLength_XiTong16 = 0;

        /// 帧编号
        UInt16 frameNO_XiTong16 = 0; 

        // 回路检测反馈数据16
        /// bRecvHeader_HuiLuJianCe16
        bool bRecvHeader_HuiLuJianCe16 = false; 

        /// 状态数据
        byte[] statusBuffer_HuiLuJianCe16 = null; 

        /// 帧总长度
        byte totalCountCan_HuiLuJianCe16 = 0; 

        /// 数据段总长度
        byte frameLength_HuiLuJianCe16 = 0;

        /// 帧编号
        UInt16 frameNO_HuiLuJianCe16 = 0; 

        // 当前帧类型（针对Id为0x16时，需要用到帧类型来区分）
        // 系统判决状态 0x15->0x01
        // 系统判决状态查询反馈 0x16->0x05 
        // 回路检测反馈数据 0x16->0x06

        /// curFrameType
        byte curFrameType = 0;

        /// frameType_XTPJZT
        const byte frameType_XTPJZT = 0x01; 


        /// frameType_XTPJFK
        const byte frameType_XTPJFK = 0x05;

        // 导航快速 弹体
        /// bRecvHeader_DHK21
        bool bRecvHeader_DHK21 = false; 

        /// 状态数据
        byte[] statusBuffer_DHK21 = null;//0x25 协议更改

        /// 帧总长度
        byte totalCountCan_DHK21 = 0; 

        /// 数据段总长度
        byte frameLength_DHK21 = 0; 

        /// 帧编号
        UInt16 frameNO_DHK21 = 0; //  

        // 导航快速 弹头
        /// bRecvHeader_DHK31(弃用)
        bool bRecvHeader_DHK31 = false; 

        /// 状态数据
        byte[] statusBuffer_DHK31 = null; 

    /// 帧总长度
        byte totalCountCan_DHK31 = 0;


        /// 数据段总长度
        byte frameLength_DHK31 = 0;   

        /// 帧编号
        UInt16 frameNO_DHK31 = 0; 

        // 导航慢速 弹体
        /// bRecvHeader_DHM25
        bool bRecvHeader_DHM25 = false;//0x21 协议更改

        /// 状态数据
        byte[] statusBuffer_DHM25 = null; 

        /// 帧总长度
        byte totalCountCan_DHM25 = 0; 

        /// 数据段总长度
        byte frameLength_DHM25 = 0;

        /// 帧编号
        UInt16 frameNO_DHM25 = 0; 

        // 导航慢速 弹头
        /// bRecvHeader_DHM35
        bool bRecvHeader_DHM35 = false; //弹头导航数据 协议更改

        /// 状态数据
        byte[] statusBuffer_DHM35 = null; 

        /// 帧总长度
        byte totalCountCan_DHM35 = 0;

        /// 数据段总长度
        byte frameLength_DHM35 = 0;

        /// 帧编号
        UInt16 frameNO_DHM35 = 0;

        // TODO 20200219 新增
        // 系统即时状态反馈 弹体
        /// bRecvHeader_XiTongJiShi26
        bool bRecvHeader_XiTongJiShi26 = false; 

        /// 状态数据
        byte[] statusBuffer_XiTongJiShi26 = null; 

        /// 帧总长度
        byte totalCountCan_XiTongJiShi26 = 0;

        /// 数据段总长度
        byte frameLength_XiTongJiShi26 = 0; 

        /// 帧编号
        UInt16 frameNO_XiTongJiShi26 = 0; 

        // 系统即时状态反馈 弹头(弹头弃用)
        /// bRecvHeader_XiTongJiShi36
        bool bRecvHeader_XiTongJiShi36 = false; 

        /// 状态数据
        byte[] statusBuffer_XiTongJiShi36 = null; 

        /// 帧总长度
        byte totalCountCan_XiTongJiShi36 = 0;

        /// 数据段总长度
        byte frameLength_XiTongJiShi36 = 0;

        /// 帧编号
        UInt16 frameNO_XiTongJiShi36 = 0; 

        //------------------------------------------------------------------------------------//

        /// UDP 缓存buffer
        byte[] UDPBuffer = null;
        //------------------------------------------------------------------------------------//

        /// Enqueue
        /// <param name="data"></param>
        public void Enqueue(byte[] data)
        {
            queue.Enqueue(data); 
        }


        /// Start
        public void Start()
        {
            while (queue.TryDequeue(out byte[] dropBuffer)) ; 
            isRuning = true;
            thread = new Thread(new ThreadStart(ThreadFunction)); 
            thread.Start();

            UDPBuffer = null; 
            UDPBuffer = new byte[0];

            LastRecvDataTime = DateTime.Now;
            IdleHandler?.Invoke(ParserPriority, true);
            idleTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            idleTimer.Start();
        }


        /// Stop
        public void Stop()
        {
            isRuning = false; 
            thread?.Join(); 

            UDPBuffer = null; 
            UDPBuffer = new byte[0];
            idleTimer.Stop();
        }
 
        /// ThreadFunction
        private void ThreadFunction()
        {
            while (isRuning)
            { 
                byte[] dataBuffer;
                if (queue.TryDequeue(out dataBuffer))
                {
                    ParseDatas(dataBuffer);
                }
                else
                {
                    Thread.Sleep(5); 
                }
            }
        }
        /// 报告指定的 System.Byte[] 在此实例中的第一个匹配项的索引。
        /// <param name="srcBytes">被执行查找的 System.Byte[]。</param>
        /// <param name="searchBytes">要查找的 System.Byte[]。</param>
        /// <returns>如果找到该字节数组，则为 searchBytes 的索引位置；如果未找到该字节数组，则为 -1。如果 searchBytes 为 null 或者长度为0，则返回值为 -1。</returns>
        internal int IndexOf(byte[] srcBytes, byte[] searchBytes)
        {
            if (srcBytes == null) { return -1;   }
// 
            if (searchBytes == null) { return -1;  }
// 
            if (srcBytes.Length == 0) { return -1;   }
// 
            if (searchBytes.Length == 0) { return -1;   }
// 
            if (srcBytes.Length < searchBytes.Length) { return -1;   }
// 
            for (int i = 0;   i < srcBytes.Length - searchBytes.Length;   i++)
// 
            {
// 
                if (srcBytes[i] == searchBytes[0])
// 
                {
// 
                    if (searchBytes.Length == 1) { return i;   }
// 
                    bool flag = true; //
// 
                    for (int j = 1;   j < searchBytes.Length;   j++)
// 
                    {
// 
                        if (srcBytes[i + j] != searchBytes[j])
// 
                        {
// 
                            flag = false; //
// 
                            break; //
// 
                        }
// 
                    }
// 
                    if (flag) { return i;   }
// 
                }
// 
            }
// 
            return -1; //
// 
        }
// 

        /// ParseDatas
        /// <param name="buffer"></param>
        private void ParseDatas(byte[] buffer)
// 
        {
// 
            if (UDPBuffer == null) 
// 
            {
// 
                UDPBuffer = new byte[0]; //
// 
            }
// 

// 
            // 帧头标识
// 
            byte[] searchBytes = new byte[4]; //
// 
            searchBytes[0] = 0xAA; //
// 
            searchBytes[1] = 0x00; //
// 
            searchBytes[2] = 0x55; //
// 
            searchBytes[3] = 0x77; //
// 

// 
            //--------------------------------------------------------------------------------//
// 

// 
            // 拼接上一次剩余的包
// 
            UDPBuffer = UDPBuffer.Concat(buffer).ToArray(); //
// 

// 
            //--------------------------------------------------------------------------------//
// 

// 
            while(true)
// 
            {
// 
                int findPos = IndexOf(UDPBuffer, searchBytes); //
// 
                if (findPos == -1)
// 
                {
// 
                    break; //
// 
                }
// 
                // 判断数据长度是否足够
// 
                if (UDPBuffer.Length < findPos + UDPLENGTH)
// 
                {
// 
                    break; //
// 
                }
// 
                byte[] findBuffer = UDPBuffer.Skip(findPos).Take(UDPLENGTH).ToArray(); //
// 
                // 二次判断，判断数据帧中是否存在标识，若存在则直接定义为当前帧不完整，第二帧数据补充进来了
// 
                byte[] findDataBuffer = findBuffer.Skip(searchBytes.Length).Take(UDPLENGTH - searchBytes.Length).ToArray(); //
// 
                int findPos2 = IndexOf(findDataBuffer, searchBytes); //
// 
                if (findPos2 != -1)
// 
                {
// 
                    int skipLength = findPos + searchBytes.Length + findPos2; //
// 
                    UDPBuffer = UDPBuffer.Skip(skipLength).Take(UDPBuffer.Length - skipLength).ToArray(); //
// 
                    continue; //
// 
                }
// 

// 
                // 进行数据解析
// 
                handleData(findBuffer); //
// 

// 
                // 正常的数据偏移
// 
                int skipLengthNormal = findPos + UDPLENGTH; //
// 
                UDPBuffer = UDPBuffer.Skip(skipLengthNormal).Take(UDPBuffer.Length - skipLengthNormal).ToArray(); //
// 
            }
// 
        }
// 


        /// handleData
        /// <param name="buffer"></param>
        private void handleData(byte[] buffer)
        {
            //--------------------------------------------------------------------------------//
            if (buffer.Length < UDPLENGTH) 
            {
                return; 
            }
            //--------------------------------------------------------------------------------//
// 
            // TODO 针对粘包的情况进行处理（几个UDP包粘在了一起）
// 
            int alreadRead = 0; //
// 
            while (true)
// 
            {
// 
                if (buffer.Length - alreadRead >= UDPLENGTH)
// 
                {
// 
                    byte[] subBuffer = buffer.Skip(alreadRead).Take(UDPLENGTH).ToArray(); //
// 
                    alreadRead += subBuffer.Length; //
// 
                    ParseData(subBuffer); //
// 
                }
// 
                else
// 
                {
// 
                    break; //
// 
                }
// 
            }
// 
            //--------------------------------------------------------------------------------//
// 
        }

        /// ParseData
        /// <param name="buffer"></param>
        private void ParseData(byte[] buffer)
        {
            //--------------------------------------------------------------------------------//
            String errMsg;
            UInt16 dataLength;
            if (!CheckPacket(buffer, out errMsg, out dataLength))
            {
                // TODO 20200218 错误数据太多，影响界面刷新，卡顿
                // Logger.GetInstance().Log(Logger.LOG_LEVEL.LOG_ERROR, "数据包错误:" + errMsg); 
                return; 
            }

            // 如果dataLength长度等于0，直接不进行下面数据的处理
            if (dataLength == 0)
                return; 

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                // 位置偏移到CAN数据域
                stream.Seek(Marshal.SizeOf(typeof(UDPHead)), 0);

                // 解析CAN数据帧
                using (BinaryReader br = new BinaryReader(stream))
                {
                    UInt16 dataReadPos = 0; 
                    while (stream.Position < stream.Length - 1 && dataReadPos < dataLength)
                    {
                        // 解析CAN帧头
                        CANHead packHead = new CANHead
                        {
                            frameInfo1 = br.ReadByte(),
                            frameInfo2 = br.ReadByte()
                        };

                        // TODO 这里的CAN帧头是大端字节序
                        UInt16 frameInfo = (UInt16)(((UInt16)packHead.frameInfo1 << 8) + packHead.frameInfo2); 
                        // 3bit占位 8bit 帧id(仲裁场) 1bitRTR(0) 4bit数据场(数据长度)
                        byte canDataId = (byte)(frameInfo >> 5 & 0xFF);

                        // 偏移
                        dataReadPos += (UInt16)Marshal.SizeOf(typeof(CANHead));

                        // 当前CAN帧的数据长度
                        int canLen = (int)frameInfo & 0xF;
                        // 一帧CAN数据最多只有八个字节，往后的数据不进行处理，直接丢弃
                        if (canLen > 8)
                            return;
                        // 读取can数据
                        // TODO 这里剩下的数据长度不包括校验的两个字节，如果想用校验，需要再读取两个字节的校验值
// 
                        // TODO 20200217 更改为直接读取8个字节
// 
                        byte[] canData = br.ReadBytes(/*canLen*/8); //
// 
                        // 偏移
// 
                        // dataReadPos += (UInt16)canLen; //
// 
                        // 这里直接默认偏移8个字节，数据不足会进行填充
// 
                        dataReadPos += 8; //
// 
                        // if (8 - canLen > 0)
// 
                        // {
// 
                        //     byte[] unUseData = br.ReadBytes(8 - canLen); //
// 
                        // }
// 

// 
                        //---------------------------------------------------------------------------------//
// 
                        // 只进行如下状态数据
// 
                        switch (canDataId)
// 
                        {
// 
                            case frameType_systemStatus_1:
// 
                            case frameType_systemStatus_2:
// 
                            case frameType_daoHangKuaiSu:
// 
                            case frameType_daoHangManSu:
// 
                            case frameType_danTouDaoHang:
// 
                            // TODO 20200219 新增系统即时反馈状态
// 
                            case frameType_XiTongJiShi:
// 
                                // 将数据放入CAN数据处理模块，进行长帧的拼包工作
// 
                                ParseCANData(canData, canDataId); //
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
// 
                }
// 
            }
// 
        }
// 


        /// CheckPacket
        /// <param name="buffer"></param>
        /// <param name="errMsg"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        private bool CheckPacket(byte[] buffer, out String errMsg, out UInt16 dataLength)
        {
            // 默认初始化 
            dataLength = 0;  
            int length = buffer.Length; 
            if (length > UDPLENGTH)
            {
                errMsg = "数据不是合法数据，大于了标准帧长";
                return false; 
            }

            if (length < Marshal.SizeOf(typeof(UDPHead)))
            {
                errMsg = "数据长度小于UDP报文头"; 
                return false; 
            }

            // 判断帧头信息
            if (!(buffer[0] == 0xAA && buffer[1] == 0x00 && buffer[2] == 0x55 && buffer[3] == 0x77))
            {
                errMsg = "数据帧头标识错误";
                return false;
            }

            //-------------------------------------------------------------------------------------// 
            // TODO 20200316 添加亮灯提示，表示收到数据(长度以及帧头正确) 
            UDP_PROPERTY udpPocket = new UDP_PROPERTY
            {
                ret = true
            };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UDP_PROPERTY))); //
            Marshal.StructureToPtr(udpPocket, ptr, true); //
            if (PostMessageEnable)
            {
                PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_UDPPROPERTY_DATA, 0, ptr); //
            }
            //-------------------------------------------------------------------------------------//
// 

// 
            // 20200212更改
// 
            // 数据长度判断（大端）
// 
            dataLength = (UInt16)(((UInt16)buffer[4] << 8) + buffer[5]); //
// 
            if ((length - Marshal.SizeOf(typeof(UDPHead))) < dataLength)
// 
            {
// 
                errMsg = "数据包不完整"; //
// 
                return false; //
// 
            }
// 

// 
            errMsg = String.Empty; //
// 
            return true; //
// 
        }


        /// HandleCanDataPinJie
        /// <param name="canDataId"></param>
        /// <param name="statusBuffer"></param>
        /// <param name="totalCountCan"></param>
        /// <param name="frameLength"></param>
        /// <param name="bRecvHeader"></param>
        /// <param name="frameNO"></param> 
        /// <param name="buffer"></param>
        /// <param name="frameType"></param>
        private void HandleCanDataPinJie(
            byte canDataId,
            ref byte[] statusBuffer,
            ref byte totalCountCan,
            ref byte frameLength,
            ref bool bRecvHeader,
            ref UInt16 frameNO,
            byte[] buffer,
            byte frameType = 0x00)
        {
            // 子帧序号
            byte xuHao = buffer[0]; 

            // 数据第一帧
            if (xuHao == 0x00)
            {
                // 设置空 回到原始状态
// 
                statusBuffer = null; //
// 
                totalCountCan = buffer[1]; //  // 帧总长度
// 
                frameLength = buffer[4]; //    // 数据段总长度
// 
                statusBuffer = new byte[0]; //
// 
                bRecvHeader = true; //
// 
                frameNO = (UInt16)(((UInt16)buffer[3] << 8) + buffer[2]); //
                frameNO %= 256;
// 
            }
// 
            else
// 
            {
// 
                if (!bRecvHeader)
// 
                {
// 
                    return; //
// 
                }
// 
                // 中间帧
// 
                if (xuHao != totalCountCan - 1)
// 
                {
// 
                    // 拼接上一次剩余的包(中间帧包括：1字节序号，7字节数据)
// 
                    byte[] canData = new byte[7]; //
// 
                    Array.Copy(buffer, 1, canData, 0, 7); //   // 从buffer的第1个位置开始拷贝7个字节到canData中
// 
                    statusBuffer = statusBuffer.Concat(canData).ToArray(); //
// 
                }
// 
                // 结束帧
// 
                else
// 
                {
// 
                    // 整个长帧剩余长度
// 
                    int lastDataLen = frameLength - statusBuffer.Length; //
// 
                    // CAN结束帧数据域最多不超过7个字节（包括了2Byte的校验，但不包括一个字节的帧序号）
// 
                    if (lastDataLen > 7 || lastDataLen < 0)
// 
                    {
// 
                        bRecvHeader = false; //
// 
                        return; //
// 
                    }
// 

// 
                    // 拼接上一次剩余的包
// 
                    // TODO +2添加两个字节的校验
// 
                    byte[] canData = new byte[lastDataLen + 2]; //
// 
                    Array.Copy(buffer, 1, canData, 0, Math.Min(buffer.Length - 1, lastDataLen + 2)); //
// 
                    statusBuffer = statusBuffer.Concat(canData).ToArray(); //
// 

// 
                    //---------------------------------------------------//
// 
                    // 拼接完成，分类型进行数据的处理
// 
                    ParseStatusData(statusBuffer, canDataId, frameType, frameNO); //
// 
                    bRecvHeader = false; //
// 
                    //---------------------------------------------------//
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
        /// ParseCANData
// 
        
// 
        /// <param name="buffer"></param>
// 
        /// <param name="canDataId"></param>
// 
        private void ParseCANData(byte[] buffer, byte canDataId)
// 
        {
// 
            // can数据长度（至少大于等于1才能取出数据中的第一个字节：帧序号）
// 
            if (buffer.Length < 1) return; //
                                           // 

            // 
            //---------------------------------------------------------------//
            // 
            LastRecvDataTime = DateTime.Now;
            IdleHandler?.Invoke(ParserPriority, true);
            // 
            switch (canDataId)
// 
            {
// 
                // 系统判据状态
                case frameType_systemStatus_1:
// 
                    HandleCanDataPinJie(canDataId, ref statusBuffer_XiTong15,
// 
                        ref totalCountCan_XiTong15, ref frameLength_XiTong15, ref bRecvHeader_XiTong15,
// 
                        ref frameNO_XiTong15, buffer); //
// 
                    break; //
// 
                // 系统判据状态 0x16(中间存在两种情况，需要通过帧类型来做进一步的区分)
// 
                case frameType_systemStatus_2:
// 
                    {
// 
                        // 第一子帧，才包含帧类型等信息
// 
                        if (buffer[0] == 0x00 && buffer.Length >= 6)
// 
                        {
// 
                            curFrameType = buffer[5]; //
// 
                        }
// 
                        if (curFrameType == frameType_XTPJFK)
// 
                        {
// 
                            HandleCanDataPinJie(canDataId, ref statusBuffer_XiTong16,
// 
                        ref totalCountCan_XiTong16, ref frameLength_XiTong16, ref bRecvHeader_XiTong16,
// 
                        ref frameNO_XiTong16, buffer, curFrameType); //
// 
                        }
// 
//                        else if (curFrameType == frameType_HLJCFK)
//// 
//                        {
//// 
//                            HandleCanDataPinJie(canDataId, ref statusBuffer_HuiLuJianCe16,
//// 
//                        ref totalCountCan_HuiLuJianCe16, ref frameLength_HuiLuJianCe16, ref bRecvHeader_HuiLuJianCe16,
//// 
//                        ref frameNO_HuiLuJianCe16, buffer, curFrameType); //
//// 
//                        }
// 
                    }
// 
                    break; //
// 
                case frameType_daoHangKuaiSu:
// 
                    HandleCanDataPinJie(canDataId, ref statusBuffer_DHK21,
// 
                        ref totalCountCan_DHK21, ref frameLength_DHK21, ref bRecvHeader_DHK21,
// 
                        ref frameNO_DHK21, buffer); //
// 
                    break; //

                case frameType_daoHangManSu:
// 
                    HandleCanDataPinJie(canDataId, ref statusBuffer_DHM25,
// 
                        ref totalCountCan_DHM25, ref frameLength_DHM25, ref bRecvHeader_DHM25,
// 
                        ref frameNO_DHM25, buffer); //
// 
                    break; //
// 
                case frameType_danTouDaoHang:
// 
                    HandleCanDataPinJie(canDataId, ref statusBuffer_DHM35,
// 
                        ref totalCountCan_DHM35, ref frameLength_DHM35, ref bRecvHeader_DHM35,
// 
                        ref frameNO_DHM35, buffer); //
// 
                    break; //
// 
                // TODO 20200219 新增
// 
                case frameType_XiTongJiShi:
// 
                    HandleCanDataPinJie(canDataId, ref statusBuffer_XiTongJiShi26,
// 
                        ref totalCountCan_XiTongJiShi26, ref frameLength_XiTongJiShi26, ref bRecvHeader_XiTongJiShi26,
// 
                        ref frameNO_XiTongJiShi26, buffer); //
// 
                    break;
// 
                default:
// 
                    break; //
// 
            }
// 
        }
// 

// 
        /// <summary>
// 
        /// ParseStatusData
// 
        
// 
        /// <param name="buffer"></param>
// 
        /// <param name="canId"></param>
// 
        /// <param name="frameType"></param>
// 
        /// <param name="frameNO"></param>
// 
        private void ParseStatusData(byte[] buffer, byte canId, byte frameType, UInt16 frameNO)
// 0
        {
// 
            // TODO 添加CRC16校验待定，此处需要在原数据获取上，添加两个字节
// 
            if (!ParseDataCRC16(buffer))
// 
            {
// 
                return; //
// 
            }
// 

// 
            //---------------------------------------//
// 

// 
            switch (canId)
// 
            {
// 
                case frameType_systemStatus_1:                  // 系统判据状态
// 
                    ParseStatusData_SystemStatus(buffer, canId, frameType_XTPJZT, frameNO); //
// 
                    break; //
// 
                case frameType_systemStatus_2:                  // 系统判据状态
// 
                    if (frameType == frameType_XTPJFK)
// 
                    {
// 
                        ParseStatusData_SystemStatus(buffer, canId, frameType, frameNO); //
// 
                    }
// 
//                    else if (frameType == frameType_HLJCFK)
//// 
//                    {
//// 
//                        ParseStatusData_huiLuJianCe(buffer, canId, frameType, frameNO); //
//// 
//                    }
// 
                    // 重新置为0
// 
                    curFrameType = 0x00; 
                    break; //
// 
                // TODO 注意导航快速数据需要分别显示在弹头弹体上  20210312改
                /*去除弹头显示
                 *0x35 弹头导航数据显示
                 */
// 
                case frameType_daoHangKuaiSu:                // 导航快速（弹体）
                    ParseStatusData_daoHangKuaiSu(buffer, canId, frameNO); //                                                             // 
                    break; 
// 
                // TODO 注意导航慢速数据需要分别显示在弹头弹体上
                case frameType_daoHangManSu:                 // 导航慢速（弹体）
                    ParseStatusData_daoHangManSu(buffer, canId, frameNO); 
                    break; 
                    
                case frameType_danTouDaoHang:                //0x35 弹头导航数据显示
                    ParseStatusData_daoTouDaoHangData(buffer, canId, frameNO);
                    break;
                // 
                // TODO 20200219 新增
                // 
                case frameType_XiTongJiShi:                 // 系统状态即时反馈（弹体）=
                    ParseStatusData_XiTongJiShi(buffer, canId, frameNO); 
                    break; 

                default:
                    break; 
            }
        }

        private static ushort[] crc16tab = new ushort[] {0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
                                0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef,
                                0x1231,0x0210,0x3273,0x2252,0x52b5,0x4294,0x72f7,0x62d6,
                                0x9339,0x8318,0xb37b,0xa35a,0xd3bd,0xc39c,0xf3ff,0xe3de,
                                0x2462,0x3443,0x0420,0x1401,0x64e6,0x74c7,0x44a4,0x5485,
                                0xa56a,0xb54b,0x8528,0x9509,0xe5ee,0xf5cf,0xc5ac,0xd58d,
                                0x3653,0x2672,0x1611,0x0630,0x76d7,0x66f6,0x5695,0x46b4,
                                0xb75b,0xa77a,0x9719,0x8738,0xf7df,0xe7fe,0xd79d,0xc7bc,
                                0x48c4,0x58e5,0x6886,0x78a7,0x0840,0x1861,0x2802,0x3823,
                                0xc9cc,0xd9ed,0xe98e,0xf9af,0x8948,0x9969,0xa90a,0xb92b,
                                0x5af5,0x4ad4,0x7ab7,0x6a96,0x1a71,0x0a50,0x3a33,0x2a12,
                                0xdbfd,0xcbdc,0xfbbf,0xeb9e,0x9b79,0x8b58,0xbb3b,0xab1a,
                                0x6ca6,0x7c87,0x4ce4,0x5cc5,0x2c22,0x3c03,0x0c60,0x1c41,
                                0xedae,0xfd8f,0xcdec,0xddcd,0xad2a,0xbd0b,0x8d68,0x9d49,
                                0x7e97,0x6eb6,0x5ed5,0x4ef4,0x3e13,0x2e32,0x1e51,0x0e70,
                                0xff9f,0xefbe,0xdfdd,0xcffc,0xbf1b,0xaf3a,0x9f59,0x8f78,
                                0x9188,0x81a9,0xb1ca,0xa1eb,0xd10c,0xc12d,0xf14e,0xe16f,
                                0x1080,0x00a1,0x30c2,0x20e3,0x5004,0x4025,0x7046,0x6067,
                                0x83b9,0x9398,0xa3fb,0xb3da,0xc33d,0xd31c,0xe37f,0xf35e,
                                0x02b1,0x1290,0x22f3,0x32d2,0x4235,0x5214,0x6277,0x7256,
                                0xb5ea,0xa5cb,0x95a8,0x8589,0xf56e,0xe54f,0xd52c,0xc50d,
                                0x34e2,0x24c3,0x14a0,0x0481,0x7466,0x6447,0x5424,0x4405,
                                0xa7db,0xb7fa,0x8799,0x97b8,0xe75f,0xf77e,0xc71d,0xd73c,
                                0x26d3,0x36f2,0x0691,0x16b0,0x6657,0x7676,0x4615,0x5634,
                                0xd94c,0xc96d,0xf90e,0xe92f,0x99c8,0x89e9,0xb98a,0xa9ab,
                                0x5844,0x4865,0x7806,0x6827,0x18c0,0x08e1,0x3882,0x28a3,
                                0xcb7d,0xdb5c,0xeb3f,0xfb1e,0x8bf9,0x9bd8,0xabbb,0xbb9a,
                                0x4a75,0x5a54,0x6a37,0x7a16,0x0af1,0x1ad0,0x2ab3,0x3a92,
                                0xfd2e,0xed0f,0xdd6c,0xcd4d,0xbdaa,0xad8b,0x9de8,0x8dc9,
                                0x7c26,0x6c07,0x5c64,0x4c45,0x3ca2,0x2c83,0x1ce0,0x0cc1,
                                0xef1f,0xff3e,0xcf5d,0xdf7c,0xaf9b,0xbfba,0x8fd9,0x9ff8,
                                0x6e17,0x7e36,0x4e55,0x5e74,0x2e93,0x3eb2,0x0ed1,0x1ef0};


        public static ushort Crc16(byte[] buf, int index, int len)
        {
            ushort crc = 0;
            for (int counter = 0; counter <= len - 1; counter++)
            {
                crc = (ushort)((crc << 8) ^ crc16tab[((crc >> 8) ^ buf[index + counter]) & 0xFF]);
            }
            return crc;
        }

        private bool ParseDataCRC16(byte[] buffer)
        {
            if(buffer.Length <= CRCLENGTH)
            {
                return false;
            }

            ushort crc = Crc16(buffer, 0, buffer.Length - CRCLENGTH);
            ushort packetCrc = BitConverter.ToUInt16(buffer, buffer.Length - CRCLENGTH);

            return crc == packetCrc;
        }


        /// ParseDataCRC16
        /// <param name="buffer"></param>
        /// <returns></returns>
       /* private bool ParseDataCRC16(byte[] buffer)
        {
            // CRC16
            string crcValueSTR = CRC.ToCRC16(buffer, true); //    // bool 是否逆序
// 
            byte[] crcValue = CRC.StringToHexByte(crcValueSTR); //
// 
            // 取出最后两字节的校验位
// 
            byte crcHig = buffer[buffer.Length - 1]; //
// 
            byte crcLow = buffer[buffer.Length - 2]; //
// 
            if (crcValue.Length >= 2)
// 
            {
// 
                if (crcValue[0] == crcLow && crcValue[0] == crcHig)
// 
                {
// 
                    return true; //
// 
                }
// 
                // TODO 测试阶段，不添加添加CRC16校验
// 
                // return false; //
// 
            }
// 

// 
            return true; //
// 
        }*/

        //弹头导航数据
        private void ParseStatusData_daoTouDaoHangData(byte[] buffer, byte canId, UInt16 frameNO)
        { 
            if (buffer.Length < Marshal.SizeOf(typeof(DANTOUDAOHANGDATA)) + CRCLENGTH)
            { 
                return;  
            }
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    DANTOUDAOHANGDATA sObject = new DANTOUDAOHANGDATA
                    {
                        GNSSTime = br.ReadUInt32(),
                        jingDu_ZuHe = br.ReadInt32(),
                        weiDu_ZuHe = br.ReadInt32(),
                        gaoDu_ZuHe = br.ReadInt32(),
                        dongXiangSuDu_ZuHe = br.ReadInt32(),
                        beiXiangSuDu_ZuHe = br.ReadInt32(),
                        tianXiangSuDu_ZuHe = br.ReadInt32(),
                        jingDu_GNSS = br.ReadInt32(),
                        weiDu_GNSS = br.ReadInt32(),
                        gaoDu_GNSS = br.ReadInt32(),
                        dongXiangSuDu_GNSS = br.ReadInt32(),
                        beiXiangSuDu_GNSS = br.ReadInt32(),
                        tianXiangSuDu_GNSS = br.ReadInt32(),
                        fuYangJiao = br.ReadSingle(),
                        gunZhuanJiao = br.ReadSingle(),
                        pianHangJiao = br.ReadSingle(),
                        WxJiaoSuDu = br.ReadSingle(),
                        WyJiaoSuDu = br.ReadSingle(),
                        WzJiaoSuDu = br.ReadSingle(),
                        xBiLi = br.ReadSingle(),
                        yBiLi = br.ReadSingle(),
                        zBiLi = br.ReadSingle(),
                        HDOP = br.ReadUInt16(),
                        VDOP = br.ReadUInt16(),
                        keShiWeiXingShu = br.ReadByte(),
                        shiYongWeiXingShu = br.ReadByte(),
                        tuoLuoGuZhangBiaoShi = br.ReadByte(),
                    };
                    // 向界面传递数据
                    if (PostMessageEnable)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DANTOUDAOHANGDATA)));
                        Marshal.StructureToPtr(sObject, ptr, true);
                        PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_danTouDaoHang_DATA, 0, ptr);
                    }
                    // 发送帧序号信息
                    postFrameInfo(canId, frameNO);  
                }
            }
        }

        // 
        /// <summary>
        // 
        /// ParseStatusData_SystemStatus
        // 
        
        // 
        /// <param name="buffer"></param>
        // 
        /// <param name="canId"></param>
        // 
        /// <param name="frameType"></param>
        // 
        /// <param name="frameNO"></param>
        // 
        private void ParseStatusData_SystemStatus(byte[] buffer, byte canId, byte frameType, UInt16 frameNO)
// 
        {
// 
            if (buffer.Length < Marshal.SizeOf(typeof(SYSTEMPARSE_STATUS)) + CRCLENGTH)
// 
            {
// 
                return; //
// 
            }
// 
            using (MemoryStream stream = new MemoryStream(buffer))
// 
            {
// 
                using (BinaryReader br = new BinaryReader(stream))
// 
                {
// 
                    SYSTEMPARSE_STATUS sObject = new SYSTEMPARSE_STATUS
// 
                    {
// 
                        // NOTE 20200506 协议弄反
// 
                        weiDu = br.ReadInt32(),                    // 纬度
// 
                        jingDu = br.ReadInt32(),                   // 经度
// 
                        haiBaGaoDu = br.ReadInt32(),               // 海拔高度
// 

// 
                        dongXiangSuDu = br.ReadInt32(),            // 东向速度
// 
                        beiXiangSuDu = br.ReadInt32(),             // 北向速度
// 
                        tianXiangSuDu = br.ReadInt32(),            // 天向速度
// 

// 
                        WxJiaoSuDu = br.ReadSingle(),               // Wx角速度
// 
                        WyJiaoSuDu = br.ReadSingle(),               // Wy角速度
// 
                        WzJiaoSuDu = br.ReadSingle(),               // Wz角速度
// 

// 
                        zhouXiangGuoZai = br.ReadSingle(),          // 轴向过载
// 
                        GNSSTime = br.ReadUInt32(),                 // GNSS时间
// 

// 
                        curFaSheXi_X = br.ReadSingle(),             // 当前发射系X
// 
                        curFaSheXi_Y = br.ReadSingle(),             // 当前发射系Y
// 
                        curFaSheXi_Z = br.ReadSingle(),             // 当前发射系Z
// 

// 
                        yuShiLuoDianSheCheng = br.ReadSingle(),     // 预示落点射程
// 
                        yuShiLuoDianZ = br.ReadSingle(),            // 预示落点Z
// 
                        feiXingZongShiJian = br.ReadSingle(),       // 飞行总时间
                                                                    // 
                        canShiZhuangTai = br.ReadByte(),    // 参试状态 0x00:无意义，0x01:正式实验，0x10：测试1，数据输出状态

                    // 
                    ceLueJieDuan = br.ReadByte(),               // 策略阶段(0-准备 1-起飞 2-一级 3-二级 4-结束)
                                                                // 

                        jueCePanJueJieGuo1 = br.ReadByte(),         // 策略判决结果1
                                                                    // 
                        jueCePanJueJieGuo2 = br.ReadByte(),         // 策略判决结果2

                        shuRuCaiJi1 = br.ReadByte(),    // 输入采集1

                        shuRuCaiJi2 = br.ReadByte(),    // 输入采集2

                        shuRuCaiJi3 = br.ReadByte(),    // 输入采集3

                        shuRuCaiJi4 = br.ReadByte(),    // 输入采集4

         danTouJieBaoXinHao = br.ReadSingle(),    // 弹头解保信号

         qiBaoXinHao = br.ReadSingle(),    // 起爆状态遥测信号

         neiBuKongZhiDianYa = br.ReadSingle(),    // 内部控制电压

         gongLvDianDianYa = br.ReadSingle(),  // 功率电电压

                        daoHangTip1 = br.ReadByte(),                // 导航状态提示1
// 
                        daoHangTip2 = br.ReadByte(),                // 导航状态提示2
// 
                        daoHangTip3 = br.ReadByte(),                // 导航状态提示3
// 
                        daoHangTip4 = br.ReadByte(),                // 导航状态提示4
                    }; //
// 
                    // 向界面传递数据
// 
                    
                                                                // 
                    if (PostMessageEnable)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SYSTEMPARSE_STATUS)));// 
                        Marshal.StructureToPtr(sObject, ptr, true); //
                        PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_SystemStatus_DATA, 0, ptr); //
                    }
// 

// 
                    // 发送帧序号信息
// 
                    postFrameInfo(canId, frameNO, frameType); //
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
        /// ParseStatusData_daoHangKuaiSu
// 
        
// 
        /// <param name="buffer"></param>
// 
        /// <param name="canId"></param>
// 
        /// <param name="frameNO"></param>
// 
        private void ParseStatusData_daoHangKuaiSu(byte[] buffer, byte canId, UInt16 frameNO)
// 
        {
// 
            if (buffer.Length < Marshal.SizeOf(typeof(DAOHANGSHUJU_KuaiSu)) + CRCLENGTH)
// 
            {
// 
                return; //
// 
            }
// 
            using (MemoryStream stream = new MemoryStream(buffer))
// 
            {
// 
                using (BinaryReader br = new BinaryReader(stream))
// 
                {
// 
                    DAOHANGSHUJU_KuaiSu sObject = new DAOHANGSHUJU_KuaiSu
// 
                    {
// 
                        daoHangXiTongShiJian = br.ReadUInt32(),     // 导航系统时间
// 
                        jingDu = br.ReadInt32(),                    // 经度（组合结果）当量：1e-7
// 
                        weiDu = br.ReadInt32(),                     // 纬度（组合结果）当量：1e-7
// 
                        haiBaGaoDu = br.ReadInt32(),                // 海拔高度（组合结果）当量：1e-7
// 

// 
                        dongXiangSuDu = br.ReadInt32(),             // 东向速度（组合结果）当量：1e-7
// 
                        beiXiangSuDu = br.ReadInt32(),              // 北向速度（组合结果）当量：1e-7
// 
                        tianXiangSuDu = br.ReadInt32(),             // 天向速度（组合结果）当量：1e-7
// 

// 
                        GNSSTime = br.ReadUInt32(),                 // GNSS时间 单位s,UTC秒部
// 
                        fuYangJiao = br.ReadSingle(),               // 俯仰角
// 
                        gunZhuanJiao = br.ReadSingle(),             // 滚转角
// 
                        pianHangJiao = br.ReadSingle(),             // 偏航角
// 

// 
                        // 上5ms速度
// 
                        tuoLuoShuJu_X = br.ReadSingle(),            // 陀螺X数据
// 
                        tuoLuoShuJu_Y = br.ReadSingle(),            // 陀螺Y数据
// 
                        tuoLuoShuJu_Z = br.ReadSingle(),            // 陀螺Z数据
// 

// 
                        // 上5ms加速度
// 
                        jiaSuDuJiShuJu_X = br.ReadSingle(),         // 加速度计X数据
// 
                        jiaSuDuJiShuJu_Y = br.ReadSingle(),         // 加速度计Y数据
// 
                        jiaSuDuJiShuJu_Z = br.ReadSingle(),         // 加速度计Z数据

                        zhuangTaiBiaoZhiWei = br.ReadByte(),        // 状态标志位
// 
                        tuoLuoGuZhangBiaoZhi = br.ReadByte(),       // 陀螺故障标志
                    };
// 
                    // 向界面传递数据
// 
                    
                                                                // 
                    if (PostMessageEnable)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DAOHANGSHUJU_KuaiSu))); //
                                                                                                        // 
                        Marshal.StructureToPtr(sObject, ptr, true); //
                        PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_daoHangKuaiSu_Ti_DATA, 0, ptr);
                    }

                    // 发送帧序号信息
                    postFrameInfo(canId, frameNO); 
                }
// 
            }
// 
        }
// 

// 
        /// <summary>
// 
        /// ParseStatusData_daoHangManSu
// 
        
// 
        /// <param name="buffer"></param>
// 
        /// <param name="canId"></param>
// 
        /// <param name="frameNO"></param>
// 
        private void ParseStatusData_daoHangManSu(byte[] buffer, byte canId, UInt16 frameNO)
// 
        {
// 
            if (buffer.Length < Marshal.SizeOf(typeof(DAOHANGSHUJU_ManSu)) + CRCLENGTH)
// 
            {
// 
                return; //
// 
            }
// 
            using (MemoryStream stream = new MemoryStream(buffer))
// 
            {
// 
                using (BinaryReader br = new BinaryReader(stream))
// 
                {
// 
                    DAOHANGSHUJU_ManSu sObject = new DAOHANGSHUJU_ManSu
// 
                    {
// 
                        GPSTime = br.ReadUInt32(),                          // GPS时间 单位s,UTC秒部
// 
                        GPSDingWeiMoShi = br.ReadByte(),                    // GPS定位模式
// 

// 
                        GPS_SV = br.ReadByte(),                             // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 
                        BD2_SV = br.ReadByte(),                             // BD2 SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 

// 
                        jingDu = br.ReadInt32(),                            // 经度（GPS测量）当量：1e-7
// 
                        weiDu = br.ReadInt32(),                             // 纬度（GPS测量）当量：1e-7
// 
                        haiBaGaoDu = br.ReadInt32(),                        // 海拔高度（GPS测量）当量：1e-2
// 

// 
                        dongXiangSuDu = br.ReadInt32(),                     // 东向速度（GPS测量）当量：1e-2
// 
                        beiXiangSuDu = br.ReadInt32(),                      // 北向速度（GPS测量）当量：1e-2
// 
                        tianXiangSuDu = br.ReadInt32(),                     // 天向速度（GPS测量）当量：1e-2
// 

// 
                        PDOP = br.ReadUInt16(),                             // PDOP 当量0.01
// 
                        HDOP = br.ReadUInt16(),                             // HDOP 当量0.01
// 
                        VDOP = br.ReadUInt16(),                             // VDOP 当量0.01
// 

// 
                        tuoLuoWenDu_X = br.ReadByte(),                      // X陀螺温度
// 
                        tuoLuoWenDu_Y = br.ReadByte(),                      // Y陀螺温度
// 
                        tuoLuoWenDu_Z = br.ReadByte(),                      // Z陀螺温度
// 

// 
                        jiaJiWenDu_X = br.ReadByte(),                       // X加计温度
// 
                        jiaJiWenDu_Y = br.ReadByte(),                       // Y加计温度
// 
                        jiaJiWenDu_Z = br.ReadByte(),                       // Z加计温度
// 

// 
                        dianYaZhi_zheng5V = br.ReadSByte(),                  // +5V电压值     当量0.05
// 
                        dianYaZhi_fu5V = br.ReadSByte(),                     // -5V电压值     当量0.05
// 

// 
                        dianYaZhi_zheng15V = br.ReadSByte(),                 // +15V电压值    当量0.02
// 
                        dianYaZhi_fu15V = br.ReadSByte(),                    // -15V电压值    当量0.02
// 

// 
                        tuoLuoDianYaZhi_X_zheng5V = br.ReadSByte(),          // X陀螺+5V电压值     当量0.05
// 
                        tuoLuoDianYaZhi_X_fu5V = br.ReadSByte(),             // X陀螺-5V电压值     当量0.05
// 

// 
                        tuoLuoDianYaZhi_Y_zheng5V = br.ReadSByte(),          // Y陀螺+5V电压值     当量0.05
// 
                        tuoLuoDianYaZhi_Y_fu5V = br.ReadSByte(),             // Y陀螺-5V电压值     当量0.05
// 

// 
                        tuoLuoDianYaZhi_Z_zheng5V = br.ReadSByte(),          // Z陀螺+5V电压值     当量0.05
// 
                        tuoLuoDianYaZhi_Z_fu5V = br.ReadSByte(),             // Z陀螺-5V电压值     当量0.05
// 

// 
                        yuTuoLuoTongXingCuoWuJiShu_X = br.ReadByte(),       // 与X陀螺通信错误计数（一直循环计数）
// 
                        yuTuoLuoTongXingCuoWuJiShu_Y = br.ReadByte(),       // 与Y陀螺通信错误计数（一直循环计数）
// 
                        yuTuoLuoTongXingCuoWuJiShu_Z = br.ReadByte(),       // 与Z陀螺通信错误计数（一直循环计数）
// 
                        yuGPSJieShouJiTongXingCuoWuJiShu = br.ReadByte(),   // 与GPS接收机通信错误计数（一直循环计数）
// 

// 
                        IMUJinRuZhongDuanCiShu = br.ReadByte(),             // IMU进入中断次数（每800次+1 循环计数）
// 
                        GPSZhongDuanCiShu = br.ReadByte(),                  // GPS中断次数（每10次+1 循环计数）
// 

// 
                        biaoZhiWei1 = br.ReadByte(),                        // 标志位1
// 
                        biaoZhiWei2 = br.ReadByte(),                        // 标志位2
                                                                            // 

                        jingDu_ZuHe = br.ReadInt32(),                            // 经度（GPS测量）当量：1e-7
                                                                                 // 
                        weiDu_ZuHe = br.ReadInt32(),                             // 纬度（GPS测量）当量：1e-7
                                                                                 // 
                        haiBaGaoDu_ZuHe = br.ReadInt32(),                        // 海拔高度（GPS测量）当量：1e-2
                                                                                 // 

                        // 
                        dongXiangSuDu_ZuHe = br.ReadInt32(),                     // 东向速度（GPS测量）当量：1e-2
                                                                                 // 
                        beiXiangSuDu_ZuHe = br.ReadInt32(),                      // 北向速度（GPS测量）当量：1e-2
                                                                                 // 
                        tianXiangSuDu_ZuHe = br.ReadInt32(),                     // 天向速度（GPS测量）当量：1e-2

                        fuYangJiao = br.ReadSingle(),               // 俯仰角
                                                                    // 
                        gunZhuanJiao = br.ReadSingle(),             // 滚转角
                                                                    // 
                        pianHangJiao = br.ReadSingle(),             // 偏航角
                                                                    // 

                        // 
                        // 上5ms速度
                        // 
                        tuoLuoShuJu_X = br.ReadSingle(),            // 陀螺X数据
                                                                    // 
                        tuoLuoShuJu_Y = br.ReadSingle(),            // 陀螺Y数据
                                                                    // 
                        tuoLuoShuJu_Z = br.ReadSingle(),            // 陀螺Z数据
                                                                    // 

                        // 
                        // 上5ms加速度
                        // 
                        jiaSuDuJiShuJu_X = br.ReadSingle(),         // 加速度计X数据
                                                                    // 
                        jiaSuDuJiShuJu_Y = br.ReadSingle(),         // 加速度计Y数据
                                                                    // 
                        jiaSuDuJiShuJu_Z = br.ReadSingle(),         // 加速度计Z数据
                                                                    // 
                                                                    //-----------------------------------------------------------------------------------------//
                                                                    // 

                        // 
                        /*
// 
                         GPSTime = br.ReadUInt32(),                          // GPS时间 单位s,UTC秒部
// 
                        GPSDingWeiMoShi = br.ReadByte(),                    // GPS定位模式
// 

// 
                        GPS_SV = br.ReadByte(),                             // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 
                        BD2_SV = br.ReadByte(),                             // BD2 SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 

// 
                        jingDu = br.ReadInt32(),                            // 经度（GPS测量）当量：1e-7
// 
                        weiDu = br.ReadInt32(),                             // 纬度（GPS测量）当量：1e-7
// 
                        haiBaGaoDu = br.ReadInt32(),                        // 海拔高度（GPS测量）当量：1e-2
// 

// 
                        dongXiangSuDu = br.ReadInt32(),                     // 东向速度（GPS测量）当量：1e-2
// 
                        beiXiangSuDu = br.ReadInt32(),                      // 北向速度（GPS测量）当量：1e-2
// 
                        tianXiangSuDu = br.ReadInt32(),                     // 天向速度（GPS测量）当量：1e-2
// 

// 
                        PDOP = br.ReadUInt16(),                             // PDOP 当量0.01
// 
                        HDOP = br.ReadUInt16(),                             // HDOP 当量0.01
// 
                        VDOP = br.ReadUInt16(),                             // VDOP 当量0.01
// 

// 
                        tuoLuoWenDu_X = br.ReadByte(),                      // X陀螺温度
// 
                        tuoLuoWenDu_Y = br.ReadByte(),                      // Y陀螺温度
// 
                        tuoLuoWenDu_Z = br.ReadByte(),                      // Z陀螺温度
// 

// 
                        jiaJiWenDu_X = br.ReadByte(),                       // X加计温度
// 
                        jiaJiWenDu_Y = br.ReadByte(),                       // Y加计温度
// 
                        jiaJiWenDu_Z = br.ReadByte(),                       // Z加计温度
// 

// 
                        dianYaZhi_zheng5V = br.ReadSByte(),                  // +5V电压值     当量0.05
// 
                        dianYaZhi_fu5V = br.ReadSByte(),                     // -5V电压值     当量0.05
// 

// 
                        dianYaZhi_zheng15V = br.ReadSByte(),                 // +15V电压值    当量0.02
// 
                        dianYaZhi_fu15V = br.ReadSByte(),                    // -15V电压值    当量0.02
// 

// 
                        tuoLuoDianYaZhi_X_zheng5V = br.ReadSByte(),          // X陀螺+5V电压值     当量0.05
// 
                        tuoLuoDianYaZhi_X_fu5V = br.ReadSByte(),             // X陀螺-5V电压值     当量0.05
// 

// 
                        tuoLuoDianYaZhi_Y_zheng5V = br.ReadSByte(),          // Y陀螺+5V电压值     当量0.05
// 
                        tuoLuoDianYaZhi_Y_fu5V = br.ReadSByte(),             // Y陀螺-5V电压值     当量0.05
// 

// 
                        tuoLuoDianYaZhi_Z_zheng5V = br.ReadSByte(),          // Z陀螺+5V电压值     当量0.05
// 
                        tuoLuoDianYaZhi_Z_fu5V = br.ReadSByte(),             // Z陀螺-5V电压值     当量0.05
// 

// 
                        yuTuoLuoTongXingCuoWuJiShu_X = br.ReadByte(),       // 与X陀螺通信错误计数（一直循环计数）
// 
                        yuTuoLuoTongXingCuoWuJiShu_Y = br.ReadByte(),       // 与Y陀螺通信错误计数（一直循环计数）
// 
                        yuTuoLuoTongXingCuoWuJiShu_Z = br.ReadByte(),       // 与Z陀螺通信错误计数（一直循环计数）
// 
                        yuGPSJieShouJiTongXingCuoWuJiShu = br.ReadByte(),   // 与GPS接收机通信错误计数（一直循环计数）
// 

// 
                        IMUJinRuZhongDuanCiShu = br.ReadByte(),             // IMU进入中断次数（每800次+1 循环计数）
// 
                        GPSZhongDuanCiShu = br.ReadByte(),                  // GPS中断次数（每10次+1 循环计数）
// 

// 
                        biaoZhiWei1 = br.ReadByte(),                        // 标志位1
// 
                        biaoZhiWei2 = br.ReadByte(),                        // 标志位2
// 
                         */
                        // 
                    }; //
// 
                    // 向界面传递数据
// 
                    
                                                                // 
                                                                // 
                    if (PostMessageEnable)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DAOHANGSHUJU_ManSu))); //
                                                                                                       // 
                        Marshal.StructureToPtr(sObject, ptr, true); //
                        PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_daoHangManSu_Ti_DATA, 0, ptr);
                    }

                    // 发送帧序号信息
                    postFrameInfo(canId, frameNO); 
                }
            }
        }
        // 

        // 
        /// <summary>
        // 
        /// ParseStatusData_huiLuJianCe
        // 

        // 
        /// <param name="buffer"></param>
        // 
        /// <param name="canId"></param>
        // 
        /// <param name="frameType"></param>
        // 
        /// <param name="frameNO"></param>
        // 
#if false
        private void ParseStatusData_huiLuJianCe(byte[] buffer, byte canId, byte frameType, UInt16 frameNO)
// 
        {
// 
            if (buffer.Length < Marshal.SizeOf(typeof(HUILUJIANCE_STATUS)) + CRCLENGTH)
// 
            {
// 
                return; //
// 
            }
// 
            using (MemoryStream stream = new MemoryStream(buffer))
// 
            {
// 
                using (BinaryReader br = new BinaryReader(stream))
// 
                {
// 
                    HUILUJIANCE_STATUS sObject = new HUILUJIANCE_STATUS
// 
                    {
// 
                        shuChu1HuiLuDianZu = br.ReadSingle(),    // 电机驱动输出1回路电阻
// 
                        reserve1 = br.ReadUInt32(),              // 保留1
// 
                        shuChu2HuiLuDianZu = br.ReadSingle(),    // 电机驱动输出2回路电阻
// 
                        reserve2 = br.ReadUInt32(),              // 保留2
// 
                        QBDH1AHuiLuDianZu = br.ReadSingle(),     // 起爆点火1A回路电阻
// 
                        QBDH1BHuiLuDianZu = br.ReadSingle(),     // 起爆点火1B回路电阻
// 
                        QBDH2AHuiLuDianZu = br.ReadSingle(),     // 起爆点火2A回路电阻
// 
                        QBDH2BHuiLuDianZu = br.ReadSingle()      // 起爆点火2B回路电阻
// 

// 
                        //--------------------------------------------------------------//
// 

// 
                        /*
// 
                         shuChu1HuiLuDianZu = br.ReadSingle(),    // 电机驱动输出1回路电阻
// 
                        reserve1 = br.ReadUInt32(),              // 保留1
// 
                        shuChu2HuiLuDianZu = br.ReadSingle(),    // 电机驱动输出2回路电阻
// 
                        reserve2 = br.ReadUInt32(),              // 保留2
// 
                        QBDH1AHuiLuDianZu = br.ReadSingle(),     // 起爆点火1A回路电阻
// 
                        QBDH1BHuiLuDianZu = br.ReadSingle(),     // 起爆点火1B回路电阻
// 
                        QBDH2AHuiLuDianZu = br.ReadSingle(),     // 起爆点火2A回路电阻
// 
                        QBDH2BHuiLuDianZu = br.ReadSingle()      // 起爆点火2B回路电阻
// 
                         */
// 
                    }; //
// 
                    // 向界面传递数据
// 
                    
// 
                    if (PostMessageEnable)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(HUILUJIANCE_STATUS))); //
// 
                        Marshal.StructureToPtr(sObject, ptr, true); //
                        PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_HuiLuJianCe_DATA, 0, ptr); //
                    }
// 
                    //-------------------------------------------------------------------------------//
// 
                    // 发送帧序号信息
// 
                    postFrameInfo(canId, frameNO, frameType); //
// 
                }
// 
            }
// 
        }
#endif
        // 

        // 
        /// <summary>
        // 
        /// ParseStatusData_XiTongJiShi
        // 

        // 
        /// <param name="buffer"></param>
        // 
        /// <param name="canId"></param>
        // 
        /// <param name="frameNO"></param>
        // 
        private void ParseStatusData_XiTongJiShi(byte[] buffer, byte canId, UInt16 frameNO)
// 
        {
// 
            if (buffer.Length < Marshal.SizeOf(typeof(SYSTEMImmediate_STATUS)) + CRCLENGTH)
// 
            {
// 
                return; //
// 
            }
// 
            using (MemoryStream stream = new MemoryStream(buffer))
// 
            {
// 
                using (BinaryReader br = new BinaryReader(stream))
// 
                {
// 
                    SYSTEMImmediate_STATUS sObject = new SYSTEMImmediate_STATUS
// 
                    {
// 
                        guZhangBiaoZhi = br.ReadByte(),         // 故障标志位
// 

// 
                        tuoLuoWenDu_X = br.ReadByte(),          // X陀螺温度
// 
                        tuoLuoWenDu_Y = br.ReadByte(),          // Y陀螺温度
// 
                        tuoLuoWenDu_Z = br.ReadByte(),          // Z陀螺温度
// 

// 
                        GPS_SV = br.ReadByte(),                 // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 
                        GPSDingWeiMoShi = br.ReadByte(),        // GPS定位模式
// 

// 
                        PDOP = br.ReadUInt16(),                 // PDOP 
// 
                        HDOP = br.ReadUInt16(),                 // HDOP 
// 
                        VDOP = br.ReadUInt16(),                 // VDOP 
// 

// 
                        GPSTime = br.ReadUInt32(),              // GPS时间 单位s,UTC秒，当量：0.1
// 

// 
                        jingDu = br.ReadInt32(),                // 经度           当量：1e-7
// 
                        weiDu = br.ReadInt32(),                 // 纬度           当量：1e-7
// 
                        haiBaGaoDu = br.ReadInt32(),            // 海拔高度       当量：1e-2
// 

// 
                        dongXiangSuDu = br.ReadInt32(),         // 东向速度       当量：1e-2
// 
                        beiXiangSuDu = br.ReadInt32(),          // 北向速度       当量：1e-2
// 
                        tianXiangSuDu = br.ReadInt32(),         // 天向速度       当量：1e-2
// 

// 
                        zhouXiangGuoZai = br.ReadSingle(),      // 轴向过载
// 
                        faXiangGuoZai = br.ReadSingle(),        // 法向过载
// 
                        ceXiangGuoZai = br.ReadSingle(),        // 侧向过载
// 

// 
                        WxJiaoSuDu = br.ReadSingle(),           // Wx角速度
// 
                        WyJiaoSuDu = br.ReadSingle(),           // Wy角速度
// 
                        WzJiaoSuDu = br.ReadSingle(),           // Wz角速度
                                                                // 
                        BD2SV = br.ReadByte(),                  // BD2SV 可用/参与定位
                        // 
                        //-----------------------------------------------------------------//
                        // 

                        // 
                        /*
// 
                         guZhangBiaoZhi = br.ReadByte(),         // 故障标志位
// 

// 
                        tuoLuoWenDu_X = br.ReadByte(),          // X陀螺温度
// 
                        tuoLuoWenDu_Y = br.ReadByte(),          // Y陀螺温度
// 
                        tuoLuoWenDu_Z = br.ReadByte(),          // Z陀螺温度
// 

// 
                        GPS_SV = br.ReadByte(),                 // GPS SV可用/参与定位数（低4位为可用数，高4位为参与定位数）
// 
                        GPSDingWeiMoShi = br.ReadByte(),        // GPS定位模式
// 

// 
                        PDOP = br.ReadUInt16(),                 // PDOP 
// 
                        HDOP = br.ReadUInt16(),                 // HDOP 
// 
                        VDOP = br.ReadUInt16(),                 // VDOP 
// 

// 
                        GPSTime = br.ReadUInt32(),              // GPS时间 单位s,UTC秒，当量：0.1
// 

// 
                        jingDu = br.ReadInt32(),                // 经度           当量：1e-7
// 
                        weiDu = br.ReadInt32(),                 // 纬度           当量：1e-7
// 
                        haiBaGaoDu = br.ReadInt32(),            // 海拔高度       当量：1e-2
// 

// 
                        dongXiangSuDu = br.ReadInt32(),         // 东向速度       当量：1e-2
// 
                        beiXiangSuDu = br.ReadInt32(),          // 北向速度       当量：1e-2
// 
                        tianXiangSuDu = br.ReadInt32(),         // 天向速度       当量：1e-2
// 

// 
                        zhouXiangGuoZai = br.ReadSingle(),      // 轴向过载
// 
                        faXiangGuoZai = br.ReadSingle(),        // 法向过载
// 
                        ceXiangGuoZai = br.ReadSingle(),        // 侧向过载
// 

// 
                        WxJiaoSuDu = br.ReadSingle(),           // Wx角速度
// 
                        WyJiaoSuDu = br.ReadSingle(),           // Wy角速度
// 
                        WzJiaoSuDu = br.ReadSingle(),           // Wz角速度
// 
                         */
                        // 
                    }; //
// 
                    // 向界面传递数据
// 
                    
                                                                // 
                    if (PostMessageEnable)
                    {
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SYSTEMImmediate_STATUS))); //
                                                                                                           // 
                        Marshal.StructureToPtr(sObject, ptr, true); //
                        PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_XiTongJiShi_Ti_DATA, 0, ptr); //
                    }

// 
                    //-------------------------------------------------------------------------------//
// 
                    // 发送帧序号信息
// 
                    postFrameInfo(canId, frameNO); //
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
        /// postFrameInfo
// 
        
// 
        /// <param name="canId"></param>
// 
        /// <param name="frameNO"></param>
// 
        /// <param name="frameType"></param>
// 
        private void postFrameInfo(byte canId, UInt16 frameNO, byte frameType = 0)
// 
        {
// 
            FRAME_PROPERTY frameObject = new FRAME_PROPERTY
// 
            {
// 
                CanId = canId,              // 帧ID
// 
                frameType = frameType,      // 帧类型
// 
                frameNo = frameNO,          // 帧序号
// 
            }; //
// 
            
                                                                 // 
            if (PostMessageEnable)
            {
                IntPtr ptrFrame = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FRAME_PROPERTY))); //
                                                                                                // 
                Marshal.StructureToPtr(frameObject, ptrFrame, true); //
                PostMessage(mainFormHandle, YaoCeShuJuXianShi.WM_YAOCE_FRAMEPROPERTY_DATA, 0, ptrFrame); //
            }
// 
        }
// 
    }
    // 
}
// 
