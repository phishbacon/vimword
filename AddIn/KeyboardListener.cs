using Microsoft.Office.Interop.Word;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using vimword.Vimulator;

namespace vimword.AddIn
{
    /// <summary>
    /// Keyboard hook that intercepts key presses before Word processes them.
    /// Uses Windows API hooks at the thread level.
    /// </summary>
    public class KeyboardListener
    {
        private delegate IntPtr KeyboardProc(int code, IntPtr wParam, IntPtr lParam);
        
        private static IntPtr _hookID = IntPtr.Zero;
        private readonly KeyboardListener _instance;
        private static KeyboardProc _proc;

        private const int WH_KEYBOARD = 2;
        private const int HC_ACTION = 0;

        private readonly IVimMachine _vimMachine;

        public KeyboardListener(IVimMachine vimMachine)
        {
            _vimMachine = vimMachine;
            _instance = this;
        }

        public void Install()
        {
            if (_hookID == IntPtr.Zero)
            {
                _proc = HookCallback;
                _hookID = SetWindowsHookEx(WH_KEYBOARD, _proc, IntPtr.Zero, GetCurrentThreadId());
            }
        }

        public void Uninstall()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }
        
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (_instance != null && nCode == HC_ACTION && BindingFunctions.IsKeyDown((Keys)wParam))
            {
                Keys key = (Keys)wParam;
                bool handled = _instance._vimMachine.HandleKey(key);

                if (handled)
                {
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        #region Win32 API Imports

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        #endregion
    }

    public class BindingFunctions
    {
        [DllImport("user32.dll")]
        static extern short GetKeyState(int nVirtKey);

        public static bool IsKeyDown(Keys keys)
        {
            return (GetKeyState((int)keys) & 0x8000) == 0x8000;
        }
    }
}
