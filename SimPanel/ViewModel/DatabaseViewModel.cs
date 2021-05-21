
using Newtonsoft.Json;
using SimPanel.Model;
using SimPanel.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.ViewModel
{
    public class DatabaseViewModel
    {


        public DatabaseViewModel()
        {
            //string fn = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data", Settings.Default.Database);
            if (File.Exists(Settings.Default.Database))
            {
                this.ConnectioString = $"Data Source = {Settings.Default.Database}; Cache = Shared";
            }

        }

        public void Init()
        {
            this.Airports = this.FillAirports();
            this.Runways = this.FillRunways();
            this.RunwayEnds = this.FillRunwayEnds();
            this.Parkings = this.FillParking();
            this.ILSs = this.FillILSs();

            AirPortsInfo ai = this.GetAiportInfo("LHBP");

            string res = JsonConvert.SerializeObject(ai);
            Console.WriteLine(res);

        }

        public List<AirPort> SelectAirPorts(MapDetailsModel d)
        {
            return this.Airports.Where(k =>
                k.rating >= d.rating &&
                k.laty >= d.bounds._southWest.lat &&
                k.laty <= d.bounds._northEast.lat &&
                k.lonx >= d.bounds._southWest.lng &&
                k.lonx <= d.bounds._northEast.lng

            ).ToList();
        }

        public AirPort SelectAirPorts(string ident)
        {
            return this.Airports.Where(k => k.ident == ident).FirstOrDefault();
        }


        public List<Parking> SelectParkings(MapDetailsModel d)
        {
            return this.Parkings.Where(k =>
                 k.laty >= d.bounds._southWest.lat &&
                 k.laty <= d.bounds._northEast.lat &&
                 k.lonx >= d.bounds._southWest.lng &&
                 k.lonx <= d.bounds._northEast.lng
             ).ToList();
        }
        public AirPortsInfo GetAiportInfo(string ident)
        {
            AirPortsInfo ai = new AirPortsInfo();
            //ai.AirPort = this.Airports.Where(k => k.ident == ident).FirstOrDefault();
            //ai.Runways = new List<RunwayInfo>();

            //List<Runway> runways = this.Runways.Where(k => k.airport_id == ai.AirPort.airport_id).ToList();
            //foreach (Runway rw in runways)
            //{
            //    RunwayInfo ri = new RunwayInfo();
            //    ri.Runway = rw;

            //    ri.PrimaryEnd = this.RunwayEnds.Where(k => k.runway_end_id == rw.primary_end_id).FirstOrDefault();
            //    ri.SecondaryEnd = this.RunwayEnds.Where(k => k.runway_end_id == rw.secondary_end_id).FirstOrDefault();
            //    ai.Runways.Add(ri);
            //}
            return ai;
        }

        public string ConnectioString { get; set; }

        public List<AirPort> Airports { get; set; }
        public List<Runway> Runways { get; set; }
        public List<RunwayEnd> RunwayEnds { get; set; }
        public List<Parking> Parkings { get; set; }
        public List<ILS> ILSs { get; set; }

        public List<AirPort> FillAirports()
        {
            List<AirPort> list = new List<AirPort>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM AIRPORT", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            AirPort a = new AirPort();
                            a.airport_id = r.GetInt64(0);
                            a.file_id = r.GetInt64(1);
                            a.ident = r.GetString(2);
                            a.icao = r.GetValue(3) != DBNull.Value ? r.GetString(3) : "";
                            a.iata = r.GetValue(4) != DBNull.Value ? r.GetString(4) : "";
                            a.xpident = r.GetValue(5) != DBNull.Value ? r.GetString(5) : "";
                            a.name = r.GetValue(6) != DBNull.Value ? r.GetString(6) : "";
                            a.city = r.GetValue(7) != DBNull.Value ? r.GetString(7) : "";
                            a.state = r.GetValue(8) != DBNull.Value ? r.GetString(8) : "";
                            a.country = r.GetValue(9) != DBNull.Value ? r.GetString(9) : "";
                            a.region = r.GetValue(10) != DBNull.Value ? r.GetString(10) : "";
                            a.flatten = r.GetValue(11) != DBNull.Value ? r.GetInt64(11) : 0;
                            a.fuel_flags = r.GetInt64(12);
                            a.has_avgas = r.GetInt64(13);
                            a.has_jetfuel = r.GetInt64(14);
                            a.has_tower_object = r.GetInt64(15);
                            a.tower_frequency = r.GetValue(16) != DBNull.Value ? r.GetInt64(16) : 0;
                            a.atis_frequency = r.GetValue(17) != DBNull.Value ? r.GetInt64(17) : 0;
                            a.awos_frequency = r.GetValue(18) != DBNull.Value ? r.GetInt64(18) : 0;
                            a.asos_frequency = r.GetValue(19) != DBNull.Value ? r.GetInt64(19) : 0;
                            a.unicom_frequency = r.GetValue(20) != DBNull.Value ? r.GetInt64(20) : 0;
                            a.is_closed = r.GetInt64(21);
                            a.is_military = r.GetInt64(22);
                            a.is_addon = r.GetInt64(23);
                            a.num_com = r.GetInt64(24);
                            a.num_parking_gate = r.GetInt64(25);
                            a.num_parking_ga_ramp = r.GetInt64(26);
                            a.num_parking_cargo = r.GetInt64(27);
                            a.num_parking_mil_cargo = r.GetInt64(28);
                            a.num_parking_mil_combat = r.GetInt64(29);
                            a.num_approach = r.GetInt64(30);
                            a.num_runway_hard = r.GetInt64(31);
                            a.num_runway_soft = r.GetInt64(32);
                            a.num_runway_water = r.GetInt64(33);
                            a.num_runway_light = r.GetInt64(34);
                            a.num_runway_end_closed = r.GetInt64(35);
                            a.num_runway_end_vasi = r.GetInt64(36);
                            a.num_runway_end_als = r.GetInt64(37);
                            a.num_runway_end_ils = r.GetValue(38) != DBNull.Value ? r.GetInt64(38) : 0;
                            a.num_apron = r.GetInt64(39);
                            a.num_taxi_path = r.GetInt64(40);
                            a.num_helipad = r.GetInt64(41);
                            a.num_jetway = r.GetInt64(42);
                            a.num_starts = r.GetInt64(43);
                            a.longest_runway_length = r.GetInt64(44);
                            a.longest_runway_width = r.GetInt64(45);
                            a.longest_runway_heading = r.GetDouble(46);
                            a.longest_runway_surface = r.GetValue(47) != DBNull.Value ? r.GetString(47) : "";
                            a.num_runways = r.GetInt64(48);
                            a.largest_parking_ramp = r.GetValue(49) != DBNull.Value ? r.GetString(49) : "";
                            a.largest_parking_gate = r.GetValue(50) != DBNull.Value ? r.GetString(50) : "";
                            a.rating = r.GetInt64(51);
                            a.is_3d = r.GetInt64(52);
                            a.scenery_local_path = r.GetString(53);
                            a.bgl_filename = r.GetString(54);
                            a.left_lonx = r.GetDouble(55);
                            a.top_laty = r.GetDouble(56);
                            a.right_lonx = r.GetDouble(57);
                            a.bottom_laty = r.GetDouble(58);
                            a.mag_var = r.GetDouble(59);
                            a.tower_altitude = r.GetValue(60) != DBNull.Value ? r.GetInt64(60) : 0;
                            a.tower_lonx = r.GetValue(61) != DBNull.Value ? r.GetDouble(61) : 0;
                            a.tower_laty = r.GetValue(62) != DBNull.Value ? r.GetDouble(62) : 0;
                            a.transition_altitude = r.GetValue(63) != DBNull.Value ? r.GetInt64(63) : 0;
                            a.altitude = r.GetInt64(64);
                            a.lonx = r.GetDouble(65);
                            a.laty = r.GetDouble(66);

                            list.Add(a);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return list;
        }

        public List<Runway> FillRunways()
        {

            List<Runway> list = new List<Runway>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM RUNWAY", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Runway rw = new Runway();

                            rw.runway_id = r.GetValue(0) != DBNull.Value ? r.GetInt64(0) : 0;
                            rw.airport_id = r.GetValue(1) != DBNull.Value ? r.GetInt64(1) : 0;
                            rw.primary_end_id = r.GetValue(2) != DBNull.Value ? r.GetInt64(2) : 0;
                            rw.secondary_end_id = r.GetValue(3) != DBNull.Value ? r.GetInt64(3) : 0;
                            rw.surface = r.GetValue(4) != DBNull.Value ? r.GetString(4) : "";
                            rw.smoothness = r.GetValue(5) != DBNull.Value ? r.GetDouble(5) : 0;
                            rw.shoulder = r.GetValue(6) != DBNull.Value ? r.GetString(6) : "";
                            rw.length = r.GetValue(7) != DBNull.Value ? r.GetInt64(7) : 0;
                            rw.width = r.GetValue(8) != DBNull.Value ? r.GetInt64(8) : 0;
                            rw.heading = r.GetValue(9) != DBNull.Value ? r.GetDouble(9) : 0;
                            rw.pattern_altitude = r.GetValue(10) != DBNull.Value ? r.GetInt64(10) : 0;
                            rw.marking_flags = r.GetValue(11) != DBNull.Value ? r.GetInt64(11) : 0;
                            rw.edge_light = r.GetValue(12) != DBNull.Value ? r.GetString(12) : "";
                            rw.center_light = r.GetValue(13) != DBNull.Value ? r.GetString(13) : "";
                            rw.has_center_red = r.GetValue(14) != DBNull.Value ? r.GetInt64(14) : 0;
                            rw.primary_lonx = r.GetValue(15) != DBNull.Value ? r.GetDouble(15) : 0;
                            rw.primary_laty = r.GetValue(16) != DBNull.Value ? r.GetDouble(16) : 0;
                            rw.secondary_lonx = r.GetValue(17) != DBNull.Value ? r.GetDouble(17) : 0;
                            rw.secondary_laty = r.GetValue(18) != DBNull.Value ? r.GetDouble(18) : 0;
                            rw.altitude = r.GetValue(19) != DBNull.Value ? r.GetInt64(19) : 0;
                            rw.lonx = r.GetValue(20) != DBNull.Value ? r.GetDouble(20) : 0;
                            rw.laty = r.GetValue(21) != DBNull.Value ? r.GetDouble(21) : 0;

                            list.Add(rw);

                        }
                    }
                }
                catch (Exception ex)
                { }
            }
            return list;
        }


        public List<RunwayEnd> FillRunwayEnds()
        {
            List<RunwayEnd> list = new List<RunwayEnd>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM RUNWAY_END", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            RunwayEnd re = new RunwayEnd();

                            re.runway_end_id = r.GetValue(0) != DBNull.Value ? r.GetInt64(0) : 0;
                            re.name = r.GetValue(1) != DBNull.Value ? r.GetString(1) : "";
                            re.end_type = r.GetValue(2) != DBNull.Value ? r.GetString(2) : "";
                            re.offset_threshold = r.GetValue(3) != DBNull.Value ? r.GetInt64(3) : 0;
                            re.blast_pad = r.GetValue(4) != DBNull.Value ? r.GetInt64(4) : 0;
                            re.overrun = r.GetValue(5) != DBNull.Value ? r.GetInt64(5) : 0;
                            re.left_vasi_type = r.GetValue(6) != DBNull.Value ? r.GetString(6) : "";
                            re.left_vasi_pitch = r.GetValue(7) != DBNull.Value ? r.GetDouble(7) : 0;
                            re.right_vasi_type = r.GetValue(8) != DBNull.Value ? r.GetString(8) : "";
                            re.right_vasi_pitch = r.GetValue(9) != DBNull.Value ? r.GetDouble(9) : 0;
                            re.has_closed_markings = r.GetValue(10) != DBNull.Value ? r.GetInt64(10) : 0;
                            re.has_stol_markings = r.GetValue(11) != DBNull.Value ? r.GetInt64(11) : 0;
                            re.is_takeoff = r.GetValue(12) != DBNull.Value ? r.GetInt64(12) : 0;
                            re.is_landing = r.GetValue(13) != DBNull.Value ? r.GetInt64(13) : 0;
                            re.is_pattern = r.GetValue(14) != DBNull.Value ? r.GetString(14) : "";
                            re.app_light_system_type = r.GetValue(15) != DBNull.Value ? r.GetString(15) : "";
                            re.has_end_lights = r.GetValue(16) != DBNull.Value ? r.GetInt64(16) : 0;
                            re.has_reils = r.GetValue(17) != DBNull.Value ? r.GetInt64(17) : 0;
                            re.has_touchdown_lights = r.GetValue(18) != DBNull.Value ? r.GetInt64(18) : 0;
                            re.num_strobes = r.GetValue(19) != DBNull.Value ? r.GetInt64(19) : 0;
                            re.ils_ident = r.GetValue(20) != DBNull.Value ? r.GetString(20) : "";
                            re.heading = r.GetValue(21) != DBNull.Value ? r.GetDouble(21) : 0;
                            re.altitude = r.GetValue(22) != DBNull.Value ? r.GetInt64(22) : 0;
                            re.lonx = r.GetValue(23) != DBNull.Value ? r.GetDouble(23) : 0;
                            re.laty = r.GetValue(24) != DBNull.Value ? r.GetDouble(24) : 0;

                            list.Add(re);

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return list;
        }

        public List<Parking> FillParking()
        {
            List<Parking> list = new List<Parking>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM Parking", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            Parking pa = new Parking();

                            pa.parking_id = r.GetValue(0) != DBNull.Value ? r.GetInt64(0) : 0;
                            pa.airport_id = r.GetValue(1) != DBNull.Value ? r.GetInt64(1) : 0;
                            pa.type = r.GetValue(2) != DBNull.Value ? r.GetString(2) : "";
                            pa.pushback = r.GetValue(3) != DBNull.Value ? r.GetString(3) : "";
                            pa.name = r.GetValue(4) != DBNull.Value ? r.GetString(4) : "";
                            pa.number = r.GetValue(5) != DBNull.Value ? r.GetInt32(5) : 0;
                            pa.airline_codes = r.GetValue(6) != DBNull.Value ? r.GetString(6) : "";
                            pa.radius = r.GetValue(7) != DBNull.Value ? r.GetDouble(7) : 0;
                            pa.heading = r.GetValue(8) != DBNull.Value ? r.GetDouble(8) : 0;
                            pa.has_jetway = r.GetValue(9) != DBNull.Value ? r.GetInt64(9) : 0;
                            pa.lonx = r.GetValue(10) != DBNull.Value ? r.GetDouble(10) : 0;
                            pa.laty = r.GetValue(11) != DBNull.Value ? r.GetDouble(11) : 0;

                            list.Add(pa);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return list;
        }

        public List<ILS> FillILSs()
        {
            List<ILS> list = new List<ILS>();
            if (this.ConnectioString != null)
            {
                try
                {
                    using (var conn = new SQLiteConnection(this.ConnectioString))
                    {
                        conn.Open();

                        SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM ILS", conn);
                        SQLiteDataReader r = cmd.ExecuteReader();

                        while (r.Read())
                        {
                            ILS ils = new ILS();

                            ils.ils_id = r.GetValue(0) != DBNull.Value ? r.GetInt64(0) : 0;
                            ils.ident = r.GetValue(1) != DBNull.Value ? r.GetString(1) : "";
                            ils.name = r.GetValue(2) != DBNull.Value ? r.GetString(2) : "";
                            ils.region = r.GetValue(3) != DBNull.Value ? r.GetString(3) : "";
                            ils.frequency = r.GetValue(4) != DBNull.Value ? r.GetInt64(4) : 0;
                            ils.range = r.GetValue(5) != DBNull.Value ? r.GetInt64(5) : 0;
                            ils.mag_var = r.GetValue(6) != DBNull.Value ? r.GetDouble(6) : 0;
                            ils.has_backcourse = r.GetValue(7) != DBNull.Value ? r.GetInt64(7) : 0;
                            ils.dme_range = r.GetValue(8) != DBNull.Value ? r.GetInt64(8) : 0;
                            ils.dme_altitude = r.GetValue(9) != DBNull.Value ? r.GetInt64(9) : 0;
                            ils.dme_lonx = r.GetValue(10) != DBNull.Value ? r.GetDouble(10) : 0;
                            ils.dme_laty = r.GetValue(11) != DBNull.Value ? r.GetDouble(11) : 0;
                            ils.gs_range = r.GetValue(12) != DBNull.Value ? r.GetInt64(12) : 0;
                            ils.gs_pitch = r.GetValue(13) != DBNull.Value ? r.GetDouble(13) : 0;
                            ils.gs_altitude = r.GetValue(14) != DBNull.Value ? r.GetInt64(14) : 0;
                            ils.gs_lonx = r.GetValue(15) != DBNull.Value ? r.GetDouble(15) : 0;
                            ils.gs_laty = r.GetValue(16) != DBNull.Value ? r.GetDouble(16) : 0;
                            ils.loc_runway_end_id = r.GetValue(17) != DBNull.Value ? r.GetInt64(17) : 0;
                            ils.loc_airport_ident = r.GetValue(18) != DBNull.Value ? r.GetString(18) : "";
                            ils.loc_runway_name = r.GetValue(19) != DBNull.Value ? r.GetString(19) : "";
                            ils.loc_heading = r.GetValue(20) != DBNull.Value ? r.GetDouble(20) : 0;
                            ils.loc_width = r.GetValue(21) != DBNull.Value ? r.GetDouble(21) : 0;
                            ils.end1_lonx = r.GetValue(22) != DBNull.Value ? r.GetDouble(22) : 0;
                            ils.end1_laty = r.GetValue(23) != DBNull.Value ? r.GetDouble(23) : 0;
                            ils.end_mid_lonx = r.GetValue(24) != DBNull.Value ? r.GetDouble(24) : 0;
                            ils.end_mid_laty = r.GetValue(25) != DBNull.Value ? r.GetDouble(25) : 0;
                            ils.end2_lonx = r.GetValue(26) != DBNull.Value ? r.GetDouble(26) : 0;
                            ils.end2_laty = r.GetValue(27) != DBNull.Value ? r.GetDouble(27) : 0;
                            ils.altitude = r.GetValue(28) != DBNull.Value ? r.GetInt64(28) : 0;
                            ils.lonx = r.GetValue(29) != DBNull.Value ? r.GetDouble(29) : 0;
                            ils.laty = r.GetValue(30) != DBNull.Value ? r.GetDouble(30) : 0;

                            list.Add(ils);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return list;

        }
    }
}
