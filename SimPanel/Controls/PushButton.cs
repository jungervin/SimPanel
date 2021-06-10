using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimPanel.Controls
{
    public class PushButton : Control
    {
        private bool Pressed = false;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if(this.Pressed == false)
            {
                this.Pressed = true;
                if(this.SimConnect != null)
                {
                    this.SimConnect.SendEvent(this.PressEvent, 0);
                }


            }
            this.Pressed = true;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.Pressed = false;
            base.OnMouseLeftButtonUp(e);
        }



        public string PressEvent
        {
            get { return (string)GetValue(PressEventProperty); }
            set { SetValue(PressEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressEvenet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressEventProperty =
            DependencyProperty.Register("PressEvent", typeof(string), typeof(PushButton), new PropertyMetadata(null));



        public string LongPressEvent
        {
            get { return (string)GetValue(LongPressEventProperty); }
            set { SetValue(LongPressEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LongPressEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LongPressEventProperty =
            DependencyProperty.Register("LongPressEvent", typeof(string), typeof(PushButton), new PropertyMetadata(null));


        public SimConnectViewModel SimConnect
        {
            get { return (SimConnectViewModel)GetValue(SimConnectProperty); }
            set { SetValue(SimConnectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SimConnect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SimConnectProperty =
            DependencyProperty.Register("SimConnect", typeof(SimConnectViewModel), typeof(PushButton), new PropertyMetadata(null));



        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadiusX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusXProperty =
            DependencyProperty.Register("RadiusX", typeof(double), typeof(PushButton), new PropertyMetadata(0.0));



        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadiusY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusYProperty =
            DependencyProperty.Register("RadiusY", typeof(double), typeof(PushButton), new PropertyMetadata(0.0));



    }
}
