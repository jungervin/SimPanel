using SimPanel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Utility
{
    public class StaticLists
    {

        public static string VariablesFile = @".\Data\variables.txt";
        public static string EventsFile = @".\Data\events.txt";

        private static List<SimVar> SimVarList = null;
        public static void LoadVariables(string filename)
        {
            if (File.Exists(filename))
            {
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
                    SimVarList.Add(svar);
                }
            }
        }

        public static List<SimVar> SimConnectVariables
        {
            get
            {
                if (SimVarList == null)
                {
                    SimVarList = new List<SimVar>();
                    LoadVariables(VariablesFile);
                }
                return SimVarList;
            }
        }


        private static List<SimEvent> SimEventList = null;
        public static void LoadEvents(string filename)
        {
            if (File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);
                foreach (string l in lines)
                {
                    string[] items = l.Split(',');
                    SimEvent se = new SimEvent()
                    {
                    };
                    string n = "";
                    if (items.Length == 2)
                    {
                        se.EventName = items[0];
                        SimEventList.Add(se);
                    }
                    if (items.Length > 2)
                    {
                        se.EventName = items[1];
                        SimEventList.Add(se);
                    }



                    
                }
            }
        }

        public static List<SimEvent> SimConnectEvents
        {
            get
            {
                if (SimEventList == null)
                {
                    SimEventList = new List<SimEvent>();
                    LoadEvents(EventsFile);
                }
                return SimEventList;
            }
        }

    }
}
