using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// Interaction logic for G1000MFDView.xaml
    /// </summary>
    public partial class G1000MFDView : Window
    {


        public G1000MFDView()
        {
            InitializeComponent();
            this.DataContext = this;
            this.SimConnectViewModel = Globals.MainWindow.SimConnectViewModel;


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

        private void G1000_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left)
            //    this.DragMove();
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

        private SimConnectViewModel FSimConnectViewModel;

        public SimConnectViewModel SimConnectViewModel
        {
            get { return FSimConnectViewModel; }
            set
            {
                FSimConnectViewModel = value;
                this.OnPropertyChanged();
            }
        }

    }
}
