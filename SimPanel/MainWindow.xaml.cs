
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using SimPanel.Properties;
using SimPanel.View;
using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

            this.MainWindowViewModel.CloseConnections();
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
            System.Diagnostics.Process.Start("http://127.0.0.1:" + Settings.Default.TCPPort.ToString());
        }


       
    }
}
