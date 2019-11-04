


// Scheme "message" description

namespace MESSAGE
{
    partial class FutAddOrder
    {
        public const int table_index = 0;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutAddOrder()
        {
        }

        public FutAddOrder(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public string isin
        {
            get
            {
                checkReader();
                stream.Position = 5;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 5;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public string client_code
        {
            get
            {
                checkReader();
                stream.Position = 31;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 31;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public int type
        {
            get
            {
                checkReader();
                stream.Position = 36;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 36;
                writer.Write(value);
            }
        }


        public int dir
        {
            get
            {
                checkReader();
                stream.Position = 40;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 40;
                writer.Write(value);
            }
        }


        public int amount
        {
            get
            {
                checkReader();
                stream.Position = 44;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 44;
                writer.Write(value);
            }
        }


        public string price
        {
            get
            {
                checkReader();
                stream.Position = 48;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 48;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string comment
        {
            get
            {
                checkReader();
                stream.Position = 66;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
            }
            set
            {
                checkWriter();
                stream.Position = 66;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
            }
        }


        public string broker_to
        {
            get
            {
                checkReader();
                stream.Position = 87;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
            }
            set
            {
                checkWriter();
                stream.Position = 87;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
            }
        }


        public int ext_id
        {
            get
            {
                checkReader();
                stream.Position = 108;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 108;
                writer.Write(value);
            }
        }


        public int du
        {
            get
            {
                checkReader();
                stream.Position = 112;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 112;
                writer.Write(value);
            }
        }


        public string date_exp
        {
            get
            {
                checkReader();
                stream.Position = 116;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 8);
            }
            set
            {
                checkWriter();
                stream.Position = 116;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 8);
            }
        }


        public int hedge
        {
            get
            {
                checkReader();
                stream.Position = 128;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 128;
                writer.Write(value);
            }
        }


        public int dont_check_money
        {
            get
            {
                checkReader();
                stream.Position = 132;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 132;
                writer.Write(value);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 136;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 136;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    public partial class FORTS_MSG101
    {
        public const int table_index = 1;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG101()
        {
        }

        public FORTS_MSG101(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public long order_id
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    partial class FutAddMultiLegOrder
    {
        public const int table_index = 2;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutAddMultiLegOrder()
        {
        }

        public FutAddMultiLegOrder(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int sess_id
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public int isin_id
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                writer.Write(value);
            }
        }


        public string client_code
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public int type
        {
            get
            {
                checkReader();
                stream.Position = 20;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 20;
                writer.Write(value);
            }
        }


        public int dir
        {
            get
            {
                checkReader();
                stream.Position = 24;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 24;
                writer.Write(value);
            }
        }


        public int amount
        {
            get
            {
                checkReader();
                stream.Position = 28;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 28;
                writer.Write(value);
            }
        }


        public string price
        {
            get
            {
                checkReader();
                stream.Position = 32;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 32;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string rate_price
        {
            get
            {
                checkReader();
                stream.Position = 50;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 50;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string comment
        {
            get
            {
                checkReader();
                stream.Position = 68;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
            }
            set
            {
                checkWriter();
                stream.Position = 68;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
            }
        }


        public int hedge
        {
            get
            {
                checkReader();
                stream.Position = 92;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 92;
                writer.Write(value);
            }
        }


        public string broker_to
        {
            get
            {
                checkReader();
                stream.Position = 96;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
            }
            set
            {
                checkWriter();
                stream.Position = 96;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
            }
        }


        public int ext_id
        {
            get
            {
                checkReader();
                stream.Position = 120;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 120;
                writer.Write(value);
            }
        }


        public int trust
        {
            get
            {
                checkReader();
                stream.Position = 124;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 124;
                writer.Write(value);
            }
        }


        public string date_exp
        {
            get
            {
                checkReader();
                stream.Position = 128;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 8);
            }
            set
            {
                checkWriter();
                stream.Position = 128;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 8);
            }
        }


        public int trade_mode
        {
            get
            {
                checkReader();
                stream.Position = 140;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 140;
                writer.Write(value);
            }
        }


        public int dont_check_money
        {
            get
            {
                checkReader();
                stream.Position = 144;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 144;
                writer.Write(value);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 148;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 148;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    public partial class FORTS_MSG129
    {
        public const int table_index = 3;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG129()
        {
        }

        public FORTS_MSG129(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public long order_id
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    public partial class FutDelOrder
    {
        public const int table_index = 4;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutDelOrder()
        {
        }

        public FutDelOrder(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public long order_id
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    public partial class FORTS_MSG102
    {
        public const int table_index = 5;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG102()
        {
        }

        public FORTS_MSG102(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public int amount
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    partial class FutDelUserOrders
    {
        public const int table_index = 6;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutDelUserOrders()
        {
        }

        public FutDelUserOrders(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int buy_sell
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public int non_system
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public string code_vcb
        {
            get
            {
                checkReader();
                stream.Position = 20;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 20;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public int ext_id
        {
            get
            {
                checkReader();
                stream.Position = 48;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 48;
                writer.Write(value);
            }
        }


        public int work_mode
        {
            get
            {
                checkReader();
                stream.Position = 52;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 52;
                writer.Write(value);
            }
        }


        public string isin
        {
            get
            {
                checkReader();
                stream.Position = 56;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 56;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 82;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 82;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    public partial class FORTS_MSG103
    {
        public const int table_index = 7;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG103()
        {
        }

        public FORTS_MSG103(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public int num_orders
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    partial class FutMoveOrder
    {
        public const int table_index = 8;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutMoveOrder()
        {
        }

        public FutMoveOrder(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int regime
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public long order_id1
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                writer.Write(value);
            }
        }


        public int amount1
        {
            get
            {
                checkReader();
                stream.Position = 20;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 20;
                writer.Write(value);
            }
        }


        public string price1
        {
            get
            {
                checkReader();
                stream.Position = 24;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 24;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public int ext_id1
        {
            get
            {
                checkReader();
                stream.Position = 44;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 44;
                writer.Write(value);
            }
        }


        public long order_id2
        {
            get
            {
                checkReader();
                stream.Position = 48;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 48;
                writer.Write(value);
            }
        }


        public int amount2
        {
            get
            {
                checkReader();
                stream.Position = 56;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 56;
                writer.Write(value);
            }
        }


        public string price2
        {
            get
            {
                checkReader();
                stream.Position = 60;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 60;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public int ext_id2
        {
            get
            {
                checkReader();
                stream.Position = 80;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 80;
                writer.Write(value);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 84;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 84;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    partial class FORTS_MSG105
    {
        public const int table_index = 9;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG105()
        {
        }

        public FORTS_MSG105(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public long order_id1
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }


        public long order_id2
        {
            get
            {
                checkReader();
                stream.Position = 268;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 268;
                writer.Write(value);
            }
        }




    }

    partial class OptAddOrder
    {
        public const int table_index = 10;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public OptAddOrder()
        {
        }

        public OptAddOrder(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public string isin
        {
            get
            {
                checkReader();
                stream.Position = 5;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 5;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public string client_code
        {
            get
            {
                checkReader();
                stream.Position = 31;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 31;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public int type
        {
            get
            {
                checkReader();
                stream.Position = 36;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 36;
                writer.Write(value);
            }
        }


        public int dir
        {
            get
            {
                checkReader();
                stream.Position = 40;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 40;
                writer.Write(value);
            }
        }


        public int amount
        {
            get
            {
                checkReader();
                stream.Position = 44;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 44;
                writer.Write(value);
            }
        }


        public string price
        {
            get
            {
                checkReader();
                stream.Position = 48;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 48;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string comment
        {
            get
            {
                checkReader();
                stream.Position = 66;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
            }
            set
            {
                checkWriter();
                stream.Position = 66;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
            }
        }


        public string broker_to
        {
            get
            {
                checkReader();
                stream.Position = 87;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 20);
            }
            set
            {
                checkWriter();
                stream.Position = 87;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 20);
            }
        }


        public int ext_id
        {
            get
            {
                checkReader();
                stream.Position = 108;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 108;
                writer.Write(value);
            }
        }


        public int du
        {
            get
            {
                checkReader();
                stream.Position = 112;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 112;
                writer.Write(value);
            }
        }


        public int check_limit
        {
            get
            {
                checkReader();
                stream.Position = 116;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 116;
                writer.Write(value);
            }
        }


        public string date_exp
        {
            get
            {
                checkReader();
                stream.Position = 120;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 8);
            }
            set
            {
                checkWriter();
                stream.Position = 120;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 8);
            }
        }


        public int hedge
        {
            get
            {
                checkReader();
                stream.Position = 132;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 132;
                writer.Write(value);
            }
        }


        public int dont_check_money
        {
            get
            {
                checkReader();
                stream.Position = 136;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 136;
                writer.Write(value);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 140;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 140;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    partial class FORTS_MSG109
    {
        public const int table_index = 11;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG109()
        {
        }

        public FORTS_MSG109(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public long order_id
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    partial class OptDelOrder
    {
        public const int table_index = 12;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public OptDelOrder()
        {
        }

        public OptDelOrder(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public long order_id
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    partial class FORTS_MSG110
    {
        public const int table_index = 13;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG110()
        {
        }

        public FORTS_MSG110(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public int amount
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    partial class OptDelUserOrders
    {
        public const int table_index = 14;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public OptDelUserOrders()
        {
        }

        public OptDelUserOrders(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int buy_sell
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public int non_system
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public string code_vcb
        {
            get
            {
                checkReader();
                stream.Position = 20;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 20;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public int ext_id
        {
            get
            {
                checkReader();
                stream.Position = 48;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 48;
                writer.Write(value);
            }
        }


        public int work_mode
        {
            get
            {
                checkReader();
                stream.Position = 52;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 52;
                writer.Write(value);
            }
        }


        public string isin
        {
            get
            {
                checkReader();
                stream.Position = 56;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 56;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 82;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 82;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    partial class FORTS_MSG111
    {
        public const int table_index = 15;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG111()
        {
        }

        public FORTS_MSG111(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public int num_orders
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    partial class OptMoveOrder
    {
        public const int table_index = 16;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public OptMoveOrder()
        {
        }

        public OptMoveOrder(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int regime
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public long order_id1
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                writer.Write(value);
            }
        }


        public int amount1
        {
            get
            {
                checkReader();
                stream.Position = 20;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 20;
                writer.Write(value);
            }
        }


        public string price1
        {
            get
            {
                checkReader();
                stream.Position = 24;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 24;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public int ext_id1
        {
            get
            {
                checkReader();
                stream.Position = 44;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 44;
                writer.Write(value);
            }
        }


        public int check_limit
        {
            get
            {
                checkReader();
                stream.Position = 48;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 48;
                writer.Write(value);
            }
        }


        public long order_id2
        {
            get
            {
                checkReader();
                stream.Position = 52;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 52;
                writer.Write(value);
            }
        }


        public int amount2
        {
            get
            {
                checkReader();
                stream.Position = 60;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 60;
                writer.Write(value);
            }
        }


        public string price2
        {
            get
            {
                checkReader();
                stream.Position = 64;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 64;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public int ext_id2
        {
            get
            {
                checkReader();
                stream.Position = 84;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 84;
                writer.Write(value);
            }
        }


        public System.DateTime local_stamp
        {
            get
            {
                checkReader();
                stream.Position = 88;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 88;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    partial class FORTS_MSG113
    {
        public const int table_index = 17;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG113()
        {
        }

        public FORTS_MSG113(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public long order_id1
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }


        public long order_id2
        {
            get
            {
                checkReader();
                stream.Position = 268;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 268;
                writer.Write(value);
            }
        }




    }

    partial class FutChangeClientMoney
    {
        public const int table_index = 18;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutChangeClientMoney()
        {
        }

        public FutChangeClientMoney(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public string limit_money
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string limit_pledge
        {
            get
            {
                checkReader();
                stream.Position = 34;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 34;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string coeff_liquidity
        {
            get
            {
                checkReader();
                stream.Position = 52;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 52;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string coeff_go
        {
            get
            {
                checkReader();
                stream.Position = 70;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 70;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public int is_auto_update_limit
        {
            get
            {
                checkReader();
                stream.Position = 88;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 88;
                writer.Write(value);
            }
        }


        public int is_auto_update_spot_limit
        {
            get
            {
                checkReader();
                stream.Position = 92;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 92;
                writer.Write(value);
            }
        }


        public string limit_spot_buy
        {
            get
            {
                checkReader();
                stream.Position = 96;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 96;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public int no_fut_discount
        {
            get
            {
                checkReader();
                stream.Position = 116;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 116;
                writer.Write(value);
            }
        }


        public int check_limit
        {
            get
            {
                checkReader();
                stream.Position = 120;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 120;
                writer.Write(value);
            }
        }




    }

    partial class FORTS_MSG104
    {
        public const int table_index = 19;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG104()
        {
        }

        public FORTS_MSG104(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class FutChangeClientVcb
    {
        public const int table_index = 20;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutChangeClientVcb()
        {
        }

        public FutChangeClientVcb(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public string code_vcb
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public string coeff_go
        {
            get
            {
                checkReader();
                stream.Position = 42;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 42;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string limit_spot
        {
            get
            {
                checkReader();
                stream.Position = 60;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 10);
            }
            set
            {
                checkWriter();
                stream.Position = 60;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 10);
            }
        }




    }

    partial class FORTS_MSG106
    {
        public const int table_index = 21;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG106()
        {
        }

        public FORTS_MSG106(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class FutChangeBrokerVcb
    {
        public const int table_index = 22;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutChangeBrokerVcb()
        {
        }

        public FutChangeBrokerVcb(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string code_vcb
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public string limit_spot
        {
            get
            {
                checkReader();
                stream.Position = 38;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 10);
            }
            set
            {
                checkWriter();
                stream.Position = 38;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 10);
            }
        }




    }

    partial class FORTS_MSG114
    {
        public const int table_index = 23;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG114()
        {
        }

        public FORTS_MSG114(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class FutChangeBFMoney
    {
        public const int table_index = 24;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutChangeBFMoney()
        {
        }

        public FutChangeBFMoney(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 2);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 2);
            }
        }


        public string limit_money
        {
            get
            {
                checkReader();
                stream.Position = 15;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 15;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string limit_pledge
        {
            get
            {
                checkReader();
                stream.Position = 33;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 33;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }




    }

    partial class FORTS_MSG107
    {
        public const int table_index = 25;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG107()
        {
        }

        public FORTS_MSG107(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class FutChangeMoney
    {
        public const int table_index = 26;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutChangeMoney()
        {
        }

        public FutChangeMoney(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string limit_spot_buy
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public int is_auto_update_spot_limit
        {
            get
            {
                checkReader();
                stream.Position = 32;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 32;
                writer.Write(value);
            }
        }


        public int state
        {
            get
            {
                checkReader();
                stream.Position = 36;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 36;
                writer.Write(value);
            }
        }




    }

    partial class FORTS_MSG116
    {
        public const int table_index = 27;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG116()
        {
        }

        public FORTS_MSG116(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class OptChangeExpiration
    {
        public const int table_index = 28;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public OptChangeExpiration()
        {
        }

        public OptChangeExpiration(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public int order_id
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public string isin
        {
            get
            {
                checkReader();
                stream.Position = 20;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 20;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public int amount
        {
            get
            {
                checkReader();
                stream.Position = 48;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 48;
                writer.Write(value);
            }
        }




    }

    partial class FORTS_MSG112
    {
        public const int table_index = 29;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG112()
        {
        }

        public FORTS_MSG112(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }


        public int order_id
        {
            get
            {
                checkReader();
                stream.Position = 260;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 260;
                writer.Write(value);
            }
        }




    }

    partial class FutChangeClientProhibit
    {
        public const int table_index = 30;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutChangeClientProhibit()
        {
        }

        public FutChangeClientProhibit(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public string code_vcb
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public string isin
        {
            get
            {
                checkReader();
                stream.Position = 42;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 42;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public int state
        {
            get
            {
                checkReader();
                stream.Position = 68;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 68;
                writer.Write(value);
            }
        }


        public int state_mask
        {
            get
            {
                checkReader();
                stream.Position = 72;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 72;
                writer.Write(value);
            }
        }




    }

    partial class FORTS_MSG115
    {
        public const int table_index = 31;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG115()
        {
        }

        public FORTS_MSG115(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class OptChangeClientProhibit
    {
        public const int table_index = 32;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public OptChangeClientProhibit()
        {
        }

        public OptChangeClientProhibit(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string code
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 3);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 3);
            }
        }


        public string code_vcb
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public string isin
        {
            get
            {
                checkReader();
                stream.Position = 42;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 25);
            }
            set
            {
                checkWriter();
                stream.Position = 42;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 25);
            }
        }


        public int state
        {
            get
            {
                checkReader();
                stream.Position = 68;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 68;
                writer.Write(value);
            }
        }


        public int state_mask
        {
            get
            {
                checkReader();
                stream.Position = 72;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 72;
                writer.Write(value);
            }
        }




    }

    partial class FORTS_MSG117
    {
        public const int table_index = 33;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG117()
        {
        }

        public FORTS_MSG117(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class FutExchangeBFMoney
    {
        public const int table_index = 34;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FutExchangeBFMoney()
        {
        }

        public FutExchangeBFMoney(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int mode
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }


        public string code_from
        {
            get
            {
                checkReader();
                stream.Position = 12;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 2);
            }
            set
            {
                checkWriter();
                stream.Position = 12;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 2);
            }
        }


        public string code_to
        {
            get
            {
                checkReader();
                stream.Position = 15;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 2);
            }
            set
            {
                checkWriter();
                stream.Position = 15;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 2);
            }
        }


        public string amount_money
        {
            get
            {
                checkReader();
                stream.Position = 18;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 18;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }


        public string amount_pledge
        {
            get
            {
                checkReader();
                stream.Position = 36;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 17);
            }
            set
            {
                checkWriter();
                stream.Position = 36;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 17);
            }
        }




    }

    partial class FORTS_MSG130
    {
        public const int table_index = 35;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG130()
        {
        }

        public FORTS_MSG130(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class OptRecalcCS
    {
        public const int table_index = 36;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public OptRecalcCS()
        {
        }

        public OptRecalcCS(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public string broker_code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 4);
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 4);
            }
        }


        public int isin_id
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                writer.Write(value);
            }
        }




    }

    partial class FORTS_MSG132
    {
        public const int table_index = 37;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG132()
        {
        }

        public FORTS_MSG132(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }

    partial class FORTS_MSG99
    {
        public const int table_index = 38;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG99()
        {
        }

        public FORTS_MSG99(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int queue_size
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public int penalty_remain
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 8;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 128);
            }
            set
            {
                checkWriter();
                stream.Position = 8;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 128);
            }
        }




    }

    partial class FORTS_MSG100
    {
        public const int table_index = 39;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public FORTS_MSG100()
        {
        }

        public FORTS_MSG100(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
        }

        public System.IO.UnmanagedMemoryStream Data
        {
            set
            {
                stream = value;
                reader = null;
                writer = null;
            }
            get
            {
                return stream;
            }
        }

        private void checkReader()
        {
            if (reader == null)
                reader = new System.IO.BinaryReader(stream);
        }

        private void checkWriter()
        {
            if (writer == null)
                writer = new System.IO.BinaryWriter(stream);
        }


        public int code
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 4;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 255);
            }
            set
            {
                checkWriter();
                stream.Position = 4;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 255);
            }
        }




    }




}