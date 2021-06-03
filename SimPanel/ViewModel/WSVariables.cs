using Newtonsoft.Json;
using SimPanel.Model;
using SimPanel.Properties;
using SimPanel.Utility;
using System;
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
    class WSVariables : WebSocketBehavior
    {
        public WSVariables()
        {
            //this.EmitOnPing = true;
            this.BroadCasting();
        }

        DateTime PrevFlightPlanDt = DateTime.Now;
        private void BroadCasting()
        {
            this.FStopped = false;
            this.Counter = 0;
            Task t = new Task(() =>
            {
                while (!FStopped)
                {
                    //if (Sessions != null && Sessions.Count > 0 && this.ConnectionState == WebSocketState.Open)
                    if (this.ConnectionState == WebSocketState.Open)
                        {
                            this.Send(Globals.MainWindow.SimConnectViewModel.GetVariablesJSON());

                        if(this.Counter == 0 || this.PrevFlightPlanDt != Globals.MainWindow.FlightPlanViewModel.LoadDt)
                        {
                            this.Send(Globals.MainWindow.FlightPlanViewModel.FlightPlanJSON);
                            this.PrevFlightPlanDt = Globals.MainWindow.FlightPlanViewModel.LoadDt;
                        }

                        Sessions.Sweep();
                        Thread.Sleep(75);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                    this.Counter++;
                }
                
            });

            t.Start();

        }
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data.StartsWith("cmd:"))
            {
                uint val = 0;
                string[] items = e.Data.Replace(" ", "").Split(',');

                Console.WriteLine(e.Data);
                try
                {
                    string ev = items[0].Replace("cmd:", "");

                    if (items.Length == 2)
                    {
                        if (uint.TryParse(items[1].Replace("val:", ""), out val))
                        {
                        }
                    }

                    this.MainWindow.SimConnectViewModel.SendEvent(ev, val);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                this.Send("{\"type\":\"status\", \"data\": \"ok\"}");
                return;

            }
            
            this.Send("{\"type\":\"error\", \"data\": \"Variables Socket\"}");
        }



        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            this.Sessions.Sweep();
            //this.Send(Globals.MainWindow.FlightPlanViewModel.FlightPlanJSON);
           // Globals.MainWindow.FlightPlanViewModel.FlightPlanChanged += FlightPlaneChanged;

        }

        //private void FlightPlaneChanged(object sender, EventArgs e)
        //{
            
        //    this.Send(Globals.MainWindow.FlightPlanViewModel.FlightPlanJSON);
        //}
        protected override void OnClose(CloseEventArgs e)
        {
            //Globals.MainWindow.FlightPlanViewModel.FlightPlanChanged -= FlightPlaneChanged;
            base.OnClose(e);

        }
        public MainWindowViewModel MainWindow
        {
            get
            {
                return SimPanel.MainWindow.App.MainWindowViewModel;
            }
        }

        public WebSocketServer WebSocketServer { get; private set; }
        public bool FStopped { get; private set; }

        private int Counter;
    }
}
