namespace MOEX.ASTS.Client
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public abstract class Parser
    {
        protected byte[] buffer;
        protected MOEX.ASTS.Client.Client client;
        protected string currentTickerBoard = string.Empty;
        protected string currentTickerPaper = string.Empty;
        public static readonly Parser Empty = new TableParser(null, null, null, null);
        private static readonly int[] everything = new int[0x100];
        private int fldCount;
        private int[] fldIndex;
        private bool hasDecimals;
        private bool isNewRow;
        private bool isPartialOrderbook;
        protected string lastOrderbookBoard = string.Empty;
        protected string lastOrderbookPaper = string.Empty;
        protected int lastOrderbookScale;
        private const int MaxFieldCount = 0x100;
        private long orderbookHeadPos;
        private bool parseKeyPhase;
        private readonly int[] projection = new int[0x100];
        private int rowDecimals;
        public static readonly Encoding Win1251 = Encoding.GetEncoding(0x4e3);


        static Parser()
        {
            for (int i = 0; i < everything.Length; i++)
            {
                everything[i] = i;
            }
        }

        internal Parser(MOEX.ASTS.Client.Client client, byte[] buffer)
        {
            this.client = client;
            this.buffer = buffer;
        }

        public int Execute(IBinder binder)
        {
            return this.Parse(binder);
        }

        protected int GetRowDecimals(Meta.Message source, ITarget target)
        {
            if (!this.hasDecimals)
            {
                if (this.isNewRow)
                {
                    if (source.Output.HasSecCode)
                    {
                        if (!this.isPartialOrderbook)
                        {
                            int numDecimal=0;
							if (!(source.Output.HasSecBoard ? this.client.Scales.Find(this.currentTickerBoard, this.currentTickerPaper, out  numDecimal) : this.client.Scales.Find(this.currentTickerPaper, out  numDecimal)))
                            {

								//var res = this.client.Scales.Find(this.currentTickerBoard, this.currentTickerPaper, out  numDecimal);

								// Changed 2017-05-16 ot prevent problem with unknown SECs in 
								// TradeTimes table
								
								if (!target.IsNullDecimal)								
									throw new ClientException(-16, "Trying to parse data having not loaded/parsed SECURITIES (" + this.currentTickerBoard + "." + this.currentTickerPaper + ")");
                            }
							this.SetRowDecimals(target, numDecimal);
                            if (source.IsOrderbook)
                            {
								this.lastOrderbookScale = numDecimal;
                            }
                        }
                        else
                        {
                            this.SetRowDecimals(target, this.lastOrderbookScale);
                        }
                    }
                    else
                    {
                        this.SetRowDecimals(target, 0);
                        if (source.IsOrderbook)
                        {
                            this.lastOrderbookScale = 0;
                        }
                    }
                }
                else
                {
                    this.SetRowDecimals(target, (target == null) ? 0 : target.GetRecordDecimals());
                }
            }
            return this.rowDecimals;
        }

        internal bool IsEmptyOrderbook(Meta.Message source, string filter, ITarget target)
        {
            bool flag2;
            if ((source.Type == Meta.TableType.Boardbooks) ? (string.Compare(this.lastOrderbookBoard, this.currentTickerBoard, StringComparison.Ordinal) == 0) : ((string.Compare(this.lastOrderbookBoard, this.currentTickerBoard, StringComparison.Ordinal) == 0) && (string.Compare(this.lastOrderbookPaper, this.currentTickerPaper, StringComparison.Ordinal) == 0)))
            {
                return false;
            }
            Reset(ref this.lastOrderbookBoard, ref this.lastOrderbookPaper);
            this.lastOrderbookBoard = this.currentTickerBoard;
            if (source.Type == Meta.TableType.Boardbooks)
            {
                flag2 = this.fldCount < 2;
            }
            else
            {
                this.lastOrderbookPaper = this.currentTickerPaper;
                flag2 = this.fldCount <= 2;
            }
            if (target != null)
            {
                target.SwitchOrderbook(source, filter, this.lastOrderbookBoard, this.lastOrderbookPaper);
            }
            return flag2;
        }

        protected abstract int Parse(IBinder binder);
        protected int Parse(BinaryReader reader, Meta.Message source, string filter, ITarget target)
        {
            int num3;
            int num = reader.ReadInt32();
            Reset(ref this.currentTickerBoard, ref this.currentTickerPaper);
            Reset(ref this.lastOrderbookBoard, ref this.lastOrderbookPaper);
            if (target != null)
            {
                target.InitTableUpdate(source, filter);
            }
            try
            {
                for (int i = 0; i < num; i++)
                {
                    this.ParseRow(reader, source, filter, target);
                }
                num3 = num;
            }
            finally
            {
                if (target != null)
                {
                    target.DoneTableUpdate(source, filter);
                }
            }
            return num3;
        }

        protected object ParseData(BinaryReader reader, Meta.Message source, Meta.Field field, ITarget target)
        {
            object obj2;
            if (field.IsOneChar)
            {
                obj2 = reader.ReadChar();
            }
            else if (field.CanEscape && (reader.PeekChar() == 0x23))
            {
                obj2 = null;
                reader.ReadChar();
            }
            else
            {
                int size = field.Size;
                switch (field.Type)
                {
                    case Meta.FieldType.Char:
                        obj2 = Read(reader, size);
                        break;

                    case Meta.FieldType.Integer:
                        Int64? nullable=null;//KAA
                         obj2 = SToL(reader, size);

                      /*
                        if (size <= 9)
                        {
                            nullable = SToI(reader, size);
                        }
                        //KAA
                        obj2 = (nullable.HasValue ? SToL(reader, size) : null);
                        //obj2 = nullable.HasValue ? nullable. : null;
                        //obj2 = nullable.GetValueOrDefault();
                       */
                        break;
                        
                    case Meta.FieldType.Fixed:
                        obj2 = SToF(reader, size, (byte) field.Decimals);
                        break;

                    case Meta.FieldType.Float:
                        obj2 = SToF(reader, size, (byte) this.GetRowDecimals(source, target));
                        break;

                    case Meta.FieldType.Date:
                        obj2 = SToI(reader, size);
                        break;

                    case Meta.FieldType.Time:
                        obj2 = SToI(reader, size);
                        break;

                    case Meta.FieldType.FloatPoint:
                    {
                        char[] chArray = reader.ReadChars(size);
                        int length = chArray.Length;
                        while ((length > 0) && (chArray[length - 1] == ' '))
                        {
                            length--;
                        }
                        while ((length > 0) && (chArray[length - 1] == '0'))
                        {
                            length--;
                        }
                        if (length == 0)
                        {
                            obj2 = null;
                        }
                        else
                        {
                            decimal num3;
                            if (decimal.TryParse(new string(chArray, 0, length), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num3))
                            {
                                obj2 = num3;
                            }
                            else
                            {
                                obj2 = null;
                            }
                        }
                        break;
                    }
                    default:
                        obj2 = null;
                        break;
                }
            }
            if (target != null)
            {
                if (this.parseKeyPhase)
                {
                    target.SetKeyValue(field, obj2);
                    return obj2;
                }
                target.SetFieldValue(field, obj2);
            }
            return obj2;
        }

        protected int ParseKeys(BinaryReader reader, Meta.Message source, ITarget target)
        {
            int num = -1;
            Reset(ref this.currentTickerBoard, ref this.currentTickerPaper);
            int count = source.Output.Keys.Count;
            if (source.Output.HasDecimals)
            {
                count++;
            }
            if (source.Output.HasSecBoard)
            {
                count++;
            }
            if (source.Output.HasSecCode)
            {
                count++;
            }
            if (source.Output.HasMarketID)
            {
                count++;
            }
            long position = reader.BaseStream.Position;
            try
            {
                for (int i = 0; (i < this.fldCount) && (count > 0); i++)
                {
                    int num5 = this.fldIndex[i];
                    if (num5 > source.maxEssentialIndex)
                    {
                        return num;
                    }
                    Meta.Field field = source.Output[num5];
                    bool isKey = field.IsKey;
                    int size = field.Size;
                    object obj2 = null;
                    if (isKey)
                    {
                        count--;
                        obj2 = this.ParseData(reader, source, field, target);
                        size = 0;
                    }
                    else if (field.CanEscape && (reader.PeekChar() == 0x23))
                    {
                        reader.ReadChar();
                        size = 0;
                    }
                    switch ((field.Flags & 0xff02))
                    {
                        case 2:
                            count--;
                            this.currentTickerPaper = Convert.ToString(isKey ? obj2 : Read(reader, size));
                            break;

                        case 0x100:
                            count--;
                            num = Convert.ToInt32(isKey ? obj2 : SToI(reader, size));
                            break;

                        case 0x200:
                            count--;
                            this.currentTickerBoard = Convert.ToString(isKey ? obj2 : Read(reader, size));
                            break;

                        case 0x400:
                            count--;
                            if (!isKey)
                            {
                                Stream baseStream = reader.BaseStream;
                                baseStream.Position += size;
                            }
                            break;

                        default:
                        {
                            Stream stream2 = reader.BaseStream;
                            stream2.Position += size;
                            break;
                        }
                    }
                }
            }
            finally
            {
                reader.BaseStream.Position = position;
            }
            return num;
        }

        protected void ParseRow(BinaryReader reader, Meta.Message source, string filter, ITarget target)
        {
            int num4;
            this.fldCount = reader.ReadByte();
            int num = reader.ReadInt32();
            if (this.fldCount == 0)
            {
                this.fldCount = source.Output.Count;
                this.fldIndex = everything;
            }
            else
            {
                this.fldIndex = this.projection;
                for (int i = 0; i < this.fldCount; i++)
                {
                    this.fldIndex[i] = reader.ReadByte();
                }
            }
            this.isPartialOrderbook = source.IsOrderbook && (this.fldIndex[0] != 0);
            this.parseKeyPhase = true;
            try
            {
                if ((source.maxEssentialIndex < 0) || this.isPartialOrderbook)
                {
                    this.hasDecimals = false;
                    this.rowDecimals = 0;
                }
                else
                {
                    int num3 = this.ParseKeys(reader, source, target);
                    this.hasDecimals = num3 >= 0;
                    this.rowDecimals = this.hasDecimals ? num3 : 0;
                }
            }
            finally
            {
                this.parseKeyPhase = false;
            }
            if (((source.IsOrderbook && this.IsEmptyOrderbook(source, filter, target)) || ((source.Output.IsSecNoBoard && (this.currentTickerPaper.Length > 0)) && !this.client.Scales.Find(this.currentTickerPaper, out num4))) || (num == 0))
            {
                Stream baseStream = reader.BaseStream;
                baseStream.Position += num;
            }
            else
            {
                this.isNewRow = ((target == null) || target.InitRecordUpdate(source, filter)) || source.IsOrderbook;
                try
                {
                    int num7;
                    if (this.hasDecimals)
                    {
                        if (source.Output.HasSecCode)
                        {
                            this.client.Scales.Add(this.currentTickerBoard, this.currentTickerPaper, this.rowDecimals);
                        }
                        if ((target != null) && this.isNewRow)
                        {
                            target.SetRecordDecimals(this.rowDecimals);
                        }
                    }
                    else if (this.isNewRow)
                    {
                        this.GetRowDecimals(source, target);
                    }
                    if (source.IsOrderbook)
                    {
                        if (this.isPartialOrderbook)
                        {
                            long position = reader.BaseStream.Position;
                            try
                            {
                                reader.BaseStream.Position = this.orderbookHeadPos;
                                for (int j = 0; j < this.fldIndex[0]; j++)
                                {
                                    Meta.Field field = source.Output[j];
                                    this.ParseData(reader, source, field, target);
                                }
                                goto Label_0251;
                            }
                            finally
                            {
                                reader.BaseStream.Position = position;
                            }
                        }
                        this.orderbookHeadPos = reader.BaseStream.Position;
                    }
                Label_0251:
                    num7 = 0;
                    while (num7 < this.fldCount)
                    {
                        Meta.Field field2 = source.Output[this.fldIndex[num7]];
                        this.ParseData(reader, source, field2, target);
                        num7++;
                    }
                }
                finally
                {
                    if (target != null)
                    {
                        target.DoneRecordUpdate(source, filter);
                    }
                }
            }
        }

        private static string Read(BinaryReader reader, int length)
        {
            char[] chArray = reader.ReadChars(length);
            int num = chArray.Length;
            while ((num > 0) && (chArray[num - 1] == ' '))
            {
                num--;
            }
            if (num != 0)
            {
                return new string(chArray, 0, num);
            }
            return null;
        }

        protected static void Reset(ref string board, ref string paper)
        {
            board = string.Empty;
            paper = string.Empty;
        }

        protected void SetRowDecimals(ITarget target, int decimals)
        {
            this.hasDecimals = true;
            this.rowDecimals = decimals;
            if ((target != null) && this.isNewRow)
            {
                target.SetRecordDecimals(decimals);
            }
        }

        private static decimal? SToF(BinaryReader reader, int length, byte scale)
        {
            long? nullable = SToL(reader, length);
            if (!nullable.HasValue)
            {
                return null;
            }
            bool isNegative = nullable < 0L;
            if (isNegative)
            {
                long? nullable3 = nullable;
                nullable = nullable3.HasValue ? new long?(-nullable3.GetValueOrDefault()) : null;
            }
            long? nullable7 = nullable & ((long) 0xffffffffL);
            long? nullable12 = (nullable >> 0x20) & ((long) 0xffffffffL);
            return new decimal((int) nullable7.Value, (int) nullable12.Value, 0, isNegative, scale);
        }



        /*
        private static Int16? SToI2(BinaryReader reader, int length)
        {
            if (length <= 0)
            {
                return null;
            }
            char[] chArray = reader.ReadChars(length);
            if (chArray.Length < length)
            {
                throw new EndOfStreamException();
            }
            int index = 0;
            bool flag = false;
            switch (chArray[0])
            {
                case '+':
                    index++;
                    break;

                case '-':
                    flag = true;
                    index++;
                    break;
            }
            while ((index < length) && (chArray[index] == ' '))
            {
                index++;
            }
            while ((index < length) && (chArray[index] == '0'))
            {
                index++;
            }
            if (index >= length)
            {
                return null;
            }
            int num2 = 0;
            while (index < length)
            {
                int num3 = chArray[index] - '0';
                if ((num3 >= 0) && (num3 < 10))
                {
                    num2 *= 10;
                    num2 += num3;
                }
                else
                {
                    return null;
                }
                index++;
            }
            Int16 i16 = Convert.ToInt16(num2);
            return new Int16?(flag ? -i16 : i16);
            //return new Int16?(i16);
        }
        */
        


        private static int? SToI(BinaryReader reader, int length)
        {
            if (length <= 0)
            {
                return null;
            }
            char[] chArray = reader.ReadChars(length);
            if (chArray.Length < length)
            {
                throw new EndOfStreamException();
            }
            int index = 0;
            bool flag = false;
            switch (chArray[0])
            {
                case '+':
                    index++;
                    break;

                case '-':
                    flag = true;
                    index++;
                    break;
            }
            while ((index < length) && (chArray[index] == ' '))
            {
                index++;
            }
            while ((index < length) && (chArray[index] == '0'))
            {
                index++;
            }
            if (index >= length)
            {
                return null;
            }
            int num2 = 0;
            while (index < length)
            {
                int num3 = chArray[index] - '0';
                if ((num3 >= 0) && (num3 < 10))
                {
                    num2 *= 10;
                    num2 += num3;
                }
                else
                {
                    return null;
                }
                index++;
            }
            return new int?(flag ? -num2 : num2);
        }

        private static long? SToL(BinaryReader reader, int length)
        {
            if (length <= 0)
            {
                return null;
            }
            char[] chArray = reader.ReadChars(length);
            if (chArray.Length < length)
            {
                throw new EndOfStreamException();
            }
            int index = 0;
            bool flag = false;
            switch (chArray[0])
            {
                case '+':
                    index++;
                    break;

                case '-':
                    flag = true;
                    index++;
                    break;
            }
            while ((index < length) && (chArray[index] == ' '))
            {
                index++;
            }
            while ((index < length) && (chArray[index] == '0'))
            {
                index++;
            }
            if (index >= length)
            {
                return null;
            }
            long num2 = 0L;
            while (index < length)
            {
                int num3 = chArray[index] - '0';
                if ((num3 >= 0) && (num3 < 10))
                {
                    num2 *= 10L;
                    num2 += num3;
                }
                else
                {
                    return null;
                }
                index++;
            }
            return new long?(flag ? -num2 : num2);
        }

        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
        }

        public abstract bool IsEmpty { get; }

        public int Length
        {
            get
            {
                if (this.buffer != null)
                {
                    return this.buffer.Length;
                }
                return 0;
            }
        }
    }
}

