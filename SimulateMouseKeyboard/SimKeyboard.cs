// SimulateMouseKeyboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// SimulateMouseKeyboard.SimKeyboard
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SimulateMouseKeyboard
{
    public static class SimKeyboard
    {
        private static readonly Key[] ExtendedKeys = new Key[16]
        {
        Key.RightAlt,
        Key.RightCtrl,
        Key.NumLock,
        Key.Insert,
        Key.Delete,
        Key.Home,
        Key.End,
        Key.Prior,
        Key.Next,
        Key.Up,
        Key.Down,
        Key.Left,
        Key.Right,
        Key.Apps,
        Key.RWin,
        Key.LWin
        };

        public static void Press(Key key)
        {
            SendKeyboardInput(key, press: true);
        }

        public static void Release(Key key)
        {
            SendKeyboardInput(key, press: false);
        }

        public static void Reset()
        {
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (key != 0 && (int)(Keyboard.GetKeyStates(key) & KeyStates.Down) > 0)
                {
                    Release(key);
                }
            }
        }

        public static void Type(Key key)
        {
            Press(key);
            Release(key);
        }

        public static void Type(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                short num = NativeMethods.VkKeyScan(text[i]);
                bool keyIsShifted = (num & 0x100) == 256;
                Key key = KeyInterop.KeyFromVirtualKey(num & 0xFF);
                if (keyIsShifted)
                {
                    Type(key, new Key[1] { Key.LeftShift });
                }
                else
                {
                    Type(key);
                }
            }
        }

        private static void Type(Key key, Key[] modifierKeys)
        {
            for (int i = 0; i < modifierKeys.Length; i++)
            {
                Press(modifierKeys[i]);
            }
            Type(key);
            foreach (Key item in modifierKeys.Reverse())
            {
                Release(item);
            }
        }

        private static void SendKeyboardInput(Key key, bool press)
        {
            NativeMethods.INPUT ki = default(NativeMethods.INPUT);
            ki.type = 1;
            ki.union.keyboardInput.wVk = (short)KeyInterop.VirtualKeyFromKey(key);
            ki.union.keyboardInput.wScan = (short)NativeMethods.MapVirtualKey(ki.union.keyboardInput.wVk, 0);
            int dwFlags = 0;
            if (ki.union.keyboardInput.wScan > 0)
            {
                dwFlags |= 8;
            }
            if (!press)
            {
                dwFlags |= 2;
            }
            ki.union.keyboardInput.dwFlags = dwFlags;
            if (ExtendedKeys.Contains(key))
            {
                ki.union.keyboardInput.dwFlags |= 1;
            }
            ki.union.keyboardInput.time = 0;
            ki.union.keyboardInput.dwExtraInfo = new IntPtr(0);
            if (NativeMethods.SendInput(1, ref ki, Marshal.SizeOf(ki)) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}