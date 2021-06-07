using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimPanel.View
{
    /// <summary>
    /// Interaction logic for G1000MFDView.xaml
    /// </summary>
    public partial class G1000MFDView : Window
    {

        public G1000MFDViewModel ViewModel { get; }
        public G1000MFDView()
        {
            InitializeComponent();
            this.ViewModel = new G1000MFDViewModel();
            this.DataContext = this.ViewModel;

        }

        private void G1000_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void AP_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void FMSInner_Button_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.ViewModel.FMSInnerAngle = this.ViewModel.FMSInnerAngle == 0 ? 4 : 0;
            }
            else
            {
                this.ViewModel.FMSInnerAngle = this.ViewModel.FMSInnerAngle == 0 ? -4 : 0;
            }
        }

        private void FMSOuter_Button_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                this.ViewModel.FMSOuterAngle = this.ViewModel.FMSOuterAngle == 0 ? 8 : 0;
            }
            else
            {
                this.ViewModel.FMSOuterAngle = this.ViewModel.FMSOuterAngle == 0 ? -8 : 0;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
            else if (e.Key == Key.F12)
            {
                this.Topmost = !this.Topmost;
            }
        }

        private void HDG_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("", 0);
        }

        private void Soft6_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_6", 0);
        }

        private void NAV_FF_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_NAV_Switch", 0);
        }

        private void AS1000_MFD_VOL_1_INC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_VOL_1_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_VOL_1_INC", 0);
            }
        }

        private void AS1000_MFD_NAV_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_NAV_Push", 0);
        }

        private void AS1000_MFD_NAV_Small_Rotate(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_NAV_Small_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_NAV_Small_INC", 0);
            }

        }

        private void AS1000_MFD_NAV_Large_Rotate(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_NAV_Large_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_NAV_Large_INC", 0);
            }
        }

        private void AS1000_MFD_HEADING_SYNC(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_HEADING_SYNC", 1);
        }

        private void AS1000_MFD_HEADING_Rotate(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_HEADING_FAST_DEC", 0);
                this.ViewModel.HDGAngle -= 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_HEADING_DEC", 0);
            }
            else
            {
                //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_HEADING_FAST_INC", 0);
                this.ViewModel.HDGAngle += 5;
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_HEADING_INC", 0);
            }

        }

        private void AP_MASTER(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("AP_MASTER", 0);
        }

        private void TOGGLE_FLIGHT_DIRECTOR(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("TOGGLE_FLIGHT_DIRECTOR", 0);
        }

        private void AP_HDG_HOLD(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("AP_HDG_HOLD", 0);
        }

        private void AP_ALT_HOLD(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("AP_ALT_HOLD", 0);
        }

        private void AP_NAV1_HOLD(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("AP_NAV1_HOLD", 0);
        }

        private void AP_APR_HOLD(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("AP_APR_HOLD", 0);
        }

        private void AP_BC_HOLD(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("AP_BC_HOLD", 0);
        }

        private void AP_VS_HOLD(object sender, MouseButtonEventArgs e)
        {

        }

        private void NOSEUP(object sender, MouseButtonEventArgs e)
        {
            var flc = Globals.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "AUTOPILOT_FLIGHT_LEVEL_CHANGE").FirstOrDefault();
            var vs = Globals.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "AUTOPILOT_VERTICAL_HOLD").FirstOrDefault();
            if (flc != null && (double)flc.Value == 1)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("AP_SPD_VAR_INC", 0);
            }
            else if (vs != null && (double)vs.Value == 1)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("AP_VS_VAR_INC", 0);
            }

        }

        private void FLIGHT_LEVEL_CHANGE(object sender, MouseButtonEventArgs e)
        {
            var sp = Globals.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "AIRSPEED INDICATED").FirstOrDefault();
            if (sp != null)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("FLIGHT_LEVEL_CHANGE", 0);
                Globals.MainWindow.SimConnectViewModel.SendEvent("AP_SPD_VAR_SET", Convert.ToUInt32(sp.Value));
            }

        }

        private void NOSEDOWN(object sender, MouseButtonEventArgs e)
        {
            var flc = Globals.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "AUTOPILOT_FLIGHT_LEVEL_CHANGE").FirstOrDefault();
            var vs = Globals.MainWindow.SimConnectViewModel.VarList.Where(k => k.VarName == "AUTOPILOT_VERTICAL_HOLD").FirstOrDefault();
            if (flc != null && (double)flc.Value == 1)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("AP_SPD_VAR_DEC", 0);
            }
            else if (vs != null && (double)vs.Value == 1)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("AP_VS_VAR_DEC", 0);
            }
        }

        private void AS1000_AP_ALT_DEC_100(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_AP_ALT_DEC_100", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_AP_ALT_INC_100", 0);
            }
        }

        private void AS1000_AP_ALT_DEC_1000(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_AP_ALT_DEC_1000", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_AP_ALT_INC_1000", 0);
            }
        }

        private void AS1000_MFD_SOFTKEYS_1(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_1", 0);
        }

        private void AS1000_MFD_SOFTKEYS_2(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_2", 0);
        }

        private void AS1000_MFD_SOFTKEYS_3(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_3", 0);
        }

        private void AS1000_MFD_SOFTKEYS_4(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_4", 0);
        }

        private void AS1000_MFD_SOFTKEYS_5(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_5", 0);
        }

        private void AS1000_MFD_SOFTKEYS_6(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_6", 0);
        }

        private void AS1000_MFD_SOFTKEYS_7(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_7", 0);
        }

        private void AS1000_MFD_SOFTKEYS_8(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_8", 0);
        }

        private void AS1000_MFD_SOFTKEYS_9(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_9", 0);
        }

        private void AS1000_MFD_SOFTKEYS_10(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_10", 0);
        }

        private void AS1000_MFD_SOFTKEYS_11(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_11", 0);
        }

        private void AS1000_MFD_SOFTKEYS_12(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_SOFTKEYS_12", 0);
        }

        private void AS1000_MFD_VOL_2_INC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_VOL_2_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_VOL_2_INC", 0);
            }
        }

        private void AS1000_MFD_COM_Switch(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Switch", 0);
        }

        private void AS1000_MFD_COM_Small_DEC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Small_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Small_INC", 0);
            }
        }

        private void AS1000_MFD_COM_Large_DEC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Large_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Large_INC", 0);
            }
        }


        bool COM_Down = false;
        private void AS1000_MFD_COM_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Push", 0);

            if (this.COM_Down == false)
            {
                this.COM_Down = true;
                DateTime dt = DateTime.Now;
                Task t = new Task(() =>
                {
                    while (this.COM_Down)
                    {
                        if ((DateTime.Now - dt).TotalSeconds > 2)
                        {
                            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Push_Long", 0);
                            this.COM_Down = false;
                            return;
                        }
                    }
                    this.COM_Down = false;
                    //                    Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_PFD_COM_Push", 0);
                });
                t.Start();
            }

            //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_PFD_COM_Switch_Long", 0);
            //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Push", 0);
        }

        private void AS1000_MFD_COM_Push_Up(object sender, MouseButtonEventArgs e)
        {
            this.COM_Down = false;
        }

        //private void AS1000_MFD_COM_Push(object sender, MouseButtonEventArgs e)
        //{

        //    //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_PFD_COM_Switch_Long", 0);
        //    Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_COM_Push", 0);
        //}

        //private void AS1000_MFD_COM_Push_Up(object sender, MouseButtonEventArgs e)
        //{

        //}

        private void AS1000_MFD_CRS_DEC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CRS_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CRS_INC", 0);
            }
        }

        private void AS1000_MFD_CRS_PUSH(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CRS_PUSH", 0);
        }

        private void AS1000_MFD_BARO_DEC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_BARO_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_BARO_INC", 0);
            }

        }

        private void AS1000_MFD_RANGE_DEC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_RANGE_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_RANGE_INC", 0);
            }
        }

        private void AS1000_MFD_DIRECTTO(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_DIRECTTO", 0);
        }

        private void AS1000_MFD_MENU_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_MENU_Push", 0);
        }

        private void AS1000_MFD_FPL_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_FPL_Push", 0);
        }

        private void AS1000_MFD_PROC_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_PROC_Push", 0);
        }



        bool CLR_Down = false;
        private void AS1000_MFD_CLR(object sender, MouseButtonEventArgs e)
        {
            //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CLR_Long", 0);
            //Thread.Sleep(1500);
            //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CLR_Long", 0);
            //return;

            if (this.CLR_Down == false)
            {
                this.CLR_Down = true;
                DateTime dt = DateTime.Now;
                Task t = new Task(() =>
                {
                    while (this.CLR_Down)
                    {
                        if((DateTime.Now - dt).TotalSeconds > 2)
                        {
                            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CLR_Long", 0);
                            this.CLR_Down = false;
                            return;
                        }
                    }
                    this.CLR_Down = false;
                    Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CLR", 0);
                });
                t.Start();
            }
        }


        //Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_CLR", 0);
        //}

        private void AS1000_MFD_CLR_Up(object sender, MouseButtonEventArgs e)
        {
            this.CLR_Down = false;
        }

        private void AS1000_MFD_ENT_Push(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_ENT_Push", 0);
        }

        private void AS1000_MFD_FMS_Upper_PUSH(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_FMS_Upper_PUSH", 0);
        }

        private void AS1000_MFD_FMS_Upper_DEC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_FMS_Upper_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_FMS_Upper_INC", 0);
            }
        }

        private void AS1000_MFD_FMS_Lower_DEC(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_FMS_Lower_DEC", 0);
            }
            else
            {
                Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_FMS_Lower_INC", 0);
            }
        }

        private void AS1000_MFD_JOYSTICK_PUSH(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_JOYSTICK_PUSH", 0);
        }

        private void AS1000_MFD_JOYSTICK_LEFT(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_JOYSTICK_LEFT", 0);
        }

        private void AS1000_MFD_JOYSTICK_RIGHT(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_JOYSTICK_RIGHT", 0);
        }

        private void AS1000_MFD_JOYSTICK_UP(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_JOYSTICK_UP", 0);
        }

        private void AS1000_MFD_JOYSTICK_DOWN(object sender, MouseButtonEventArgs e)
        {
            Globals.MainWindow.SimConnectViewModel.SendEvent("MobiFlight.AS1000_MFD_JOYSTICK_DOWN", 0);
        }
    }
}

