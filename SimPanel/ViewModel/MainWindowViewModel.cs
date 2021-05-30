using Microsoft.Win32;
using SimPanel.Model;
using SimPanel.Properties;
using SimPanel.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SimPanel.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            Globals.MainWindow = this;

            AddVariableCommand = new RelayCommand(p => { AddRequest(); });
            RemoveVariableCommand = new RelayCommand(p => { RemoveVariable(); }, p => { return this.SimConnectViewModel != null && this.SimConnectViewModel.SelectedSimVar != null; });
            AddEventCommand = new RelayCommand(p => { AddEvent(); });
            RemoveEventCommand = new RelayCommand(p => { RemoveEvent(); }, p => { return this.SimConnectViewModel != null && this.SimConnectViewModel.SelectedEvent != null; });

            SaveCommand = new RelayCommand(p => Save(), p => { return this.SimConnectViewModel.VarList.Count > 0; });
            OpenCommand = new RelayCommand(p => Open());
            SendEventCommand = new RelayCommand(p => SendEvent(p), p => { return this.FSimConnectViewModel.SelectedEvent != null; });

            SaveSettingsCommand = new RelayCommand(p => Settings.Default.Save());

            OpenNavdataReader = new RelayCommand(p => {
                Process.Start(new ProcessStartInfo(p.ToString()));
            });


            this.SimConnectViewModel = new SimConnectViewModel();
            if (Settings.Default.LastVariableFile != "")
            {
                this.SimConnectViewModel.LoadVariablesFromFile(Settings.Default.LastVariableFile);
            }
            if (Settings.Default.LastEventFile != "")
            {
                this.SimConnectViewModel.LoadEventsFromFile(Settings.Default.LastEventFile);
            }

            this.DatabaseViewModel = new DatabaseViewModel();
            this.DatabaseViewModel.Init();

            this.FlightPlanViewModel = new FlightPlanViewModel();
            if (Settings.Default.LastFlightplanFile != "")
            {
                this.FlightPlanViewModel.LoadFromFile(Settings.Default.LastFlightplanFile);
            }

            this.LuaCodeViewModel = new LuaCodeViewModel();
            if (Settings.Default.LastLuaFile != "")
            {
                this.LuaCodeViewModel.LoadFromFile(Settings.Default.LastLuaFile);
            }

            this.SerialDeviceViewModel = new SerialDeviceViewModel();
            this.SerialDeviceViewModel.Start();

            //Settings.Default.TCPPort = 5001;
            try
            {
                this.SimpleHttpSever = new SimpleHttpServer(Settings.Default.TCPPort);
                this.SimpleHttpSever.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void CloseConnections()
        {
            if (this.SimConnectViewModel != null)
            {
                this.SimConnectViewModel.Stop();
                if (this.SimConnectViewModel.Connected)
                {
                    this.SimConnectViewModel.Disconnect();
                }
            }

            if(this.SerialDeviceViewModel != null)
            {
                this.SerialDeviceViewModel.Stop();
            }

            if(this.SimpleHttpSever != null)
            {
                this.SimpleHttpSever.Stop();
            }

            Thread.Sleep(1000);
        }
        private void RemoveVariable()
        {
            if (MessageBox.Show("You will remove the selected variable!\r\n\r\nAre you sure?", "Waining", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.SimConnectViewModel.RemoveSelectedVariable();
        }

        private void RemoveEvent()
        {
            if (MessageBox.Show("You will remove the selected Event!\r\n\r\nAre you sure?", "Waining", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.SimConnectViewModel.RemoveSelectedEvent();
        }

        private void SendEvent(object p)
        {
            // NAV1_STBY_SET
            //this.SimEventValue = 0x11120;
            this.SimConnectViewModel.SendEvent(this.SimConnectViewModel.SelectedEvent, this.SimEventValue);
        }

        private void Open()
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "Files|*.vars; *.evnts;*.pln;*.lua;*.flt";
            //d.Filter = "Files|*.vars; *.evnts;*.lua;*.flt";
            d.InitialDirectory = Globals.DataDir;
            if (d.ShowDialog() == true)
            {
                string ext = Path.GetExtension(d.FileName).ToLower();
                if (ext == ".vars")
                {
                    if (this.SimConnectViewModel.LoadVariablesFromFile(d.FileName))
                    {
                    }
                }
                else if (ext == ".evnts")
                {
                    if (this.SimConnectViewModel.LoadEventsFromFile(d.FileName))
                    {
                    }
                }
                else if (ext == ".pln" || ext == ".flt")
                {
                    if (this.FlightPlanViewModel.LoadFromFile(d.FileName))
                    {
                        Settings.Default.LastFlightplanFile = d.FileName;
                        Settings.Default.Save();
                    }
                }
                else if (ext == ".lua")
                {
                    if (this.LuaCodeViewModel.LoadFromFile(d.FileName))
                    {
                    }
                }
            }
        }

        private void Save()
        {
            SaveFileDialog d = new SaveFileDialog();

            d.InitialDirectory = Globals.DataDir;
            if (this.SelectedTabIndex == 0)
            {
                d.Filter = "Variables|*.vars";
                d.DefaultExt = ".vars";
                if (d.ShowDialog() == true)
                {
                    this.SimConnectViewModel.SaveVariablesToFile(d.FileName);
                }
            }
            else if (this.SelectedTabIndex == 1)
            {
                d.Filter = "Events|*.evnts";
                d.DefaultExt = ".evnts";
                d.DefaultExt = ".evnts";
                if (d.ShowDialog() == true)
                {
                    this.SimConnectViewModel.SaveEventsToFile(d.FileName);
                }
            }
            else if (this.SelectedTabIndex == 3)
            {
                d.DefaultExt = ".lua";
                d.Filter = "Script|*.lua";
                if (d.ShowDialog() == true)
                {

                    this.LuaCodeViewModel.SaveLuaToFile(d.FileName);
                }
            }
        }

        string LastVarFilter = "";
        private void AddRequest()
        {
            VariableEditorWindow d = new VariableEditorWindow();
            d.ViewModel.Filter = LastVarFilter;
            if (d.ShowDialog() == true)
            {
                this.LastVarFilter = d.Filter.Text;

                SimVar sv = new SimVar();
                sv.VarName = d.ViewModel.SelectedSimVar.VarName;
                if (d.ViewModel.VarIndex > 0)
                {
                    sv.VarName += ":" + d.ViewModel.VarIndex.ToString();
                }
                sv.Unit = d.ViewModel.SelectedSimVar.Unit;
                sv.Writable = d.ViewModel.SelectedSimVar.Writable;
                this.SimConnectViewModel.AddRequest(sv);
            }
        }

        string LastEventFilter = "";
        private void AddEvent()
        {
            EventEditorWindow d = new EventEditorWindow();
            d.ViewModel.Filter = this.LastEventFilter;
            if (d.ShowDialog() == true)
            {
                this.LastEventFilter = d.Filter.Text;
                this.SimConnectViewModel.AddEvent(d.ViewModel.SelectedSimEvent.EventName);
            }
        }

        private SimConnectViewModel FSimConnectViewModel;

        public SimConnectViewModel SimConnectViewModel
        {
            get { return FSimConnectViewModel; }
            set
            {
                FSimConnectViewModel = value;
                this.OnPropertyChanged();
            }
        }

        private FlightPlanViewModel FFlightPlanViewModel;

        public FlightPlanViewModel FlightPlanViewModel
        {
            get { return FFlightPlanViewModel; }
            set
            {
                FFlightPlanViewModel = value;
                this.OnPropertyChanged();
            }
        }

        private SerialDeviceViewModel FSerialDeviceViewModel;

        public SerialDeviceViewModel SerialDeviceViewModel
        {
            get { return FSerialDeviceViewModel; }
            set
            {
                FSerialDeviceViewModel = value;
                this.OnPropertyChanged();
            }
        }


        public LuaCodeViewModel LuaCodeViewModel { get; }

        public DatabaseViewModel DatabaseViewModel { get; set; }


        private uint FSimEventValue;

        public uint SimEventValue
        {
            get { return FSimEventValue; }
            set
            {
                FSimEventValue = value;
                this.OnPropertyChanged();
            }
        }

        private int FSelectedTabIndex;
        private SimpleHttpServer SimpleHttpSever;
        private WSServer WSServer;

        public int SelectedTabIndex
        {
            get { return FSelectedTabIndex; }
            set
            {
                FSelectedTabIndex = value;
                this.OnPropertyChanged();
            }
        }

        public uint SimConnectConfigIndex
        {
            get { return Settings.Default.SimConnectConfigIndex; }
            set
            {
                Settings.Default.SimConnectConfigIndex = value;
                this.OnPropertyChanged();
            }
        }

        public string IgnoredSerialPorts
        {
            get { return Settings.Default.DisabledComports; }
            set
            {
                Settings.Default.DisabledComports = value;
                this.OnPropertyChanged();
            }
        }


        public int ServerPort
        {
            get { return Settings.Default.TCPPort; }
            set
            {
                Settings.Default.TCPPort = value;
                this.OnPropertyChanged();
            }
        }

        public string Database
        {
            get { return Settings.Default.Database; }
            set
            {
                Settings.Default.Database = value;
                this.OnPropertyChanged();
            }
        }
        public RelayCommand AddVariableCommand { get; private set; }
        public RelayCommand RemoveVariableCommand { get; }
        public RelayCommand AddEventCommand { get; }
        public RelayCommand RemoveEventCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand OpenCommand { get; }
        public RelayCommand SendEventCommand { get; }
        public RelayCommand SaveSettingsCommand { get; }
        public RelayCommand OpenNavdataReader { get; }
    }
}
