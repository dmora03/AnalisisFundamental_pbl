// SimulateMouseKeyboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// SimulateMouseKeyboard.NativeMethods
using System;
using System.Runtime.InteropServices;

namespace SimulateMouseKeyboard
{
    internal static class NativeMethods
    {
        internal struct INPUT
        {
            internal int type;

            internal INPUTUNION union;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUTUNION
        {
            [FieldOffset(0)]
            internal MOUSEINPUT mouseInput;

            [FieldOffset(0)]
            internal KEYBDINPUT keyboardInput;
        }

        internal struct MOUSEINPUT
        {
            internal int dx;

            internal int dy;

            internal int mouseData;

            internal int dwFlags;

            internal int time;

            internal IntPtr dwExtraInfo;
        }

        internal struct KEYBDINPUT
        {
            internal short wVk;

            internal short wScan;

            internal int dwFlags;

            internal int time;

            internal IntPtr dwExtraInfo;
        }

        [Flags]
        internal enum SendMouseInputFlags
        {
            Move = 1,
            LeftDown = 2,
            LeftUp = 4,
            RightDown = 8,
            RightUp = 0x10,
            MiddleDown = 0x20,
            MiddleUp = 0x40,
            XDown = 0x80,
            XUp = 0x100,
            Wheel = 0x800,
            Absolute = 0x8000
        }

        internal const int VKeyShiftMask = 256;

        internal const int VKeyCharMask = 255;

        internal const int KeyeventfExtendedkey = 1;

        internal const int KeyeventfKeyup = 2;

        internal const int KeyeventfScancode = 8;

        internal const int MouseeventfVirtualdesk = 16384;

        internal const int SMXvirtualscreen = 76;

        internal const int SMYvirtualscreen = 77;

        internal const int SMCxvirtualscreen = 78;

        internal const int SMCyvirtualscreen = 79;

        internal const int XButton1 = 1;

        internal const int XButton2 = 2;

        internal const int WheelDelta = 120;

        internal const int InputMouse = 0;

        internal const int InputKeyboard = 1;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int MapVirtualKey(int nVirtKey, int nMapType);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SendInput(int nInputs, ref INPUT mi, int cbSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern short VkKeyScan(char ch);
    }
}