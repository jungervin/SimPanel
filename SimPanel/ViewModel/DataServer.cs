using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SimPanel.ViewModel
{
    public class DataServer : WebSocketBehavior
    {
        public DataServer()
        {

        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data.StartsWith("getparking"))
            {
                string id = e.Data.Replace("getparking:", "");
                string res = ToCsv(this.SelectParking(int.Parse(id)));

                this.Send("{\"type\":\"parking\", \"data\": {" + res + "}}");
                return;
            }
            
            this.Send("{\"type\":\"status\", \"data\": \"Invalid\"}");
        }

        public DataTable FillAirports()
        {
            var db = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data", "little_navmap_navigraph.sqlite");
            var cs = $"Data Source = {db}; Cache = Shared";
            using (var conn = new SQLiteConnection(cs))
            {
                conn.Open();
                DataTable dt = new DataTable();
                SQLiteCommand cmd = new SQLiteCommand("select * from vor", conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                return dt;
            }
        }

        public DataTable SelectParking(int airport_id)
        {
            var db = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data", "little_navmap_navigraph.sqlite");
            var cs = $"Data Source = {db}; Cache = Shared";
            using (var conn = new SQLiteConnection(cs))
            {
                conn.Open();
                DataTable dt = new DataTable();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM PARKING WHERE AIRPORT_ID = @AIRPORT_ID", conn);
                cmd.Parameters.AddWithValue("@AIRPORT_ID", airport_id);
                SQLiteDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                return dt;
            }
        }

        public string ToCsv(DataTable dt)
        {
            string res = "";
            string sep = "";
            int n = 0;
            foreach (DataRow r in dt.Rows)
            {
                res = n++ > 0 ? "\n" : "";
                sep = "";
                foreach(object o in r.ItemArray)
                {
                    res += sep + o.ToString();
                    sep = ",";
                }
            
            }

            return res;
        }
    }
}
