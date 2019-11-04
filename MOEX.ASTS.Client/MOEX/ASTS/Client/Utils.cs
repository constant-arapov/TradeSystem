namespace MOEX.ASTS.Client
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class Utils
    {
        public static string FetchAscii(ref IntPtr buffer)
        {
            byte[] bytes = new byte[0x400];
            int count = 0;
            while (true)
            {
                byte num2 = FetchByte(ref buffer);
                if (num2 == 0)
                {
                    return Encoding.ASCII.GetString(bytes, 0, count);
                }
                bytes[count++] = num2;
            }
        }

        public static byte FetchByte(ref IntPtr buffer)
        {
            byte num = Marshal.ReadByte(buffer);
            buffer += Marshal.SizeOf(typeof(byte));
            return num;
        }

        public static int FetchInt(ref IntPtr buffer)
        {
            int num = Marshal.ReadInt32(buffer);
            buffer += Marshal.SizeOf(typeof(int));
            return num;
        }
    }
}

