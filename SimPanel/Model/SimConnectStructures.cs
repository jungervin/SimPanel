using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BridgeClient.DataModel
{
    enum DATA_REQUESTS
    {
        ReadFromSimChanged,
        GenericData,
        GenericData2,
    }

    enum Structs
    {
        WriteToSim,
        ReadFromSim,
        GenericData,
    }

    enum ClientData
    {
        WriteToSim,
        ReadFromSim,
    }

    enum NOTIFICATION_GROUPS
    {
        GENERIC,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ReadFromSim
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
        public double[] data;
        public int valueCount;
        public int lastCommandId;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WriteToSim
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] name;
        public int index;
        public int isSet;
        public double value;
        public int lastCommandId;

        public static byte[] AllocString(string txt)
        {
            return GetArrayOfSize(Encoding.ASCII.GetBytes(txt), 256);
        }

        static byte[] GetArrayOfSize(byte[] input, int size)
        {
            var ret = new byte[size];
            Array.Copy(input, ret, input.Length);
            return ret;
        }
    }

}
