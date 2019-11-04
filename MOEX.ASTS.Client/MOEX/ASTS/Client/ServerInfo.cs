namespace MOEX.ASTS.Client
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class ServerInfo
    {
        public readonly byte Beta_version;
        public readonly List<string> Boards = new List<string>();
        public readonly byte Debug_flag;
        public readonly int Event_Date;
        public readonly string MICEX_Sever_Name;
        public readonly int Next_Event;
        public readonly string ServerIP;
        public readonly int SessionID;
        public readonly int Start_Time;
        public readonly int Stop_Time_Max;
        public readonly int Stop_Time_Min;
        public readonly char SystemID;
        public readonly byte Test_flag;
        public readonly string UserID;
        public readonly byte Version_Build;
        public readonly byte Version_Major;
        public readonly byte Version_Minor;

        internal ServerInfo(IntPtr buffer)
        {
            Utils.FetchInt(ref buffer);
            this.SessionID = Utils.FetchInt(ref buffer);
            byte[] destination = new byte[0x21];
            Marshal.Copy(buffer, destination, 0, destination.Length);
            int length = destination.Length;
            while ((length > 0) && (destination[length - 1] == 0))
            {
                length--;
            }
            this.MICEX_Sever_Name = Encoding.ASCII.GetString(destination, 0, length);
            buffer += destination.Length;
            this.Version_Major = Utils.FetchByte(ref buffer);
            this.Version_Minor = Utils.FetchByte(ref buffer);
            this.Version_Build = Utils.FetchByte(ref buffer);
            this.Beta_version = Utils.FetchByte(ref buffer);
            this.Debug_flag = Utils.FetchByte(ref buffer);
            this.Test_flag = Utils.FetchByte(ref buffer);
            this.Start_Time = Utils.FetchInt(ref buffer);
            this.Stop_Time_Min = Utils.FetchInt(ref buffer);
            this.Stop_Time_Max = Utils.FetchInt(ref buffer);
            this.Next_Event = Utils.FetchInt(ref buffer);
            this.Event_Date = Utils.FetchInt(ref buffer);
            this.Boards.AddRange(Utils.FetchAscii(ref buffer).Split(new char[] { ',' }));
            byte[] buffer3 = new byte[13];
            Marshal.Copy(buffer, buffer3, 0, buffer3.Length);
            int count = buffer3.Length;
            while ((count > 0) && (buffer3[count - 1] == 0))
            {
                count--;
            }
            this.UserID = Encoding.ASCII.GetString(buffer3, 0, count);
            buffer += buffer3.Length;
            this.SystemID = (char) Utils.FetchByte(ref buffer);
            this.ServerIP = Utils.FetchAscii(ref buffer);
        }

        public override string ToString()
        {
            return string.Concat(new object[] { this.MICEX_Sever_Name, ":", this.SessionID, "/", this.UserID });
        }
    }
}

