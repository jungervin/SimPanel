using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
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

    }
}
