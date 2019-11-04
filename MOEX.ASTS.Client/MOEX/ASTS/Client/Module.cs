namespace MOEX.ASTS.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class Module : IDisposable
    {
        private List<MOEX.ASTS.Client.Client> clients;
        private bool disposed;
        private static readonly StringDictionary Greetings;
        private IntPtr handle;
        internal static readonly StringDictionary LangToName;
        internal TMTEAddTable MTEAddTable;
        /*internal*/public TMTECloseTable MTECloseTable;
        internal TMTEConnect MTEConnect;
        internal TMTEDisconnect MTEDisconnect;
        internal TMTEErrorMsg MTEErrorMsg;
        internal TMTEErrorMsgEx MTEErrorMsgEx;
        internal TMTEExecTrans MTEExecTrans;
        internal TMTEGetServInfo MTEGetServInfo;
        internal TMTEGetSnapshot MTEGetSnapshot;
        internal TMTEGetVersion MTEGetVersion;
        internal TMTEOpenTable MTEOpenTable;
        internal TMTEOpenTableAtSnapshot MTEOpenTableAtSnapshot;
        internal TMTERefresh MTERefresh;
        internal TMTESelectBoards MTESelectBoards;
        internal TMTEStructureEx MTEStructureEx;
        internal static readonly StringDictionary NameToLang;

        static Module()
        {
            StringDictionary dictionary = new StringDictionary();
            dictionary.Add("firm:", "en");
            dictionary.Add("фирма:", "ru");
            dictionary.Add("фiрма:", "ua");
            Greetings = dictionary;
            StringDictionary dictionary2 = new StringDictionary();
            dictionary2.Add("en", "English");
            dictionary2.Add("ru", "Russian");
            dictionary2.Add("ua", "Ukranian");
            LangToName = dictionary2;
            StringDictionary dictionary3 = new StringDictionary();
            dictionary3.Add("ENGLISH", "en");
            dictionary3.Add("RUSSIAN", "ru");
            dictionary3.Add("UKRANIAN", "ua");
            NameToLang = dictionary3;
        }

        private Module(IntPtr handle)
        {
            this.handle = handle;
            this.clients = new List<MOEX.ASTS.Client.Client>();
            this.MTEConnect = this.LoadProc("MTEConnect", typeof(TMTEConnect), true) as TMTEConnect;
            this.MTEDisconnect = this.LoadProc("MTEDisconnect", typeof(TMTEDisconnect), true) as TMTEDisconnect;
            this.MTEGetVersion = this.LoadProc("MTEGetVersion", typeof(TMTEGetVersion), true) as TMTEGetVersion;
            this.MTEErrorMsg = this.LoadProc("MTEErrorMsg", typeof(TMTEErrorMsg), true) as TMTEErrorMsg;
            this.MTEErrorMsgEx = this.LoadProc("MTEErrorMsgEx", typeof(TMTEErrorMsgEx), false) as TMTEErrorMsgEx;
            this.MTEGetServInfo = this.LoadProc("MTEGetServInfo", typeof(TMTEGetServInfo), false) as TMTEGetServInfo;
            this.MTESelectBoards = this.LoadProc("MTESelectBoards", typeof(TMTESelectBoards), true) as TMTESelectBoards;
            this.MTEStructureEx = this.LoadProc("MTEStructureEx", typeof(TMTEStructureEx), true) as TMTEStructureEx;
            this.MTEGetSnapshot = this.LoadProc("MTEGetSnapshot", typeof(TMTEGetSnapshot), false) as TMTEGetSnapshot;
            this.MTEOpenTable = this.LoadProc("MTEOpenTable", typeof(TMTEOpenTable), true) as TMTEOpenTable;
            this.MTEOpenTableAtSnapshot = this.LoadProc("MTEOpenTableAtSnapshot", typeof(TMTEOpenTableAtSnapshot), false) as TMTEOpenTableAtSnapshot;
            this.MTECloseTable = this.LoadProc("MTECloseTable", typeof(TMTECloseTable), true) as TMTECloseTable;
            this.MTEAddTable = this.LoadProc("MTEAddTable", typeof(TMTEAddTable), true) as TMTEAddTable;
            this.MTERefresh = this.LoadProc("MTERefresh", typeof(TMTERefresh), true) as TMTERefresh;
            this.MTEExecTrans = this.LoadProc("MTEExecTrans", typeof(TMTEExecTrans), true) as TMTEExecTrans;
        }

        public void Close()
        {
            this.Dispose();
        }

        internal void Closed(MOEX.ASTS.Client.Client client)
        {
            lock (this.clients)
            {
                this.clients.Remove(client);
            }
        }

        public MOEX.ASTS.Client.Client Connect(StringDictionary parameters)
        {
            return this.Connect(parameters, new Scales());
        }

        public MOEX.ASTS.Client.Client Connect(StringDictionary parameters, Scales scales)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
            string twoLetterISOLanguageName = null;
            StringBuilder builder = new StringBuilder();
            foreach (DictionaryEntry entry in parameters)
            {
                if (string.Equals("LANGUAGE", entry.Key.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    twoLetterISOLanguageName = NameToLang[entry.Value.ToString().ToUpper()];
                }
                StringBuilder introduced13 = builder.Append(entry.Key).Append("=");
                introduced13.Append(entry.Value).AppendLine();
            }
            if (twoLetterISOLanguageName == null)
            {
                twoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
            StringBuilder report = new StringBuilder(0x400);
            int code = this.MTEConnect(builder.ToString(), report);
            if (code < 0)
            {
                throw new ClientException(code, report.ToString());
            }
            string language = DetectLanguage(report.ToString());
            if (!language.Equals(twoLetterISOLanguageName))
            {
                char ch = twoLetterISOLanguageName.ToUpper()[0];
                if (this.MTEExecTrans(code, "CHANGE_LANGUAGE", ch.ToString(), report) == 0)
                {
                    language = twoLetterISOLanguageName;
                }
            }
            MOEX.ASTS.Client.Client item = new MOEX.ASTS.Client.Client(this, code, language, scales);
            lock (this.clients)
            {
                this.clients.Add(item);
            }
            return item;
        }

        private static string DetectLanguage(string report)
        {
            foreach (string str in Greetings.Keys)
            {
                if (report.Contains(str))
                {
                    return Greetings[str];
                }
            }
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        MOEX.ASTS.Client.Client[] array = new MOEX.ASTS.Client.Client[0];
                        lock (this.clients)
                        {
                            if (this.clients.Count > 0)
                            {
                                array = new MOEX.ASTS.Client.Client[this.clients.Count];
                                this.clients.CopyTo(array);
                                this.clients.Clear();
                            }
                        }
                        foreach (MOEX.ASTS.Client.Client client in array)
                        {
                            client.Close();
                        }
                    }
                    if (this.handle != IntPtr.Zero)
                    {
                        Dynamic.FreeLibrary(this.handle);
                        this.handle = IntPtr.Zero;
                    }
                }
                finally
                {
                    this.disposed = true;
                }
            }
        }

        internal string ErrorMessage(int code, string language)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
            string str = LangToName[language];
            if ((this.MTEErrorMsgEx != null) && (str != null))
            {
                return Marshal.PtrToStringAnsi(this.MTEErrorMsgEx(code, str));
            }
            return Marshal.PtrToStringAnsi(this.MTEErrorMsg(code));
        }

        ~Module()
        {
            this.Dispose(false);
        }

        public static Module Load(string path)
        {
            IntPtr handle = Dynamic.LoadLibrary(path);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return new Module(handle);
        }

        private Delegate LoadProc(string name, Type type, bool required)
        {
            IntPtr procAddress = Dynamic.GetProcAddress(this.handle, name);
            if (!(procAddress == IntPtr.Zero))
            {
                return Marshal.GetDelegateForFunctionPointer(procAddress, type);
            }
            if (required)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), name + " not found");
            }
            return null;
        }

        public string Version
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException(base.GetType().Name);
                }
                return Marshal.PtrToStringAnsi(this.MTEGetVersion());
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEAddTable(int handle, int table, int marker);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        /*internal*/public delegate int TMTECloseTable(int handle, int table);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEConnect(string parameters, StringBuilder report);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEDisconnect(int handle);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate IntPtr TMTEErrorMsg(int code);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate IntPtr TMTEErrorMsgEx(int code, string language);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEExecTrans(int handle, string transaction, string parameters, StringBuilder report);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEGetServInfo(int handle, out IntPtr buffer, out int length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEGetSnapshot(int handle, out IntPtr buffer, out int length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate IntPtr TMTEGetVersion();

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEOpenTable(int handle, string table, string parameters, int complete, out IntPtr buffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEOpenTableAtSnapshot(int handle, string table, string parameters, byte[] snapshot, int length, out IntPtr buffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTERefresh(int handle, out IntPtr buffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTESelectBoards(int handle, string boards, StringBuilder report);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet=CharSet.Ansi)]
        internal delegate int TMTEStructureEx(int handle, int version, out IntPtr buffer);
    }
}

