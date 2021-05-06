using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Model
{
    public class DataModel
    {

        public string cmd { get; set; }
        public string data { get; set; }

    }


    public class QueryModel
    {

        public string cmd { get; set; }
        public object data { get; set; }

    }

    public class AirPort
    {
        public AirPort()
        {
        }

        public long airport_id { get; set; }
        public long file_id { get; set; }
        public string ident { get; set; }
        public string icao { get; set; }
        public string iata { get; set; }
        public string xpident { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public long flatten { get; set; }
        public long fuel_flags { get; set; }
        public long has_avgas { get; set; }
        public long has_jetfuel { get; set; }
        public long has_tower_object { get; set; }
        public long tower_frequency { get; set; }
        public long atis_frequency { get; set; }
        public long awos_frequency { get; set; }
        public long asos_frequency { get; set; }
        public long unicom_frequency { get; set; }
        public long is_closed { get; set; }
        public long is_military { get; set; }
        public long is_addon { get; set; }
        public long num_com { get; set; }
        public long num_parking_gate { get; set; }
        public long num_parking_ga_ramp { get; set; }
        public long num_parking_cargo { get; set; }
        public long num_parking_mil_cargo { get; set; }
        public long num_parking_mil_combat { get; set; }
        public long num_approach { get; set; }
        public long num_runway_hard { get; set; }
        public long num_runway_soft { get; set; }
        public long num_runway_water { get; set; }
        public long num_runway_light { get; set; }
        public long num_runway_end_closed { get; set; }
        public long num_runway_end_vasi { get; set; }
        public long num_runway_end_als { get; set; }
        public long num_runway_end_ils { get; set; }
        public long num_apron { get; set; }
        public long num_taxi_path { get; set; }
        public long num_helipad { get; set; }
        public long num_jetway { get; set; }
        public long num_starts { get; set; }
        public long longest_runway_length { get; set; }
        public long longest_runway_width { get; set; }
        public double longest_runway_heading { get; set; }
        public string longest_runway_surface { get; set; }
        public long num_runways { get; set; }
        public string largest_parking_ramp { get; set; }
        public string largest_parking_gate { get; set; }
        public long rating { get; set; }
        public long is_3d { get; set; }
        public string scenery_local_path { get; set; }
        public string bgl_filename { get; set; }
        public double left_lonx { get; set; }
        public double top_laty { get; set; }
        public double right_lonx { get; set; }
        public double bottom_laty { get; set; }
        public double mag_var { get; set; }
        public long tower_altitude { get; set; }
        public double tower_lonx { get; set; }
        public double tower_laty { get; set; }
        public long transition_altitude { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }

    }

    public class Runway
    {
        public Runway()
        {
        }

        public long runway_id { get; set; }
        public long airport_id { get; set; }
        public long primary_end_id { get; set; }
        public long secondary_end_id { get; set; }
        public string surface { get; set; }
        public double smoothness { get; set; }
        public string shoulder { get; set; }
        public long length { get; set; }
        public long width { get; set; }
        public double heading { get; set; }
        public long pattern_altitude { get; set; }
        public long marking_flags { get; set; }
        public string edge_light { get; set; }
        public string center_light { get; set; }
        public long has_center_red { get; set; }
        public double primary_lonx { get; set; }
        public double primary_laty { get; set; }
        public double secondary_lonx { get; set; }
        public double secondary_laty { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }


        RunwayEnd Primary { get; set; }
        RunwayEnd Secondary { get; set; }
    }

    public class RunwayEnd
    {
        public RunwayEnd()
        {
        }

        public long runway_end_id { get; set; }
        public string name { get; set; }
        public string end_type { get; set; }
        public long offset_threshold { get; set; }
        public long blast_pad { get; set; }
        public long overrun { get; set; }
        public string left_vasi_type { get; set; }
        public double left_vasi_pitch { get; set; }
        public string right_vasi_type { get; set; }
        public double right_vasi_pitch { get; set; }
        public long has_closed_markings { get; set; }
        public long has_stol_markings { get; set; }
        public long is_takeoff { get; set; }
        public long is_landing { get; set; }
        public string is_pattern { get; set; }
        public string app_light_system_type { get; set; }
        public long has_end_lights { get; set; }
        public long has_reils { get; set; }
        public long has_touchdown_lights { get; set; }
        public long num_strobes { get; set; }
        public string ils_ident { get; set; }
        public double heading { get; set; }
        public long altitude { get; set; }
        public double lonx { get; set; }
        public double laty { get; set; }
    }

    public class Parking
    {
        public Parking()
        { }

        public long parking_id { get; set; }
        public long airport_id { get; set; }
        public string type { get; set; }
        public string pushback { get; set; }
        public string name { get; set; }
        public long number { get; set; }
        public string airline_codes { get; set; }
        public double radius { get; set; }
        public double heading { get; set; }
        public long has_jetway { get; set; }
        public double latx { get; set; }
        public double laty { get; set; }
    }
    public class AirPortInfo
    {
        public AirPortInfo()
        {

        }

        public string type { get { return "airportinfobyname"; } }
        public AirPort AirPort { get; set; }
        public List<RunwayInfo> Runways { get; set; }
    }

    public class RunwayInfo
    {
        public Runway Runway { get; set; }
        public RunwayEnd PrimaryEnd { get; set; }
        public RunwayEnd SecondaryEnd { get; set; }

    }


    public class ResponseRunways
    {
        public string type { get; set; }
        public DataTable runways { get; set; }
    }

    public class ResponseParkings
    {
        public string type { get; set; }
        public DataTable parkings { get; set; }
    }

    public class MapDetailsModel
    {
        public int zoom { get; set; }
        public int rating { get; set; }
        public MapBoundModel bounds { get; set; }
    }


    public class MapCoordModel
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class MapBoundModel
    {
        public MapCoordModel _southWest { get; set; }
        public MapCoordModel _northEast { get; set; }
    }


}
