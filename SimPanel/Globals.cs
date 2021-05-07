using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel
{
    public class Globals
    {
        public static MainWindowViewModel MainWindow
        {
            get
            {
                return SimPanel.MainWindow.App.MainWindowViewModel;
            }
        }

        public static string DataDir
        {
            get { return Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data"); }
        }
    }
}
