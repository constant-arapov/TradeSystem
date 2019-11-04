namespace MOEX.ASTS.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public sealed class Client : IDisposable
    {
        private bool disposed;
        private int handle = -1;
        private string language;
        private Meta.Market market;
        private readonly Module module;
        private int refreshes;
        private readonly OrderedDictionary requests = new OrderedDictionary();
        private readonly MOEX.ASTS.Client.Scales scales;
        private MOEX.ASTS.Client.ServerInfo server;

        internal Client(Module module, int handle, string language, MOEX.ASTS.Client.Scales scales)
        {
            this.module = module;
            this.handle = handle;
            this.language = language;
            this.scales = scales ?? new MOEX.ASTS.Client.Scales();
            this.LoadServerInfo();
            this.LoadMarketInfo();
        }

        private void CheckClosed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
        }

        internal void CheckResult(int code)
        {
            this.CheckResult(code, null);
        }

        internal void CheckResult(int code, StringBuilder text)
        {
            if (code < 0)
            {
                if ((text != null) && (text.Length != 0))
                {
                    throw new ClientException(code, text.ToString());
                }
                throw new ClientException(code, this.module.ErrorMessage(code, this.language));
            }
        }

        private void CheckScales(Meta.Message source)
        {
            if (source.IsScaleNeeded && this.scales.Empty)
            {
                Meta.Message message = this.market.Tables.Find(Meta.TableType.Securities);
                if (message != null)
                {
                    IntPtr ptr;
                    string parameters = message.Encode(null, this.scales);
                    int code = this.module.MTEOpenTable(this.handle, message.Name, parameters, 1, out ptr);
                    this.CheckResult(code);
                    try
                    {
                        int structure = Marshal.ReadInt32(ptr);
                        ptr += Marshal.SizeOf(structure);
                        byte[] destination = new byte[structure];
                        Marshal.Copy(ptr, destination, 0, structure);
                        TableParser.Acquire(this, message, parameters, destination).Execute(null);
                    }
                    finally
                    {
                        this.module.MTECloseTable(this.handle, code);
                    }
                }
            }
        }

        public void Close()
        {
            this.Dispose();
        }

        private Request CreateRequest(int handle, Meta.Message source, string filter, string marker)
        {
            Request request = new Request(this, handle, source, filter, marker);
            this.requests.Add(marker, request);
            return request;
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
                        this.requests.Clear();
                    }
                    if (this.handle >= 0)
                    {
                        this.module.MTEDisconnect(this.handle);
                        this.handle = -1;
                    }
                    this.module.Closed(this);
                }
                finally
                {
                    this.disposed = true;
                }
            }
        }

        public bool Execute(string transaction, IDictionary<string, object> param, out string report)
        {
            this.CheckClosed();
            Meta.Message source = this.market.Transactions[transaction];
            if (source == null)
            {
                throw new ClientException(-17, "Transaction " + transaction + " is not listed");
            }
            this.CheckScales(source);
            string parameters = source.Encode(param, this.scales);
            StringBuilder builder = new StringBuilder(0x400);
            int code = this.module.MTEExecTrans(this.handle, transaction, parameters, builder);
            report = builder.ToString();
            if ((code != 0) && (code != -18))
            {
                this.CheckResult(code, builder);
            }
            return (code == 0);
        }

        ~Client()
        {
            this.Dispose(false);
        }

        public Request GetRequest(string marker)
        {
            return (Request) this.requests[marker];
        }

        public IList<Request> GetRequests()
        {
            List<Request> list = new List<Request>();
            IDictionaryEnumerator enumerator = this.requests.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Value as Request);
            }
            return list;
        }

        public byte[] GetSnapshot()
        {
            byte[] target = null;
            this.GetSnapshot(ref target);
            return target;
        }

        public int GetSnapshot(ref byte[] target)
        {
            IntPtr ptr;
            int num;
            this.CheckClosed();
            if (this.module.MTEGetSnapshot == null)
            {
                return 0;
            }
            this.CheckResult(this.module.MTEGetSnapshot(this.handle, out ptr, out num));
            if ((target == null) || (target.Length < num))
            {
                target = new byte[num];
            }
            Marshal.Copy(ptr, target, 0, num);
            return num;
        }

        public Parser Load(string table, IDictionary<string, object> param)
        {
            IntPtr ptr;
            Parser parser;
            this.CheckClosed();
            Meta.Message source = this.market.Tables[table];
            if (source == null)
            {
                throw new ClientException(-17, "Table " + table + " is not listed");
            }
            this.CheckScales(source);
            string parameters = source.Encode(param, this.scales);
            int code = this.module.MTEOpenTable(this.handle, table, parameters, 1, out ptr);
            this.CheckResult(code);
            try
            {
                int structure = Marshal.ReadInt32(ptr);
                ptr += Marshal.SizeOf(structure);
                byte[] destination = new byte[structure];
                Marshal.Copy(ptr, destination, 0, structure);
                parser = TableParser.Acquire(this, source, parameters, destination);
            }
            finally
            {
                this.module.MTECloseTable(this.handle, code);
            }
            return parser;
        }

        private void LoadMarketInfo()
        {
            IntPtr ptr;
            this.CheckResult(this.module.MTEStructureEx(this.handle, 0x103, out ptr));
            int structure = Marshal.ReadInt32(ptr);
            ptr += Marshal.SizeOf(structure);
            byte[] destination = new byte[structure];
            Marshal.Copy(ptr, destination, 0, structure);
            this.market = Meta.Market.Parse(new MemoryStream(destination, false), 0x103, this.Language);
        }

        private void LoadServerInfo()
        {
            if (this.module.MTEGetServInfo != null)
            {
                IntPtr ptr;
                int num;
                this.CheckResult(this.module.MTEGetServInfo(this.handle, out ptr, out num));
                this.server = new MOEX.ASTS.Client.ServerInfo(ptr);
            }
        }

        public Parser Open(string table, IDictionary<string, object> param)
        {
            string str;
            return this.Open(table, param, false, null, out str);
        }

        public Parser Open(string table, IDictionary<string, object> param, bool complete)
        {
            string str;
            return this.Open(table, param, complete, null, out str);
        }

        public Parser Open(string table, IDictionary<string, object> param, out string marker)
        {
            return this.Open(table, param, false, null, out marker);
        }

        public Parser Open(string table, IDictionary<string, object> param, byte[] snapshot)
        {
            string str;
            return this.Open(table, param, false, snapshot, out str);
        }

        public Parser Open(string table, IDictionary<string, object> param, bool complete, out string marker)
        {
            return this.Open(table, param, complete, null, out marker);
        }

        public Parser Open(string table, IDictionary<string, object> param, byte[] snapshot, out string marker)
        {
            return this.Open(table, param, false, snapshot, out marker);
        }

        private Parser Open(string table, IDictionary<string, object> param, bool complete, byte[] snapshot, out string marker)
        {
            this.CheckClosed();
            if ((snapshot != null) && (this.module.MTEOpenTableAtSnapshot == null))
            {
                throw new ClientException(-34, "MTEOpenTableAtSnapshot() not supported");
            }
            Meta.Message source = this.market.Tables[table];
            if (source == null)
            {
                throw new ClientException(-17, "Table " + table + " is not listed");
            }
            this.CheckScales(source);
            string parameters = source.Encode(param, this.scales);
            marker = source.Name + '@' + parameters;
            if (this.GetRequest(marker) == null)
            {
                IntPtr ptr;
                int code = (snapshot == null) ? this.module.MTEOpenTable(this.handle, table, parameters, (complete || !source.IsUpdatable) ? 1 : 0, out ptr) : this.module.MTEOpenTableAtSnapshot(this.handle, table, parameters, snapshot, snapshot.Length, out ptr);
                this.CheckResult(code);
                try
                {
                    int structure = Marshal.ReadInt32(ptr);
                    ptr += Marshal.SizeOf(structure);
                    byte[] destination = new byte[structure];
                    Marshal.Copy(ptr, destination, 0, structure);
                    return TableParser.Acquire(this, source, parameters, destination);
                }
                finally
                {
                    if (source.IsUpdatable)
                    {
                        this.CreateRequest(code, source, parameters, marker);
                    }
                    else
                    {
                        this.module.MTECloseTable(this.handle, code);
                    }
                }
            }
            return Parser.Empty;
        }

        private IList<Request> Prepare(IList<Request> list)
        {
            int num = Interlocked.Increment(ref this.refreshes);
            int num2 = -1;
            int num3 = -1;
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].Source.Type)
                {
                    case Meta.TableType.Orders:
                        num2 = i;
                        break;

                    case Meta.TableType.Trades:
                        num3 = i;
                        break;
                }
            }
            if ((((num % 2) == 0) && (num2 >= 0)) && (num3 >= 0))
            {
                Request request = list[num2];
                list[num2] = list[num3];
                list[num3] = request;
            }
            if ((num % 7) == 0)
            {
                int num5 = list.Count - 1;
                for (int j = num5; j > 0; j--)
                {
                    if ((j == num2) || (j == num3))
                    {
                        Request request2 = list[j];
                        list[j] = list[num5];
                        list[num5] = request2;
                        num5--;
                    }
                }
            }
            return list;
        }

        public Parser Refresh()
        {
            return this.Refresh(null);
        }

        public Parser Refresh(Sorter sorter)
        {
            IntPtr ptr;
            this.CheckClosed();
            if (this.requests.Count == 0)
            {
                return Parser.Empty;
            }
            IList<Request> items = this.Prepare(this.GetRequests());
            if (sorter != null)
            {
                sorter(items);
            }
            if (items.Count == 0)
            {
                return Parser.Empty;
            }
            Meta.Message[] sources = new Meta.Message[items.Count];
            string[] filters = new string[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                Request request = items[i];
                if (request != null)
                {
                    sources[i] = request.Source;
                    filters[i] = request.Filter;
                    this.CheckResult(this.module.MTEAddTable(this.handle, request.Handle, i));
                }
            }
            this.CheckResult(this.module.MTERefresh(this.handle, out ptr));
            int structure = Marshal.ReadInt32(ptr);
            ptr += Marshal.SizeOf(structure);
            byte[] destination = new byte[structure];
            Marshal.Copy(ptr, destination, 0, structure);
            return MultiParser.Acquire(this, sources, filters, destination);
        }

        private void RemoveRequest(string marker)
        {
            Request request = (Request) this.requests[marker];
            if (request != null)
            {
                try
                {
                    if (this.handle >= 0)
                    {
                        this.module.MTECloseTable(this.handle, request.Handle);
                    }
                }
                finally
                {
                    this.requests.Remove(marker);
                }
            }
        }

        public void SelectBoards(ISet<string> boards)
        {
            this.CheckClosed();
            if (boards.Count != 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string str in boards)
                {
                    builder.Append(str).Append(',');
                }
                StringBuilder text = new StringBuilder(0x400);
                this.CheckResult(this.module.MTESelectBoards(this.handle, builder.ToString(0, builder.Length - 1), text), text);
            }
        }

        public int Handle
        {
            get
            {
                return this.handle;
            }
        }

        public string Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.CheckClosed();
                if (value == null)
                {
                    value = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                }
                if (value.Length > 2)
                {
                    string str = Module.NameToLang[value.ToUpper()];
                    if (str == null)
                    {
                        throw new ClientException(-17, "Unknown language " + value);
                    }
                    value = str;
                }
                if (!value.Equals(this.language))
                {
                    StringBuilder report = new StringBuilder(0x400);
                    char ch = value.ToUpper()[0];
                    int code = this.module.MTEExecTrans(this.handle, "CHANGE_LANGUAGE", ch.ToString(), report);
                    this.CheckResult(code, report);
                    this.market.Language = this.language = value;
                }
            }
        }

        public Meta.Market MarketInfo
        {
            get
            {
                this.CheckClosed();
                if (this.market == null)
                {
                    this.LoadMarketInfo();
                }
                return this.market;
            }
        }

        public MOEX.ASTS.Client.Scales Scales
        {
            get
            {
                return this.scales;
            }
        }

        public MOEX.ASTS.Client.ServerInfo ServerInfo
        {
            get
            {
                this.CheckClosed();
                if (this.server == null)
                {
                    this.LoadServerInfo();
                }
                return this.server;
            }
        }

        public class Request
        {
            private readonly MOEX.ASTS.Client.Client client;
            private readonly string marker;

            internal Request(MOEX.ASTS.Client.Client client, int handle, Meta.Message source, string filter, string marker)
            {
                this.client = client;
                this.Handle = handle;
                this.Source = source;
                this.Filter = filter;
                this.marker = marker;
            }

            public void Close()
            {
                this.client.RemoveRequest(this.marker);
            }

            public string Filter { get; private set; }

            public int Handle { get; private set; }

            public Meta.Message Source { get; private set; }
        }

        public delegate void Sorter(IList<MOEX.ASTS.Client.Client.Request> items);
    }
}

