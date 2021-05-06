using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Model
{
    public class CoordModel
    {
        public static CoordModel Parse(string t)
        {
            
            string[] items = t.Replace(" ", "").Replace("\t", " ").Replace("�", "°").Split(',');

            


            if (items.Length >= 2)
            {
                CoordModel coord = new CoordModel();
                coord.Lat = DMS.Parse(items[0]);
                coord.Lng = DMS.Parse(items[1]);
                return coord;
            }

            return null;
        
        }

        private DMS FLat;

        public DMS Lat
        {
            get { return FLat; }
            set { FLat = value; }
        }

        private DMS FLng;

        public DMS Lng
        {
            get { return FLng; }
            set { FLng = value; }
        }

    }

    public class DMS
    {
        public double d;
        public double m;
        public double s;
        public double dir = 1;
        public static DMS Parse(string t)
        {
            string elem = "";

            DMS dms = new DMS();
            foreach(char c in t)
            {
                switch(c)
                {
                    case '°':
                        if(elem.StartsWith("W") || elem.StartsWith("S")) {
                            dms.dir = -1;
                        }

                        elem = elem.Replace("N", "").Replace("W", "").Replace("S", "").Replace("E", "");
                        dms.d = double.Parse(elem);

                        elem = "";
                        break;

                    case '\'':
                        dms.m = double.Parse(elem);
                        elem = "";
                        break;
                    case '"':
                        dms.s = double.Parse(elem);
                        elem = "";
                        break;

                    default: elem += c;
                        break;
                }

            }
            return dms;


            //string res = t.Replace(" ", "");
            //res = res.Replace('°', ' ');
            //res = res.Replace('\'', ' ');
            //res = res.Replace('"', ' ');
            //string[] items = res.Split(' ');
            //if (items.Length >= 2)
            //{
            //    DMS dms = new DMS();

            //    string d = items[0];
            //    switch(d[0])
            //    {
            //        case 'N':
            //            dms.dir = 1;
            //            break;
            //        case 'W':
            //            dms.dir = -1;
            //            break;
            //        case 'S':
            //            dms.dir = -1;
            //            break;
            //        case 'E':
            //            dms.dir = 1;
            //            break;
            //    }

            //    d = d.Replace("N", "").Replace("W", "").Replace("S", "").Replace("E", "");
            //    if (items.Length == 3)
            //    {
            //        dms.d = double.Parse(d);
            //        dms.m = double.Parse(items[1]);
            //        dms.s = double.Parse(items[2]);
            //    }
            //    else
            //    {
            //        dms.d = double.Parse(d);
            //        dms.m = double.Parse(items[1]);
            //        dms.s = double.Parse(items[2]);
            //    }
            //return dms;
            //}

            return null;
        }

        public double Degree
        {
            get
            {
                return (this.d + this.m / 60.0 + this.s / 3600.0) * this.dir;
            }
            
        }
    }
}
