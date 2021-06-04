using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimPanel.View
{
    /// <summary>
    /// Interaction logic for G530View.xaml
    /// </summary>
    public partial class G530View : Window,  INotifyPropertyChanged
    {
        public G530View()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string _sPropertyName = null)
        {
            PropertyChangedEventHandler hEventHandler = this.PropertyChanged;
            if (hEventHandler != null && !string.IsNullOrEmpty(_sPropertyName))
            {
                hEventHandler(this, new PropertyChangedEventArgs(_sPropertyName));
            }
        }

        private void G530_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
            else if (e.Key == Key.F12)
            {
                this.Topmost = !this.Topmost;
            }
        }

        private void CDI_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_PFD_SOFTKEYS_6", 0);
        }

        private void AS530_OBS_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_OBS_Push", 0);
        }

        private void AS530_MSG_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_MSG_Push", 0);
        }

        private void AS530_FPL_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_FPL_Push", 0);
        }

        private void AS530_VNAV_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_VNAV_Push", 0);
        }

        private void AS530_PROC_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_PROC_Push", 0);
        }

        private void AS530_DirectTo_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_DirectTo_Push", 0);
        }

        private void AS530_MENU_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_MENU_Push", 0);
        }

        private void AS530_CLR_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_CLR_Push", 0);
        }

        private void AS530_ENT_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_ENT_Push", 0);
        }

        private void AS530_COMSWAP_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_COMSWAP_Push", 0);
        }

        private void AS530_NAVSWAP_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_NAVSWAP_Push", 0);
        }

        private void AS530_RNG_Zoom(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_RNG_Zoom", 0);
        }

        private void AS530_RNG_Dezoom(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_RNG_Dezoom", 0);
        }

        private double FCRSInnerAngle;

        public double CRSInnerAngle
        {
            get { return FCRSInnerAngle; }
            set
            {
                FCRSInnerAngle = value;
                this.OnPropertyChanged();
            }
        }

        private double FCRSOuterAngle;

        public double CRSOuterAngle
        {
            get { return FCRSOuterAngle; }
            set
            {
                FCRSOuterAngle = value;
                this.OnPropertyChanged();
            }
        }

        private double FCVInnerAngle;

        public double CVInnerAngle
        {
            get { return FCVInnerAngle; }
            set
            {
                FCVInnerAngle = value;
                this.OnPropertyChanged();
            }
        }

        private double FCVOuterAngle;

        public double CVOuterAngle
        {
            get { return FCVOuterAngle; }
            set
            {
                FCVOuterAngle = value;
                this.OnPropertyChanged();
            }
        }

        private void AS530_RightSmallKnob_Rotate(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.CRSInnerAngle -= 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_RightSmallKnob_Left", 0);
            }
            else
            {
                this.CRSInnerAngle += 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_RightSmallKnob_Right", 0);
            }
        }

        private void AS530_RightLargeKnob_Rotate(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.CRSOuterAngle -= 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_RightLargeKnob_Left", 0);
            }
            else
            {
                this.CRSOuterAngle += 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_RightLargeKnob_Right", 0);
            }
        }

        private void AS530_LeftLargeKnob_Rotate(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.CVOuterAngle -= 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_LeftLargeKnob_Left", 0);
            }
            else
            {
                this.CVOuterAngle += 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_LeftLargeKnob_Right", 0);
            }
        }

        private void AS530_LeftSmallKnob_Rotate(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.CVInnerAngle -= 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_LeftSmallKnob_Left", 0);
            }
            else
            {
                this.CVInnerAngle += 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_LeftSmallKnob_Right", 0);
            }
        }

        private void AS530_LeftSmallKnob_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_LeftSmallKnob_Push", 0);
        }

        private void AS530_RightSmallKnob_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS530_RightSmallKnob_Push", 0);
        }
    }
}
