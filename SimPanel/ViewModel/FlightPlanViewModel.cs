using SimPanel.Model;
using SimPanel.Properties;
using SimPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace SimPanel.ViewModel
{
    public class FlightPlanViewModel : BaseViewModel
    {



        string ErrMsg = "";
        string ErrSep = "";


        public EventHandler FlightPlanChanged = null;
        public FlightPlanViewModel() : base()
        {
            this.WaypointList = new ObservableCollection<Waypoint>();
            ClearCommand = new RelayCommand(p =>
            {
                this.WaypointList.Clear();
            });
            //this.LoadFLT(@"C:\Users\junge\AppData\Local\Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\LocalState\MISSIONS\Custom\CustomFlight\CustomFlight.FLT");
             // Make empty
            this.FlightPlanJSON = this.GetJSON();

        }

        public bool LoadFromFile(string filename)
        {
            
            string ext = Path.GetExtension(filename).ToLower();
            if (ext == ".flt")
            {
               if( this.LoadFromFLT(filename))
                {
                    this.FlightPlanJSON = this.GetJSON();
                    if(this.FlightPlanChanged != null)
                    {
                        this.FlightPlanChanged(this, new EventArgs());
                    }

                    return true;
                }

            }
            else if (ext == ".pln")
            {
                //if (this.LoadFromPLN(filename))
                //{
                //    this.FlightPlanJSON = this.GetJSON();
                //    return true;
                //}
            }
            else
            {
                MessageBox.Show($"{ext} is an unknow file extension! ");
            }
            return false;
        }
        private bool LoadFromFLT(string filename)
        {

            //      C: \Users\junge\AppData\Local\Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\LocalState\MISSIONS\Custom\CustomFlight\CustomFlight.FLT
          
            this.LoadDt = DateTime.Now;
            if (File.Exists(filename))
            {               
                try
                {
                    this.WaypointList.Clear();
                    IniFile ini = new IniFile();
                    if(ini.LoadFromFile(filename))
                    {
                        string section = "";
                        if (ini.GetValue("ATC_Aircraft.0", "RequestedFlightPlan", false) == true) {
                            section = "atc_requestedflightplan.0";
                        } else if(ini.GetValue("ATC_Aircraft.0", "ActiveFlightPlan", false) == true)
                        {
                            section = "atc_activeflightplan.0";
                        }

                        if(section != "")
                        {
                            for(int i = 0; i < 100; i++)
                            {
                                string kw = "waypoint." + i.ToString();
                                string l = ini.GetValue(section, kw);
                                if (l != "")
                                {
                                    string[] items = l.Split(',');
                                    if (items.Length == 20)
                                    {

                                        // waypoint.0 =, BIRK, , BIRK,      A, N64° 7.80',  W21° 56.43', +000027.75, , , , , , NONE, 0, 0, -1, 0, 0,  
                                        // waypoint.1 =,     , , TIMECRUIS, U, N59° 4.62',  W1° 3.30',   +007500.00, , , , , , NONE, 0, 0, -1, 0, 0,  
                                        // waypoint.2 =,     , , TIMEDSCNT, U, N55° 33.92', E6° 53.28',  +007500.00, , , , , , NONE, 0, 0, -1, 0, 0,  
                                        // waypoint.3 =, LHTL, , LHTL,      A, N47° 20.73', E18° 58.85', +000350.00, , , , , , NONE, 0, 0, -1, 0, 0,  


                                        Waypoint wp = new Waypoint();
                                        wp.ID = i;
                                        wp.ICAO = items[1];
                                        wp.WaypointDesc = items[3];
                                        wp.WaypointType = items[4];
                                        wp.Coord = CoordModel.Parse(items[5] + "," + items[6]);
                                        wp.Altitude = double.Parse(items[7]);

                                        this.WaypointList.Add(wp);

                                    }
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }


                    //string[] lines = File.ReadAllLines(filename);
                    //string section = "";
                    //foreach (string line in lines)
                    //{
                    //    string l = line.Trim();
                    //    if (l == "")
                    //    {
                    //        section = "";
                    //    }
                    //    else if (l.StartsWith("[") && line.EndsWith("]"))
                    //    {
                    //        section = l.ToLower();
                    //    }

                    //    switch (section)
                    //    {
                    //        case "[departure]":
                    //            break;

                    //        case "[arrival]":
                    //            break;

                    //        case "[atc_aircraft.0]":
                    //            //ActiveFlightPlan=True
                    //            //RequestedFlightPlan=False"

                    //            break;

                    //        case "[atc_requestedflightplan.0]":

                    //            if (l.StartsWith("waypoint."))
                    //            {
                    //                string[] items = l.Split(',');
                    //                if (items.Length == 20)
                    //                {

                    //                    // waypoint.0 =, BIRK, , BIRK,      A, N64° 7.80',  W21° 56.43', +000027.75, , , , , , NONE, 0, 0, -1, 0, 0,  
                    //                    // waypoint.1 =,     , , TIMECRUIS, U, N59° 4.62',  W1° 3.30',   +007500.00, , , , , , NONE, 0, 0, -1, 0, 0,  
                    //                    // waypoint.2 =,     , , TIMEDSCNT, U, N55° 33.92', E6° 53.28',  +007500.00, , , , , , NONE, 0, 0, -1, 0, 0,  
                    //                    // waypoint.3 =, LHTL, , LHTL,      A, N47° 20.73', E18° 58.85', +000350.00, , , , , , NONE, 0, 0, -1, 0, 0,  


                    //                    Waypoint wp = new Waypoint();
                    //                    wp.ID = int.Parse(line.Split('.')[1].Split('=')[0]);
                    //                    wp.ICAO = items[1];
                    //                    wp.WaypointDesc = items[3];
                    //                    wp.WaypointType = items[4];
                    //                    wp.Coord = CoordModel.Parse(items[5] + "," + items[6]);
                    //                    wp.Altitude = double.Parse(items[7]);

                    //                    this.WaypointList.Add(wp);
                    //                }
                    //            }

                    //            break;
                    //    }
                    //}
                    this.FileName = filename;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            return false;
        }
        private bool LoadFromPLN(string filename)
        {
            this.ErrMsg = "";
            this.ErrSep = "";
            this.WaypointList.Clear();
            this.LoadDt = DateTime.Now;
            int id = 0;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                XmlNode root = doc.FirstChild;

                XmlNode fp = doc.SelectSingleNode("SimBase.Document/FlightPlan.FlightPlan");
                if (fp != null)
                {
                    this.Title = GetNodeText(fp, "Title");
                    this.FPType = GetNodeText(fp, "FPType");
                    this.RouteType = GetNodeText(fp, "RouteType");
                    this.CruisingAlt = GetNodeText(fp, "CruisingAlt");
                    this.DepartureID = GetNodeText(fp, "DepartureID");
                    this.DepartureLLA = GetNodeText(fp, "DepartureLLA");
                    this.DestinationID = GetNodeText(fp, "DestinationID");
                    this.DestinationLLA = GetNodeText(fp, "DestinationLLA");
                    this.Description = GetNodeText(fp, "Descr");
                    this.DepartureName = GetNodeText(fp, "DepartureName");
                    this.DestinationName = GetNodeText(fp, "DestinationName");


                    foreach (XmlNode node in fp.ChildNodes)
                    {
                        if (node.Name == "ATCWaypoint")
                        {

                            Waypoint wp = new Waypoint();
                            wp.ID = id++;
                            wp.ICAO = GetAttr(node, "id");
                            wp.WaypointType = GetNodeText(node, "ATCWaypointType");
                            wp.WorldPosition = GetNodeText(node, "WorldPosition");
                            wp.Coord = CoordModel.Parse(GetNodeText(node, "WorldPosition"));

                            XmlNode icao = node.SelectSingleNode("ICAO");
                            //if (icao != null)
                            //{
                            //    wp.ICAOIdent = GetNodeText(icao, "ICAOIdent");
                            //}

                            this.WaypointList.Add(wp);
                        }
                    }
                }

                this.FileName = filename;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

       

        private string GetNodeText(XmlNode node, string key)
        {
            if (node != null && node.SelectSingleNode(key) != null)
            {
                return node.SelectSingleNode(key).InnerText;
            }
            else
            {
                this.ErrMsg += this.ErrSep + $"{key} does not exsist!";
                this.ErrSep = "\r\n";
            }
            return "";
        }

        private string GetAttr(XmlNode node, string attr)
        {
            if (node != null)
            {
                return node.Attributes[attr].Value ?? "";

            }
            else
            {
                this.ErrMsg += this.ErrSep + $"{attr} does not exsist!";
                this.ErrSep = "\r\n";
            }
            return "";
        }

        private string GetJSON()
        {
            string fp = "";
            try
            {
                string res = "";
                string sep = "";
                for (int i = 0; i < 1; i++)
                {
                    foreach (Waypoint wp in this.WaypointList)
                    {
                        res = "";
                        res = JSon.ToString(res, "id", wp.ID);
                        res = JSon.ToString(res, "icao", wp.ICAO);
                        res = JSon.ToString(res, "desc", wp.WaypointDesc);
                        res = JSon.ToString(res, "wptype", wp.WaypointType);
                        res = JSon.ToString(res, "lat", wp.Coord.Lat.Degree);
                        res = JSon.ToString(res, "lng", wp.Coord.Lng.Degree);
                        res = JSon.ToString(res, "alt", wp.Altitude);
                        //res = JSon.ToString(res, "dest", 0);
                        //res = JSon.ToString(res, "brg", 0);
                        fp += sep + "{" + res + "}";
                        sep = ",";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "{\"type\":\"flightplan\", \"timestamp\": " + this.LoadDt.Ticks.ToString() + ", \"data\": [" + fp + "]}";
        }

        private ObservableCollection<Waypoint> FWaypointList;

        public ObservableCollection<Waypoint> WaypointList
        {
            get { return FWaypointList; }
            set
            {
                FWaypointList = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand ClearCommand { get; }

        private Waypoint FSelectedWaypoint;

        public Waypoint SelectedWaypoint
        {
            get { return FSelectedWaypoint; }
            set
            {
                FSelectedWaypoint = value;
                this.OnPropertyChanged();
            }
        }


        public string FileName
        {
            get { return Settings.Default.LastFlightplanFile; }
            set
            {
                Settings.Default.LastFlightplanFile = value;
                Settings.Default.Save();
                this.OnPropertyChanged();
            }
        }


        private string FTitle;

        public string Title
        {
            get { return FTitle; }
            set
            {
                FTitle = value;
                this.OnPropertyChanged();
            }
        }

        private string FFPType;

        public string FPType
        {
            get { return FFPType; }
            set
            {
                FFPType = value;
                this.OnPropertyChanged();
            }
        }

        private string FRouteType;

        public string RouteType
        {
            get { return FRouteType; }
            set
            {
                FRouteType = value;
                this.OnPropertyChanged();
            }
        }

        private string FCruisingAlt;

        public string CruisingAlt
        {
            get { return FCruisingAlt; }
            set
            {
                FCruisingAlt = value;
                this.OnPropertyChanged();
            }
        }

        private string FDepatureID;

        public string DepartureID
        {
            get { return FDepatureID; }
            set
            {
                FDepatureID = value;
                this.OnPropertyChanged();
            }
        }

        private string FDepartureLLA;

        public string DepartureLLA
        {
            get { return FDepartureLLA; }
            set
            {
                FDepartureLLA = value;
                this.OnPropertyChanged();
            }
        }

        private string FDestinationID;

        public string DestinationID
        {
            get { return FDestinationID; }
            set
            {
                FDestinationID = value;
                this.OnPropertyChanged();
            }
        }

        private string FDestinationLLA;

        public string DestinationLLA
        {
            get { return FDestinationLLA; }
            set
            {
                FDestinationLLA = value;
                this.OnPropertyChanged();
            }
        }

        private string FDescription;

        public string Description
        {
            get { return FDescription; }
            set
            {
                FDescription = value;
                this.OnPropertyChanged();
            }
        }

        private string FDepartureName;

        public string DepartureName
        {
            get { return FDepartureName; }
            set
            {
                FDepartureName = value;
                this.OnPropertyChanged();
            }
        }

        private string FDestinationName;

        public string DestinationName
        {
            get { return FDestinationName; }
            set
            {
                FDestinationName = value;
                this.OnPropertyChanged();
            }
        }

        private string FAppVersion;

        public string AppVersion
        {
            get { return FAppVersion; }
            set
            {
                FAppVersion = value;
                this.OnPropertyChanged();
            }
        }

        private DateTime FTimestamFLoadDt;

        public DateTime LoadDt
        {
            get { return FTimestamFLoadDt; }
            set
            {
                FTimestamFLoadDt = value;
                this.OnPropertyChanged();
            }
        }

        public string FlightPlanJSON { get; private set; }
    }

    public class Waypoint : BaseViewModel
    {
        private int FID;

        public int ID
        {
            get { return FID; }
            set
            {
                FID = value;
                this.OnPropertyChanged();
            }
        }

        private string FICAO;

        public string ICAO
        {
            get { return FICAO; }
            set
            {
                FICAO = value;
                this.OnPropertyChanged();
            }
        }


        private string FWaypointDesc;

        public string WaypointDesc
        {
            get { return FWaypointDesc; }
            set
            {
                FWaypointDesc = value;
                this.OnPropertyChanged();
            }
        }


        private string FATCWaypointType;

        public string WaypointType
        {
            get { return FATCWaypointType; }
            set
            {
                FATCWaypointType = value;
                this.OnPropertyChanged();
            }
        }

        private string FWorldPosition;

        public string WorldPosition
        {
            get { return FWorldPosition; }
            set
            {
                FWorldPosition = value;
                this.OnPropertyChanged();
            }
        }

        private CoordModel FCoord;

        public CoordModel Coord
        {
            get { return FCoord; }
            set
            {
                FCoord = value;
                this.OnPropertyChanged();
            }
        }

        private double FAltitude;

        public double Altitude
        {
            get { return FAltitude; }
            set
            {
                FAltitude = value;
                this.OnPropertyChanged();
            }
        }


        //private string FLatitude;

        //public string Latitude
        //{
        //    get { return FLatitude; }
        //    set
        //    {
        //        FLatitude = value;
        //        this.OnPropertyChanged();
        //    }
        //}

        //private string FLongitude;

        //public string Longitude
        //{
        //    get { return FLongitude; }
        //    set
        //    {
        //        FLongitude = value;
        //        this.OnPropertyChanged();
        //    }
        //}

    }
}
