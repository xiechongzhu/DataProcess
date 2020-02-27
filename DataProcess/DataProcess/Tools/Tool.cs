using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Tools
{
    public static class Tool
    {   
        //将Byte转换为结构体类型
        public static T ByteToStruct<T>(byte[] bytes, int offset, int count)
        {
            //分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(count);
            //将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, offset, structPtr, count);
            //将内存空间转换为目标结构体
            T obj = (T)Marshal.PtrToStructure(structPtr, typeof(T));
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        public static ushort SwapUInt16(this ushort n)
        {
            return (ushort)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
        }

        public static ushort Channel(this ushort n)
        {
            return (ushort)((n >> 12) & 0xf);
        }

        public static ushort Data(this ushort n)
        {
            return (ushort)(n & 0x0fff);
        }
    }
}
