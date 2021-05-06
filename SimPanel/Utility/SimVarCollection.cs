using SimDebugger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimDebugger.Utility
{
    public class SimVarCollection : ObservableCollection<SimVar>
    {

        public new void Add(SimVar simvar)
        {
            base.Add(simvar);
            this.Modified = true;
        
        }

        private bool FModified;

        public bool Modified
        {
            get { return FModified; }
            set { FModified = value; }
        }



    }
}
