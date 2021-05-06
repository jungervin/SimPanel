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
    /// Interaction logic for VariableEditorWindow.xaml
    /// </summary>
    public partial class VariableEditorWindow : Window
    {
        public VariableEditorViewModel ViewModel = new VariableEditorViewModel();
        public VariableEditorWindow()
        {
            InitializeComponent();
            this.Owner = MainWindow.App;
            this.DataContext = this.ViewModel;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
    }
}
