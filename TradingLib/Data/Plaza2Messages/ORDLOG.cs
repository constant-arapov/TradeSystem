


// Scheme "ORDLOG" description

namespace ORDLOG
{


    // Scheme "ORDLOG" description


    public partial class orders_log
    {
        public const int table_index = 0;
        private System.IO.UnmanagedMemoryStream stream;
        private int offset;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;
        public orders_log()
        {
        }
        public orders_log(System.IO.UnmanagedMemoryStream stream_)
        {
            Data = stream_;
            offset = 0;
        }
        public orders_log(System.IO.UnmanagedMemoryStream stream_, int offset_)
        {
            Data = stream_;
            offset = offset_;
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
        public int Offset
        {
            get
            {
                return offset;
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



        public long replID
        {
            get
            {
                checkReader();
                stream.Position = offset + 0;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 0;
                writer.Write(value);
            }
        }






        public long replRev
        {
            get
            {
                checkReader();
                stream.Position = offset + 8;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 8;
                writer.Write(value);
            }
        }






        public long replAct
        {
            get
            {
                checkReader();
                stream.Position = offset + 16;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 16;
                writer.Write(value);
            }
        }






        public int isin_id
        {
            get
            {
                checkReader();
                stream.Position = offset + 24;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 24;
                writer.Write(value);
            }
        }






        public decimal price
        {
            get
            {
                checkReader();
                stream.Position = offset + 28;
                return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
            }
            set
            {
                checkWriter();
                stream.Position = offset + 28;
                ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
            }
        }

        public byte price_scale
        {
            get
            {
                stream.Position = offset + 28;
                return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
            }
        }






        public long volume
        {
            get
            {
                checkReader();
                stream.Position = offset + 40;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 40;
                writer.Write(value);
            }
        }






        public System.DateTime moment
        {
            get
            {
                checkReader();
                stream.Position = offset + 48;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = offset + 48;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }






        public ulong moment_ns
        {
            get
            {
                checkReader();
                stream.Position = offset + 60;
                return reader.ReadUInt64();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 60;
                writer.Write(value);
            }
        }






        public sbyte dir
        {
            get
            {
                checkReader();
                stream.Position = offset + 68;
                return reader.ReadSByte();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 68;
                writer.Write(value);
            }
        }






        public System.DateTime timestamp
        {
            get
            {
                checkReader();
                stream.Position = offset + 70;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = offset + 70;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }






        public int sess_id
        {
            get
            {
                checkReader();
                stream.Position = offset + 80;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = offset + 80;
                writer.Write(value);
            }
        }




    }
	
	




    partial class multileg_orders_log
    {
        public const int table_index = 1;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public multileg_orders_log()
        {
        }

        public multileg_orders_log(System.IO.UnmanagedMemoryStream stream_)
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


        public long replID
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public long replRev
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


        public long replAct
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                writer.Write(value);
            }
        }


        public long id_ord
        {
            get
            {
                checkReader();
                stream.Position = 24;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 24;
                writer.Write(value);
            }
        }


        public int sess_id
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


        public int isin_id
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


        public int amount
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


        public int amount_rest
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


        public long id_deal
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


        public long xstatus
        {
            get
            {
                checkReader();
                stream.Position = 56;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 56;
                writer.Write(value);
            }
        }


        public int status
        {
            get
            {
                checkReader();
                stream.Position = 64;
                return reader.ReadInt32();
            }
            set
            {
                checkWriter();
                stream.Position = 64;
                writer.Write(value);
            }
        }


        public decimal price
        {
            get
            {
                checkReader();
                stream.Position = 68;
                return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
            }
            set
            {
                checkWriter();
                stream.Position = 68;
                ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
            }
        }

        public byte price_scale
        {
            get
            {
                stream.Position = 68;
                return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
            }
        }


        public System.DateTime moment
        {
            get
            {
                checkReader();
                stream.Position = 80;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 80;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }


        public sbyte dir
        {
            get
            {
                checkReader();
                stream.Position = 90;
                return reader.ReadSByte();
            }
            set
            {
                checkWriter();
                stream.Position = 90;
                writer.Write(value);
            }
        }


        public sbyte action
        {
            get
            {
                checkReader();
                stream.Position = 91;
                return reader.ReadSByte();
            }
            set
            {
                checkWriter();
                stream.Position = 91;
                writer.Write(value);
            }
        }


        public decimal deal_price
        {
            get
            {
                checkReader();
                stream.Position = 92;
                return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
            }
            set
            {
                checkWriter();
                stream.Position = 92;
                ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
            }
        }

        public byte deal_price_scale
        {
            get
            {
                stream.Position = 92;
                return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
            }
        }


        public decimal rate_price
        {
            get
            {
                checkReader();
                stream.Position = 103;
                return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
            }
            set
            {
                checkWriter();
                stream.Position = 103;
                ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
            }
        }

        public byte rate_price_scale
        {
            get
            {
                stream.Position = 103;
                return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
            }
        }


        public decimal swap_price
        {
            get
            {
                checkReader();
                stream.Position = 114;
                return (decimal)ru.micexrts.cgate.P2TypeParser.ParseBCDAsDecimal(reader, stream);
            }
            set
            {
                checkWriter();
                stream.Position = 114;
                ru.micexrts.cgate.P2TypeComposer.ComposeDecimalAsBCD(writer, "d16.5", (decimal)value);
            }
        }

        public byte swap_price_scale
        {
            get
            {
                stream.Position = 114;
                return ru.micexrts.cgate.P2TypeParser.ParseBCDAsScale(reader, stream);
            }
        }




    }

    partial class heartbeat
    {
        public const int table_index = 2;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public heartbeat()
        {
        }

        public heartbeat(System.IO.UnmanagedMemoryStream stream_)
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


        public long replID
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public long replRev
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


        public long replAct
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                writer.Write(value);
            }
        }


        public System.DateTime server_time
        {
            get
            {
                checkReader();
                stream.Position = 24;
                return ru.micexrts.cgate.P2TypeParser.ParseTimeAsDate(reader);
            }
            set
            {
                checkWriter();
                stream.Position = 24;
                ru.micexrts.cgate.P2TypeComposer.ComposeDateAsTime(writer, value);
            }
        }




    }

    partial class sys_events
    {
        public const int table_index = 3;
        private System.IO.UnmanagedMemoryStream stream;
        private System.IO.BinaryReader reader;
        private System.IO.BinaryWriter writer;

        public sys_events()
        {
        }

        public sys_events(System.IO.UnmanagedMemoryStream stream_)
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


        public long replID
        {
            get
            {
                checkReader();
                stream.Position = 0;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 0;
                writer.Write(value);
            }
        }


        public long replRev
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


        public long replAct
        {
            get
            {
                checkReader();
                stream.Position = 16;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 16;
                writer.Write(value);
            }
        }


        public long event_id
        {
            get
            {
                checkReader();
                stream.Position = 24;
                return reader.ReadInt64();
            }
            set
            {
                checkWriter();
                stream.Position = 24;
                writer.Write(value);
            }
        }


        public int sess_id
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


        public int event_type
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


        public string message
        {
            get
            {
                checkReader();
                stream.Position = 40;
                return ru.micexrts.cgate.P2TypeParser.ParseCXX(reader, 64);
            }
            set
            {
                checkWriter();
                stream.Position = 40;
                ru.micexrts.cgate.P2TypeComposer.ComposeCXX(writer, value, 64);
            }
        }




    }


}

