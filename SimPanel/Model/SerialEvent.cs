using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Model
{
    public class SerialEvent : BaseViewModel
    {
        private string FTrigger;

        public string Trigger
        {
            get { return FTrigger; }
            set
            {
                FTrigger = value;
                this.OnPropertyChanged();
            }
        }

        private string FSimEventName;

        public string SimEventName
        {
            get { return FSimEventName; }
            set
            {
                FSimEventName = value;
                this.OnPropertyChanged();
            }
        }

        
    }
}
