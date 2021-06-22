using SimPanel.Properties;
using SimPanel.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        public enum HookModes { None, G1000PFD, G1000MFD };


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

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

            //this.G1000PFDReadPos = new RelayCommand(p => {
            //    if (this.G1000PFDHandle > 0)
            //    {
            //        Rect rect = new Rect();
            //        if (GetWindowRect((IntPtr)this.G1000PFDHandle, ref rect))
            //        {
            //            this.G1000PFDPosX = Convert.ToInt32(rect.Left);
            //        }
            //    }
            //});

            this.G1000PFDSetPos = new RelayCommand(p =>
            {
                //Settings.Default.G1000PFDPosX = this.G1000PFDPosX;
                //Settings.Default.G1000PFDPosY = this.G1000PFDPosY;
                //Settings.Default.G1000PFDPosW = this.G1000PFDPosW;
                //Settings.Default.G1000PFDPosH = this.G1000PFDPosH;
                Settings.Default.Save();

                if (this.G1000PFDHandle != 0)
                {
                    //hwnd = (IntPtr)0x000E0546;
                    //SetWindowPos(hwnd, x, y, w, h, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                    SetWindowPos((IntPtr)this.G1000PFDHandle, HWND_TOP, this.G1000PFDPosX, this.G1000PFDPosY, this.G1000PFDPosW, this.G1000PFDPosH, SWP_SHOWWINDOW);
                }
            }, p => { return this.G1000PFDHandle > 0; });

            this.OpenG1000PFDCommand = new RelayCommand(p =>
            {
                if (this.G1000PFDView == null)
                {
                    this.G1000PFDView = new G1000PFDView();
                    this.G1000PFDView.Closed += G1000PFDView_Closed;
                    this.G1000PFDView.Show();
                }
            }, p => { return this.G1000PFDView == null; });

            this.G1000PFDFrameSetPos = new RelayCommand(p =>
            {
                if (this.G1000PFDView != null)
                {
                    this.G1000PFDView.Left = this.G1000PFDFramePosX;
                    this.G1000PFDView.Top = this.G1000PFDFramePosY;
                    this.G1000PFDView.Width = this.G1000PFDFramePosW;
                    this.G1000PFDView.Height = this.G1000PFDFramePosH;
                    //this.G1000PFDView.WindowState = System.Windows.WindowState.Maximized;
                    this.G1000PFDView.Topmost = true;
                    Settings.Default.Save();

                    //SetWindowPos((IntPtr)this.G1000PFDView., HWND_TOP, this.G1000PFDPosX, this.G1000PFDPosY, this.G1000PFDPosW, this.G1000PFDPosH, SWP_SHOWWINDOW);
                }
            }, p => { return this.G1000PFDView != null; });



            // =============================================================================================
            // G1000 MFD 
            // =============================================================================================

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
                //Settings.Default.G1000MFDPosX = this.G1000MFDPosX;
                //Settings.Default.G1000MFDPosY = this.G1000MFDPosY;
                //Settings.Default.G1000MFDPosW = this.G1000MFDPosW;
                //Settings.Default.G1000MFDPosH = this.G1000MFDPosH;
                Settings.Default.Save();

                if (this.G1000MFDHandle != 0)
                {
                    //hwnd = (IntPtr)0x000E0546;
                    //SetWindowPos(hwnd, x, y, w, h, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                    SetWindowPos((IntPtr)this.G1000MFDHandle, HWND_TOP, this.G1000MFDPosX, this.G1000MFDPosY, this.G1000MFDPosW, this.G1000MFDPosH, SWP_SHOWWINDOW);
                }
            }, p => { return this.G1000MFDHandle > 0; });

            this.OpenG1000MFDCommand = new RelayCommand(p =>
            {
                if (this.G1000MFDView == null)
                {
                    this.G1000MFDView = new G1000MFDView();
                    this.G1000MFDView.Closed += G1000MFDView_Closed;
                    this.G1000MFDView.Show();
                }
            }, p => { return this.G1000MFDView == null; });

            this.G1000MFDFrameSetPos = new RelayCommand(p =>
            {
                if (this.G1000MFDView != null)
                {
                    this.G1000MFDView.Left = this.G1000MFDFramePosX;
                    this.G1000MFDView.Top = this.G1000MFDFramePosY;
                    this.G1000MFDView.Width = this.G1000MFDFramePosW;
                    this.G1000MFDView.Height = this.G1000MFDFramePosH;
                    //this.G1000MFDView.WindowState = System.Windows.WindowState.Maximized;
                    this.G1000MFDView.Topmost = true;

                    Settings.Default.Save();

                    //SetWindowPos((IntPtr)this.G1000PFDView., HWND_TOP, this.G1000PFDPosX, this.G1000PFDPosY, this.G1000PFDPosW, this.G1000PFDPosH, SWP_SHOWWINDOW);
                }
            }, p => { return this.G1000MFDView != null; });

            this.FPFDBackgroundIsVisible = Visibility.Visible;
            this.Timer = new DispatcherTimer();
            this.Timer.Tick += Timer_Tick;
            this.Timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (this.FG1000PFDHandle > 0)
            {
                if (IsWindowVisible((IntPtr)this.FG1000PFDHandle))
                {
                    this.PFDBackgroundIsVisible = Visibility.Hidden;
                }
                else
                {
                    this.G1000PFDHandle = 0;
                    this.PFDBackgroundIsVisible = Visibility.Visible;
                }
            }
        }

        private void G1000MFDView_Closed(object sender, EventArgs e)
        {
            if (this.G1000MFDView != null)
            {
                this.G1000MFDView.Closed -= G1000MFDView_Closed;
                this.G1000PFDView = null;
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void G1000PFDView_Closed(object sender, EventArgs e)
        {
            if (this.G1000PFDView != null)
            {
                this.G1000PFDView.Closed -= G1000PFDView_Closed;
                this.G1000PFDView = null;
            }
            CommandManager.InvalidateRequerySuggested();
        }

        public void Init()
        {
            //this.G1000PFDPosX = Settings.Default.G1000PFDPosX;
            //this.G1000PFDPosY = Settings.Default.G1000PFDPosY;
            //this.G1000PFDPosW = Settings.Default.G1000PFDPosW;
            //this.G1000PFDPosH = Settings.Default.G1000PFDPosH;

            //this.G1000PFDFramePosX = Settings.Default.G1000PFDFramePosX;
            //this.G1000PFDFramePosY = Settings.Default.G1000PFDFramePosY;
            //this.G1000PFDFramePosW = Settings.Default.G1000PFDFramePosW;
            //this.G1000PFDFramePosH = Settings.Default.G1000PFDFramePosH;



            //this.G1000MFDPosX = Settings.Default.G1000MFDPosX;
            //this.G1000MFDPosY = Settings.Default.G1000MFDPosY;
            //this.G1000MFDPosW = Settings.Default.G1000MFDPosW;
            //this.G1000MFDPosH = Settings.Default.G1000MFDPosH;

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
                if (value > 0)
                {
                    //this.PFDBackgroundIsVisible = Visibility.Hidden;
                    this.G1000PFDSetPos.Execute(null);

                    if (this.G1000PFDView == null)
                    {
                        this.OpenG1000PFDCommand.Execute(null);
                        this.G1000PFDFrameSetPos.Execute(null);
                    }
                }
                else
                {
                    //this.PFDBackgroundIsVisible = Visibility.Visible;
                }

                this.OnPropertyChanged();
            }
        }



        public int G1000PFDPosX
        {
            get { return Settings.Default.G1000PFDPosX; }
            set
            {
                Settings.Default.G1000PFDPosX = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000PFDPosY
        {
            get { return Settings.Default.G1000PFDPosY; }
            set
            {
                Settings.Default.G1000PFDPosY = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000PFDPosW
        {
            get { return Settings.Default.G1000PFDPosW; }
            set
            {
                Settings.Default.G1000PFDPosW = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000PFDPosH
        {
            get { return Settings.Default.G1000PFDPosH; }
            set
            {
                Settings.Default.G1000PFDPosH = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000PFDFramePosX
        {
            get { return Settings.Default.G1000PFDFramePosX; }
            set
            {
                Settings.Default.G1000PFDFramePosX = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000PFDFramePosY
        {
            get { return Settings.Default.G1000PFDFramePosY; }
            set
            {
                Settings.Default.G1000PFDFramePosY = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000PFDFramePosW
        {
            get { return Settings.Default.G1000PFDFramePosW; }
            set
            {
                Settings.Default.G1000PFDFramePosW = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000PFDFramePosH
        {
            get { return Settings.Default.G1000PFDFramePosH; }
            set
            {
                Settings.Default.G1000PFDFramePosH = value;
                this.OnPropertyChanged();
            }
        }








        // MFD Properties

        private int FG1000MFDHandle;
        public int G1000MFDHandle
        {
            get { return FG1000MFDHandle; }
            set
            {
                FG1000MFDHandle = value;
                if (value > 0)
                {
                    this.G1000MFDSetPos.Execute(null);

                    if (this.G1000MFDView == null)
                    {
                        this.OpenG1000MFDCommand.Execute(null);
                        this.G1000MFDFrameSetPos.Execute(null);
                    }
                }
                this.OnPropertyChanged();
            }
        }




        public int G1000MFDPosX
        {
            get { return Settings.Default.G1000MFDPosX; }
            set
            {
                Settings.Default.G1000MFDPosX = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000MFDPosY
        {
            get { return Settings.Default.G1000MFDPosY; }
            set
            {
                Settings.Default.G1000MFDPosY = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000MFDPosW
        {
            get { return Settings.Default.G1000MFDPosW; }
            set
            {
                Settings.Default.G1000MFDPosW = value;
                this.OnPropertyChanged();
            }
        }



        public int G1000MFDPosH
        {
            get { return Settings.Default.G1000MFDPosH; }
            set
            {
                Settings.Default.G1000MFDPosH = value;
                this.OnPropertyChanged();
            }
        }

        // MFD FRAME

        public int G1000MFDFramePosX
        {
            get { return Settings.Default.G1000MFDFramePosX; }
            set
            {
                Settings.Default.G1000MFDFramePosX = value;
                this.OnPropertyChanged();
            }
        }

        public int G1000MFDFramePosY
        {
            get { return Settings.Default.G1000MFDFramePosY; }
            set
            {
                Settings.Default.G1000MFDFramePosY = value;
                this.OnPropertyChanged();
            }
        }


        public int G1000MFDFramePosW
        {
            get { return Settings.Default.G1000MFDFramePosW; }
            set
            {
                Settings.Default.G1000MFDFramePosW = value;
                this.OnPropertyChanged();
            }
        }

        public int G1000MFDFramePosH
        {
            get { return Settings.Default.G1000MFDFramePosH; }
            set
            {
                Settings.Default.G1000MFDFramePosH = value;
                this.OnPropertyChanged();
            }
        }


        public RelayCommand G1000PFDFindHandle { get; }
        public RelayCommand G1000PFDReadPos { get; }
        public RelayCommand G1000PFDSetPos { get; }
        public RelayCommand OpenG1000PFDCommand { get; }
        public RelayCommand G1000PFDFrameSetPos { get; }
        public RelayCommand G1000MFDFindHandle { get; }
        public RelayCommand G1000MFDSetPos { get; }
        public RelayCommand OpenG1000MFDCommand { get; }
        public RelayCommand G1000MFDFrameSetPos { get; }
        public DispatcherTimer Timer { get; }

        private G1000PFDView FG1000PFDView = null;
        public G1000PFDView G1000PFDView
        {
            get { return this.FG1000PFDView; }
            set
            {
                this.FG1000PFDView = value;
                this.OnPropertyChanged();
            }
        }

        private G1000MFDView FG1000MFDView = null;
        public G1000MFDView G1000MFDView
        {
            get { return this.FG1000MFDView; }
            set
            {
                this.FG1000MFDView = value;
                this.OnPropertyChanged();
            }
        }

        Visibility FPFDBackgroundIsVisible = Visibility.Visible;
        public Visibility PFDBackgroundIsVisible
        {
            get { return this.FPFDBackgroundIsVisible; }
            set
            {
                this.FPFDBackgroundIsVisible = value;
                this.OnPropertyChanged();
            }
        }

        private static HookModes HookMode = HookModes.None;
    }
}
