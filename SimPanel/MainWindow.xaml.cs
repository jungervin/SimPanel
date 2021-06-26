
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SimPanel.Properties;
using SimPanel.Utility;
using SimPanel.View;
using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimPanel
{
    interface IBaseSimConnectWrapper
    {
        int GetUserSimConnectWinEvent();
        void ReceiveSimConnectMessage();
        void SetWindowHandle(IntPtr _hWnd);
        void Disconnect();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow App = null;
        public MainWindowViewModel MainWindowViewModel = null; // new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            App = this;
            this.Title = "SimPanel Server v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.MainWindowViewModel = new MainWindowViewModel();
            this.DataContext = this.MainWindowViewModel;

            //this.Topmost = true;
            
        }
        protected HwndSource GetHWinSource()
        {
            return PresentationSource.FromVisual(this) as HwndSource;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            GetHWinSource().AddHook(WndProc);
            if (this.MainWindowViewModel.SimConnectViewModel is IBaseSimConnectWrapper oBaseSimConnectWrapper)
            {
                oBaseSimConnectWrapper.SetWindowHandle(GetHWinSource().Handle);
            }

            //WinManViewModel.SetStyle(new WindowInteropHelper(this).Handle);

        }

        private IntPtr WndProc(IntPtr hWnd, int iMsg, IntPtr hWParam, IntPtr hLParam, ref bool bHandled)
        {
            if (this.MainWindowViewModel.SimConnectViewModel is IBaseSimConnectWrapper oBaseSimConnectWrapper)
            {
                try
                {
                    if (iMsg == oBaseSimConnectWrapper.GetUserSimConnectWinEvent())
                    {
                        oBaseSimConnectWrapper.ReceiveSimConnectMessage();
                    }
                }
                catch
                {
                    oBaseSimConnectWrapper.Disconnect();
                }
            }

            return IntPtr.Zero;
        }


        private void LinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            string sText = e.Text;
            foreach (char c in sText)
            {
                if (!(('0' <= c && c <= '9') || c == '+' || c == '-' || c == ','))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string msg = "";
            string sep = "";
            if (this.MainWindowViewModel.SimConnectViewModel.VarListModified)
            {
                msg += sep + "The Variable List has been modified!";
                sep = "\r\n";
            }

            if (this.MainWindowViewModel.SimConnectViewModel.EventListModified)
            {
                msg += sep + "The EventList has been modified!";
                sep = "\r\n";
            }

            if (this.MainWindowViewModel.LuaCodeViewModel.CodeModified)
            {
                msg += sep + "The Lua Script has been modified!";
                sep = "\r\n";
            }

            if (msg != "")
            {
                msg = msg + "\r\n\r\nAre you sure you wat to quit?";

                if (MessageBox.Show(msg, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                }
            }

            //this.MainWindowViewModel.CloseConnections();
            Environment.Exit(0);

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SelectDB_Clck(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "SQLite|*.sqlite";
            if (d.ShowDialog() == true)
            {
                this.MainWindowViewModel.Database = d.FileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string hostName = Dns.GetHostName();
            //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            //String strHostName = string.Empty;
            //IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress[] addr = ipEntry.AddressList;
            //Globals.MainWindow.SimpleHttpSever.ToString();

            System.Diagnostics.Process.Start("http://127.0.0.1:" + Settings.Default.TCPPort.ToString());
        }

        //private void G1000PFD_Click(object sender, RoutedEventArgs e)
        //{
        //    SimPanel.View.G1000PFDView pfd = new SimPanel.View.G1000PFDView();
        //    pfd.Show();
        //}

        //private void G1000MFD_Click(object sender, RoutedEventArgs e)
        //{
        //    SimPanel.View.G1000MFDView mfd = new SimPanel.View.G1000MFDView();
        //    mfd.Show();
        //}

        private void G530_Click(object sender, RoutedEventArgs e)
        {
            G530View g = new G530View();
            g.Show();
        }

        private void Win_Click(object sender, RoutedEventArgs e)
        {
            //Process[] p = Process.GetProcesses();
            //Process fs = null;
            //foreach(var pr in p)
            //{
            //    Console.WriteLine(pr.ProcessName);
            //    if(pr.ProcessName == "FlightSimulator")
            //    {
            //        Console.WriteLine(pr.Handle.ToString("X"));
            //        IntPtr child2 = Helpers.FindWindowEx2(pr.Handle, IntPtr.Zero, "AceApp", "$AS1000_PFD_1");
            //    }
            //}
            // return;

            IntPtr parent = IntPtr.Zero;
            foreach (var p in Process.GetProcesses())
            {
                if (p.ProcessName.Contains("Simu"))
                {
                    Console.WriteLine(p.ProcessName);
                    parent = p.Handle;
                }
            }
            
            //IntPtr parent = Helpers.FindWindowA("Microsoft Flight Simulator - 1.16.2.0");
            //IntPtr parent = Helpers.FindWindowA("Microsoft Flight Simulator");
            //Console.WriteLine(parent.ToString("X"));

            WindowHandleInfo hi = new WindowHandleInfo(parent);
            var list = hi.GetAllChildHandles();
            foreach (var w in list)
            {

                Console.WriteLine($"{w.ToString()},  {w.ToString("X")}");

                Rect size = hi.GetPos(w);
                //{ 319,479,1024,1024}
                //hi.SetPos(w, 0, 0, 600, 400);
                if (size.Left == 319 && size.Width == 1024)
                {
                    //hi.SetPos((IntPtr)w, 400, 400, 800, 800);
                }
            }
        }
    }
}
