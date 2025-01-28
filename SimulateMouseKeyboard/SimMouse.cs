// SimulateMouseKeyboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// SimulateMouseKeyboard.SimMouse
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SimulateMouseKeyboard
{
    public static class SimMouse
    {
        public static void Click(MouseButton mouseButton)
        {
            Down(mouseButton);
            Up(mouseButton);
        }

        public static void DoubleClick(MouseButton mouseButton)
        {
            Click(mouseButton);
            Click(mouseButton);
        }

        public static void Down(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.LeftDown);
                    break;
                case MouseButton.Right:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.RightDown);
                    break;
                case MouseButton.Middle:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.MiddleDown);
                    break;
                case MouseButton.XButton1:
                    SendMouseInput(0, 0, 1, NativeMethods.SendMouseInputFlags.XDown);
                    break;
                case MouseButton.XButton2:
                    SendMouseInput(0, 0, 2, NativeMethods.SendMouseInputFlags.XDown);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported MouseButton input.");
            }
        }

        public static void MoveTo(Point point)
        {
            SendMouseInput(point.X, point.Y, 0, NativeMethods.SendMouseInputFlags.Move | NativeMethods.SendMouseInputFlags.Absolute);
        }

        public static void Reset()
        {
            MoveTo(new Point(0, 0));
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.LeftUp);
            }
            if (Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.MiddleUp);
            }
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.RightUp);
            }
            if (Mouse.XButton1 == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 1, NativeMethods.SendMouseInputFlags.XUp);
            }
            if (Mouse.XButton2 == MouseButtonState.Pressed)
            {
                SendMouseInput(0, 0, 2, NativeMethods.SendMouseInputFlags.XUp);
            }
        }

        public static void Scroll(double lines)
        {
            int amount = (int)(120.0 * lines);
            SendMouseInput(0, 0, amount, NativeMethods.SendMouseInputFlags.Wheel);
        }

        public static void Up(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.LeftUp);
                    break;
                case MouseButton.Right:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.RightUp);
                    break;
                case MouseButton.Middle:
                    SendMouseInput(0, 0, 0, NativeMethods.SendMouseInputFlags.MiddleUp);
                    break;
                case MouseButton.XButton1:
                    SendMouseInput(0, 0, 1, NativeMethods.SendMouseInputFlags.XUp);
                    break;
                case MouseButton.XButton2:
                    SendMouseInput(0, 0, 2, NativeMethods.SendMouseInputFlags.XUp);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported MouseButton input.");
            }
        }

        private static void SendMouseInput(int x, int y, int data, NativeMethods.SendMouseInputFlags flags)
        {
            int intflags = (int)flags;
            if (((uint)intflags & 0x8000u) != 0)
            {
                NormalizeCoordinates(ref x, ref y);
                intflags |= 0x4000;
            }
            NativeMethods.INPUT mi = default(NativeMethods.INPUT);
            mi.type = 0;
            mi.union.mouseInput.dx = x;
            mi.union.mouseInput.dy = y;
            mi.union.mouseInput.mouseData = data;
            mi.union.mouseInput.dwFlags = intflags;
            mi.union.mouseInput.time = 0;
            mi.union.mouseInput.dwExtraInfo = new IntPtr(0);
            if (NativeMethods.SendInput(1, ref mi, Marshal.SizeOf(mi)) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private static void NormalizeCoordinates(ref int x, ref int y)
        {
            int vScreenWidth = NativeMethods.GetSystemMetrics(78);
            int vScreenHeight = NativeMethods.GetSystemMetrics(79);
            int vScreenLeft = NativeMethods.GetSystemMetrics(76);
            int vScreenTop = NativeMethods.GetSystemMetrics(77);
            x = (x - vScreenLeft) * 65536 / vScreenWidth + 65536 / (vScreenWidth * 2);
            y = (y - vScreenTop) * 65536 / vScreenHeight + 65536 / (vScreenHeight * 2);
        }
    }
}