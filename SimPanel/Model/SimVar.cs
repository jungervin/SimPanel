using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimPanel.Model
{
    public class SimVar : ObservableObject
    {


        //public enum SIMCONNECT_DATATYPE
        //{
        //    INVALID = 0,
        //    INT32 = 1,
        //    INT64 = 2,
        //    FLOAT32 = 3,
        //    FLOAT64 = 4,
        //    STRING8 = 5,
        //    STRING32 = 6,
        //    STRING64 = 7,
        //    STRING128 = 8,
        //    STRING256 = 9,
        //    STRING260 = 10,
        //    STRINGV = 11,
        //    INITPOSITION = 12,
        //    MARKERSTATE = 13,
        //    WAYPOINT = 14,
        //    LATLONALT = 15,
        //    XYZ = 16,
        //    MAX = 17
        //}

        public struct STRING8
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public String Value;
        }
        public struct STRING32
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String Value;
        }
        public struct STRING64
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String Value;
        }
        public struct STRING128
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String Value;
        }
        public struct STRING256
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Value;
        }
        public struct STRING260
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public String Value;
        }
        public struct STRINGV
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Value;
        }



        public enum SIMDEFINITION
        {
            Dummy
        }

        public enum SIMREQUEST
        {
            Dummy
        }

        private string FVarName;

        public string VarName
        {
            get { return FVarName; }
            set
            {
                FVarName = value;
                this.ID = value.Replace(' ', '_').Replace(':', '_');
                this.OnPropertyChanged();
            }
        }

        private string FID;

        public string ID
        {
            get { return FID; }
            set
            {
                FID = value;
                this.OnPropertyChanged();
            }
        }


        private string FUnit;

        public string Unit
        {
            get { return FUnit; }
            set
            {
                FUnit = value;
                this.OnPropertyChanged();
            }
        }


        private bool FSubscribed;

        public bool Subscribed
        {
            get { return FSubscribed; }
            set
            {
                FSubscribed = value;
                this.OnPropertyChanged();
            }
        }

        private int FDefID;

        public int DefID
        {
            get { return FDefID; }
            set
            {
                FDefID = value;
                this.OnPropertyChanged();
            }
        }

        private object FValue;

        public object Value
        {
            get { return FValue; }
            set
            {
                FValue = value;
                this.LastUpdate = DateTime.Now;
                this.OnPropertyChanged();
            }
        }

        private int FClientRequestID = -1;

        public int ClientRequestID
        {
            get { return FClientRequestID; }
            set { FClientRequestID = value;
                this.OnPropertyChanged();
            }
        }


        public SIMCONNECT_SIMOBJECT_TYPE SimObjectType
        {
            get { return m_eSimObjectType; }
            set
            {
                this.SetProperty(ref m_eSimObjectType, value);
                //bObjectIDSelectionEnabled = (m_eSimObjectType != SIMCONNECT_SIMOBJECT_TYPE.USER);
                //ClearResquestsPendingState();
            }
        }
        private SIMCONNECT_SIMOBJECT_TYPE m_eSimObjectType = SIMCONNECT_SIMOBJECT_TYPE.USER;

        private string FWritable;

        public string Writable
        {
            get { return FWritable; }
            set
            {
                FWritable = value;
                this.OnPropertyChanged();
            }
        }

        private DateTime FLastUpdate;

        public DateTime LastUpdate
        {
            get { return FLastUpdate; }
            set
            {
                FLastUpdate = value;
                this.OnPropertyChanged();
            }
        }

        private SIMCONNECT_DATATYPE FDATATYPE;

        public SIMCONNECT_DATATYPE DataType
        {
            get { return FDATATYPE; }
            set
            {
                FDATATYPE = value;
                this.OnPropertyChanged();
            }
        }

    }
}
