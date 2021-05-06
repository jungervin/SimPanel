using SimDebugger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimDebugger.ViewModel
{
    public class SerialportViewModel : BaseViewModel
    {
        public SerialportViewModel()
        {



            this.SerialEvents = new ObservableCollection<SerialEvent>();
            this.LoadFromFile("./Data/SerialEvents.ser");

            SaveCommand = new RelayCommand(p => {
                this.SaveToFile("./Data/SerialEvents.ser");
            }, p => { return true; });
        }

        private ObservableCollection<SerialEvent> FSerialEvents;

        //public SerialDeviceViewModel SerialDeviceViewModel { 
        //    get; ; }

        private SerialDeviceViewModel FSerialDevicemodel;

        public SerialDeviceViewModel SerialDeviceViewModel
        {
            get { return FSerialDevicemodel; }
            set { FSerialDevicemodel = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<SerialEvent> SerialEvents
        {
            get { return FSerialEvents; }
            set
            {
                FSerialEvents = value;
                this.OnPropertyChanged();
            }
        }

        private SerialEvent FSelectedSeriaEvent;

        public SerialEvent SelectedSerialEvent
        {
            get { return FSelectedSeriaEvent; }
            set
            {
                FSelectedSeriaEvent = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; }

        public bool SaveToFile(string fname)
        {
            string lines = "";
            string sep = "";
            foreach (SerialEvent se in this.SerialEvents)
            {
                lines += sep + $"{se.SimEventName},{se.Trigger}";
                sep = "\r\n";
            }

            try
            {
                File.WriteAllText(fname, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public bool LoadFromFile(string fname)
        {
            try
            {
                string[] lines = File.ReadAllLines(fname);
                this.SerialEvents.Clear();
                foreach (string line in lines)
                {
                    if (line.Trim() != "")
                    {
                        string[] items = line.Split(',');
                        if (items.Length >= 2)
                        {
                            SerialEvent se = new SerialEvent()
                            {
                                SimEventName = items[0].Trim(),
                                Trigger = items[1].Trim()
                            };
                            this.SerialEvents.Add(se);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }
    }
}
