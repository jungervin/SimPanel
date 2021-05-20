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

        public static string DataDir
        {
            get { return Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data"); }
        }

        static MainWindowViewModel FMainWindow = null;
        public static MainWindowViewModel MainWindow
        {
            get
            {
                return FMainWindow;
            }
            set { FMainWindow = value; }
        }

        public static DatabaseViewModel DatabaseViewModel
        {
            get { return MainWindow.DatabaseViewModel; }
        }
    }
}
