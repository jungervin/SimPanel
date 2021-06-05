using SimPanel.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.ViewModel
{
    public class WinManViewModel : ObservableObject
    {
        const short HWND_BOTTOM = 1;
        const int HWND_NOTOPMOST = -2;
        const int HWND_TOP = 0;
        const int HWND_TOPMOST = -1;

        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;

        const int SWP_ASYNCWINDOWPOS = 0x4000;

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        static IntPtr hHook = IntPtr.Zero;
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        public enum HookModes { None, G1000PFD, G1000MFD };

        public WinManViewModel() : base()
        {
            this.G1000PFDFindHandle = new RelayCommand(p =>
            {
                if (IntPtr.Zero == hHook)
                {
                    WinManViewModel.HookMode = HookModes.G1000PFD;
                    using (Process curProcess = Process.GetCurrentProcess())
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        hHook = SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
                    }
                }
            });

            this.G1000PFDSetPos = new RelayCommand(p =>
            {
                Settings.Default.G1000PFDPosX = this.G1000PFDPosX;
                Settings.Default.G1000PFDPosY = this.G1000PFDPosY;
                Settings.Default.G1000PFDPosW = this.G1000PFDPosW;
                Settings.Default.G1000PFDPosH = this.G1000PFDPosH;
                Settings.Default.Save();

                if (this.G1000PFDHandle != 0)
                {
                    //hwnd = (IntPtr)0x000E0546;
                    //SetWindowPos(hwnd, x, y, w, h, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                    SetWindowPos((IntPtr)this.G1000PFDHandle, HWND_TOP, this.G1000PFDPosX, this.G1000PFDPosY, this.G1000PFDPosW, this.G1000PFDPosH, SWP_SHOWWINDOW);
                }
            });

            this.G1000MFDFindHandle = new RelayCommand(p =>
            {
                if (IntPtr.Zero == hHook)
                {
                    WinManViewModel.HookMode = HookModes.G1000MFD;
                    using (Process curProcess = Process.GetCurrentProcess())
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        hHook = SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
                    }
                }
            });

            this.G1000MFDSetPos = new RelayCommand(p =>
            {
                Settings.Default.G1000MFDPosX = this.G1000MFDPosX;
                Settings.Default.G1000MFDPosY = this.G1000MFDPosY;
                Settings.Default.G1000MFDPosW = this.G1000MFDPosW;
                Settings.Default.G1000MFDPosH = this.G1000MFDPosH;
                Settings.Default.Save();

                if (this.G1000PFDHandle != 0)
                {
                    //hwnd = (IntPtr)0x000E0546;
                    //SetWindowPos(hwnd, x, y, w, h, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                    SetWindowPos((IntPtr)this.G1000MFDHandle, HWND_TOP, this.G1000MFDPosX, this.G1000MFDPosY, this.G1000MFDPosW, this.G1000MFDPosH, SWP_SHOWWINDOW);
                }
            });





        }

        public void Init()
        {
            this.G1000PFDPosX = Settings.Default.G1000PFDPosX;
            this.G1000PFDPosY = Settings.Default.G1000PFDPosY;
            this.G1000PFDPosW = Settings.Default.G1000PFDPosW;
            this.G1000PFDPosH = Settings.Default.G1000PFDPosH;

            this.G1000MFDPosX = Settings.Default.G1000MFDPosX;
            this.G1000MFDPosY = Settings.Default.G1000MFDPosY;
            this.G1000MFDPosW = Settings.Default.G1000MFDPosW;
            this.G1000MFDPosH = Settings.Default.G1000MFDPosH;

        }


        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                POINT cusorPoint;
                bool ret = GetCursorPos(out cusorPoint);
                IntPtr winHandle = WindowFromPoint(cusorPoint);

                switch (HookMode)
                {
                    case HookModes.G1000PFD:
                        Globals.MainWindow.WinManViewModel.G1000PFDHandle = (int)winHandle;
                        break;

                    case HookModes.G1000MFD:
                        Globals.MainWindow.WinManViewModel.G1000MFDHandle = (int)winHandle;
                        break;
                }

                UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
                WinManViewModel.HookMode = HookModes.None;

                // Here I do not use the GetActiveWindow(). Let's call the window you clicked "DesWindow" and explain my reason.
                // I think the hook intercepts the mouse click message before the mouse click message delivered to the DesWindow's 
                // message queue. The application came to this function before the DesWindow became the active window, so the handle 
                // abtained from calling GetActiveWindow() here is not the DesWindow's handle, I did some tests, and What I got is always 
                // the Form's handle, but not the DesWindow's handle. You can do some test too.

                //IntPtr handle = GetActiveWindow();


            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }








        private int FG1000PFDHandle;
        public int G1000PFDHandle
        {
            get { return FG1000PFDHandle; }
            set
            {
                FG1000PFDHandle = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000PFDPosX;

        public int G1000PFDPosX
        {
            get { return FG1000PFDPosX; }
            set
            {
                FG1000PFDPosX = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000PFDPosY;

        public int G1000PFDPosY
        {
            get { return FG1000PFDPosY; }
            set
            {
                FG1000PFDPosY = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000PFDPosW;

        public int G1000PFDPosW
        {
            get { return FG1000PFDPosW; }
            set
            {
                FG1000PFDPosW = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000PFDPosH;

        public int G1000PFDPosH
        {
            get { return FG1000PFDPosH; }
            set
            {
                FG1000PFDPosH = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000MFDHandle;
        public int G1000MFDHandle
        {
            get { return FG1000MFDHandle; }
            set
            {
                FG1000MFDHandle = value;
                this.OnPropertyChanged();
            }
        }


        private int FG1000MFDPosX;

        public int G1000MFDPosX
        {
            get { return FG1000MFDPosX; }
            set
            {
                FG1000MFDPosX = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000MFDPosY;

        public int G1000MFDPosY
        {
            get { return FG1000MFDPosY; }
            set
            {
                FG1000MFDPosY = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000MFDPosW;

        public int G1000MFDPosW
        {
            get { return FG1000MFDPosW; }
            set
            {
                FG1000MFDPosW = value;
                this.OnPropertyChanged();
            }
        }

        private int FG1000MFDPosH;

        public int G1000MFDPosH
        {
            get { return FG1000MFDPosH; }
            set
            {
                FG1000MFDPosH = value;
                this.OnPropertyChanged();
            }
        }


        public RelayCommand G1000PFDFindHandle { get; }
        public RelayCommand G1000PFDSetPos { get; }
        public RelayCommand G1000MFDFindHandle { get; }
        public RelayCommand G1000MFDSetPos { get; }
        private static HookModes HookMode = HookModes.None;
    }
}
