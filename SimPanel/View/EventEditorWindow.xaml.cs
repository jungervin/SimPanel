using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for EventEditorWindow.xaml
    /// </summary>
    public partial class EventEditorWindow : Window
    {
        public EventEditorViewModel ViewModel = new EventEditorViewModel();
        public EventEditorWindow()
        {
            InitializeComponent();
            this.Owner = MainWindow.App;
            this.DataContext = this.ViewModel;

        }
    }
}
