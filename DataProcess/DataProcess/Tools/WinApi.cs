using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataProcess.Tools
{
    public class WinApi
    {
        private const int WM_USER = 0x400;
        public const int WM_SLOW_DATA = WM_USER + 100;
        public const int WM_FAST_DATA = WM_USER + 101;
        public const int WM_TAIL_DATA = WM_USER + 102;
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr hwnd, int Msg, int wParam, IntPtr lParam);
    }
}
