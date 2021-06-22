using SimPanel.Model;
using SimPanel.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SimPanel.ViewModel
{
    public class SerialDeviceViewModel : BaseViewModel
    {


        public SerialDeviceViewModel() : base()
        {

            this.SerialDevices = new ObservableCollection<ExSerialDevice>();
            this.Connected = false;
            this.LogEnabled = true;
        }


        public void InitPorts()
        {

            foreach (string pname in System.IO.Ports.SerialPort.GetPortNames())
            {
                if (Settings.Default.DisabledComports.Contains(pname) == false)
                {
                    ExSerialDevice sd = this.SerialDevices.Where(k => k.PortName == pname).FirstOrDefault();
                    if (sd == null)
                    {
                        ExSerialDevice sp = new ExSerialDevice(pname, 9600);

                        sp.CallbackProcessHandle += new ExSerialDevice.CallbackProcess(Process);
                        //this.SerialDevices.Add(sp);
                        System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            this.SerialDevices.Add(sp);
                        });
                    }
                }
            }
        }


        private void Process(string port, string data)
        {
            //if (this.SimConnectViewModel != null)
            //{
            //    //ISerialInstrument si = this.SimConnectViewModel.SelectedElement as ISerialInstrument;
            //    si.DataReceived(data);
            //}

            //if (this.SimConnectViewModel.SimConnect != null)
            //{
            //    foreach (var e in Globals.MainWindow.SimConnectViewModel.EventList)
            //    {
            //        if (e.SerialTrigger == data)
            //        {
            //            if (this.SimConnectViewModel.SimConnect != null)
            //            {
            //                this.SimConnectViewModel.SendEvent(e, 0);
            //            }
            //            //e.DateTime = DateTime.Now;
            //        }
            //    }
            //}

            //SimEvent ev = Globals.MainWindow.SimConnectViewModel.EventList.Where(k => k.SerialTrigger == data).FirstOrDefault();
            //if (ev != null)
            //{
            //    if (this.SimConnectViewModel.SimConnect != null)
            //    {
            //        this.SimConnectViewModel.SendEvent(ev, 0);
            //    }
            //}

            Globals.MainWindow.LuaCodeViewModel.Run(data);
            Globals.MainWindow.SimConnectViewModel.Run(data);

            if (this.LogEnabled)
            {
                this.IncommingData += $"{port}:{data}\r\n";
            }

            //if (this.CurrentInstrument != null)
            //{
            //    this.CurrentInstrument.DataReceived(data);
            //}

            //foreach (SerialInstrument inst in this.Instruments)
            //{

            //    // if (inst.CallbackHandler != null && inst.SerialDeviceID == deviceid && inst.SerialEventID == event_id)
            //    //Dispatcher.CurrentDispatcher.Invoke(() =>
            //    //{
            //    inst.CallbackHandler?.Invoke(data);
            //}
        }

        private bool Stopped = false;

        public void Start()
        {
            this.SerialDevices.Clear();

            Task t = new Task(() =>
            {
                int n = 0;
                while (!this.Stopped)
                {
                    bool res = true;
                    for (int i = 0; i < this.SerialDevices.Count; i++)
                    {
                        ExSerialDevice device = this.SerialDevices[i];
                        if (device.IsOpen == false)
                        {

                            try
                            {
                                device.Open();
                                res &= true;
                            }
                            catch (Exception ex)
                            {
                                res &= false;
                                Console.WriteLine(ex.Message);
                                this.SerialDevices.Remove(device);
                                continue;
                            }
                        }

                    }

                    if (MainWindow.App.MainWindowViewModel != null)
                    {
                        //MainWindow.App.MainWindowViewModel.SimConnectViewModel.SerialPortConnected = res;
                        this.Connected = res && this.SerialDevices.Count > 0;
                    }
                    if (n++ % 10 == 0)
                    {
                        this.InitPorts();
                    }

                    Thread.Sleep(1000);
                }

                for (int i = 0; i < this.SerialDevices.Count; i++)
                {
                    ExSerialDevice device = this.SerialDevices[i];
                    if (device.IsOpen)
                    {
                        try
                        {
                            device.Close();
                            Console.WriteLine($"SerialDeviceViewModel.Start(): {device.PortName} Closed-");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"SerialDeviceViewModel.Start(): {ex.Message}");
                        }
                    }
                }
            });
            t.Start();
        }

        public void Stop()
        {
            this.Stopped = true;
        }

        private ObservableCollection<ExSerialDevice> FSerialDevices = null;

        public ObservableCollection<ExSerialDevice> SerialDevices
        {
            get { return FSerialDevices; }
            set
            {
                FSerialDevices = value;
                this.OnPropertyChanged();
            }
        }

        private bool FLogEnabled;

        public bool LogEnabled
        {
            get { return FLogEnabled; }
            set
            {
                FLogEnabled = value;
                this.OnPropertyChanged();
            }
        }

        private string FIncommingData;

        public string IncommingData
        {
            get { return FIncommingData; }
            set
            {
                FIncommingData = value;
                this.OnPropertyChanged();
            }
        }


        private bool FConnected;

        public bool Connected
        {
            get { return FConnected; }
            set
            {
                FConnected = value;
                this.OnPropertyChanged();
            }
        }

        //private ISerialInstrument FCurrentInstrument;

        //public ISerialInstrument CurrentInstrument
        //{
        //    get { return FCurrentInstrument; }
        //    set
        //    {
        //        if (this.FCurrentInstrument != null)
        //        {
        //            //this.FCurrentInstrument.Selected = false;
        //            this.FCurrentInstrument = null;
        //        }

        //        this.FCurrentInstrument = value;

        //        if (this.FCurrentInstrument != null)
        //        {
        //            //this.FCurrentInstrument.Selected = true;
        //        }
        //        this.OnPropertyChanged();
        //    }
        //}


        public SimConnectViewModel SimConnectViewModel
        {
            get { return Globals.MainWindow.SimConnectViewModel; }
        }




    }



    public class ExSerialDevice : SerialPort
    {

        string Line = "";

        public ExSerialDevice(string portname, int baud) : base(portname, baud)
        {
            this.DataReceived += SerialDevice_DataReceived;
            this.ErrorReceived += SerialDevice_ErrorReceived;

        }

        private void SerialDevice_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        public delegate void CallbackProcess(string port, string data);
        public CallbackProcess CallbackProcessHandle = null;

        private void SerialDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                foreach (char c in this.ReadExisting())
                {
                    if (c == (char)10)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.CallbackProcessHandle(this.PortName, this.Line);
                        });
                        //if (this.CallbackProcessHandle != null)
                        //{
                        //    string[] items = this.Line.Split(':');
                        //    if (items.Length == 2)
                        //    {
                        //        string device = items[0];
                        //        string[] data = items[1].Split('=');
                        //    }

                        //}
                        this.Line = "";
                    }
                    else if (c != (char)10 && c != (char)13)
                    {
                        this.Line += c;
                    }
                }
            }
            else
            {
                this.Line = "";
            }
        }


    }


}
