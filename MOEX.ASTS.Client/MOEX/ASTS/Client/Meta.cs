namespace MOEX.ASTS.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Meta
    {
        public const int STRUCTURE_LOCALIZATION = 0x100;
        public const int STRUCTURE_OPTIONS_MASK = 0xff00;
        public const int STRUCTURE_VERSION_0 = 0;
        public const int STRUCTURE_VERSION_1 = 1;
        public const int STRUCTURE_VERSION_2 = 2;
        public const int STRUCTURE_VERSION_3 = 3;
        public const int STRUCTURE_VERSION_MASK = 0xff;

        public static bool IsLocalized(int version)
        {
            return ((version & 0x100) != 0);
        }

        internal static void LoadItems<E>(BinaryReader reader, BaseList<E> target, Func<E> loader) where E: BaseItem
        {
            for (int i = reader.ReadInt32(); i > 0; i--)
            {
                E item = loader();
                target.Add(item);
            }
        }

        public static int PureVersion(int version)
        {
            return (version & 0xff);
        }

        public abstract class BaseItem
        {
            private readonly MOEX.ASTS.Client.Meta.Caption caption;
            private readonly MOEX.ASTS.Client.Meta.Caption description;
            private readonly string name;

            protected BaseItem(BinaryReader reader, int version, string language)
            {
                this.name = reader.ReadString();
                if (Meta.PureVersion(version) < 2)
                {
                    string text = reader.ReadString();
                    int index = text.IndexOf('\0');
                    if (index > 0)
                    {
                        this.caption = new MOEX.ASTS.Client.Meta.Caption(language, text.Substring(0, index));
                        this.description = new MOEX.ASTS.Client.Meta.Caption(language, text.Substring(index + 1));
                    }
                    else
                    {
                        this.caption = new MOEX.ASTS.Client.Meta.Caption(language, text);
                        this.description = new MOEX.ASTS.Client.Meta.Caption(language, "");
                    }
                }
                else
                {
                    this.caption = new MOEX.ASTS.Client.Meta.Caption(reader, version, language);
                    this.description = new MOEX.ASTS.Client.Meta.Caption(reader, version, language);
                }
            }

            public MOEX.ASTS.Client.Meta.Caption Caption
            {
                get
                {
                    return this.caption;
                }
            }

            public MOEX.ASTS.Client.Meta.Caption Description
            {
                get
                {
                    return this.description;
                }
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
            }
        }

        public class BaseList<E> where E: Meta.BaseItem
        {
            private readonly OrderedDictionary items;

            protected BaseList()
            {
                this.items = new OrderedDictionary();
            }

            internal void Add(E item)
            {
                this.items.Add(item.Name, item);
            }

            public bool Find(string name, out E item)
            {
                item = this[name];
                return (((E) item) != null);
            }

            public int Count
            {
                get
                {
                    return this.items.Count;
                }
            }

            public E this[int index]
            {
                get
                {
                    return (E) this.items[index];
                }
            }

            public E this[string name]
            {
                get
                {
                    return (E) this.items[name];
                }
            }
        }

        public class Caption
        {
            private readonly string first;
            private readonly StringDictionary items;

            internal Caption(string language, string text)
            {
                this.items = new StringDictionary();
                this.items.Add(language, text);
                this.first = language;
            }

            internal Caption(BinaryReader reader, int version, string language)
            {
                this.items = new StringDictionary();
                if (Meta.IsLocalized(version))
                {
                    for (int i = reader.ReadInt32(); i > 0; i--)
                    {
                        string str = reader.ReadString();
                        int index = str.IndexOf(':');
                        if (index > 0)
                        {
                            if (this.items.Count == 0)
                            {
                                this.first = str.Substring(0, index);
                            }
                            this.items.Add(str.Substring(0, index), str.Substring(index + 1));
                        }
                    }
                }
                else
                {
                    this.items.Add(language, reader.ReadString());
                    this.first = language;
                }
            }

            public string this[string language]
            {
                get
                {
                    string str = this.items[language];
                    return (str ?? this.items[this.first]);
                }
            }
        }

        public class EnumItem
        {
            private readonly MOEX.ASTS.Client.Meta.Caption caption;
            private readonly MOEX.ASTS.Client.Meta.Caption description;
            private readonly string id;

            internal EnumItem(BinaryReader reader, int version, string language)
            {
                if (Meta.PureVersion(version) < 2)
                {
                    throw new IOException("Deprecated version loading is not implemented");
                }
                this.id = reader.ReadString();
                this.description = new MOEX.ASTS.Client.Meta.Caption(reader, version, language);
                this.caption = new MOEX.ASTS.Client.Meta.Caption(reader, version, language);
            }

            public MOEX.ASTS.Client.Meta.Caption Caption
            {
                get
                {
                    return this.caption;
                }
            }

            public MOEX.ASTS.Client.Meta.Caption Description
            {
                get
                {
                    return this.description;
                }
            }

            public string ID
            {
                get
                {
                    return this.id;
                }
            }
        }

        public enum EnumKind
        {
            Check,
            Group,
            Combo
        }

        public class EnumType : Meta.BaseItem
        {
            private readonly OrderedDictionary hash;
            private readonly Meta.EnumKind kind;
            private readonly int size;

            internal EnumType(BinaryReader reader, int version, string language) : base(reader, version, language)
            {
                this.size = reader.ReadInt32();
                this.kind = (Meta.EnumKind) Enum.ToObject(typeof(Meta.EnumKind), reader.ReadInt32());
                this.hash = new OrderedDictionary();
                for (int i = reader.ReadInt32(); i > 0; i--)
                {
                    Meta.EnumItem item = new Meta.EnumItem(reader, version, language);
                    this.hash.Add(item.ID, item);
                }
            }

            public bool Find(string id, out Meta.EnumItem item)
            {
                item = this[id];
                return (item != null);
            }

            public int Count
            {
                get
                {
                    return this.hash.Count;
                }
            }

            public Meta.EnumItem this[int index]
            {
                get
                {
                    return (this.hash[index] as Meta.EnumItem);
                }
            }

            public Meta.EnumItem this[string id]
            {
                get
                {
                    string str = string.IsNullOrEmpty(id) ? new string(' ', this.size) : id;
                    return (this.hash[str] as Meta.EnumItem);
                }
            }

            public Meta.EnumKind Kind
            {
                get
                {
                    return this.kind;
                }
            }
        }

        public class EnumTypes : Meta.BaseList<Meta.EnumType>
        {
        }

        public class Field : Meta.BaseItem
        {
            internal int decimals;
            internal string defaults;
            internal Meta.EnumType enumeration;
            internal const int ffFixed1 = 0x10;
            internal const int ffFixed3 = 0x20;
            internal const int ffFixed4 = 0x30;
            internal const int ffFixedMask = 0x30;
            internal int flags;
            internal bool isKey;
            internal const int mffContent = 0xff02;
            internal const int mffDecimals = 0x100;
            internal const int mffKey = 1;
            internal const int mffMarketId = 0x400;
            internal const int mffNotNull = 4;
            internal const int mffOneChar = 0x10000;
            internal const int mffSecBoard = 0x200;
            internal const int mffSecCode = 2;
            internal const int mffSharp = 0x80;
            internal const int mffVarBlock = 8;
            internal int offset;
            internal readonly int size;
            internal readonly Meta.FieldType type;

            internal Field(BinaryReader reader, int version, string language, bool input, Meta.EnumTypes enumerations) : base(reader, version, language)
            {
                string str;
                this.size = reader.ReadInt32();
                this.type = (Meta.FieldType) Enum.ToObject(typeof(Meta.FieldType), reader.ReadInt32());
                if (Meta.PureVersion(version) >= 2)
                {
                    this.decimals = reader.ReadInt32();
                }
                this.flags = reader.ReadInt32();
                if (Meta.PureVersion(version) < 2)
                {
                    switch (this.type)
                    {
                        case Meta.FieldType.Fixed:
                            this.decimals = 2;
                            switch ((this.flags & 0x30))
                            {
                                case 0x10:
                                    this.decimals = 1;
                                    goto Label_00BC;

                                case 0x20:
                                    this.decimals = 3;
                                    goto Label_00BC;

                                case 0x30:
                                    this.decimals = 4;
                                    goto Label_00BC;
                            }
                            break;

                        case Meta.FieldType.Float:
                            this.decimals = -1;
                            break;
                    }
                }
            Label_00BC:
                str = reader.ReadString();
                if (str.Length > 0)
                {
                    if (!enumerations.Find(str, out this.enumeration))
                    {
                        throw new IOException("Invalid enumeration " + str);
                    }
                }
                else
                {
                    this.enumeration = null;
                }
                this.defaults = input ? reader.ReadString() : string.Empty;
            }

            internal StringBuilder Encode(StringBuilder builder, object value, int scale)
            {
                switch (this.type)
                {
                    case Meta.FieldType.Integer:
                        this.Format(builder, this.size, Convert.ToInt64(value));
                        return builder;

                    case Meta.FieldType.Fixed:
                        this.Format(builder, this.size, Convert.ToDecimal(value), this.decimals);
                        return builder;

                    case Meta.FieldType.Float:
                        this.Format(builder, this.size, Convert.ToDecimal(value), scale);
                        return builder;

                    case Meta.FieldType.Date:
                    {
                        if (!(value is DateTime))
                        {
                            this.Format(builder, this.size, Convert.ToInt64(value));
                            return builder;
                        }
                        DateTime time = Convert.ToDateTime(value);
                        this.Format(builder, this.size, (long) (((0x2710 * time.Year) + (100 * time.Month)) + time.Day));
                        return builder;
                    }
                    case Meta.FieldType.Time:
                    {
                        if (!(value is DateTime))
                        {
                            this.Format(builder, this.size, Convert.ToInt64(value));
                            return builder;
                        }
                        DateTime time2 = Convert.ToDateTime(value);
                        this.Format(builder, this.size, (long) (((0x2710 * time2.Hour) + (100 * time2.Minute)) + time2.Second));
                        return builder;
                    }
                    case Meta.FieldType.FloatPoint:
                        throw new ClientException(-34, string.Format("Encoding {0} not implemented", this.type));
                }
                string str = value.ToString();
                int length = str.Length;
                if (length > this.size)
                {
                    throw new ClientException(-17, string.Format("Value of {0} is too long for {1} ({2})", str, base.Name, this.size));
                }
                builder.Append(str);
                if (length < this.size)
                {
                    builder.Append(' ', this.size - length);
                }
                return builder;
            }

            internal static int Factor(int scale)
            {
                int num = 1;
                while (scale > 0)
                {
                    num *= 10;
                    scale--;
                }
                return num;
            }

            internal void Format(StringBuilder builder, int space, long value)
            {
                string str = (value < 0L) ? Convert.ToString(-value) : Convert.ToString(value);
                int length = str.Length;
                if (value < 0L)
                {
                    builder.Append('-');
                    length++;
                }
                if (length > space)
                {
                    throw new ClientException(-17, string.Format("Value of {0} is too long for {1} ({2})", value, base.Name, this.size));
                }
                while (length < space)
                {
                    builder.Append('0');
                    length++;
                }
                builder.Append(str);
            }

            internal void Format(StringBuilder builder, int space, decimal value, int scale)
            {
                this.Format(builder, space, Convert.ToInt64(decimal.Truncate(value * Factor(scale))));
            }

            internal bool CanEscape
            {
                get
                {
                    return ((this.flags & 0x80) != 0);
                }
            }

            public int Decimals
            {
                get
                {
                    return this.decimals;
                }
            }

            public string Defaults
            {
                get
                {
                    return this.defaults;
                }
            }

            public Meta.EnumType Enumeration
            {
                get
                {
                    return this.enumeration;
                }
            }

            internal int Flags
            {
                get
                {
                    return this.flags;
                }
            }

            public bool IsKey
            {
                get
                {
                    return this.isKey;
                }
            }

            public bool IsNotNull
            {
                get
                {
                    return ((this.flags & 4) != 0);
                }
            }

            internal bool IsOneChar
            {
                get
                {
                    return ((this.type == Meta.FieldType.Char) && (this.size == 1));
                }
            }

            public bool IsSecCode
            {
                get
                {
                    return ((this.flags & 2) != 0);
                }
            }

            internal int Offset
            {
                get
                {
                    return this.offset;
                }
            }

            public int Size
            {
                get
                {
                    return this.size;
                }
            }

            public Meta.FieldType Type
            {
                get
                {
                    return this.type;
                }
            }
        }

        public class Fields : Meta.BaseList<Meta.Field>
        {
            private static readonly HashSet<string> BoardFieldNames = new HashSet<string> { "BOARDID", "SECBOARD", "INDEXBOARD" };
            internal Meta.Field fldDecimals;
            internal Meta.Field fldMarketID;
            internal Meta.Field fldSecBoard;
            internal Meta.Field fldSecCode;
            private readonly List<Meta.Field> keys;
            private static readonly HashSet<string> MarketFieldNames = new HashSet<string> { "MARKETID", "MARKETCODE" };
            private static readonly HashSet<string> ScaleFieldNames = new HashSet<string> { "DECIMALS" };
            private int size;
            internal int varBlockCount;
            internal int varBlockFirst;

            internal Fields()
            {
                this.keys = new List<Meta.Field>();
                this.varBlockFirst = -1;
            }

            internal Fields(Meta.Message owner, BinaryReader reader, int version, string language, bool input, Meta.EnumTypes enumerations)
            {
                Func<Meta.Field> loader = null;
                this.keys = new List<Meta.Field>();
                this.varBlockFirst = -1;
                if (loader == null)
                {
                    loader = () => new Meta.Field(reader, version, language, input, enumerations);
                }
                Meta.LoadItems<Meta.Field>(reader, this, loader);
                for (int i = 0; i < base.Count; i++)
                {
                    Meta.Field item = base[i];
                    item.offset = this.size;
                    this.size += item.Size;
                    item.isKey = owner.CanHaveKeys && ((item.flags & 1) != 0);
                    if (item.isKey)
                    {
                        this.keys.Add(item);
                        if (!input)
                        {
                            owner.maxEssentialIndex = i;
                        }
                    }
                    if ((item.flags & 8) != 0)
                    {
                        if (this.varBlockCount == 0)
                        {
                            this.varBlockFirst = i;
                            this.varBlockCount++;
                        }
                        else
                        {
                            this.varBlockCount = (1 + i) - this.varBlockFirst;
                        }
                    }
                    if (item.IsSecCode)
                    {
                        this.fldSecCode = item;
                        if (!input)
                        {
                            owner.maxEssentialIndex = i;
                        }
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (BoardFieldNames.Contains(base[j].Name))
                            {
                                this.fldSecBoard = base[j];
                                this.fldSecBoard.flags |= 0x200;
                                break;
                            }
                        }
                    }
                    if (MarketFieldNames.Contains(item.Name))
                    {
                        item.flags |= 0x400;
                        this.fldMarketID = item;
                    }
                    if (ScaleFieldNames.Contains(item.Name))
                    {
                        item.flags |= 0x100;
                        this.fldDecimals = item;
                        if (!input)
                        {
                            owner.maxEssentialIndex = i;
                        }
                    }
                }
                if (this.fldDecimals == null)
                {
                    for (int k = 0; k < base.Count; k++)
                    {
                        Meta.Field field2 = base[k];
                        if ((field2.Type == Meta.FieldType.Float) && (field2.Decimals < 0))
                        {
                            owner.flags |= 0x400;
                        }
                    }
                }
            }

            internal bool HasDecimals
            {
                get
                {
                    return (this.fldDecimals != null);
                }
            }

            internal bool HasMarketID
            {
                get
                {
                    return (this.fldMarketID != null);
                }
            }

            internal bool HasSecBoard
            {
                get
                {
                    return (this.fldSecBoard != null);
                }
            }

            internal bool HasSecCode
            {
                get
                {
                    return (this.fldSecCode != null);
                }
            }

            internal bool IsSecNoBoard
            {
                get
                {
                    return ((this.fldSecCode != null) && (this.fldSecBoard == null));
                }
            }

            internal List<Meta.Field> Keys
            {
                get
                {
                    return this.keys;
                }
            }

            internal int Size
            {
                get
                {
                    return this.size;
                }
            }
        }

        public enum FieldType
        {
            Char,
            Integer,
            Fixed,
            Float,
            Date,
            Time,
            FloatPoint
        }

        public class Market : Meta.BaseItem
        {
            private readonly Meta.EnumTypes enumerations;
            private readonly Meta.Messages tables;
            private readonly Meta.Messages transactions;

            private Market(BinaryReader reader, int version, string language) : base(reader, version, language)
            {
                Func<Meta.EnumType> loader = null;
                Func<Meta.Message> func2 = null;
                Func<Meta.Message> func3 = null;
                this.Language = language;
                this.enumerations = new Meta.EnumTypes();
                if (loader == null)
                {
                    loader = () => new Meta.EnumType(reader, version, language);
                }
                Meta.LoadItems<Meta.EnumType>(reader, this.enumerations, loader);
                this.tables = new Meta.Messages();
                if (func2 == null)
                {
                    func2 = () => new Meta.Message(reader, version, language, true, this.enumerations);
                }
                Meta.LoadItems<Meta.Message>(reader, this.tables, func2);
                this.transactions = new Meta.Messages();
                if (func3 == null)
                {
                    func3 = () => new Meta.Message(reader, version, language, false, this.enumerations);
                }


				Meta.LoadItems<Meta.Message>(reader, /*this.tables*/this.transactions, func3);
            }

            public static Meta.Market Parse(Stream stream, int version, string language)
            {
                Encoding encoding = Encoding.GetEncoding(0x4e3);
                using (Reader reader = new Reader(stream, encoding))
                {
                    return new Meta.Market(reader, version, language);
                }
            }

            public override string ToString()
            {
                string str = base.Description[this.Language];
                if (string.IsNullOrEmpty(str))
                {
                    return (base.Name + ":" + base.Caption[this.Language]);
                }
                return (base.Name + ":" + base.Caption[this.Language] + " (" + str + ")");
            }

            public Meta.EnumTypes Enumerations
            {
                get
                {
                    return this.enumerations;
                }
            }

            internal string Language { get; set; }

            public Meta.Messages Tables
            {
                get
                {
                    return this.tables;
                }
            }

            public Meta.Messages Transactions
            {
                get
                {
                    return this.transactions;
                }
            }

            private class Reader : BinaryReader
            {
                private byte[] buffer;
                private readonly Encoding encoding;

                public Reader(Stream stream, Encoding encoding) : base(stream, encoding)
                {
                    this.buffer = new byte[0x400];
                    this.encoding = encoding;
                }

                public override string ReadString()
                {
                    int count = this.ReadInt32();
                    if (count <= 0)
                    {
                        return string.Empty;
                    }
                    if (this.buffer.Length < count)
                    {
                        this.buffer = new byte[count];
                    }
                    int num2 = this.Read(this.buffer, 0, count);
                    if (num2 < count)
                    {
                        throw new EndOfStreamException(string.Concat(new object[] { "Expected ", count, " bytes, got only ", num2 }));
                    }
                    return this.encoding.GetString(this.buffer, 0, num2);
                }
            }
        }

        public class Message : Meta.BaseItem
        {
            internal int flags;
            private readonly Meta.Fields input;
            internal int maxEssentialIndex;
            internal const int mmfClearOnUpdate = 2;
            internal const int mmfNeedsScale = 0x400;
            internal const int mmfOrderBook = 4;
            internal const int mmfUpdatable = 1;
            private readonly Meta.Fields reply;
            private readonly int system;
            private readonly Meta.TableType type;
            private static Dictionary<string, Meta.TableType> WellKnownTables;

            static Message()
            {
                Dictionary<string, Meta.TableType> dictionary = new Dictionary<string, Meta.TableType>();
                dictionary.Add("SYSTIME", Meta.TableType.SysTime);
                dictionary.Add("TESYSTIME", Meta.TableType.SysTime);
                dictionary.Add("STATS", Meta.TableType.Stats);
                dictionary.Add("BCMESSAGES", Meta.TableType.BCMessages);
                dictionary.Add("CURRENCY", Meta.TableType.Currencies);
                dictionary.Add("USERS", Meta.TableType.Users);
                dictionary.Add("FIRMS", Meta.TableType.Firms);
                dictionary.Add("EXCHANGES", Meta.TableType.Exchanges);
                dictionary.Add("MARKETS", Meta.TableType.Markets);
                dictionary.Add("BOARDS", Meta.TableType.Boards);
                dictionary.Add("SECURITIES", Meta.TableType.Securities);
                dictionary.Add("SECS", Meta.TableType.Securities);
                dictionary.Add("INDEXES", Meta.TableType.Indexes);
                dictionary.Add("ORDERS", Meta.TableType.Orders);
                dictionary.Add("EXT_ORDERS", Meta.TableType.Orders);
                dictionary.Add("TRADES", Meta.TableType.Trades);
                dictionary.Add("NEGDEALS", Meta.TableType.Negdeals);
                dictionary.Add("POSITIONS", Meta.TableType.Positions);
                dictionary.Add("USTRADES", Meta.TableType.UsTrades);
                dictionary.Add("REPORTS", Meta.TableType.Reports);
                dictionary.Add("ALLTRADES", Meta.TableType.AllTrades);
                dictionary.Add("ALL_TRADES", Meta.TableType.AllTrades);
                dictionary.Add("ORDERBOOK", Meta.TableType.Orderbooks);
                dictionary.Add("QUOTEBOOK", Meta.TableType.Orderbooks);
                dictionary.Add("EXT_ORDERBOOK", Meta.TableType.Orderbooks);
                dictionary.Add("NEGDEALBOOK", Meta.TableType.Boardbooks);
                dictionary.Add("REPO_NEGDEALBOOK", Meta.TableType.Boardbooks);
                dictionary.Add("REPO_QUOTEBOOK", Meta.TableType.Boardbooks);
                //KAA
                dictionary.Add("RM_HOLD", Meta.TableType.RM_HOLD);
                dictionary.Add("TRADETIME", Meta.TableType.TradeTime);
				dictionary.Add("TRDTIMETYPES", Meta.TableType.TrdTimeTypes);
                

                WellKnownTables = dictionary;
            }

            internal Message(BinaryReader reader, int version, string language, bool table, Meta.EnumTypes enumerations) : base(reader, version, language)
            {
                this.maxEssentialIndex = -1;
                this.system = (Meta.PureVersion(version) >= 2) ? reader.ReadInt32() : 0;
                this.flags = table ? reader.ReadInt32() : 0;
                this.input = new Meta.Fields(this, reader, version, language, true, enumerations);
                this.reply = table ? new Meta.Fields(this, reader, version, language, false, enumerations) : new Meta.Fields();
                if (!WellKnownTables.TryGetValue(base.Name, out this.type))
                {
                    this.type = Meta.TableType.None;
                }
            }

            internal string Encode(IDictionary<string, object> values, Scales scales)
            {
                StringBuilder builder = new StringBuilder(this.input.Size);
                if ((values == null) || (values.Count == 0))
                {
                    return builder.Append(' ', this.input.Size).ToString();
                }
                int scale = 0;
                if (this.Params.HasSecCode)
                {
                    object obj2 = null;
                    if (values.TryGetValue(this.Params.fldSecCode.Name, out obj2))
                    {
                        object obj3 = null;
                        if (this.Params.HasSecBoard)
                        {
                            values.TryGetValue(this.Params.fldSecBoard.Name, out obj3);
                        }
                        if (obj3 == null)
                        {
                            scales.Find(obj2.ToString(), out scale);
                        }
                        else
                        {
                            scales.Find(obj3.ToString(), obj2.ToString(), out scale);
                        }
                    }
                }
                int num2 = 0;
                for (int i = 0; i < this.Params.varBlockCount; i++)
                {
                    Meta.Field field = this.Params[i + this.Params.varBlockFirst];
                    if ((field.flags & 8) != 0)
                    {
                        if (num2 == 0)
                        {
                            num2 = 1;
                        }
                        object obj4 = null;
                        if ((values.TryGetValue(field.Name, out obj4) && (obj4 != null)) && obj4.GetType().IsArray)
                        {
                            Array array = obj4 as Array;
                            num2 = Math.Max(num2, array.Length);
                        }
                    }
                }
                int num4 = (this.Params.varBlockFirst + this.Params.varBlockCount) - 1;
                int num5 = 0;
                int varBlockFirst = 0;
                while (varBlockFirst < this.Params.Count)
                {
                    Meta.Field field2 = this.Params[varBlockFirst];
                    object obj5 = null;
                    if (values.TryGetValue(field2.Name, out obj5))
                    {
                        if (obj5 == null)
                        {
                            builder.Append(' ', field2.Size);
                        }
                        else
                        {
                            System.Type type = obj5.GetType();
                            if (((field2.flags & 8) != 0) && type.IsArray)
                            {
                                Array array2 = obj5 as Array;
                                if (num5 < array2.Length)
                                {
                                    field2.Encode(builder, array2.GetValue(new int[] { num5 }), scale);
                                }
                                else
                                {
                                    builder.Append(' ', field2.Size);
                                }
                            }
                            else
                            {
                                field2.Encode(builder, obj5, scale);
                            }
                        }
                    }
                    else if (string.IsNullOrEmpty(field2.Defaults))
                    {
                        builder.Append(' ', field2.Size);
                    }
                    else
                    {
                        builder.Append(field2.Defaults);
                    }
                    if (varBlockFirst == num4)
                    {
                        if (++num5 < num2)
                        {
                            varBlockFirst = this.Params.varBlockFirst;
                        }
                        else
                        {
                            varBlockFirst++;
                        }
                    }
                    else
                    {
                        varBlockFirst++;
                    }
                }
                return builder.ToString();
            }

            internal bool CanHaveKeys
            {
                get
                {
                    return ((this.flags & 4) == 0);
                }
            }

            public bool ClearOnUpdate
            {
                get
                {
                    return ((this.flags & 2) != 0);
                }
            }

            public bool IsOrderbook
            {
                get
                {
                    return ((this.flags & 4) != 0);
                }
            }

            internal bool IsScaleNeeded
            {
                get
                {
                    return ((this.flags & 0x400) != 0);
                }
            }

            public bool IsUpdatable
            {
                get
                {
                    return ((this.flags & 1) != 0);
                }
            }

            public Meta.Fields Output
            {
                get
                {
                    return this.reply;
                }
            }

            public Meta.Fields Params
            {
                get
                {
                    return this.input;
                }
            }

            public int System
            {
                get
                {
                    return this.system;
                }
            }

            public Meta.TableType Type
            {
                get
                {
                    return this.type;
                }
            }
        }

        public class Messages : Meta.BaseList<Meta.Message>
        {
            public Meta.Message Find(Meta.TableType type)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    Meta.Message message = base[i];
                    if (message.Type == type)
                    {
                        return message;
                    }
                }
                return null;
            }
        }

        public enum TableType
        {
            None,
            SysTime,
            Stats,
            BCMessages,
            Currencies,
            Exchanges,
            Markets,
            Users,
            Firms,
            Boards,
            Securities,
            Indexes,
            Orderbooks,
            Boardbooks,
            Orders,
            Trades,
            Negdeals,
            UsTrades,
            AllTrades,
            Positions,
            Reports,
            RM_HOLD, 
            TradeTime,
			TrdTimeTypes
            
        }
    }
}

