using Newtonsoft.Json;
using SimPanel.Model;
using SimPanel.Properties;
using SimPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SimPanel.ViewModel
{
    class WSServer : WebSocketBehavior
    {
        public WSServer()
        {
            this.EmitOnPing = true;
            this.DealWithDataBase = File.Exists(Settings.Default.Database);

        }

        private DateTime LastFlightPlanDt = DateTime.Now;
        private bool DealWithDataBase;

        protected override void OnMessage(MessageEventArgs e)
        {
            string res = "";

            if (this.MainWindow != null && this.MainWindow.SimConnectViewModel != null && this.MainWindow.FlightPlanViewModel != null)
            {
                //if (Globals.MainWindow.FlightPlanViewModel.LoadDt != this.LastFlightPlanDt)
                //{
                //    this.Send(Globals.MainWindow.FlightPlanViewModel.FlightPlanJSON);
                //    LastFlightPlanDt = Globals.MainWindow.FlightPlanViewModel.LoadDt;
                //}

                if (e.Data == "getposition")
                {
                    var lat = this.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "PLANE LATITUDE").FirstOrDefault();
                    var lng = this.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "PLANE LONGITUDE").FirstOrDefault();
                    var hdg = this.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "PLANE HEADING DEGREES TRUE").FirstOrDefault();
                    if (lat != null && lng != null && hdg != null)
                    {

                        res = JSon.ToString(res, "PLANE_LATITUDE", lat.Value != null ? (double)lat.Value : 47.434387018741525);
                        res = JSon.ToString(res, "PLANE_LONGITUDE", lng.Value != null ? (double)lng.Value : 19.264183044433597);
                        res = JSon.ToString(res, "PLANE_HEADING_DEGREES_TRUE", hdg.Value != null ? (double)hdg.Value : 0);
                        this.Send("{\"type\":\"position\", \"data\": {" + res + "}}");
                    }
                    else
                    {
                        this.Send("{\"type\":\"status\", \"data\": \"No valid position!\"}");
                    }
                    return;
                }
                else if (this.DealWithDataBase)
                {

                    if (e.Data.StartsWith("{\"cmd\":"))
                    {
                        QueryModel q = JsonConvert.DeserializeObject<QueryModel>(e.Data);
                        if (q.cmd == "getairports")
                        {

                            MapDetailsModel d = JsonConvert.DeserializeObject<MapDetailsModel>(q.data.ToString());


                            AirPortInfo ai = new AirPortInfo();
                            ai.AirPorts = Globals.DatabaseViewModel.SelectAirPorts(d);
                            res = JsonConvert.SerializeObject(ai);
                            this.Send(res);
                            return;
                        }
                        else if (q.cmd == "getparkings")
                        {
                            MapDetailsModel d = JsonConvert.DeserializeObject<MapDetailsModel>(q.data.ToString());

                            ResponseParkings rap = new ResponseParkings();
                            rap.type = "parkings";
                            rap.parkings = Globals.DatabaseViewModel.SelectParkings(d);
                            res = JsonConvert.SerializeObject(rap);
                            this.Send(res);
                            return;
                        }
                        else if (q.cmd == "getairportinfobyname")
                        {
                            //DataModel d = JsonConvert.DeserializeObject<DataModel>(q.data.ToString());

                            AirPortInfo ai = Globals.MainWindow.DatabaseViewModel.GetAiportInfo(q.data.ToString());

                            res = JsonConvert.SerializeObject(ai);
                            this.Send(res);
                            return;
                        }
                        this.Send("{\"type\":\"status\", \"data\": \"Unknown cmd\"}");
                    }
                    else
                    {
                        this.Send("{\"type\":\"status\", \"data\": \"Unknown query\"}");
                    }
                }
                else
                {
                    this.Send("{\"type\":\"status\", \"data\": \"No database\"}");
                }
            }
            else
            {
                this.Send("{\"type\":\"status\", \"data\": \"Not Connected\"}");
            }

        }

        //public List<Parking> SelectParkings(MapDetailsModel d)
        //{
        //    List<Parking> list = MainWindow.DatabaseViewModel.Parkings.Where(k =>
        //        k.laty >= d.bounds._southWest.lat &&
        //        k.laty <= d.bounds._northEast.lat &&
        //        k.lonx >= d.bounds._southWest.lng &&
        //        k.lonx <= d.bounds._northEast.lng
        //    ).ToList();

        //    return list;
        //    //var db = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data", Settings.Default.Database);
        //    //var cs = $"Data Source = {db}; Cache = Shared";
        //    //using (var conn = new SQLiteConnection(cs))
        //    //{
        //    //    conn.Open();
        //    //    DataTable dt = new DataTable();
        //    //    SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM PARKING WHERE laty >= @LAT_MIN and laty <= @LAT_MAX and lonx >= @LNG_MIN and lonx <= @LNG_MAX ", conn);
        //    //    //SQLiteCommand cmd = new SQLiteCommand("select * from parking", conn);
        //    //    cmd.Parameters.AddWithValue("@LAT_MIN", d.bounds._southWest.lat);
        //    //    cmd.Parameters.AddWithValue("@LAT_MAX", d.bounds._northEast.lat);
        //    //    cmd.Parameters.AddWithValue("@LNG_MIN", d.bounds._southWest.lng);
        //    //    cmd.Parameters.AddWithValue("@LNG_MAX", d.bounds._northEast.lng);


        //    //    SQLiteDataReader reader = cmd.ExecuteReader();
        //    //    dt.Load(reader);
        //    //    return dt;
        //    //}
        //    //return null;
        //}

        //public DataTable SelectParkings2(MapDetailsModel d)
        //{
        //    var db = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data", Settings.Default.Database);
        //    var cs = $"Data Source = {db}; Cache = Shared";
        //    using (var conn = new SQLiteConnection(cs))
        //    {
        //        conn.Open();
        //        DataTable dt = new DataTable();
        //        SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM PARKING WHERE laty >= @LAT_MIN and laty <= @LAT_MAX and lonx >= @LNG_MIN and lonx <= @LNG_MAX ", conn);
        //        //SQLiteCommand cmd = new SQLiteCommand("select * from parking", conn);
        //        cmd.Parameters.AddWithValue("@LAT_MIN", d.bounds._southWest.lat);
        //        cmd.Parameters.AddWithValue("@LAT_MAX", d.bounds._northEast.lat);
        //        cmd.Parameters.AddWithValue("@LNG_MIN", d.bounds._southWest.lng);
        //        cmd.Parameters.AddWithValue("@LNG_MAX", d.bounds._northEast.lng);


        //        SQLiteDataReader reader = cmd.ExecuteReader();
        //        dt.Load(reader);
        //        return dt;
        //    }
        //    return null;
        //}


        //public List<AirPort> SelectAirPorts(MapDetailsModel d)
        //{
        //    return MainWindow.DatabaseViewModel.Airports.Where(k =>
        //        //k.ident == "LHBP" &&
        //        k.rating >= d.rating &&
        //        k.laty >= d.bounds._southWest.lat &&
        //        k.laty <= d.bounds._northEast.lat &&
        //        k.lonx >= d.bounds._southWest.lng &&
        //        k.lonx <= d.bounds._northEast.lng

        //    ).ToList();
        //}

        public DataTable SelectRunways2(MapDetailsModel d)
        {
            var db = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data", Settings.Default.Database);
            var cs = $"Data Source = {db}; Cache = Shared";
            using (var conn = new SQLiteConnection(cs))
            {
                conn.Open();
                DataTable dt = new DataTable();
                SQLiteCommand cmd = new SQLiteCommand(@"
SELECT 
    A.IDENT, A.NAME, A.CITY,A.FUEL_FLAGS,A.TOWER_FREQUENCY, A.ATIS_FREQUENCY,
    R.RUNWAY_ID,R.HEADING,R.LONX, R.LATY, R.ALTITUDE
FROM AIRPORT A
LEFT JOIN RUNWAY R ON A.AIRPORT_ID = R.AIRPORT_ID
WHERE A.LATY >= @LAT_MIN AND A.LATY <= @LAT_MAX AND A.LONX >= @LNG_MIN AND A.LONX <= @LNG_MAX AND A.RATING >= @RATING
LIMIT 1000"

, conn);
                //SQLiteCommand cmd = new SQLiteCommand("select * from parking", conn);
                cmd.Parameters.AddWithValue("@LAT_MIN", d.bounds._southWest.lat);
                cmd.Parameters.AddWithValue("@LAT_MAX", d.bounds._northEast.lat);
                cmd.Parameters.AddWithValue("@LNG_MIN", d.bounds._southWest.lng);
                cmd.Parameters.AddWithValue("@LNG_MAX", d.bounds._northEast.lng);
                cmd.Parameters.AddWithValue("@RATING", d.rating);


                SQLiteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                return dt;
            }
            return null;
        }




        public string ToCsv(DataTable dt)
        {
            string res = "";
            string sep = "";
            int n = 0;
            foreach (DataRow r in dt.Rows)
            {
                res += n++ > 0 ? ";" : "";
                sep = "";
                foreach (object o in r.ItemArray)
                {
                    res += sep + o.ToString();
                    sep = ",";
                }

            }

            return res;
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            this.Send(Globals.MainWindow.FlightPlanViewModel.FlightPlanJSON);
            //Globals.MainWindow.FlightPlanViewModel.FlightPlanChanged += FlightPlaneChanged;
        }

        private void FlightPlaneChanged(object sender, EventArgs e)
        {
            //this.Send(Globals.MainWindow.FlightPlanViewModel.FlightPlanJSON);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            //Globals.MainWindow.FlightPlanViewModel.FlightPlanChanged -= FlightPlaneChanged;
            base.OnClose(e);
        }

        public void Stop()
        {
            if (this.WebSocketServer != null)
            {
                this.WebSocketServer.Stop();
            }
        }
        public MainWindowViewModel MainWindow
        {
            get
            {
                return SimPanel.MainWindow.App.MainWindowViewModel;
            }
        }


        //private string _name;
        //private static int _number = 0;
        //private string _prefix;
        //private bool FStopped;



        public WebSocketServer WebSocketServer { get; private set; }
    }
}
