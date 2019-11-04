namespace MOEX.ASTS.Client
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Dynamic
    {
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll", CharSet=CharSet.Ansi, SetLastError=true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern IntPtr LoadLibrary(string lpFilename);
    }
}

