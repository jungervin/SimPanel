using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Model
{
    
    public class SimVarCollection : ObservableCollection<SimVar>, INotifyPropertyChanged
    {
        //private ObservableCollection<SimVar> List = new ObservableCollection<SimVar>();

        public new void Add(SimVar simvar)
        {
            base.Add(simvar);
            this.Modified = true;

        }

        private bool FModified;

        public bool Modified
        {
            get { return FModified; }
            set
            {
                FModified = value;
                this.OnPropertyChanged();
            }
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
    }
}
