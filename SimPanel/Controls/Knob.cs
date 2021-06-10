using SimPanel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SimPanel.Controls
{
    public class Knob : Control
    {
        private double prevAngle = double.MaxValue;
        private double diffAngle = double.MaxValue;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //
            if(this.SimConnect is SimConnectViewModel)
            {
                //this.Selected = true;
                this.SimConnect.SelectedKnob = this;

            }


            Point pos = Mouse.GetPosition(this);

            double dx = pos.X - (this.Width / 2.0);
            double dy = pos.Y - (this.Height / 2.0);

            double a = Math.Atan2(dy, dx) * 180.0 / Math.PI;
            if (dy < 0)
            {
                a = 360 + a;
            }

            diffAngle = this.Angle - a;

            //e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //base.OnMouseLeftButtonUp(e);
            diffAngle = double.MaxValue;
            this.ReleaseMouseCapture();
            //e.Handled = true;
            base.OnMouseLeftButtonUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            
            if (this.diffAngle < double.MaxValue)
            {
                    Point pos = Mouse.GetPosition(this);

                    double dx = pos.X - (this.Width / 2.0);
                    double dy = pos.Y - (this.Height / 2.0);

                    double a = Math.Atan2(dy, dx) * 180.0 / Math.PI;
                    if (dy < 0)
                    {
                        a = 360 + a;
                    }

                    this.Angle = (a + this.diffAngle) % 360.0;
                    if (Math.Abs(this.Angle - this.prevAngle) > 5)
                    {
                        if (this.Angle < prevAngle)
                        {
                        this.SimConnect.SendEvent(this.EventLeft, 0);

                        //                            simconnect.SendEvent(this.EventLeft);
                    }
                    else
                        {

                        this.SimConnect.SendEvent(this.EventRight, 0);
                        //                          simconnect.SendEvent(this.EventRight);
                    }
                    prevAngle = this.Angle;
                    }
                    this.CaptureMouse();
                //e.Handled = true;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
                this.Angle += e.Delta / 10;

                if (e.Delta < 0)
                {
                this.SimConnect.SendEvent(this.EventLeft, 0);
            }
            else
                {
                this.SimConnect.SendEvent(this.EventRight, 0);
            }

            //prevDelta = e.Delta;
            //e.Handled = true;
            //base.OnMouseWheel(e);
        }

        internal void Process(string data)
        {
            //throw new NotImplementedException();
            if (this.SimConnect != null)
            {
                if (data == "D1:R1=1" && this.EventRight != null)
                {
                    this.SimConnect.SendEvent(this.EventRight, 0);
                    this.Angle += 5;
                }
                else if (data == "D1:R1=-1" && this.EventLeft != null)
                {
                    this.SimConnect.SendEvent(this.EventLeft, 0);
                    this.Angle -= 5;
                }
                else if (data == "D1:R1=10")
                {
                    if (this.EventRightFast != null)
                    {
                        this.SimConnect.SendEvent(this.EventRightFast, 0);
                        this.Angle += 10;

                    }
                    else if (this.EventRight != null)
                    {
                        this.SimConnect.SendEvent(this.EventRight, 0);
                        this.Angle += 5;
                    }
                }
                else if (data == "D1:R1=-10")
                {
                    if (this.EventLeftFast != null)
                    {
                        this.SimConnect.SendEvent(this.EventLeftFast, 0);
                        this.Angle -= 10;
                    }
                    else if (this.EventLeft != null)
                    {
                        this.SimConnect.SendEvent(this.EventLeft, 0);
                        this.Angle -= 5;
                    }
                }
            }

        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(Knob), new PropertyMetadata(0.0));



        public SimConnectViewModel SimConnect
        {
            get { return (SimConnectViewModel)GetValue(SimConnectProperty); }
            set { SetValue(SimConnectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SimConnect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SimConnectProperty =
            DependencyProperty.Register("SimConnect", typeof(SimConnectViewModel), typeof(Knob), new PropertyMetadata(null));




        //public ImageSource ImageKnob
        //{
        //    get { return (ImageSource)GetValue(ImageKnobProperty); }
        //    set { SetValue(ImageKnobProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ImageKnob.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ImageKnobProperty =
        //    DependencyProperty.Register("ImageKnob", typeof(ImageSource), typeof(Knob), new PropertyMetadata(null));

        //public ImageSource ImageSelection
        //{
        //    get { return (ImageSource)GetValue(ImageSelectionProperty); }
        //    set { SetValue(ImageSelectionProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ImageSelection.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ImageSelectionProperty =
        //    DependencyProperty.Register("ImageSelection", typeof(ImageSource), typeof(Knob), new PropertyMetadata(null));

        //public EVENTS EventLeft
        //{
        //    get { return (EVENTS)GetValue(EventLeftProperty); }
        //    set { SetValue(EventLeftProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for EventLeft.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty EventLeftProperty =
        //    DependencyProperty.Register("EventLeft", typeof(EVENTS), typeof(Knob), new PropertyMetadata(EVENTS.KEY_NONE));

        //public EVENTS EventRight
        //{
        //    get { return (EVENTS)GetValue(EventRightProperty); }
        //    set { SetValue(EventRightProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for EventRight.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty EventRightProperty =
        //    DependencyProperty.Register("EventRight", typeof(EVENTS), typeof(Knob), new PropertyMetadata(EVENTS.KEY_NONE));




        //public EVENTS EventPush
        //{
        //    get { return (EVENTS)GetValue(EventPushProperty); }
        //    set { SetValue(EventPushProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for EventPush.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty EventPushProperty =
        //    DependencyProperty.Register("EventPush", typeof(EVENTS), typeof(Knob), new PropertyMetadata(EVENTS.KEY_NONE));



        public string EventLeft
        {
            get { return (string)GetValue(EventLeftProperty); }
            set { SetValue(EventLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventLeft.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventLeftProperty =
            DependencyProperty.Register("EventLeft", typeof(string), typeof(Knob), new PropertyMetadata(null));



        public string EventLeftFast
        {
            get { return (string)GetValue(EventLeftFastProperty); }
            set { SetValue(EventLeftFastProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventLeftFast.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventLeftFastProperty =
            DependencyProperty.Register("EventLeftFast", typeof(string), typeof(Knob), new PropertyMetadata(null));



        public string EventRight
        {
            get { return (string)GetValue(EventRightProperty); }
            set { SetValue(EventRightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventRight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventRightProperty =
            DependencyProperty.Register("EventRight", typeof(string), typeof(Knob), new PropertyMetadata(null));



        public string EventRightFast
        {
            get { return (string)GetValue(EventRightFastProperty); }
            set { SetValue(EventRightFastProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventRightFast.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventRightFastProperty =
            DependencyProperty.Register("EventRightFast", typeof(string), typeof(Knob), new PropertyMetadata(null));



        public string EventPush
        {
            get { return (string)GetValue(EventPushProperty); }
            set { SetValue(EventPushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventPush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventPushProperty =
            DependencyProperty.Register("EventPush", typeof(string), typeof(Knob), new PropertyMetadata(null));




        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(bool), typeof(Knob), new PropertyMetadata(false));

        //public void DataReceived(string data)
        //{
        //    //if (this.DataContext is ISimConnect simconnect)
        //    {
        //        if (data == "D1:R1=1")
        //        {
        //            Angle += 10.0;
        //            simconnect.SendEvent(this.EventRight);
        //        }
        //        else if (data == "D1:R1=-1")
        //        {
        //            Angle -= 10.0;
        //            simconnect.SendEvent(this.EventLeft);
        //        }
        //    }
        //}
    }
}
