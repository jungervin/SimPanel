using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Model
{
    public class SimEvent : ObservableObject
    {

        public enum DEFINITIONS
        {
            Dummy
        }

        private int FId;

        public int Id
        {
            get { return FId; }
            set
            {
                FId = 10000 + value;
                this.OnPropertyChanged();
            }
        }

        private string FEventName;

        public string EventName
        {
            get { return FEventName; }
            set
            {
                FEventName = value;
                this.OnPropertyChanged();
            }
        }

        private bool FSubscribed;

        public bool Subscribed
        {
            get { return FSubscribed; }
            set
            {
                FSubscribed = value;
                this.OnPropertyChanged();
            }
        }

        private string FSerialTrigger = "";

        public string SerialTrigger
        {
            get { return FSerialTrigger; }
            set { FSerialTrigger = value;
                this.OnPropertyChanged();
            }
        }

    }
}
