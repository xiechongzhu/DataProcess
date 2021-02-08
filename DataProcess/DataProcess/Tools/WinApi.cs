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
        public const int WM_USER = 0x400;
        public const int WM_SLOW_DATA = WM_USER + 100;
        public const int WM_FAST_DATA = WM_USER + 101;
        public const int WM_TAIL_DATA = WM_USER + 102;
        public const int WM_NAV_DATA = WM_USER + 103;
        public const int WM_ANGLE_DATA = WM_USER + 104;
        public const int WM_PROGRAM_DATA = WM_USER + 105;
        public const int WM_SERVO_DATA = WM_USER + 106;
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr hwnd, int Msg, int wParam, IntPtr lParam);
    }
}
