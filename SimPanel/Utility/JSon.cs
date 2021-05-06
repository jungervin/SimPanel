using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Utility
{
    public class JSon
    {


        public Dictionary<String, Single> AirPlanePosition = new Dictionary<string, float>();
        public static string ToString(Dictionary<String, Single> dict)
        {
            string res = "";
            string sep = "";
            foreach (string key in dict.Keys)
            {
                res += sep + $"\"{key}\": {dict[key]}";
                sep = ", ";
            }

            return res + "";
        }




        public static string ToString(SIMCONNECT_RECV_VOR_LIST list)
        {
            string res = "";
            string sep = "";
            foreach (SIMCONNECT_DATA_FACILITY_VOR vor in list.rgData)
            {
                res += sep + "{";
                res += $"\"Latitude\": {(float)vor.Latitude},";
                res += $"\"Longitude\": {(float)vor.Longitude},";
                res += $"\"Altitude\": {(float)vor.Altitude},";
                res += $"\"fFrequency\": {(int)vor.fFrequency},";
                res += $"\"fGlideSlopeAngle\": {(float)vor.fGlideSlopeAngle},";
                res += $"\"Flags\": {(float)vor.Flags},";
                res += $"\"fLocalizer\": {(float)vor.fLocalizer},";
                res += $"\"fMagVar\": {(float)vor.fMagVar},";
                res += $"\"GlideAlt\": {(float)vor.GlideAlt},";
                res += $"\"GlideLat\": {(float)vor.GlideLat},";
                res += $"\"GlideLon\": {(float)vor.GlideLon},";
                res += $"\"Icao\": \"{vor.Icao}\"";
                res += "}";
                sep = ", ";
            }
            return $"\"vors\": [ {res} ]";
        }

        public static string ToString(SIMCONNECT_RECV_AIRPORT_LIST list)
        {
            string res = "";
            string sep = "";
            foreach (SIMCONNECT_DATA_FACILITY_AIRPORT a in list.rgData)
            {
                res += sep + "{";
                res += $"\"Latitude\": {(float)a.Latitude},";
                res += $"\"Longitude\": {(float)a.Longitude},";
                res += $"\"Altitude\": {(float)a.Altitude},";
                res += $"\"Icao\": \"{a.Icao}\"";
                res += "}";
                sep = ", ";
            }
            return $"\"airports\": [ {res} ]";
        }



        static void Dump(Object item)
        {
            String s = "";
            foreach (System.Reflection.FieldInfo f in item.GetType().GetFields())
            {
                if (!f.FieldType.IsArray)
                {
                    s += "  " + f.Name + ": " + f.GetValue(item);
                }
            }
            Console.WriteLine(s);
        }


        public static string ToString(string res, string name, string value)
        {
            if (res != "" )
            {
                res += ", ";
            }
            res += $"\"{name}\": \"{value}\"";
            return res;
        }
        public static string ToString(string res, string name, int value)
        {
            if (res != "")
            {
                res += ", ";
            }
            res += $"\"{name}\": {value}";
            return res;
        }

        public static string ToString(string res, string name, float value)
        {
            if (res != "")
            {
                res += ", ";
            }
            res += $"\"{name}\": {value}";
            return res;
        }

        public static string ToString(string res, string name, double value)
        {
            if (res != "")
            {
                res += ", ";
            }
            res += $"\"{name}\": {value}";
            return res;
        }


    }
}
