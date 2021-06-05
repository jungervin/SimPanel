using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimPanel.Utility
{
    public class Helpers
    {
        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

        public static IntPtr FindWindow(string caption)
        {
            return FindWindow("AceApp", caption);
        }

        public static IntPtr FindWindowA(string caption)
        {
            return FindWindowA("AceApp", caption);
        }

        public static IntPtr FindWindowEx2(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow)
        {
            return FindWindowEx(hwndParent, hwndChildAfter, "AceApp", lpszWindow);
        }


    }
    /// <summary>
    /// Gets the handle to the horizontal scroll bar.
    /// </summary>
    /// <param name="parentControl">The parent window of the scrollbar.</param>
    /// <returns>Handle to the scrollbar window.</returns>
    //private IntPtr GetHandleToHorizontalScrollBar(Control parent)
    //{
    //    // Locals
    //    IntPtr childHandle;
    //    string appDomainHexedHash;

    //    // Get the hexadecimal value of AppDomain hash code.
    //    // This value is dynamically appended to the window class name of the child window
    //    // for .NET Windows Forms.  This name is viewable via the Spy++ tool.
    //    appDomainHexedHash = AppDomain.CurrentDomain.GetHashCode().ToString("x");

    //    // Find window handle
    //    childHandle = FindWindowEx(
    //        parent.Handle,    // Parent handle
    //        IntPtr.Zero,    // Child window after which to seek
    //        "WindowsForms10.SCROLLBAR.app.0." + appDomainHexedHash, // Class name to seek (viewable in the Spy++ tool)
    //        IntPtr.Zero);    // Window title to seek

    //    // Return handle
    //    return childHandle;
    //}

    //private string GetUrlFromIE()
    //{
    //    IntPtr windowHandle = GetForegroundWindow();
    //    IntPtr childHandle;
    //    String strUrlToReturn = "";

    //    //try to get a handle to IE's toolbar container
    //    childHandle = FindWindowEx(windowHandle, IntPtr.Zero, "WorkerW", IntPtr.Zero);
    //    if (childHandle != IntPtr.Zero)
    //    {
    //        //get a handle to address bar
    //        childHandle = FindWindowEx(childHandle, IntPtr.Zero, "ReBarWindow32", IntPtr.Zero);
    //        if (childHandle != IntPtr.Zero)
    //        {
    //            // get a handle to combo boxex
    //            childHandle = FindWindowEx(childHandle, IntPtr.Zero, "ComboBoxEx32", IntPtr.Zero);
    //            if (childHandle != IntPtr.Zero)
    //            {
    //                // get a handle to combo box
    //                childHandle = FindWindowEx(childHandle, IntPtr.Zero, "ComboBox", IntPtr.Zero);
    //                if (childHandle != IntPtr.Zero)
    //                {
    //                    //get handle to edit
    //                    childHandle = FindWindowEx(childHandle, IntPtr.Zero, "Edit", IntPtr.Zero);
    //                    if (childHandle != IntPtr.Zero)
    //                    {
    //                        strUrlToReturn = GetText(childHandle);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return strUrlToReturn;
    //}


    public class WindowHandleInfo
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



        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        public void SetPos(IntPtr hwnd, int x, int y, int w, int h)
        {
            if (hwnd != IntPtr.Zero)
            {
                //hwnd = (IntPtr)0x000E0546;
                //SetWindowPos(hwnd, x, y, w, h, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                SetWindowPos(hwnd, HWND_TOP, x, y, w, h, SWP_NOSIZE | SWP_SHOWWINDOW);
            }
        }


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public Rect GetPos(IntPtr hwnd)
        {
            RECT rect = new RECT();
            GetWindowRect(hwnd, ref rect);

            Rect r = new Rect(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            return r;

        }
    }
}
