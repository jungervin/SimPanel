
using Microsoft.FlightSimulator.SimConnect;
using SimPanel.Model;
using SimPanel.Properties;
using SimPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static SimPanel.Model.SimVar;

namespace SimPanel.ViewModel
{
    public class SimConnectViewModel : BaseViewModel, IBaseSimConnectWrapper
    {

        public enum DEFINITIONS
        {
            AIRPLANE = 1000,
            TRANSPONDER,
        }

        public enum REQUESTS
        {
            AIRPLANE = 1000,
            TRANSPONDER,
            SUBSCRIBE_REQ,
            NONSUBSCRIBE_REQ
        }

        public enum SYSEVENTS
        {
            FileLoaded = 2000,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct C172Struct
        {
            public SIMCONNECT_DATA_XYZ AIRCRAFT_ORIENTAION_XYZ;
        }


        public struct SimDataString256
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string value;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SimDataDouble
        {
            public double value;
        }

        public const int WM_USER_SIMCONNECT = 0x0402;
        private IntPtr FHWnd = new IntPtr(0);
        private bool FStopped = true;

        private DispatcherTimer Timer = new DispatcherTimer();
        public SimConnectViewModel() : base()
        {
            this.VarList = new SimVarCollection();
            this.VarList.CollectionChanged += VarList_CollectionChanged;

            //            this.EventList = new ObservableCollection<SimEvent>(Utility.StaticLists.SimConnectEvents);
            this.EventList = new ObservableCollection<SimEvent>();
            this.EventList.CollectionChanged += EventList_CollectionChanged;

            SendEventCommand = new RelayCommand(p => SendEvent(p));
            SetCamCommand = new RelayCommand(p =>
            {

                //CameraSetRelative6DOF(float fDeltaX, float fDeltaY, float fDeltaZ, float fPitchDeg, float fBankDeg, float fHeadingDeg);
                this.SimConnect.CameraSetRelative6DOF(10, 10, 10, 10, 10, 01);
            });


            //this.AddRequest("AIRSPEED TRUE", "knots");
            //this.LoadVariables(@".\Data\variables.txt");

            Timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            Timer.Tick += new EventHandler(OnTick);





            this.Start();





        }

        private void EventList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.EventListModified = true;
        }

        private void VarList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.VarListModified = true;
        }

        private void SendEvent(object p)
        {
            if (this.SimConnect != null)
            {
                //if (ev < EVENTS.KEY_NONE)
                //{
                //    this.SimConnect.TransmitClientEvent(0, ev, 1, SIMCONNECT_NOTIFICATION_GROUP_ID.SIMCONNECT_GROUP_PRIORITY_DEFAULT, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                //}
                //else
                SimEvent ev = p as SimEvent;
                {
                    this.SimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, (SimEvent.DEFINITIONS)ev.Id, 0, (SimEvent.DEFINITIONS)0, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
            }
        }

        internal void RemoveSelectedVariable()
        {
            if (this.SelectedSimVar != null)
            {
                this.VarList.Remove(this.SelectedSimVar);
                this.Disconnect();
            }
        }
        internal void RemoveSelectedEvent()
        {
            if (this.SelectedEvent != null)
            {
                this.EventList.Remove(this.SelectedEvent);
                this.Disconnect();
            }
        }

        public void SendEvent(SimEvent ev, uint data)
        {
            if (this.SimConnect != null)
            {
                //if (ev < EVENTS.KEY_NONE)
                //{
                //    this.SimConnect.TransmitClientEvent(0, ev, 1, SIMCONNECT_NOTIFICATION_GROUP_ID.SIMCONNECT_GROUP_PRIORITY_DEFAULT, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                //}
                //else
                //m_simConnect.m_simConnect.TransmitClientEvent(0U, m_events[name], value, (Enum)NOTIFICATION_GROUPS.GENERIC, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);

                try
                {
                    this.SimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, (SimEvent.DEFINITIONS)ev.Id, data, (SimEvent.DEFINITIONS)0, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public void SendEvent(string cmd, uint data)
        {
            SimEvent ev = this.EventList.Where(k => k.EventName == cmd).FirstOrDefault();


            if (ev != null)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    this.SendEvent(ev, data);
                }));
                Thread.Sleep(10);
            }
            else
            {

                ev = this.EventList.Where(k => k.EventName == cmd).FirstOrDefault();

                if (ev == null)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(() =>
                    {
                        ev = new SimEvent() { EventName = cmd, Id = this.EventList.Count + 1 };
                        this.Map(ev);
                        this.EventList.Add(ev);
                        Thread.Sleep(50);
                        this.SendEvent(ev, 0);
                    }));
                }

            }

            //if (this.SimConnect != null)
            //{
            //    //if (ev < EVENTS.KEY_NONE)
            //    //{
            //    //    this.SimConnect.TransmitClientEvent(0, ev, 1, SIMCONNECT_NOTIFICATION_GROUP_ID.SIMCONNECT_GROUP_PRIORITY_DEFAULT, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            //    //}
            //    //else

            //    {
            //        this.SimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, (SimEvent.DEFINITIONS)ev.Id, data, (SimEvent.DEFINITIONS)0, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            //    }
            //}
        }


        public void Start()
        {

            this.FStopped = false;
            Task t = new Task(() =>
            {
                while (!FStopped)
                {
                    if (!this.Connected)
                    {
                        this.Connect();
                    }                  
                    Thread.Sleep(3000);
                }
            });
            t.Start();
        }

        public void Stop()
        {
            this.FStopped = true;
        }


        // PZS

        //private void initDataRequest()
        //{
        //    try
        //    {
        //        my_simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(simconnect_OnRecvOpen);
        //        my_simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(simconnect_OnRecvQuit);
        //        my_simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(simconnect_OnRecvException);
        //        my_simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(simconnect_OnRecvSimobjectDataBytype);
        //        my_simconnect.OnRecvEvent += new SimConnect.RecvEventEventHandler(simconnect_OnRecvEventID);


        //        //*************EVENTS cuccok****************************
        //        //magassági
        //        my_simconnect.MapClientEventToSimEvent(EVENTS.EVENT_YAXIS, "AXIS_ELEVATOR_SET");
        //        my_simconnect.AddClientEventToNotificationGroup(GROUPS.GROUP_1, EVENTS.EVENT_YAXIS, true);
        //        my_simconnect.SetNotificationGroupPriority(GROUPS.GROUP_1, SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST_MASKABLE);
        //        my_simconnect.MapInputEventToClientEvent(GROUPS.GROUP_1, "joystick:0:YAxis", EVENTS.EVENT_YAXIS, 0, EVENTS.EVENT_YAXIS, SimConnect.SIMCONNECT_UNUSED, true);
        //        my_simconnect.SetInputGroupState(GROUPS.GROUP_1, 0);

        //        //csűrő
        //        my_simconnect.MapClientEventToSimEvent(EVENTS.EVENT_XAXIS, "AXIS_AILERONS_SET");
        //        my_simconnect.AddClientEventToNotificationGroup(GROUPS.GROUP_1, EVENTS.EVENT_XAXIS, true);
        //        my_simconnect.SetNotificationGroupPriority(GROUPS.GROUP_1, SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST_MASKABLE);
        //        my_simconnect.MapInputEventToClientEvent(GROUPS.GROUP_1, "joystick:0:XAxis", EVENTS.EVENT_XAXIS, 0, EVENTS.EVENT_XAXIS, SimConnect.SIMCONNECT_UNUSED, true);
        //        my_simconnect.SetInputGroupState(GROUPS.GROUP_1, 0);
        //    }
        //    catch (COMException exception1)
        //    {
        //        this.Text = exception1.Message;
        //    }
        //}

        //private void simconnect_OnRecvEventID(SimConnect sender, SIMCONNECT_RECV_EVENT data)
        //{
        //    switch (data.uEventID)
        //    {
        //        case (uint)EVENTS.EVENT_SIMSTART:

        //            break;

        //        // Magassági kormány
        //        case (uint)EVENTS.EVENT_YAXIS:
        //            int kormany_elev = (int)data.dwData;
        //            double kormanypos_elev = kormany_elev;
        //            kormanypos_elev = Math.Pow(kormanypos_elev / 16384, 2) * 16384 * Math.Sign(kormany_elev) * 1;  //már lövésem sincs itt mit matekoltam

        //            // ITT megmatekozod a magassági kormányjelet, majd visszaküldöd az FS-nek

        //            my_simconnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.EVENT_YAXIS, (uint)kormanypos_elev, GROUPS.GROUP_1, 0);
        //            break;

        //        // Csűrőkormány
        //        case (uint)EVENTS.EVENT_XAXIS:
        //            int kormany_ail = (int)data.dwData;
        //            double kormanypos_ail = kormany_ail;
        //            kormanypos_ail = Math.Pow(kormanypos_ail / 16384, 2) * 16384 * Math.Sign(kormany_ail) * 1;

        //            // ITT megmatekozod a csűrő kormányjelet, majd visszaküldöd az FS-nek

        //            my_simconnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.EVENT_XAXIS, (uint)kormanypos_ail, GROUPS.GROUP_1, 0);
        //            break;

        //        default:
        //            this.Text = "Unknown request ID: " + ((uint)data.uEventID);
        //            break;
        //    }
        //}


        private void Connect()
        {
            this.Ready = false;
            Console.WriteLine("Connect");

            try
            {
                bool bFSXcompatible = false;

                this.SimConnect = new SimConnect("SimPanel", this.FHWnd, WM_USER_SIMCONNECT, null, Settings.Default.SimConnectConfigIndex);
                this.SimConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                this.SimConnect.OnRecvQuit += SimConnect_OnRecvQuit;
                this.SimConnect.OnRecvException += SimConnect_OnRecvException;

                this.SimConnect.OnRecvSimobjectData += new SimConnect.RecvSimobjectDataEventHandler(SimConnect_OnRecvSimobjectData);
                this.SimConnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);


                ////https://forums.flightsimulator.com/t/demo-lvar-write-access-for-any-aircraft-control/353443
                //this.SimConnect.OnRecvClientData += new SimConnect.RecvClientDataEventHandler(SimConnect_OnRecvClientData);
                //this.SimConnect.OnRecvEventFilename += SimConnect_OnRecvEventFilename;


                //SimConnect.MapClientDataNameToID("BRIDGE_WriteToSim", ClientData.WriteToSim);
                //SimConnect.MapClientDataNameToID("BRIDGE_ReadFromSim", ClientData.ReadFromSim);
                //SimConnect.AddToClientDataDefinition(ClientData.WriteToSim, 0, (uint)Marshal.SizeOf(typeof(WriteToSim)), 0, 0);
                //SimConnect.AddToClientDataDefinition(ClientData.ReadFromSim, 0, (uint)Marshal.SizeOf(typeof(ReadFromSim)), 0, 0);

                //SimConnect.RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, ReadFromSim>(ClientData.ReadFromSim);
                //SimConnect.RequestClientData(
                //    ClientData.ReadFromSim,
                //    DATA_REQUESTS.ReadFromSimChanged,
                //    ClientData.ReadFromSim, SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET, SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
                //    0, 0, 0);


                //new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);
                //this.SimConnect.OnRecvSimobjectDataBytype += SimConnect_OnRecvSimobjectDataBytype; // new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);

                //RequestDataOnSimObject(Enum RequestID, Enum DefineID, uint ObjectID, SIMCONNECT_PERIOD Period, SIMCONNECT_DATA_REQUEST_FLAG Flags, uint origin, uint interval, uint limit);

                //this.SimConnect.RequestDataOnSimObject(REQUESTS.AIRPLANE, DEFINITIONS.AIRPLANE, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.SECOND, 0, 0, 0, 0);

                //this.SimConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                //this.SimConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);
                //this.SimConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);





                //this.SimConnect.OnRecvCloudState += SimConnect_OnRecvCloudState1;
                //this.SimConnect.SubscribeToFacilities(SIMCONNECT_FACILITY_LIST_TYPE.AIRPORT, REQUESTS.SUBSCRIBE_REQ);
                //this.SimConnect.SubscribeToFacilities(SIMCONNECT_FACILITY_LIST_TYPE.WAYPOINT, REQUESTS.SUBSCRIBE_REQ);
                //this.SimConnect.SubscribeToFacilities(SIMCONNECT_FACILITY_LIST_TYPE.NDB, REQUESTS.SUBSCRIBE_REQ);
                //this.SimConnect.SubscribeToFacilities(SIMCONNECT_FACILITY_LIST_TYPE.VOR, REQUESTS.SUBSCRIBE_REQ);
                //this.SimConnect.SubscribeToFacilities(SIMCONNECT_FACILITY_LIST_TYPE.COUNT, REQUESTS.SUBSCRIBE_REQ);


                //this.SimConnect.OnRecvAirportList += SimConnect_OnRecvAirportList;
                //this.SimConnect.OnRecvVorList += SimConnect_OnRecvVorList;
                //this.SimConnect.OnRecvNdbList += SimConnect_OnRecvNdbList;
                //this.SimConnect.OnRecvWaypointList += SimConnect_OnRecvWaypointList;
                //this.SimConnect.OnRecvWeatherObservation += SimConnect_OnRecvWeatherObservation;
                //this.SimConnect.OnRecvCloudState += SimConnect_OnRecvCloudState;


            }
            catch (COMException ex)
            {
                Console.WriteLine("Connection to KH failed: " + ex.Message);
            }
        }

        private void SimConnect_OnRecvCloudState(SimConnect sender, SIMCONNECT_RECV_CLOUD_STATE data)
        {
            throw new NotImplementedException();
        }

        private void SimConnect_OnRecvWaypointList(SimConnect sender, SIMCONNECT_RECV_WAYPOINT_LIST data)
        {
            //throw new NotImplementedException();
            Console.WriteLine("WAYPOINTS");
        }

        private void SimConnect_OnRecvNdbList(SimConnect sender, SIMCONNECT_RECV_NDB_LIST data)
        {
            //throw new NotImplementedException();
            Console.WriteLine("NDBLIST");
        }

        private void SimConnect_OnRecvVorList(SimConnect sender, SIMCONNECT_RECV_VOR_LIST data)
        {
            //throw new NotImplementedException();
            Console.WriteLine("VORLIST");
        }

        private void SimConnect_OnRecvWeatherObservation(SimConnect sender, SIMCONNECT_RECV_WEATHER_OBSERVATION data)
        {
            throw new NotImplementedException();
        }

        private void SimConnect_OnRecvAirportList(SimConnect sender, SIMCONNECT_RECV_AIRPORT_LIST data)
        {
            List<SIMCONNECT_DATA_FACILITY_AIRPORT> list = new List<SIMCONNECT_DATA_FACILITY_AIRPORT>();
            foreach (SIMCONNECT_DATA_FACILITY_AIRPORT a in data.rgData)
            {

                SIMCONNECT_DATA_FACILITY_AIRPORT airport = new SIMCONNECT_DATA_FACILITY_AIRPORT();
                list.Add(a);
                Console.WriteLine(a.Icao);
            }

            // throw new NotImplementedException();
            Console.WriteLine("AIRPORTS");
        }

        private void SimConnect_OnRecvEventFilename(SimConnect sender, SIMCONNECT_RECV_EVENT_FILENAME data)
        {
            if (data.uEventID == (int)SYSEVENTS.FileLoaded && Globals.MainWindow.FlightPlanViewModel != null)
            {
                Globals.MainWindow.FlightPlanViewModel.LoadFromFile(data.szFileName);
            }
        }

        private void SimConnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
        }

        protected virtual void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            if (data.dwRequestID == (uint)REQUESTS.AIRPLANE)
            {
                //this.AirPlaneData = (C172Struct)data.dwData[0];
                SIMCONNECT_DATA_XYZ AIRCRAFT_ORIENTATION_AXIS = (SIMCONNECT_DATA_XYZ)data.dwData[0];
                Console.WriteLine("{0}, {1}, {2}", AIRCRAFT_ORIENTATION_AXIS.x, AIRCRAFT_ORIENTATION_AXIS.y, AIRCRAFT_ORIENTATION_AXIS.z);
            }


        }


        private object simlock;
        protected virtual void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            uint iRequest = data.dwRequestID;
            uint iObject = data.dwObjectID;

            //lock (simlock)
            {
                foreach (SimVar oSimvarRequest in this.VarList)
                {
                    if (iRequest == (uint)oSimvarRequest.DefID)
                    {
                        object d = data.dwData[0];
                        switch (oSimvarRequest.DataType)
                        {
                            case SIMCONNECT_DATATYPE.STRING128:
                                oSimvarRequest.Value = ((STRING128)data.dwData[0]).Value;
                                break;

                            case SIMCONNECT_DATATYPE.FLOAT64:
                                oSimvarRequest.Value = (double)data.dwData[0];
                                break;

                            case SIMCONNECT_DATATYPE.XYZ:
                                //oSimvarRequest.Value = (double)data.dwData[0];
                                SIMCONNECT_DATA_XYZ AIRCRAFT_ORIENTATION_AXIS = (SIMCONNECT_DATA_XYZ)data.dwData[0];
                                Console.WriteLine("{0}, {1}, {2}", AIRCRAFT_ORIENTATION_AXIS.x, AIRCRAFT_ORIENTATION_AXIS.y, AIRCRAFT_ORIENTATION_AXIS.z);
                                break;
                        }
                    }
                }
                this.Ready = true;
            }
        }
        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
            Console.WriteLine("SimConnect_OnRecvException: " + eException.ToString());
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            foreach (SimVar sv in this.VarList)
            {
                sv.Subscribed = false;
                sv.Value = null;
            }

            foreach (SimEvent se in this.EventList)
            {
                se.Subscribed = false;
            }


            this.Timer.Stop();
            this.Connected = false;
            if (WSServer != null)
            {
                this.WSServer.Stop();
            }
            this.Ready = false;

        }

        protected virtual void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            this.ClientRequestID = 0;



            this.SimConnect.SubscribeToSystemEvent(SYSEVENTS.FileLoaded, "FlightLoaded");

            this.Ready = false;
            Console.WriteLine("OnRecOpen");

            foreach (SimVar svar in this.VarList)
            {
                if (!svar.Subscribed)
                {
                    this.RegisterToSimConnect(svar);
                    svar.Subscribed = true;
                }
                else
                {

                }

            }

            foreach (SimEvent se in this.EventList)
            {
                if (!se.Subscribed)
                {
                    this.Map(se);
                    this.SimConnect.AddClientEventToNotificationGroup(SimEvent.SimEnum.group1 , (SimEvent.DEFINITIONS)se.Id , false);
                    se.Subscribed = true;
                }
            }
            this.Connected = true;
            this.Timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            foreach (SimVar sv in this.VarList)
            {
                if (sv.Subscribed)
                {
                    try
                    {
                        if (sv.VarName.StartsWith("L:"))
                        {
                            //Console.WriteLine("SKIP:" + oSimvarRequest.VarName);
                        }
                        else
                        {
                            SimConnect?.RequestDataOnSimObjectType((SIMREQUEST)sv.DefID, (SIMREQUEST)sv.DefID, 0, sv.SimObjectType);
                        }

                        sv.Subscribed = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                }
            }
        }


        public void Disconnect()
        {

            if (this.SimConnect != null)
            {
                this.Timer.Stop();
                this.SimConnect.Dispose();
                this.SimConnect = null;
            }

            foreach (SimVar sv in this.VarList)
            {
                sv.Subscribed = false;
            }

            foreach (SimEvent se in this.EventList)
            {
                se.Subscribed = false;
            }

            this.Connected = false;
            this.Ready = false;

        }

        public void AddRequest(string varname, string varunit, string writable = "")
        {
            SimVar svar = this.VarList.Where(k => k.VarName == varname && k.Unit == varunit).FirstOrDefault();
            if (svar == null)
            {
                svar = new SimVar()
                {
                    VarName = varname,
                    Unit = varunit,
                    //Value = 0.0,
                    Subscribed = false,
                    DefID = this.VarList.Count() + 1,
                    SimObjectType = SIMCONNECT_SIMOBJECT_TYPE.USER,
                    Writable = writable
                };
                svar.Subscribed = this.RegisterToSimConnect(svar);
                this.VarList.Add(svar);

                //this.VarList.

            }
            else
            {
                MessageBox.Show($"Variable already exists!\r\n\r\n{varname}");
            }
        }

        internal void AddRequest(SimVar selectedSimVar)
        {
            this.AddRequest(selectedSimVar.VarName, selectedSimVar.Unit, selectedSimVar.Writable);
        }

        public static byte[] AllocString(string txt)
        {
            return GetArrayOfSize(Encoding.ASCII.GetBytes(txt), 256);
        }

        static byte[] GetArrayOfSize(byte[] input, int size)
        {
            var ret = new byte[size];
            Array.Copy(input, ret, input.Length);
            return ret;
        }
        private bool RegisterToSimConnect(SimVar svar)
        {
            if (this.SimConnect != null)
            {
                if (svar.VarName.StartsWith("L:"))
                {
                    //svar.ClientRequestID = this.ClientRequestID++;
                    //var vdata = new WriteToSim();
                    //vdata.isSet = 0;
                    //vdata.name = WriteToSim.AllocString(svar.VarName.Remove(0, 2));
                    //vdata.index = svar.ClientRequestID;//  m_localVarNames.IndexOf(op.name);
                    //svar.DataType = SIMCONNECT_DATATYPE.FLOAT64;
                    //svar.Unit = "Number";
                    //this.SimConnect.SetClientData(ClientData.WriteToSim, ClientData.WriteToSim, SIMCONNECT_CLIENT_DATA_SET_FLAG.DEFAULT, 0, vdata);

                    return true;
                }
                /// Define a data structure
                if (svar.Unit.ToLower().Contains("string"))
                {
                    this.SimConnect.AddToDataDefinition((SIMDEFINITION)svar.DefID, svar.VarName, null, SIMCONNECT_DATATYPE.STRING128, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    svar.DataType = SIMCONNECT_DATATYPE.STRING128;
                    /// IMPORTANT: Register it with the simconnect managed wrapper marshaller
                    /// If you skip this step, you will only receive a uint in the .dwData field.
                    this.SimConnect.RegisterDataDefineStruct<STRING128>((SIMDEFINITION)svar.DefID);
                }
                else if (svar.Unit.ToLower() == "xyz")
                {

                    // this.SimConnect.AddToDataDefinition(DEFINITIONS.AIRPLANE, "AIRCRAFT_ORIENTATION_AXIS", "XYZ", SIMCONNECT_DATATYPE.XYZ, 0f, SimConnect.SIMCONNECT_UNUSED);
                    // this.SimConnect.RequestDataOnSimObject(REQUESTS.AIRPLANE, DEFINITIONS.AIRPLANE, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.VISUAL_FRAME, 0, 0, 0, 0);
                    ////this.SimConnect.AddToDataDefinition(DEFINITIONS.AIRPLANE, "AIRCRAFT_ORIENTATION_AXIS", "XYZ", SIMCONNECT_DATATYPE.XYZ, 0f, SimConnect.SIMCONNECT_UNUSED);

                    ////this.SimConnect.AddToDataDefinition((SIMDEFINITION)svar.DefID, "AIRCRAFT_ORIENTATION_AXIS", "XYZ", SIMCONNECT_DATATYPE.XYZ, 0f, SimConnect.SIMCONNECT_UNUSED);
                    //this.SimConnect.AddToDataDefinition((SIMDEFINITION)svar.DefID, "AIRCRAFT_ORIENTATION_AXIS", "XYZ", SIMCONNECT_DATATYPE.XYZ, 0f, SimConnect.SIMCONNECT_UNUSED);
                    ////this.SimConnect.AddToDataDefinition((SIMDEFINITION)svar.DefID, svar.VarName.Replace(" ", "_"), null, SIMCONNECT_DATATYPE.XYZ, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    //svar.DataType = SIMCONNECT_DATATYPE.XYZ;
                    //this.SimConnect.RegisterDataDefineStruct<SIMCONNECT_DATA_XYZ>((SIMDEFINITION)svar.DefID);
                }
                else
                {

                    this.SimConnect.AddToDataDefinition((SIMDEFINITION)svar.DefID, svar.VarName, svar.Unit, SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                    svar.DataType = SIMCONNECT_DATATYPE.FLOAT64;
                    /// IMPORTANT: Register it with the simconnect managed wrapper marshaller
                    /// If you skip this step, you will only receive a uint in the .dwData field.
                    /// 
                    this.SimConnect.RegisterDataDefineStruct<double>((SIMDEFINITION)svar.DefID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddEvent(string eventname)
        {

            SimEvent ev = this.EventList.Where(k => k.EventName == eventname).FirstOrDefault();

            if (ev == null)
            {
                ev = new SimEvent()
                {
                    Id = this.EventList.Count + 1,
                    EventName = eventname
                };

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    this.EventList.Add(ev);
                }));

                if (this.Connected)
                {
                    this.Map(ev);
                }

            }
            else
            {
                MessageBox.Show($"Event already exists!\r\n\r\n{eventname}");
            }

        }
        public void Map(SimEvent se)
        {
            if (SimConnect != null)
            {

                this.SimConnect.MapClientEventToSimEvent((SimEvent.DEFINITIONS)se.Id, se.EventName);
            }
        }

        private int Counter = 0;
        public string GetVariablesJSON()
        {
            string res = "";
            DateTime dt = DateTime.Now;
            res = JSon.ToString(res, "PacketID", (Int32)this.Counter);
            this.Counter++;
            foreach (var item in this.VarList)
            {
                if (item.Value != null)
                {
                    if (item.Value is string)
                    {
                        res = JSon.ToString(res, item.ID, item.Value.ToString());
                    }
                    else if (item.Value is Int32)
                    {
                        res = JSon.ToString(res, item.ID, (Int32)item.Value);
                    }
                    else
                    {
                        res = JSon.ToString(res, item.ID, (Double)item.Value);
                    }
                }
                else
                {
                    if (item.Unit.ToLower() == "string")
                    {
                        res = JSon.ToString(res, item.ID, "0");
                    }
                    else
                    {
                        res = JSon.ToString(res, item.ID, 0.0);
                    }
                }
            }
            return "{\"type\":\"variables\", \"data\": {" + res + "}}";
        }

        public bool LoadVariablesFromFile(string filename)
        {

            if (File.Exists(filename))
            {
                this.VarList.Clear();
                string[] lines = File.ReadAllLines(filename);
                foreach (string l in lines)
                {
                    string[] items = l.Split(',');
                    //this.AddRequest(items[0], items[1], items[2]);
                    SimVar svar = new SimVar()
                    {
                        VarName = items[0],
                        Unit = items[1],
                        Writable = items[2]
                    };
                    this.AddRequest(svar);
                }
                this.VariablesFileName = filename;
                this.VarListModified = false;
                return true;
            }
            else
            {
                MessageBox.Show($"The '{filename}' does not exists!");
            }
            return false;
        }

        public bool LoadEventsFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                this.EventList.Clear();
                string[] lines = File.ReadAllLines(filename);
                foreach (string li in lines)
                {
                    string l = li.Trim();
                    if (!l.Contains("/") && l != "")
                    {
                        string[] items = l.Split(',');
                        if (items.Length >= 1)
                        {
                            //this.AddRequest(items[0], items[1], items[2]);
                            SimEvent se = new SimEvent()
                            {
                                Id = this.EventList.Count + 1,
                                EventName = items[0],
                                //SerialTrigger = items[1]
                                //Writable = items[2]
                            };
                            this.EventList.Add(se);
                        }
                    }
                }
                this.EventListModified = false;
                this.EventsFileName = filename;
                return true;
            }
            else
            {
                MessageBox.Show($"The {filename} does not exists!");
            }
            return false;
        }

        public void SaveVariablesToFile(string filename)
        {
            string text = "";
            string sep = "";
            foreach (SimVar v in this.VarList)
            {
                text += sep + $"{v.VarName},{v.Unit},{v.Writable}";
                sep = "\r\n";
            }

            File.WriteAllText(filename, text);
            this.VarListModified = false;
            this.VariablesFileName = filename;

        }


        public void SaveEventsToFile(string filename)
        {
            string text = "";
            string sep = "";
            foreach (SimEvent v in this.EventList)
            {
                //text += sep + $"{v.EventName},{v.SerialTrigger}";
                text += sep + $"{v.EventName}";
                sep = "\r\n";
            }

            File.WriteAllText(filename, text);
            this.EventListModified = false;
            this.EventsFileName = filename;
        }

        public void LoadVariables(string filename)
        {
            if (File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);
                foreach (string l in lines)
                {
                    string[] items = l.Split(',');
                    this.AddRequest(items[0], items[1], items[2]);

                }
                this.VariablesFileName = filename;
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

        private SimConnect FSimConnect;

        public SimConnect SimConnect
        {
            get { return FSimConnect; }
            set
            {
                FSimConnect = value;
                this.OnPropertyChanged();
            }
        }


        private SimVarCollection FVarList;

        public SimVarCollection VarList
        {
            get { return FVarList; }
            set
            {
                FVarList = value;
                this.OnPropertyChanged();
            }
        }

        private SimVar FSelectedSimVar;


        public SimVar SelectedSimVar
        {
            get { return FSelectedSimVar; }
            set
            {
                FSelectedSimVar = value;
                this.OnPropertyChanged();
            }
        }

        private bool FVarListModified;

        public bool VarListModified
        {
            get { return FVarListModified; }
            set
            {
                FVarListModified = value;
                this.OnPropertyChanged();
            }
        }


        public string VariablesFileName
        {
            get { return Settings.Default.LastVariableFile; }
            set
            {
                Settings.Default.LastVariableFile = value;
                Settings.Default.Save();
                this.OnPropertyChanged();
            }
        }


        private ObservableCollection<SimEvent> FEventList;

        public ObservableCollection<SimEvent> EventList
        {
            get { return FEventList; }
            set
            {
                FEventList = value;
                this.OnPropertyChanged();
            }
        }

        private SimEvent FSelectedEvent;
        public SimEvent SelectedEvent
        {
            get { return FSelectedEvent; }
            set
            {
                FSelectedEvent = value;
                this.OnPropertyChanged();
            }
        }

        private bool FEventListModified;

        public bool EventListModified
        {
            get { return FEventListModified; }
            set
            {
                FEventListModified = value;
                this.OnPropertyChanged();
            }
        }

        public string EventsFileName
        {
            get { return Settings.Default.LastEventFile; }
            set
            {
                Settings.Default.LastEventFile = value;
                Settings.Default.Save();
                this.OnPropertyChanged();
            }
        }

        public RelayCommand SendEventCommand { get; }
        public RelayCommand SetCamCommand { get; }


        internal bool SerialPortConnected;
        public bool Ready = false;

        private int FClientRequestID;
        private int Counte = 0;

        public int ClientRequestID
        {
            get { return FClientRequestID; }
            set { FClientRequestID = value; }
        }



        internal WSServer WSServer { get; set; }

        public int GetUserSimConnectWinEvent()
        {
            return WM_USER_SIMCONNECT;
        }

        public void ReceiveSimConnectMessage()
        {
            this.SimConnect?.ReceiveMessage();
        }

        public void SetWindowHandle(IntPtr _hWnd)
        {
            this.FHWnd = _hWnd;
        }

    }
}

